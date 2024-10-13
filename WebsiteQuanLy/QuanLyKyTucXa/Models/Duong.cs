namespace QuanLyKyTucXa.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Duong")]
    public partial class Duong
    {
        [Key]
        public int MaDuong { get; set; }

        [Required]
        [StringLength(100)]
        public string TenDuong { get; set; }

        [StringLength(1)]
        public string TrangThai { get; set; }

        public int? MaPhong { get; set; }

        public virtual Phong Phong { get; set; }
    }
}
