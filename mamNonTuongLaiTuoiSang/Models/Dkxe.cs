using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class Dkxe
    {
        public string IdHs { get; set; } = null!;
        public string IdXeBus { get; set; } = null!;
        public string? DiaChiDonTra { get; set; }

        public virtual HocSinh IdHsNavigation { get; set; } = null!;
        public virtual XeBu IdXeBusNavigation { get; set; } = null!;
    }
}
