using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class HoaDonKhoaHoc
    {
        public string IdKh { get; set; } = null!;
        public string IdHd { get; set; } = null!;
        public int? SoLuong { get; set; }
        public DateTime? NgayDk { get; set; }
        public DateTime? NgayKt { get; set; }

        public virtual HoaDon IdHdNavigation { get; set; } = null!;
        public virtual KhoaHoc IdKhNavigation { get; set; } = null!;
    }
}
