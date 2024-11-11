namespace QuanLyKyTucXa.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SinhVienChinhSach")]
    public partial class SinhVienChinhSach
    {
        [Key]
        public int MaSVChinhSach { get; set; }

        [StringLength(10)]
        public string MaSinhVien { get; set; }

        public int? MaDienChinhSach { get; set; }

        public bool? TrangThai { get; set; }

        public virtual DienChinhSach DienChinhSach { get; set; }

        public virtual SinhVien SinhVien { get; set; }
    }
}
