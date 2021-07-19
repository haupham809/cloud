namespace TracNghiemOnline.Modell
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KiThi")]
    public partial class KiThi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public KiThi()
        {
            LopHocPhans = new HashSet<LopHocPhan>();
        }

        [Key]
        public long MAKI { get; set; }

        [StringLength(100)]
        public string TenKi { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TGBatDau { get; set; }

        [Column(TypeName = "date")]
        public DateTime? TGKetThuc { get; set; }

        public bool? TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LopHocPhan> LopHocPhans { get; set; }
    }
}
