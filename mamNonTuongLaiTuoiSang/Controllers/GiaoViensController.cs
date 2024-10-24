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
    public class GiaoViensController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public GiaoViensController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/GiaoViens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GiaoVien>>> GetGiaoViens()
        {
          if (_context.GiaoViens == null)
          {
              return NotFound();
          }
            return await _context.GiaoViens.ToListAsync();
        }

        /// GET: api/GiaoViens/{maSt} (hãy gán link này api/GiaoViens/St006 vì trong database mới thêm thằng này là tên công việc giáo viên)
        [HttpGet("{maSt}")]
        public async Task<ActionResult<NhanVien>> GetNhanVienByGiaoVien(string maSt)
        {
            // Kiểm tra xem _context có null không
            if (_context.GiaoViens == null || _context.NhanViens == null)
            {
                return NotFound("Dữ liệu không tồn tại.");
            }

            // Tìm giáo viên theo mã số
            var giaoVien = await _context.GiaoViens
                .Include(g => g.MaStNavigation) // Bao gồm thông tin nhân viên
                .FirstOrDefaultAsync(g => g.MaSt == maSt);

            // Nếu không tìm thấy giáo viên
            if (giaoVien == null || giaoVien.MaStNavigation.TenCv != "GiaoVien")
            {
                return NotFound("Không tìm thấy giáo viên với mã số đã cho hoặc nhân viên không phải là giáo viên.");
            }

            // Chuyển đổi thông tin sang DTO
            var nhanVienDto = new NhanVien
            {
                MaSt = giaoVien.MaStNavigation.MaSt,
                HoTen = giaoVien.MaStNavigation.HoTen,
                MatKhau=giaoVien.MaStNavigation.MatKhau,
                DiaChi = giaoVien.MaStNavigation.DiaChi,
                NamSinh = giaoVien.MaStNavigation.NamSinh,
                GioiTinh = giaoVien.MaStNavigation.GioiTinh,
                Email = giaoVien.MaStNavigation.Email,
                Sdt = giaoVien.MaStNavigation.Sdt,
                TenCv = giaoVien.MaStNavigation.TenCv
            };

            return Ok(nhanVienDto);
        }

        // PUT: api/GiaoViens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGiaoVien(string id, GiaoVien giaoVien)
        {
            _context.Entry(giaoVien).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GiaoVienExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/GiaoViens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<GiaoVien>> PostGiaoVien(GiaoVien giaoVien)
        {
          if (_context.GiaoViens == null)
          {
              return Problem("Entity set 'QLMamNonContext.GiaoViens'  is null.");
          }
            _context.GiaoViens.Add(giaoVien);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (GiaoVienExists(giaoVien.MaSt))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetGiaoVien", new { id = giaoVien.MaSt }, giaoVien);
        }

        // DELETE: api/GiaoViens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGiaoVien(string id)
        {
            if (_context.GiaoViens == null)
            {
                return NotFound();
            }
            var giaoVien = await _context.GiaoViens.FindAsync(id);
            if (giaoVien == null)
            {
                return NotFound();
            }

            _context.GiaoViens.Remove(giaoVien);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool GiaoVienExists(string id)
        {
            return (_context.GiaoViens?.Any(e => e.MaSt == id)).GetValueOrDefault();
        }
    }
}
