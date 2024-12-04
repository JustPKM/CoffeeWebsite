using Microsoft.AspNetCore.Mvc;

namespace Coffee.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
