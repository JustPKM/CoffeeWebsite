using Microsoft.AspNetCore.Mvc;
using Coffee.Models;
using Coffee.DataContext;
using Microsoft.EntityFrameworkCore;
namespace CoffeeManagement.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RolesController : Controller
    {
        QlcfContext db = new QlcfContext();
        public IActionResult Index()
        {
            return View(db.Quyens);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Quyen quyen)
        {
            // Kiểm tra tên quyền có bị trùng hay không
            if (db.Quyens.Any(q => q.TenQuyen == quyen.TenQuyen))
            {
                ModelState.AddModelError("TenQuyen", "Tên quyền đã tồn tại.");
                return View(quyen);
            }
            if (ModelState.IsValid)
            {
                //Nếu table không có dữ liệu, mặc định ID bắt đầu là 1.
                int nextId = db.Quyens.Any()
                            ? db.Quyens.Max(d => d.QuyenId) + 1 : 1;

                quyen.QuyenId = (byte)nextId;
                db.Quyens.Add(quyen);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(quyen);
            }
        }
        public IActionResult Edit(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var quyen = db.Quyens.Find(id);
            if (quyen == null)
            {
                return NotFound();
            }
            return View(quyen);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Quyen quyen)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var q = db.Quyens.Find(quyen.QuyenId);
                    if (q == null)
                    {
                        return BadRequest();
                    }
                    // Kiểm tra tên quyền có bị trùng hay không
                    if (db.Quyens.Any(q => q.TenQuyen == quyen.TenQuyen))
                    {
                        ModelState.AddModelError("TenQuyen", "Tên quyền đã tồn tại.");
                        return View(quyen);
                    }
                    q.TenQuyen = quyen.TenQuyen;
                    db.SaveChanges();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }
            }
            else
            {
                return View(quyen);
            }
        }
        public IActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var a = db.Nhanviens.Where(n => n.QuyenId == id).ToList().Count;
            if (a <= 0)
                ViewBag.flagDelete = true;
            else
                ViewBag.flagDelete = false;
            var dm = db.Quyens.Find(id);
            if (dm == null)
            {
                return NotFound();
            }
            return View(dm);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult Delete_Confirmed(byte? id)
        {
            var q = db.Quyens.Find(id);
            if (q != null)
            {
                db.Quyens.Remove(q);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
