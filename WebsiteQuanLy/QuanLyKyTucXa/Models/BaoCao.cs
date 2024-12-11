namespace QuanLyKyTucXa.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("BaoCao")]
    public partial class BaoCao
    {
        [Key]
        public int MaBaoCao { get; set; }

        [Required]
        public string NoiDung { get; set; }

        public DateTime NgayGui { get; set; }

        public DateTime? NgayXem { get; set; }

        public int? MaLoaiBaoCao { get; set; }

        [StringLength(10)]
        public string MaSinhVien { get; set; }

        [StringLength(10)]
        public string TaiKhoanNV { get; set; }

        public virtual LoaiBaoCao LoaiBaoCao { get; set; }

        public virtual NhanVien NhanVien { get; set; }

        public virtual SinhVien SinhVien { get; set; }
    }
}
