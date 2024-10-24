using Microsoft.AspNetCore.Mvc;
using mamNonTuongLaiTuoiSang.Models;


namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class GiaoVienController : Controller
    {
        private readonly QLMamNonContext db = new QLMamNonContext();
        public IActionResult Index(string id)
        {
            if(id == null)
            {
                return NotFound();
            }
            GiaoVien gv = db.GiaoViens.SingleOrDefault(gv => gv.MaSt == id);
            gv.MaStNavigation = db.NhanViens.SingleOrDefault(nv=> nv.MaSt == gv.MaSt);
            ViewData["GiaoVien"] = id;
            return View(gv);
        }
    }
}
