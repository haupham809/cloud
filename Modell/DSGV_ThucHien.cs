namespace TracNghiemOnline.Modell
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class DSGV_ThucHien
    {
        public long id { get; set; }

        public long? MaLich { get; set; }

        [StringLength(10)]
        public string MaGV { get; set; }

        [StringLength(200)]
        public string LyDo { get; set; }

        [StringLength(15)]
        public string trangthai { get; set; }

        public DateTime? NgayNop { get; set; }

        public long? MaDE { get; set; }

        public virtual Bo_De Bo_De { get; set; }

        public virtual GiaoVien GiaoVien { get; set; }

        public virtual LichNop LichNop { get; set; }
    }
}
