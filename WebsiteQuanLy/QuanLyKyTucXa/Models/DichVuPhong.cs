namespace QuanLyKyTucXa.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("DichVuPhong")]
    public partial class DichVuPhong
    {
        [Key]
        public int MaDichVuPhong { get; set; }

        public bool? Xoa { get; set; }

        public DateTime? NgayThem { get; set; }

        public DateTime? NgayXoa { get; set; }

        public int? MaPhong { get; set; }

        public int? MaDichVu { get; set; }

        public virtual DichVu DichVu { get; set; }

        public virtual Phong Phong { get; set; }
    }
}
