using System;
using System.Collections.Generic;

namespace mamNonTuongLaiTuoiSang.Models
{
    public partial class NhanVien
    {
        public NhanVien()
        {
            HoaDons = new HashSet<HoaDon>();
            Lops = new HashSet<Lop>();
            TinTucs = new HashSet<TinTuc>();
            Vouchers = new HashSet<Voucher>();
            XeBus = new HashSet<XeBu>();
        }

        public string MaSt { get; set; } = null!;
        public string? HoTen { get; set; }
        public string? MatKhau { get; set; }
        public string? DiaChi { get; set; }
        public int? NamSinh { get; set; }
        public bool? GioiTinh { get; set; }
        public int? NamLam { get; set; }
        public string? Email { get; set; }
        public string? Sdt { get; set; }
        public string? TenCv { get; set; }
        public string? ViTri { get; set; }

        public virtual ChucVu? ChucVu { get; set; }
        public virtual GiaoVien? GiaoVien { get; set; }
        public virtual ICollection<HoaDon> HoaDons { get; set; }
        public virtual ICollection<Lop> Lops { get; set; }
        public virtual ICollection<TinTuc> TinTucs { get; set; }
        public virtual ICollection<Voucher> Vouchers { get; set; }
        public virtual ICollection<XeBu> XeBus { get; set; }
    }
}
