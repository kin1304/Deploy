using Microsoft.AspNetCore.Mvc;

namespace mamNonTuongLaiTuoiSang.Controllers
{
    public class MainController : Controller
    {
        public ActionResult trangchu()
        {
            return View("trangchu");
        }
    }
}
