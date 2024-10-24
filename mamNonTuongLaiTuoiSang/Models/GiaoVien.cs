using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class GiaoVien
    {
        public GiaoVien()
        {
            IdNks = new HashSet<NgoaiKhoa>();
        }

        public string MaSt { get; set; } = null!;
        public string? TrinhDoChuyenMon { get; set; }
        public decimal? SaoDanhGia { get; set; }

        public virtual NhanVien MaStNavigation { get; set; } = null!;

        public virtual ICollection<NgoaiKhoa> IdNks { get; set; }
    }
}
