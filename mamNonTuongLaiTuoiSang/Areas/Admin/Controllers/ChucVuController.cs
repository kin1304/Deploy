
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mamNonTuongLaiTuoiSang.Models;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace mamNonTuongLaiTuoiSang.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ChucVuController : Controller
    {
        private string url = "http://localhost:5005/api/chucvus/";
        private HttpClient client = new HttpClient();

        // GET: Admin/ChucVu
        [HttpGet]
        public IActionResult Index()
        {
            List<ChucVu> chucvus = new List<ChucVu>();
            HttpResponseMessage response = client.GetAsync(url).Result;
            if (response.IsSuccessStatusCode)
            {
                string result = response.Content.ReadAsStringAsync().Result;
                var data = JsonConvert.DeserializeObject<List<ChucVu>>(result);
                if (data != null)
                {
                    chucvus = data;
                }
            }
            return View(chucvus);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Admin/ChucVu/Create
        [HttpPost]
        public IActionResult Create(ChucVu cv)
        {
            string data = JsonConvert.SerializeObject(cv);
            StringContent content = new StringContent(data, Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(url, content).Result;
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }
            return View();
        }

        // GET: Admin/ChucVu/Edit/a/a
        // GET: Admin/ChucVu/Edit/{TenCv}/{ViTri}
        [HttpGet("Admin/ChucVu/Edit/{TenCv}/{ViTri}")]
        public async Task<IActionResult> Edit(string TenCv, string ViTri)
        {
            if (string.IsNullOrEmpty(TenCv) || string.IsNullOrEmpty(ViTri))
            {
                return NotFound();
            }

            ChucVu chucVu = await FindHSL(TenCv, ViTri);
            if (chucVu == null)
            {
                return NotFound();
            }
            return View(chucVu);
        }

        // POST: Admin/ChucVu/Edit/{TenCv}/{ViTri}
        [HttpPost("Admin/ChucVu/Edit/{TenCv}/{ViTri}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string TenCv, string ViTri, ChucVu cv)
        {
            if (TenCv != cv.TenCv || ViTri != cv.ViTri)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                string data = JsonConvert.SerializeObject(cv);
                StringContent content = new StringContent(data, Encoding.UTF8, "application/json");

                // Xây dựng URL đúng cách (loại bỏ dấu +)
                string apiUrl = $"{url}{cv.TenCv}/{cv.ViTri}";

                // Sử dụng async/await để tránh deadlock
                HttpResponseMessage response = await client.PutAsync(apiUrl, content);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    // Thêm thông báo lỗi vào ModelState để hiển thị cho người dùng
                    ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi cập nhật chức vụ.");
                }
            }

            // Nếu ModelState không hợp lệ, trả về View cùng với dữ liệu hiện tại
            return View(cv);
        }


        // GET: Admin/ChucVu/Details/a/a
        [HttpGet("Admin/ChucVu/Details/{TenCv}/{ViTri}")]
        public async Task<IActionResult> Details(string TenCv, string ViTri)
        {
            if (string.IsNullOrEmpty(TenCv) || string.IsNullOrEmpty(ViTri))
            {
                return NotFound();
            }

            ChucVu chucVu = await FindHSL(TenCv, ViTri);
            if (chucVu == null)
            {
                return NotFound();
            }
            return View(chucVu);
        }

        // GET: Admin/ChucVu/Delete/{TenCv}/{ViTri}
        [HttpGet("Admin/ChucVu/Delete/{TenCv}/{ViTri}")]
        public async Task<IActionResult> Delete(string TenCv, string ViTri)
        {
            if (string.IsNullOrEmpty(TenCv) || string.IsNullOrEmpty(ViTri))
            {
                return NotFound();
            }

            ChucVu chucVu = await FindHSL(TenCv, ViTri);
            if (chucVu == null)
            {
                return NotFound();
            }
            return View(chucVu);
        }

        // POST: Admin/ChucVu/Delete/{TenCv}/{ViTri}
        [HttpPost("Admin/ChucVu/Delete/{TenCv}/{ViTri}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string TenCv, string ViTri)
        {
            // Xây dựng URL API để xóa ChucVu
            string apiUrl = $"{url}{TenCv}/{ViTri}";

            // Gửi yêu cầu DELETE đến API
            HttpResponseMessage response = await client.DeleteAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("Index");
            }

            // Nếu có lỗi, thêm thông báo lỗi vào ModelState và trả về View
            ModelState.AddModelError(string.Empty, "Có lỗi xảy ra khi xóa chức vụ.");
            ChucVu chucVu = await FindHSL(TenCv, ViTri);
            return View(chucVu);
        }

        public async Task<ChucVu> FindHSL(string TenCv, string ViTri)
        {
            if (string.IsNullOrEmpty(TenCv) || string.IsNullOrEmpty(ViTri))
            {
                return null;
            }

            using (var chucvu = new HttpClient())
            {
                string path = $"{url}{TenCv}/{ViTri}";
                chucvu.DefaultRequestHeaders.Accept.Clear();
                chucvu.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                HttpResponseMessage getData = await chucvu.GetAsync(path);
                if (getData.IsSuccessStatusCode)
                {
                    var data = await getData.Content.ReadAsStringAsync();
                    var response = JsonConvert.DeserializeObject<ChucVu>(data);
                    return response;
                }
            }
            return null;
        }
    }
}
