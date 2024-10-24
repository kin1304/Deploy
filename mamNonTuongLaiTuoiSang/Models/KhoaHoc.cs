using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class KhoaHoc
    {
        public KhoaHoc()
        {
            HoaDonKhoaHocs = new HashSet<HoaDonKhoaHoc>();
        }

        public string IdKh { get; set; } = null!;
        public string? TenKh { get; set; }
        public decimal? DonGia { get; set; }
        public string? MoTa { get; set; }
        public int? SoLuong { get; set; }
        public string? ThoiHan { get; set; }

        public virtual ICollection<HoaDonKhoaHoc> HoaDonKhoaHocs { get; set; }
    }
}
