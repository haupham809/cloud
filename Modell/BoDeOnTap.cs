namespace TracNghiemOnline.Modell
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("BoDeOnTap")]
    public partial class BoDeOnTap
    {
        public long? MaBoDe { get; set; }

        [StringLength(10)]
        public string MaLopHP { get; set; }

        public DateTime? ThoiGianMo { get; set; }

        public DateTime? ThoiGianDong { get; set; }

        public long id { get; set; }

        public virtual Bo_De Bo_De { get; set; }

        public virtual LopHocPhan LopHocPhan { get; set; }
    }
}
