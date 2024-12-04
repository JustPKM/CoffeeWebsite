using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Coffee.Models
{
    public class SanphamModel
    {
        [Required(ErrorMessage = "Nhập tên sản phẩm")]
        [DisplayName("Tên sản phẩm")]
        public string ProductName { get; set; } = null!;
        [DisplayName("#")]
        public string? ProductId { get; set; }
        [DisplayName("Đơn giá")]
        [Required(ErrorMessage = "Nhập đơn giá cho sản phẩm")]
        public decimal Price { get; set; }
        [DisplayName("Mô tả")]
        public string? ProductDescription { get; set; }
        [DisplayName("Hình")]
        public IFormFile? Hinhanh { get; set; }
        [DisplayName("Mã danh mục")]
        public byte CategoryId { get; set; }
        [DisplayName("Mã khuyến mãi")]
        public string? PromotionId { get; set; }
    }
}
