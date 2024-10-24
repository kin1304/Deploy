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
    public class LopsController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public LopsController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/Lops
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lop>>> GetLops()
        {
          if (_context.Lops == null)
          {
              return BadRequest("Dữ liệu không tồn tại.");
          }
            return await _context.Lops.ToListAsync();
        }

        // GET: api/Lops/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lop>> GetLop(string id)
        {
          if (_context.Lops == null)
          {
              return BadRequest("Dữ liệu không tồn tại.");
          }
            var lop = await _context.Lops.FindAsync(id);

            if (lop == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            return lop;
        }
        [HttpGet("getLopByMaSt/{maSt}")]
        public async Task<ActionResult<IEnumerable<Lop>>> GetLopByMaSt(string maSt)
        {
          if (_context.Lops == null)
          {
              return BadRequest("Dữ liệu không tồn tại.");
          }
            var lop = await _context.Lops.Where(l => l.MaSt == maSt).ToListAsync();

            if (lop == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            return lop;
        }

        // PUT: api/Lops/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLop(string id, Lop lop)
        {
            _context.Entry(lop).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!LopExists(id))
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

        // POST: api/Lops
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Lop>> PostLop(Lop lop)
        {
          if (_context.Lops == null)
          {
              return Problem("Entity set 'QLMamNonContext.Lops'  is null.");
          }
            _context.Lops.Add(lop);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (LopExists(lop.IdLop))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetLop", new { id = lop.IdLop }, lop);
        }

        // DELETE: api/Lops/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLop(string id)
        {
            if (_context.Lops == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }
            var lop = await _context.Lops.FindAsync(id);
            if (lop == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            _context.Lops.Remove(lop);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LopExists(string id)
        {
            return (_context.Lops?.Any(e => e.IdLop == id)).GetValueOrDefault();
        }
    }
}
