using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class XeBu
    {
        public XeBu()
        {
            Dkxes = new HashSet<Dkxe>();
        }

        public string IdXeBus { get; set; } = null!;
        public string? BienSo { get; set; }
        public string? ViTri { get; set; }
        public string? MaSt { get; set; }

        public virtual NhanVien? MaStNavigation { get; set; }
        public virtual ICollection<Dkxe> Dkxes { get; set; }
    }
}
