using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class ChucVu
    {
        public ChucVu()
        {
            NhanViens = new HashSet<NhanVien>();
        }

        public string TenCv { get; set; } = null!;
        public string ViTri { get; set; } = null!;
        public int? LuongCoBan { get; set; }
        public decimal? HeSoLuong { get; set; }

        public virtual ICollection<NhanVien> NhanViens { get; set; }
    }
}
