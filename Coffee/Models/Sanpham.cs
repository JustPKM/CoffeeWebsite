using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Coffee.Models;

public partial class Sanpham
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
    public byte[]? Hinhanh { get; set; }
    [DisplayName("Mã danh mục")]
    public byte CategoryId { get; set; }
    [DisplayName("Mã khuyến mãi")]
    public string? PromotionId { get; set; }

    public virtual Danhmuc Category { get; set; } = null!;

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual Khuyenmai? Promotion { get; set; }
}
