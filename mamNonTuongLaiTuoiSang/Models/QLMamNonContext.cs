using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class QLMamNonContext : DbContext
    {
        public QLMamNonContext()
        {
        }

        public QLMamNonContext(DbContextOptions<QLMamNonContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChucVu> ChucVus { get; set; } = null!;
        public virtual DbSet<DichVu> DichVus { get; set; } = null!;
        public virtual DbSet<DiemDanh> DiemDanhs { get; set; } = null!;
        public virtual DbSet<Dkxe> Dkxes { get; set; } = null!;
        public virtual DbSet<GiaoVien> GiaoViens { get; set; } = null!;
        public virtual DbSet<HoaDon> HoaDons { get; set; } = null!;
        public virtual DbSet<HoaDonDichVu> HoaDonDichVus { get; set; } = null!;
        public virtual DbSet<HoaDonKhoaHoc> HoaDonKhoaHocs { get; set; } = null!;
        public virtual DbSet<HocSinh> HocSinhs { get; set; } = null!;
        public virtual DbSet<HocSinhLop> HocSinhLops { get; set; } = null!;
        public virtual DbSet<KhoaHoc> KhoaHocs { get; set; } = null!;
        public virtual DbSet<Lop> Lops { get; set; } = null!;
        public virtual DbSet<MonHoc> MonHocs { get; set; } = null!;
        public virtual DbSet<NgoaiKhoa> NgoaiKhoas { get; set; } = null!;
        public virtual DbSet<NhanVien> NhanViens { get; set; } = null!;
        public virtual DbSet<PhuHuynh> PhuHuynhs { get; set; } = null!;
        public virtual DbSet<TiemChung> TiemChungs { get; set; } = null!;
        public virtual DbSet<TinTuc> TinTucs { get; set; } = null!;
        public virtual DbSet<Tkb> Tkbs { get; set; } = null!;
        public virtual DbSet<VacXin> VacXins { get; set; } = null!;
        public virtual DbSet<Voucher> Vouchers { get; set; } = null!;
        public virtual DbSet<VoucherCuaPh> VoucherCuaPhs { get; set; } = null!;
        public virtual DbSet<XeBu> XeBus { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=NHA\\SQLEXPRESS;Initial Catalog=QLMamNon;Integrated Security=True;Encrypt=True;Trust Server Certificate=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChucVu>(entity =>
            {
                entity.HasKey(e => new { e.TenCv, e.ViTri })
                    .HasName("PK__ChucVu__0025B48122057C78");

                entity.ToTable("ChucVu");

                entity.Property(e => e.TenCv)
                    .HasMaxLength(100)
                    .HasColumnName("TenCV");

                entity.Property(e => e.ViTri).HasMaxLength(100);

                entity.Property(e => e.HeSoLuong).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<DichVu>(entity =>
            {
                entity.HasKey(e => e.IdDv)
                    .HasName("PK__DichVu__B77398B489F9558D");

                entity.ToTable("DichVu");

                entity.Property(e => e.IdDv)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IdDV")
                    .IsFixedLength();

                entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TenDv)
                    .HasMaxLength(100)
                    .HasColumnName("TenDV");

                entity.Property(e => e.ThoiHan).HasMaxLength(50);
            });

            modelBuilder.Entity<DiemDanh>(entity =>
            {
                entity.HasKey(e => e.IdDd)
                    .HasName("PK__DiemDanh__B77398A6FD69E830");

                entity.ToTable("DiemDanh");

                entity.Property(e => e.IdDd)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IdDD")
                    .IsFixedLength();

                entity.Property(e => e.IdHs)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IdHS")
                    .IsFixedLength();

                entity.Property(e => e.IdLop)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Ngay).HasColumnType("date");

                entity.HasOne(d => d.Id)
                    .WithMany(p => p.DiemDanhs)
                    .HasForeignKey(d => new { d.IdHs, d.IdLop })
                    .HasConstraintName("FK__DiemDanh__06CD04F7");
            });

            modelBuilder.Entity<Dkxe>(entity =>
            {
                entity.HasKey(e => new { e.IdHs, e.IdXeBus })
                    .HasName("PK__DKXe__D704D72C839BA317");

                entity.ToTable("DKXe");

                entity.Property(e => e.IdHs)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IdHS")
                    .IsFixedLength();

                entity.Property(e => e.IdXeBus)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DiaChiDonTra).HasMaxLength(100);

                entity.HasOne(d => d.IdHsNavigation)
                    .WithMany(p => p.Dkxes)
                    .HasForeignKey(d => d.IdHs)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DKXe__IdHS__44FF419A");

                entity.HasOne(d => d.IdXeBusNavigation)
                    .WithMany(p => p.Dkxes)
                    .HasForeignKey(d => d.IdXeBus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__DKXe__IdXeBus__440B1D61");
            });

            modelBuilder.Entity<GiaoVien>(entity =>
            {
                entity.HasKey(e => e.MaSt)
                    .HasName("PK__GiaoVien__2725081828AAA3C1");

                entity.ToTable("GiaoVien");

                entity.Property(e => e.MaSt)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MaST")
                    .IsFixedLength();

                entity.Property(e => e.SaoDanhGia).HasColumnType("decimal(2, 1)");

                entity.Property(e => e.TrinhDoChuyenMon).HasMaxLength(100);

                entity.HasOne(d => d.MaStNavigation)
                    .WithOne(p => p.GiaoVien)
                    .HasForeignKey<GiaoVien>(d => d.MaSt)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GiaoVien__SaoDan__59FA5E80");
            });

            modelBuilder.Entity<HoaDon>(entity =>
            {
                entity.HasKey(e => e.IdHd)
                    .HasName("PK__HoaDon__B773FA29CA7430BA");

                entity.ToTable("HoaDon");

                entity.Property(e => e.IdHd)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IdHD")
                    .IsFixedLength();

                entity.Property(e => e.IdHs)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IdHS")
                    .IsFixedLength();

                entity.Property(e => e.MaSt)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MaST")
                    .IsFixedLength();

                entity.Property(e => e.NgayHetHan).HasColumnType("date");

                entity.Property(e => e.NgayLap).HasColumnType("date");

                entity.Property(e => e.SoTien).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TrangThai).HasMaxLength(20);

                entity.HasOne(d => d.IdHsNavigation)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.IdHs)
                    .HasConstraintName("FK__HoaDon__IdHS__5DCAEF64");

                entity.HasOne(d => d.MaStNavigation)
                    .WithMany(p => p.HoaDons)
                    .HasForeignKey(d => d.MaSt)
                    .HasConstraintName("FK__HoaDon__MaST__5CD6CB2B");
            });

            modelBuilder.Entity<HoaDonDichVu>(entity =>
            {
                entity.HasKey(e => new { e.IdHd, e.IdDv })
                    .HasName("PK__HoaDon_D__EC04C3A23F8DA051");

                entity.ToTable("HoaDon_DichVu");

                entity.Property(e => e.IdHd)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IdHD")
                    .IsFixedLength();

                entity.Property(e => e.IdDv)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IdDV")
                    .IsFixedLength();

                entity.Property(e => e.NgayDk)
                    .HasColumnType("date")
                    .HasColumnName("NgayDK");

                entity.Property(e => e.NgayHetHan).HasColumnType("date");

                entity.HasOne(d => d.IdDvNavigation)
                    .WithMany(p => p.HoaDonDichVus)
                    .HasForeignKey(d => d.IdDv)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HoaDon_Dic__IdDV__74AE54BC");

                entity.HasOne(d => d.IdHdNavigation)
                    .WithMany(p => p.HoaDonDichVus)
                    .HasForeignKey(d => d.IdHd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HoaDon_Dic__IdHD__73BA3083");
            });

            modelBuilder.Entity<HoaDonKhoaHoc>(entity =>
            {
                entity.HasKey(e => new { e.IdKh, e.IdHd })
                    .HasName("PK__HoaDon_K__3C04EE23FE969D59");

                entity.ToTable("HoaDon_KhoaHoc");

                entity.Property(e => e.IdKh)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IdKH")
                    .IsFixedLength();

                entity.Property(e => e.IdHd)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IdHD")
                    .IsFixedLength();

                entity.Property(e => e.NgayDk)
                    .HasColumnType("date")
                    .HasColumnName("NgayDK");

                entity.Property(e => e.NgayKt)
                    .HasColumnType("date")
                    .HasColumnName("NgayKT");

                entity.HasOne(d => d.IdHdNavigation)
                    .WithMany(p => p.HoaDonKhoaHocs)
                    .HasForeignKey(d => d.IdHd)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HoaDon_Kho__IdHD__6D0D32F4");

                entity.HasOne(d => d.IdKhNavigation)
                    .WithMany(p => p.HoaDonKhoaHocs)
                    .HasForeignKey(d => d.IdKh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HoaDon_Kho__IdKH__6C190EBB");
            });

            modelBuilder.Entity<HocSinh>(entity =>
            {
                entity.HasKey(e => e.IdHs)
                    .HasName("PK__HocSinh__B773FA38D933436F");

                entity.ToTable("HocSinh");

                entity.Property(e => e.IdHs)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IdHS")
                    .IsFixedLength();

                entity.Property(e => e.GioiTinh).HasMaxLength(10);

                entity.Property(e => e.IdPh)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IdPH")
                    .IsFixedLength();

                entity.Property(e => e.QuanHe).HasMaxLength(10);

                entity.Property(e => e.TenHs)
                    .HasMaxLength(100)
                    .HasColumnName("TenHS");

                entity.HasOne(d => d.IdPhNavigation)
                    .WithMany(p => p.HocSinhs)
                    .HasForeignKey(d => d.IdPh)
                    .HasConstraintName("FK__HocSinh__IdPH__412EB0B6");
            });

            modelBuilder.Entity<HocSinhLop>(entity =>
            {
                entity.HasKey(e => new { e.IdHs, e.IdLop })
                    .HasName("PK__HocSinh___77B6B7871C6A8BC0");

                entity.ToTable("HocSinh_Lop");

                entity.Property(e => e.IdHs)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IdHS")
                    .IsFixedLength();

                entity.Property(e => e.IdLop)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.DiemChuyenCan).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.IdHsNavigation)
                    .WithMany(p => p.HocSinhLops)
                    .HasForeignKey(d => d.IdHs)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HocSinh_Lo__IdHS__778AC167");

                entity.HasOne(d => d.IdLopNavigation)
                    .WithMany(p => p.HocSinhLops)
                    .HasForeignKey(d => d.IdLop)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__HocSinh_L__IdLop__787EE5A0");
            });

            modelBuilder.Entity<KhoaHoc>(entity =>
            {
                entity.HasKey(e => e.IdKh)
                    .HasName("PK__KhoaHoc__B773D1812C3EF6A5");

                entity.ToTable("KhoaHoc");

                entity.Property(e => e.IdKh)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IdKH")
                    .IsFixedLength();

                entity.Property(e => e.DonGia).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TenKh)
                    .HasMaxLength(100)
                    .HasColumnName("TenKH");

                entity.Property(e => e.ThoiHan).HasMaxLength(20);
            });

            modelBuilder.Entity<Lop>(entity =>
            {
                entity.HasKey(e => e.IdLop)
                    .HasName("PK__Lop__0C54DBFDA2E6C652");

                entity.ToTable("Lop");

                entity.Property(e => e.IdLop)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaSt)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MaST")
                    .IsFixedLength();

                entity.Property(e => e.TenLop).HasMaxLength(100);

                entity.HasOne(d => d.MaStNavigation)
                    .WithMany(p => p.Lops)
                    .HasForeignKey(d => d.MaSt)
                    .HasConstraintName("FK__Lop__MaST__47DBAE45");
            });

            modelBuilder.Entity<MonHoc>(entity =>
            {
                entity.HasKey(e => e.IdMh)
                    .HasName("PK__MonHoc__B773C1433E4F4AB9");

                entity.ToTable("MonHoc");

                entity.Property(e => e.IdMh)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IdMH")
                    .IsFixedLength();

                entity.Property(e => e.TenMh)
                    .HasMaxLength(50)
                    .HasColumnName("TenMH");
            });

            modelBuilder.Entity<NgoaiKhoa>(entity =>
            {
                entity.HasKey(e => e.IdNk)
                    .HasName("PK__NgoaiKho__B773C965EA66F81B");

                entity.ToTable("NgoaiKhoa");

                entity.Property(e => e.IdNk)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IdNK")
                    .IsFixedLength();

                entity.Property(e => e.NgayBatDau).HasColumnType("date");

                entity.Property(e => e.NgayKetThu).HasColumnType("date");

                entity.Property(e => e.TenNk)
                    .HasMaxLength(100)
                    .HasColumnName("TenNK");

                entity.HasMany(d => d.MaSts)
                    .WithMany(p => p.IdNks)
                    .UsingEntity<Dictionary<string, object>>(
                        "NgoaiKhoaGiaoVien",
                        l => l.HasOne<GiaoVien>().WithMany().HasForeignKey("MaSt").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__NgoaiKhoa___MaST__70DDC3D8"),
                        r => r.HasOne<NgoaiKhoa>().WithMany().HasForeignKey("IdNk").OnDelete(DeleteBehavior.ClientSetNull).HasConstraintName("FK__NgoaiKhoa___IdNK__6FE99F9F"),
                        j =>
                        {
                            j.HasKey("IdNk", "MaSt").HasName("PK__NgoaiKho__250199E40CF5A751");

                            j.ToTable("NgoaiKhoa_GiaoVien");

                            j.IndexerProperty<string>("IdNk").HasMaxLength(10).IsUnicode(false).HasColumnName("IdNK").IsFixedLength();

                            j.IndexerProperty<string>("MaSt").HasMaxLength(10).IsUnicode(false).HasColumnName("MaST").IsFixedLength();
                        });
            });

            modelBuilder.Entity<NhanVien>(entity =>
            {
                entity.HasKey(e => e.MaSt)
                    .HasName("PK__NhanVien__272508185B30D5E7");

                entity.ToTable("NhanVien");

                entity.Property(e => e.MaSt)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MaST")
                    .IsFixedLength();

                entity.Property(e => e.DiaChi).HasMaxLength(100);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HoTen).HasMaxLength(100);

                entity.Property(e => e.MatKhau)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Sdt)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("SDT");

                entity.Property(e => e.TenCv)
                    .HasMaxLength(100)
                    .HasColumnName("TenCV");

                entity.Property(e => e.ViTri).HasMaxLength(100);

                entity.HasOne(d => d.ChucVu)
                    .WithMany(p => p.NhanViens)
                    .HasForeignKey(d => new { d.TenCv, d.ViTri })
                    .HasConstraintName("FK_NV_CV");
            });

            modelBuilder.Entity<PhuHuynh>(entity =>
            {
                entity.HasKey(e => e.IdPh)
                    .HasName("PK__PhuHuynh__B7703B3BC0677421");

                entity.ToTable("PhuHuynh");

                entity.Property(e => e.IdPh)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IdPH")
                    .IsFixedLength();

                entity.Property(e => e.DiaChi).HasMaxLength(100);

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.HoTen).HasMaxLength(30);

                entity.Property(e => e.MatKhau)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.NgheNghiep).HasMaxLength(100);

                entity.Property(e => e.Sdt)
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("SDT");
            });

            modelBuilder.Entity<TiemChung>(entity =>
            {
                entity.HasKey(e => new { e.IdVacXin, e.IdHs })
                    .HasName("PK__TiemChun__C2684C40360D3849");

                entity.ToTable("TiemChung");

                entity.Property(e => e.IdVacXin)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.IdHs)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IdHS")
                    .IsFixedLength();

                entity.Property(e => e.NgayTiem).HasColumnType("date");

                entity.HasOne(d => d.IdHsNavigation)
                    .WithMany(p => p.TiemChungs)
                    .HasForeignKey(d => d.IdHs)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TiemChung__IdHS__534D60F1");

                entity.HasOne(d => d.IdVacXinNavigation)
                    .WithMany(p => p.TiemChungs)
                    .HasForeignKey(d => d.IdVacXin)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TiemChung__IdVac__52593CB8");
            });

            modelBuilder.Entity<TinTuc>(entity =>
            {
                entity.HasKey(e => e.IdTinTuc)
                    .HasName("PK__TinTuc__B782967680B54D17");

                entity.ToTable("TinTuc");

                entity.Property(e => e.IdTinTuc)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaSt)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MaST")
                    .IsFixedLength();

                entity.HasOne(d => d.MaStNavigation)
                    .WithMany(p => p.TinTucs)
                    .HasForeignKey(d => d.MaSt)
                    .HasConstraintName("FK__TinTuc__MaST__693CA210");
            });

            modelBuilder.Entity<Tkb>(entity =>
            {
                entity.HasKey(e => new { e.IdLop, e.Ngay, e.CaHoc })
                    .HasName("PK__TKB__3F89372EB653714F");

                entity.ToTable("TKB");

                entity.Property(e => e.IdLop)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Ngay).HasMaxLength(15);

                entity.Property(e => e.CaHoc).HasMaxLength(15);

                entity.Property(e => e.IdMh)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IdMH")
                    .IsFixedLength();

                entity.HasOne(d => d.IdLopNavigation)
                    .WithMany(p => p.Tkbs)
                    .HasForeignKey(d => d.IdLop)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__TKB__IdLop__4D94879B");

                entity.HasOne(d => d.IdMhNavigation)
                    .WithMany(p => p.Tkbs)
                    .HasForeignKey(d => d.IdMh)
                    .HasConstraintName("FK__TKB__IdMH__4CA06362");
            });

            modelBuilder.Entity<VacXin>(entity =>
            {
                entity.HasKey(e => e.IdVacXin)
                    .HasName("PK__VacXin__591F73E39AA73117");

                entity.ToTable("VacXin");

                entity.Property(e => e.IdVacXin)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.LoaiVacXin).HasMaxLength(50);

                entity.Property(e => e.NuocSx)
                    .HasMaxLength(50)
                    .HasColumnName("NuocSX");
            });

            modelBuilder.Entity<Voucher>(entity =>
            {
                entity.HasKey(e => e.IdVoucher)
                    .HasName("PK__Voucher__329D557EB10A3292");

                entity.ToTable("Voucher");

                entity.Property(e => e.IdVoucher)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.MaSt)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MaST")
                    .IsFixedLength();

                entity.Property(e => e.NgayHetHan).HasColumnType("date");

                entity.Property(e => e.NgayTao).HasColumnType("date");

                entity.Property(e => e.PhanTramGiam).HasColumnType("decimal(5, 2)");

                entity.HasOne(d => d.MaStNavigation)
                    .WithMany(p => p.Vouchers)
                    .HasForeignKey(d => d.MaSt)
                    .HasConstraintName("FK__Voucher__MaST__628FA481");
            });

            modelBuilder.Entity<VoucherCuaPh>(entity =>
            {
                entity.HasKey(e => new { e.IdPh, e.IdVoucher })
                    .HasName("PK__VoucherC__4459EE6C6E193682");

                entity.ToTable("VoucherCuaPH");

                entity.Property(e => e.IdPh)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("IdPH")
                    .IsFixedLength();

                entity.Property(e => e.IdVoucher)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.HasOne(d => d.IdPhNavigation)
                    .WithMany(p => p.VoucherCuaPhs)
                    .HasForeignKey(d => d.IdPh)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__VoucherCua__IdPH__656C112C");

                entity.HasOne(d => d.IdVoucherNavigation)
                    .WithMany(p => p.VoucherCuaPhs)
                    .HasForeignKey(d => d.IdVoucher)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__VoucherCu__IdVou__66603565");
            });

            modelBuilder.Entity<XeBu>(entity =>
            {
                entity.HasKey(e => e.IdXeBus)
                    .HasName("PK__XeBus__0772D14722855D22");

                entity.Property(e => e.IdXeBus)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.BienSo)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.MaSt)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("MaST")
                    .IsFixedLength();

                entity.Property(e => e.ViTri).HasMaxLength(100);

                entity.HasOne(d => d.MaStNavigation)
                    .WithMany(p => p.XeBus)
                    .HasForeignKey(d => d.MaSt)
                    .HasConstraintName("FK__XeBus__MaST__3C69FB99");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
