using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using TracNghiemOnline.Model;

namespace TracNghiemOnline.Modell.Dao
{
    public class CauHoiDao
    {
        TracNghiemOnlineDB tracNghiem = new TracNghiemOnlineDB();
        public List<Kho_CauHoi> ListQuestion(long id)
        {
            return tracNghiem.Kho_CauHoi.Where(x => x.Ma_Chuong == id).ToList();
        }

     

        public Kho_CauHoi Question(long id)
        {
            var Question = tracNghiem.Kho_CauHoi.Find(id);
            Question.Dap_AN = lisALLAnsWer(Question.Ma_CauHoi);
            return Question ;
        }

        public long CreatrQuestion(Kho_CauHoi QuesTion)
        {
            try
            {
                
                tracNghiem.Kho_CauHoi.Add(QuesTion);
               
                tracNghiem.SaveChanges();
            }
            catch (Exception e){
    
                return 0 ; }


            return tracNghiem.Kho_CauHoi.Select(x=>x).ToList().Last().Ma_CauHoi;
        }

        public  void UpdateQuestion(Kho_CauHoi QuesTion)
        {
            var cmd = Question(QuesTion.Ma_CauHoi);
            cmd.MucDo = QuesTion.MucDo;
            cmd.NoiDung = QuesTion.NoiDung;
            cmd.HinhAnh = QuesTion.MucDo;
            tracNghiem.SaveChanges();

        }

        public bool DeleteQuestion(Kho_CauHoi QuesTion)
        {
            try
            {
                tracNghiem.Kho_CauHoi.Remove(QuesTion);
                tracNghiem.SaveChanges();
            }
            catch
            {
                return false;
            }
            return true;
            
        }

        internal List<Kho_CauHoi> Nuberofquestion(long ma_Chuong, string v)
        {
            List<Kho_CauHoi> kho_CauHois = tracNghiem.Kho_CauHoi.Where(x => x.Ma_Chuong == ma_Chuong && x.MucDo.Equals(v)).ToList();
            foreach (var item in kho_CauHois)
            {
                item.Dap_AN = lisALLAnsWer(item.Ma_CauHoi);
            }
            return kho_CauHois;
        }

        private ICollection<Dap_AN> lisALLAnsWer(long ma_CauHoi)
        {
            return tracNghiem.Dap_AN.Where(x => x.Ma_CauHoi == ma_CauHoi).ToList();
        }

        internal void CreateTopic(Bo_De bo_De1, List<SoLuongChuong> soluong)
        {
            bo_De1.CauHois = new List<CauHoi>();

            foreach (var item in soluong)
            {
                Random(bo_De1, item.nhanBiet,item.Chuong, "Nhận Biết");
                Random(bo_De1, item.thongHieu, item.Chuong, "Thông Hiểu");
                Random(bo_De1, item.vandung, item.Chuong, "Vận Dụng");
                Random(bo_De1, item.vandungcao, item.Chuong, "Vận Dụng Cao");
            }
        }

        private void Random(Bo_De bo_De1, string sl,Chuong_Hoc chuong, string v)
        {
            List<Kho_CauHoi> kho_CauHois = Nuberofquestion(chuong.Ma_Chuong, v);
            Random random = new Random();
            if (kho_CauHois.Count > 0)
            {
                for (int i = 0; i < int.Parse(sl); i++)
                {
                    int vt = random.Next(kho_CauHois.Count);
                    CauHoi cauHoi = new CauHoi();
                    cauHoi.Ma_BoDe = bo_De1.Ma_BoDe;
                    cauHoi.Ma_CauHoi = kho_CauHois[vt].Ma_CauHoi;
                    cauHoi.Kho_CauHoi = kho_CauHois[vt];
                    bo_De1.CauHois.Add(cauHoi);
                    kho_CauHois.RemoveAt(vt);
                }
            }
        
        }
    }
}