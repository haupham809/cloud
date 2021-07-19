using Microsoft.Ajax.Utilities;
using Microsoft.Office.Interop.Excel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;

using TracNghiemOnline.Model;

namespace TracNghiemOnline.Modell.Dao
{
    public class QuanLyThiDAO
    {
        TracNghiemOnlineDB db = new TracNghiemOnlineDB();
        public DanhGia Mark(De_Thi exam)
        {
            exam.Da_SVLuaChon = db.Da_SVLuaChon.Where(x => x.MaDeThi == exam.MaDeThi).ToList();
            int socauDung = 0;
            var Bode = new BoDeDao().ChapterStudy(long.Parse(exam.Ma_BoDe.ToString()));

            List<Kho_CauHoi> kho_CauHois = new List<Kho_CauHoi>();
            List<Chuong_Hoc> chuonghoc1 = new List<Chuong_Hoc>();
            List<Chuong_Hoc> chuong2 = new List<Chuong_Hoc>();
            foreach (var item in exam.CauHoiDeThis)
            {
                kho_CauHois.Add(item.Kho_CauHoi);
            }
            //Lay Ra So Cau HOI cua moi chuong
            foreach (var item in db.Chuong_Hoc.Where(x => x.Ma_Mon == Bode.Ma_Mon).ToList())
            {
                Chuong_Hoc chuong = new Chuong_Hoc();
                chuong.Ma_Chuong = item.Ma_Chuong;
                chuong.TenChuong = item.TenChuong;
                chuong.Kho_CauHoi = new List<Kho_CauHoi>();
                chuonghoc1.Add(chuong);
            }
            db = new TracNghiemOnlineDB();
            foreach (var item in db.Chuong_Hoc.Where(x => x.Ma_Mon == Bode.Ma_Mon).ToList())
            {
                Chuong_Hoc chuong = new Chuong_Hoc();
                chuong.Ma_Chuong = item.Ma_Chuong;
                chuong.Kho_CauHoi = new List<Kho_CauHoi>();
                chuong2.Add(chuong);
            }
            foreach (var item in chuonghoc1)
            {
                foreach (var item1 in kho_CauHois.Where(x => x.Ma_Chuong == item.Ma_Chuong).ToList())
                {
                    item.Kho_CauHoi.Add(item1);

                }

            }

            //Lay Ra So Cau sv da lam  dung cua moi chuong
            var kho_CauHoi1 = new List<Kho_CauHoi>();
            foreach (var item in exam.CauHoiDeThis)
            {
                item.Kho_CauHoi = new CauHoiDao().Question(item.Kho_CauHoi.Ma_CauHoi);
                foreach (var item1 in item.Kho_CauHoi.Dap_AN)
                {
                    if (exam.Da_SVLuaChon.ToList().Exists(x => x.Ma_DAN == item1.MA_DAN && item1.TrangThai == true))
                    {
                        socauDung++;
                        kho_CauHoi1.Add(item.Kho_CauHoi);

                    }

                }

            }
            //  Lay Ra So Cau sv da lam dung cua moi chuong
            foreach (var item0 in chuong2)
            {
                item0.Kho_CauHoi = kho_CauHoi1.Where(x => x.Ma_Chuong == item0.Ma_Chuong).ToList();
            }
            DanhGia danhGia = new DanhGia();
            danhGia.DanhGiaMucDo = new List<SoLuongChuong>();
            danhGia.ketQuaThi = new KetQuaThi();
            for (int i = 0; i < chuonghoc1.Count; i++)
            {
                if (chuonghoc1[i].Kho_CauHoi.Count > 0)
                {
                    db = new TracNghiemOnlineDB();
                    Danh_Gia danh_Gia = new Danh_Gia();
                    SoLuongChuong soLuongChuong = new SoLuongChuong();
                    danh_Gia.MaChuong = chuonghoc1[i].Ma_Chuong;
                    danh_Gia.MaDeThi = exam.MaDeThi;
                    danh_Gia.SoCauDung = chuong2[i].Kho_CauHoi.Count;
                    danh_Gia.TongCau = chuonghoc1[i].Kho_CauHoi.Count;
                    soLuongChuong.TongSoCau = "" + danh_Gia.SoCauDung + "/" + danh_Gia.TongCau;
                    double a1 = (double)(chuong2[i].Kho_CauHoi.Where(X => X.MucDo.Equals("Nhận Biết")).ToList().Count * 1);
                    double a2 = (double)(chuong2[i].Kho_CauHoi.Where(X => X.MucDo.Equals("Thông Hiểu")).ToList().Count * 2);
                    double a3 = (double)(chuong2[i].Kho_CauHoi.Where(X => X.MucDo.Equals("Vận Dụng")).ToList().Count * 3);
                    double a4 = (double)(chuong2[i].Kho_CauHoi.Where(X => X.MucDo.Equals("Vận Dụng Cao")).ToList().Count * 4);


                    soLuongChuong.Chuong = chuonghoc1[i];
                    soLuongChuong.nhanBiet = chuong2[i].Kho_CauHoi.Where(X => X.MucDo.Equals("Nhận Biết")).ToList().Count + "/" +
                        chuonghoc1[i].Kho_CauHoi.Where(X => X.MucDo.Equals("Nhận Biết")).ToList().Count;


                    soLuongChuong.thongHieu = chuong2[i].Kho_CauHoi.Where(X => X.MucDo.Equals("Thông Hiểu")).ToList().Count + "/" +
                        chuonghoc1[i].Kho_CauHoi.Where(X => X.MucDo.Equals("Thông Hiểu")).ToList().Count;

                    soLuongChuong.vandung = chuong2[i].Kho_CauHoi.Where(X => X.MucDo.Equals("Vận Dụng")).ToList().Count + "/" +
                        chuonghoc1[i].Kho_CauHoi.Where(X => X.MucDo.Equals("Vận Dụng")).ToList().Count;

                    soLuongChuong.vandungcao = chuong2[i].Kho_CauHoi.Where(X => X.MucDo.Equals("Vận Dụng Cao")).ToList().Count + "/" +
                        chuonghoc1[i].Kho_CauHoi.Where(X => X.MucDo.Equals("Vận Dụng Cao")).ToList().Count;
                    danhGia.DanhGiaMucDo.Add(soLuongChuong);

                    double b1 = (double)(chuonghoc1[i].Kho_CauHoi.Where(X => X.MucDo.Equals("Nhận Biết")).ToList().Count * 1);
                    double b2 = (double)(chuonghoc1[i].Kho_CauHoi.Where(X => X.MucDo.Equals("Thông Hiểu")).ToList().Count * 2);
                    double b3 = (double)(chuonghoc1[i].Kho_CauHoi.Where(X => X.MucDo.Equals("Vận Dụng")).ToList().Count * 3);
                    double b4 = (double)(chuonghoc1[i].Kho_CauHoi.Where(X => X.MucDo.Equals("Vận Dụng Cao")).ToList().Count * 4);

                    double DG = 0;
                    double A = a1 + a2 + a3 + a4;
                    double b = b1 + b2 + b3 + b4;
                    DG = (double)(A / b) * 10;
                    double tile = (danh_Gia.SoCauDung / danh_Gia.TongCau) * 100;
                    danh_Gia.NhanXet = "Bạn làm đúng (" + soLuongChuong.TongSoCau + " đạt tỉ lệ " + tile + " %) \n                                                      ";
                    if (DG < 5)
                    {
                        danh_Gia.DanhGia = "Yếu";

                        danh_Gia.NhanXet += "-Kiến thức phần này của bạn còn rất hạn chế điểm phần này bài test còn chưa cao.Bạn cần cố gắng cải thiện hơn nữa";


                    }
                    if (DG >= 5 && DG < 7)
                    {
                        danh_Gia.DanhGia = "Trung Bình";
                        danh_Gia.NhanXet += "Kiến thức của bạn ở phần này chỉ ở mức trung bình. Bạn cần cố gắng hơn để cải thiện thành tích của mình";
                    }

                    if (DG >= 7 && DG < 8.5)
                    {
                        danh_Gia.DanhGia = "Khá";
                        danh_Gia.NhanXet += "Kiến thức của bạn ở phần này khá tốt. Bạn cố gắng thêm để đặt được số điểm cao hơn nữa";
                    }
                    if (DG >= 8.5)
                    {
                        danh_Gia.DanhGia = "Giỏi";
                        danh_Gia.NhanXet += "Kiến thức của bạn ở phần bạn rất làm rất tốt. Bạn cố gắng duy trì phong độ nhé";
                    }

                    danh_Gia.Chuong_Hoc = db.Chuong_Hoc.Find(danh_Gia.MaChuong);
                    exam.Danh_Gia.Add(danh_Gia);
                }

            }


            double Hediem = (double)((double)10 / (double)(exam.CauHoiDeThis.Count));
            KetQuaThi ketQuaThi = new KetQuaThi();
            ketQuaThi.Ma_DeThi = exam.MaDeThi;
            ketQuaThi.NgayThi = DateTime.Now;
            ketQuaThi.SoCauDung = socauDung;
            ketQuaThi.SoCauSai = exam.Da_SVLuaChon.Count - socauDung;
            ketQuaThi.DiemSo = Math.Round((double)((double)(socauDung) * (double)(Hediem)), 3);
            exam.Bo_De = Bode;
            //    exam.Bo_De.
            ketQuaThi.De_Thi = exam;
            ketQuaThi.De_Thi.SinhVien = db.SinhViens.Find(exam.Ma_SV);
            danhGia.ketQuaThi = ketQuaThi;
            return danhGia;

        }

        //Lay bài thi cua 1 phong
        internal object ListALLexam(string id)
        {
            List<DanhGia> ketQuas = new List<DanhGia>();
            Phong_Thi phong_Thi = ExamitionRoom(id);
            foreach (var item in phong_Thi.DS_SVThi)
            {
                var DeThi = new QuanLyThiDAO().SeachForTheExam(phong_Thi, item.Ma_SV);
                if (DeThi != null)
                {
                    ketQuas.Add(Mark(DeThi));
                }


            }


            return ketQuas;
        }

        //lay ds lop hoc
        internal List<LopHocPhan> ListClassRom(string id)
        {
            return db.LopHocPhans.Where(x => x.MaGV.Equals(id)).ToList();
        }

        //them hoi phong hoc
        internal void CreateExamitionRoom(string malop, string id, List<DS_SVThi> dSSV, string nd,string GV,string GV1 ,DateTime date)
        {
            try
            {
                db = new TracNghiemOnlineDB();
                Phong_Thi phong_Thi = new Phong_Thi();
                phong_Thi.TrangThai = "Chưa Thi";
                phong_Thi.MaLopHP = malop;
                phong_Thi.NguoiTao = id;
                phong_Thi.TenPhong = nd;
                phong_Thi.MaCanBo1 = GV;
                phong_Thi.MaCanBo2 = GV1;
                phong_Thi.ThoiGianMo = date;
                phong_Thi.Xoa = true;
                while (true)
                {
                    phong_Thi.MaPhong = RandomString(10);
                    if (!db.Phong_Thi.ToList().Exists(x => x.MaPhong.Equals(phong_Thi.MaPhong)))
                    {
                        break;
                    }


                }
                db.Phong_Thi.Add(phong_Thi);
                db.SaveChanges();
                CreateSinhVienRoom(phong_Thi.MaPhong, dSSV);



            }
            catch (Exception e)
            {
                
            }

        }
        internal void CreateExamitionRoom(string malop, string id, List<DS_SVThi> dSSV, string nd)
        {
            try
            {

                Phong_Thi phong_Thi = new Phong_Thi();
                phong_Thi.TrangThai = "Chưa Thi";
                phong_Thi.MaLopHP = malop;
                phong_Thi.NguoiTao = id;
                phong_Thi.TenPhong = nd;
                phong_Thi.Xoa = true;
                while (true)
                {
                    phong_Thi.MaPhong = RandomString(10);
                    if (!db.Phong_Thi.ToList().Exists(x => x.MaPhong.Equals(phong_Thi.MaPhong)))
                    {
                        break;
                    }


                }
                db.Phong_Thi.Add(phong_Thi);
                db.SaveChanges();
                CreateSinhVienRoom(phong_Thi.MaPhong, dSSV);
            }
            catch (Exception e)
            {
                
            }

        }
        public void CreateSinhVienRoom(string maphong, List<DS_SVThi> dSSV)
        {
            foreach (var item in dSSV)
            {
                item.MaPhong = maphong;
                db.DS_SVThi.Add(item);
                db.SaveChanges();
                try
                {
                    if (new TracNghiemOnlineDB().Phong_Thi.Find(maphong).MaBoDe != null)
                    {
                        var list = new BoDeDao().ChapterStudy(long.Parse(new TracNghiemOnlineDB().Phong_Thi.Find(maphong).MaBoDe.ToString()));
                        var dethi = new BoDeDao().MixExemQuestion(list, item.Ma_SV);
                        new BoDeDao().UpdateDsThi(new TracNghiemOnlineDB().Phong_Thi.Find(maphong), dethi, item.Ma_SV, "Chưa Làm");

                    }

                }
                catch { }

            }

        }

        internal object ClassRom(string MAlOP)
        {
            var Lop = db.LopHocPhans.Find(MAlOP);
            Lop.DS_LopHP = new List<DS_LopHP>();
            Lop.DS_LopHP = db.DS_LopHP.Where(X => X.Ma_LOP.Equals(MAlOP)).ToList();

            foreach (var item in Lop.DS_LopHP)
            {
                item.SinhVien = db.SinhViens.Find(item.MA_SV);
                item.SinhVien.Lop = db.Lops.Find(item.SinhVien.Ma_Lop);
            }

            return Lop;
        }
        //sua phong tho
        internal void UpDatePhongThi(Phong_Thi classRoom)
        {
            
         //   classRoom.ThoiGianDong = classRoom.ThoiGianMo.AddMinutes(double.Parse(classRoom.Bo_De.ThoiGianThi));
            var Room = db.Phong_Thi.Find(classRoom.MaPhong);
            Room.MaBoDe = classRoom.MaBoDe;
            Room.TrangThai = classRoom.TrangThai;
            Room.Xoa = classRoom.Xoa;
            Room.ThoiGianMo = classRoom.ThoiGianMo;
            Room.ThoiGianDong = classRoom.ThoiGianDong;
            db.SaveChanges();
            db = new TracNghiemOnlineDB();

            var ds = db.DS_SVThi.Where(x => x.MaPhong == classRoom.MaPhong).ToList();
            foreach (var item in ds)
            {
                var list = new BoDeDao().ChapterStudy(long.Parse(classRoom.MaBoDe.ToString()));
                var dethi = new BoDeDao().MixExemQuestion(list, item.Ma_SV);
                new BoDeDao().UpdateDsThi(classRoom, dethi, item.Ma_SV, "Chưa Làm");
            }

        }
        internal void UpDatePhongThi1(Phong_Thi classRoom)
        {
           
            var Room = db.Phong_Thi.Find(classRoom.MaPhong);
            Room.MaBoDe = classRoom.MaBoDe;
            Room.TrangThai = classRoom.TrangThai;
            Room.Xoa = classRoom.Xoa;          
            db.SaveChanges();
        }

        //lay ra sanh sach sv cua mot lop học phan
        internal object LissAllSinhVien(string id)
        {
            var lop = (LopHocPhan)ClassRom(id);
            var lissv = db.SinhViens.Where(X => X.DaXoa !=1);
            var SinhVien = new List<SinhVien>();
           
            
                    foreach (var item2 in lissv)
                    {
                        if (!lop.DS_LopHP.ToList().Exists(x => x.MA_SV == item2.MaSV))
                        {
                            item2.Lop = db.Lops.Find(item2.Ma_Lop);
                            SinhVien.Add(item2);
                        }

            }
            return SinhVien;

        }

        public string RandomString(int size, bool lowerCase = false)
        {
            Random random = new Random();
            var builder = new StringBuilder(size);

            char offset = lowerCase ? 'a' : 'A';
            const int lettersOffset = 26; // A...Z or a..z: length = 26  

            for (var i = 0; i < size; i++)
            {
                var @char = (char)random.Next(offset, offset + lettersOffset);
                builder.Append(@char);
            }

            return lowerCase ? builder.ToString().ToLower() : builder.ToString();
        }

        //them lop học phan
        internal void CreateClassRoom(string malop, long mamon, string id, long maki)
        {
            try
            {
                LopHocPhan lopHocPhan = new LopHocPhan();
                while (true)
                {
                    Random random = new Random();
                    lopHocPhan.MaLop = RandomString(2) + random.Next(8);
                    if (!db.LopHocPhans.ToList().Exists(x => x.MaLop.Equals(lopHocPhan.MaLop)))
                    {
                        break;
                    }
                }
                db = new TracNghiemOnlineDB();
                lopHocPhan.TenLop = malop;
                lopHocPhan.MaMon = mamon;
                lopHocPhan.MaGV = id;
                lopHocPhan.MaKi = maki;
                lopHocPhan.TrangThai = 1;
                db.LopHocPhans.Add(lopHocPhan);
                db.SaveChanges();
            }
            catch (Exception e)
            {
            }

        }
        internal object CoiThi(string id)
        {
            var phong = db.Phong_Thi.Where(x => !x.TrangThai.Equals("Đã Đóng") && x.Xoa == true && (x.MaCanBo1.Equals(id)|| x.MaCanBo2.Equals(id))).ToList();

            foreach (var item in phong)
            {
                if (item.ThoiGianDong <= DateTime.Now)
                {
                    var p = db.Phong_Thi.Find(item.MaPhong);
                    p.TrangThai = "Đã Đóng";
                    db.SaveChanges();

                }

            }
             phong = db.Phong_Thi.Where(x => !x.TrangThai.Equals("Đã Đóng") && x.Xoa == true && (x.MaCanBo1.Equals(id) || x.MaCanBo2.Equals(id))).ToList();
            foreach (var item in phong)
            {

                item.LopHocPhan = (LopHocPhan)ClassRom(item.MaLopHP);
            }
            return phong;
        }

        //lay ra ds  phong thi
        internal object ListAllClassRom(string id)
        {
            var phong = db.Phong_Thi.Where(x => !x.TrangThai.Equals("Đã Đóng") && x.Xoa == true && x.NguoiTao.Equals(id)).ToList();
            foreach (var item in phong)
            {
                if (item.ThoiGianDong <= DateTime.Now)
                {
                    var p = db.Phong_Thi.Find(item.MaPhong);
                    p.TrangThai = "Đã Đóng";
                    db.SaveChanges();

                }

            }
            phong = db.Phong_Thi.Where(x => !x.TrangThai.Equals("Đã Đóng") && x.Xoa == true && x.NguoiTao.Equals(id)).ToList();

            foreach (var item in phong)
            {

                item.LopHocPhan = (LopHocPhan)ClassRom(item.MaLopHP);
            }
            return phong;
        }
        internal object DanhGiaKetQua(string id)
        {

            var phong = db.Phong_Thi.Where(x => x.TrangThai.Equals("Đang Thi") && x.NguoiTao.Equals(id) && x.Xoa == true).ToList();

            foreach (var item in phong)
            {
                if (item.ThoiGianDong <= DateTime.Now)
                {

                    var p = db.Phong_Thi.Find(item.MaPhong);
                    p.TrangThai = "Đã Đóng";
                    db.SaveChanges();

                }

            }

            phong = db.Phong_Thi.Where(x => x.TrangThai.Equals("Đã Đóng")&& x.NguoiTao.Equals(id) && x.Xoa == true).ToList();


            foreach (var item in phong)
            {
                item.LopHocPhan = (LopHocPhan)ClassRom(item.MaLopHP);

            }

            return phong;
        }



        //lay ra phong thi va ds phong thi
        public Phong_Thi ExamitionRoom(string MaPhong)
        {
            var phong = db.Phong_Thi.Find(MaPhong);
            try
            {
                phong.LopHocPhan = (LopHocPhan)ClassRom(phong.MaLopHP);

                phong.DS_SVThi = db.DS_SVThi.Where(x => x.MaPhong == MaPhong).ToList();
                foreach (var item in phong.DS_SVThi)
                {
                    item.SinhVien = db.SinhViens.Find(item.Ma_SV);
                    item.SinhVien.Lop = db.Lops.Find(item.SinhVien.Ma_Lop);
                    try
                    {
                        item.SinhVien.De_Thi.ToList()[0] = new TracNghiemOnlineDB().De_Thi.Find(item.MaDeThi);

                    }
                    catch { }
                }

            }
            catch { }



            return phong;
        }

        //kiem tra sv co trong phong thi hay k0
        internal DS_SVThi Check(string maPhong, string taiKhoan1)
        {
            return db.DS_SVThi.SingleOrDefault(x => x.MaPhong == maPhong && x.Ma_SV.Equals(taiKhoan1));
        }

        //tim kiem bai thi sv
        internal De_Thi SeachForTheExam(Phong_Thi phong, string Masv)
        {
            var DSSV = db.DS_SVThi.SingleOrDefault(x => x.MaPhong == phong.MaPhong && x.Ma_SV == Masv);

            return SearDethi(DSSV.MaDeThi);
        }

        public De_Thi SearDethi(long? maDeThi)
        {
            De_Thi de_Thi = new De_Thi();

            de_Thi = db.De_Thi.Find(maDeThi);

            if (de_Thi != null)
            {
                de_Thi.CauHoiDeThis = db.CauHoiDeThis.Where(x => x.MaDeThi == de_Thi.MaDeThi).ToList();
                de_Thi.Da_SVLuaChon = db.Da_SVLuaChon.Where(x => x.MaDeThi == de_Thi.MaDeThi).ToList();

                foreach (var item1 in de_Thi.CauHoiDeThis)
                {
                    item1.Kho_CauHoi = new CauHoiDao().Question(long.Parse(item1.MaCauHoi.ToString()));
                    foreach (var item2 in item1.Kho_CauHoi.Dap_AN)
                    {
                        if (de_Thi.Da_SVLuaChon.ToList().Exists(x => x.Ma_DAN == item2.MA_DAN))
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
            return de_Thi;
        }
    }
}