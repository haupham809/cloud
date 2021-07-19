//using HtmlAgilityPack;
//using OpenQA.Selenium;
//using OpenQA.Selenium.Chrome;
//using OpenQA.Selenium.Support.UI;

//using System;
//using System.Collections.Generic;
//using System.Globalization;
//using System.IO;
//using System.Linq;
//using System.Net;
//using System.Web;
//using System.Web.Mvc;
//using TracNghiemOnline.Modell;

//namespace TracNghiemOnline.Areas.Admin.Controllers
//{
//    public class getdataController : Controller
//    {
//        // GET: Admin/getdata
//        public ActionResult Index()
//        {
//            return View();
//        }
//        public ActionResult getdata()
//        {
//            return View();
//        }
//        public bool ktlink(string s)
//        {

//            FileStream fs = new FileStream(Server.MapPath("~/Content/excel/textfile.txt"), FileMode.OpenOrCreate, FileAccess.Read);
//            using (StreamReader sr = new StreamReader(fs))
//            {
//                string a = sr.ReadToEnd();
//                Console.WriteLine(a);
//                if (a.Contains(s))
//                {
//                    return false;
//                }
//                else
//                {
//                    sr.Close();
//                    using (StreamWriter sw = new StreamWriter(Server.MapPath("~/Content/excel/textfile.txt"), append: true))
//                    {
//                        sw.WriteLine(s);
//                        sw.Close();


//                    }
//                    return true;
//                }
//            }

//            return false;
//        }

//        public List<string> getfiletkb()
//        {
//            List<string> trangtt = new List<string>();
//            HtmlWeb htmlWeb = new HtmlWeb();
//            /* {
//                 AutoDetectEncoding = false,
//                 OverrideEncoding = Encoding.UTF8  //Set UTF8 để hiển thị tiếng Việt
//             };*/
//            for (int i = 1; i <= 45; i++)
//            {
//                HtmlDocument document = htmlWeb.Load("https://utc2.edu.vn/thong-bao/p=" + i);
//                var threadItems = document.DocumentNode.SelectNodes("//div[@class='mucdichvu']/h3").ToList();

//                var items = new List<object>();
//                foreach (var item in threadItems)
//                {
//                    //Extract các giá trị từ các tag con của tag li
//                    var linkNode = item.SelectSingleNode(".//a");
//                    var link = linkNode.Attributes["href"].Value;

//                    if (link.Contains("chinh-thuc-thoi-khoa-bieu") && ktlink(link))
//                    {

//                        HtmlDocument document1 = htmlWeb.Load("https://utc2.edu.vn/" + link);
//                        var threadItems1 = document1.DocumentNode.SelectNodes("//div[@id='noidungtrong']/p").ToList();
//                        var items1 = new List<object>();
//                        foreach (var item1 in threadItems1)
//                        {
//                            //Extract các giá trị từ các tag con của tag li
//                            var linkNode1 = item1.SelectSingleNode(".//a");
//                            if (linkNode1 != null)
//                            {
//                                var link1 = linkNode1.Attributes["href"].Value;
//                                if (link1.Contains("https://utc2.edu.vn"))
//                                {

//                                    trangtt.Add(Server.MapPath("~/Content/excel/") + link + ".xls");
//                                    WebClient webClient1 = new WebClient();
//                                    webClient1.DownloadFile(link1, Server.MapPath("~/Content/excel/") + link + ".xls");
//                                }
//                                else
//                                {
//                                    trangtt.Add(Server.MapPath("~/Content/excel/") + link + ".xls");
//                                    WebClient webClient = new WebClient();
//                                    webClient.DownloadFile("https://utc2.edu.vn" + link1, Server.MapPath("~/Content/excel/") + link + ".xls");
//                                }


//                                break;
//                            }

//                        }
//                    }
//                }
//            }

//            return trangtt;
//        }
//        public ActionResult getclass()
//        {

//            /* List<string> linkfile = getfiletkb();
//             foreach (var item in linkfile)
//             {
//                 Workbook workbook = new Workbook();

//                 workbook.LoadFromFile("" + item);

//                 Worksheet sheet = workbook.Worksheets[0];

//                 for (int k = 9; k < 10000; k++)
//                 {
//                     if (sheet.Range["D" + k].Text != null&& sheet.Range["B" + k].Text != null&& sheet.Range["D" + k].Text != null)
//                     {
//                         LopHocPhan lhp = new LopHocPhan();
//                         lhp.TenLop = sheet.Range["D" + k].Text;
//                         lhp.SiSo = sheet.Range["D" + k].Text;
//                         TracNghiemOnlineDB db = new TracNghiemOnlineDB();
//                         db.LopHocPhans.Add(lhp);
//                         db.SaveChanges();

//                     }



//                 }
//             }*/


//            return Content("lay  thanh cong");

//        }
//        public ActionResult getnganh()
//        {

//            var options = new ChromeOptions();
//            options.AddArgument("no-sandbox");
//            // Chạy ngầm không pop up trình duyệt ra ngoài 
//            options.AddArgument("headless");
//            IWebDriver webDriver = new ChromeDriver(Server.MapPath("~/Content/chromdriver"), options);
//            webDriver.Url = "http://xemdiem.utc2.edu.vn/xemdiem.aspx";

//            return Content("lay  thanh cong");
//        }


//        public ActionResult getstudent()
//        {

//            var options = new ChromeOptions();
//            options.AddArgument("no-sandbox");
//            // Chạy ngầm không pop up trình duyệt ra ngoài 
//            options.AddArgument("headless");
//            IWebDriver webDriver = new ChromeDriver(Server.MapPath("~/Content/chromdriver"),options );
//            webDriver.Url = "http://xemdiem.utc2.edu.vn/xemdiem.aspx";

//            var radiobutton = webDriver.FindElement(By.XPath("//*[@id='sfsdfdslf']/table/tbody/tr[1]/td[2]/input[2]"));
//            radiobutton.Click();

//            var khoa = webDriver.FindElement(By.Name("DropDownList2"));
//            var selectkhoa = new SelectElement(khoa);

//            var yy = webDriver.FindElements(By.XPath("//*[@id='DropDownList2']/option"));
//            foreach (var j in yy)
//            {
//                if (j.Text.Equals("Tất cả")) continue;
//                else if (j.Text.Equals("All")) continue;
//                else if (j.Text.Equals("Khóa 59"))
//                {
//                    selectkhoa.SelectByText(j.Text);
//                    var lop = webDriver.FindElement(By.Name("DropDownList3"));
//                    var selectlop = new SelectElement(lop);
//                    var xx = webDriver.FindElements(By.XPath("//*[@id='DropDownList3']/option"));

//                    foreach (var i in xx)
//                    {
//                        TracNghiemOnlineDB db = new TracNghiemOnlineDB();
//                        if (i.Text.Equals("Tất cả")) continue;
//                        else
//                        {

//                            if (db.Lops.Where(x => x.TenLop.Equals(i.Text)).ToList().Count <= 0)
//                            {
//                                /* try
//                                 {*/


//                                Lop lop1 = new Lop();

//                                var malop = "";
//                                int z = 0;
//                                int vtt = 0;
//                                var s = i.Text.ToString().Substring(0, i.Text.ToString().Length);
//                                if (i.Text.ToString().Contains("Quận"))
//                                {
//                                    s = i.Text.ToString().Substring(0, i.Text.IndexOf("Quận") - 1);
//                                }

//                                foreach (var c in s)
//                                {
//                                    if (z == 0)
//                                    {
//                                        malop = malop + c.ToString();


//                                    }
//                                    else if (c.ToString() == " ")
//                                    {
//                                        vtt = z;
//                                        malop = malop + s[z + 1].ToString();
//                                    }
//                                    z++;
//                                }
//                                if (i.Text.ToString().Contains("Quận"))
//                                {
//                                    lop1.Ma_Lop = (malop.Substring(0, malop.Length - 1) + i.Text.ToString().Substring(vtt + 4, i.Text.Length - vtt - 4)).ToUpper();
//                                }
//                                else
//                                    lop1.Ma_Lop = (malop.Substring(0, malop.Length - 1) + i.Text.ToString().Substring(vtt, i.Text.Length - vtt)).ToUpper();

//                                lop1.TenLop = i.Text.ToString();
//                                if (db.Nganhs.Where(x => s.ToUpper().Contains(x.TenNganh.ToUpper())).ToList().Count <= 0)
//                                {
//                                    lop1.Ma_Nganh = null;

//                                }
//                                else lop1.Ma_Nganh = db.Nganhs.SingleOrDefault(x => s.ToUpper().Contains(x.TenNganh.ToUpper())).Ma_Nganh;
//                                db.Lops.Add(lop1);
//                                db.SaveChanges();
//                                /*   }
//                               catch (Exception e)
//                                   {

//                                   }*/



//                            }

//                            if (i.Text.Equals("Công nghệ thông tin K59"))
//                            {


//                                selectlop.SelectByText(i.Text);

//                                var button = webDriver.FindElement(By.XPath("//*[@id='Tim']"));
//                                button.Click();
//                                var table = webDriver.FindElement(By.XPath("//*[@id='listsinhvien']/div/table/tbody"));
//                                var rows_table = table.FindElements(By.TagName("tr"));
//                                int rows_count = rows_table.Count;
//                                int z = 1;
//                                foreach (var item in rows_table)
//                                {
//                                    if (z == 1 || z == rows_count)
//                                    {

//                                    }
//                                    else
//                                    {
//                                        var tablesv = item.FindElements(By.TagName("td"));


//                                        SinhVien sv = new SinhVien();
//                                        sv.MaSV = tablesv[2].Text;
//                                        sv.Ten = tablesv[3].Text;

//                                        sv.NgaySinh = DateTime.ParseExact(tablesv[4].Text, "d/M/yyyy", CultureInfo.InvariantCulture);
//                                        sv.DiaChi = tablesv[5].Text;

//                                        var tenlop = item.FindElement(By.XPath("//*[@id='lbADMINCLASSID']")).GetAttribute("value");
//                                        var malop = db.Lops.Where(x => x.TenLop.ToUpper().Equals(tenlop.ToUpper()) && x.DaXoa == null).ToList();
//                                        if (malop.Count > 0)
//                                        {
//                                            sv.Ma_Lop = malop[0].Ma_Lop.ToString();
//                                            if (db.SinhViens.Where(x => x.MaSV.Equals(sv.MaSV)).ToList().Count <= 0 && tenlop.Length > 0)
//                                            {
//                                                try
//                                                {
//                                                    db.SinhViens.Add(sv);
//                                                    db.SaveChanges();
//                                                }
//                                                catch
//                                                (Exception e)
//                                                {

//                                                }
//                                                if (db.TaiKhoans.Where(x => x.TaiKhoan1.Equals(sv.MaSV) && x.TenDangNhap.Equals(sv.MaSV)).ToList().Count <= 0)
//                                                {
//                                                    TaiKhoan tk = new TaiKhoan();
//                                                    tk.TenDangNhap = sv.MaSV;
//                                                    tk.TaiKhoan1 = sv.MaSV;
//                                                    tk.MatKhau = "1";
//                                                    tk.TrangThai = true;
//                                                    tk.ChưcVu = "SinhViên";
//                                                    db.TaiKhoans.Add(tk);
//                                                    db.SaveChanges();
//                                                }
//                                            }
//                                        }




//                                    }
//                                    z++;
//                                }

//                                var sltrang = webDriver.FindElement(By.XPath("//*[@id='listsinhvien']/table[2]/tbody/tr/td/font[4]")).Text;


//                                for (int slpage = 1; slpage <= Convert.ToInt32(sltrang) - 1; slpage++)
//                                {
//                                    var page = webDriver.FindElement(By.XPath("//*[@id='listsinhvien']/table[1]/tbody/tr/td/font/font/b/a[" + slpage + "]"));
//                                    page.Click();

//                                    var table1 = webDriver.FindElement(By.XPath("//*[@id='listsinhvien']/div/table/tbody"));
//                                    var rows_table1 = table1.FindElements(By.TagName("tr"));
//                                    int rows_count1 = rows_table1.Count;
//                                    int z1 = 1;
//                                    foreach (var item in rows_table1)
//                                    {
//                                        if (z1 == 1 || z1 == rows_count1)
//                                        {

//                                        }
//                                        else
//                                        {



//                                            var tablesv = item.FindElements(By.TagName("td"));


//                                            SinhVien sv = new SinhVien();
//                                            sv.MaSV = tablesv[2].Text;
//                                            sv.Ten = tablesv[3].Text;

//                                            sv.NgaySinh = DateTime.ParseExact(tablesv[4].Text, "d/M/yyyy", CultureInfo.InvariantCulture);
//                                            sv.DiaChi = tablesv[5].Text;

//                                            var tenlop = item.FindElement(By.XPath("//*[@id='lbADMINCLASSID']")).GetAttribute("value");
//                                            var malop = db.Lops.Where(x => x.TenLop.Equals(tenlop) && x.DaXoa == null).ToList();
//                                            if (malop.Count > 0)
//                                            {
//                                                sv.Ma_Lop = malop[0].Ma_Lop.ToString();
//                                                if (db.SinhViens.Where(x => x.MaSV.Equals(sv.MaSV)).ToList().Count <= 0 && tenlop.Length > 0)
//                                                {

//                                                    db.SinhViens.Add(sv);
//                                                    db.SaveChanges();
//                                                    if (db.TaiKhoans.Where(x => x.TaiKhoan1.Equals(sv.MaSV) && x.TenDangNhap.Equals(sv.MaSV)).ToList().Count <= 0)
//                                                    {
//                                                        TaiKhoan tk = new TaiKhoan();
//                                                        tk.TenDangNhap = sv.MaSV;
//                                                        tk.TaiKhoan1 = sv.MaSV;
//                                                        tk.MatKhau = "1";
//                                                        tk.TrangThai = true;
//                                                        tk.ChưcVu = "SinhViên";
//                                                        db.TaiKhoans.Add(tk);
//                                                        db.SaveChanges();
//                                                    }
//                                                }
//                                            }





//                                        }
//                                        z1++;
//                                    }
//                                }

//                            }
//                        }



//                    }

//                }

//            }
//            return Content("lay  thanh cong");
//        }

//    }
//}