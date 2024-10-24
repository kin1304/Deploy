using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class NgoaiKhoa
    {
        public NgoaiKhoa()
        {
            MaSts = new HashSet<GiaoVien>();
        }

        public string IdNk { get; set; } = null!;
        public string? TenNk { get; set; }
        public DateTime? NgayBatDau { get; set; }
        public DateTime? NgayKetThu { get; set; }
        public string? MoTa { get; set; }

        public virtual ICollection<GiaoVien> MaSts { get; set; }
    }
}
