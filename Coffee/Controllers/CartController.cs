using Coffee.DataContext;
using Coffee.Helpers;
using Coffee.Models;
using Coffee.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Coffee.Controllers
{
	public class CartController : Controller
	{
		QlcfContext db = new QlcfContext();
		const string CART_KEY = "MYCART";
		public IActionResult Index()
		{
            List<CartItem> Cart = HttpContext.Session.Get<List<CartItem>>(CART_KEY) ?? new List<CartItem>();
			CartItemViewModel cartItemVM = new ()
			{
				CartItems = Cart,
				GrandTotal = Cart.Sum(x=>x.SoLuong * x.DonGia)
			};
            return View(cartItemVM);
		}
		
		public IActionResult AddToCart(string id)
		{
			Sanpham sp = db.Sanphams.Find(id);
			List<CartItem> cart = HttpContext.Session.Get<List<CartItem>>(CART_KEY)?? new List<CartItem>();
			CartItem cartItems = cart.Where(c=>c.ProductId == id).FirstOrDefault();
			if (cartItems == null)
			{
				cart.Add(new CartItem(sp));
			}
			else
			{
				cartItems.SoLuong++;
			}
			HttpContext.Session.SetJson(CART_KEY, cart);
			TempData["success"] = "Add Item To Cart Successfully";
			return RedirectToAction("Index","/Product");

		}
		public IActionResult Increase(string id)
		{
			List<CartItem> cart = HttpContext.Session.Get<List<CartItem>>(CART_KEY) ?? new List<CartItem>();
			CartItem cartItems = cart.Where(c => c.ProductId == id).FirstOrDefault();
			if (cartItems.SoLuong > 0)
			{
				++cartItems.SoLuong;
                TempData["success"] = "Add Item To Cart Successfully";
            }
			HttpContext.Session.SetJson(CART_KEY, cart);
			
			return RedirectToAction("Index", "/Cart");
		}
		public IActionResult Decrease(string id)
		{
			List<CartItem> cart = HttpContext.Session.Get<List<CartItem>>(CART_KEY) ?? new List<CartItem>();
			CartItem cartItems = cart.Where(c => c.ProductId == id).FirstOrDefault();
			if (cartItems.SoLuong > 1)
			{
				--cartItems.SoLuong;
			}
			else
			{
				cart.RemoveAll(c => c.ProductId == id);
                TempData["delete"] = "Delete Item from Cart Successfully";
            }
			if (cart.Count == 0)
			{
				HttpContext.Session.Remove(CART_KEY);
			}
			else
			{
				HttpContext.Session.SetJson(CART_KEY, cart);
			}
			return RedirectToAction("Index", "/Cart");
		}
		public IActionResult Remove(string id)
		{
			List<CartItem> cart = HttpContext.Session.Get<List<CartItem>>(CART_KEY) ?? new List<CartItem>();
			cart.RemoveAll(c => c.ProductId == id);
            TempData["delete"] = "Delete Item from Cart Successfully";
            if (cart.Count == 0)
			{
				HttpContext.Session.Remove(CART_KEY);
			}
			else
			{
				HttpContext.Session.SetJson(CART_KEY, cart);
			}
			return RedirectToAction("Index", "/Cart");
		}
        public string GetOrCreateCustomerId(string hoTen, string email, string phone)
        {
            // Kiểm tra xem mã khách hàng có tồn tại trong cơ sở dữ liệu hay không
            var existingCustomer = db.Khachhangs
                                     .FirstOrDefault(k => k.HoTen == hoTen && k.Email == email);

            if (existingCustomer != null)
            {
                // Nếu khách hàng đã tồn tại, kiểm tra và cập nhật số điện thoại nếu cần
                var existingPhone = db.KhachhangSdts.FirstOrDefault(p => p.CustomerId == existingCustomer.CustomerId && p.Phone == phone);

                if (existingPhone == null)
                {
                    // Thêm số điện thoại mới cho khách hàng
                    KhachhangSdt newPhone = new KhachhangSdt
                    {
                        CustomerId = existingCustomer.CustomerId,
                        Phone = phone
                    };

                    db.KhachhangSdts.Add(newPhone);
                    db.SaveChanges();
                }

                // Trả về mã khách hàng
                return existingCustomer.CustomerId;
            }
            else
            {
                // Nếu khách hàng chưa tồn tại, tạo mã khách hàng mới
                string prefix = "KH";
                int nextNumber = 1;

                var existingIds = db.Khachhangs
                                    .Select(k => k.CustomerId)
                                    .Where(id => id.StartsWith(prefix))
                                    .ToList();

                if (existingIds.Any())
                {
                    nextNumber = existingIds
                                 .Select(id => int.Parse(id.Substring(prefix.Length)))
                                 .Max() + 1;
                }

                string newCustomerId = $"{prefix}{nextNumber:D3}";

                // Tạo khách hàng mới và lưu vào cơ sở dữ liệu
                Khachhang newCustomer = new Khachhang
                {
                    CustomerId = newCustomerId,
                    HoTen = hoTen,
                    Email = email
                };

                db.Khachhangs.Add(newCustomer);
                db.SaveChanges();

                // Lưu số điện thoại mới cho khách hàng
                KhachhangSdt newPhone = new KhachhangSdt
                {
                    CustomerId = newCustomerId,
                    Phone = phone
                };

                db.KhachhangSdts.Add(newPhone);
                db.SaveChanges();

                // Trả về mã khách hàng mới
                return newCustomerId;
            }
        }


        public string GenerateOrderId()
		{
            // Tạo ID đơn hàng
            string prefix = "DH";
            int nextNumber = 1;

            var existingIds = db.Donhangs
                                .Select(d => d.DonhangId)
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
        public string GenerateInvoiceId()
        {
            // Tạo ID đơn hàng
            string prefix = "IV";
            int nextNumber = 1;

            var existingIds = db.Hoadons
                                .Select(d => d.HoadonId)
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
        [HttpGet]
        public IActionResult Checkout()
        {
            var model = new CheckoutViewModel
            {
                CartItems = HttpContext.Session.Get<List<CartItem>>(CART_KEY) ?? new List<CartItem>()
            };

            return View(model);
        }
        [HttpPost]
        public IActionResult Checkout(CheckoutViewModel model)
        {
            if (ModelState.IsValid)
            {
                List<CartItem> cart = HttpContext.Session.Get<List<CartItem>>(CART_KEY) ?? new List<CartItem>();

                if (!cart.Any())
                {
                    TempData["failed"] = "Giỏ hàng của bạn đang trống!";
                    return View(model);
                }

                // Tạo đơn hàng
                Donhang dh = new Donhang
                {
                    DonhangId = GenerateOrderId(),
                    Ngaydat = DateTime.Now,
                    Trangthai = "Pending",
                    EmployeeId = "NV001",
                    CustomerId = GetOrCreateCustomerId(model.HoTen, model.Email, model.SoDienThoai),
                    Diachi = model.DiaChi,
                    Tongtien = cart.Sum(item => item.ThanhTien)
                };

                db.Donhangs.Add(dh);
                

                // Tạo hóa đơn
                Hoadon hd = new Hoadon
                {
                    HoadonId = GenerateInvoiceId(),
                    Ngaythanhtoan = DateTime.Now,
                    Thanhtien = dh.Tongtien,
                    Phuongthuc = model.PhuongThuc
                };

                db.Hoadons.Add(hd);
                

                // Lưu chi tiết đơn hàng và chi tiết hóa đơn trong một giao dịch tách biệt
                foreach (var item in cart)
                {
                    // Tạo đối tượng ChiTietDonHang cho từng mặt hàng trong giỏ hàng
                    var chiTietDonHang = new ChiTietDonHang
                    {
                        DonhangId = dh.DonhangId,
                        ProductId = item.ProductId,
                        Soluong = item.SoLuong,
                        Dongia = item.DonGia,
                        Thanhtien = item.ThanhTien
                    };

                    db.ChiTietDonHangs.Add(chiTietDonHang);
                }
                // Tạo đối tượng ChiTietHoaDon cho từng mặt hàng trong giỏ hàng
                var chiTietHoaDon = new ChiTietHoaDon
                {
                    DonhangId = dh.DonhangId,
                    HoadonId = hd.HoadonId,
                    ThanhTien = db.ChiTietDonHangs.Sum(d=>d.Thanhtien),
                    Ghichu = model.GhiChu
                };

                db.ChiTietHoaDons.Add(chiTietHoaDon);
                db.SaveChanges();

                // Xóa giỏ hàng khỏi Session sau khi đã lưu đơn hàng
                HttpContext.Session.Remove(CART_KEY);

                TempData["success"] = "Đơn hàng đã được tạo thành công!";
                return RedirectToAction("Index", "Cart");
            }

            TempData["failed"] = "Đơn hàng đã được tạo thất bại!";
            return View(model);
        }


    }
}
