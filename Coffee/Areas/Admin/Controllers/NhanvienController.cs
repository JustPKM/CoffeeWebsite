using Microsoft.AspNetCore.Mvc;
using Coffee.DataContext;
using Microsoft.EntityFrameworkCore;
using Coffee.Models;
using Coffee.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;
namespace Coffee.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class NhanvienController : Controller
    {
        QlcfContext db = new QlcfContext();
        public IActionResult Index()
        {
            return View(db.Nhanviens.Include(q=>q.Quyen));
        }
        public IActionResult Create()
        {
            ViewBag.QuyenList = new SelectList(db.Quyens.ToList(), "QuyenId", "TenQuyen");
            return View();
        }
        public string GenerateEmployeeId()
        {
            // Tạo ID đơn hàng
            string prefix = "NV";
            int nextNumber = 1;

            var existingIds = db.Nhanviens
                                .Select(d => d.EmployeeId)
                                .Where(id => id.StartsWith(prefix))
                                .ToList();

            if (existingIds.Any())
            {
                nextNumber = existingIds
                             .Select(id => int.Parse(id.Substring(prefix.Length)))
                             .Max() + 1;
            }
            return $"{prefix}{nextNumber:D3}";
        }
        [HttpPost]
        public IActionResult Create(NhanVienModel model)
        {
            var quyen = db.Quyens.FirstOrDefault(q => q.QuyenId == model.Nhanvien.QuyenId);
            if (quyen != null)
            {
                model.Nhanvien.Quyen = quyen;
            }
            else
            {
                ModelState.AddModelError("Nhanvien.Quyen", "Quyen is required."); 
                TempData["error"] = "Tạo nhân viên thất bại! Không tìm thấy quyền."; 
                return View(model);
            }

            if (ModelState.IsValid)
            {
                model.Nhanvien.EmployeeId = GenerateEmployeeId();
                
                db.Nhanviens.Add(model.Nhanvien);

                var sdtNV = new SodtNhanvien
                {
                    EmployeeId = model.Nhanvien.EmployeeId,
                    SoDienThoai = model.SoDienThoai
                };
                db.SodtNhanviens.Add(sdtNV);
                db.SaveChanges();
                TempData["success"] = "Tạo nhân viên thành công!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Tạo nhân viên thất bại!";
                return View(model);
            }
        }
        public IActionResult Delete(string id)
        {
            var nv = db.Nhanviens.Find(id);
            if (nv == null)
            {
                return NotFound();
            }
            return View(nv);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult Delete_Confirmed(string id)
        {
            var nv = db.Nhanviens.Find(id);
            if(nv != null)
            {
                var sdtNV = db.SodtNhanviens.Where(s=>s.EmployeeId==nv.EmployeeId).ToList();
                if (sdtNV.Any())
                {
                    db.SodtNhanviens.RemoveRange(sdtNV);
                    db.Nhanviens.Remove(nv);
                }
                db.SaveChanges();
                TempData["success"] = "Xóa nhân viên thành công";
            }
            return RedirectToAction("Index");
        }
    }
}
