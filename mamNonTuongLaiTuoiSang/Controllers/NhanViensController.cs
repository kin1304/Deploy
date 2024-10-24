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
    public class NhanViensController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public NhanViensController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/NhanViens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetNhanViens()
        {
          if (_context.NhanViens == null)
          {
              return BadRequest("Dữ liệu không tồn tại.");
          }

            return await _context.NhanViens.ToListAsync();
        }

        // GET: api/NhanViens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NhanVien>> GetNhanVien(string id)
        {
          if (_context.NhanViens == null)
          {
              return BadRequest("Dữ liệu không tồn tại.");
          }

            var nhanVien = await _context.NhanViens.FindAsync(id);

            if (nhanVien == null)
            {
                return NotFound();
            }

            return nhanVien;
        }
        // GET: api/NhanViens/ByChucVu/{TenCv} ( tìm thông tin thông qua tên chức vụ)
        [HttpGet("ByChucVu/{TenCv}")]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetNhanVienbyTenCv(string TenCv)
        {
            if (_context.NhanViens == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            var NhanViens = await _context.NhanViens
                .Where(Nv => Nv.TenCv == TenCv)
                .ToListAsync();

            if (NhanViens == null || NhanViens.Count == 0)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            return NhanViens;
        }

        // GET: api/NhanViens/ByChucVu/{TenCv} ( tìm thông tin  thông qua tên vitri)
        [HttpGet("ByVitri/{vitri}")]
        public async Task<ActionResult<IEnumerable<NhanVien>>> GetNhanVienbyViTri(string vitri)
        {
            if (_context.NhanViens == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            var NhanViens = await _context.NhanViens
                .Where(Nv => Nv.ViTri == vitri)
                .ToListAsync();

            if (NhanViens == null || NhanViens.Count == 0)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            return NhanViens;
        }

        // PUT: api/NhanViens/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNhanVien(string id, NhanVien nhanVien)
        {
            _context.Entry(nhanVien).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NhanVienExists(id))
                {
                    return BadRequest("Dữ liệu không tồn tại.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/NhanViens
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NhanVien>> PostNhanVien(NhanVien nhanVien)
        {

          if (_context.NhanViens == null)
          {
              return Problem("Entity set 'QLMamNonContext.NhanViens'  is null.");
          }

            _context.NhanViens.Add(nhanVien);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (NhanVienExists(nhanVien.MaSt))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetNhanVien", new { id = nhanVien.MaSt }, nhanVien);
        }

        // DELETE: api/NhanViens/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNhanVien(string id)
        {
            if (_context.NhanViens == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }
            var nhanVien = await _context.NhanViens.FindAsync(id);
            if (nhanVien == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            _context.NhanViens.Remove(nhanVien);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NhanVienExists(string id)
        {
            return (_context.NhanViens?.Any(e => e.MaSt == id)).GetValueOrDefault();
        }
    }
}

