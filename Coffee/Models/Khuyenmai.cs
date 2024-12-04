using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Coffee.Models;

public partial class Khuyenmai
{
    [DisplayName("#")]
    public string? PromotionId { get; set; }
    [Required(ErrorMessage = "Nhập thông tin mô tả cho chương trình khuyến mãi")]
    [DisplayName("Mô tả")]
    public string PromotionDescription { get; set; } = null!;
    [Required(ErrorMessage = "Nhập ngày kết thúc chương trình khuyến mãi")]
    [DisplayName("Ngày kết thúc")]
    [DataType(DataType.Date)]
    public DateTime Enddate { get; set; }
    [Required(ErrorMessage = "Nhập ngày bắt đầu chương trình khuyến mãi")]
    [DisplayName("Ngày bắt đầu")]
    [DataType(DataType.Date)]
    public DateTime Startdate { get; set; }
    [Required(ErrorMessage = "Nhập tên chương trình khuyến mãi")]
    [DisplayName("Tên mã khuyến mãi")]
    public string PromotionName { get; set; } = null!;
    [Required(ErrorMessage = "Nhập giá trị được giảm giá")]
    [DisplayName("Giá trị")]
    public int PromotionValue { get; set; }
    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
