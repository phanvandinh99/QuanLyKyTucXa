namespace QuanLyKyTucXa.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LoaiPhong")]
    public partial class LoaiPhong
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LoaiPhong()
        {
            Phong = new HashSet<Phong>();
        }

        [Key]
        public int MaLoaiPhong { get; set; }

        [Required]
        [StringLength(100)]
        public string TenLoaiPhong { get; set; }

        public int SoGiuong { get; set; }

        [StringLength(100)]
        public string HinhAnh { get; set; }

        public double DonGia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Phong> Phong { get; set; }
    }
}
