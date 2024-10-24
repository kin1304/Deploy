using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class HoaDon
    {
        public HoaDon()
        {
            HoaDonDichVus = new HashSet<HoaDonDichVu>();
            HoaDonKhoaHocs = new HashSet<HoaDonKhoaHoc>();
        }

        public string IdHd { get; set; } = null!;
        public DateTime? NgayLap { get; set; }
        public DateTime? NgayHetHan { get; set; }
        public decimal? SoTien { get; set; }
        public string? TrangThai { get; set; }
        public string? MaSt { get; set; }
        public string? IdHs { get; set; }

        public virtual HocSinh? IdHsNavigation { get; set; }
        public virtual NhanVien? MaStNavigation { get; set; }
        public virtual ICollection<HoaDonDichVu> HoaDonDichVus { get; set; }
        public virtual ICollection<HoaDonKhoaHoc> HoaDonKhoaHocs { get; set; }
    }
}
