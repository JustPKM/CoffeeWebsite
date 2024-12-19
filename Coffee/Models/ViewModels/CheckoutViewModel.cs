using System.ComponentModel.DataAnnotations;

namespace Coffee.Models.ViewModels
{
    public class CheckoutViewModel
    {
        [Required(ErrorMessage = "Vui lòng nhập họ tên!")]
        public string HoTen { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập địa chỉ!")]
        public string DiaChi { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập Email!")]
        [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Vui lòng nhập số điện thoại!")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string SoDienThoai { get; set; }
        public string? GhiChu { get; set; }
        [Required(ErrorMessage = "Vui lòng chọn phương thức thanh toán!")]
        public string PhuongThuc { get; set; }
        public List<CartItem>? CartItems { get; set; }
    }
}
