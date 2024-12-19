using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Coffee.Models;

public partial class Nhanvien
{
    public string? EmployeeId { get; set; }
    [Required(ErrorMessage = "Họ tên là bắt buộc.")]
    public string HoTen { get; set; } = null!;
    [Required(ErrorMessage = "Mật khẩu là bắt buộc.")]
    public string Matkhau { get; set; } = null!;
    [Required(ErrorMessage = "Email là bắt buộc.")]
    [EmailAddress(ErrorMessage = "Email không hợp lệ.")]
    public string Email { get; set; } = null!;
    [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
    public string Username { get; set; } = null!;
    [Required(ErrorMessage = "Quyền là bắt buộc.")]
    public byte QuyenId { get; set; }

    public virtual ICollection<Donhang> Donhangs { get; set; } = new List<Donhang>();
    public virtual Quyen Quyen { get; set; } = null!;

    public virtual ICollection<SodtNhanvien> SodtNhanviens { get; set; } = new List<SodtNhanvien>();
}
