using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TracNghiemOnline.Modell.Dao
{
    public class LoginDao
    {
        TracNghiemOnlineDB db = new TracNghiemOnlineDB();
        internal TaiKhoan Login(TaiKhoan taiKhoan)
        {
            return db.TaiKhoans.SingleOrDefault(x => x.TenDangNhap.Equals(taiKhoan.TenDangNhap) && x.MatKhau.Equals(taiKhoan.MatKhau));
        }
    }
}