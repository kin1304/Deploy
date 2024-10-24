using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class TinTuc
    {
        public string IdTinTuc { get; set; } = null!;
        public string? TieuDe { get; set; }
        public string? NoiDung { get; set; }
        public string? Anh { get; set; }
        public string? MaSt { get; set; }

        public virtual NhanVien? MaStNavigation { get; set; }
    }
}
