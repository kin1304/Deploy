using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class DichVu
    {
        public DichVu()
        {
            HoaDonDichVus = new HashSet<HoaDonDichVu>();
        }

        public string IdDv { get; set; } = null!;
        public string? TenDv { get; set; }
        public decimal? DonGia { get; set; }
        public string? MoTa { get; set; }
        public string? ThoiHan { get; set; }

        public virtual ICollection<HoaDonDichVu> HoaDonDichVus { get; set; }
    }
}
