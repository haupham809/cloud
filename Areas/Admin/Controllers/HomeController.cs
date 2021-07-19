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
using EXCELL = Microsoft.Office.Interop.Excel;
using TracNghiemOnline.Controllers;
using System.IO;


namespace TracNghiemOnline.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        // GET: Admin/Home
        [HttpGet]
    

        public ActionResult Index()
        {
            reseach();
            return View();
          
        }
        public ActionResult ChonDe1(long id)
        {
            Session["mads"] = id;
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var ds = new TracNghiemOnlineDB().DSGV_ThucHien.Find(id);
            var dethi = new TracNghiemOnlineDB().Bo_De.Where(x=>x.TrangThai==true && x.Ma_Mon==ds.LichNop.MaMon && x.Ma_NguoiTao==session.TaiKhoan1).ToList();
            return View(dethi);
        }
        public void save_file_from_url(string file_name, string url)
        {
            byte[] content;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            WebResponse response = request.GetResponse();


            Stream stream = response.GetResponseStream();

            using (BinaryReader br = new BinaryReader(stream))
            {
                content = br.ReadBytes(500000);
                br.Close();
            }
            response.Close();

            FileStream fs = new FileStream(file_name, FileMode.Create);
            BinaryWriter bw = new BinaryWriter(fs);
            try
            {
                bw.Write(content);
            }
            finally
            {
                fs.Close();
                bw.Close();
            }
        }

        public string copyanh(string path)
        {

            DateTime aDateTime = DateTime.Now;
            int sas = Convert.ToInt32(aDateTime.Year * 12 * 30 * 24 * 60 * 60 + aDateTime.Month * 30 * 24 * 60 * 60 + aDateTime.Day * 24 * 60 * 60 + aDateTime.Hour * 60 * 60 + aDateTime.Minute * 60 + aDateTime.Second);
            string Filename = "Anh" + sas+".jpg";
            string saveLocation = "~/Content/Img/" + Filename;
            string file_name = Server.MapPath(saveLocation);
            if (path.Contains("http"))
            {

                save_file_from_url(file_name, path);
                

            }
            else
            {
                // path = Server.MapPath(path);
                string path1 = System.IO.Path.GetFullPath(path);
                Image png = Image.FromFile(path1);
                png.Save(file_name, System.Drawing.Imaging.ImageFormat.Jpeg);
                png.Dispose();

            }

            return "/Content/Img/" + Filename; ;
        }
        public JsonResult XuLyFile(HttpPostedFileBase file)
        {
            string strExtexsion = Path.GetExtension(file.FileName).Trim();
          
            List<Kho_CauHoi> cauHois = new List<Kho_CauHoi>(); 
            try
            {
                string path = Server.MapPath("~/Content/" + file.FileName);
                try
                {
                    if (System.IO.File.Exists(path))
                    {
                        System.IO.File.Delete(path);
                    }
                    file.SaveAs(path);
                }
                catch { }

                EXCELL.Application application = new EXCELL.Application();
                EXCELL.Workbook workbook = application.Workbooks.Open(path);
                EXCELL.Worksheet worksheet = workbook.ActiveSheet;


                EXCELL.Range range = worksheet.UsedRange;

                cauHois = new List<Kho_CauHoi>();

                for (int i = 2; i <= range.Rows.Count; i++)
                {
                    try
                    {
                        Kho_CauHoi cauHoi = new Kho_CauHoi();

                        cauHoi.NoiDung = ((EXCELL.Range)range.Cells[i, 1]).Text;

                        cauHoi.HinhAnh = ((EXCELL.Range)range.Cells[i, 8]).Text;


                        if (((EXCELL.Range)range.Cells[i, 7]).Text.Equals("1"))
                        {
                            cauHoi.MucDo = "Nhận Biết";
                        }
                        else if (((EXCELL.Range)range.Cells[i, 7]).Text.Equals("2"))
                        {
                            cauHoi.MucDo = "Thông Hiểu";
                        }
                        else if (((EXCELL.Range)range.Cells[i, 7]).Text.Equals("3"))
                        {
                            cauHoi.MucDo = "Vận Dụng";
                        }
                        else
                        {
                            cauHoi.MucDo = "Vận Dụng Cao";
                        }


                        cauHoi.Dap_AN = new List<Dap_AN>();
                        Dap_AN dapAn = new Dap_AN();
                        dapAn.NoiDung = ((EXCELL.Range)range.Cells[i, 2]).Text;
                        dapAn.HinhAnh = ((EXCELL.Range)range.Cells[i, 9]).Text;

                        if (((EXCELL.Range)range.Cells[i, 6]).Text.Equals("A"))
                        {
                            dapAn.TrangThai = true;
                        }
                        else { dapAn.TrangThai = false; }
                        cauHoi.Dap_AN.Add(dapAn);
                        dapAn = new Dap_AN();
                        dapAn.NoiDung = ((EXCELL.Range)range.Cells[i, 3]).Text;
                        dapAn.HinhAnh = ((EXCELL.Range)range.Cells[i, 10]).Text;

                        if (((EXCELL.Range)range.Cells[i, 6]).Text.Equals("B"))
                        {
                            dapAn.TrangThai = true;
                        }
                        else { dapAn.TrangThai = false; }
                        cauHoi.Dap_AN.Add(dapAn);
                        dapAn = new Dap_AN();
                        dapAn.NoiDung = ((EXCELL.Range)range.Cells[i, 4]).Text;
                        dapAn.HinhAnh = ((EXCELL.Range)range.Cells[i, 11]).Text;

                        if (((EXCELL.Range)range.Cells[i, 6]).Text.Equals("C"))
                        {
                            dapAn.TrangThai = true;
                        }
                        else { dapAn.TrangThai = false; }
                        cauHoi.Dap_AN.Add(dapAn);

                        dapAn = new Dap_AN();
                        dapAn.NoiDung = ((EXCELL.Range)range.Cells[i, 5]).Text;
                        dapAn.HinhAnh = ((EXCELL.Range)range.Cells[i, 12]).Text;

                        if (((EXCELL.Range)range.Cells[i, 6]).Text.Equals("D"))
                        {
                            dapAn.TrangThai = true;
                        }
                        else { dapAn.TrangThai = false; }
                        cauHoi.Dap_AN.Add(dapAn);

                        cauHois.Add(cauHoi);
                    }
                    catch
                    {

                    }
                }


                application.Workbooks.Close();
                try
                {
                    System.IO.File.Delete(path);
                }
                catch { }

                foreach (var item in cauHois)
                {
                    try
                    {
                        if (item.HinhAnh.Length > 0)
                        {
                            item.HinhAnh = copyanh(item.HinhAnh.Trim());
                        }
                        foreach (var item1 in item.Dap_AN)
                        {

                            if (item1.HinhAnh.Length > 0)
                            {
                                item1.HinhAnh = copyanh(item1.HinhAnh.Trim());

                            }
                        }
                    }
                    catch (Exception e)
                    {
                     
                    }
                }
                Session[ComMon.ComMonStants.Cauhoi] = cauHois;
                return Json(new
                {
                    status = true
                    
                });


            }
            catch
            {
                return Json(new
                {
                    status = false
                });
            }
        }
        public string xuatpdf(long id, string tenmon)
        {
            PdfDocument pdf = new PdfDocument();
            PdfPageBase page = pdf.Pages.Add();
            PdfTrueTypeFont font1 = new PdfTrueTypeFont(new Font("Arial Unicode MS", 24f, FontStyle.Bold), true);
            PdfStringFormat centerAlignment = new PdfStringFormat(PdfTextAlignment.Center);
            PdfTrueTypeFont font2 = new PdfTrueTypeFont(new Font("Arial Unicode MS", 13f, FontStyle.Regular), true);

            PdfPen pen1 = new PdfPen(Color.Black, 1f);
            page.Canvas.DrawRectangle(pen1, new Rectangle(new Point(390, 150), new Size(120, 40)));
            PdfPen pen2 = new PdfPen(Color.Black, 1f);


            page.Canvas.DrawRectangle(pen1, new Rectangle(new Point(0, 0), new Size((int)page.GetClientSize().Width, (int)page.GetClientSize().Height)));
            page.Canvas.DrawString("TRƯỜNG ĐẠI HỌC GIAO THÔNG VẬN TẢI PHÂN HIỆU TẠI TP HỒ CHÍ MINH", font1, new PdfSolidBrush(Color.Black), new RectangleF(10, 50, page.GetClientSize().Width - 10, page.GetClientSize().Height), centerAlignment);
            page.Canvas.DrawString("Đề thi môn:" + tenmon, new PdfTrueTypeFont(new Font("Arial Unicode MS", 15f, FontStyle.Bold), true), new PdfSolidBrush(Color.Black), new RectangleF(40, 160, page.GetClientSize().Width / 2 - 40 - 10, page.GetClientSize().Height), new PdfStringFormat() { LineLimit = true });
            page.Canvas.DrawString("Mã đề:" + id, new PdfTrueTypeFont(new Font("Arial Unicode MS", 15f, FontStyle.Bold), true), new PdfSolidBrush(Color.Black), new RectangleF(400, 160, page.GetClientSize().Width / 2 - 400 - 10, page.GetClientSize().Height), new PdfStringFormat() { LineLimit = true });
            var khoch = new Bode().listkhocauhoi(id);
            int vitridong = 220;
            int slcau = 0;
            int x = 50;
            int y = (int)(page.GetClientSize().Width) / 2 + 50;
            string[] cau = { "A", "B", "C", "D" };

            foreach (var item in khoch)
            {

                slcau++;
                vitridong += 13;
                page.Canvas.DrawString("Câu " + slcau + ":", font2, new PdfSolidBrush(Color.Blue), new RectangleF(20, vitridong, page.GetClientSize().Width - 90, page.GetClientSize().Height), new PdfStringFormat() { LineLimit = true });
                page.Canvas.DrawString(item.NoiDung, font2, new PdfSolidBrush(Color.Black), new RectangleF(20 + 55, vitridong, page.GetClientSize().Width - 90, page.GetClientSize().Height), new PdfStringFormat() { LineLimit = true });
                vitridong += (int)((("Câu " + slcau + ":" + item.NoiDung).Length * 13) / (page.GetClientSize().Width - 40)) * 13;
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
                    vtdongdapan[slda] = (int)(((cau[slda] + ":" + da.NoiDung).Length * 13) / (page.GetClientSize().Width / 2)) * 13;
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
                    if (da.TrangThai == false) { page.Canvas.DrawString(cau[slda] + ":", font2, new PdfSolidBrush(Color.Blue), new RectangleF(z - 10, vitridong, page.GetClientSize().Width / 2 - 20, page.GetClientSize().Height - 80), new PdfStringFormat() { LineLimit = true }); }
                    else { page.Canvas.DrawString(cau[slda] + ":", font2, new PdfSolidBrush(Color.Red), new RectangleF(z - 10, vitridong, page.GetClientSize().Width / 2 - 20, page.GetClientSize().Height - 80), new PdfStringFormat() { LineLimit = true }); }

                    page.Canvas.DrawString(da.NoiDung, font2, new PdfSolidBrush(Color.Black), new RectangleF(z + 5, vitridong, page.GetClientSize().Width / 2 - 20, page.GetClientSize().Height - 80), new PdfStringFormat() { LineLimit = true });


                    if (da.HinhAnh != null)
                    {
                        if (13 + vtdongdapan[slda] + vitridong + 150 > page.GetClientSize().Height - 20)
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
                        if (vtdongdapan[0] > vtdongdapan[1])
                            vitridong += vtdongdapan[0];
                        else vitridong += vtdongdapan[1];
                        vitridong += k;
                        k = 0;
                    }
                    else if (slda == 3)
                    {
                        if ((int)(((cau[slda] + ":" + da.NoiDung).Length * 13) / page.GetClientSize().Width) * 13 < 1) vitridong += 13;
                        if (vtdongdapan[2] > vtdongdapan[3])
                            vitridong += vtdongdapan[2];
                        else vitridong += vtdongdapan[3];
                        vitridong += k;
                    }



                    if (vitridong > page.GetClientSize().Height - 20)
                    {
                        vitridong = 20;
                        page = pdf.Pages.Add();
                        page.Canvas.DrawRectangle(pen1, new Rectangle(new Point(0, 0), new Size((int)page.GetClientSize().Width, (int)page.GetClientSize().Height)));
                    }
                    slda++;
                }

            }
            page.Canvas.SetTransparency(0.2f);
            page.Canvas.DrawString("Hết", font1, new PdfSolidBrush(Color.Black), new RectangleF(20, page.GetClientSize().Height - 30, page.GetClientSize().Width, page.GetClientSize().Height), centerAlignment);
            page.Canvas.SetTransparency(1f);
            pdf.SaveToFile(Server.MapPath("~/Content/" + "De_" + id + ".pdf"));
            pdf.Close();

            return "/Content/" + "De_" + id + ".pdf";



        }



        public ActionResult Menull()
        {
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
           
            if (session.ChưcVu.Equals("Admin"))
            {
              
                
                return PartialView();
            }
            else if (session.ChưcVu.Equals("BoMon"))
            {
                return View("MenuBM");
            }

            return View("menulCanBo");         
        }
        public ActionResult BoMon()
        {
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];

            var ds = new TracNghiemOnlineDB();
            foreach (var item in new TracNghiemOnlineDB().LichNops.Where(x=>x.ThoiGian<DateTime.Now))
            {
                var lich = ds.LichNops.Find(item.id);
              
                ds.SaveChanges();
                foreach (var item1 in new TracNghiemOnlineDB().DSGV_ThucHien.Where(X=>X.MaDE==null && X.MaLich==item.id))
                {
                    var gv = ds.DSGV_ThucHien.Find(item1.id);
                    gv.trangthai = "Đã hết hạn nộp";
                    ds.SaveChanges();
                }

            }
            return View();
        }

        public JsonResult DanhGia(string id)
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
                  //  danh_Gia.SoCauDung = item1.SoCauDung;
                  //  danh_Gia.TongCau += item1.TongCau; ;
                  //  danh_Gia.DanhGia = "" + ((double)(item1.SoCauDung / item1.TongCau) * (double)100);
                    DanhGia.Add(danh_Gia);
                }
                break;
            }
            for (int i = 0; i < Ketqua.Count; i++)
            {
                foreach (var item1 in DanhGia)
                {
                    foreach (var item in Ketqua[i].ketQuaThi.De_Thi.Danh_Gia)
                    {
                        if (item1.MaChuong == item.MaChuong)
                        {
                            item1.SoCauDung += item.SoCauDung;
                            item1.TongCau += item.TongCau;
                            item1.DanhGia = "" + ((double)(item1.SoCauDung / item1.TongCau) * (double)100);
                        }

                    }

                }

            }
           
            ViewBag.DanhGia = DanhGia;


            var arr = from c in DanhGia
                      select new{ 
                    ten=  c.Chuong_Hoc.TenChuong,
                    gt=  c.DanhGia

                      } ;
            
            return Json(new
            {
                mang=arr
        
            },JsonRequestBehavior.AllowGet);

        }
        public ActionResult PhongThi()
        {
            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var phong = db.Phong_Thi.Where(x => !x.TrangThai.Equals("Đã Đóng") && x.Xoa == true && (x.MaCanBo1==session.TaiKhoan1|| x.MaCanBo2==session.TaiKhoan1) && x.MaBoDe!=null).ToList();
            foreach (var item in phong)
            {
             if (item.ThoiGianDong <= DateTime.Now)
                {
                    var p = db.Phong_Thi.Find(item.MaPhong);
                    p.TrangThai = "Đã Đóng";
                    db.SaveChanges();

                }

            }
            phong = db.Phong_Thi.Where(x => !x.TrangThai.Equals("Đã Đóng") && x.Xoa == true && (x.MaCanBo1 == session.TaiKhoan1 || x.MaCanBo2 == session.TaiKhoan1) && x.MaBoDe != null).ToList();
            return View(phong);
        }
        public JsonResult Update(long maDe,bool trangthai)
        {
            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            var Bode = db.Bo_De.Find(maDe);
            //  Bode.TrangThai = trangthai;
            Bode.TrangThai = false;
            db.SaveChanges();
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            List<Bo_De> bo_Des = new BoDeDao().ListALLChapterStudy();
          //  if (session.ChưcVu.Equals("Cán Bộ"))
          //  {
                bo_Des = new TracNghiemOnlineDB().Bo_De.Where(x => x.Ma_NguoiTao == session.TaiKhoan1 && x.Xoa == true).ToList();
          //  }
            
            var bode1 = (from n in bo_Des
                         select new
                         {
                             Ten = n.NoiDung,
                             MaDe = n.Ma_BoDe,
                             SoCau = n.SoCau,
                             ThoiGian = n.ThoiGianThi,
                             TenMon = n.MonHoc.TenMon,
                             Giaovien = n.GiaoVien.TenGV,
                             TrangThai = n.TrangThai,
                         }).ToList();
            return Json(new
            {
                Bode = bode1

            }, JsonRequestBehavior.AllowGet); 
        }

        public JsonResult UpdateDethi(long maDe ,string nd,string tg,bool xoa )
        {
            
        
            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            var Bode = db.Bo_De.Find(maDe); 
            Bode.ThoiGianThi = tg;
            Bode.NoiDung = nd;
            Bode.Xoa = xoa;
            db.SaveChanges();
                    var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            List<Bo_De> bo_Des = new BoDeDao().ListALLChapterStudy();
          //  if (session.ChưcVu.Equals("Cán Bộ"))
          //  {
                bo_Des = new TracNghiemOnlineDB().Bo_De.Where(x => x.Ma_NguoiTao == session.TaiKhoan1 && x.Xoa==true).ToList();
          //  }
            
            var bode1 = (from n in bo_Des
                         select new
                         {
                             Ten = n.NoiDung,
                             MaDe = n.Ma_BoDe,
                             SoCau = n.CauHois.Count,
                             ThoiGian = n.ThoiGianThi,
                             TenMon = n.MonHoc.TenMon,
                             Giaovien=n.GiaoVien.TenGV,
                             TrangThai=n.TrangThai,
                         }).ToList();
            return Json(new
            {
                Bode = bode1

            }, JsonRequestBehavior.AllowGet); ;

        }

        public ActionResult DSSVThi(string id)
        {
            

            Phong_Thi phong_Thi = new QuanLyThiDAO().ExamitionRoom(id);
            return View(phong_Thi);

        }



        public void reseach()
        {
            Model.BoDeThi boDeThi = new BoDeThi();
            bo_De1.Ma_Mon = 0;
            bo_De1.ThoiGianThi = "";
            boDeThi.LoaiDe1 = "";
            boDeThi.BoDeThi1 = bo_De1;
            Session[ComMon.ComMonStants.ChapterStudy] = boDeThi;
        }
        public ActionResult DSDETHI(string id)
        {
            reseach();
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
           List<Bo_De> bo_Des = new BoDeDao().ListALLChapterStudy();
            //  if (session.ChưcVu.Equals("Cán Bộ"))
            // {
            // bo_Des = new TracNghiemOnlineDB().Bo_De.Where(x =>( x.PheDuyet==null || x.PheDuyet.Contains("Từ"))&& x.Ma_NguoiTao  == session.TaiKhoan1 && x.Xoa==true).ToList();
            // }
            bo_Des = new TracNghiemOnlineDB().Bo_De.Where(x =>x.Ma_NguoiTao == session.TaiKhoan1 && x.Xoa == true && x.TrangThai==false).ToList();

            return View(bo_Des);
        }
      
        public ActionResult TaoDeThi()
        {
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var gv = new TracNghiemOnlineDB().GiaoViens.Find(session.TaiKhoan1);
            var dao = new TracNghiemOnline.Modell.TracNghiemOnlineDB().MonHocs.Where(x => x.MaBoMon.Equals(gv.MaBoMon)).ToList();
            ViewBag.MonHoc = dao;
            ViewBag.A ="";
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
            dethi.LoaiDe1=id;
            Session[ComMon.ComMonStants.ChapterStudy] = dethi;
            return Content("");
        }

       public void ChonCauHoi(String CauHoi)
        {


            var khocauhoi = new JavaScriptSerializer().Deserialize<List<Kho_CauHoi>>(CauHoi);
            var sessetion = (BoDeThi)Session[ComMon.ComMonStants.ChapterStudy];
            var bo_De = sessetion.BoDeThi1;
            foreach (var item in khocauhoi)
            {
                Modell.CauHoi cau = new Modell.CauHoi();
                cau.Ma_CauHoi = item.Ma_CauHoi;
                cau.Ma_BoDe = bo_De.Ma_BoDe;
                cau.Kho_CauHoi = new CauHoiDao().Question(item.Ma_CauHoi);
                bo_De.CauHois.Add(cau);

            }
            sessetion.BoDeThi1 = bo_De;
            Session[ComMon.ComMonStants.ChapterStudy] = sessetion;


        }

        private static Bo_De bo_De1=new Bo_De();
        [HttpPost]
        public ActionResult MonHoc(Bo_De bo_De)
        {
            var dethi = (Model.BoDeThi)Session[ComMon.ComMonStants.ChapterStudy];
            if (ModelState.IsValid && dethi.BoDeThi1.ThoiGianThi.Length>0 && dethi.LoaiDe1.Length>0 && dethi.BoDeThi1.Ma_Mon>0)
            {
                bo_De.ThoiGianThi = dethi.BoDeThi1.ThoiGianThi;
                bo_De.Ma_Mon = dethi.BoDeThi1.Ma_Mon;

                bo_De.MonHoc = new MonHocDao().Subject(long.Parse(bo_De.Ma_Mon.ToString()));
                dethi.BoDeThi1 = bo_De;
                Session[ComMon.ComMonStants.ChapterStudy] = dethi;
                
               if(dethi.LoaiDe1.Equals("Tự Chọn"))
                {
                    List<Kho_CauHoi> kho_CauHois = new List<Kho_CauHoi>();
                    foreach (var item in new MonHocDao().ListChapterStudy(long.Parse(dethi.BoDeThi1.Ma_Mon.ToString()))) 
                    {
                        kho_CauHois.AddRange(new CauHoiDao().ListQuestion(long.Parse(item.Ma_Chuong.ToString())));
                    }


                    ViewBag.Question = kho_CauHois;
                    return View("ChonCauhoi");

                }
                else {
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
                if (dethi.BoDeThi1.Ma_Mon<= 0)
                {
                    mess1 = "Bạn Vui Lòng Chọn Môn Học ";
                }
                if (dethi.LoaiDe1.Length<= 0)
                {
                    mess2 = "Bạn Vui Lòng Chọn Cách Tạo Đề";
                }
                ViewBag.A = mess;
                ViewBag.B = mess1;
            var dao = new TracNghiemOnline.Modell.TracNghiemOnlineDB().MonHocs.Select(x => x).ToList();
            ViewBag.MonHoc = dao;
                ViewBag.C = mess2;

            }
            reseach();

            return View("TaoDeThi");
      
        }

        public void GuiboMon(long id)
        {

            long id1 = (long)Session["mads"];
            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            var bode = db.DSGV_ThucHien.Find(id1);
            bode.MaDE = id;
            var lich = db.LichNops.Find(bode.MaLich);
            try
            {
                if (DateTime.UtcNow > lich.ThoiGian)
                {
                    bode.trangthai = "Nộp Muộn";
                }
                else
                {
                    bode.trangthai = "Đang xử lý";
                }
            }
            catch {
                bode.trangthai = "Đang xử lý";
            }

            bode.NgayNop = DateTime.Now;
            Session["mads"]=null;
            db.SaveChanges();
        }
        public void HuyGui(long id)
        {
            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            var bode = db.Bo_De.Find(id);
            if (!bode.PheDuyet.Contains("Đã"))
            {
                bode.TrangThai = false;
                bode.PheDuyet = null;
                bode.NguoiDuyet = null;
                db.SaveChanges();
            }
        }

        public ActionResult LoadDeThi(string id)
        {

            try
            {
                if (id.Length > 0)
                {
                    var BODETHI = (Model.BoDeThi)Session[ComMon.ComMonStants.ChapterStudy];
                    BODETHI.BoDeThi1 = new BoDeDao().ChapterStudy(long.Parse(id));
                    Session[ComMon.ComMonStants.ChapterStudy] = BODETHI;
                  
                    ViewBag.Mess = id;
                    ViewBag.linkpdf = xuatpdf(long.Parse(id), Request["tenmon"]);

                }
                else
                {
                    ViewBag.Mess = "";

                }
            }
            catch
            {
                ViewBag.Mess = "";
            }
            ViewBag.Mess = id;
               var dethi = (Model.BoDeThi)Session[ComMon.ComMonStants.ChapterStudy];

            return View(dethi.BoDeThi1);
        }
        public ActionResult DeOnTap(long id)
        {
            var bodeontap = (BoDeOnTap)Session[ComMon.ComMonStants.OnTap];
            bodeontap.MaBoDe = id;
            Session[ComMon.ComMonStants.OnTap] = bodeontap;
            var bode = new TracNghiemOnlineDB().Bo_De.Where(x => x.Ma_BoDe == id).ToList()[0];
            ViewBag.MaLop = bodeontap.MaLopHP;
            return View(bode);
        }


        public void ThemDeOn1(string nd, string tg, string malop, DateTime bd)
        {

            var Lop = new TracNghiemOnlineDB().LopHocPhans.Find(malop);
            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            List<Kho_CauHoi> cauHois = (List<Kho_CauHoi>)Session[ComMon.ComMonStants.Cauhoi];
            foreach (var item in cauHois)
            {
                item.Ma_CauHoi = new CauHoiDao().CreatrQuestion(item);
            }
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            Bo_De bode = new Bo_De();
            bode.NoiDung = nd;
            bode.TrangThai = false;
            bode.ThoiGianThi = tg;
            bode.Ma_Mon = Lop.MaMon;
            bode.Ma_NguoiTao = session.TaiKhoan1;
            db.Bo_De.Add(bode);
            db.SaveChanges();


            db = new TracNghiemOnlineDB();
            var Bode = new TracNghiemOnlineDB().Bo_De.Where(x => x.TrangThai == false && x.Ma_NguoiTao.Equals(session.TaiKhoan1)).ToList().Last();
            BoDeOnTap boDeOn = new BoDeOnTap();
            boDeOn.MaLopHP = malop;
            boDeOn.MaBoDe = Bode.Ma_BoDe;
            boDeOn.ThoiGianDong = bd;
            db.BoDeOnTaps.Add(boDeOn);
            db.SaveChanges();
            foreach (var item in cauHois)
            {
                Modell.CauHoi cauHoi = new Modell.CauHoi();
                cauHoi.Ma_BoDe = Bode.Ma_BoDe;
                cauHoi.Ma_CauHoi = item.Ma_CauHoi;
                db.CauHois.Add(cauHoi);
                db.SaveChanges();
            }

            cauHois = new List<Kho_CauHoi>();
            Session[ComMon.ComMonStants.Cauhoi] = cauHois;
            //  db.BoDeOnTaps.Add(deontap);
        }
        public void ThemDeOn(string nd,string tg,string malop)
        {
            var Lop = new TracNghiemOnlineDB().LopHocPhans.Find(malop);

            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            List<Kho_CauHoi> cauHois = (List<Kho_CauHoi>)Session[ComMon.ComMonStants.Cauhoi];
            foreach (var item in cauHois)
            {
              item.Ma_CauHoi=new CauHoiDao().CreatrQuestion(item);
            }
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            Bo_De  bode = new Bo_De();
            bode.NoiDung = nd;
            bode.TrangThai = false;
            bode.ThoiGianThi = tg;
            bode.Ma_Mon = Lop.MaMon;
            bode.Ma_NguoiTao = session.TaiKhoan1;
            db.Bo_De.Add(bode);
            db.SaveChanges();

          
            db = new TracNghiemOnlineDB();
            var Bode = new TracNghiemOnlineDB().Bo_De.Where(x => x.TrangThai==false && x.Ma_NguoiTao.Equals(session.TaiKhoan1)).ToList().Last();
            BoDeOnTap boDeOn = new BoDeOnTap();
            boDeOn.MaLopHP = malop;
            boDeOn.MaBoDe = Bode.Ma_BoDe;
            db.BoDeOnTaps.Add(boDeOn);
            db.SaveChanges();
            foreach (var item in cauHois)
            {          
                Modell.CauHoi cauHoi = new Modell.CauHoi();
                cauHoi.Ma_BoDe = Bode.Ma_BoDe;
                cauHoi.Ma_CauHoi = item.Ma_CauHoi;
                db.CauHois.Add(cauHoi);
                db.SaveChanges();
            }
            
            cauHois = new List<Kho_CauHoi>();
            Session[ComMon.ComMonStants.Cauhoi] = cauHois;
          //  db.BoDeOnTaps.Add(deontap);
        }
        public void TaoDeOn(string nd, string tg, string malop, DateTime TGBD ,DateTime TGKT)
        {
            var Lop = new TracNghiemOnlineDB().LopHocPhans.Find(malop);
            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
            List<Kho_CauHoi> cauHois = (List<Kho_CauHoi>)Session[ComMon.ComMonStants.Cauhoi];
            foreach (var item in cauHois)
            {
                item.Ma_CauHoi = new CauHoiDao().CreatrQuestion(item);
            }
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            Bo_De bode = new Bo_De();
            bode.NoiDung = nd;
            bode.TrangThai = false;
            bode.ThoiGianThi = tg;
            bode.Ma_Mon = Lop.MaMon;
            bode.Ma_NguoiTao = session.TaiKhoan1;
            db.Bo_De.Add(bode);
            db.SaveChanges();


            db = new TracNghiemOnlineDB();
            var Bode = new TracNghiemOnlineDB().Bo_De.Where(x => x.TrangThai == false && x.Ma_NguoiTao.Equals(session.TaiKhoan1)).ToList().Last();
            BoDeOnTap boDeOn = new BoDeOnTap();
            boDeOn.MaLopHP = malop;
            boDeOn.MaBoDe = Bode.Ma_BoDe;
            boDeOn.ThoiGianMo = TGBD;
            boDeOn.ThoiGianDong = TGKT;
            db.BoDeOnTaps.Add(boDeOn);
            db.SaveChanges();
            



            foreach (var item in cauHois)
            {
                Modell.CauHoi cauHoi = new Modell.CauHoi();
                cauHoi.Ma_BoDe = Bode.Ma_BoDe;
                cauHoi.Ma_CauHoi = item.Ma_CauHoi;
                db.CauHois.Add(cauHoi);
                db.SaveChanges();
            }

            cauHois = new List<Kho_CauHoi>();
            Session[ComMon.ComMonStants.Cauhoi] = cauHois;

        }

        public ActionResult AddChapterStudy()
        {
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var dethi = (Model.BoDeThi)Session[ComMon.ComMonStants.ChapterStudy];
            dethi.BoDeThi1.Ma_NguoiTao = session.TaiKhoan1;
            new BoDeDao().CreateChapterStudy(dethi.BoDeThi1);
            return RedirectToAction("DSDETHI","Home");
        }

        public ActionResult DSDeOn(string id)
        {
            TracNghiemOnlineDB db = new TracNghiemOnlineDB();
           var bode = db.BoDeOnTaps.Where(x => x.MaLopHP.Equals(id)).ToList();
            ViewBag.Malop = id;
            List<Kho_CauHoi> cauHois = (List<Kho_CauHoi>)Session[ComMon.ComMonStants.Cauhoi];
            try
            {
                if (cauHois.Count == 0)
                {
                    cauHois = new List<Kho_CauHoi>();
                }
            }
            catch { cauHois = new List<Kho_CauHoi>(); }
            ViewBag.DeOn = cauHois;
            return View(bode);
        }

        public ActionResult NgânHangCauHoi(string id)
        {
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            var canbo = new TracNghiemOnlineDB().GiaoViens.Find(session.TaiKhoan1);

            reseach();
            return View(new TracNghiemOnlineDB().MonHocs.Where(x=>x.MaBoMon.Equals(canbo.MaBoMon)).ToList());
        }

        public ActionResult ChuongHoc(string id)
        {
            bool check = true;
            var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];
            Session[ComMon.ComMonStants.Cauhoi]= null;
            if (session.ChưcVu.Equals("BoMon"))
            {
                check = false;
            }
            ViewBag.id = id;
            ViewBag.check = check;
            reseach();
            List<Chuong_Hoc> chuong_Hocs = new MonHocDao().ListChapterStudy(long.Parse(id));
            return View(chuong_Hocs);
        }
        public JsonResult TaoDe( string SoLuong)
        {
            var soluong = new JavaScriptSerializer().Deserialize<List<SoLuongChuong>>(SoLuong);
            var sessetion = (BoDeThi)Session[ComMon.ComMonStants.ChapterStudy];
          var  bo_De1 = sessetion.BoDeThi1;
            new CauHoiDao().CreateTopic(bo_De1,soluong);
            return Json(new
            {
                status = true
            }) ;
        } 
    }
}