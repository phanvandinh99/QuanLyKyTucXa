namespace QuanLyKyTucXa.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Giuong")]
    public partial class Giuong
    {
        [Key]
        public int MaGiuong { get; set; }

        [Required]
        [StringLength(100)]
        public string TenGiuong { get; set; }

        [StringLength(1)]
        public string TrangThai { get; set; }

        public int? MaPhong { get; set; }

        public virtual Phong Phong { get; set; }
    }
}
