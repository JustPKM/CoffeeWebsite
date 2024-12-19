using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Coffee.Models;

public partial class ChiTietHoaDon
{
    [StringLength(100, ErrorMessage = "Ghi chú không được quá 100 ký tự!")]
    public string? Ghichu { get; set; }

    public decimal ThanhTien { get; set; }

    public string DonhangId { get; set; } = null!;

    public string HoadonId { get; set; } = null!;

    public virtual Donhang Donhang { get; set; } = null!;

    public virtual Hoadon Hoadon { get; set; } = null!;
}
