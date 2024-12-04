using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Coffee.Models;

public partial class Danhmuc
{
    [DisplayName("#")]
    public byte CategoryId { get; set; }
    [DisplayName("Tên danh mục")]
    [StringLength(30)]
    [Required(ErrorMessage = "Tên danh mục không được để trống!")]
    public string CategoryName { get; set; } = null!;

    public virtual ICollection<Sanpham> Sanphams { get; set; } = new List<Sanpham>();
}
