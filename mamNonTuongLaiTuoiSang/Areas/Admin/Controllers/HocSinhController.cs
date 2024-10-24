using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mamNonTuongLaiTuoiSang.Models;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json;
using mamNonTuongLaiTuoiSang.Areas.Admin.Models.DTO;
using System.Text;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HocSinhController : Controller
    {
        private const string baseURL = "http://localhost:5005/api/HocSinhs";
        private readonly QLMamNonContext _context;
        private HttpClient client = new HttpClient();

        public HocSinhController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: Admin/HocSinh
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewData["TenHsSortParam"] = String.IsNullOrEmpty(sortOrder) ? "tenhs_desc" : "";
            ViewData["NamSinhSortParam"] = sortOrder == "namsinh_asc" ? "namsinh_desc" : "namsinh_asc";

            List<HocSinh> hocSinhs = new List<HocSinh>();
            HttpResponseMessage response = client.GetAsync(baseURL).Result;
            
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<HocSinh>>(result);
                if (data != null)
                {
                    hocSinhs = data;
                }
            }
            switch (sortOrder)
            {
                case "tenhs_desc":
                    hocSinhs = hocSinhs.OrderByDescending(Hs => Hs.TenHs).ToList();
                    break;
                case "namsinh_asc":
                    hocSinhs = hocSinhs.OrderBy(Hs => Hs.NamSinh).ToList();
                    break;
                case "namsinh_desc":
                    hocSinhs = hocSinhs.OrderByDescending(Hs => Hs.NamSinh).ToList();
                    break;
                default:
                    hocSinhs = hocSinhs.OrderBy(Hs => Hs.TenHs).ToList();
                    break;
            }
            return View(hocSinhs);
        }

        // GET: Admin/HocSinh/Details/5
        public async Task<IActionResult> Details(string id)
        {
            HocSinh hs = new HocSinh();
            HttpResponseMessage response = client.GetAsync(baseURL + "/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<HocSinh>(result);
                if (data != null)
                {
                    hs = data;
                }
            }
            return View(hs);
        }

        // GET: Admin/HocSinh/Create
        public IActionResult Create()
        {
            ViewBag.PhuHuynhList = new SelectList(_context.PhuHuynhs, "IdPh", "IdPh");
            return View();
        }

        // POST: Admin/HocSinh/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHs,TenHs,GioiTinh,NamSinh,IdPh,QuanHe, ChieuCao, CanNang")] HocSinh hocSinh)
        {

            string data = JsonConvert.SerializeObject(hocSinh);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(baseURL, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Insert_message"] = "Add Success...";
                return RedirectToAction("Index");
            }
            ViewBag.PhuHuynhList = new SelectList(_context.PhuHuynhs, "IdPh", "IdPh", hocSinh.IdPh);
            return View(hocSinh);
        }

        // GET: Admin/HocSinh/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            HocSinh hocSinh = await FindHS(id);
            if (hocSinh == null)
            {
                return NotFound();
            }
            ViewBag.PhuHuynhList = new SelectList(_context.PhuHuynhs, "IdPh", "IdPh", hocSinh.IdPh);
            return View(hocSinh);
        }

        // POST: Admin/HocSinh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdHs,TenHs,GioiTinh,NamSinh,IdPh,QuanHe")] HocSinh hocSinh)
        {
            string data = JsonConvert.SerializeObject(hocSinh);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(baseURL + "/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["update_message"] = "Edit Success...";
                return RedirectToAction("Index");
            }
            return View(hocSinh);
        }

        // GET: Admin/HocSinh/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            HocSinh hs = new HocSinh();
            HttpResponseMessage response = client.GetAsync(baseURL + "/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<HocSinh>(result);
                if (data != null)
                {
                    hs = data;
                }
            }
            return View(hs);
        }

        // POST: Admin/HocSinh/Delete/5
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
        public async Task<HocSinh> FindHS(string id)
        {
            if (id == null)
            {
                return null;
            }
            using (var hocsinh = new HttpClient())
            {
                string path = baseURL + "/" + id;
                hocsinh.DefaultRequestHeaders.Accept.Clear();
                hocsinh.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await hocsinh.GetAsync(path);
                if (getData.IsSuccessStatusCode)
                {
                    var data = getData.Content.ReadAsStringAsync().Result;
                    var HocSinhResponse = JsonConvert.DeserializeObject<HocSinh>(data);
                    if (HocSinhResponse == null)
                    {
                        return null;
                    }
                    HocSinh hocSinh = HocSinhResponse;
                    return hocSinh;
                }
            }
            return null;
        }
        [HttpGet]
        public async Task<IActionResult> Search(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                return RedirectToAction("Index"); // Redirect to list if the query is empty
            }

            // Search by HoTen or Sdt
            var hocSinh = await _context.HocSinhs
                .FirstOrDefaultAsync(hs => hs.TenHs.Contains(searchQuery) || hs.IdPh.Contains(searchQuery));

            if (hocSinh == null)
            {
                return NotFound(); // Handle case where no parent is found
            }

            // Redirect to the details page of the found parent
            return RedirectToAction("Details", new { id = hocSinh.IdHs });
        }
        private bool HocSinhExists(string id)
        {
          return (_context.HocSinhs?.Any(e => e.IdHs == id)).GetValueOrDefault();
        }
    }
}
