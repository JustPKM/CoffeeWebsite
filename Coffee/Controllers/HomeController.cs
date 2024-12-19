using Coffee.DataContext;
using Coffee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace Coffee.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        QlcfContext db = new QlcfContext();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
		public IActionResult About()
		{
			return View();
		}
		public IActionResult Contact()
		{
			return View();
		}

		
    }
}
