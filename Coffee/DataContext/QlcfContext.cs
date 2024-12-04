using System;
using System.Collections.Generic;
using Coffee.Models;
using Microsoft.EntityFrameworkCore;

namespace Coffee.DataContext;

public partial class QlcfContext : DbContext
{
    public QlcfContext()
    {
    }

    public QlcfContext(DbContextOptions<QlcfContext> options)
        : base(options)
    {
    }

    public virtual DbSet<ChiTietDonHang> ChiTietDonHangs { get; set; }

    public virtual DbSet<ChiTietHoaDon> ChiTietHoaDons { get; set; }

    public virtual DbSet<Danhmuc> Danhmucs { get; set; }

    public virtual DbSet<Donhang> Donhangs { get; set; }

    public virtual DbSet<Hoadon> Hoadons { get; set; }

    public virtual DbSet<Khachhang> Khachhangs { get; set; }

    public virtual DbSet<KhachhangSdt> KhachhangSdts { get; set; }

    public virtual DbSet<Khuyenmai> Khuyenmais { get; set; }

    public virtual DbSet<Nhanvien> Nhanviens { get; set; }

    public virtual DbSet<Quyen> Quyens { get; set; }

    public virtual DbSet<Sanpham> Sanphams { get; set; }

    public virtual DbSet<SodtNhanvien> SodtNhanviens { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Data Source=LAPTOP-46L7JC1T;Initial Catalog=QLCF;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ChiTietDonHang>(entity =>
        {
            entity.HasKey(e => new { e.DonhangId, e.ProductId }).HasName("PK__ChiTietD__52EA509B728EA9E2");

            entity.ToTable("ChiTietDonHang");

            entity.Property(e => e.DonhangId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.ProductId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ProductID");
            entity.Property(e => e.Dongia).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Thanhtien).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Donhang).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.DonhangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDo__Donha__52593CB8");

            entity.HasOne(d => d.Product).WithMany(p => p.ChiTietDonHangs)
                .HasForeignKey(d => d.ProductId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietDo__Produ__5165187F");
        });

        modelBuilder.Entity<ChiTietHoaDon>(entity =>
        {
            entity.HasKey(e => new { e.DonhangId, e.HoadonId }).HasName("PK__ChiTietH__F572FA5C2D86A61A");

            entity.ToTable("ChiTietHoaDon");

            entity.Property(e => e.DonhangId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.HoadonId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Ghichu).HasMaxLength(100);
            entity.Property(e => e.ThanhTien).HasColumnType("decimal(18, 0)");

            entity.HasOne(d => d.Donhang).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.DonhangId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHo__Donha__5535A963");

            entity.HasOne(d => d.Hoadon).WithMany(p => p.ChiTietHoaDons)
                .HasForeignKey(d => d.HoadonId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ChiTietHo__Hoado__5629CD9C");
        });

        modelBuilder.Entity<Danhmuc>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Danhmuc__19093A2B42A069F2");

            entity.ToTable("Danhmuc");

            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.CategoryName).HasMaxLength(30);
        });

        modelBuilder.Entity<Donhang>(entity =>
        {
            entity.HasKey(e => e.DonhangId).HasName("PK__Donhang__99AA9CF5870FCBB9");

            entity.ToTable("Donhang");

            entity.Property(e => e.DonhangId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CustomerId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CustomerID");
            entity.Property(e => e.Diachi).HasMaxLength(100);
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("EmployeeID");
            entity.Property(e => e.Ngaydat).HasColumnType("datetime");
            entity.Property(e => e.Tongtien).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.Trangthai)
                .HasMaxLength(30)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Customer).WithMany(p => p.Donhangs)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Donhang__Custome__4E88ABD4");

            entity.HasOne(d => d.Employee).WithMany(p => p.Donhangs)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Donhang__Employe__4D94879B");
        });

        modelBuilder.Entity<Hoadon>(entity =>
        {
            entity.HasKey(e => e.HoadonId).HasName("PK__Hoadon__CD866A97FF58838E");

            entity.ToTable("Hoadon");

            entity.Property(e => e.HoadonId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Ngaythanhtoan).HasColumnType("datetime");
            entity.Property(e => e.Phuongthuc)
                .HasMaxLength(10)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Thanhtien).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<Khachhang>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Khachhan__A4AE64B8B847CE1E");

            entity.ToTable("Khachhang");

            entity.Property(e => e.CustomerId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CustomerID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.HoTen).HasMaxLength(50);
        });

        modelBuilder.Entity<KhachhangSdt>(entity =>
        {
            entity.HasKey(e => new { e.Phone, e.CustomerId }).HasName("PK__Khachhan__C634D3D40D51E32D");

            entity.ToTable("KhachhangSDT");

            entity.Property(e => e.Phone)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CustomerId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("CustomerID");

            entity.HasOne(d => d.Customer).WithMany(p => p.KhachhangSdts)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Khachhang__Custo__412EB0B6");
        });

        modelBuilder.Entity<Khuyenmai>(entity =>
        {
            entity.HasKey(e => e.PromotionId).HasName("PK__Khuyenma__52C42F2F5265F220");

            entity.ToTable("Khuyenmai");

            entity.Property(e => e.PromotionId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PromotionID");
            entity.Property(e => e.Enddate).HasColumnType("datetime");
            entity.Property(e => e.PromotionDescription).HasMaxLength(100);
            entity.Property(e => e.PromotionName).HasMaxLength(50);
            entity.Property(e => e.Startdate).HasColumnType("datetime");
        });

        modelBuilder.Entity<Nhanvien>(entity =>
        {
            entity.HasKey(e => e.EmployeeId).HasName("PK__Nhanvien__7AD04FF1237D4E1A");

            entity.ToTable("Nhanvien");

            entity.Property(e => e.EmployeeId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("EmployeeID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.HoTen).HasMaxLength(50);
            entity.Property(e => e.Matkhau)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .IsUnicode(false)
                .IsFixedLength();

            entity.HasOne(d => d.Quyen).WithMany(p => p.Nhanviens)
                .HasForeignKey(d => d.QuyenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Nhanvien__QuyenI__47DBAE45");
        });

        modelBuilder.Entity<Quyen>(entity =>
        {
            entity.HasKey(e => e.QuyenId).HasName("PK__Quyen__12926E6C9EB61157");

            entity.ToTable("Quyen");

            entity.Property(e => e.TenQuyen).HasMaxLength(50);
        });

        modelBuilder.Entity<Sanpham>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__Sanpham__B40CC6ED1EA402E8");

            entity.ToTable("Sanpham");

            entity.Property(e => e.ProductId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("ProductID");
            entity.Property(e => e.CategoryId).HasColumnName("CategoryID");
            entity.Property(e => e.Hinhanh).HasColumnType("image");
            entity.Property(e => e.Price).HasColumnType("decimal(18, 0)");
            entity.Property(e => e.ProductDescription).HasMaxLength(100);
            entity.Property(e => e.ProductName).HasMaxLength(50);
            entity.Property(e => e.PromotionId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("PromotionID");

            entity.HasOne(d => d.Category).WithMany(p => p.Sanphams)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Sanpham__Categor__440B1D61");

            entity.HasOne(d => d.Promotion).WithMany(p => p.Sanphams)
                .HasForeignKey(d => d.PromotionId)
                .HasConstraintName("FK__Sanpham__Promoti__44FF419A");
        });

        modelBuilder.Entity<SodtNhanvien>(entity =>
        {
            entity.HasKey(e => new { e.SoDienThoai, e.EmployeeId }).HasName("PK__SODT_Nha__0424B3433F0100B9");

            entity.ToTable("SODT_Nhanvien");

            entity.Property(e => e.SoDienThoai)
                .HasMaxLength(11)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.EmployeeId)
                .HasMaxLength(5)
                .IsUnicode(false)
                .IsFixedLength()
                .HasColumnName("EmployeeID");

            entity.HasOne(d => d.Employee).WithMany(p => p.SodtNhanviens)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SODT_Nhan__Emplo__4AB81AF0");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
