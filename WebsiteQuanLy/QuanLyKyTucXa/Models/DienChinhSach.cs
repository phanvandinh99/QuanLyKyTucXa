namespace QuanLyKyTucXa.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DienChinhSach")]
    public partial class DienChinhSach
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DienChinhSach()
        {
            SinhVienChinhSach = new HashSet<SinhVienChinhSach>();
        }

        [Key]
        public int MaDienChinhSach { get; set; }

        [Required]
        [StringLength(255)]
        public string TenDienChinhSach { get; set; }

        public double? DiemDienChinhSach { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SinhVienChinhSach> SinhVienChinhSach { get; set; }
    }
}
