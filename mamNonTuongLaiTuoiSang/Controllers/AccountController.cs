using mamNonTuongLaiTuoiSang.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Org.BouncyCastle.Crypto.IO;

namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class AccountController : Controller
    {
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        private readonly QLMamNonContext db;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly ILogger<AccountController> logger;
        public AccountController(QLMamNonContext context, IHttpContextAccessor httpContextAccessor, ILogger<AccountController> logger)
        {
            this.db = context;
            this.httpContextAccessor = httpContextAccessor;
            this.logger = logger;
        }

        [HttpPost]
        public ActionResult Login(string email, string password)
        {
            try
            {

                var session = httpContextAccessor.HttpContext.Session;
                var parent = db.PhuHuynhs.SingleOrDefault(ph => ph.Email == email && ph.MatKhau == password);
                if (parent != null)
                {
                    session.SetString("UserEmail", parent.Email);
                    session.SetString("Password", parent.MatKhau);
                    ViewData["PhuHuynh"] = parent.IdPh;
                    return RedirectToAction("PhuHuynh", "PhuHuynh", parent);
                }

                var user = db.NhanViens.SingleOrDefault(nv => nv.Email == email && nv.MatKhau == password);
                if (user != null)
                {
                    session.SetString("UserEmail", user.Email);
                    session.SetString("Password", user.MatKhau);

                    if (user.TenCv == "Admin")
                    {
                        return RedirectToAction("Index", "NhanVien", new { area = "Admin" });
                    }
                    else if(user.TenCv == "Giáo Viên")
                    {
                        ViewData["GiaoVien"] = user.MaSt;
                        ViewBag.GiaoVien = user.MaSt;
                        return RedirectToAction("Index", "GiaoVien", new { id = user.MaSt });
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = "Invalid email or password";
                    return View();
                }

                // Nếu đăng nhập thất bại, hiển thị thông báo lỗi
                ViewBag.ErrorMessage = "Invalid email or password";
                return View();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Error occurred while logging in.");
                ViewBag.ErrorMessage = "An error occurred while accessing the database.";
                return View();
            }
        }

        [HttpPost]
        public ActionResult Logout()
        {
            var session = httpContextAccessor.HttpContext.Session;
            session.Clear();

            // Redirect to login page
            return Ok(); // Trả về Ok để báo cho client rằng đã đăng xuất thành công
        }


    }
}
