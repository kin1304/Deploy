using mamNonTuongLaiTuoiSang.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NgoaiKhoaController : Controller
    {
        private string url = "http://localhost:5005/api/ngoaikhoas/";
        private HttpClient client = new HttpClient();
        private readonly QLMamNonContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        public NgoaiKhoaController(QLMamNonContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, string searchQuery, string filterOption)
        {
            // Truyền sortOrder, searchQuery và filterOption qua ViewBag để sử dụng trong View
            ViewBag.IdSortParm = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            ViewBag.SearchQuery = searchQuery;
            ViewBag.FilterOption = filterOption;

            // Định nghĩa các tùy chọn lọc
            var filterOptions = new List<SelectListItem>
            {
                new SelectListItem { Value = "All", Text = "All" },
                new SelectListItem { Value = "DaQua", Text = "Đã qua" },
                new SelectListItem { Value = "DangDiChuyen", Text = "Đang diễn ra" },
                new SelectListItem { Value = "SapDiChuyen", Text = "Sắp diễn ra" }
            };

            // Truyền danh sách các tùy chọn lọc sang ViewBag
            ViewBag.FilterOptions = new SelectList(filterOptions, "Value", "Text", filterOption);

            List<NgoaiKhoa> ngoaikhoas = new List<NgoaiKhoa>();
            var client = _httpClientFactory.CreateClient();

            try
            {
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    // Kiểm tra xem searchQuery có phải là IdNk không
                    HttpResponseMessage responseById = await client.GetAsync(url + searchQuery);
                    if (responseById.IsSuccessStatusCode)
                    {
                        string result = await responseById.Content.ReadAsStringAsync();
                        var ngk = JsonConvert.DeserializeObject<NgoaiKhoa>(result);
                        if (ngk != null)
                        {
                            return RedirectToAction("Details", new { id = ngk.IdNk });
                        }
                    }

                    // Nếu không tìm thấy bằng IdNk, tiến hành tìm kiếm theo TenNk hoặc MoTa
                    HttpResponseMessage responseAll = await client.GetAsync(url);
                    if (responseAll.IsSuccessStatusCode)
                    {
                        string result = await responseAll.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<List<NgoaiKhoa>>(result);
                        if (data != null)
                        {
                            ngoaikhoas = data
                                .Where(nk =>
                                    (!string.IsNullOrEmpty(nk.TenNk) && nk.TenNk.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)) ||
                                    (!string.IsNullOrEmpty(nk.MoTa) && nk.MoTa.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                                )
                                .ToList();
                        }
                    }
                }
                else
                {
                    // Nếu không có tìm kiếm, lấy toàn bộ danh sách
                    HttpResponseMessage response = await client.GetAsync(url);
                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<List<NgoaiKhoa>>(result);
                        if (data != null)
                        {
                            ngoaikhoas = data;
                        }
                    }
                }

                // Áp dụng bộ lọc dựa trên filterOption
                if (!string.IsNullOrEmpty(filterOption) && filterOption != "All")
                {
                    DateTime now = DateTime.Now;
                    switch (filterOption)
                    {
                        case "DaQua":
                            ngoaikhoas = ngoaikhoas
                                .Where(nk => nk.NgayKetThu.HasValue && nk.NgayKetThu.Value < now)
                                .ToList();
                            break;
                        case "DangDiChuyen":
                            ngoaikhoas = ngoaikhoas
                                .Where(nk => nk.NgayBatDau.HasValue && nk.NgayBatDau.Value <= now
                                            && nk.NgayKetThu.HasValue && nk.NgayKetThu.Value > now)
                                .ToList();
                            break;
                        case "SapDiChuyen":
                            ngoaikhoas = ngoaikhoas
                                .Where(nk => nk.NgayBatDau.HasValue && nk.NgayBatDau.Value > now)
                                .ToList();
                            break;
                        default:
                            // Không áp dụng bộ lọc
                            break;
                    }
                }

                // Sắp xếp dữ liệu theo giá trị của sortOrder
                switch (sortOrder)
                {
                    case "id_desc":
                        ngoaikhoas = ngoaikhoas.OrderByDescending(nk => nk.IdNk).ToList();
                        break;
                    case "name_asc":
                        ngoaikhoas = ngoaikhoas.OrderBy(nk => nk.TenNk).ToList();
                        break;
                    case "name_desc":
                        ngoaikhoas = ngoaikhoas.OrderByDescending(nk => nk.TenNk).ToList();
                        break;
                    default:
                        // Mặc định là sắp xếp tăng dần theo IdNk
                        ngoaikhoas = ngoaikhoas.OrderBy(nk => nk.IdNk).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                // Log lỗi (nếu có hệ thống logging)
                // _logger.LogError(ex, "Lỗi khi thực hiện tìm kiếm và lọc NgoaiKhoa");

                // Hiển thị thông báo lỗi cho người dùng
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi thực hiện tìm kiếm và lọc. Vui lòng thử lại sau.";
            }

            return View(ngoaikhoas);
        }


        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(NgoaiKhoa ngk)
        {
            string data = JsonConvert.SerializeObject(ngk);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            NgoaiKhoa ngk = null;
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                ngk = JsonConvert.DeserializeObject<NgoaiKhoa>(result);
            }

            if (ngk == null)
            {
                return NotFound();
            }

            return View(ngk);
        }

        // GET: Admin/NgoaiKhoa/Edit/{id}
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            NgoaiKhoa ngk = new NgoaiKhoa();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<NgoaiKhoa>(result);
                if (data != null)
                {
                    ngk = data;
                }
            }

            return View(ngk);
        }

        [HttpPost]
        public IActionResult Edit(NgoaiKhoa ngk)
        {
            string data = JsonConvert.SerializeObject(ngk);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(url + ngk.IdNk, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Admin/NgoaiKhoa/Delete/{id}
        [HttpGet]
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            NgoaiKhoa ngk = null;
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                ngk = JsonConvert.DeserializeObject<NgoaiKhoa>(result);
            }

            if (ngk == null)
            {
                return NotFound();
            }

            return View(ngk);
        }

        // POST: Admin/NgoaiKhoa/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            HttpResponseMessage response = client.DeleteAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Delete", new { id = id });
            }
        }

    }
}