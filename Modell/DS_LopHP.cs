namespace TracNghiemOnline.Modell
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DS_LopHP
    {
        [StringLength(10)]
        public string Ma_LOP { get; set; }

        [StringLength(10)]
        public string MA_SV { get; set; }

        public long ID { get; set; }

        public bool? TrangThai { get; set; }

        public virtual LopHocPhan LopHocPhan { get; set; }

        public virtual SinhVien SinhVien { get; set; }
    }
}
