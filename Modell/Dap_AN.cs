namespace TracNghiemOnline.Modell
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Dap_AN
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Dap_AN()
        {
            Da_SVLuaChon = new HashSet<Da_SVLuaChon>();
        }

        [Key]
        public long MA_DAN { get; set; }

        public string NoiDung { get; set; }

        [Column(TypeName = "ntext")]
        public string HinhAnh { get; set; }

        public long? Ma_CauHoi { get; set; }

        public bool? TrangThai { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Da_SVLuaChon> Da_SVLuaChon { get; set; }

        public virtual Kho_CauHoi Kho_CauHoi { get; set; }
    }
}
