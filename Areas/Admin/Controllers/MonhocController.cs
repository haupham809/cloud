using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TracNghiemOnline.Modell;
using TracNghiemOnline.Modell.Dao;

namespace TracNghiemOnline.Areas.Admin.Controllers
{
    public class MonhocController : BaseController
    {
        // GET: Admin/Monhoc
        TracNghiemOnlineDB db = new TracNghiemOnlineDB();
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public JsonResult DeleteMon(long maNganh)
        {
            try
            {
                var n = (from d in db.MonHocs where d.Ma_Mon == maNganh select d).Single();
                n.TrangThai = false;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult DeleteChuong(long ma)
        {
            try
            {
                var n = (from d in db.Chuong_Hoc where d.Ma_Chuong == ma select d).Single();
                n.TrangThai = 0;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateMon(long maMON, string ten)
        {
            try
            {
                var n = db.MonHocs.SingleOrDefault(x => x.Ma_Mon ==maMON);
                n.TenMon = ten;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateChuong(long ma, string ten)
        {
            try
            {
                var n = db.Chuong_Hoc.Find(ma);
                n.TenChuong = ten;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    
        public JsonResult addMon(string tenMON)
        {
           
            try

            {
                var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
                MonHoc mon = new MonHoc();
                mon.TenMon = tenMON;
                mon.TrangThai = true;
                mon.MaBoMon = session.TaiKhoan1;
                db.MonHocs.Add(mon);
                
                db.SaveChanges();

                return Json(new { code = 200, msg = "Thêm mới thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult addChuong( long ma, string ten)
        {
            try

            {
               
               Chuong_Hoc chuong = new Chuong_Hoc();
                chuong.TenChuong = ten;
                chuong.TrangThai = 1;
                chuong.Ma_Mon = ma;
                db.Chuong_Hoc.Add(chuong);

                db.SaveChanges();

                return Json(new { code = 200, msg = "Thêm mới thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult dsMon()
        {
            
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            List<Modell.MonHoc> monHocs = new MonHocDao().lisALL(session.TaiKhoan1);
            try
            {

                var dsNganh = (from n in monHocs
                               where n.TrangThai == true
                               select new
                               {
                                   MaNganh = n.Ma_Mon,
                                   TenNganh = n.TenMon
                               }).ToList();



                return Json(new { code = 200, dsNganh = dsNganh, msg = "Lấy danh sách thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public JsonResult dsChuong(string ma)
        {

            
            try
            {
         var mon=new MonHocDao().ListChapterStudy(long.Parse(ma));

                var dsNganh = (from n in mon
                               select new
                               {
                                   Ma = n.Ma_Chuong,
                                   Ten = n.TenChuong,
                                   SoCau = n.Kho_CauHoi.Count
                               }).ToList();

                return Json(new { code = 200, dsNganh = dsNganh, msg = "Lấy danh sách thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

    }
}