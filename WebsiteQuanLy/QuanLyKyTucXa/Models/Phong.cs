namespace QuanLyKyTucXa.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Phong")]
    public partial class Phong
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Phong()
        {
            Duong = new HashSet<Duong>();
            HoaDon = new HashSet<HoaDon>();
            HopDong = new HashSet<HopDong>();
        }

        [Key]
        public int MaPhong { get; set; }

        [Required]
        [StringLength(100)]
        public string TenPhong { get; set; }

        public int? DaO { get; set; }

        public bool? TrangThai { get; set; }

        public int? MaTang { get; set; }

        public int? MaLoaiPhong { get; set; }

        public int? MaTrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Duong> Duong { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDon> HoaDon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HopDong> HopDong { get; set; }

        public virtual LoaiPhong LoaiPhong { get; set; }

        public virtual Tang Tang { get; set; }

        public virtual TrangThai TrangThai1 { get; set; }
    }
}
