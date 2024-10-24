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
    public class TkbController : Controller
    {
        private const string url = "http://localhost:5005/api/Lops/";
        private const string urlNhanVien = "http://localhost:5005/api/nhanviens/";
        private string baseURL = "http://localhost:5005/api/Tkbs";
        private readonly QLMamNonContext _context;
        private HttpClient client = new HttpClient();

        public TkbController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: Admin/Tkb
        public async Task<IActionResult> Index()
        {
            List<Tkb> tkbs = new List<Tkb>();
            HttpResponseMessage response = client.GetAsync(baseURL).Result;

            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<Tkb>>(result);
                if (data != null)
                {
                    tkbs = data;
                }
            }
            return View(tkbs);
        }

        // GET: Admin/Tkb/Details/5
        [HttpGet("Admin/Tkb/Details/{Ngay}/{IdLop}")]
        public async Task<IActionResult> Details(string IdLop, string Ngay)
        {
            if (string.IsNullOrEmpty(IdLop) || string.IsNullOrEmpty(Ngay))
            {
                return NotFound();
            }

            Tkb tKB = await FindTKB(IdLop, Ngay);
            if (tKB == null)
            {
                return NotFound();
            }
            return View(tKB);
        }



        // GET: Admin/Tkb/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewData["IdLop"] = new SelectList(_context.Lops, "IdLop", "IdLop");
            ViewData["IdMh"] = new SelectList(_context.MonHocs, "IdMh", "IdMh");
            return View();
        }

        // POST: Admin/Tkb/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("IdLop,Ngay,CaHoc,IdMh")] Tkb tkb)
        {
            string data = JsonConvert.SerializeObject(tkb);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(baseURL+"/", content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            else
            {
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine("Content: " + responseContent);
            }
            ViewData["IdLop"] = new SelectList(_context.Lops, "IdLop", "IdLop", tkb.IdLop);
            ViewData["IdMh"] = new SelectList(_context.MonHocs, "IdMh", "IdMh", tkb.IdMh);
            return View(tkb);
        }

        // GET: Admin/Tkb/Edit/5
        [HttpGet("Admin/Tkb/Edit/{Ngay}/{IdLop}")]
        public async Task<IActionResult> Edit(string IdLop, string ngay)
        {

            Tkb tkb = await FindTKB(IdLop, ngay);
            if (tkb == null)
            {
                return NotFound();
            }
            ViewData["IdLop"] = new SelectList(_context.Lops, "IdLop", "IdLop", tkb.IdLop);
            ViewData["IdMh"] = new SelectList(_context.MonHocs, "IdMh", "IdMh", tkb.IdMh);
            return View(tkb);
        }

        // POST: Admin/Tkb/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost("Admin/Tkb/Edit/{Ngay}/{IdLop}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string IdLop, string ngay, [Bind("IdLop,Ngay,CaHoc,IdMh")] Tkb tkb)
        {
                tkb.IdLopNavigation = new Lop { IdLop = IdLop };
             
                string data = JsonConvert.SerializeObject(tkb);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                // Xây dựng URL đúng cách (loại bỏ dấu +)
                
                string apiUrl = $"{baseURL}/{ngay}/{IdLop}";

                // Sử dụng async/await để tránh deadlock
                HttpResponseMessage response = await client.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    string responseContent = await response.Content.ReadAsStringAsync();
                    if (!string.IsNullOrEmpty(responseContent))
                    {
                        Console.WriteLine("Content: " + responseContent);
                    }
                    else
                    {
                        Console.WriteLine("No content returned.");
                    }
                }

            ViewData["IdLop"] = new SelectList(_context.Lops, "IdLop", "IdLop", tkb.IdLop);
            ViewData["IdMh"] = new SelectList(_context.MonHocs, "IdMh", "IdMh", tkb.IdMh);
            return View(tkb);
        }

        // GET: Admin/Tkb/Delete/5
        [HttpGet("Admin/Tkb/Delete/{Ngay}/{IdLop}")]
        public async Task<IActionResult> Delete(string IdLop,string Ngay)
        {
            if (string.IsNullOrEmpty(IdLop) || string.IsNullOrEmpty(Ngay))
            {
                return NotFound();
            }

            Tkb tKB = await FindTKB(IdLop, Ngay);
            if (tKB == null)
            {
                return NotFound();
            }
            return View(tKB);
        }

        // POST: Admin/Tkb/Delete/5
        [HttpPost("Admin/Tkb/Delete/{Ngay}/{IdLop}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string IdLop,string Ngay)
        {
            string apiUrl = $"{baseURL}/{Ngay}/{IdLop}";

            // Gửi yêu cầu DELETE đến API
            HttpResponseMessage response = await client.DeleteAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            // Nếu có lỗi, thêm thông báo lỗi vào ModelState và trả về View
            ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi xóa chức vụ.");
            Tkb tKB = await FindTKB(IdLop, Ngay);
            return View(tKB);
        }

        private bool TkbExists(string id)
        {
            return (_context.Tkbs?.Any(e => e.IdLop == id)).GetValueOrDefault();
        }
        public async Task<Tkb> FindTKB(string id, string ngay)
        {
            using (var tkb = new HttpClient())
            {
                string path = $"{baseURL}/{ngay}/{id}";
                tkb.DefaultRequestHeaders.Accept.Clear();
                tkb.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await tkb.GetAsync(path);
                if (getData.IsSuccessStatusCode)
                {
                    var data = await getData.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<Tkb>(data);
                    return response;
                }
                else
                {
                    string responseContent = await getData.Content.ReadAsStringAsync();
                    Console.WriteLine("Content: " + responseContent);
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

            var tkb = await _context.Tkbs
            .FirstOrDefaultAsync(tKB => tKB.IdLop.Contains(searchQuery));

            if (tkb == null)
            {
                return NotFound();
            }

            return RedirectToAction("Details", new { id = tkb.IdLop });
        }
        public async Task<Lop> FindLop(string id)
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
            return lop;
        }
    }

}
