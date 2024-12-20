using Coffee.DataContext;
using Coffee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;


namespace Coffee.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        QlcfContext db = new QlcfContext();
        public IActionResult Index()
        {
            var query = db.Sanphams.Include(c => c.Category)
                           .Include(p => p.Promotion)
                           .Select(p => new Sanpham { 
                               ProductId = p.ProductId, 
                               ProductName = p.ProductName, 
                               ProductDescription = p.ProductDescription, 
                               Price = p.Promotion != null ? p.Price - (p.Price * p.Promotion.PromotionValue / 100) : p.Price, 
                               CategoryId = p.CategoryId, 
                               PromotionId = p.PromotionId,
                               Category = p.Category,
                               Promotion = p.Promotion
                           });  
            return View(query);
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Create([FromForm] Models.SanphamModel sp)
        {
            if (ModelState.IsValid)
            {
                if (sp.Price < 0)
                {
                    ModelState.AddModelError("Price", "Đơn giá sản phẩm không được dưới 0.");
                }
                // Bắt đầu tạo ID
                string prefix = "SP";
                int nextNumber = 1;

                // Kiểm tra có ID nào tồn tại trong database chưa
                var existingIds = db.Sanphams
                                .Select(p => p.ProductId)
                                .Where(id => id.StartsWith(prefix))
                                .ToList();

                if (existingIds.Any())
                {
                    // Tách thành phần ký tự số and tìm số ID lớn nhất để kiểm tra
                    nextNumber = existingIds
                        .Select(id => int.Parse(id.Substring(prefix.Length)))
                        .Max() + 1;
                }

                

                var promotion = db.Khuyenmais.FirstOrDefault(k => k.PromotionId == sp.PromotionId); 
                // Tính giá sau khi giảm nếu có khuyến mãi
                var priceAfterDiscount = sp.Price; 
                if (promotion != null) { 
                    if (promotion.Enddate < DateTime.Now)
                    {
                        priceAfterDiscount = sp.Price - (sp.Price * promotion.PromotionValue / 100);
                    }
                }

                Sanpham s = new Sanpham
                {
                    // Format ID sản phẩm, lưu thành SP + 3 số
                    ProductId = $"{prefix}{nextNumber:D3}",
                    ProductName = sp.ProductName,
                    ProductDescription = sp.ProductDescription,
                    Price = priceAfterDiscount,
                    CategoryId = sp.CategoryId,
                    PromotionId = string.IsNullOrEmpty(sp.PromotionId) ? null : sp.PromotionId,
                    Category = db.Danhmucs.Find(sp.CategoryId),
                    Promotion = promotion
                };
                if (sp.Hinhanh != null && sp.Hinhanh.Length > 0)
                {
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sp.ProductId + "_" + sp.Hinhanh.FileName);
                    using (var h = System.IO.File.Create(path))
                    {
                        sp.Hinhanh.CopyTo(h);
                    }

                    // 2. Lưu trong database dưới dạng byte
                    using (var memoryStream = new MemoryStream())
                    {
                        sp.Hinhanh.CopyTo(memoryStream);
                        s.Hinhanh = memoryStream.ToArray(); // Lưu dưới dạng byte[]
                    }
                }
                else
                {
                    s.Hinhanh = null;
                }
                db.Sanphams.Add(s);
                db.SaveChanges();

                return RedirectToAction("Index");
            }
            return View(sp);
        }
        public IActionResult Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var sp = db.Sanphams.Find(id);
            if (sp == null)
            {
                return NotFound();
            }
            return View(sp);
        }
        [HttpPost]
        public IActionResult Edit([FromForm] Models.SanphamModel sp)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var n = db.Sanphams.Find(sp.ProductId);
                    if (n == null)
                    {
                        return BadRequest();
                    }


                    if (string.IsNullOrEmpty(sp.PromotionId))
                    {
                        n.Price = sp.Price;
                        n.PromotionId = null;
                    }
                    else
                    {
                        var promotion = db.Khuyenmais.FirstOrDefault(k => k.PromotionId == sp.PromotionId);
                        // Tính giá sau khi giảm nếu có khuyến mãi
                        var priceAfterDiscount = sp.Price;
                        if (promotion != null)
                        {
                            priceAfterDiscount = sp.Price - (sp.Price * promotion.PromotionValue / 100);
                            n.PromotionId = sp.PromotionId;
                        }
                    }
                    n.ProductName = sp.ProductName;
                    n.ProductDescription = sp.ProductDescription;
                    n.CategoryId = sp.CategoryId;
                    if (sp.Hinhanh != null && sp.Hinhanh.Length > 0)
                    {
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", sp.ProductId + "_" + sp.Hinhanh.FileName);

                        // Save the image to the file system
                        using (var h = System.IO.File.Create(path))
                        {
                            sp.Hinhanh.CopyTo(h);
                        }

                        // Save the image as a byte array to the database
                        using (var memoryStream = new MemoryStream())
                        {
                            sp.Hinhanh.CopyTo(memoryStream);
                            n.Hinhanh = memoryStream.ToArray(); // Save as byte[]
                        }
                    }

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
                return View(sp);
            }
        }
        public IActionResult Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            };
            var sp = db.Sanphams.Find(id);
            if (sp == null)
            {
                return NotFound();
            }
            return View(sp);
        }
        //Plan: Xóa sản phẩm nếu các đơn hàng có chứa sản phẩm ấy đã được giao
        [HttpPost, ActionName("Delete")]
        public IActionResult Delete_Confirmed(string? id)
        {
            var sp = db.Sanphams.Include(c=>c.ChiTietDonHangs).FirstOrDefault(s=>s.ProductId == id);
            
            if (sp != null)
            {
                if (sp.ChiTietDonHangs.Any())
                {
                    TempData["error"] = "Không thể xóa sản phẩm này vì còn chi tiết đơn hàng!";
                    return RedirectToAction("Index");
                }
                db.Sanphams.Remove(sp);
                db.SaveChanges();
                TempData["success"] = "Đã xóa sản phẩm thành công!";
            }
            else
            {
                TempData["error"] = "Không tìm thấy sản phẩm!";
            }
            return RedirectToAction("Index");
        }
    }
}
