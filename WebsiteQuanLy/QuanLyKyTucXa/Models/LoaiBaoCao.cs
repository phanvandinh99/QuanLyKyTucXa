namespace QuanLyKyTucXa.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("LoaiBaoCao")]
    public partial class LoaiBaoCao
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LoaiBaoCao()
        {
            BaoCao = new HashSet<BaoCao>();
        }

        [Key]
        public int MaLoaiBaoCao { get; set; }

        [Required]
        [StringLength(150)]
        public string TenLoaiBaoCao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BaoCao> BaoCao { get; set; }
    }
}
