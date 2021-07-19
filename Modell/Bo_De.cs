namespace TracNghiemOnline.Modell
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Bo_De
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Bo_De()
        {
            BoDeOnTaps = new HashSet<BoDeOnTap>();
            CauHois = new HashSet<CauHoi>();
            De_Thi = new HashSet<De_Thi>();
            DSGV_ThucHien = new HashSet<DSGV_ThucHien>();
            Phong_Thi = new HashSet<Phong_Thi>();
        }

        [Key]
        public long Ma_BoDe { get; set; }

        [StringLength(200)]
        public string NoiDung { get; set; }

        [StringLength(10)]
        public string Ma_NguoiTao { get; set; }

        public bool? TrangThai { get; set; }

        public long? Ma_Mon { get; set; }

        public int? SoCau { get; set; }

        [StringLength(20)]
        public string ThoiGianThi { get; set; }

        public bool? Xoa { get; set; }

        [StringLength(20)]
        public string PheDuyet { get; set; }

        public DateTime? ThoiGianMo { get; set; }

        public DateTime? ThoiGianDong { get; set; }

        [StringLength(10)]
        public string NguoiDuyet { get; set; }

        public virtual GiaoVien GiaoVien { get; set; }

        public virtual MonHoc MonHoc { get; set; }

        public virtual TaiKhoan TaiKhoan { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<BoDeOnTap> BoDeOnTaps { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CauHoi> CauHois { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<De_Thi> De_Thi { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DSGV_ThucHien> DSGV_ThucHien { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Phong_Thi> Phong_Thi { get; set; }
    }
}
