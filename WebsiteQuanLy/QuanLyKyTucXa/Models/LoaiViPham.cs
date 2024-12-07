namespace QuanLyKyTucXa.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("LoaiViPham")]
    public partial class LoaiViPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LoaiViPham()
        {
            ViPham = new HashSet<ViPham>();
        }

        [Key]
        public int MaLoaiViPham { get; set; }

        [Required]
        [StringLength(150)]
        public string TenLoaiViPham { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ViPham> ViPham { get; set; }
    }
}
