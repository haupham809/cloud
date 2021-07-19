namespace TracNghiemOnline.Modell
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GiaoVien")]
    public partial class GiaoVien
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GiaoVien()
        {
            Bo_De = new HashSet<Bo_De>();
            DSGV_ThucHien = new HashSet<DSGV_ThucHien>();
            LichNops = new HashSet<LichNop>();
            LopHocPhans = new HashSet<LopHocPhan>();
            Phong_Thi = new HashSet<Phong_Thi>();
        }

        [Key]
        [StringLength(10)]
        public string MaGV { get; set; }

        [StringLength(50)]
        public string TenGV { get; set; }

        public long? Ma_Nganh { get; set; }

        public bool? TrangThai { get; set; }

        [StringLength(10)]
        public string MaBoMon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Bo_De> Bo_De { get; set; }

        public virtual BoMon BoMon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DSGV_ThucHien> DSGV_ThucHien { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LichNop> LichNops { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LopHocPhan> LopHocPhans { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Phong_Thi> Phong_Thi { get; set; }
    }
}
