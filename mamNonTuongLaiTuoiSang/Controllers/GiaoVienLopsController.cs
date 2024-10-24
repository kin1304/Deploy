using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using mamNonTuongLaiTuoiSang.Models;

namespace mamNonTuongLaiTuoiSang.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GiaoVienLopsController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public GiaoVienLopsController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/Lops/GiaoViens/{maSt} (api/GiaoVienLops/GiaoViens/ST006) hãy thử dòng này vì trong database của mình mới chit có 1 giáo viên
        [HttpGet("GiaoViens/{maSt}")]
        public async Task<ActionResult<NhanVien>> GetNhanVienLop(string maSt)
        {
            // Kiểm tra nếu context không tồn tại
            if (_context.Lops == null || _context.NhanViens == null || _context.ChucVus == null)
            {
                return NotFound("Dữ liệu không tồn tại.");
            }

            // Tìm nhân viên có mã số và chức vụ "Giáo viên"
            var nhanVien = await _context.NhanViens
                .Include(nv => nv.ChucVu) // Bao gồm thông tin chức vụ
                .Include(nv => nv.Lops)   // Bao gồm thông tin lớp học
                .FirstOrDefaultAsync(nv => nv.MaSt == maSt && nv.TenCv == "GiaoVien");

            // Nếu không tìm thấy nhân viên hoặc không phải là giáo viên
            if (nhanVien == null)
            {
                return NotFound("Không tìm thấy nhân viên với mã số đã cho hoặc nhân viên không phải là giáo viên.");
            }

            // Chuyển đổi thông tin sang DTO để trả về
            var nhanVienLopDto = new NhanVien
            {
                MaSt = nhanVien.MaSt,
                HoTen = nhanVien.HoTen,
                DiaChi = nhanVien.DiaChi,
                NamSinh = nhanVien.NamSinh,
                GioiTinh = nhanVien.GioiTinh,
                Email = nhanVien.Email,
                Sdt = nhanVien.Sdt,
                TenCv = nhanVien.TenCv,
                Lops = nhanVien.Lops.Select(l => new Lop
                {
                    IdLop = l.IdLop,
                    TenLop = l.TenLop,
                    SiSo = l.SiSo
                }).ToList()  // Trả về danh sách lớp học
            };

            return Ok(nhanVienLopDto);
        }
    }
}
