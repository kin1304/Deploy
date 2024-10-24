using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class HocSinh
    {
        public HocSinh()
        {
            Dkxes = new HashSet<Dkxe>();
            HoaDons = new HashSet<HoaDon>();
            HocSinhLops = new HashSet<HocSinhLop>();
            TiemChungs = new HashSet<TiemChung>();
        }

        public string IdHs { get; set; } = null!;
        public string? TenHs { get; set; }
        public string? GioiTinh { get; set; }
        public int? NamSinh { get; set; }
        public string? IdPh { get; set; }
        public string? QuanHe { get; set; }
        public decimal? CanNang { get; set; }
        public decimal? ChieuCao { get; set; }

        public virtual PhuHuynh? IdPhNavigation { get; set; }
        public virtual ICollection<Dkxe> Dkxes { get; set; }
        public virtual ICollection<HoaDon> HoaDons { get; set; }
        public virtual ICollection<HocSinhLop> HocSinhLops { get; set; }
        public virtual ICollection<TiemChung> TiemChungs { get; set; }
    }
}
