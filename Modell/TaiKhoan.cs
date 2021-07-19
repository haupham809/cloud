namespace TracNghiemOnline.Modell
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaiKhoan")]
    public partial class TaiKhoan
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TaiKhoan()
        {
            Bo_De = new HashSet<Bo_De>();
        }

        [Key]
        [Column("TaiKhoan")]
        [StringLength(10)]
        public string TaiKhoan1 { get; set; }

        [StringLength(10)]
        public string MatKhau { get; set; }

        [StringLength(50)]
        public string ChưcVu { get; set; }

        public bool? TrangThai { get; set; }

        [StringLength(50)]
        public string TenDangNhap { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bo_De> Bo_De { get; set; }
    }
}
