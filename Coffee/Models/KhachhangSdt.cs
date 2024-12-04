using System;
using System.Collections.Generic;

namespace Coffee.Models;

public partial class KhachhangSdt
{
    public string Phone { get; set; } = null!;

    public string CustomerId { get; set; } = null!;

    public virtual Khachhang Customer { get; set; } = null!;
}
