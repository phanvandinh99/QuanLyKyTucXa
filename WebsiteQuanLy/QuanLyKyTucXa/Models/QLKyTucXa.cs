using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace QuanLyKyTucXa.Models
{
    public partial class QLKyTucXa : DbContext
    {
        public QLKyTucXa()
            : base("name=QLKyTucXa")
        {
        }

        public virtual DbSet<DichVu> DichVu { get; set; }
        public virtual DbSet<DichVuPhong> DichVuPhong { get; set; }
        public virtual DbSet<DienChinhSach> DienChinhSach { get; set; }
        public virtual DbSet<DonGia> DonGia { get; set; }
        public virtual DbSet<Giuong> Giuong { get; set; }
        public virtual DbSet<HoaDon> HoaDon { get; set; }
        public virtual DbSet<HopDong> HopDong { get; set; }
        public virtual DbSet<Khu> Khu { get; set; }
        public virtual DbSet<LoaiHoaDon> LoaiHoaDon { get; set; }
        public virtual DbSet<LoaiKhu> LoaiKhu { get; set; }
        public virtual DbSet<LoaiPhong> LoaiPhong { get; set; }
        public virtual DbSet<NhanVien> NhanVien { get; set; }
        public virtual DbSet<Phong> Phong { get; set; }
        public virtual DbSet<Quyen> Quyen { get; set; }
        public virtual DbSet<SinhVien> SinhVien { get; set; }
        public virtual DbSet<SinhVienChinhSach> SinhVienChinhSach { get; set; }
        public virtual DbSet<Tang> Tang { get; set; }
        public virtual DbSet<ThoiHan> ThoiHan { get; set; }
        public virtual DbSet<TrangThai> TrangThai { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Giuong>()
                .Property(e => e.TrangThai)
                .IsFixedLength()
                .IsUnicode(false);

            //modelBuilder.Entity<HoaDon>()
            //    .Property(e => e.TaiKhoanNV)
            //    .IsUnicode(false);

            modelBuilder.Entity<HopDong>()
                .Property(e => e.MaSinhVien)
                .IsUnicode(false);

            modelBuilder.Entity<HopDong>()
                .Property(e => e.TaiKhoanNV)
                .IsUnicode(false);

            modelBuilder.Entity<NhanVien>()
                .Property(e => e.TaiKhoanNV)
                .IsUnicode(false);

            modelBuilder.Entity<NhanVien>()
                .Property(e => e.MatKhau)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.MaSinhVien)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.MatKhau)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.DanToc)
                .IsUnicode(false);

            modelBuilder.Entity<SinhVienChinhSach>()
                .Property(e => e.MaSinhVien)
                .IsUnicode(false);
        }
    }
}
