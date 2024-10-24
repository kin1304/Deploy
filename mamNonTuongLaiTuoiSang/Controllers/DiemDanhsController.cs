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
    public class DiemDanhsController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public DiemDanhsController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/DiemDanhs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<DiemDanh>>> GetDiemDanhs()
        {
          if (_context.DiemDanhs == null)
          {
              return BadRequest();
          }
            return await _context.DiemDanhs.ToListAsync();
        }

        // GET: api/DiemDanhs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<DiemDanh>> GetDiemDanh(string id)
        {
          if (_context.DiemDanhs == null)
          {
              return BadRequest();
          }
            var diemDanh = await _context.DiemDanhs.FindAsync(id);

            if (diemDanh == null)
            {
                return BadRequest();
            }

            return diemDanh;
        }


       // api/diemdanhs/lop/{idLop}
    // API để lấy danh sách điểm danh qua mã lớp
    [HttpGet("lop/{idLop}")]
        public async Task<ActionResult<IEnumerable<DiemDanh>>> GetDiemDanhByLop(string idLop)
        {
            // Lấy thông tin điểm danh dựa trên mã lớp
            var diemDanhList = await _context.DiemDanhs
                .Where(dd => dd.IdLop == idLop)
                .ToListAsync();

            if (diemDanhList == null || !diemDanhList.Any())
            {
                return BadRequest("Không tìm thấy thông tin điểm danh cho mã lớp này.");
            }

            return Ok(diemDanhList);
        }

       // api/diemdanhs/hocSinh/HS000001
        [HttpGet("hocSinh/{idHs}")]
        public async Task<ActionResult<IEnumerable<DiemDanh>>> GetDiemDanhByHocSinh(string idHs)
        {
            // Lấy thông tin điểm danh dựa trên mã học sinh
            var diemDanhList = await _context.DiemDanhs
                .Where(dd => dd.IdHs == idHs)
                .ToListAsync();

            if (diemDanhList == null || !diemDanhList.Any())
            {
                return BadRequest  ("Không tìm thấy thông tin điểm danh cho mã học sinh này.");
            }

            return Ok(diemDanhList);
        }

        // PUT: api/DiemDanhs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutDiemDanh(string id, DiemDanh diemDanh)
        {
            _context.Entry(diemDanh).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DiemDanhExists(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/DiemDanhs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<DiemDanh>> PostDiemDanh(DiemDanh diemDanh)
        {
          if (_context.DiemDanhs == null)
          {
              return Problem("Entity set 'QLMamNonContext.DiemDanhs'  is null.");
          }
            _context.DiemDanhs.Add(diemDanh);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (DiemDanhExists(diemDanh.IdDd))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetDiemDanh", new { id = diemDanh.IdDd }, diemDanh);
        }

        // DELETE: api/DiemDanhs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiemDanh(string id)
        {
            if (_context.DiemDanhs == null)
            {
                return BadRequest();
            }
            var diemDanh = await _context.DiemDanhs.FindAsync(id);
            if (diemDanh == null)
            {
                return BadRequest();
            }

            _context.DiemDanhs.Remove(diemDanh);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool DiemDanhExists(string id)
        {
            return (_context.DiemDanhs?.Any(e => e.IdDd == id)).GetValueOrDefault();
        }
    }
}
