using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using TracNghiemOnline.Controllers;
using TracNghiemOnline.Model;
using TracNghiemOnline.Modell;
using TracNghiemOnline.Modell.Dao;
using EXCELL = Microsoft.Office.Interop.Excel;

using System.Drawing;
using System.IO;
using System.Net;
using Spire.Doc;
using Spire.Doc.Fields;
using Spire.Doc.Documents;
using System.Web.Script.Serialization;
using Spire.Doc.Fields.OMath;


// GUI ROI CHAY LUON NHÉ //DAU ROI nang qua gui chua chay qua xong // okey
namespace TracNghiemOnline.Areas.Admin.Controllers
{
    public class CauHoiController : BaseController
    {
        // GET: Admin/CauHoi
        // static List<Kho_CauHoi> cauHois=new List<Kho_CauHoi>();
        public ActionResult Index(string id)
        {
            ViewBag.MaChuong = id;
            List<Kho_CauHoi> kho_CauHois = new CauHoiDao().ListQuestion(long.Parse(id));

            return View(kho_CauHois);

        }
        public ActionResult DapAn(long? id)
        {

            return View(new TracNghiemOnlineDB().Kho_CauHoi.Where(x => x.Ma_CauHoi == id).ToList());
        }

        public ActionResult CreateCauHoi(long id)
        {
            ViewBag.MaChuong = id;
            List<Kho_CauHoi> cauHois = (List<Kho_CauHoi>)Session[ComMon.ComMonStants.Cauhoi];
            try
            {
                if (cauHois.Count == 0)
                {
                    cauHois = new List<Kho_CauHoi>();
                }


            }
            catch
            {
                cauHois = new List<Kho_CauHoi>();
            }
            return View(cauHois);
        }

        public ActionResult LoadQuestion()
        {
            List<Kho_CauHoi> cauHois = (List<Kho_CauHoi>)Session[ComMon.ComMonStants.Cauhoi];
            foreach (var item in cauHois)
            {

                new CauHoiDao().CreatrQuestion(item);
            }
            //cauHois = new List<Kho_CauHoi>();
            Session[ComMon.ComMonStants.Cauhoi] = null;
            return RedirectToAction("Index/" + cauHois[0].Ma_Chuong, "CauHoi");
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
            string Filename = "Anh" + sas + ".jpg";
            string saveLocation = "~/Content/Img/" + Filename;
            string file_name = Server.MapPath(saveLocation);
            if (path.Contains("https"))
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

            return "/Content/Img/" + Filename;
        }
        public void LuuCauHoi(string listCH)
        {
            var lisch = new JavaScriptSerializer().Deserialize<List<Kho_CauHoi>>(listCH);
            foreach (Kho_CauHoi item in lisch)
            {
                if (item.NoiDung.Contains("$c$4"))
                {
                    item.NoiDung = item.NoiDung.Substring(4);
                    item.MucDo = "Vận Dụng Cao";
                }
                else if (item.NoiDung.Contains("$c$3"))
                {
                    item.NoiDung = item.NoiDung.Substring(4);
                    item.MucDo = "Vận Dụng";
                }
                else if (item.NoiDung.Contains("$c$2"))
                {
                    item.NoiDung = item.NoiDung.Substring(4);
                    item.MucDo = "Thông Hiểu";
                }
                else
                {
                    item.NoiDung = item.NoiDung.Substring(4);
                    item.MucDo = "Nhận Biết";
                }
                foreach (var item1 in item.Dap_AN)
                {
                    if (item1.NoiDung.Contains("$*$"))
                    {
                        item1.NoiDung = item1.NoiDung.Substring(3);
                        item1.TrangThai = true;
                    }
                    else
                    {
                        item1.TrangThai = false;
                    }

                }
            }
            if (lisch[lisch.Count - 1].NoiDung == null)
            {
                lisch.RemoveAt(lisch.Count - 1);
            }
            Session[ComMon.ComMonStants.Cauhoi] = lisch;

        }

        public void UpdateCauHoi(string listCH)
        {

            var lisch = new JavaScriptSerializer().Deserialize<List<Kho_CauHoi>>(listCH);
            foreach (Kho_CauHoi item in lisch)
            {
                TracNghiemOnlineDB db = new TracNghiemOnlineDB();
                var ch = db.Kho_CauHoi.Find(item.Ma_CauHoi);
                if (item.NoiDung.Contains("$c$4"))
                {
                    item.NoiDung = item.NoiDung.Substring(4);
                    item.MucDo = "Vận Dụng Cao";
                }
                else if (item.NoiDung.Contains("$c$3"))
                {
                    item.NoiDung = item.NoiDung.Substring(4);
                    item.MucDo = "Vận Dụng";
                }
                else if (item.NoiDung.Contains("$c$2"))
                {
                    item.NoiDung = item.NoiDung.Substring(4);
                    item.MucDo = "Thông Hiểu";
                }
                else
                {
                    item.NoiDung = item.NoiDung.Substring(4);
                    item.MucDo = "Nhận Biết";
                }
                ch.NoiDung = item.NoiDung;
                ch.HinhAnh = item.HinhAnh;
                ch.MucDo = item.MucDo;
                db.SaveChanges();
                foreach (var item1 in item.Dap_AN)
                {
                    var dap = db.Dap_AN.Find(item1.MA_DAN);
                    if (item1.NoiDung.Contains("$*$"))
                    {
                        item1.NoiDung = item1.NoiDung.Substring(3);
                        item1.TrangThai = true;
                    }
                    else
                    {
                        item1.TrangThai = false;
                    }
                    dap.NoiDung = item1.NoiDung;
                    dap.HinhAnh = item1.HinhAnh;
                    dap.TrangThai = item1.TrangThai;
                    db.SaveChanges();
                }
            }
            if (lisch[lisch.Count - 1].NoiDung == null)
            {
                lisch.RemoveAt(lisch.Count - 1);
            }
            Session[ComMon.ComMonStants.Cauhoi] = lisch;

        }

        public ActionResult LoadCauHoi(long? id)
        {
            List<Kho_CauHoi> cauHois = (List<Kho_CauHoi>)Session[ComMon.ComMonStants.Cauhoi];
            foreach (var item in cauHois)
            {
                item.Ma_Chuong = id;
            }
            Session[ComMon.ComMonStants.Cauhoi] = cauHois;
            ViewBag.cauhoi = cauHois;
            return View();
        }
        public ActionResult ChonMucDo(string MucDo)
        {
            List<Kho_CauHoi> cauHois = (List<Kho_CauHoi>)Session[ComMon.ComMonStants.Cauhoi];
            foreach (var item in cauHois)
            {
                item.MucDo = MucDo;
            }


            return Content("");
        }
        public JsonResult Saveanh(HttpPostedFileBase file)
        {
            string path = "";
            string strExtexsion = Path.GetFileName(file.FileName).Trim();
            path = Server.MapPath("~/Content/" + file.FileName);
            try
            {
                if (System.IO.File.Exists(path))
                {
                    System.IO.File.Delete(path);
                }
                file.SaveAs(path);
            }
            catch { }
            path = "/Content/" + file.FileName;
            return Json(new { path }, JsonRequestBehavior.AllowGet);
        }
        //public JsonResult XuLyFile(HttpPostedFileBase file)
        //{
        //    List<Kho_CauHoi> cauHois = new List<Kho_CauHoi>();
        //    var session = (TaiKhoan)Session[ComMon.ComMonStants.UserLogin];


        //    object path = Server.MapPath("~/Content/" + file.FileName);
        //    if (System.IO.File.Exists(path.ToString()))
        //    {
        //        System.IO.File.Delete(path.ToString());
        //    }

        //    file.SaveAs(path.ToString());
        //    List<Kho_CauHoi> cauhoi = new List<Kho_CauHoi>();


        //    Document document = new Document(path.ToString());

        //    int sas = 1;
        //    Section section = document.Sections[0];
        //    if (section.Tables.Count > 0)
        //    {
        //        Table table = section.Tables[0] as Table;
        //        for (int i = 0; i < table.Rows.Count; i++)
        //        {
        //            Kho_CauHoi cau = new Kho_CauHoi();

        //            for (int j = 0; j < table.Rows[i].Cells.Count; j++)
        //            {

        //                foreach (Paragraph paragraph in table.Rows[i].Cells[j].Paragraphs)
        //                {
        //                    string noidung = "";
        //                    Dap_AN da = new Dap_AN();
        //                    //Get Each Document Object of Paragraph Items

        //                    foreach (DocumentObject docObject in paragraph.ChildObjects)
        //                    {

        //                        //If Type of Document Object is Picture, Extract.  
        //                        if (docObject.DocumentObjectType == DocumentObjectType.Picture)
        //                        {
        //                            String anh1 = null;
        //                            string s = session.TaiKhoan1 + "-" + sas + DateTime.Now.ToString("yyyyMMddHHmmss") + ".png";
        //                            DocPicture pic = docObject as DocPicture;
        //                            String imgName = Server.MapPath("~/Content/Img/" + s);
        //                            anh1 = "/Content/Img/" + s;
        //                            //Save Image  
        //                            pic.Image.Save(imgName, System.Drawing.Imaging.ImageFormat.Png);
        //                            noidung = noidung + "   <img src='" + anh1 + "'>  ";

        //                            sas++;
        //                        }
        //                        else if (docObject.DocumentObjectType == DocumentObjectType.TextRange)
        //                        {

        //                            TextRange nd = docObject as TextRange;

        //                            noidung = noidung + nd.Text;




        //                        }
        //                        else if (docObject.DocumentObjectType == DocumentObjectType.OfficeMath)
        //                        {

        //                            noidung = noidung + (docObject as OfficeMath).ToMathMLCode().Replace("mml:", "");

        //                        }




        //                    }
        //                    if (j == 0)
        //                    {
        //                        cau.NoiDung = noidung;

        //                    }
        //                    else if (j == 1)
        //                    {
        //                        if (noidung.Contains(1.ToString()))
        //                        {
        //                            cau.MucDo = "Nhận Biết";
        //                        }
        //                        else if (noidung.Contains(2.ToString()))
        //                        {
        //                            cau.MucDo = "Thông Hiểu";
        //                        }
        //                        else if (noidung.Contains(3.ToString()))
        //                        {
        //                            cau.MucDo = "Vận Dụng";

        //                        }
        //                        else if (noidung.Contains(4.ToString()))
        //                        {
        //                            cau.MucDo = "Vận Dụng Cao";
        //                        }


        //                    }
        //                    else if (j > 1)
        //                    {
        //                        if (!noidung.ToString().Equals(""))
        //                        {
        //                            da.NoiDung = noidung.ToString();


        //                            if (noidung.Substring(0, noidung.ToString().IndexOf("*") + 1).Replace(" ", "").Equals("*"))
        //                            {
        //                                da.NoiDung = noidung.ToString().Substring(noidung.ToString().IndexOf("*") + 1, noidung.ToString().Length - noidung.ToString().IndexOf("*") - 1);
        //                                da.TrangThai = true;
        //                            }
        //                            else
        //                            {

        //                                da.TrangThai = false;
        //                            }


        //                            cau.Dap_AN.Add(da);
        //                        }

        //                    }

        //                }

        //            }
        //            cauhoi.Add(cau);


        //        }
        //    }

        //    Session[ComMon.ComMonStants.Cauhoi] = cauhoi;

        //    System.IO.File.Delete(path.ToString());
        //    return Json(new
        //    {
        //        status = true
        //    });

        //}
        // }
        public JsonResult XuLyFile(HttpPostedFileBase file)
        {
            List<Kho_CauHoi> cauHois = new List<Kho_CauHoi>();
            string strExtexsion = Path.GetFileName(file.FileName).Trim();

            if (strExtexsion.Contains(".xls"))
            {
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
                                cauHoi.MucDo = "1";
                            }
                            else if (((EXCELL.Range)range.Cells[i, 7]).Text.Equals("2"))
                            {
                                cauHoi.MucDo = "2";
                            }
                            else if (((EXCELL.Range)range.Cells[i, 7]).Text.Equals("3"))
                            {
                                cauHoi.MucDo = "3";
                            }
                            else
                            {
                                cauHoi.MucDo = "4";
                            }

                            cauHoi.Dap_AN = new List<Dap_AN>();
                            Dap_AN dapAn = new Dap_AN();
                            dapAn.NoiDung = "A) " + ((EXCELL.Range)range.Cells[i, 2]).Text;
                            dapAn.HinhAnh = ((EXCELL.Range)range.Cells[i, 9]).Text;

                            if (((EXCELL.Range)range.Cells[i, 6]).Text.Equals("A"))
                            {
                                dapAn.TrangThai = true;
                            }


                            else { dapAn.TrangThai = false; }
                            cauHoi.Dap_AN.Add(dapAn);
                            dapAn = new Dap_AN();
                            dapAn.NoiDung = "B) " + ((EXCELL.Range)range.Cells[i, 3]).Text;
                            dapAn.HinhAnh = ((EXCELL.Range)range.Cells[i, 10]).Text;

                            if (((EXCELL.Range)range.Cells[i, 6]).Text.Equals("B"))
                            {
                                dapAn.TrangThai = true;
                            }
                            else { dapAn.TrangThai = false; }
                            cauHoi.Dap_AN.Add(dapAn);
                            dapAn = new Dap_AN();
                            dapAn.NoiDung = "C) " + ((EXCELL.Range)range.Cells[i, 4]).Text;
                            dapAn.HinhAnh = ((EXCELL.Range)range.Cells[i, 11]).Text;

                            if (((EXCELL.Range)range.Cells[i, 6]).Text.Equals("C"))
                            {
                                dapAn.TrangThai = true;
                            }
                            else { dapAn.TrangThai = false; }
                            cauHoi.Dap_AN.Add(dapAn);

                            dapAn = new Dap_AN();
                            dapAn.NoiDung = "D) " + ((EXCELL.Range)range.Cells[i, 5]).Text;
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
            else
            {
                DateTime aDateTime = DateTime.Now;
                object path = Server.MapPath("~/Content/" + file.FileName);
                if (System.IO.File.Exists(path.ToString()))
                {
                    System.IO.File.Delete(path.ToString());
                }

                file.SaveAs(path.ToString());

                List<String> anh = new List<string>();
                string totalText = "";
                Document document = new Document(path.ToString());


                //Get Each Section of Document  
                foreach (Section section in document.Sections)
                {
                    //Get Each Paragraph of Section  
                    foreach (Paragraph paragraph in section.Paragraphs)
                    {
                        //Get Each Document Object of Paragraph Items  
                        foreach (DocumentObject docObject in paragraph.ChildObjects)
                        {
                            //If Type of Document Object is Picture, Extract.  
                            if (docObject.DocumentObjectType == DocumentObjectType.Picture)
                            {
                                int sas = Convert.ToInt32(aDateTime.Year * 12 * 30 * 24 * 60 * 60 + aDateTime.Month * 30 * 24 * 60 * 60 + aDateTime.Day * 24 * 60 * 60 + aDateTime.Hour * 60 * 60 + aDateTime.Minute * 60 + aDateTime.Second);
                                DocPicture pic = docObject as DocPicture;
                                String imgName = Server.MapPath("~/Content/Img/Anh" + sas + String.Format(".png"));
                                anh.Add("/Content/Img/Anh" + sas + String.Format(".png"));
                                //Save Image  
                                pic.Image.Save(imgName, System.Drawing.Imaging.ImageFormat.Png);

                            }
                        }
                        totalText += paragraph.Text.ToString();

                    }

                }
                int slanh = 0;
                List<Dap_AN> dapan2 = new List<Dap_AN>();
                cauHois = new List<Kho_CauHoi>();
                for (int i = 0; i < totalText.Length; i++)
                {
                    if (totalText[i] == '$' && totalText[i + 1] == 'c' && totalText[i + 2] == '$')
                    {
                        int slcau = 0;
                        Kho_CauHoi ch = new Kho_CauHoi();
                        int sldapa = 0;
                        int slda = 0;
                        List<Dap_AN> dapan = new List<Dap_AN>();

                        ch.Dap_AN = new List<Dap_AN>();
                        for (int j = i; j < totalText.Length; j++)
                        {

                            if ((totalText[j] == '$' && totalText[j + 1] == '*' && totalText[j + 2] == '$') || (totalText[j] == '$' && totalText[j + 1] == '$'))
                            {
                                slcau++;
                                Dap_AN da = new Dap_AN();
                                if (slcau == 1)
                                {
                                    ch.NoiDung = totalText.Substring(i + 3, j - i - 3);
                                    if (ch.NoiDung[0] == '1')
                                    {
                                        ch.MucDo = "1";
                                    }
                                    else if (ch.NoiDung[0] == '2')
                                    {
                                        ch.MucDo = "2";
                                    }
                                    else if (ch.NoiDung[0] == '3')
                                    {
                                        ch.MucDo = "3";
                                    }
                                    else if (ch.NoiDung[0] == '4')
                                    {
                                        ch.MucDo = "4";
                                    }
                                    else ch.MucDo = "Chua có mức độ";
                                    ch.NoiDung = ch.NoiDung.Substring(1, ch.NoiDung.Length - 1);
                                    ch.HinhAnh = "";
                                    for (int z = 0; z < ch.NoiDung.Length - 2; z++)
                                    {
                                        if (ch.NoiDung[z] == '#' && ch.NoiDung[z + 1] == 'h' && ch.NoiDung[z + 2] == '#')
                                        {

                                            ch.HinhAnh = anh[slanh];
                                            slanh++;
                                            ch.NoiDung = ch.NoiDung.Substring(0, z);
                                            break;
                                        }

                                    }

                                }


                                if (ch.MucDo == "Chua có mức độ") break;
                                for (int k = j + 2; k < totalText.Length; k++)
                                {


                                    if (totalText[j] == '$' && totalText[j + 1] == '*' && totalText[j + 2] == '$')
                                    {

                                        if (totalText[k] == '$' && totalText[k + 1] == '$')
                                        {
                                            da.HinhAnh = "";
                                            da.NoiDung = totalText.Substring(j + 3, k - j - 3);
                                            da.TrangThai = true;
                                            for (int z = 0; z < da.NoiDung.Length - 2; z++)
                                            {
                                                if (da.NoiDung[z] == '#' && da.NoiDung[z + 1] == 'h' && da.NoiDung[z + 2] == '#')
                                                {
                                                    da.HinhAnh = anh[slanh];
                                                    slanh++;
                                                    da.NoiDung = da.NoiDung.Substring(0, z);
                                                }

                                            }
                                            j = k - 1;
                                            ch.Dap_AN.Add(da);


                                        }
                                        else if (totalText[k] == '$' && totalText[k + 1] == 'c' && totalText[k + 2] == '$')
                                        {
                                            da.HinhAnh = "";
                                            da.NoiDung = totalText.Substring(j + 3, k - 3 - j);
                                            da.TrangThai = true;
                                            for (int z = 0; z < da.NoiDung.Length - 2; z++)
                                            {
                                                if (da.NoiDung[z] == '#' && da.NoiDung[z + 1] == 'h' && da.NoiDung[z + 2] == '#')
                                                {
                                                    da.HinhAnh = anh[slanh];
                                                    slanh++;
                                                    da.NoiDung = da.NoiDung.Substring(0, z);
                                                }

                                            }
                                            sldapa++;
                                            j = k - 1;
                                            ch.Dap_AN.Add(da);
                                            break;
                                        }
                                        else if (k == totalText.Length - 1)
                                        {
                                            da.HinhAnh = "";
                                            da.NoiDung = totalText.Substring(j + 3, totalText.Length - j - 3);
                                            da.TrangThai = true;
                                            for (int z = 0; z < da.NoiDung.Length - 2; z++)
                                            {
                                                if (da.NoiDung[z] == '#' && da.NoiDung[z + 1] == 'h' && da.NoiDung[z + 2] == '#')
                                                {
                                                    da.HinhAnh = anh[slanh];
                                                    slanh++;
                                                    da.NoiDung = da.NoiDung.Substring(0, z);
                                                }

                                            }
                                            sldapa++;
                                            j = totalText.Length - 1;
                                            ch.Dap_AN.Add(da);
                                            break;
                                        }
                                    }

                                    else if (totalText[j] == '$' && totalText[j + 1] == '$')
                                    {

                                        if (totalText[k] == '$' && totalText[k + 1] == '$')
                                        {
                                            da.HinhAnh = "";
                                            da.NoiDung = totalText.Substring(j + 2, k - j - 2);
                                            da.TrangThai = false;
                                            for (int z = 0; z < da.NoiDung.Length - 2; z++)
                                            {
                                                if (da.NoiDung[z] == '#' && da.NoiDung[z + 1] == 'h' && da.NoiDung[z + 2] == '#')
                                                {
                                                    da.HinhAnh = anh[slanh];
                                                    slanh++;
                                                    da.NoiDung = da.NoiDung.Substring(0, z);
                                                }

                                            }
                                            j = k - 1;
                                            ch.Dap_AN.Add(da);


                                        }
                                        else if (totalText[k] == '$' && totalText[k + 1] == '*' && totalText[k + 2] == '$')
                                        {
                                            da.HinhAnh = "";
                                            da.NoiDung = totalText.Substring(j + 2, k - j - 2);
                                            da.TrangThai = false;
                                            for (int z = 0; z < da.NoiDung.Length - 2; z++)
                                            {
                                                if (da.NoiDung[z] == '#' && da.NoiDung[z + 1] == 'h' && da.NoiDung[z + 2] == '#')
                                                {
                                                    da.HinhAnh = anh[slanh];
                                                    slanh++;
                                                    da.NoiDung = da.NoiDung.Substring(0, z);
                                                }

                                            }
                                            j = k - 1;
                                            ch.Dap_AN.Add(da);

                                        }
                                        else if (totalText[k] == '$' && totalText[k + 1] == 'c' && totalText[k + 2] == '$')
                                        {
                                            da.HinhAnh = "";
                                            da.NoiDung = totalText.Substring(j + 2, k - j - 2);
                                            da.TrangThai = false;
                                            for (int z = 0; z < da.NoiDung.Length - 2; z++)
                                            {
                                                if (da.NoiDung[z] == '#' && da.NoiDung[z + 1] == 'h' && da.NoiDung[z + 2] == '#')
                                                {
                                                    da.HinhAnh = anh[slanh];
                                                    slanh++;
                                                    da.NoiDung = da.NoiDung.Substring(0, z);
                                                }

                                            }
                                            sldapa++;
                                            j = k - 1;
                                            ch.Dap_AN.Add(da);
                                            break;
                                        }
                                        else if (k == totalText.Length - 1)
                                        {
                                            da.HinhAnh = "";
                                            da.NoiDung = totalText.Substring(j + 2, totalText.Length - j - 2);
                                            da.TrangThai = false;
                                            for (int z = 0; z < da.NoiDung.Length - 2; z++)
                                            {
                                                if (da.NoiDung[z] == '#' && da.NoiDung[z + 1] == 'h' && da.NoiDung[z + 2] == '#')
                                                {
                                                    da.HinhAnh = anh[slanh];
                                                    slanh++;
                                                    da.NoiDung = da.NoiDung.Substring(0, z);
                                                }

                                            }
                                            sldapa++;
                                            ch.Dap_AN.Add(da);
                                            j = totalText.Length - 1;
                                            break;
                                        }
                                    }

                                }



                            }



                            if (sldapa != 0)
                            {
                                cauHois.Add(ch);
                                break;
                            }


                        }
                    }


                }
                Session[ComMon.ComMonStants.Cauhoi] = cauHois;
            }

            return Json(new
            {
                status = true
            });

        }
    }


}