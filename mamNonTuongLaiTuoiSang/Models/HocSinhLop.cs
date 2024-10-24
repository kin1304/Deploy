using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class HocSinhLop
    {
        public HocSinhLop()
        {
            DiemDanhs = new HashSet<DiemDanh>();
        }

        public string IdHs { get; set; } = null!;
        public string IdLop { get; set; } = null!;
        public decimal? DiemChuyenCan { get; set; }

        public virtual HocSinh IdHsNavigation { get; set; } = null!;
        public virtual Lop IdLopNavigation { get; set; } = null!;
        public virtual ICollection<DiemDanh> DiemDanhs { get; set; }
    }
}
