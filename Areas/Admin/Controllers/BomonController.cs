using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TracNghiemOnline.Model;
using TracNghiemOnline.Modell;
using TracNghiemOnline.Modell.Dao;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.Drawing;
using System.Net;
using System.Web.Script.Serialization;
using TracNghiemOnline.Controllers;

namespace TracNghiemOnline.Areas.Admin.Controllers
{
    public class BomonController : BaseController
    {
        // GET: Admin/Bomon
        
        
        //moi themF
        public ActionResult DSDethitheobm()
        {

            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            ViewBag.Mabomon = session.TaiKhoan1;
           // List<Bo_De> bo_Des = new Bode().ListALLChapterBM(session.TaiKhoan1);
            List<Bo_De> bo_Des = new TracNghiemOnlineDB().Bo_De.Where(x => x.Xoa == true && x.NguoiDuyet.Equals(session.TaiKhoan1) && x.PheDuyet.Contains("Đang")).ToList();

            return View(bo_Des);

        }
        public ActionResult DSDEDUYET()
        {


            return View();

        }

        public ActionResult deletebomon(long id)
        {
            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            var list = db.DSGV_ThucHien.Where(x => x.MaLich == id).ToList();
            foreach (var item in list)
            {
                db.DSGV_ThucHien.Remove(item);
                db.SaveChanges();
            }
            var lich = db.LichNops.Find(id);
            db.LichNops.Remove(lich);
            db.SaveChanges();
            return RedirectToAction("BoMON");

        }
        public ActionResult Updatebomon(long id)
        {
            var lich = new TracNghiemOnlineDB().LichNops.Find(id);
            string chuoi = "";
            foreach (var item in new TracNghiemOnlineDB().DSGV_ThucHien.Where(x => x.MaLich==id))
            {
                chuoi += item.MaGV + "/";
            }
            string[] arr = chuoi.Split('/');
            Session["dsgv"] = arr;
            return View(lich);
           
        }
        [HttpPost]
        public ActionResult Updatebomon(LichNop lichNop)
        {
            if (ModelState.IsValid) { 
                try
            {
                string[] ds = (string[])Session["dsgv"];
                var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
                if (ds.Length > 1)
                {
         
                    TracNghiemOnlineDB db = new TracNghiemOnlineDB();
                    var lich = db.LichNops.Find(lichNop.id);
                    lich.NoiDung = lichNop.NoiDung;
                    lich.ThoiGian = lichNop.ThoiGian;
                       lich.MaMon = lichNop.MaMon;
                    db.SaveChanges();
                    var list = db.DSGV_ThucHien.Where(x => x.MaLich == lichNop.id).ToList();
                    foreach (var item in list)
                    {
                        db.DSGV_ThucHien.Remove(item);
                        db.SaveChanges();
                    }

                    for (int i = 0; i < ds.Length - 1; i++)
                    {
                        DSGV_ThucHien GV = new DSGV_ThucHien();
                        GV.MaGV = ds[i];
                        GV.MaLich = lichNop.id;
                        db.DSGV_ThucHien.Add(GV);
                        db.SaveChanges();
                    }
                    return RedirectToAction("BoMON");

                }

            }
            catch
            {
                ModelState.AddModelError("", "Vui lòng chọn giáo viên ");

            }
        }
            return View(lichNop);


    }

        public void DSGV(string chuoi)
        {
            string[] dsgv= chuoi.Split('/');
            Session["dsgv"] = dsgv;
        }
        public ActionResult TaoLich()
        {
            

            return View();
        }
        [HttpPost]
        public ActionResult TaoLich(LichNop lichNop)
        {
            if (ModelState.IsValid)
            {
                try
                {
                string[]  ds=(string[])  Session["dsgv"];
                    var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
                    if (ds.Length > 1)
                    {
                        lichNop.TrangThai = true;
                        lichNop.xoa = true;
                        lichNop.MaBoMON = session.TaiKhoan1;
                        TracNghiemOnlineDB db = new TracNghiemOnlineDB();
                        db.LichNops.Add(lichNop);
                        db.SaveChanges();
                       
                        for (int i = 0; i < ds.Length-1; i++)
                        {
                            DSGV_ThucHien GV = new DSGV_ThucHien();
                            GV.MaGV = ds[i];
                            GV.MaLich= new TracNghiemOnlineDB().LichNops.ToList().Last().id;
                            db.DSGV_ThucHien.Add(GV);
                            db.SaveChanges();
                        }
                        return RedirectToAction("BoMON");

                    }

                }
                catch {
                    ModelState.AddModelError("", "Vui lòng chọn giáo viên ");

                }
            }

            return View(lichNop);
        }

        public ActionResult BoMON()
        {
            Session["dsgv"] = "";
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var lich=new TracNghiemOnlineDB().LichNops.Where(x=>x.MaBoMON.Equals(session.TaiKhoan1));
            return View(lich);
        }
        public ActionResult DSDethi()
        {
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            ViewBag.Mabomon = session.TaiKhoan1;
    
            var dsdethi = new TracNghiemOnlineDB().Bo_De.Where(x => x.Ma_NguoiTao.Equals(session.TaiKhoan1)&&x.Xoa==true && x.TrangThai==false ||(x.TrangThai == true && x.NguoiDuyet.Equals(session.TaiKhoan1)) ).ToList();
    

            return View(dsdethi);
        }


        public ActionResult xemde(long id)
        {
            var khoch = new Bode().listkhocauhoi(id);
            ViewBag.linkpdf = xuatpdf(id, Request["tenmon"]);
            return View(khoch);

        }

        public ActionResult KetquathiTungGVBM()
        {
            var tk = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var giaovien = new Bode().danhsachgiaovienbm(tk.TaiKhoan1);
    
            return View(giaovien);


        }
        public  string xuatpdf(long id,string tenmon)
        {
            PdfDocument pdf = new PdfDocument();
            PdfPageBase page = pdf.Pages.Add();
            PdfTrueTypeFont font1 = new PdfTrueTypeFont(new Font("Arial Unicode MS", 24f, FontStyle.Bold),true);
            PdfStringFormat centerAlignment = new PdfStringFormat(PdfTextAlignment.Center);
            PdfTrueTypeFont font2 = new PdfTrueTypeFont(new Font("Arial Unicode MS", 13f, FontStyle.Regular), true);

            PdfPen pen1 = new PdfPen(Color.Black, 1f);
            page.Canvas.DrawRectangle(pen1, new Rectangle(new Point(390, 150), new Size(120,40)));
            PdfPen pen2 = new PdfPen(Color.Black, 1f);


            page.Canvas.DrawRectangle(pen1, new Rectangle(new Point(0, 0), new Size((int)page.GetClientSize().Width, (int)page.GetClientSize().Height)));
            page.Canvas.DrawString("TRƯỜNG ĐẠI HỌC GIAO THÔNG VẬN TẢI PHÂN HIỆU TẠI TP HỒ CHÍ MINH",font1 , new PdfSolidBrush(Color.Black), new RectangleF(10, 50, page.GetClientSize().Width-10 , page.GetClientSize().Height), centerAlignment);
            page.Canvas.DrawString("Đề thi môn:"+tenmon, new PdfTrueTypeFont(new Font("Arial Unicode MS", 15f, FontStyle.Bold), true), new PdfSolidBrush(Color.Black), new RectangleF(40, 160, page.GetClientSize().Width/2-40 - 10, page.GetClientSize().Height), new PdfStringFormat() { LineLimit = true });
            page.Canvas.DrawString("Mã đề:"+id, new PdfTrueTypeFont(new Font("Arial Unicode MS", 15f, FontStyle.Bold), true), new PdfSolidBrush(Color.Black), new RectangleF(400, 160, page.GetClientSize().Width/2-400 - 10, page.GetClientSize().Height), new PdfStringFormat() { LineLimit = true });
            var khoch = new Bode().listkhocauhoi(id);
            int vitridong=220;
            int slcau=0;
            int x =  50;
            int y = (int)(page.GetClientSize().Width) / 2 + 50;
            string[] cau = { "A", "B", "C", "D" };
            
            foreach (var item in khoch)
            {
                
                slcau++;
                vitridong += 13;
                page.Canvas.DrawString("Câu " + slcau + ":", font2, new PdfSolidBrush(Color.Blue), new RectangleF(20, vitridong, page.GetClientSize().Width - 90, page.GetClientSize().Height), new PdfStringFormat() { LineLimit = true });
                page.Canvas.DrawString(item.NoiDung , font2, new PdfSolidBrush(Color.Black), new RectangleF(20+ 55, vitridong, page.GetClientSize().Width - 90, page.GetClientSize().Height), new PdfStringFormat() { LineLimit = true });
                vitridong += (int)((("Câu " + slcau + ":" + item.NoiDung).Length * 13) / (page.GetClientSize().Width-40)) * 13;
                int slda = 0;
                int z = 0;
                int k = 0;
                int[] vtdongdapan = new int[4];
                if (item.HinhAnh != null)
                {
                    try
                    {
                        Image image = Image.FromFile(Server.MapPath(item.HinhAnh));
                        int width = image.Width;
                        int height = image.Height;
                        float schale = 1.5f;
                        Size size = new Size((int)(width * schale), (int)(height * schale));
                        Bitmap schaleImage = new Bitmap(image, size);
                        PdfImage pdfImage = PdfImage.FromImage(schaleImage);
                        page.Canvas.DrawImage(pdfImage, new RectangleF((page.GetClientSize().Width - 150) / 2, vitridong, 150, 150));

                        vitridong += 150;
                    }
                    catch { }
         
                }
                vitridong += 13;
                foreach (var da in item.Dap_AN)
                {
                    if (slda == 0 || slda == 2)
                    {
                        z = x;
                    }
                    else z = y;
                    vtdongdapan[slda] = (int)((( cau[slda] + ":" + da.NoiDung).Length * 13) /( page.GetClientSize().Width/2)) * 13;
                    if (vtdongdapan[slda] <= 1) vtdongdapan[slda] = 13;

                    if (da.HinhAnh != null)
                    {
                        if (13 + vtdongdapan[slda] + vitridong + 150 > page.GetClientSize().Height - 20)
                        {
                            vitridong = 20;
                            page = pdf.Pages.Add();
                            page.Canvas.DrawRectangle(pen1, new Rectangle(new Point(0, 0), new Size((int)page.GetClientSize().Width, (int)page.GetClientSize().Height)));
                        }
                        }
                    if (da.TrangThai==false) { page.Canvas.DrawString(cau[slda] + ":", font2, new PdfSolidBrush(Color.Blue), new RectangleF(z - 10, vitridong, page.GetClientSize().Width / 2 - 20, page.GetClientSize().Height - 80), new PdfStringFormat() { LineLimit = true });  }
                    else { page.Canvas.DrawString(cau[slda] + ":", font2, new PdfSolidBrush(Color.Red), new RectangleF(z - 10, vitridong, page.GetClientSize().Width / 2 - 20, page.GetClientSize().Height - 80), new PdfStringFormat() { LineLimit = true }); }

                    page.Canvas.DrawString( da.NoiDung, font2, new PdfSolidBrush(Color.Black), new RectangleF(z+5, vitridong, page.GetClientSize().Width / 2 - 20, page.GetClientSize().Height - 80), new PdfStringFormat() { LineLimit = true });

                    
                    if (da.HinhAnh != null)
                    {
                        if (13 + vtdongdapan[slda] + vitridong +150> page.GetClientSize().Height - 20)
                        {
                            vitridong = 20;
                            page = pdf.Pages.Add();
                            page.Canvas.DrawRectangle(pen1, new Rectangle(new Point(0, 0), new Size((int)page.GetClientSize().Width, (int)page.GetClientSize().Height)));
                        }
                        try
                        {
                            Image image = Image.FromFile(Server.MapPath(item.HinhAnh));
                            int width = image.Width;
                            int height = image.Height;
                            float schale = 1.5f;
                            Size size = new Size((int)(width * schale), (int)(height * schale));
                            Bitmap schaleImage = new Bitmap(image, size);
                            PdfImage pdfImage = PdfImage.FromImage(schaleImage);
                            page.Canvas.DrawImage(pdfImage, new RectangleF(z + 10, 30 + vtdongdapan[slda] + vitridong, 150, 150));
                            k = 163;
                        }
                        catch
                        {

                        }
                       
                    }
                    if (slda == 1)
                    {
                        if ((int)(((cau[slda] + ":" + da.NoiDung).Length * 13) / page.GetClientSize().Width) * 13 < 1) vitridong += 13;
                        if (vtdongdapan[0]> vtdongdapan[1])
                        vitridong += vtdongdapan[0];
                        else vitridong += vtdongdapan[1];
                        vitridong += k;
                        k=0;
                    }
                    else if (slda == 3)
                    {
                        if ((int)(((cau[slda] + ":" + da.NoiDung).Length * 13) / page.GetClientSize().Width) * 13 < 1) vitridong += 13;
                        if (vtdongdapan[2] > vtdongdapan[3])
                            vitridong += vtdongdapan[2];
                        else vitridong += vtdongdapan[3];
                        vitridong += k;
                    }
                   


                    if (vitridong > page.GetClientSize().Height-20) {
                         vitridong = 20;
                        page = pdf.Pages.Add();
                        page.Canvas.DrawRectangle(pen1, new Rectangle(new Point(0, 0), new Size((int)page.GetClientSize().Width, (int)page.GetClientSize().Height)));
                    }
                    slda++;
                }

            }
            page.Canvas.SetTransparency(0.2f);
            page.Canvas.DrawString("Hết", font1, new PdfSolidBrush(Color.Black), new RectangleF(20, page.GetClientSize().Height-30, page.GetClientSize().Width, page.GetClientSize().Height), centerAlignment);
            page.Canvas.SetTransparency(1f);
            pdf.SaveToFile(Server.MapPath("~/Content/" + "De_" + id + ".pdf"));
            pdf.Close();

            return "/Content/" + "De_"+id + ".pdf";



        }


        public ActionResult DanhgiaketquatheoBoMon(string id)
        {
            ViewBag.sss = id;
            var phonghoc = new Bode().DanhGiaKetQuatheoBoMon(id);
            var ds = new TracNghiemOnlineDB();
            foreach (var item in new TracNghiemOnlineDB().LichNops.Where(x => x.ThoiGian < DateTime.Now))
            {
                var lich = ds.LichNops.Find(item.id);
             
                ds.SaveChanges();
                foreach (var item1 in new TracNghiemOnlineDB().DSGV_ThucHien.Where(X => X.MaDE == null))
                {
                    var gv = ds.DSGV_ThucHien.Find(item1.id);
                    gv.trangthai = "Nộp Muộn";
                    ds.SaveChanges();
                }

            }

            return View(phonghoc);

        }
        public ActionResult DSDEGUI()
        {
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var BoDe = new TracNghiemOnlineDB().Bo_De.Where(x => x.Ma_NguoiTao == session.TaiKhoan1 && x.Xoa == true
             && x.PheDuyet != null).ToList();
            return View(BoDe);
        }
        public ActionResult TaoDeThi()
        {
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];

            var giaovien = new TracNghiemOnlineDB().GiaoViens.Find(session.TaiKhoan1);

            var dao = new TracNghiemOnline.Modell.TracNghiemOnlineDB().MonHocs.Where(x => x.MaBoMon==giaovien.MaBoMon).ToList();
            ViewBag.MonHoc = dao;
            ViewBag.A = "";
            ViewBag.B = "";
            ViewBag.C = "";
            reseach();
            return View();

        }
        public ActionResult ChonMon(string id)
        {
            var dethi = (Model.BoDeThi)Session[ComMon.ComMonStants.ChapterStudy];

            try
            {
                dethi.BoDeThi1.Ma_Mon = long.Parse(id);
            }
            catch { dethi.BoDeThi1.Ma_Mon = 0; }

            Session[ComMon.ComMonStants.ChapterStudy] = dethi;
            return Content("");
        }
        public ActionResult ChonTG(string tg)
        {
            var dethi = (Model.BoDeThi)Session[ComMon.ComMonStants.ChapterStudy];
            dethi.BoDeThi1.ThoiGianThi = tg;
            Session[ComMon.ComMonStants.ChapterStudy] = dethi;
            return Content("");
        }

        public ActionResult ChonDe(string id)
        {
            var dethi = (Model.BoDeThi)Session[ComMon.ComMonStants.ChapterStudy];
            dethi.LoaiDe1 = id;
            Session[ComMon.ComMonStants.ChapterStudy] = dethi;
            return Content("");
        }
        public JsonResult Taode(string SoLuong)
        {
            var soluong = new JavaScriptSerializer().Deserialize<List<SoLuongChuong>>(SoLuong);
            var sessetion = (BoDeThi)Session[ComMon.ComMonStants.ChapterStudy];
            var bo_De1 = sessetion.BoDeThi1;
            new CauHoiDao().CreateTopic(bo_De1, soluong);
            return Json(new
            {
                status = true
            });
        }
        public ActionResult Monhoc(Bo_De bo_De)
        {
            var dethi = (Model.BoDeThi)Session[ComMon.ComMonStants.ChapterStudy];
            if (ModelState.IsValid && dethi.BoDeThi1.ThoiGianThi.Length > 0 && dethi.LoaiDe1.Length > 0 && dethi.BoDeThi1.Ma_Mon > 0)
            {
                bo_De.ThoiGianThi = dethi.BoDeThi1.ThoiGianThi;
                bo_De.Ma_Mon = dethi.BoDeThi1.Ma_Mon;

                bo_De.MonHoc = new MonHocDao().Subject(long.Parse(bo_De.Ma_Mon.ToString()));
                dethi.BoDeThi1 = bo_De;
                Session[ComMon.ComMonStants.ChapterStudy] = dethi;

                if (dethi.LoaiDe1.Equals("Tự Chọn"))
                {
                    List<Kho_CauHoi> kho_CauHois = new List<Kho_CauHoi>();
                    foreach (var item in new MonHocDao().ListChapterStudy(long.Parse(dethi.BoDeThi1.Ma_Mon.ToString())))
                    {
                        kho_CauHois.AddRange(new CauHoiDao().ListQuestion(long.Parse(item.Ma_Chuong.ToString())));
                    }


                    ViewBag.Question = kho_CauHois;
                    return View("ChonCauhoi");

                }
                else
                {
                    List<SoLuongChuong> sl = new List<SoLuongChuong>();
                    foreach (var item in new TracNghiemOnline.Modell.TracNghiemOnlineDB().Chuong_Hoc.Where(x => x.Ma_Mon == bo_De.Ma_Mon).ToList())
                    {
                        SoLuongChuong soLuong = new SoLuongChuong();
                        soLuong.Chuong = item;
                        soLuong.nhanBiet = new CauHoiDao().Nuberofquestion(item.Ma_Chuong, "Nhận Biết").Count() + "";
                        soLuong.thongHieu = new CauHoiDao().Nuberofquestion(item.Ma_Chuong, "Thông Hiểu").Count() + "";
                        soLuong.vandung = new CauHoiDao().Nuberofquestion(item.Ma_Chuong, "Vận Dụng").Count() + "";
                        soLuong.vandungcao = new CauHoiDao().Nuberofquestion(item.Ma_Chuong, "Vận Dụng Cao").Count() + "";

                        sl.Add(soLuong);
                    }

                    ViewBag.Chuong = (List<SoLuongChuong>)sl;


                    return View(bo_De);

                }


            }
            else
            {
                string mess = "";
                string mess1 = "";
                string mess2 = "";


                if (dethi.BoDeThi1.ThoiGianThi.Length <= 0)
                {
                    mess = "Bạn Vui Lòng Chọn Thời Gian Thi";
                }
                if (dethi.BoDeThi1.Ma_Mon <= 0)
                {
                    mess1 = "Bạn Vui Lòng Chọn Môn Học ";
                }
                if (dethi.LoaiDe1.Length <= 0)
                {
                    mess2 = "Bạn Vui Lòng Chọn Cách Tạo Đề";
                }
                ViewBag.A = mess;
                ViewBag.B = mess1;
                ViewBag.C = mess2;

            }
            reseach();

            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];

            var giaovien = new TracNghiemOnlineDB().GiaoViens.Find(session.TaiKhoan1);

            var dao = new TracNghiemOnline.Modell.TracNghiemOnlineDB().MonHocs.Where(x => x.MaBoMon == giaovien.MaBoMon).ToList();
            ViewBag.MonHoc = dao;
            return View("TaoDeThi");

        }


        private static Bo_De bo_De1 = new Bo_De();
        public void reseach()
        {
            Model.BoDeThi boDeThi = new BoDeThi();
            bo_De1.Ma_Mon = 0;
            bo_De1.ThoiGianThi = "";
            boDeThi.LoaiDe1 = "";
            boDeThi.BoDeThi1 = bo_De1;
            Session[ComMon.ComMonStants.ChapterStudy] = boDeThi;
        }
        public ActionResult themde()
        {
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var dethi = (Model.BoDeThi)Session[ComMon.ComMonStants.ChapterStudy];
            dethi.BoDeThi1.Ma_NguoiTao = session.TaiKhoan1;
            new Bode().themde(dethi.BoDeThi1);
            return RedirectToAction("DSDethi", "Bomon");
        }
        public ActionResult loaddethi(string id)
        {
            try
            {
                if (id.Length > 0)
                {
                    var BODETHI = (Model.BoDeThi)Session[ComMon.ComMonStants.ChapterStudy];
                    BODETHI.BoDeThi1 = new BoDeDao().ChapterStudy(long.Parse(id));
                    Session[ComMon.ComMonStants.ChapterStudy] = BODETHI;
                    ViewBag.Mess = "Xem";
                }
                else
                {
                    ViewBag.Mess = "Tao";

                }
            }
            catch
            {
                ViewBag.Mess = "Tao";
            }

            var dethi = (Model.BoDeThi)Session[ComMon.ComMonStants.ChapterStudy];

            return View(dethi.BoDeThi1);
        }

        [HttpPost]
        public JsonResult Updatepheduyet(long maDe, string Pheduyet ,string lydo)
        {
            var tk = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            if (tk.ChưcVu.Contains("Admin"))
            {
                TracNghiemOnlineDB db = new TracNghiemOnlineDB();
                var Bode = db.Bo_De.Where(x => x.Ma_BoDe.Equals(maDe));
                Bode.ToList()[0].PheDuyet = Pheduyet;
                Bode.ToList()[0].TrangThai = false;
                db.SaveChanges();
                if (Pheduyet.Equals("Đã duyệt"))
                {
                    try
                    {

                        Bo_De boDeThi = new Bo_De();
                        boDeThi.NoiDung = Bode.ToList()[0].NoiDung;
                        boDeThi.Ma_Mon = Bode.ToList()[0].Ma_Mon;
                        boDeThi.ThoiGianThi = Bode.ToList()[0].ThoiGianThi;
                        boDeThi.Ma_NguoiTao = Bode.ToList()[0].Ma_NguoiTao;
                        boDeThi.NguoiDuyet = tk.TaiKhoan1;
                        boDeThi.TrangThai = true;
                        boDeThi.Xoa = true;
                        foreach (var item in Bode.ToList()[0].CauHois)
                        {
                            Modell.CauHoi cauHoi = new Modell.CauHoi();
                            cauHoi.Ma_CauHoi = item.Ma_CauHoi;
                            boDeThi.CauHois.Add(cauHoi);
                        }
                        boDeThi.SoCau = boDeThi.CauHois.Count;
                        db.Bo_De.Add(boDeThi);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        string ess = e.Message;
                    }

                    var de = new TracNghiemOnlineDB().BoMons.Select(x => x).ToList().Last();
                }

                List<Bo_De> bo_Des = new TracNghiemOnlineDB().Bo_De.Where(x => x.Xoa == true && x.NguoiDuyet.Equals(tk.TaiKhoan1) && x.TrangThai == false && x.PheDuyet.Equals("Đang xử lý")).ToList();

                var bode1 = (from n in bo_Des
                             select new
                             {
                                 Ten = n.NoiDung,
                                 MaDe = n.Ma_BoDe,
                                 SoCau = n.SoCau,
                                 ThoiGian = n.ThoiGianThi,
                                 TenMon = n.MonHoc.TenMon,
                                 Giaovien = n.GiaoVien.TenGV,
                                 Pheduyet = n.PheDuyet,
                                 Trangthai = n.TrangThai,
                             }).ToList();
                return Json(new
                {
                    Bode = bode1

                }, JsonRequestBehavior.AllowGet); ;
            }

            else
            {
                TracNghiemOnlineDB db = new TracNghiemOnlineDB();
                var ds = db.DSGV_ThucHien.Find(maDe);
                ds.trangthai = Pheduyet;
                ds.LyDo = lydo;
                db.SaveChanges();

                if (Pheduyet.Equals("Đã duyệt"))
                {
                    try
                    {
                        var Bode = new TracNghiemOnlineDB().Bo_De.Where(x=>x.Ma_BoDe==ds.MaDE);
                        Bo_De boDeThi = new Bo_De();
                        boDeThi.NoiDung = Bode.ToList()[0].NoiDung;
                        boDeThi.Ma_Mon = Bode.ToList()[0].Ma_Mon;
                        boDeThi.ThoiGianThi = Bode.ToList()[0].ThoiGianThi;
                        boDeThi.Ma_NguoiTao = Bode.ToList()[0].Ma_NguoiTao;
                        boDeThi.NguoiDuyet = tk.TaiKhoan1;
                        boDeThi.TrangThai = true;
                        boDeThi.Xoa = true;
                        foreach (var item in Bode.ToList()[0].CauHois)
                        {
                            Modell.CauHoi cauHoi = new Modell.CauHoi();
                            cauHoi.Ma_CauHoi = item.Ma_CauHoi;
                            boDeThi.CauHois.Add(cauHoi);
                        }
                        boDeThi.SoCau = boDeThi.CauHois.Count;
                        db.Bo_De.Add(boDeThi);
                        db.SaveChanges();
                    }
                    catch (Exception e)
                    {
                        string ess = e.Message;
                    }

                    var de = new TracNghiemOnlineDB().BoMons.Select(x => x).ToList().Last();
                }

                return Json(new
                {
                  statust=  true,

                }, JsonRequestBehavior.AllowGet); ;


            }


        }
        public JsonResult guide(long maDe)
        {
            var admin = new TracNghiemOnlineDB().TaiKhoans.SingleOrDefault(x => x.ChưcVu.Equals("Admin"));
            var tk = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
          //  var bomon = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];

            

            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
        
            var Bode = db.Bo_De.Find(maDe);
           // var bomon = db.BoMons.Find(tk.TaiKhoan1);
            Bode.TrangThai = false;
            Bode.Ma_NguoiTao = tk.TaiKhoan1;
            Bode.PheDuyet = "Đang xử lý";
            Bode.ThoiGianMo = DateTime.Now;
            Bode.NguoiDuyet = admin.TaiKhoan1;
            db.SaveChanges();
          List<Bo_De> bo_Des = new Bode().ListALLChapterBM(tk.TaiKhoan1);

            var bode1 = (from n in bo_Des
                         select new
                         {
                             Ten = n.NoiDung,
                             MaDe = n.Ma_BoDe,
                             SoCau = n.CauHois.Count,
                             ThoiGian = n.ThoiGianThi,
                             TenMon = n.MonHoc.TenMon,
                             Giaovien = n.GiaoVien.TenGV,
                             Pheduyet = n.PheDuyet,
                             Trangthai =n.TrangThai,
                         }).ToList();
            return Json(new
            {
                Bode = bode1

            }, JsonRequestBehavior.AllowGet); ;


        }



    }
}