namespace mamNonTuongLaiTuoiSang.Models
{
    public class TeacherInfo
    {
        //gv
        public string MaSt { get; set; } = null!;
        public string? TrinhDoChuyenMon { get; set; }
        public decimal? SaoDanhGia { get; set; }

        //nv
        public string? HoTen { get; set; }
        public string? DiaChi { get; set; }
        public int? NamSinh { get; set; }
        public bool? GioiTinh { get; set; }
        public int? NamLam { get; set; }
        public string? Email { get; set; }
        public string? Sdt { get; set; }
        public string? TenCv { get; set; }
        public string? ViTri { get; set; }
    }
}
