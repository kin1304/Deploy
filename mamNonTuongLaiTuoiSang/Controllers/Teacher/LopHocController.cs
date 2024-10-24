using mamNonTuongLaiTuoiSang.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using System.Text;

namespace mamNonTuongLaiTuoiSang.Controllers.Teacher
{
    public class LopHocController : Controller
    {
        private string url = "http://localhost:5005/api/hocsinhs/";
        private string urlPH = "http://localhost:5005/api/phuhuynhs/";
        private string urlLop = "http://localhost:5005/api/lops/getLopByMaSt/";
        private string urldslop = "http://localhost:5005/api/HocSinhLops/lop/";
        private HttpClient client = new HttpClient();

        public IActionResult Index(string id)
        {
            List<Lop> lops = new List<Lop>();
            HttpResponseMessage response = client.GetAsync(urlLop + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<Lop>>(result);
                if (data != null)
                {
                    lops = data;
                }
            }
            ViewData["GiaoVien"] = id;
            TempData["GiaoVien"] = id;
            return View(lops);
        }
        //public IActionResult Index()
        //{
        //    List<HocSinh> hs = new List<HocSinh>(); 
        //    HttpResponseMessage response = client.GetAsync(url).Result;
        //    if (response.IsSuccessStatusCode) 
        //    {
        //        string result = response.Content.ReadAsStringAsync().Result;
        //        var data=JsonConvert.DeserializeObject<List<HocSinh>>(result);
        //        if (data != null) 
        //        {
        //            hs = data;
        //        }
        //    }
        //    return View(hs);
        //}

        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            ViewData["GiaoVien"] = TempData["GiaoVien"] as string;
            ViewData["Lop"] = TempData["Lop"] as string;
            HocSinh dd = new HocSinh();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<HocSinh>(result);
                if (data != null)
                {
                    dd = data;
                }
            }
            ViewBag.IdPh = await GetIdPhSelectListAsync();
            TempData["GiaoVien"] = ViewData["GiaoVien"] as string;
            TempData["Lop"] = ViewData["Lop"] as string;
            return View(dd);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("IdHs,TenHs,GioiTinh,NamSinh,IdPh,QuanHe,ChieuCao,CanNang")] HocSinh dd)
        {
            ViewData["GiaoVien"] = TempData["GiaoVien"] as string;
            ViewData["Lop"] = TempData["Lop"] as string;
            string data = JsonConvert.SerializeObject(dd);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PutAsync(url + dd.IdHs, content);
            if (response.IsSuccessStatusCode)
            {
                TempData["GiaoVien"] = ViewData["GiaoVien"] as string;
                TempData["Lop"] = ViewData["Lop"] as string;
                return RedirectToAction("DanhSachLop", "LopHoc", new { id = TempData["Lop"].ToString() });

            }
            else
            {
                TempData["GiaoVien"] = ViewData["GiaoVien"] as string;
                TempData["Lop"] = ViewData["Lop"] as string;
                ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi tạo điểm danh.");
            }


            // Nếu ModelState không hợp lệ hoặc API trả về lỗi, tái định nghĩa lại ViewBag
            ViewBag.IdHs = await GetIdPhSelectListAsync();
            return View(dd);
        }

        private async Task<IEnumerable<SelectListItem>> GetIdPhSelectListAsync()
        {
            List<PhuHuynh> ph = new List<PhuHuynh>();
            HttpResponseMessage response = await client.GetAsync(urlPH);
            if (response.IsSuccessStatusCode)
            {
                string result = await response.Content.ReadAsStringAsync();
                ph = JsonConvert.DeserializeObject<List<PhuHuynh>>(result) ?? new List<PhuHuynh>();
            }

            return ph.Select(l => new SelectListItem
            {
                Value = l.IdPh, // Giá trị bạn muốn gửi về
                Text = $"({l.IdPh}) {l.HoTen}" // Hiển thị tên lớp
            }).ToList();
        }
        public IActionResult DanhSachLop(string id)
        {
            ViewData["GiaoVien"] = TempData["GiaoVien"] as string;
            List<HocSinhLop> hsl = new List<HocSinhLop>();
            HttpResponseMessage response = client.GetAsync(urldslop + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<HocSinhLop>>(result);
                if (data != null)
                {
                    hsl = data;
                }
            }
            List<HocSinh> hs = new List<HocSinh>();
            for (int i = 0; i < hsl.Count; i++)
            {
                HttpResponseMessage response1 = client.GetAsync(url + hsl[i].IdHs).Result;
                if (response1.IsSuccessStatusCode)
                {
                    string result = response1.Content.ReadAsStringAsync().Result;
                    var data = JsonConvert.DeserializeObject<HocSinh>(result);
                    if (data != null)
                    {
                        hs.Add(data);
                    }
                }
            }
            TempData["GiaoVien"] = ViewData["GiaoVien"];
            TempData["Lop"] = id;
            return View(hs);
        }
    }
}
