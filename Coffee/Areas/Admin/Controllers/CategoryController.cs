using Coffee.DataContext;
using Coffee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coffee.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        QlcfContext db = new QlcfContext();
        public IActionResult Index()
        {
            return View(db.Danhmucs.OrderByDescending(q => q.CategoryId));
        }
        public IActionResult Create() {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Danhmuc dm)
        {
            if (ModelState.IsValid)
            {
                //Nếu table không có dữ liệu, mặc định ID bắt đầu là 1.
                int nextId = db.Danhmucs.Any()
                            ? db.Danhmucs.Max(d => d.CategoryId) + 1 : 1;

                dm.CategoryId = (byte)nextId;
                db.Danhmucs.Add(dm);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            else
            {
                return View(dm);
            }
        }
        public IActionResult Edit(byte? id)
        {
            if(id == null)
            {
                return NotFound();
            }
            var dm = db.Danhmucs.Find(id);
            if (dm == null)
            {
                return NotFound();
            }
            return View(dm);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Danhmuc dm)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var d = db.Danhmucs.Find(dm.CategoryId);
                    if (d == null)
                    {
                        return BadRequest();
                    }
                    d.CategoryName = dm.CategoryName;
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
                return View(dm);
            }
        }
        public IActionResult Delete(byte? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var a = db.Sanphams.Where(s => s.ProductId == id.ToString()).ToList().Count;
            if (a <= 0)
                ViewBag.flagDelete = true;
            else
                ViewBag.flagDelete = false;
            var dm = db.Danhmucs.Find(id);
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
            var dm = db.Danhmucs.Find(id);
            if(dm != null)
            {
                db.Danhmucs.Remove(dm);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}