using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class VacXin
    {
        public VacXin()
        {
            TiemChungs = new HashSet<TiemChung>();
        }

        public string IdVacXin { get; set; } = null!;
        public string? NuocSx { get; set; }
        public string? LoaiVacXin { get; set; }

        public virtual ICollection<TiemChung> TiemChungs { get; set; }
    }
}
