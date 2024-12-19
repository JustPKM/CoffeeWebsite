using Coffee.DataContext;
using Coffee.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
namespace Coffee.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeAdminController : Controller
    {
		QlcfContext db = new QlcfContext();
		public IActionResult Index()
		{
			return View();
		}
		[HttpGet]
		public IActionResult Login()
		{
            ViewBag.loginCheck = null;
            return View();
		}
		[HttpPost]
		public IActionResult Login(Nhanvien nv)
		{
			ViewBag.loginCheck = false;
            var nvDB = db.Nhanviens.FirstOrDefault(n => n.Username.Trim() == nv.Username.Trim() && n.Matkhau.Trim() == nv.Matkhau.Trim());
            if (nvDB != null)
			{
				if(nvDB.QuyenId == 1)
				{
                    HttpContext.Session.SetString("AdminCheck", "true");
                }
				else
				{
                    HttpContext.Session.SetString("AdminCheck", "false");
                }
				string json = JsonConvert.SerializeObject(nvDB);
				HttpContext.Session.SetString("Nhanvien", json);
				ViewBag.loginCheck = true;
				return RedirectToAction("Index", "HomeAdmin");
				
			}

			return View(nv);
		}
		public IActionResult Logout()
		{
            ViewBag.loginCheck = null;
			var session = HttpContext.Session;
			session.Remove("Nhanvien");
			session.Remove("AdminCheck");
            return RedirectToAction("Login","HomeAdmin");
		}
	}
}
