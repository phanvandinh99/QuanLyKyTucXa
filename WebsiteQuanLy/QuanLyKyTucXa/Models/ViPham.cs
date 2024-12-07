namespace QuanLyKyTucXa.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ViPham")]
    public partial class ViPham
    {
        [Key]
        public int MaViPham { get; set; }

        [Required]
        public string NoiDung { get; set; }

        public DateTime NgayGui { get; set; }

        public DateTime? NgayXem { get; set; }

        public int? MaLoaiViPham { get; set; }

        [StringLength(10)]
        public string MaSinhVien { get; set; }

        [StringLength(10)]
        public string TaiKhoanNV { get; set; }

        public virtual LoaiViPham LoaiViPham { get; set; }

        public virtual NhanVien NhanVien { get; set; }

        public virtual SinhVien SinhVien { get; set; }
    }
}
