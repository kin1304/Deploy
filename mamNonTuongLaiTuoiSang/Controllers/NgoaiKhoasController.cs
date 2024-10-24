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
    public class NgoaiKhoasController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public NgoaiKhoasController(QLMamNonContext context)
        {
            _context = context;
        }
        
        // GET: api/NgoaiKhoas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<NgoaiKhoa>>> GetNgoaiKhoas()
        {
          if (_context.NgoaiKhoas == null)
          {
              return BadRequest("Dữ liệu không tồn tại.");
          }
            return await _context.NgoaiKhoas.ToListAsync();
        }

        // GET: api/NgoaiKhoas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<NgoaiKhoa>> GetNgoaiKhoa(string id)
        {
          if (_context.NgoaiKhoas == null)
          {
              return BadRequest("Dữ liệu không tồn tại.");
          }
            var ngoaiKhoa = await _context.NgoaiKhoas.FindAsync(id);

            if (ngoaiKhoa == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            return ngoaiKhoa;
        }

        // PUT: api/NgoaiKhoas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutNgoaiKhoa(string id, NgoaiKhoa ngoaiKhoa)
        {
            _context.Entry(ngoaiKhoa).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!NgoaiKhoaExists(id))
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

        // POST: api/NgoaiKhoas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<NgoaiKhoa>> PostNgoaiKhoa(NgoaiKhoa ngoaiKhoa)
        {
          
            _context.NgoaiKhoas.Add(ngoaiKhoa);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (NgoaiKhoaExists(ngoaiKhoa.IdNk))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetNgoaiKhoa", new { id = ngoaiKhoa.IdNk }, ngoaiKhoa);
        }

        // DELETE: api/NgoaiKhoas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteNgoaiKhoa(string id)
        {
            var ngoaiKhoa = await _context.NgoaiKhoas.FindAsync(id);
            if (ngoaiKhoa == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            _context.NgoaiKhoas.Remove(ngoaiKhoa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool NgoaiKhoaExists(string id)
        {
            return (_context.NgoaiKhoas?.Any(e => e.IdNk == id)).GetValueOrDefault();
        }
    }
}
