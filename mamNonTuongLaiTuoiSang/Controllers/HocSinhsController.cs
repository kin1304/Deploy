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
    public class HocSinhsController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public HocSinhsController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/HocSinhs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<HocSinh>>> GetHocSinhs()
        {
            if (_context.HocSinhs == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }
            return await _context.HocSinhs.ToListAsync();
        }

        // GET: api/HocSinhs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<HocSinh>> GetHocSinh(string id)
        {
            if (_context.HocSinhs == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }
            var hocSinh = await _context.HocSinhs.FindAsync(id);

            if (hocSinh == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            return hocSinh;
        }
        // GET: api/HocSinh/ByPhuHuynh/{idPH} ( tìm thông tin học sinh thông qua id phu huynh)
        [HttpGet("ByPhuHuynh/{idPh}")]
        public async Task<ActionResult<IEnumerable<HocSinh>>> GetHocSinhByPhuHuynh(string idPh)
        {
            if (_context.HocSinhs == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            // Tìm tất cả học sinh có mã phụ huynh tương ứng
            var hocSinhs = await _context.HocSinhs
                .Where(hs => hs.IdPh.Replace(" ","") == idPh.Replace(" ",""))
                .ToListAsync();

            if (hocSinhs == null || hocSinhs.Count == 0)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            return hocSinhs;
        }
        [HttpGet("ChieuCao/CanNang/{id}")]
        public async Task<ActionResult<HocSinh>> GetHocSinhChieuCaoCanNang(string id)
        {
            if (_context.HocSinhs == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }
            var hocSinh = await _context.HocSinhs
          .Where(hs => hs.IdHs == id)
          .Select(hs => new { hs.IdHs, hs.TenHs, hs.ChieuCao, hs.CanNang })
          .FirstOrDefaultAsync();
            if (hocSinh == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }


            return Ok(hocSinh);
        }

        // PUT: api/HocSinhs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutHocSinh(string id, HocSinh hocSinh)
        {

            _context.Entry(hocSinh).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!HocSinhExists(id))
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

        // POST: api/HocSinhs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<HocSinh>> PostHocSinh(HocSinh hocSinh)
        {
            if (_context.HocSinhs == null)
            {
                return Problem("Entity set 'QLMamNonContext.HocSinhs'  is null.");
            }
            _context.HocSinhs.Add(hocSinh);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (HocSinhExists(hocSinh.IdHs))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetHocSinh", new { id = hocSinh.IdHs }, hocSinh);
        }

        // DELETE: api/HocSinhs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteHocSinh(string id)
        {
            if (_context.HocSinhs == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }
            var hocSinh = await _context.HocSinhs.FindAsync(id);
            if (hocSinh == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            _context.HocSinhs.Remove(hocSinh);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool HocSinhExists(string id)
        {
            return (_context.HocSinhs?.Any(e => e.IdHs == id)).GetValueOrDefault();
        }
    }
}
