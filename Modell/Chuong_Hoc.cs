namespace TracNghiemOnline.Modell
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Chuong_Hoc
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Chuong_Hoc()
        {
            Kho_CauHoi = new HashSet<Kho_CauHoi>();
            Danh_Gia = new HashSet<Danh_Gia>();
        }

        [Key]
        public long Ma_Chuong { get; set; }

        [StringLength(50)]
        public string TenChuong { get; set; }

        public long? Ma_Mon { get; set; }

        public long? TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Kho_CauHoi> Kho_CauHoi { get; set; }

        public virtual MonHoc MonHoc { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Danh_Gia> Danh_Gia { get; set; }
    }
}
