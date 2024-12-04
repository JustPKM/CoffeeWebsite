using System;
using System.Collections.Generic;

namespace Coffee.Models;

public partial class Nhanvien
{
    public string? EmployeeId { get; set; }

    public string HoTen { get; set; } = null!;

    public string Matkhau { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Username { get; set; } = null!;

    public byte QuyenId { get; set; }

    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();

    public virtual Quyen Quyen { get; set; } = null!;

    public virtual ICollection<SodtNhanvien> SodtNhanviens { get; set; } = new List<SodtNhanvien>();
}
