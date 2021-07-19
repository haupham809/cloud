using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace TracNghiemOnline.Model
{
    public class DapAn
    {
        private static int maDA = 1000;
        public DapAn() { maDA++; }
        private string NoiDung;
        private string HinhAnh="";
        private bool TrangThai;

     
        public string NoiDung1 { get => NoiDung; set => NoiDung = value; }
        public string HinhAnh1 { get => HinhAnh; set => HinhAnh = value; }
        public bool TrangThai1 { get => TrangThai; set => TrangThai = value; }
        public  int MaDA { get => maDA;}
    }
}