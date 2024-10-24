using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mamNonTuongLaiTuoiSang.Models;
using Newtonsoft.Json;
using System.Text;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class MonHocController : Controller
    {
        private const string baseURL = "http://localhost:5005/api/MonHocs";
        private readonly QLMamNonContext _context;
        private HttpClient client = new HttpClient();


        public MonHocController(QLMamNonContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        // GET: Admin/MonHoc
        public async Task<IActionResult> Index(string sortOrder)
        {
            ViewBag.NameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";

            List<MonHoc> monHocs = new List<MonHoc>();
            HttpResponseMessage response = client.GetAsync(baseURL).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<MonHoc>>(result);
                if (data != null)
                {
                    monHocs = data;
                }
            }
            switch (sortOrder)
            {
                case "name_desc":
                    monHocs = monHocs.OrderByDescending(nv => nv.TenMh).ToList();
                    break;
                default:
                    monHocs = monHocs.OrderBy(nv => nv.TenMh).ToList();
                    break;
            }
            return View(monHocs);

        }

        // GET: Admin/MonHoc/Details/5
        public async Task<IActionResult> Details(string id)
        {
            MonHoc mh = new MonHoc();
            HttpResponseMessage response = client.GetAsync(baseURL + "/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<MonHoc>(result);
                if (data != null)
                {
                    mh = data;
                }
            }
            return View(mh);
        }

        // GET: Admin/MonHoc/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/MonHoc/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdMh,TenMh")] MonHoc monHoc)
        {
            string data = JsonConvert.SerializeObject(monHoc);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(baseURL, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Insert_message"] = "Add Success...";
                return RedirectToAction("Index");
            }
            return View(monHoc);

        }

        // GET: Admin/MonHoc/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            MonHoc monHoc = await FindMH(id);
            if (monHoc == null)
            {
                return NotFound();
            }
            return View(monHoc);

        }

        // POST: Admin/MonHoc/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdMh,TenMh")] MonHoc monHoc)
        {
            string data = JsonConvert.SerializeObject(monHoc);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(baseURL + "/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["update_message"] = "Edit Success...";
                return RedirectToAction("Index");
            }
            return View(monHoc);
        }

        // GET: Admin/MonHoc/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            MonHoc mh = new MonHoc();
            HttpResponseMessage response = client.GetAsync(baseURL + "/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<MonHoc>(result);
                if (data != null)
                {
                    mh = data;
                }
            }
            return View(mh);
        }

        // POST: Admin/MonHoc/Delete/5
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

        private bool MonHocExists(string id)
        {
          return (_context.MonHocs?.Any(e => e.IdMh == id)).GetValueOrDefault();
        }
        public async Task<MonHoc> FindMH(string id)
        {
            if (id == null)
            {
                return null;
            }
            using (var monhoc = new HttpClient())
            {
                string path = baseURL + "/" + id;
                monhoc.DefaultRequestHeaders.Accept.Clear();
                monhoc.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await monhoc.GetAsync(path);
                if (getData.IsSuccessStatusCode)
                {
                    var data = getData.Content.ReadAsStringAsync().Result;
                    var monHocResponse = JsonConvert.DeserializeObject<MonHoc>(data);
                    if (monHocResponse == null)
                    {
                        return null;
                    }
                    MonHoc monHoc = monHocResponse;
                    return monHoc;
                }
            }
            return null;


        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchQuery)
        {
            ViewBag.Placeholder = "Tìm kiếm theo tên ...";
            if (string.IsNullOrEmpty(searchQuery))
            {
                return RedirectToAction("Index"); // Redirect to list if the query is empty
            }

            // Search by HoTen or Sdt
            var monHoc = await _context.MonHocs
                .FirstOrDefaultAsync(mh => mh.TenMh.Contains(searchQuery));

            if (monHoc == null)
            {
                return NotFound(); // Handle case where no employee is found
            }

            // Redirect to the details page of the found employee
            return RedirectToAction("Details", new { id = monHoc.IdMh });
        }

    }
}
