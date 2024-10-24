using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class MonHoc
    {
        public MonHoc()
        {
            Tkbs = new HashSet<Tkb>();
        }

        public string IdMh { get; set; } = null!;
        public string? TenMh { get; set; }

        public virtual ICollection<Tkb> Tkbs { get; set; }
    }
}
