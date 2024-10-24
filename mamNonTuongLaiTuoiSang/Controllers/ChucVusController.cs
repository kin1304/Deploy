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
    public class ChucVusController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public ChucVusController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/ChucVus
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ChucVu>>> GetChucVus()
        {
            if (_context.ChucVus == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }
            return await _context.ChucVus.ToListAsync();
        }

        // GET: api/ChucVus/5
        // GET: api/ChucVus/{TenCv}/{ViTri}
        [HttpGet("{TenCv}/{ViTri}")]
        public async Task<ActionResult<ChucVu>> GetChucVu(string TenCv, string ViTri)
        {
            var chucVu = await _context.ChucVus
                .FirstOrDefaultAsync(c => c.TenCv == TenCv && c.ViTri == ViTri);

            if (chucVu == null)
            {
                return NotFound();
            }

            return chucVu;
        }

        // GET: api/ChucVus/Vitri/{Vitri} 
        [HttpGet("Vitri/{Vitri}")]
        public async Task<ActionResult<ChucVu>> GetChucVuVitri(string Vitri)
        {
            if (_context.ChucVus == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            var Chucvuvitri = await _context.ChucVus
                .Where(cvvt => cvvt.ViTri == Vitri)
                .FirstOrDefaultAsync();

            if (Chucvuvitri == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            return Chucvuvitri;
        }

        // PUT: api/ChucVus/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        // PUT: api/ChucVus/{TenCv}/{ViTri}
        [HttpPut("{TenCv}/{ViTri}")]
        public async Task<IActionResult> PutChucVu(string TenCv, string ViTri, ChucVu chucVu)
        {
            _context.Entry(chucVu).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ChucVuExists(TenCv, ViTri))
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

        // POST: api/ChucVus
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<ChucVu>> PostChucVu(ChucVu chucVu)
        {
            if (_context.ChucVus == null)
            {
                return Problem("Entity set 'QLMamNonContext.ChucVus'  is null.");
            }
            _context.ChucVus.Add(chucVu);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (ChucVuExists(chucVu.TenCv, chucVu.ViTri))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetChucVu", new { id = chucVu.TenCv }, chucVu);
        }

        // DELETE: api/ChucVus/TenCv/ViTri
        [HttpDelete("{TenCv}/{ViTri}")]
        public async Task<IActionResult> DeleteChucVu(string TenCv, string ViTri)
        {
            if (_context.ChucVus == null)
            {
                return Problem("Entity set 'QLMamNonContext.ChucVus' is null.");
            }

            // Tìm kiếm chức vụ theo TenCv và ViTri
            var chucVu = await _context.ChucVus
                .FirstOrDefaultAsync(cv => cv.TenCv == TenCv && cv.ViTri == ViTri);

            if (chucVu == null)
            {
                return NotFound("Không tìm thấy chức vụ với tên và vị trí này.");
            }

            _context.ChucVus.Remove(chucVu);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ChucVuExists(string TenCv, string ViTri)
        {
            return _context.ChucVus.Any(e => e.TenCv == TenCv && e.ViTri == ViTri);
        }

    }
}