using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class DiemDanh
    {
        public string IdDd { get; set; } = null!;
        public string? IdHs { get; set; }
        public string? IdLop { get; set; }
        public DateTime? Ngay { get; set; }
        public byte? TrangThai { get; set; }
        public byte? TrangThaiNghi { get; set; }

        public virtual HocSinhLop? Id { get; set; }
    }
}
