using mamNonTuongLaiTuoiSang.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NhanVienController : Controller
    {
        private const string baseURL = "http://localhost:5005/api/NhanViens";
        private readonly QLMamNonContext _context;
        private readonly ILogger<NhanVienController> _logger;
        private HttpClient client = new HttpClient();

        public NhanVienController(QLMamNonContext context, ILogger<NhanVienController> logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.NamLamSortParm = sortOrder == "namlam_asc" ? "namlam_desc" : "namlam_asc";

            List<NhanVien> nhanViens = new List<NhanVien>();
            HttpResponseMessage response = client.GetAsync(baseURL).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<NhanVien>>(result);
                if (data != null)
                {
                    nhanViens = data;
                }
            }
            switch (sortOrder)
                {
                    case "name_desc":
                        nhanViens = nhanViens.OrderByDescending(nv => nv.HoTen).ToList();
                        break;
                    case "namlam_asc":
                        nhanViens = nhanViens.OrderBy(nv => nv.NamLam).ToList();
                        break;
                    case "namlam_desc":
                        nhanViens = nhanViens.OrderByDescending(nv => nv.NamLam).ToList();
                        break;
                    default:
                        nhanViens = nhanViens.OrderBy(nv => nv.HoTen).ToList();
                        break;
                }
                return View(nhanViens);
        }

        // GET: Admin/NhanVien/Create
        public async Task<IActionResult> Create()
        {
                ViewBag.ChucVuList = new SelectList(_context.ChucVus, "TenCv", "TenCv");
                ViewBag.ViTriList = new SelectList(_context.ChucVus, "ViTri", "ViTri");
                return View();
        }

        // POST: Admin/NhanVien/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(NhanVien nhanVien)
            {
                string data = JsonConvert.SerializeObject(nhanVien);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(baseURL, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Insert_message"] = "Add Success...";
                return RedirectToAction("Index");
            }
            ViewBag.ChucVuList = new SelectList(_context.ChucVus, "TenCv", "TenCv");
            ViewBag.ViTriList = new SelectList(_context.ChucVus, "ViTri", "ViTri");
            return View(nhanVien);
        }
        public async Task<IActionResult> Edit(String? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            NhanVien nhanVien = await FindNV(id);
            if (nhanVien == null)
            {
                return NotFound();
            }
            ViewBag.ChucVuList = new SelectList(_context.ChucVus, "TenCv", "TenCv");
            ViewBag.ViTriList = new SelectList(_context.ChucVus, "ViTri", "ViTri");

            return View(nhanVien);
        }

        // POST: Admin/NhanVien/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(String id, NhanVien nhanVien)
        {
            string data = JsonConvert.SerializeObject(nhanVien);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(baseURL + "/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["update_message"] = "Edit Success...";
                return RedirectToAction("Index");
            }
            return View(nhanVien);
        }
        public async Task<IActionResult> Details(string id)
        {
            NhanVien nv = new NhanVien();
            HttpResponseMessage response = client.GetAsync(baseURL + "/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<NhanVien>(result);
                if (data != null)
                {
                    nv = data;
                }
            }
            return View(nv);
        }

        public async Task<IActionResult> Delete(string id)
        {
            NhanVien nv = new NhanVien();
            HttpResponseMessage response = client.GetAsync(baseURL + "/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<NhanVien>(result);
                if (data != null)
                {
                    nv = data;
                }
            }
            return View(nv);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            HttpResponseMessage response = client.DeleteAsync(baseURL + "/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                if (response.IsSuccessStatusCode)
                {
                    TempData["delete_message"] = "Deleted...";
                    return RedirectToAction("Index");
                }
            }
            return View();
        }

        // Helper method to check if a NhanVien exists by ID
        private bool NhanVienExists(String id)
        {
            return _context.NhanViens.Any(e => e.MaSt == id);
        }
        public async Task<NhanVien> FindNV(string id)
        {
            if (id == null)
            {
                return null;
            }
            using (var nhanvien = new HttpClient())
            {
                string path = baseURL + "/" + id;
                nhanvien.DefaultRequestHeaders.Accept.Clear();
                nhanvien.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await nhanvien.GetAsync(path);
                if (getData.IsSuccessStatusCode)
                {
                    var data = getData.Content.ReadAsStringAsync().Result;
                    var nhanVienResponse = JsonConvert.DeserializeObject<NhanVien>(data);
                    if (nhanVienResponse == null)
                    {
                        return null;
                    }
                    NhanVien nhanVien = nhanVienResponse;
                    return nhanVien;
                }
            }
            return null;


        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchQuery)
        {
            ViewBag.Placeholder = "Tìm kiếm theo tên hoặc số điện thoại...";
            if (string.IsNullOrEmpty(searchQuery))
            {
                return RedirectToAction("Index"); // Redirect to list if the query is empty
            }

            // Search by HoTen or Sdt
            var nhanVien = await _context.NhanViens
                .FirstOrDefaultAsync(nv => nv.HoTen.Contains(searchQuery) || nv.Sdt.Contains(searchQuery));

            if (nhanVien == null)
            {
                return NotFound(); // Handle case where no employee is found
            }

            // Redirect to the details page of the found employee
            return RedirectToAction("Details", new { id = nhanVien.MaSt });
        }


    }
}
