using System;
using System.Collections.Generic;

namespace Coffee.Models;

public partial class ChiTietHoaDon
{
    public string? Ghichu { get; set; }

    public decimal ThanhTien { get; set; }

    public string DonhangId { get; set; } = null!;

    public string HoadonId { get; set; } = null!;

    public virtual Donhang Donhang { get; set; } = null!;

    public virtual Hoadon Hoadon { get; set; } = null!;
}
