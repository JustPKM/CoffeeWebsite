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

        public IActionResult Menu()
        {
            var product = db.Sanphams.ToList();
            return View(product);
        }
		private string GetMimeType(byte[] fileData)
		{
			if (fileData == null || fileData.Length < 4)
			{
				return "application/octet-stream";
			}

			if (fileData[0] == 0xFF && fileData[1] == 0xD8)
			{
				return "image/jpeg";
			}
			else if (fileData[0] == 0x89 && fileData[1] == 0x50 && fileData[2] == 0x4E && fileData[3] == 0x47)
			{
				return "image/png";
			}
			else if (fileData[0] == 0x47 && fileData[1] == 0x49 && fileData[2] == 0x46)
			{
				return "image/gif";
			}
			else if (fileData[0] == 0x42 && fileData[1] == 0x4D)
			{
				return "image/bmp";
			}

			return "application/octet-stream"; // Default MIME type nếu không xác định được
		}


		public IActionResult GetImage(string? id)
		{
			var product = db.Sanphams.Find(id);
			if (product == null || product.Hinhanh == null)
			{
				return NotFound();
			}

			var mimeType = GetMimeType(product.Hinhanh); // Giả sử lưu tên tệp gốc
			return File(product.Hinhanh, mimeType);
		}


		public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
