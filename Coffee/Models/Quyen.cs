using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace Coffee.Models;

public partial class Quyen
{
    [DisplayName("#")]
    public byte QuyenId { get; set; }
    [DisplayName("Tên quyền")]
    public string TenQuyen { get; set; } = null!;

    public virtual ICollection<Nhanvien> Nhanviens { get; set; } = new List<Nhanvien>();
}
