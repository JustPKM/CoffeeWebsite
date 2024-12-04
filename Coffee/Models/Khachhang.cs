using System;
using System.Collections.Generic;

namespace Coffee.Models;

public partial class Khachhang
{
    public string CustomerId { get; set; } = null!;

    public string HoTen { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();

    public virtual ICollection<KhachhangSdt> KhachhangSdts { get; set; } = new List<KhachhangSdt>();
}
