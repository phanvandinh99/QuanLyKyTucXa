namespace QuanLyKyTucXa.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("HoaDon")]
    public partial class HoaDon
    {
        [Key]
        public int MaHoaDon { get; set; }

        public int ChuSoDau { get; set; }

        public int ChuSoCuoi { get; set; }

        public int TongSoChu { get; set; }

        public double TongTien { get; set; }

        public DateTime Thang { get; set; }

        public DateTime HanCuoiThanhToan { get; set; }

        public int? MaPhong { get; set; }

        public int? MaDonGia { get; set; }

        [StringLength(10)]
        public string TaiKhoanNV { get; set; }

        public virtual DonGia DonGia { get; set; }

        public virtual NhanVien NhanVien { get; set; }

        public virtual Phong Phong { get; set; }
    }
}
