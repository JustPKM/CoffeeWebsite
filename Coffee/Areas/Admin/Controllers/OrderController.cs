using Microsoft.AspNetCore.Mvc;
using Coffee.DataContext;
using Microsoft.EntityFrameworkCore;
using Coffee.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Coffee.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        QlcfContext db = new QlcfContext();
        public IActionResult Index() { 
            ViewBag.TrangthaiList = new List<string> { "Pending", "Verified", "Reject", "Delivering", "Completed" }; 
            ViewBag.EmployeeList = new SelectList(db.Nhanviens.ToList(), "EmployeeId", "HoTen"); 
            return View(TrimDonhangList(db.Donhangs.Include(d => d.Customer)
                .Include(d => d.Employee)
                .OrderByDescending(h => h.DonhangId)
                .ToList())); 
        }
        public IActionResult PendingOrders() {
            ViewBag.TrangthaiList = new List<string> { "Pending", "Verified", "Reject", "Delivering", "Completed" };
            ViewBag.EmployeeList = new SelectList(db.Nhanviens.ToList(), "EmployeeId", "HoTen");
            var pendingOrders = db.Donhangs.Include(d => d.Customer)
                .Include(d => d.Employee)
                .Where(d => d.Trangthai.TrimEnd() == "Pending")
                .OrderByDescending(h => h.DonhangId)
                .ToList(); 
            return View(TrimDonhangList(pendingOrders)); 
        }
        public IActionResult VerifiedOrders() {
            ViewBag.TrangthaiList = new List<string> { "Verified", "Reject", "Delivering", "Completed" };
            ViewBag.EmployeeList = new SelectList(db.Nhanviens.ToList(), "EmployeeId", "HoTen");
            var verifiedOrRejectedOrders = db.Donhangs.Include(d => d.Customer)
                .Include(d => d.Employee)
                .Where(d => d.Trangthai.TrimEnd() != "Pending" && d.Trangthai.TrimEnd() != "Completed")
                .OrderByDescending(h => h.DonhangId)
                .ToList(); 
            return View(TrimDonhangList(verifiedOrRejectedOrders)); 
        }
        private List<Donhang> TrimDonhangList(List<Donhang> donhangList)
        {
            foreach (var donhang in donhangList)
            {
                donhang.Trangthai = donhang.Trangthai?.Trim(); 
                donhang.Customer.HoTen = donhang.Customer.HoTen?.Trim(); 
                donhang.Diachi = donhang.Diachi?.Trim(); 
                donhang.Employee.HoTen = donhang.Employee.HoTen?.Trim(); 
            } 
            return donhangList; 
        }
        [HttpPost] 
        public IActionResult UpdateStatus(string DonhangId, string Trangthai) 
        { 
            var donhang = db.Donhangs.Find(DonhangId); 
            if (donhang != null) { 
                if (!string.IsNullOrEmpty(Trangthai)) { 
                    donhang.Trangthai = Trangthai;
                    db.SaveChanges(); 
                    TempData["success"] = "Cập nhật trạng thái đơn hàng thành công!"; 
                } 
            } 
            else { 
                TempData["error"] = "Không tìm thấy đơn hàng!"; 
            } 
            return RedirectToAction("Index"); 
        }
        [HttpPost]
        public IActionResult UpdateEmployee(string DonhangId, string EmployeeId)
        {
            var donhang = db.Donhangs.Find(DonhangId);
            if (donhang != null)
            {
                if (!string.IsNullOrEmpty(EmployeeId))
                {
                    donhang.EmployeeId = EmployeeId;
                    db.SaveChanges();
                    TempData["success"] = "Cập nhật nhân viên đơn hàng thành công!";
                }
            }
            else
            {
                TempData["error"] = "Không tìm thấy đơn hàng!";
            }
            return RedirectToAction("Index");
        }
        public IActionResult Details(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var order = db.Donhangs.Include(o => o.Customer)
                .Include(o => o.Employee)
                .Include(o => o.ChiTietDonHangs)
                .ThenInclude(d => d.Product)
                .FirstOrDefault(m => m.DonhangId == id);

            if(order == null)
            {
                return NotFound();
            }
            return View(order);
        }
        public IActionResult Delete(string id)
        {
            var dh = db.Donhangs.Find(id);
            if(dh == null)
            {
                return NotFound();
            }
            return View(dh);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult Delete_Confirmed(string id)
        {
            var dh = db.Donhangs.Include(d => d.ChiTietDonHangs).Include(d=>d.ChiTietHoaDons).FirstOrDefault(d => d.DonhangId == id);
            if (dh != null)
            {
                if (dh.Trangthai.TrimEnd() == "Pending" || dh.Trangthai.TrimEnd() == "Completed")
                {
                    foreach (var chiTiet in dh.ChiTietDonHangs) { 
                        db.ChiTietDonHangs.Remove(chiTiet); 
                    }
                    foreach (var chiTiet in dh.ChiTietHoaDons)
                    {
                        db.ChiTietHoaDons.Remove(chiTiet);
                    }
                    db.Donhangs.Remove(dh);
                    db.SaveChanges();
                    TempData["success"] = "Xóa đơn hàng thành công!";
                }
                else
                {
                    TempData["error"] = "Xóa đơn hàng thất bại!";
                    return RedirectToAction("Index");
                }
            }
            else
            {
                TempData["error"] = "Xóa đơn hàng thất bại!";
            }
            return RedirectToAction("Index");
        }
    }
}
