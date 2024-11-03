namespace QuanLyKyTucXa.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Newtonsoft.Json;

    [Table("Khu")]
    public partial class Khu
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Khu()
        {
            Tang = new HashSet<Tang>();
        }

        [Key]
        public int MaKhu { get; set; }

        [Required]
        [StringLength(100)]
        public string TenKhu { get; set; }

        public int? MaLoaiKhu { get; set; }

        [JsonIgnore]
        public virtual LoaiKhu LoaiKhu { get; set; }

        [JsonIgnore]
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Tang> Tang { get; set; }
    }
}
