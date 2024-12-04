using System;
using System.Collections.Generic;

namespace Coffee.Models;

public partial class SodtNhanvien
{
    public string SoDienThoai { get; set; } = null!;

    public string EmployeeId { get; set; } = null!;

    public virtual Nhanvien Employee { get; set; } = null!;
}
