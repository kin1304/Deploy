using Microsoft.AspNetCore.Mvc;

namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class Thoikhoabieu : Controller
    {
        public IActionResult Schedule()
        {
            return View("thoikhoabieune");
        }
    }
}
