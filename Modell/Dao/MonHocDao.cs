using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Web;
using TracNghiemOnline.Model;

namespace TracNghiemOnline.Modell.Dao
{
    public class MonHocDao
    {
        TracNghiemOnlineDB db = new TracNghiemOnlineDB();

        public bool CreateSubject (Modell.MonHoc  monHoc)
        {
            db.MonHocs.Add(monHoc);
            db.SaveChanges();
            return true;
        }
        public List<Modell.MonHoc> lisALL(string mabomon)
        {
            return db.MonHocs.Where(x => x.MaBoMon.Equals(mabomon)).ToList();
        }
        public bool UpdateSubject(Modell.MonHoc monHoc)
        {
          var cmd =  db.MonHocs.Find(monHoc.Ma_Mon);
            cmd.TenMon = monHoc.TenMon;
            db.SaveChanges();
            return true;
        }
        public bool DeleteSubject(Modell.MonHoc monHoc)
        {
            db.MonHocs.Remove(monHoc);
            db.SaveChanges();
            return true;
        }
        public Modell.MonHoc  Subject (long mamon)
        {
            return db.MonHocs.Find(mamon);
        }

        public List<Chuong_Hoc> ListChapterStudy(long mamon)
        {
            return db.Chuong_Hoc.Where(x => x.Ma_Mon == mamon && x.TrangThai==1).ToList();
        }
        public Chuong_Hoc ChapterStudy (long idChapter)
        {
            return db.Chuong_Hoc.Find(idChapter);
        }
        public bool CreateChapterStudy(Chuong_Hoc ChapterStudy)
        {
            db.Chuong_Hoc.Add(ChapterStudy);
            db.SaveChanges();
            return true;
        }
        public bool UpdateChapterStudy(Chuong_Hoc ChapterStudy)
        {
            var cmd = db.Chuong_Hoc.Find(ChapterStudy);
            cmd.TenChuong = ChapterStudy.TenChuong;
            db.SaveChanges();
            return true;
            
        }

        public bool DeleteChapterStudy(Chuong_Hoc ChapterStudy)
        {
             db.Chuong_Hoc.Remove(ChapterStudy);
            db.SaveChanges();
            return true;

        }

   

    }
}