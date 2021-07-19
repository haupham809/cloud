using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using TracNghiemOnline.Controllers;
using TracNghiemOnline.Model;
using TracNghiemOnline.Modell;
using TracNghiemOnline.Modell.Dao;

namespace TracNghiemOnline.Areas.Admin.Controllers
{
    public class QuanLyThiController : BaseController
    {
        // GET: Admin/QuanLyThi
        public ActionResult LichSuThi()
        {
            return View();
        }
        public ActionResult KiThi()
        {

            return View(new TracNghiemOnlineDB().KiThis.Select(x => x).ToList());
        }
        public void AddKiThi(string nd)
        {
   
            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            KiThi kiThi = new KiThi();
            kiThi.TenKi = nd;
            kiThi.TrangThai = true;
            db.KiThis.Add(kiThi);
            db.SaveChanges();

        }
        public void Update(long maki, string nd, bool trangthai)
        {
            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            KiThi kiThi = db.KiThis.Find(maki);
            kiThi.TenKi = nd;
            kiThi.TrangThai = trangthai;
            db.SaveChanges();

        }

        public JsonResult DeleteKiThi(long maki)
        {
            try
            {
                TracNghiemOnlineDB db = new TracNghiemOnlineDB();
                KiThi kiThi = db.KiThis.Find(maki);
                db.KiThis.Remove(kiThi);
                db.SaveChanges();

            }
            catch (Exception e)
            {
                return Json(new
                {
                    msg = "Đang Có Lớp Học Được Tạo"
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                msg = "Xóa Thành Công"
            }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult LoadKiThi()
        {
            var Kithi = (from c in new TracNghiemOnlineDB().KiThis
                         select (new
                         {
                             c.TenKi,
                             c.MAKI,
                             c.TrangThai,
                         })).ToList();
            return Json(new
            {
                Kithi
            }, JsonRequestBehavior.AllowGet);

        }


        public JsonResult KiemTraPhongThi()
        {
            var classRoom = (Phong_Thi)Session[ComMon.ComMonStants.ExamRoom];
            var KiemTra = new QuanLyThiDAO().ExamitionRoom(classRoom.MaPhong);
            foreach (var item in classRoom.DS_SVThi)
            {
                if (KiemTra.DS_SVThi.ToList().Exists(x => x.Ma_SV == item.Ma_SV && x.TrangThai != item.TrangThai))
                {
                    classRoom = KiemTra;
                    Session[ComMon.ComMonStants.ExamRoom] = classRoom;

                    var DSSV = (from c in classRoom.DS_SVThi
                                select new
                                {
                                    MaSV = c.Ma_SV,
                                    Ten = c.SinhVien.Ten,
                                    TenLop = c.SinhVien.Lop.TenLop,
                                    TrangThai = c.TrangThai,
                                    MaDe = c.MaDeThi,
                                    TinhTrang = new TracNghiemOnlineDB().De_Thi.Find(c.MaDeThi).TrangThai,


                                }).ToList();
                    return Json(new
                    {
                        Status = true,
                        data = DSSV,

                    }, JsonRequestBehavior.AllowGet);

                }

            }

            return Json(new
            {
                Status = false,
            }, JsonRequestBehavior.AllowGet);



        }

        public JsonResult LoadPhongThi()
        {
            var classRoom = (Phong_Thi)Session[ComMon.ComMonStants.ExamRoom];
            var KiemTra = new QuanLyThiDAO().ExamitionRoom(classRoom.MaPhong);

            var DSSV = (from c in classRoom.DS_SVThi
                        select new
                        {
                            MaSV = c.Ma_SV,
                            Ten = c.SinhVien.Ten,
                            TenLop = c.SinhVien.Lop.TenLop,
                            TrangThai = c.TrangThai,
                            MaDe = c.MaDeThi,
                            TinhTrang = new TracNghiemOnlineDB().De_Thi.Find(c.MaDeThi).TrangThai,


                        }).ToList();
            return Json(new
            {
                Status = true,
                data = DSSV,

            }, JsonRequestBehavior.AllowGet);

        }

        public JsonResult ListALLClassRoom()
        {
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var lop = new TracNghiemOnlineDB().LopHocPhans.Where(x => x.TrangThai == 1).ToList();
            if (session.ChưcVu == "Cán Bộ") {
                 lop = (List<LopHocPhan>)new QuanLyThiDAO().ListClassRom(session.TaiKhoan1);
            }      
            var ListGV = (from c in new TracNghiemOnlineDB().GiaoViens.Where(x => x.TrangThai == true)
                          select new
                          {
                              c.MaGV,
                              c.TenGV,

                          }).ToList();
            var LopHoc = (from c in lop
                          select new
                          {
                              id = c.MaLop,
                              Ten = c.TenLop,

                          }).ToList();
            return Json(new
            {
                code = 200,
                lop = LopHoc,
                ListGV
            }, JsonRequestBehavior.AllowGet) ;
        }
        public ActionResult TroVe()
        {
             var taikhoan =(TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            if(taikhoan.ChưcVu.Contains("Sinh"))
            {
                
                return Redirect("/LopHocPhan");
            }
            else
            {
                return RedirectToAction("DanhGiaKetQuahocTap", "QuanLyThi");
               
            }
        } 

        public ActionResult DiemSo(long id)
        {
            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            var de = db.De_Thi.Find(id);
            de.TrangThai = true;
            db.SaveChanges();
            var SV = db.DS_SVThi.SingleOrDefault(x => x.MaDeThi == de.MaDeThi);

            try
            {
                if (SV != null)
                {
                    SV.TrangThai = "Đã Nộp";
                    db.SaveChanges();
                }
            }
            catch { }
            var exam = new QuanLyThiDAO().SearDethi(id);
            var mark = new QuanLyThiDAO().Mark(exam);
            return View(mark);
        }
        public ActionResult Diemthi(long id)
        {
            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            var de = db.De_Thi.Find(id);
            de.TrangThai = true;
            db.SaveChanges();
            var SV = db.DS_SVThi.SingleOrDefault(x => x.MaDeThi == de.MaDeThi);
            try
            {
                if (SV != null)
                {
                    SV.TrangThai = "Đã Nộp";
                    db.SaveChanges();
                }
            }
            catch { }
            var exam = new QuanLyThiDAO().SearDethi(id);
            var mark = new QuanLyThiDAO().Mark(exam);
            return View(mark);
        }

        public ActionResult DanhGiaKetQuahocTap()
        {
            var tk = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var phonghoc = new QuanLyThiDAO().DanhGiaKetQua(tk.TaiKhoan1);
            return View(phonghoc);

        }
        [HttpPost]
        public JsonResult XoaSVPhongThi(string Maphong, string Masv)
        {

            try
            {
                var phong = new QuanLyThiDAO().ExamitionRoom(Maphong);
                if (phong.TrangThai.Equals("Chưa Thi"))
                {
                    var db = new TracNghiemOnlineDB();
                    var sv = db.DS_SVThi.SingleOrDefault(x => x.Ma_SV.Equals(Masv) && x.MaPhong.Equals(Maphong));
                    db.DS_SVThi.Remove(sv);
                    db.SaveChanges();

                    return Json(new
                    {
                        status = true

                    });
                }

            }
            catch (Exception e)
            {
                return Json(new
                {
                    status = "Không Thể Xóa SV Này"

                });
            }

            return Json(new
            {
                status = "Phòng Đang Thi Không Thể Xóa"

            });

        }
        public ActionResult KetQuaphongthi(string id)
        {
            var Ketqua = (List<DanhGia>)(new QuanLyThiDAO().ListALLexam(id));

            var DanhGia = new List<Danh_Gia>();
            foreach (var item in Ketqua)
            {
                foreach (var item1 in item.ketQuaThi.De_Thi.Danh_Gia)
                {
                    Danh_Gia danh_Gia = new Danh_Gia();
                    danh_Gia.MaChuong = item1.MaChuong;
                    danh_Gia.Chuong_Hoc = item1.Chuong_Hoc;
                    danh_Gia.SoCauDung = item1.SoCauDung;
                    danh_Gia.TongCau += item1.TongCau; ;
                    danh_Gia.DanhGia = "" + ((double)(item1.SoCauDung / item1.TongCau) * 100);
                    DanhGia.Add(danh_Gia);
                }
                break;
            }
            for (int i = 1; i < Ketqua.Count; i++)
            {
                foreach (var item1 in DanhGia)
                {
                    foreach (var item in Ketqua[i].ketQuaThi.De_Thi.Danh_Gia)
                    {
                        if (item1.MaChuong == item.MaChuong)
                        {
                            item1.SoCauDung += item.SoCauDung;
                            item1.TongCau += item.TongCau;
                            item1.DanhGia = "" + ((double)(item1.SoCauDung / item1.TongCau) * 100);
                        }

                    }

                }

            }
            ViewBag.Maphong = id;
            ViewBag.DanhGia = DanhGia;
            return View(Ketqua);
        }



        public ActionResult LopHocPhan()
        {
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var LopHoc = new TracNghiemOnlineDB().LopHocPhans.Where(x=>x.TrangThai==1).ToList();
            bool quyen = true;
             if(session.ChưcVu.Equals("Cán Bộ"))
            {
                LopHoc = new QuanLyThiDAO().ListClassRom(session.TaiKhoan1);
                quyen = false;
            }
            ViewBag.Quyen=(bool)quyen;
            return View(LopHoc);
        }
        public ActionResult PhongThi()
        {
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var PhongThi = new QuanLyThiDAO().ListAllClassRom(session.TaiKhoan1);

            return View(PhongThi);
        }

        public ActionResult CoiThi()
        {
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var PhongThi = new QuanLyThiDAO().CoiThi(session.TaiKhoan1);

            return View(PhongThi);
        }

      
        public JsonResult GiaoVienDay(long MaMon)
        {
            var Mon = new TracNghiemOnlineDB().MonHocs.Find(MaMon);
            var bomon = new TracNghiemOnlineDB().BoMons.Find(Mon.MaBoMon);

            var DSGV = new TracNghiemOnlineDB().GiaoViens.Where(x => x.MaBoMon.Equals(bomon.Ma_BoMon)&&x.TrangThai==true);

            var data= from c in DSGV.ToList()
                      select (new{
                      c.MaGV,
                      c.TenGV,
                      
                      });

            return Json(new { data}, JsonRequestBehavior.AllowGet);
        }
        public JsonResult CreateClassRoom()
        {
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            //  var gv = new TracNghiemOnlineDB().GiaoViens.Find(session.TaiKhoan1);
            var monhoc = (from c in new TracNghiemOnlineDB().MonHocs.Where(x => x.TrangThai == true).ToList()
                          select (new
                          {

                              c.Ma_Mon,
                              c.TenMon,

                          })
                          ).ToList();
            var kithi = (from c in new TracNghiemOnlineDB().KiThis.Where(x => x.TrangThai == true)
                         select (new
                         {

                             c.MAKI,
                             c.TenKi,

                         })
                        ).ToList();
            return Json(new { monhoc, kithi }, JsonRequestBehavior.AllowGet);
            //return View(monhoc);
        }
        public ActionResult ChonDe(string id)
        {
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var LopHoc1 = new TracNghiemOnlineDB().LopHocPhans.Where(x => x.TrangThai == 1).ToList();
            bool quyen = true;
            if (session.ChưcVu.Equals("Cán Bộ"))
            {                
                quyen = false;
            }
            ViewBag.Quyen = quyen;
            var classRoom = (Phong_Thi)Session[ComMon.ComMonStants.ExamRoom];
            classRoom.MaBoDe = long.Parse(id);
            classRoom.Bo_De = new BoDeDao().ChapterStudy(long.Parse(id));
            ViewBag.Phong = classRoom;
            return View("VaoThi1");
        }
        public JsonResult LoadLopHP()
        {
      
       
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var LopHoc1 = new TracNghiemOnlineDB().LopHocPhans.Where(x => x.TrangThai == 1).ToList();
            bool quyen = true;
            if (session.ChưcVu.Equals("Cán Bộ"))
            {
                LopHoc1 = new QuanLyThiDAO().ListClassRom(session.TaiKhoan1);
                quyen = false;
            }

            var LopHoc = (from c in LopHoc1
                          select (new
                          {
                              c.MaLop,
                              c.TenLop,
                              c.MaMon,
                              c.MonHoc.TenMon,
                              c.MaKi,
                              c.KiThi.TenKi,
                              c.GiaoVien.TenGV,
                              c.MaGV

                          })
                          ).ToList();

            return Json(new
            {
                quyen,
                lop = LopHoc

            }, JsonRequestBehavior.AllowGet);

        }
        public JsonResult LoadBode(string id)
        {
            BoDeOnTap boDeOnTap = new BoDeOnTap();
            boDeOnTap.MaLopHP = id;
            Session[ComMon.ComMonStants.OnTap] = boDeOnTap;
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var lophoc = new TracNghiemOnlineDB().LopHocPhans.Find(id);
            var bode = new TracNghiemOnlineDB().Bo_De.Where(x => x.Ma_Mon == lophoc.MaMon && x.Ma_NguoiTao ==session.TaiKhoan1 && x.Xoa==true).ToList();
          
            var bode1 = (from n in bode
                         select new
                         {
                             Ten = n.NoiDung,
                             MaDe = n.Ma_BoDe,
                             SoCau = n.SoCau,
                             ThoiGian = n.ThoiGianThi,
                             TenMon = n.MonHoc.TenMon,


                         }).ToList();

            return Json(new
            {
                Bode = bode1

            }, JsonRequestBehavior.AllowGet); ;
        }




        public JsonResult VaoThi(string id)
        {
            var classRom = new QuanLyThiDAO().ExamitionRoom(id);
           
            Session[ComMon.ComMonStants.ExamRoom] = classRom;
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            bool quyen = true;
            var bode = new BoDeDao().ListALLChapterStudy(long.Parse(classRom.LopHocPhan.MaMon.ToString()),session.TaiKhoan1);
            if (session.ChưcVu.Equals("Cán Bộ"))
            {
               bode = new TracNghiemOnlineDB().Bo_De.Where(x => x.Ma_Mon == classRom.LopHocPhan.MaMon && x.Ma_NguoiTao == session.TaiKhoan1 && x.Xoa == true).ToList();
               
            }
            long made = 0;
            string trangthai = classRom.TrangThai;
            try
            {
                made = long.Parse(classRom.MaBoDe.ToString());
            }
            catch {
                made = 0;
            }

            var bode1 = (from n in bode
                         select new
                         {
                             Ten = n.NoiDung,
                             MaDe = n.Ma_BoDe,
                             SoCau = n.SoCau,
                             ThoiGian = n.ThoiGianThi,
                             TenMon = n.MonHoc.TenMon,
                             
                             NguoiTao = new TracNghiemOnlineDB().GiaoViens.Find(n.Ma_NguoiTao).TenGV

                         }).ToList();

            return Json(new
            {
                Bode = bode1,trangthai,made,

            }, JsonRequestBehavior.AllowGet); ;

        }
        public void DeleteSinhVien(string maSV, string MaLop)
        {

            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            var SV = db.DS_LopHP.SingleOrDefault(x => x.Ma_LOP.Equals(MaLop) && x.MA_SV.Equals(maSV));
            db.DS_LopHP.Remove(SV);
            db.SaveChanges();

        }


        public string CreateLopHoc(string malop, long mamon, long maki,string GV)
        {
          
            new QuanLyThiDAO().CreateClassRoom(malop, mamon, GV, maki);
            return "";
        }
        public string CreatePhongThi(string MaLop, string DS, string MaSV, string nd)
        {
            var DSSV = new JavaScriptSerializer().Deserialize<List<DS_SVThi>>(DS);
            if (MaSV.Length == 0)
            {
                var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
                new QuanLyThiDAO().CreateExamitionRoom(MaLop, session.TaiKhoan1, DSSV, nd);
            }
            else
            {
                new QuanLyThiDAO().CreateSinhVienRoom(MaSV, DSSV);
            }
            return "";
        }
        public JsonResult UpateTrangThai(bool s, long dethi)
        {
            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            var de = db.De_Thi.Find(dethi);
            if (s == true)
            {
                de.TrangThai = false;
            }
            else
            {
                de.TrangThai = true;
            }
            db.SaveChanges();
            var classRoom = (Phong_Thi)Session[ComMon.ComMonStants.ExamRoom];
            var KiemTra = new QuanLyThiDAO().ExamitionRoom(classRoom.MaPhong);
            var DSSV = (from c in classRoom.DS_SVThi
                        select new
                        {
                            MaSV = c.Ma_SV,
                            Ten = c.SinhVien.Ten,
                            TenLop = c.SinhVien.Lop.TenLop,
                            TrangThai = c.TrangThai,
                            MaDe = c.MaDeThi,
                            TinhTrang = new TracNghiemOnlineDB().De_Thi.Find(c.MaDeThi).TrangThai,

                        }).ToList();
            return Json(new
            {
                Status = true,
                data = DSSV,

            }, JsonRequestBehavior.AllowGet);


        }

        public ActionResult ToChucThi(string id)
        {
            var classRom = new QuanLyThiDAO().ExamitionRoom(id);
            classRom.Bo_De = new BoDeDao().ChapterStudy(long.Parse(classRom.MaBoDe.ToString()));
            Session[ComMon.ComMonStants.ExamRoom] = classRom;
            DateTime dateTime = DateTime.Parse(classRom.ThoiGianDong.ToString());
            ViewBag.GioThi = dateTime.ToString("yyyy/MM/dd HH:mm:ss");
            return View(classRom);
        }

        public JsonResult UpdateExamRoom(string id,long? made)
        {
            try
            {
                if (id.Length > 0)
                {
                    TracNghiemOnlineDB db = new TracNghiemOnlineDB();
                    var claass = db.Phong_Thi.Find(id);
                    claass.Xoa = false;
                    db.SaveChanges();
                    return Json(new
                    {
                        status = true
                    });
                }

            }
            catch { };
            var classRoom = (Phong_Thi)Session[ComMon.ComMonStants.ExamRoom];
           
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            if (session.ChưcVu.Equals("Cán Bộ"))
            {
                classRoom.TrangThai = "Đang Thi";
                new QuanLyThiDAO().UpDatePhongThi(classRoom);
                return Json(new
                {
                    status = classRoom.MaPhong
                });

            }
            else
            {
                classRoom.MaBoDe = made;
                new QuanLyThiDAO().UpDatePhongThi1(classRoom);
                return Json(new
                {
                    status = classRoom.MaPhong
                });
            }
          
        }

        public JsonResult UpdateRoom(string tgbd)
        {
            string[] ngay = tgbd.Split('/');

            var classRoom = (Phong_Thi)Session[ComMon.ComMonStants.ExamRoom];

            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            if (session.ChưcVu.Equals("Cán Bộ"))
            {
                classRoom.TrangThai = "Đang Thi";
                classRoom.ThoiGianMo = new DateTime(int.Parse(ngay[0]), int.Parse(ngay[1]), int.Parse(ngay[2]), int.Parse(ngay[3]), int.Parse(ngay[4]), int.Parse(ngay[5]));
               classRoom.ThoiGianDong = new DateTime(int.Parse(ngay[0]), int.Parse(ngay[1]), int.Parse(ngay[2]), int.Parse(ngay[3]), int.Parse(ngay[4]), int.Parse(ngay[5])).AddMinutes(double.Parse(classRoom.Bo_De.ThoiGianThi));
                new QuanLyThiDAO().UpDatePhongThi(classRoom);
                return Json(new
                {
                    status = classRoom.MaPhong
                });

            }
            else
            {
                new QuanLyThiDAO().UpDatePhongThi1(classRoom);
                return Json(new
                {
                    status = classRoom.MaPhong
                });
            }

        }
        public ActionResult DSSinhVen(string id)
        {
            bool quyen = true;
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            if (session.ChưcVu.Equals("Cán Bộ"))
            {
                quyen = false;
            }


            var phong = new QuanLyThiDAO().ClassRom(id);
            ViewBag.phong = phong;
            ViewBag.MaPhong = (string)"";
            ViewBag.Quyen = quyen;

            return View();
        }

        public ActionResult DSSVPhongThi(string id)
        {
            bool quyen = true;
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            if (session.ChưcVu.Equals("Cán Bộ"))
            {
                quyen = false;
            }

            Phong_Thi phong_Thi = new QuanLyThiDAO().ExamitionRoom(id);
            var lopHocPhan = phong_Thi.LopHocPhan;

            foreach (var item in phong_Thi.LopHocPhan.DS_LopHP.ToList())
            {
                if (phong_Thi.DS_SVThi.ToList().Exists(x => x.Ma_SV.Equals(item.MA_SV)))
                {
                    lopHocPhan.DS_LopHP.Remove(item);
                }

            }
            ViewBag.Quyen = quyen;
            ViewBag.phong = lopHocPhan;
            ViewBag.MaPhong = (string)phong_Thi.MaPhong;
            return View("DSSinhVen");
        }

        public ActionResult ThemSinhVien(string id)
        {
            var Lisv = new QuanLyThiDAO().LissAllSinhVien(id);
            ViewBag.MaPhong = id;
            return View(Lisv);
            
        }

        public string LuuSinhVien(string DSlop)
        {
            var dS = new JavaScriptSerializer().Deserialize<List<DS_LopHP>>(DSlop);
            var db = new TracNghiemOnlineDB();
            db.DS_LopHP.AddRange(dS);
            db.SaveChanges();
            return "";
        }
    }
}