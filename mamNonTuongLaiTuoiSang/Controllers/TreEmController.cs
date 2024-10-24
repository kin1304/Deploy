using Microsoft.AspNetCore.Mvc;
using mamNonTuongLaiTuoiSang.Models;
using Newtonsoft.Json;

namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class TreEmController : Controller
    {
        private readonly string url = "http://localhost:5005/api/HocSinhs/ByPhuHuynh/";
        private readonly string urlDetails = "http://localhost:5005/api/HocSinhs/";

        private HttpClient client = new HttpClient();

        [HttpGet]
        public IActionResult Index(string id)
        {
            ViewData["PhuHuynh"] = id;
            TempData["PhuHuynh"] = id;
            List<HocSinh> hocSinhs = new List<HocSinh>();
            HttpResponseMessage response = client.GetAsync(url + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<HocSinh>>(result);
                if (data != null)
                {
                    hocSinhs = data;
                }
                
            }
            else
            {
                var responseContent = response.Content.ReadAsStringAsync();
                Console.WriteLine("Content: " + responseContent);
            }
            return View(hocSinhs);
        }
        [HttpGet]
        public IActionResult babyinfo(string id)
        {
            ViewData["PhuHuynh"] = TempData["PhuHuynh"] as string;
            HocSinh hocSinh = new HocSinh();
            HttpResponseMessage response = client.GetAsync(urlDetails + id).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<HocSinh>(result);
                if (data != null)
                {
                    hocSinh = data;
                }

            }
            TempData["PhuHuynh"] = ViewData["PhuHuynh"] as string;
            return View(hocSinh);
        }
    }
}
