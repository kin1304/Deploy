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
    public class PhuHuynhsController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public PhuHuynhsController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/PhuHuynhs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PhuHuynh>>> GetPhuHuynhs()
        {
          if (_context.PhuHuynhs == null)
          {
              return BadRequest("Dữ liệu không tồn tại.");
          }
            return await _context.PhuHuynhs.ToListAsync();
        }

        // GET: api/PhuHuynhs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PhuHuynh>> GetPhuHuynh(string id)
        {
          if (_context.PhuHuynhs == null)
          {
              return BadRequest("Dữ liệu không tồn tại.");
          }
            var phuHuynh = await _context.PhuHuynhs.FindAsync(id);

            if (phuHuynh == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            return phuHuynh;
        }

        // PUT: api/PhuHuynhs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPhuHuynh(string id, PhuHuynh phuHuynh)
        {

            _context.Entry(phuHuynh).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PhuHuynhExists(id))
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

        // POST: api/PhuHuynhs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<PhuHuynh>> PostPhuHuynh(PhuHuynh phuHuynh)
        {
          if (_context.PhuHuynhs == null)
          {
              return Problem("Entity set 'QLMamNonContext.PhuHuynhs'  is null.");
          }
            _context.PhuHuynhs.Add(phuHuynh);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PhuHuynhExists(phuHuynh.IdPh))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPhuHuynh", new { id = phuHuynh.IdPh }, phuHuynh);
        }

        // DELETE: api/PhuHuynhs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhuHuynh(string id)
        {
            if (_context.PhuHuynhs == null)
            {
                return NotFound();
            }
            var phuHuynh = await _context.PhuHuynhs.FindAsync(id);
            if (phuHuynh == null)
            {
                return NotFound();
            }

            _context.PhuHuynhs.Remove(phuHuynh);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PhuHuynhExists(string id)
        {
            return (_context.PhuHuynhs?.Any(e => e.IdPh == id)).GetValueOrDefault();
        }
    }
}
