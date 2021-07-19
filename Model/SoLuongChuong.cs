using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TracNghiemOnline.Modell;

namespace TracNghiemOnline.Model
{
    [Serializable]
    public class SoLuongChuong
    {
       public Chuong_Hoc Chuong { get; set; }
       public string vandung { get; set; }
        public string thongHieu { get; set; }
        public string nhanBiet { get; set; }
        public string vandungcao { get; set; }
        public string TongSoCau { get; set; }

    }
}