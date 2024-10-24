using mamNonTuongLaiTuoiSang.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Ganss.XSS;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TinTucController : Controller
    {
        private string url = "http://localhost:5005/api/tintucs/";
        private string urlNhanVien = "http://localhost:5005/api/nhanviens/";
        private readonly QLMamNonContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        private HttpClient client = new HttpClient();

        public TinTucController(QLMamNonContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string searchQuery)
        {
            List<TinTuc> tintucs = new List<TinTuc>();
            var client = _httpClientFactory.CreateClient();

            try
            {
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    // Tìm kiếm bằng IdTinTuc
                    HttpResponseMessage responseById = await client.GetAsync(url + Uri.EscapeDataString(searchQuery));
                    if (responseById.IsSuccessStatusCode)
                    {
                        string result = await responseById.Content.ReadAsStringAsync();
                        var tt = JsonConvert.DeserializeObject<TinTuc>(result);
                        if (tt != null)
                        {
                            return RedirectToAction("Details", new { id = tt.IdTinTuc });
                        }
                    }

                    // Tìm kiếm theo TieuDe và NoiDung
                    HttpResponseMessage responseAll = await client.GetAsync(url);
                    if (responseAll.IsSuccessStatusCode)
                    {
                        string result = await responseAll.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<List<TinTuc>>(result);
                        if (data != null)
                        {
                            tintucs = data
                                .Where(t => (!string.IsNullOrEmpty(t.TieuDe) && t.TieuDe.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)) ||
                                            (!string.IsNullOrEmpty(t.NoiDung) && t.NoiDung.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)))
                                .ToList();
                        }
                    }
                }
                else
                {
                    // Lấy toàn bộ danh sách
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<List<TinTuc>>(result);
                        if (data != null)
                        {
                            tintucs = data;
                        }
                    }
                }

                // Lấy danh sách NhanVien để map MaSt thành HoTen
                List<NhanVien> nhanViens = new List<NhanVien>();
                HttpResponseMessage nvResponse = await client.GetAsync(urlNhanVien);
                if (nvResponse.IsSuccessStatusCode)
                {
                    string nvResult = await nvResponse.Content.ReadAsStringAsync();
                    nhanViens = JsonConvert.DeserializeObject<List<NhanVien>>(nvResult);
                }

                // Tạo dictionary để map MaSt thành HoTen
                var maStToHoTen = nhanViens.ToDictionary(nv => nv.MaSt, nv => nv.HoTen, StringComparer.OrdinalIgnoreCase);
                ViewBag.MaStToHoTen = maStToHoTen;

                return View(tintucs);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi tải dữ liệu. Vui lòng thử lại sau.";
                return View(tintucs);
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.MaSt = GetMaStSelectList();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TinTuc tt, IFormFile? anhFile)
        {
            if (anhFile != null && anhFile.Length > 0)
            {
                // Tạo đường dẫn cho file
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Content/images", anhFile.FileName);

                // Lưu file vào thư mục
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await anhFile.CopyToAsync(stream);
                }

                // Lưu tên file vào thuộc tính Anh
                tt.Anh = anhFile.FileName;
            }

            // Làm sạch nội dung HTML
            var sanitizer = new HtmlSanitizer();
            tt.NoiDung = sanitizer.Sanitize(tt.NoiDung);

            string data = JsonConvert.SerializeObject(tt);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync(url, content);
            if (response.IsSuccessStatusCode)
            {
                TempData["insert_message"] = "Đã thêm bản tin..";
                return RedirectToAction("Index");
            }
            return View(tt);
        }


        [HttpGet]
        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            TinTuc tt = null;
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                tt = JsonConvert.DeserializeObject<TinTuc>(result);
            }

            if (tt == null)
            {
                return NotFound();
            }

            // Lấy thông tin nhân viên từ API
            if (!string.IsNullOrEmpty(tt.MaSt))
            {
                HttpResponseMessage nvResponse = client.GetAsync(urlNhanVien + tt.MaSt).Result;
                if (nvResponse.IsSuccessStatusCode)
                {
                    string nvResult = nvResponse.Content.ReadAsStringAsync().Result;
                    tt.MaStNavigation = JsonConvert.DeserializeObject<NhanVien>(nvResult);
                }
            }

            return View(tt);
        }


        // GET: Admin/NgoaiKhoa/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            TinTuc tt = new TinTuc();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<TinTuc>(result);
                if (data != null)
                {
                    tt = data;
                }
            }
            ViewBag.MaSt = GetMaStSelectList();
            return View(tt);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(TinTuc tt, IFormFile? anhFile)
        {
            if (anhFile != null && anhFile.Length > 0)
            {
                // Tạo đường dẫn cho file mới
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "Content/images", anhFile.FileName);

                // Lưu file mới vào thư mục
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await anhFile.CopyToAsync(stream);
                }

                // Cập nhật tên file vào thuộc tính Anh
                tt.Anh = anhFile.FileName;
            }

            // Làm sạch nội dung HTML
            var sanitizer = new HtmlSanitizer();
            tt.NoiDung = sanitizer.Sanitize(tt.NoiDung);

            string data = JsonConvert.SerializeObject(tt);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url + tt.IdTinTuc, content);
            if (response.IsSuccessStatusCode)
            {
                TempData["update_message"] = "Đã sửa tin tức..";
                return RedirectToAction("Index");
            }
            return View(tt);
        }

        // GET: Admin/NgoaiKhoa/Delete/{id}
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            TinTuc tt = null;
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                tt = JsonConvert.DeserializeObject<TinTuc>(result);
            }

            if (tt == null)
            {
                return NotFound();
            }

            return View(tt);
        }

        // POST: Admin/NgoaiKhoa/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            HttpResponseMessage response = client.DeleteAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["SuccessMessage"] = "Ngoại khóa đã được xóa thành công.";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMessage"] = "Xóa Ngoại khóa không thành công.";
                return RedirectToAction("Delete", new { id = id });
            }
        }
        private IEnumerable<SelectListItem> GetMaStSelectList()
        {
            List<NhanVien> nhanViens = new List<NhanVien>();
            HttpResponseMessage response = client.GetAsync(urlNhanVien).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                nhanViens = JsonConvert.DeserializeObject<List<NhanVien>>(result);
            }

            // Chuyển đổi danh sách NhanVien thành danh sách SelectListItem
            var selectListItems = nhanViens.Select(nv => new SelectListItem
            {
                Value = nv.MaSt, // Giá trị bạn muốn gửi về
                Text = $" ({nv.MaSt})" + nv.HoTen// Hiển thị tên nhân viên
            }).ToList();

            return selectListItems;
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchQuery)
        {
            ViewBag.Placeholder = "Tìm kiếm theo mã tin tức hoặc tên tin tức";
            if (string.IsNullOrEmpty(searchQuery))
            {
                return RedirectToAction("Index"); // Redirect về danh sách nếu không nhập từ khóa
            }

            // Gửi yêu cầu đến API để lấy toàn bộ TinTuc
            HttpResponseMessage response = await client.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                var ttList = JsonConvert.DeserializeObject<List<TinTuc>>(result);

                if (ttList != null && ttList.Any())
                {
                    // Tìm kiếm chính xác IdTinTuc hoặc tên tin tức chứa searchQuery
                    var tt = ttList.FirstOrDefault(t =>
                        t.IdTinTuc.Equals(searchQuery, StringComparison.OrdinalIgnoreCase) ||
                        (t.TieuDe != null && t.TieuDe.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)));

                    if (tt == null)
                    {
                        return NotFound(); // Không tìm thấy bản tin
                    }

                    // Redirect đến trang chi tiết của bản tin tìm thấy
                    return RedirectToAction("Details", new { id = tt.IdTinTuc });
                }
            }

            return Problem("Lỗi khi tìm kiếm thông tin.");
        }

        // Action UploadImage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadImage(IFormFile upload)
        {
            if (upload != null && upload.Length > 0)
            {
                // Kiểm tra loại file
                var permittedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(upload.FileName).ToLowerInvariant();
                if (string.IsNullOrEmpty(extension) || !permittedExtensions.Contains(extension))
                {
                    return Json(new { uploaded = 0, error = new { message = "Chỉ cho phép upload các định dạng hình ảnh (.jpg, .jpeg, .png, .gif)." } });
                }

                // Kiểm tra kích thước file (ví dụ: tối đa 2MB)
                if (upload.Length > 2 * 1024 * 1024)
                {
                    return Json(new { uploaded = 0, error = new { message = "Kích thước hình ảnh không được vượt quá 2MB." } });
                }

                // Tạo đường dẫn lưu hình ảnh
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Content", "images");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Tạo tên file duy nhất
                var uniqueFileName = Guid.NewGuid().ToString() + extension;
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                // Lưu file vào thư mục
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await upload.CopyToAsync(fileStream);
                }

                // Trả về đường dẫn hình ảnh cho CKEditor
                var imageUrl = Url.Content($"~/Content/images/{uniqueFileName}");
                return Json(new { uploaded = 1, fileName = uniqueFileName, url = imageUrl });
            }

            return Json(new { uploaded = 0, error = new { message = "Không thể tải hình ảnh lên." } });
        }

    }
}