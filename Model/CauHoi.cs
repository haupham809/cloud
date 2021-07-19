using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace TracNghiemOnline.Model
{
    public class CauHoi
    {
        private static int maCH = 1;
        public CauHoi() { maCH++; }
        private string HinhAnh;
        private string NoiDubg;
        private List<DapAn> cauHois;
      
        public string HinhAnh1 { get => HinhAnh; set => HinhAnh = value; }
        public string NoiDubg1 { get => NoiDubg; set => NoiDubg = value; }
        public List<DapAn> CauHois { get => cauHois; set => cauHois = value; }
        public  int MaCH { get => maCH;  }
    }
}