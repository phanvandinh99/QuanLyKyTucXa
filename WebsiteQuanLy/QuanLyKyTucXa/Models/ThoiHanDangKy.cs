namespace QuanLyKyTucXa.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ThoiHanDangKy")]
    public partial class ThoiHanDangKy
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public ThoiHanDangKy()
        {
            HopDong = new HashSet<HopDong>();
        }

        [Key]
        public int MaThoiHanDangKy { get; set; }

        [Required]
        [StringLength(100)]
        public string TenThoiHanDangKy { get; set; }

        public DateTime NgayMo { get; set; }

        public DateTime NgayDong { get; set; }

        public DateTime NgayBatDau { get; set; }

        public DateTime NgayKetThuc { get; set; }

        public bool TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HopDong> HopDong { get; set; }
    }
}
