namespace QuanLyKyTucXa.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("LoaiKhu")]
    public partial class LoaiKhu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LoaiKhu()
        {
            Khu = new HashSet<Khu>();
        }

        [Key]
        public int MaLoaiKhu { get; set; }

        [Required]
        [StringLength(100)]
        public string TenLoaiKhu { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Khu> Khu { get; set; }
    }
}
