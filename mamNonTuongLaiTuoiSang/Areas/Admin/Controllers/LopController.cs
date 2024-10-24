
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mamNonTuongLaiTuoiSang.Models;
using Newtonsoft.Json;
using System.Security.Policy;
using System.Text;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LopController : Controller
    {
        private const string url = "http://localhost:5005/api/Lops/";
        private const string urlNhanVien = "http://localhost:5005/api/nhanviens/";
        private HttpClient client = new HttpClient();
        private readonly QLMamNonContext _context;
        private readonly IHttpClientFactory _httpClientFactory;
        public LopController(QLMamNonContext context, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _httpClientFactory = httpClientFactory;
        }

        // GET: Admin/Lop
        [HttpGet]
        public async Task<IActionResult> Index(string sortOrder, string filterOption, string searchQuery)
        {
            // Truyền sortOrder, filterOption và searchQuery qua ViewBag để sử dụng trong View
            ViewBag.IdSortParm = string.IsNullOrEmpty(sortOrder) ? "id_desc" : "";
            ViewBag.SiSoSortParm = sortOrder == "siso_asc" ? "siso_desc" : "siso_asc";
            ViewBag.TenLopSortParm = sortOrder == "tenlop_asc" ? "tenlop_desc" : "tenlop_asc";
            ViewBag.FilterOption = filterOption;
            ViewBag.SearchQuery = searchQuery;

            // Tạo danh sách các tùy chọn lọc
            ViewBag.siSoFilter = new SelectList(new[]
            {
                new { Value = "All", Text = "All" },
                new { Value = "<10", Text = "<10" },
                new { Value = "10-30", Text = "10-30" },
                new { Value = ">30", Text = ">30" }
            }, "Value", "Text", filterOption);
            

            List<Lop> lops = new List<Lop>();
            var client = _httpClientFactory.CreateClient();

            try
            {
                if (!string.IsNullOrEmpty(searchQuery))
                {
                    HttpResponseMessage responseById = await client.GetAsync($"{url}{Uri.EscapeDataString(searchQuery)}");

                    if (responseById.IsSuccessStatusCode)
                    {
                        string result = await responseById.Content.ReadAsStringAsync();
                        var lop = JsonConvert.DeserializeObject<Lop>(result);

                        if (lop != null)
                        {
                            return RedirectToAction("Details", new { id = lop.IdLop });
                        }
                    }
                    HttpResponseMessage responseAll = await client.GetAsync(url);

                    if (responseAll.IsSuccessStatusCode)
                    {
                        string result = await responseAll.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<List<Lop>>(result);

                        if (data != null)
                        {
                            lops = data
                                .Where(l => !string.IsNullOrEmpty(l.TenLop) &&
                                           l.TenLop.Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                                .ToList();
                        }
                    }
                }
                else
                {
                    HttpResponseMessage response = await client.GetAsync(url);

                    if (response.IsSuccessStatusCode)
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        var data = JsonConvert.DeserializeObject<List<Lop>>(result);

                        if (data != null)
                        {
                            lops = data;
                        }
                    }
                }

                // Lấy danh sách NhanVien để map MaStNavigation
                HttpResponseMessage responseNhanVien = await client.GetAsync(urlNhanVien);
                if (responseNhanVien.IsSuccessStatusCode)
                {
                    string resultNhanVien = await responseNhanVien.Content.ReadAsStringAsync();
                    var nhanViens = JsonConvert.DeserializeObject<List<NhanVien>>(resultNhanVien);
                    if (nhanViens != null)
                    {
                        foreach (var lop in lops)
                        {
                            lop.MaStNavigation = nhanViens.FirstOrDefault(nv => nv.MaSt == lop.MaSt);
                        }
                    }
                }
                // Áp dụng bộ lọc dựa trên filterOption
                if (!string.IsNullOrEmpty(filterOption) && filterOption != "All")
                {
                    switch (filterOption)
                    {
                        case "<10":
                            lops = lops.Where(l => l.SiSo < 10).ToList();
                            break;
                        case "10-30":
                            lops = lops.Where(l => l.SiSo >= 10 && l.SiSo <= 30).ToList();
                            break;
                        case ">30":
                            lops = lops.Where(l => l.SiSo > 30).ToList();
                            break;
                        default:
                            // Không áp dụng bộ lọc nếu filterOption không hợp lệ
                            break;
                    }
                }
                switch (sortOrder)
                {
                    case "id_desc":
                        lops = lops.OrderByDescending(l => l.IdLop).ToList();
                        break;
                    case "siso_asc":
                        lops = lops.OrderBy(l => l.SiSo).ToList();
                        break;
                    case "siso_desc":
                        lops = lops.OrderByDescending(l => l.SiSo).ToList();
                        break;
                    case "tenlop_asc":
                        lops = lops.OrderBy(l => l.TenLop).ToList();
                        break;
                    case "tenlop_desc":
                        lops = lops.OrderByDescending(l => l.TenLop).ToList();
                        break;
                    default:
                        // Mặc định là sắp xếp tăng dần theo IdLop
                        lops = lops.OrderBy(l => l.IdLop).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Đã xảy ra lỗi khi tải dữ liệu. Vui lòng thử lại sau.";
            }

            return View(lops);
        }

        // GET: Admin/Lop/Details/5
        [HttpGet]
        public IActionResult Details(string id)
        {
            Lop lop = new Lop();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Lop>(result);
                if (data != null)
                {
                    lop = data;
                }
            }
            // Lấy thông tin nhân viên từ API
            if (!string.IsNullOrEmpty(lop.MaSt))
            {
                HttpResponseMessage nvResponse = client.GetAsync(urlNhanVien + lop.MaSt).Result;
                if (nvResponse.IsSuccessStatusCode)
                {
                    string nvResult = nvResponse.Content.ReadAsStringAsync().Result;
                    lop.MaStNavigation = JsonConvert.DeserializeObject<NhanVien>(nvResult);
                }
            }
            return View(lop);
        }

        // GET: Admin/Lop/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.MaSt = GetMaStSelectList();
            return View();
        }

        // POST: Admin/Lop/Create
        [HttpPost]
        public IActionResult Create(Lop lop)
        {
            HttpResponseMessage nvResponse = client.GetAsync(urlNhanVien + lop.MaSt).Result;
            if (nvResponse.IsSuccessStatusCode)
            {
                string nvResult = nvResponse.Content.ReadAsStringAsync().Result;
                lop.MaStNavigation = JsonConvert.DeserializeObject<NhanVien>(nvResult);
            }
            string data = JsonConvert.SerializeObject(lop);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Admin/Lop/Edit/5
        [HttpGet]
        public IActionResult Edit(string id)
        {
            Lop lop = new Lop();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Lop>(result);
                if (data != null)
                {
                    lop = data;
                }
            }
            ViewBag.MaSt = GetMaStSelectList();
            return View(lop);
        }

        [HttpPost]
        public IActionResult Edit(Lop lop)
        {
            string data = JsonConvert.SerializeObject(lop);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(url + lop.IdLop, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Admin/Lop/Delete/5
        [HttpGet]
        public IActionResult Delete(string id)
        {
            Lop lop = new Lop();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<Lop>(result);
                if (data != null)
                {
                    lop = data;
                }
            }
            // Lấy thông tin nhân viên từ API
            if (!string.IsNullOrEmpty(lop.MaSt))
            {
                HttpResponseMessage nvResponse = client.GetAsync(urlNhanVien + lop.MaSt).Result;
                if (nvResponse.IsSuccessStatusCode)
                {
                    string nvResult = nvResponse.Content.ReadAsStringAsync().Result;
                    lop.MaStNavigation = JsonConvert.DeserializeObject<NhanVien>(nvResult);
                }
            }
            return View(lop);
        }

        // POST: Admin/Lop/Delete/5
        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(string id)
        {
            HttpResponseMessage response = client.DeleteAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
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
            if(nhanViens == null)
            {
                return new List<SelectListItem>();
            }
            nhanViens = nhanViens.Where(nv => nv.TenCv == "Giáo Viên").ToList();

            // Chuyển đổi danh sách NhanVien thành danh sách SelectListItem
            var selectListItems = nhanViens.Select(nv => new SelectListItem
            {
                Value = nv.MaSt, // Giá trị bạn muốn gửi về
                Text = $" ({nv.MaSt})" + nv.HoTen// Hiển thị tên nhân viên
            }).ToList();

            return selectListItems;
        }
    }
}
