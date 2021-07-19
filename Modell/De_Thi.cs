namespace TracNghiemOnline.Modell
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class De_Thi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public De_Thi()
        {
            CauHoiDeThis = new HashSet<CauHoiDeThi>();
            CT_Dethi = new HashSet<CT_Dethi>();
            Da_SVLuaChon = new HashSet<Da_SVLuaChon>();
            Danh_Gia = new HashSet<Danh_Gia>();
            Danh_Gia1 = new HashSet<Danh_Gia>();
            KetQuaThis = new HashSet<KetQuaThi>();
        }

        [StringLength(10)]
        public string Ma_SV { get; set; }

        public long? Ma_BoDe { get; set; }

        public bool? TrangThai { get; set; }

        [Key]
        public long MaDeThi { get; set; }

        public int? dem { get; set; }

        public string ChiTiet { get; set; }

        public double? DiemTru { get; set; }

        public bool? xoa { get; set; }

        [StringLength(50)]
        public string CanhCao { get; set; }

        public virtual Bo_De Bo_De { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CauHoiDeThi> CauHoiDeThis { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CT_Dethi> CT_Dethi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Da_SVLuaChon> Da_SVLuaChon { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Danh_Gia> Danh_Gia { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Danh_Gia> Danh_Gia1 { get; set; }

        public virtual SinhVien SinhVien { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<KetQuaThi> KetQuaThis { get; set; }
    }
}
