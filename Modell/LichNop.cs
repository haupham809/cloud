namespace TracNghiemOnline.Modell
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("LichNop")]
    public partial class LichNop
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public LichNop()
        {
            DSGV_ThucHien = new HashSet<DSGV_ThucHien>();
        }

        public long id { get; set; }

        [StringLength(50)]
        public string NoiDung { get; set; }

        public DateTime? ThoiGian { get; set; }

        public bool? TrangThai { get; set; }

        public bool? xoa { get; set; }

        [StringLength(10)]
        public string MaBoMON { get; set; }

        public long? MaMon { get; set; }

        public virtual GiaoVien GiaoVien { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DSGV_ThucHien> DSGV_ThucHien { get; set; }

        public virtual MonHoc MonHoc { get; set; }
    }
}
