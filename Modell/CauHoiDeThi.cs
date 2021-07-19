namespace TracNghiemOnline.Modell
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CauHoiDeThi")]
    public partial class CauHoiDeThi
    {
        public long? MaDeThi { get; set; }

        public long? MaCauHoi { get; set; }

        [Key]
        public long MaCT { get; set; }

        public virtual De_Thi De_Thi { get; set; }

        public virtual Kho_CauHoi Kho_CauHoi { get; set; }
    }
}
