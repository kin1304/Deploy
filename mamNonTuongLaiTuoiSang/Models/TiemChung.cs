using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class TiemChung
    {
        public string IdVacXin { get; set; } = null!;
        public string IdHs { get; set; } = null!;
        public DateTime? NgayTiem { get; set; }

        public virtual HocSinh IdHsNavigation { get; set; } = null!;
        public virtual VacXin IdVacXinNavigation { get; set; } = null!;
    }
}
