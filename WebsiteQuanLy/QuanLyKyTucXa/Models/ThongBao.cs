namespace QuanLyKyTucXa.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ThongBao")]
    public partial class ThongBao
    {
        [Key]
        public int MaThongBao { get; set; }

        [Required]
        public string NoiDung { get; set; }

        public DateTime NgayGui { get; set; }

        public DateTime? NgayXem { get; set; }
 
        public Boolean TrangThai { get; set; }

        public int? MaLoaiThongBao { get; set; }

        [StringLength(10)]
        public string MaSinhVien { get; set; }

        [StringLength(10)]
        public string TaiKhoanNV { get; set; }

        public virtual LoaiThongBao LoaiThongBao { get; set; }

        public virtual NhanVien NhanVien { get; set; }

        public virtual SinhVien SinhVien { get; set; }
    }
}
