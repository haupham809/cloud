namespace TracNghiemOnline.Modell
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Phong_Thi
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Phong_Thi()
        {
            DS_SVThi = new HashSet<DS_SVThi>();
        }

        [Key]
        [StringLength(10)]
        public string MaPhong { get; set; }

        [StringLength(10)]
        public string NguoiTao { get; set; }

        public DateTime? ThoiGianMo { get; set; }

        public DateTime? ThoiGianDong { get; set; }

        [StringLength(20)]
        public string TrangThai { get; set; }

        public long? MaBoDe { get; set; }

        [Required]
        [StringLength(10)]
        public string MaLopHP { get; set; }

        public bool? Xoa { get; set; }

        [StringLength(100)]
        public string TenPhong { get; set; }

        [StringLength(10)]
        public string MaCanBo1 { get; set; }

        [StringLength(10)]
        public string MaCanBo2 { get; set; }

        public virtual Bo_De Bo_De { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DS_SVThi> DS_SVThi { get; set; }

        public virtual GiaoVien GiaoVien { get; set; }

        public virtual LopHocPhan LopHocPhan { get; set; }
    }
}
