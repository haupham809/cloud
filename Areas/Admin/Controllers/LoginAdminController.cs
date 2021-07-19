using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TracNghiemOnline.Modell;

namespace TracNghiemOnline.Areas.Admin.Controllers
{
    public class LoginAdminController : Controller
    {
        // GET: Admin/Login
        public ActionResult Login()
        {
            Session[ComMon.ComMonStants.UserLogin] = null;
            return View();
        }
        
        public ActionResult DangNhap(TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                var TK = new Modell.Dao.LoginDao().Login(taiKhoan);
                if (TK != null)
                {
                    Session.Add(ComMon.ComMonStants.UserLogin, TK);
                    if (TK.ChưcVu.Equals("Cán Bộ"))
                    {
                        return RedirectToAction("DSDETHI", "Home");

                    }
                    else if (TK.ChưcVu.Equals("Admin"))
                    {
                        return RedirectToAction("DSDETHI", "Admin");

                    }
                    else if (TK.ChưcVu.Equals("BoMon")) return RedirectToAction("DSDethi", "Bomon");


                    return Redirect("/LopHocPhan");
                }
                else
                {
                    ModelState.AddModelError("", "Đăng Nhập Không Đúng ");
                }

            }
            return View("Login");
        }
    }
}