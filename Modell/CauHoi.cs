namespace TracNghiemOnline.Modell
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CauHoi")]
    public partial class CauHoi
    {
        public long Ma_BoDe { get; set; }

        public long Ma_CauHoi { get; set; }

        [Key]
        public long MaCT { get; set; }

        public bool? trangThai { get; set; }

        public virtual Bo_De Bo_De { get; set; }

        public virtual Kho_CauHoi Kho_CauHoi { get; set; }
    }
}
