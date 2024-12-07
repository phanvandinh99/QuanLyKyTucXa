namespace QuanLyKyTucXa.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Giuong")]
    public partial class Giuong
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Giuong()
        {
            HopDong = new HashSet<HopDong>();
        }

        [Key]
        public int MaGiuong { get; set; }

        [Required]
        [StringLength(100)]
        public string TenGiuong { get; set; }

        [StringLength(1)]
        public string TrangThai { get; set; }

        public int? MaPhong { get; set; }

        public virtual Phong Phong { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HopDong> HopDong { get; set; }
    }
}
