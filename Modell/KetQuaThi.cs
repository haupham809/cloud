namespace TracNghiemOnline.Modell
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("KetQuaThi")]
    public partial class KetQuaThi
    {
        public long? Ma_DeThi { get; set; }

        public double? DiemSo { get; set; }

        public int? SoCauSai { get; set; }

        public int? SoCauDung { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayThi { get; set; }

        [Key]
        public long MAKQ { get; set; }

        public virtual De_Thi De_Thi { get; set; }
    }
}
