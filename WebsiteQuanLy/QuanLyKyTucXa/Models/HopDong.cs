namespace QuanLyKyTucXa.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HopDong")]
    public partial class HopDong
    {
        [Key]
        public int MaHopDong { get; set; }

        [Required]
        [StringLength(100)]
        public string TenHopDong { get; set; }

        public DateTime? NgayDuyet { get; set; }

        public DateTime? NgayDenHanThanhToan { get; set; }

        public double GiaThue { get; set; }

        public int SoNgayThue { get; set; } 

        public double SoThangThue { get; set; }

        public double ThanhTien { get; set; }

        public int? MaPhong { get; set; }

        [StringLength(10)]
        public string MaSinhVien { get; set; }

        public int? MaThoiHanDangKy { get; set; }

        [StringLength(10)]
        public string TaiKhoanNV { get; set; }

        public bool TrangThai { get; set; }

        public bool ThanhToan { get; set; }

        public virtual Phong Phong { get; set; }

        public virtual SinhVien SinhVien { get; set; }

        public virtual ThoiHanDangKy ThoiHanDangKy { get; set; }
    }
}
