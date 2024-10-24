using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class Lop
    {
        public Lop()
        {
            HocSinhLops = new HashSet<HocSinhLop>();
            Tkbs = new HashSet<Tkb>();
        }

        public string IdLop { get; set; } = null!;
        public string? TenLop { get; set; }
        public int? SiSo { get; set; }
        public string? MaSt { get; set; }
        
        public virtual NhanVien? MaStNavigation { get; set; }
        public virtual ICollection<HocSinhLop> HocSinhLops { get; set; }
        public virtual ICollection<Tkb> Tkbs { get; set; }
    }
}
