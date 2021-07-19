using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TracNghiemOnline.Modell;
namespace TracNghiemOnline.Areas.Admin.Controllers
{
    public class HeThongController : BaseController
    {
        TracNghiemOnlineDB db = new TracNghiemOnlineDB();
        // GET: Admin/HeThong
        
        public ActionResult NganhHoc()
        {
            
            return View();
        }
        public ActionResult BoMon()
        {

            return View();
        }

        public JsonResult dsBoMon()
        {
            try
            {

                var dsNganh = (from n in db.BoMons
                               where n.TrangThai==true
                               select new
                               {
                                   MaNganh = n.Ma_BoMon,
                                   TenNganh = n.Ten
                               }).ToList();



                return Json(new { code = 200, dsNganh = dsNganh, msg = "Lấy danh sách thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        public JsonResult dsNganh()
        {
            try
            {

                   var dsNganh = (from n in db.Nganhs where n.DaXoa != 1 
                               select new
                               {
                                   MaNganh = n.Ma_Nganh,
                                   TenNganh = n.TenNganh }).ToList();

 
                    
                return Json(new { code = 200, dsNganh = dsNganh, msg="Lấy danh sách thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddNganh(int maNganh, string tenNganh)
        {
            try
            {
                var n = new Nganh();
                n.Ma_Nganh = maNganh;
                n.TenNganh = tenNganh;
                db.Nganhs.Add(n);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Thêm mới thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message}, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddBoMon( string tenNganh)
        {
            try

            {
                var n = new BoMon();
                Random random = new Random();
                while (true)
                {

                    n.TrangThai = true;
                    n.Ten = tenNganh;
                    var ma = "BM"+random.Next(10000000, 90000000);
                    if (!db.GiaoViens.ToList().Exists(x => x.MaGV.Equals(ma)))
                    {
                        n.Ma_BoMon = ma;
                        break;
                    };
                }
                    db.BoMons.Add(n);
                GiaoVien giaoVien = new GiaoVien();
                giaoVien.MaGV = n.Ma_BoMon;
                giaoVien.MaBoMon = n.Ma_BoMon;
                giaoVien.TrangThai = false;
                db.GiaoViens.Add(giaoVien);
                db.SaveChanges();
                    TaiKhoan taiKhoan = new TaiKhoan();
                    taiKhoan.TenDangNhap = "BM" + n.Ma_BoMon;
                    taiKhoan.MatKhau = ""+1;
                    taiKhoan.TaiKhoan1 = n.Ma_BoMon;
                   taiKhoan.ChưcVu = "BoMon";
                taiKhoan.TrangThai = true;
                db.TaiKhoans.Add(taiKhoan);
                     db.SaveChanges();               
                return Json(new { code = 200, msg = "Thêm mới thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateBoMon(string maNganh, string tenNganh)
        {
            try
            {
                var n = db.BoMons.SingleOrDefault(x => x.Ma_BoMon .Equals(maNganh));
                n.Ten = tenNganh;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult UpdateNganh(int maNganh, string tenNganh)
        {
            try
            {
                var n = db.Nganhs.SingleOrDefault(x => x.Ma_Nganh == maNganh);
                n.TenNganh = tenNganh;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeleteNganh(int maNganh)
        {
            try
            {
                var n = (from d in db.Nganhs where d.Ma_Nganh == maNganh select d).Single();
                n.DaXoa = 1;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeleteBoMon(string maNganh)
        {
            try
            {
                var n = (from d in db.BoMons where d.Ma_BoMon == maNganh select d).Single();
                n.TrangThai = false;
                db.SaveChanges();
                var b = (from d in db.TaiKhoans where d.MatKhau == maNganh select d).Single();
                b.TrangThai = false;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult SinhVien()
        {

            List<Lop> ltsLop = db.Lops.Where(x=>x.DaXoa!=1).ToList();

            ViewBag.lstLop = new SelectList(ltsLop, "Ma_Lop", "TenLop");
            return View();
        }
        public JsonResult dsSinhVien()
        {
            try
            {

        
                var dsSV = (from n in db.SinhViens
                            where n.DaXoa != 1
                            select new

                               {
                                    MaSV = n.MaSV,
                                    TenSV = n.Ten,
                                    NgaySinh = n.NgaySinh.ToString(),
                                    DiaChi = n.DiaChi,
                                    TenLop = n.Lop.TenLop,
                                    MaLop = n.Lop.Ma_Lop
                               }).ToList();

                return Json(new { code = 200, dsSinhVien = dsSV, msg = "Lấy danh sách thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddSinhVien(string maSV, string tenSV, DateTime ngaySinh, string diaChi, string maLop )
        {
            try
            {
                var n = new SinhVien();
                n.MaSV = maSV;
                n.Ten = tenSV;
                n.NgaySinh = ngaySinh;
                n.DiaChi = diaChi;               
                n.Ma_Lop = maLop;
                db.SinhViens.Add(n);                 
                db.SaveChanges();
                TracNghiemOnlineDB tracNghiem = new TracNghiemOnlineDB();
                TaiKhoan taiKhoan = new TaiKhoan();
                taiKhoan.MatKhau = "1";
                taiKhoan.TaiKhoan1 = ""+n.MaSV;
                taiKhoan.TenDangNhap =""+n.MaSV;
                taiKhoan.ChưcVu = "SinhViên";
                taiKhoan.TrangThai = true;
                tracNghiem.TaiKhoans.Add(taiKhoan);
                tracNghiem.SaveChanges();
                return Json(new { code = 200, msg = "Thêm mới thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        
        [HttpPost]
        public JsonResult UpdateSinhVien(string maSV, string tenSV, DateTime ngaySinh, string diaChi, string maLop)
        {
            try
            {
                var n = db.SinhViens.SingleOrDefault(x => x.MaSV.Equals(maSV));
                n.Ten = tenSV;
                n.NgaySinh = ngaySinh;
                n.DiaChi = diaChi;
                n.Ma_Lop = maLop;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeleteSinhVien(string maSV)
        {
            try
            {
                var n = (from d in db.SinhViens where d.MaSV == maSV select d).Single();
                n.DaXoa = 1;
                db.SaveChanges();
                var tk = db.TaiKhoans.Find(maSV);
                tk.TrangThai = false;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //Quan li giao vien
        public ActionResult GiaoVien()
        {
            List<BoMon> lstNganh = db.BoMons.Where(x => x.TrangThai==true).ToList();
            
            ViewBag.lstNganh = new SelectList(lstNganh, "Ma_BoMon", "Ten");
            return View();
        }
        public JsonResult dsGiaoVien()
        {
            try
            {
                var dsGV = (from n in db.GiaoViens.Where(x=>x.MaBoMon!=x.MaGV && x.TrangThai==true && x.MaBoMon!=null)
                            select new
                            {
                                MaGV = n.MaGV,
                                TenGV = n.TenGV,
                                TenNganh = n.BoMon.Ten,
                                MaNganh = n.BoMon.Ma_BoMon
                               
                            }).ToList();

                return Json(new { code = 200, dsGiaoVien = dsGV, msg = "Lấy danh sách thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddGiaoVien(string maGV, string tenGV, string maNganh)
        {
            try
            {
                var n = new GiaoVien();
                n.MaGV = maGV;
                n.TenGV = tenGV;
                n.MaBoMon = maNganh;
                n.TrangThai = true;
                db.GiaoViens.Add(n);
                db.SaveChanges();
                TaiKhoan taiKhoan = new TaiKhoan();
                taiKhoan.TenDangNhap = ""+maGV;
                taiKhoan.TaiKhoan1 = maGV;
                taiKhoan.MatKhau = "1";
                taiKhoan.ChưcVu = "Cán Bộ";
                taiKhoan.TrangThai = true;
                db.TaiKhoans.Add(taiKhoan);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Thêm mới thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateGiaoVien(long maGV, string tenGV, long maNganh)
        {
            try
            {
                var n = db.GiaoViens.SingleOrDefault(x => x.MaGV.Equals(maGV));
                n.TenGV = tenGV;
                n.Ma_Nganh = maNganh;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeleteGiaoVien(string maGV)
        {
            try
            {
                var n = (from d in db.GiaoViens where d.MaGV.Equals(maGV) select d).Single();
                n.TrangThai = false;
                db.SaveChanges();
               var tk= db.TaiKhoans.Find(maGV);
                tk.TrangThai = false;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        //Quan li lop
        public ActionResult Lop()
        {
            List<Nganh> lstNganh = db.Nganhs.Where(x => x.DaXoa != 1).ToList();
            ViewBag.lstNganh = new SelectList(lstNganh, "Ma_Nganh", "TenNganh");
            return View();
        }
        public JsonResult dsLop()
        {
            try
            {
                var dsLop = (from n in db.Lops where n.DaXoa != 1
                            select new
                            {
                                MaLop = n.Ma_Lop,
                                TenLop = n.TenLop,
                                //TenNganh = n.Nganh.TenNganh,
                              //  MaNganh = n.Nganh.Ma_Nganh

                            }).ToList();

                return Json(new { code = 200, dsLop = dsLop, msg = "Lấy danh sách thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult AddLop(string maLop, string tenLop)
        {
            try
            {
                var n = new Lop();
                n.Ma_Lop = maLop;
                n.TenLop = tenLop;
               // n.Ma_Nganh = maNganh;
                
                db.Lops.Add(n);
                db.SaveChanges();
                return Json(new { code = 200, msg = "Thêm mới thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateLop(string maLop, string tenLop)
        {
            try
            {
                var n = db.Lops.SingleOrDefault(x => x.Ma_Lop == maLop);
                n.TenLop = tenLop;
              //  n.Ma_Nganh = maNganh;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpPost]
        public JsonResult DeleteLop(string maLop)
        {
            try
            {
                var n = (from d in db.Lops where d.Ma_Lop == maLop select d).Single();
                n.DaXoa = 1;
                db.SaveChanges();
                return Json(new { code = 200, msg = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { code = 500, msg = "Không thành công" + e.Message }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}