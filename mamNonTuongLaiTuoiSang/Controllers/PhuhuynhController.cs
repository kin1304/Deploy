using Microsoft.AspNetCore.Mvc;
using mamNonTuongLaiTuoiSang.Models;
using Newtonsoft.Json;

namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class PhuhuynhController : Controller
    {
        private const string baseURL = "http://localhost:5005/api/PhuHuynhs";
        private HttpClient client = new HttpClient();
        [HttpGet]
        public IActionResult PhuHuynh(PhuHuynh ph)
        {
            TempData["PhuHuynh"] = ph.IdPh;
            ViewData["PhuHuynh"] = ph.IdPh;
            return View(ph);
        }
        [HttpGet]
        public IActionResult Info(string id)
        {
            TempData["PhuHuynh"] = id;
            ViewData["PhuHuynh"] = id;
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
    }
}
