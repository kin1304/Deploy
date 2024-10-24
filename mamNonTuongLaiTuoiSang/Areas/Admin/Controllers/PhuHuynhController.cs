using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using mamNonTuongLaiTuoiSang.Models;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json;
using mamNonTuongLaiTuoiSang.Areas.Admin.Models.DTO;
using System.Text;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PhuHuynhController : Controller
    {
        private const string baseURL = "http://localhost:5005/api/PhuHuynhs";
        private readonly QLMamNonContext _context;
        private HttpClient client=new HttpClient();

        public PhuHuynhController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: Admin/PhuHuynh
        public IActionResult Index(string sortOrder)
        {
            ViewData["HoTenSortParam"] = String.IsNullOrEmpty(sortOrder) ? "hoten_desc" : "";
            ViewData["NamSinhSortParam"] = sortOrder == "namsinh_asc" ? "namsinh_desc" : "namsinh_asc";

            List<PhuHuynh> phuHuynhs = new List<PhuHuynh>();
            HttpResponseMessage response = client.GetAsync(baseURL).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<PhuHuynh>>(result);
                if (data != null)
                {
                    phuHuynhs = data;
                }
            }
            switch (sortOrder)
            {
                case "hoten_desc":
                    phuHuynhs = phuHuynhs.OrderByDescending(ph => ph.HoTen).ToList();
                    break;
                case "namsinh_asc":
                    phuHuynhs = phuHuynhs.OrderBy(ph => ph.NamSinh).ToList();
                    break;
                case "namsinh_desc":
                    phuHuynhs = phuHuynhs.OrderByDescending(ph => ph.NamSinh).ToList();
                    break;
                default:
                    phuHuynhs = phuHuynhs.OrderBy(ph => ph.HoTen).ToList();
                    break;
            }

            return View(phuHuynhs);
        }


        // GET: Admin/PhuHuynh/Details/5
        [HttpGet]
        public IActionResult Details(string id)
        {
            PhuHuynh ph=new PhuHuynh();
            HttpResponseMessage response= client.GetAsync(baseURL+"/"+id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result =response.Content.ReadAsStringAsync().Result;
                var data=JsonConvert.DeserializeObject<PhuHuynh>(result);
                if (data != null)
                {
                    ph = data;
                }
            }
            return View(ph);
        }


        // GET: Admin/PhuHuynh/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/PhuHuynh/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdPh,HoTen,DiaChi,GioiTinh,NgheNghiep,NamSinh,MatKhau,Email,Sdt")] PhuHuynh phuHuynh)
        {
            // Kiểm tra xem IdPH đã tồn tại hay chưa
            string data = JsonConvert.SerializeObject(phuHuynh);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(baseURL, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["Insert_message"] = "Add Success...";
                return RedirectToAction("Index");
            }
            return View(phuHuynh);

        }

        // GET: Admin/PhuHuynh/Edit/5
        public async Task<PhuHuynh> FindPH(string id)
        {
            if (id == null)
            {
                return null;
            }
            using (var phuhuynh = new HttpClient())
            {
                string path = baseURL + "/" + id;
                phuhuynh.DefaultRequestHeaders.Accept.Clear();
                phuhuynh.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await phuhuynh.GetAsync(path);
                if (getData.IsSuccessStatusCode)
                {
                    var data = getData.Content.ReadAsStringAsync().Result;
                    var phuHuynhResponse = JsonConvert.DeserializeObject<PhuHuynh>(data);
                    if (phuHuynhResponse == null)
                    {
                        return null;
                    }
                    PhuHuynh phuHuynh = phuHuynhResponse;
                    return phuHuynh;
                }
            }
            return null;


        }
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            PhuHuynh phuHuynh = await FindPH(id);
            if (phuHuynh == null)
            {
                return NotFound();
            }
            return View(phuHuynh);
        }

        // POST: Admin/PhuHuynh/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdPh,HoTen,DiaChi,GioiTinh,NgheNghiep,NamSinh,MatKhau,Email,Sdt")] PhuHuynh phuHuynh)
        {
            string data = JsonConvert.SerializeObject(phuHuynh);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PutAsync(baseURL+"/" + id, content).Result;
            if (response.IsSuccessStatusCode)
            {
                TempData["update_message"] = "Edit Success...";
                return RedirectToAction("Index");
            }
            return View(phuHuynh);
        }

        // GET: Admin/PhuHuynh/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            PhuHuynh ph = new PhuHuynh();
            HttpResponseMessage response = client.GetAsync(baseURL + "/" + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<PhuHuynh>(result);
                if (data != null)
                {
                    ph = data;
                }
            }
            return View(ph);
        }

        // POST: Admin/PhuHuynh/Delete/5
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

        private bool PhuHuynhExists(string id)
        {
            return (_context.PhuHuynhs?.Any(e => e.IdPh == id)).GetValueOrDefault();
        }

        [HttpGet]
        public async Task<IActionResult> Search(string searchQuery)
        {
            if (string.IsNullOrEmpty(searchQuery))
            {
                return RedirectToAction("Index"); // Redirect to list if the query is empty
            }

            // Search by HoTen or Sdt
            var phuHuynh = await _context.PhuHuynhs
                .FirstOrDefaultAsync(ph => ph.HoTen.Contains(searchQuery) || ph.Sdt.Contains(searchQuery));

            if (phuHuynh == null)
            {
                return NotFound(); // Handle case where no parent is found
            }

            // Redirect to the details page of the found parent
            return RedirectToAction("Details", new { id = phuHuynh.IdPh });
        }

    }
}
