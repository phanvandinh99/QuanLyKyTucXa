namespace QuanLyKyTucXa.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("NhanVien")]
    public partial class NhanVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public NhanVien()
        {
            HoaDon = new HashSet<HoaDon>();
            ThongBao = new HashSet<ThongBao>();
            ViPham = new HashSet<ViPham>();
        }

        [Key]
        [StringLength(10)]
        public string TaiKhoanNV { get; set; }

        [Required]
        [StringLength(50)]
        public string MatKhau { get; set; }

        [Required]
        [StringLength(255)]
        public string AnhChanDung { get; set; }

        [Required]
        [StringLength(100)]
        public string Ho { get; set; }

        [Required]
        [StringLength(50)]
        public string Ten { get; set; }

        public bool GioiTinh { get; set; }

        public DateTime NgaySinh { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [Required]
        [StringLength(12)]
        public string SDT { get; set; }

        public bool? DoiMatKhau { get; set; }

        public bool TrangThai { get; set; }

        public int? MaQuyen { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<HoaDon> HoaDon { get; set; }

        public virtual Quyen Quyen { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ThongBao> ThongBao { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ViPham> ViPham { get; set; }
    }
}
