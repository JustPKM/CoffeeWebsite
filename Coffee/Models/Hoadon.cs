using System;
using System.Collections.Generic;

namespace Coffee.Models;

public partial class Hoadon
{
    public string HoadonId { get; set; } = null!;

    public DateTime Ngaythanhtoan { get; set; }

    public decimal Thanhtien { get; set; }

    public string Phuongthuc { get; set; } = null!;

    public virtual ICollection<ChiTietHoaDon> ChiTietHoaDons { get; set; } = new List<ChiTietHoaDon>();
}
