using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace TracNghiemOnline.Modell
{
    public partial class TracNghiemOnlineDB : DbContext
    {
        public TracNghiemOnlineDB()
            : base("name=TracNghiemOnlineDB")
        {
        }

        public virtual DbSet<Bo_De> Bo_De { get; set; }
        public virtual DbSet<BoMon> BoMons { get; set; }
        public virtual DbSet<CauHoi> CauHois { get; set; }
        public virtual DbSet<CauHoiDeThi> CauHoiDeThis { get; set; }
        public virtual DbSet<Chuong_Hoc> Chuong_Hoc { get; set; }
        public virtual DbSet<CT_Dethi> CT_Dethi { get; set; }
        public virtual DbSet<Da_SVLuaChon> Da_SVLuaChon { get; set; }
        public virtual DbSet<Danh_Gia> Danh_Gia { get; set; }
        public virtual DbSet<Dap_AN> Dap_AN { get; set; }
        public virtual DbSet<De_Thi> De_Thi { get; set; }
        public virtual DbSet<DS_LopHP> DS_LopHP { get; set; }
        public virtual DbSet<DS_SVThi> DS_SVThi { get; set; }
        public virtual DbSet<GiaoVien> GiaoViens { get; set; }
        public virtual DbSet<KetQuaThi> KetQuaThis { get; set; }
        public virtual DbSet<Kho_CauHoi> Kho_CauHoi { get; set; }
        public virtual DbSet<KiThi> KiThis { get; set; }
        public virtual DbSet<LichNop> LichNops { get; set; }
        public virtual DbSet<Lop> Lops { get; set; }
        public virtual DbSet<LopHocPhan> LopHocPhans { get; set; }
        public virtual DbSet<MonHoc> MonHocs { get; set; }
        public virtual DbSet<Nganh> Nganhs { get; set; }
        public virtual DbSet<Phong_Thi> Phong_Thi { get; set; }
        public virtual DbSet<SinhVien> SinhViens { get; set; }
        public virtual DbSet<TaiKhoan> TaiKhoans { get; set; }
        public virtual DbSet<BoDeOnTap> BoDeOnTaps { get; set; }
        public virtual DbSet<DSGV_ThucHien> DSGV_ThucHien { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bo_De>()
                .Property(e => e.Ma_NguoiTao)
                .IsFixedLength();

            modelBuilder.Entity<Bo_De>()
                .Property(e => e.NguoiDuyet)
                .IsFixedLength();

            modelBuilder.Entity<Bo_De>()
                .HasMany(e => e.BoDeOnTaps)
                .WithOptional(e => e.Bo_De)
                .HasForeignKey(e => e.MaBoDe);

            modelBuilder.Entity<Bo_De>()
                .HasMany(e => e.CauHois)
                .WithRequired(e => e.Bo_De)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Bo_De>()
                .HasMany(e => e.DSGV_ThucHien)
                .WithOptional(e => e.Bo_De)
                .HasForeignKey(e => e.MaDE);

            modelBuilder.Entity<Bo_De>()
                .HasMany(e => e.Phong_Thi)
                .WithOptional(e => e.Bo_De)
                .HasForeignKey(e => e.MaBoDe);

            modelBuilder.Entity<BoMon>()
                .Property(e => e.Ma_BoMon)
                .IsFixedLength();

            modelBuilder.Entity<BoMon>()
                .HasMany(e => e.GiaoViens)
                .WithOptional(e => e.BoMon)
                .HasForeignKey(e => e.MaBoMon);

            modelBuilder.Entity<BoMon>()
                .HasMany(e => e.MonHocs)
                .WithOptional(e => e.BoMon)
                .HasForeignKey(e => e.MaBoMon);

            modelBuilder.Entity<Chuong_Hoc>()
                .HasMany(e => e.Danh_Gia)
                .WithOptional(e => e.Chuong_Hoc)
                .HasForeignKey(e => e.MaChuong);

            modelBuilder.Entity<De_Thi>()
                .Property(e => e.Ma_SV)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<De_Thi>()
                .HasMany(e => e.Danh_Gia)
                .WithOptional(e => e.De_Thi)
                .HasForeignKey(e => e.MaDeThi);

            modelBuilder.Entity<De_Thi>()
                .HasMany(e => e.Danh_Gia1)
                .WithOptional(e => e.De_Thi1)
                .HasForeignKey(e => e.MaDeThi);

            modelBuilder.Entity<De_Thi>()
                .HasMany(e => e.KetQuaThis)
                .WithOptional(e => e.De_Thi)
                .HasForeignKey(e => e.Ma_DeThi);

            modelBuilder.Entity<DS_LopHP>()
                .Property(e => e.Ma_LOP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DS_LopHP>()
                .Property(e => e.MA_SV)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DS_SVThi>()
                .Property(e => e.Ma_SV)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DS_SVThi>()
                .Property(e => e.MaPhong)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<GiaoVien>()
                .Property(e => e.MaGV)
                .IsFixedLength();

            modelBuilder.Entity<GiaoVien>()
                .Property(e => e.MaBoMon)
                .IsFixedLength();

            modelBuilder.Entity<GiaoVien>()
                .HasMany(e => e.Bo_De)
                .WithOptional(e => e.GiaoVien)
                .HasForeignKey(e => e.Ma_NguoiTao);

            modelBuilder.Entity<GiaoVien>()
                .HasMany(e => e.LichNops)
                .WithOptional(e => e.GiaoVien)
                .HasForeignKey(e => e.MaBoMON);

            modelBuilder.Entity<GiaoVien>()
                .HasMany(e => e.Phong_Thi)
                .WithOptional(e => e.GiaoVien)
                .HasForeignKey(e => e.NguoiTao);

            modelBuilder.Entity<Kho_CauHoi>()
                .HasMany(e => e.CauHois)
                .WithRequired(e => e.Kho_CauHoi)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Kho_CauHoi>()
                .HasMany(e => e.CauHoiDeThis)
                .WithOptional(e => e.Kho_CauHoi)
                .HasForeignKey(e => e.MaCauHoi);

            modelBuilder.Entity<LichNop>()
                .Property(e => e.MaBoMON)
                .IsFixedLength();

            modelBuilder.Entity<LichNop>()
                .HasMany(e => e.DSGV_ThucHien)
                .WithOptional(e => e.LichNop)
                .HasForeignKey(e => e.MaLich);

            modelBuilder.Entity<LopHocPhan>()
                .Property(e => e.MaLop)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<LopHocPhan>()
                .Property(e => e.MaGV)
                .IsFixedLength();

            modelBuilder.Entity<LopHocPhan>()
                .Property(e => e.SiSo)
                .IsFixedLength();

            modelBuilder.Entity<LopHocPhan>()
                .HasMany(e => e.DS_LopHP)
                .WithOptional(e => e.LopHocPhan)
                .HasForeignKey(e => e.Ma_LOP);

            modelBuilder.Entity<LopHocPhan>()
                .HasMany(e => e.BoDeOnTaps)
                .WithOptional(e => e.LopHocPhan)
                .HasForeignKey(e => e.MaLopHP);

            modelBuilder.Entity<LopHocPhan>()
                .HasMany(e => e.Phong_Thi)
                .WithRequired(e => e.LopHocPhan)
                .HasForeignKey(e => e.MaLopHP)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MonHoc>()
                .Property(e => e.MaBoMon)
                .IsFixedLength();

            modelBuilder.Entity<MonHoc>()
                .HasMany(e => e.LichNops)
                .WithOptional(e => e.MonHoc)
                .HasForeignKey(e => e.MaMon);

            modelBuilder.Entity<MonHoc>()
                .HasMany(e => e.LopHocPhans)
                .WithOptional(e => e.MonHoc)
                .HasForeignKey(e => e.MaMon);

            modelBuilder.Entity<Phong_Thi>()
                .Property(e => e.MaPhong)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Phong_Thi>()
                .Property(e => e.NguoiTao)
                .IsFixedLength();

            modelBuilder.Entity<Phong_Thi>()
                .Property(e => e.MaLopHP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<Phong_Thi>()
                .Property(e => e.MaCanBo1)
                .IsFixedLength();

            modelBuilder.Entity<Phong_Thi>()
                .Property(e => e.MaCanBo2)
                .IsFixedLength();

            modelBuilder.Entity<Phong_Thi>()
                .HasMany(e => e.DS_SVThi)
                .WithRequired(e => e.Phong_Thi)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SinhVien>()
                .Property(e => e.MaSV)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<SinhVien>()
                .HasMany(e => e.De_Thi)
                .WithOptional(e => e.SinhVien)
                .HasForeignKey(e => e.Ma_SV);

            modelBuilder.Entity<SinhVien>()
                .HasMany(e => e.DS_LopHP)
                .WithOptional(e => e.SinhVien)
                .HasForeignKey(e => e.MA_SV);

            modelBuilder.Entity<SinhVien>()
                .HasMany(e => e.DS_SVThi)
                .WithRequired(e => e.SinhVien)
                .HasForeignKey(e => e.Ma_SV)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.TaiKhoan1)
                .IsFixedLength();

            modelBuilder.Entity<TaiKhoan>()
                .Property(e => e.MatKhau)
                .IsFixedLength();

            modelBuilder.Entity<TaiKhoan>()
                .HasMany(e => e.Bo_De)
                .WithOptional(e => e.TaiKhoan)
                .HasForeignKey(e => e.NguoiDuyet);

            modelBuilder.Entity<BoDeOnTap>()
                .Property(e => e.MaLopHP)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<DSGV_ThucHien>()
                .Property(e => e.MaGV)
                .IsFixedLength();
        }
    }
}
