using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class HoaDonDichVu
    {
        public string IdHd { get; set; } = null!;
        public string IdDv { get; set; } = null!;
        public int? SoLuong { get; set; }
        public DateTime? NgayDk { get; set; }
        public DateTime? NgayHetHan { get; set; }

        public virtual DichVu IdDvNavigation { get; set; } = null!;
        public virtual HoaDon IdHdNavigation { get; set; } = null!;
    }
}
