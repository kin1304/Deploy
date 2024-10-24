using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class Tkb
    {
        public string IdLop { get; set; } = null!;
        public string Ngay { get; set; } = null!;
        public string? CaHoc { get; set; }
        public string? IdMh { get; set; }
        
        public virtual Lop? IdLopNavigation { get; set; }
        public virtual MonHoc? IdMhNavigation { get; set; }
    }
}
