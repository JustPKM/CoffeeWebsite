using Coffee.DataContext;
using Coffee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coffee.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PromotionController : Controller
    {
        QlcfContext db = new QlcfContext();
        public IActionResult Index()
        {
            return View(db.Khuyenmais.OrderByDescending(k=>k.PromotionId));
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Khuyenmai km)
        {
            if (ModelState.IsValid)
            {
                if (km.Enddate < km.Startdate) {
                    ModelState.AddModelError("Startdate", "Thời gian bắt đầu khuyến mãi không được lớn hơn thời gian kết thúc");
                    return View(km);
                }
                if(km.PromotionValue < 0)
                {
                    ModelState.AddModelError("PromotionValue", "Giá trị không được dưới 0");
                    return View(km);
                }
                // Bắt đầu tạo ID
                string prefix = "KM";
                int nextNumber = 1;

                // Kiểm tra có ID nào tồn tại trong database chưa
                var existingIds = db.Khuyenmais
                                .Select(p => p.PromotionId)
                                .Where(id => id.StartsWith(prefix))
                                .ToList();

                if (existingIds.Any())
                {
                    // Tách thành phần ký tự số and tìm số ID lớn nhất để kiểm tra
                    nextNumber = existingIds
                        .Select(id => int.Parse(id.Substring(prefix.Length))) // Bỏ tiền và ép thành kiểu integer
                        .Max() + 1;
                }

                // Format ID sản phẩm, lưu thành SP + 3 số
                km.PromotionId = $"{prefix}{nextNumber:D3}";

                db.Khuyenmais.Add(km);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(km);
        }
        public IActionResult Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var km = db.Khuyenmais.Find(id);
            if (km == null)
            {
                return NotFound();
            }
            return View(km);
        }
        [HttpPost]
        public IActionResult Edit(Khuyenmai km)
        {
            if (ModelState.IsValid)
            {
                if (km.Startdate > km.Enddate)
                {
                    ModelState.AddModelError("Startdate", "Thời gian bắt đầu khuyến mãi không được lớn hơn thời gian kết thúc");
                    return View(km);
                }
                try
                {
                    var n = db.Khuyenmais.Find(km.PromotionId);
                    if (n == null)
                    {
                        return BadRequest();
                    }
                    n.PromotionName = km.PromotionName;
                    n.PromotionDescription = km.PromotionDescription; 
                    n.PromotionValue = km.PromotionValue;
                    n.Startdate = km.Startdate;
                    n.Enddate = km.Enddate;

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
                return View(km);
            }
        }
        public IActionResult Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var km = db.Khuyenmais.Find(id);
            if (km == null)
                return NotFound();
            return View(km);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult Delete_Confirmed(string? id)
        {
            var km = db.Khuyenmais.Find(id);
            if (km != null)
            {
                db.Khuyenmais.Remove(km);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
