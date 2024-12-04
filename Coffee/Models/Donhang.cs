using System;
using System.Collections.Generic;

namespace Coffee.Models;

public partial class Donhang
{
    public DateTime Ngaydat { get; set; }

    public string? DonhangId { get; set; }

    public string Trangthai { get; set; } = null!;

    public string? Diachi { get; set; }

    public decimal Tongtien { get; set; }

    public string EmployeeId { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; } = new List<ChiTietDonHang>();

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();

    public virtual Khachhang Customer { get; set; } = null!;

    public virtual Nhanvien Employee { get; set; } = null!;
}
