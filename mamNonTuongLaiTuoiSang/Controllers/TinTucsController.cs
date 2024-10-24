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
    public class TinTucsController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public TinTucsController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/TinTucs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TinTuc>>> GetTinTucs()
        {
          if (_context.TinTucs == null)
          {
                return BadRequest("Dữ liệu không tồn tại.");
          }
            return await _context.TinTucs.ToListAsync();
        }

        // GET: api/TinTucs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TinTuc>> GetTinTuc(string id)
        {
          if (_context.TinTucs == null)
          {
              return BadRequest("Dữ liệu không tồn tại.");
          }
            var tinTuc = await _context.TinTucs.FindAsync(id);

            if (tinTuc == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            return tinTuc;
        }
        // GET: api/NhanViens/ByMast/{MaSt} ( tìm thông tin  thông qua MaSt)
        [HttpGet("ByMast/{MaSt}")]
        public async Task<ActionResult<IEnumerable<TinTuc>>> GetTinTucByMaSt(string MaSt)
        {
            if (_context.TinTucs == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }


            var TinTucs = await _context.TinTucs
                .Where(Tt=> Tt.MaSt == MaSt)
                .ToListAsync();

            if (TinTucs == null || TinTucs.Count == 0)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            return TinTucs;
        }

        // PUT: api/TinTucs/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTinTuc(string id, TinTuc tinTuc)
        {

            _context.Entry(tinTuc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TinTucExists(id))
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

        // POST: api/TinTucs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<TinTuc>> PostTinTuc(TinTuc tinTuc)
        {
          if (_context.TinTucs == null)
          {
              return Problem("Entity set 'QLMamNonContext.TinTucs'  is null.");
          }
            _context.TinTucs.Add(tinTuc);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (TinTucExists(tinTuc.IdTinTuc))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetTinTuc", new { id = tinTuc.IdTinTuc }, tinTuc);
        }

        // DELETE: api/TinTucs/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTinTuc(string id)
        {
            if (_context.TinTucs == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }
            var tinTuc = await _context.TinTucs.FindAsync(id);
            if (tinTuc == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            _context.TinTucs.Remove(tinTuc);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TinTucExists(string id)
        {
            return (_context.TinTucs?.Any(e => e.IdTinTuc == id)).GetValueOrDefault();
        }
    }
}
