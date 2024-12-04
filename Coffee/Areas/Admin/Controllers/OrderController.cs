using Microsoft.AspNetCore.Mvc;
using Coffee.DataContext;
using Microsoft.EntityFrameworkCore;
using Coffee.Models;
namespace Coffee.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        QlcfContext db = new QlcfContext();
        public IActionResult Index()
        {
            return View(db.Donhangs.Include(d => d.Customer).Include(d => d.Employee));
        }
        [HttpPost]
        public IActionResult VerifyOrder(string? orderId, string action)
        {
            var order = db.Donhangs.Find(orderId);
            if (order == null) return NotFound();

            if (action == "Verify")
            {
                // Assume you retrieve the current logged-in employee's ID
                var currentEmployeeId = User.FindFirst("EmployeeId")?.Value; // Example for claims-based authentication
                if (string.IsNullOrEmpty(currentEmployeeId)) return Unauthorized();

                order.Trangthai = "Verified";
                order.EmployeeId = currentEmployeeId; // Save the employee ID
            }
            else if (action == "Reject")
            {
                order.Trangthai = "Rejected";
                order.EmployeeId = null; // No employee assigned if rejected
            }

            db.SaveChanges();
            return RedirectToAction("PendingOrders");
        }

    }
}
