using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mamNonTuongLaiTuoiSang.Models;
using Newtonsoft.Json;
using mamNonTuongLaiTuoiSang.Areas.Admin.Models.DTO;
using System.Text;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HocSinhLopController : Controller
    {
        private string baseURL = "http://localhost:5005/api/HocSinhLops";
        private readonly QLMamNonContext _context;
        private HttpClient client = new HttpClient();

        public HocSinhLopController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: Admin/HocSinhLop
        public async Task<IActionResult> Index(string sortOrder, string diemChuyenCanFilter)
        {
            ViewData["DiemChuyenCanSortParam"] = sortOrder == "diemchuyencan_asc" ? "diemchuyencan_desc" : "diemchuyencan_asc";
            ViewBag.DiemChuyenCanSortParm = sortOrder == "diemchuyencan_asc" ? "diemchuyencan_desc" : "diemchuyencan_asc";
            ViewBag.FilterOption = diemChuyenCanFilter;

            ViewBag.DiemChuyenCanFilterOptions = new SelectList(new[]
            {
                new { Value = "All", Text = "All" },
                new { Value = "below5", Text = "Dưới 5" },
                new { Value = "between5and8", Text = "Từ 5 đến dưới 8" },
                new { Value = "between8and10", Text = "Từ 8 đến 10" }
            }, "Value", "Text", diemChuyenCanFilter);

            List<HocSinhLop> hocSinhLops = new List<HocSinhLop>();
            HttpResponseMessage response = client.GetAsync(baseURL).Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<HocSinhLop>>(result);
                if (data != null)
                {
                    hocSinhLops = data;
                }
            }
            if (!string.IsNullOrEmpty(diemChuyenCanFilter) && diemChuyenCanFilter != "All")
            {
                switch (diemChuyenCanFilter)
                {
                    case "below5":
                        hocSinhLops = hocSinhLops.Where(hs => hs.DiemChuyenCan < 5).ToList();
                        break;
                    case "between5and8":
                        hocSinhLops = hocSinhLops.Where(hs => hs.DiemChuyenCan >= 5 && hs.DiemChuyenCan < 8).ToList();
                        break;
                    case "between8and10":
                        hocSinhLops = hocSinhLops.Where(hs => hs.DiemChuyenCan >= 8 && hs.DiemChuyenCan <= 10).ToList();
                        break;
                    default:
                        break;
                }
            }

            // Sắp xếp kết quả
            switch (sortOrder)
            {
                case "id_desc":
                    hocSinhLops = hocSinhLops.OrderByDescending(hs => hs.IdHs).ToList();
                    break;
                case "diemchuyencan_asc":
                    hocSinhLops = hocSinhLops.OrderBy(hs => hs.DiemChuyenCan).ToList();
                    break;
                case "diemchuyencan_desc":
                    hocSinhLops = hocSinhLops.OrderByDescending(hs => hs.DiemChuyenCan).ToList();
                    break;
                default:
                    hocSinhLops = hocSinhLops.OrderBy(hs => hs.IdHs).ToList();
                    break;
            }
        
            return View(hocSinhLops);
        }

        // GET: Admin/HocSinhLop/Details/5
        public async Task<IActionResult> Details(string id)
        {
            HocSinhLop hsl = new HocSinhLop();
            HttpResponseMessage response = client.GetAsync(baseURL + "/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<HocSinhLop>(result);
                if (data != null)
                {
                    hsl = data;
                }
            }
            return View(hsl);
        }

        // GET: Admin/HocSinhLop/Create
        public IActionResult Create()
        {
            ViewData["IdHs"] = new SelectList(_context.HocSinhs, "IdHs", "IdHs");
            ViewData["IdLop"] = new SelectList(_context.Lops, "IdLop", "IdLop");
            return View();
        }

        // POST: Admin/HocSinhLop/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdHs,IdLop,DiemChuyenCan")] HocSinhLop hocSinhLop)
        {
            string data = JsonConvert.SerializeObject(hocSinhLop);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(baseURL+"/", content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Insert_message"] = "Add Success...";
                return RedirectToAction("Index");
            }
            ViewData["IdHs"] = new SelectList(_context.HocSinhs, "IdHs", "IdHs");
            ViewData["IdLop"] = new SelectList(_context.Lops, "IdLop", "IdLop");
            return View(hocSinhLop);
            
        }


        // GET: Admin/HocSinhLop/Edit/5
        public async Task<IActionResult> Edit(string id)
        {

            if (id == null)
            {
                return NotFound();
            }
            HocSinhLop hocSinhLop = await FindHSL(id);
            if (hocSinhLop == null)
            {
                return NotFound();
            }
            ViewData["IdHs"] = new SelectList(_context.HocSinhs, "IdHs", "IdHs", hocSinhLop.IdHs);
            ViewData["IdLop"] = new SelectList(_context.Lops, "IdLop", "IdLop", hocSinhLop.IdLop);
            return View(hocSinhLop);
        }

        // POST: Admin/HocSinhLop/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdHs,IdLop,DiemChuyenCan")] HocSinhLop hocSinhLop)
        {
            string data = JsonConvert.SerializeObject(hocSinhLop);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(baseURL + "/" + hocSinhLop.IdHs, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["update_message"] = "Edit Success...";
                return RedirectToAction("Index");
            }
            return View(hocSinhLop);
        }


        // GET: Admin/HocSinhLop/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            HocSinhLop hsl = new HocSinhLop();
            HttpResponseMessage response = client.GetAsync(baseURL + "/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<HocSinhLop>(result);
                if (data != null)
                {
                    hsl = data;
                }
            }
            return View(hsl);
        }

        // POST: Admin/HocSinhLop/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            HttpResponseMessage response = client.DeleteAsync(baseURL + "/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                    TempData["delete_message"] = "Deleted...";
                    return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<HocSinhLop> FindHSL(string id)
        {
            if (id == null)
            {
                return null;
            }
            using (var hocsinhlop = new HttpClient())
            {
                string path = baseURL + "/" + id;
                hocsinhlop.DefaultRequestHeaders.Accept.Clear();
                hocsinhlop.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await hocsinhlop.GetAsync(path);
                if (getData.IsSuccessStatusCode)
                {
                    var data = getData.Content.ReadAsStringAsync().Result;
                    var HocSinhLopResponse = JsonConvert.DeserializeObject<HocSinhLop>(data);
                    if (HocSinhLopResponse == null)
                    {
                        return null;
                    }
                    HocSinhLop hocSinhLop = HocSinhLopResponse;
                    return hocSinhLop;
                }
            }
            return null;
        }
        [HttpGet]
        public async Task<IActionResult> Search(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                return RedirectToAction("Index"); 
            }

            var hocSinhLop = await _context.HocSinhLops
            .FirstOrDefaultAsync(hsl => hsl.IdHs.Contains(searchQuery));

            if (hocSinhLop == null)
            {
                return NotFound(); 
            }

            return RedirectToAction("Details", new { id = hocSinhLop.IdHs });
        }
        private bool HocSinhLopExists(string id)
        {
          return (_context.HocSinhLops?.Any(e => e.IdHs == id)).GetValueOrDefault();
        }
        private void LogError(string message)
        {
            // You can expand this method to log to a file, database, or monitoring system
            Console.WriteLine($"Error: {message}"); // Replace with your logging mechanism
        }

    }
}
