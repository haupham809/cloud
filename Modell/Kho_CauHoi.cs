namespace TracNghiemOnline.Modell
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Kho_CauHoi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kho_CauHoi()
        {
            CauHois = new HashSet<CauHoi>();
            CauHoiDeThis = new HashSet<CauHoiDeThi>();
            Dap_AN = new HashSet<Dap_AN>();
        }

        [Key]
        public long Ma_CauHoi { get; set; }

        public string NoiDung { get; set; }

        [Column(TypeName = "ntext")]
        public string HinhAnh { get; set; }

        [StringLength(20)]
        public string MucDo { get; set; }

        public long? Ma_Chuong { get; set; }

        public bool? TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CauHoi> CauHois { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CauHoiDeThi> CauHoiDeThis { get; set; }

        public virtual Chuong_Hoc Chuong_Hoc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Dap_AN> Dap_AN { get; set; }
    }
}
