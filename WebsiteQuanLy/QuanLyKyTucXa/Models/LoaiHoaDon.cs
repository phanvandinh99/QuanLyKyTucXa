namespace QuanLyKyTucXa.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LoaiHoaDon")]
    public partial class LoaiHoaDon
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LoaiHoaDon()
        {
            DonGia = new HashSet<DonGia>();
        }

        [Key]
        public int MaLoaiHoaDon { get; set; }

        [Required]
        [StringLength(100)]
        public string TenLoaiHoaDon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DonGia> DonGia { get; set; }
    }
}
