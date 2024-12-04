using System;
using System.Collections.Generic;

namespace Coffee.Models;

public partial class ChiTietDonHang
{
    public int Soluong { get; set; }

    public decimal Dongia { get; set; }

    public decimal Thanhtien { get; set; }

    public string ProductId { get; set; } = null!;

    public string DonhangId { get; set; } = null!;

    public virtual Donhang Donhang { get; set; } = null!;

    public virtual Sanpham Product { get; set; } = null!;
}
