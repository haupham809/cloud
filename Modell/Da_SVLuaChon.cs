namespace TracNghiemOnline.Modell
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Da_SVLuaChon
    {
        public long? MaDeThi { get; set; }

        public long? Ma_DAN { get; set; }

        [Key]
        public long MA_CT { get; set; }

        public virtual Dap_AN Dap_AN { get; set; }

        public virtual De_Thi De_Thi { get; set; }
    }
}
