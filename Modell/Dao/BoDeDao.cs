using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;


namespace TracNghiemOnline.Modell.Dao
{
    public class BoDeDao
    {
        TracNghiemOnlineDB db = new TracNghiemOnlineDB();

        public void CreateChapterStudy(Bo_De bo_De1)
        {
            Bo_De bo_De = new Bo_De();
            bo_De.NoiDung = bo_De1.NoiDung;
            bo_De.ThoiGianThi = bo_De1.ThoiGianThi;
            bo_De.Ma_Mon = bo_De1.Ma_Mon;
            bo_De.Ma_NguoiTao = bo_De1.Ma_NguoiTao;
            bo_De.TrangThai = true;
            bo_De.Xoa = true;
            foreach (var item in bo_De1.CauHois)
            {
                item.Kho_CauHoi = null;
                bo_De.CauHois.Add(item);  
            }
            bo_De.SoCau = bo_De.CauHois.Count;
            try
            {
                db = new TracNghiemOnlineDB();
                db.Bo_De.Add(bo_De);
                db.SaveChanges();
             
            }catch(Exception e)
            {
              
            }
      
        }


        public De_Thi MixExemQuestion(Bo_De bo_De ,string SV)
        {
            Random random = new Random();
            De_Thi de_Thi = new De_Thi();
            de_Thi.Ma_BoDe = bo_De.Ma_BoDe;
           de_Thi.Ma_SV = SV;
            de_Thi.TrangThai = true;
            var lisQuestion = (List<CauHoi>)bo_De.CauHois;
            int Lenght = lisQuestion.Count;

            for (int i = 0; i < Lenght; i++)
            {
                int vt = random.Next(lisQuestion.Count);

                CauHoiDeThi cauHoi = new CauHoiDeThi();
                cauHoi.MaCauHoi = lisQuestion[vt].Ma_CauHoi;

                de_Thi.CauHoiDeThis.Add(cauHoi);
                lisQuestion.RemoveAt(vt);
            }
            de_Thi.TrangThai = true;
            db.De_Thi.Add(de_Thi);
            db.SaveChanges();
            de_Thi.MaDeThi = db.De_Thi.Select(x => x).ToList().Last().MaDeThi;
            de_Thi.CauHoiDeThis = db.CauHoiDeThis.Where(x => x.MaDeThi == de_Thi.MaDeThi).ToList();
            foreach (var item in de_Thi.CauHoiDeThis)
            {
                item.Kho_CauHoi = new CauHoiDao().Question(long.Parse(item.MaCauHoi.ToString()));
                foreach (var item1 in item.Kho_CauHoi.Dap_AN)
                {
                    item1.TrangThai = false;
                }
            }
            de_Thi.SinhVien = db.SinhViens.SingleOrDefault(x=>x.MaSV.Equals(SV));
            de_Thi.SinhVien.Lop= new TracNghiemOnlineDB().Lops.Find(de_Thi.SinhVien.Ma_Lop);
            de_Thi.SinhVien.Lop.Nganh= new TracNghiemOnlineDB().Nganhs.Find(de_Thi.SinhVien.Lop.Ma_Nganh);
            de_Thi.Bo_De = bo_De;
            return de_Thi;
        }

        internal void OptionStudent(De_Thi examQuestion)
        {
            var ListOptionStudent = db.Da_SVLuaChon.Where(x => x.MaDeThi == examQuestion.MaDeThi).ToList();
            if (ListOptionStudent.Count > 0)
            {
                foreach (var item in ListOptionStudent)
                {
                    db.Da_SVLuaChon.Remove(item);
                    db.SaveChanges();
                }
            }
            foreach (var item in examQuestion.CauHoiDeThis)
            {
                foreach (var item1 in item.Kho_CauHoi.Dap_AN)
                {
                    if (item1.TrangThai == true)
                    {
                        Da_SVLuaChon da_SVLuaChon = new Da_SVLuaChon();
                        da_SVLuaChon.Ma_DAN = item1.MA_DAN;
                        da_SVLuaChon.MaDeThi = examQuestion.MaDeThi;
                        db.Da_SVLuaChon.Add(da_SVLuaChon);
                        db.SaveChanges();
                    }

                }

            }

        }

        internal void UpdateDsThi(Phong_Thi phong, De_Thi deThi,string Masv,string trangthai)
        {
          var dSSV= db.DS_SVThi.SingleOrDefault(x => x.MaPhong == phong.MaPhong && x.Ma_SV.Equals(Masv));
            dSSV.MaDeThi = deThi.MaDeThi;
            dSSV.TrangThai = trangthai;
            db.SaveChanges();

        }

        public List<Bo_De> ListALLChapterStudy(long mamom,string tk)
        {
            var bode= db.Bo_De.Where(x => x.Ma_Mon==mamom &&x.NguoiDuyet== tk &&x.TrangThai==true).ToList();
        
            return bode;
        }
    
        public List<Bo_De> ListALLChapterStudy()
        {
            var bode = db.Bo_De.Where(x=>x.Xoa==true).ToList();
         
            return bode;
        }
        public Bo_De ChapterStudy(long id)
        {
            var bode = db.Bo_De.Find(id);
            bode.MonHoc = new MonHocDao().Subject(long.Parse(bode.Ma_Mon.ToString()));
            ListChapterQuestion(bode);
            return bode;
        }
        
        public void ListChapterQuestion(Bo_De bo_De)
        {
           bo_De.CauHois = db.CauHois.Where(x => x.Ma_BoDe==bo_De.Ma_BoDe).ToList();
         
        }




    }
}