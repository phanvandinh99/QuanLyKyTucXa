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

        public DateTime NgayBatDau { get; set; }

        //public DateTime NgayKetThuc { get; set; }

        public DateTime NgayDuyet { get; set; }

        public int? MaPhong { get; set; }

        [StringLength(10)]
        public string MaSinhVien { get; set; }

        public int? MaThoiHan { get; set; }

        [StringLength(10)]
        public string TaiKhoanNV { get; set; }

        public virtual NhanVien NhanVien { get; set; }

        public virtual Phong Phong { get; set; }

        public virtual SinhVien SinhVien { get; set; }

        public virtual ThoiHan ThoiHan { get; set; }
    }
}
