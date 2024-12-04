using Microsoft.AspNetCore.Mvc;

namespace Coffee.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NhanvienController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
