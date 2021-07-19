using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;

using TracNghiemOnline.Model;
//using System.Windows.Forms;
using TracNghiemOnline.Modell;
using TracNghiemOnline.Modell.Dao;

namespace TracNghiemOnline.Controllers
{
    public class TrangChuController : BaseController
    {
        // GET: TrangChu
        [HttpGet]
        
        public ActionResult Index()
        {
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var DsMonHoc = lisALLSubject(long.Parse(session.TaiKhoan1));
            return View(DsMonHoc);
        }
        public ActionResult Phong()
        {
            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var phong = db.DS_SVThi.Where(x => x.Ma_SV.Equals(session.TaiKhoan1) && !x.TrangThai.Equals("Đã Nộp"));
            List<Phong_Thi> phong_This = new List<Phong_Thi>();
            foreach (var item in phong)
            {
                if(new TracNghiemOnlineDB().Phong_Thi.Where(x=>x.MaPhong.Equals(item.MaPhong)&& !x.TrangThai.Equals("Đã Đóng") && x.Xoa==true) != null)
                {
                    phong_This.AddRange(new TracNghiemOnlineDB().Phong_Thi.Where(x => x.MaPhong.Equals(item.MaPhong) && !x.TrangThai.Equals("Đã Đóng") && x.Xoa == true));
                }
            }
            return View(phong_This);

        }
      public  void HuyDe(long madethi)
        {
            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            var dethi = db.De_Thi.Find(madethi);

            dethi.TrangThai = false;
           
            db.SaveChanges();
        //    TracNghiemOnlineDB = new TracNghiemOnlineDB();
            var ds = db.DS_SVThi.SingleOrDefault(x=>x.MaDeThi==madethi);
            ds.TrangThai = "Đã chuyển tab";
            db.SaveChanges();
        }
        public void TGTHI(string tgbd)
        {
            Session["TGTHI"] = tgbd;

        }
        public ActionResult DSDETHI(string id) {

            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var listPhong = db.Phong_Thi.Where(x => x.MaLopHP.Equals(id));
            List<DanhGia> danhgia = new List<DanhGia>();
            foreach (var item in listPhong)
            {
                var dsdethi = new TracNghiemOnlineDB().DS_SVThi.Where(x => x.Ma_SV.Equals(session.TaiKhoan1) && x.MaPhong.Equals(item.MaPhong) && x.TrangThai.Equals("Đã Nộp"));
                if (dsdethi != null)
                {
                    foreach (var item1 in dsdethi)
                    {
                        try
                        {
                            if (item1.MaDeThi != null)
                            {
                                var exam = new QuanLyThiDAO().SearDethi(item1.MaDeThi);
                                danhgia.Add(new QuanLyThiDAO().Mark(exam));

                            }
                         
                        }
                        catch { }
                    
                    }
                }

            }
           


            return View(danhgia);
        }
        public ActionResult LopHocPhan() {
            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var dsphong = db.DS_LopHP.Where(x => x.MA_SV.Equals(session.TaiKhoan1.Trim()));
            List<LopHocPhan> lop = new List<LopHocPhan>();

            foreach (var item in dsphong)
            {
                if(new TracNghiemOnlineDB().LopHocPhans.Where(x => x.MaLop.Equals(item.Ma_LOP)) != null)
                {
                    lop.AddRange(new TracNghiemOnlineDB().LopHocPhans.Where(x => x.MaLop.Equals(item.Ma_LOP)));

                }

            }


            
            return View(lop);
        }
        public ActionResult Bode(string  id)
        {
            


            var Bodeom = new TracNghiemOnlineDB().BoDeOnTaps.Where(x => x.MaLopHP.Equals(id)).ToList();

           List<Bo_De> list = new List<Bo_De>();
            foreach (var item in Bodeom)
            {
                if (item.ThoiGianMo == null && item.ThoiGianDong == null)
                {
                    list.Add(item.Bo_De);
                }
                else if (item.ThoiGianMo <=DateTime.Now && item.ThoiGianDong >= DateTime.Now)
                {
                    list.Add(item.Bo_De);
                }
                else if (item.ThoiGianMo>=DateTime.Now && item.ThoiGianDong == null)
                {
                    list.Add(item.Bo_De);
                }
                else if (item.ThoiGianDong <= DateTime.Now)
                {
                    list.Add(item.Bo_De);
                }
                else if(item.ThoiGianDong == null)
                {
                    list.Add(item.Bo_De);
                }
              
            }
            return View(list);
        }
        public ActionResult Loald(string id)
        {
          string tgbd= (string) Session["TGTHI"];
            var ngay = tgbd.Split('/');
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var list = new BoDeDao().ChapterStudy(long.Parse(id));
            var Exem = new De_Thi();
            try
            {
               Exem=(De_Thi)Session[ComMon.ComMonStants.ExamQuesTion];


                if (Exem == null)
                {
                    Exem = new BoDeDao().MixExemQuestion(list, session.TaiKhoan1);
             
                }
            }
            catch
            {
                Exem = new BoDeDao().MixExemQuestion(list, session.TaiKhoan1);
           
            }
            Session[ComMon.ComMonStants.ExamQuesTion] = Exem;
            DateTime data = new DateTime(int.Parse(ngay[0]), int.Parse(ngay[1]), int.Parse(ngay[2]), int.Parse(ngay[3]), int.Parse(ngay[4]), int.Parse(ngay[5])).AddMinutes(double.Parse(list.ThoiGianThi));
            ViewBag.GioThi= data.ToString("yyyy/MM/dd HH:mm:ss");
            ViewBag.DeThi =(De_Thi) Exem;
            return View();
        }

        public ActionResult Menul()
        {
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var DsMonHoc = lisALLSubject(long.Parse(session.TaiKhoan1));

            return PartialView(DsMonHoc);

        }
        internal object lisALLSubject(long taiKhoan1)
        {
            //TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            //var sv = db.SinhViens.Find(taiKhoan1);
            //var lop = db.Lops.Find(sv.Ma_Lop);
            //var dsmon = db.DS_MonHoc.Where(x => x.Ma_Nganh == lop.Ma_Nganh).ToList();
            //var MonHoc = new List<MonHoc>();
            //foreach (var item in dsmon)
            //{
            //    MonHoc.Add(new TracNghiemOnlineDB().MonHocs.Find(item.Ma_Mon));
            //}
            return null;
        }
        public  ActionResult LuaChon(string madethi ,string id)
        {
            var examQuestion = (De_Thi)Session[ComMon.ComMonStants.ExamQuesTion];
            foreach (var item in examQuestion.CauHoiDeThis)
            {
                foreach (var item1 in item.Kho_CauHoi.Dap_AN)
                {
                    if (item1.MA_DAN == long.Parse(id))
                    {
                        foreach (var item2 in item.Kho_CauHoi.Dap_AN)
                        {
                            if (item2.MA_DAN == long.Parse(id))
                            {
                                item2.TrangThai = true;
                            }
                            else
                            {
                                item2.TrangThai = false;
                            }
                        }
                    }

                  
                }

            }
          
            new BoDeDao().OptionStudent(examQuestion);
            Session[ComMon.ComMonStants.ExamQuesTion] = examQuestion;
            return Content("");

        }
        [ChildActionOnly]
        public ActionResult DangNhapPhongThi()
        {
            return PartialView();
        }
  [HttpGet]
        public ActionResult VaoThi(string id)
        {
          
            var phong = new QuanLyThiDAO().ExamitionRoom(id);
          
            if(phong.TrangThai.Equals("Chưa Thi"))
            {
                ViewBag.MaPhong = id;
                return View("PhongCho");
            }
           
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var DeThi = new QuanLyThiDAO().SeachForTheExam(phong,session.TaiKhoan1);
            if (DeThi == null)
            {
          
                var list = new BoDeDao().ChapterStudy(long.Parse(phong.MaBoDe.ToString()));
                DeThi = new BoDeDao().MixExemQuestion(list, session.TaiKhoan1);
           
           

            }
            new BoDeDao().UpdateDsThi(phong, DeThi,session.TaiKhoan1, "Đang Làm");
            Session[ComMon.ComMonStants.ExamQuesTion] = DeThi;
            DateTime dateTime =DateTime.Parse(phong.ThoiGianDong.ToString());
            ViewBag.GioThi = dateTime.ToString("yyyy/MM/dd HH:mm:ss");
          
            ViewBag.DeThi = DeThi;
            return View(DeThi);
         
        }
        public JsonResult KiemTraDe(long madethi)
        {
           var dethi = new TracNghiemOnlineDB().De_Thi.Find(madethi);

            try
            {
                var ds = new TracNghiemOnlineDB().DS_SVThi.SingleOrDefault(x => x.MaDeThi == madethi);
                if (ds.TrangThai.Equals("Đã Nộp"))
                {
                    return Json(new
                    {

                        status = false

                    });
                }
            }
            catch { }
         

            if (dethi.TrangThai == false)
            {
                return Json(new
                {

                    status = false

                }) ;
            }
            else
            {
                return Json(new
                {

                    status = true

                }) ;
            }
         }

        public JsonResult KiemTra(string MaPhong)
        {
            var phong = new QuanLyThiDAO().ExamitionRoom(MaPhong);
            try
            {
                if (phong.TrangThai.Contains("Chưa Thi"))
                {
                    return Json(new
                    {

                        status = false

                    }, JsonRequestBehavior.AllowGet);

                }
                return Json(new
                {

                    status = true

                }, JsonRequestBehavior.AllowGet);
            }
            catch {
                return Json(new
                {

                    status = true

                }, JsonRequestBehavior.AllowGet);
            }

          


        }

        public JsonResult PhongThi(string MaPhong)
        {
            var Phong = new QuanLyThiDAO().ExamitionRoom(MaPhong);
            try {
                if (Phong.ThoiGianDong <= DateTime.Now) {
                    Phong.TrangThai = "Đã Đóng";
                    new QuanLyThiDAO().UpDatePhongThi(Phong);
                } }
            catch (Exception e)
            {


            }
            if (Phong == null)
            {
                return Json(new
                {

                    status = "Mã Bạn Nhập Không Đúng Vui Lòng Kiểm Tra Lai"

                }, JsonRequestBehavior.AllowGet);

            }
            else
            {
                
                    if (Phong.TrangThai.Contains("Đã Đóng"))
                    {
                        return Json(new
                        {

                            status = "Phòng Thi Đã Kết Thúc Lúc" + Phong.ThoiGianDong.ToString()

                        }, JsonRequestBehavior.AllowGet);


                    }
               
                else
                {
                    var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
                    var Check = new QuanLyThiDAO().Check(MaPhong, session.TaiKhoan1);
                    if (Check != null)
                    {
                        try
                        {
                            if (Check.TrangThai.Equals("Đã Nộp"))
                            {


                                return Json(new
                                {

                                    status = "Bạn Đã Nộp Bài "

                                }, JsonRequestBehavior.AllowGet);
                            }
                            else
                            {
                                try
                                {
                                    if (new TracNghiemOnlineDB().De_Thi.Find(Check.MaDeThi).TrangThai == false)
                                    {
                                        return Json(new
                                        {

                                            status = "Bạn Đã Bị Cấm Thi"

                                        }, JsonRequestBehavior.AllowGet);
                                    }
                                }
                                catch { }

                                return Json(new
                                {

                                    status = true

                                }, JsonRequestBehavior.AllowGet);
                            }
                        }
                        catch
                        {

                            return Json(new
                            {

                                status = true

                            }, JsonRequestBehavior.AllowGet);
                        }
                     
                   
                    }
                    else
                    {
                        return Json(new
                        {

                            status = "Phòng Này Bạn Không Thể Thi"

                        },JsonRequestBehavior.AllowGet);
                    }

                }
            }
            
         
        }

      


        public ActionResult MonHoc(string id)
        {

            return View();
        }

        
             public ActionResult DanhGia(string id)
        {

            return View();
        }

      
        public ActionResult TAODE(string id)
        {
            return View();
        }
        public ActionResult Menu()
        {
            var taikhoan = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var tendangnhap = new TracNghiemOnlineDB().TaiKhoans.Where(x => x.TenDangNhap == taikhoan.TenDangNhap).FirstOrDefault().TaiKhoan1;
            ViewBag.HoTen = new TracNghiemOnlineDB().SinhViens.Where(x => x.MaSV == tendangnhap).FirstOrDefault().Ten;
            return PartialView();
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
            Session[ComMon.ComMonStants.ExamQuesTion] = null;
            return View(mark);
        }

        public ActionResult DiemSo(long? id)
        {
            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            var de = db.De_Thi.Find(id);
            de.TrangThai = false;
          
            db.SaveChanges();
        
            var exam = new QuanLyThiDAO().SearDethi(id);
            var mark = new QuanLyThiDAO().Mark(exam);
            Session[ComMon.ComMonStants.ExamQuesTion] = null;
            return View(mark);
        }
    }
}