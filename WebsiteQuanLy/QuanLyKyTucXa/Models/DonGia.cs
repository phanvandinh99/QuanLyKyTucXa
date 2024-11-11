namespace QuanLyKyTucXa.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DonGia")]
    public partial class DonGia
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public DonGia()
        {
            HoaDon = new HashSet<HoaDon>();
        }

        [Key]
        public int MaDonGia { get; set; }

        public DateTime NgayBatDau { get; set; }

        public DateTime? NgayKetThuc { get; set; }

        public double? DonVi { get; set; }

        [Column("DonGia")]
        public double? DonGia1 { get; set; }

        public int? MaLoaiHoaDon { get; set; }

        public virtual LoaiHoaDon LoaiHoaDon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDon> HoaDon { get; set; }
    }
}
