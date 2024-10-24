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
    public class TkbsController : ControllerBase
    {
        private readonly QLMamNonContext _context;

        public TkbsController(QLMamNonContext context)
        {
            _context = context;
        }

        // GET: api/Tkbs
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tkb>>> GetTkbs()
        {
          if (_context.Tkbs == null)
          {
                return BadRequest();
            }
            return await _context.Tkbs.ToListAsync();
        }

        // GET: api/Tkbs/IdLop/Ngay
        [HttpGet("{IdLop}/{Ngay}")]
        public async Task<ActionResult<Tkb>> GetTkb(string IdLop, string Ngay)
        {
            if (_context.Tkbs == null)
            {
                return BadRequest();
            }

            // Truy vấn dữ liệu từ bảng Tkb theo hai khóa chính
            var tkb = await _context.Tkbs
                .Include(t => t.IdMhNavigation )   
                .FirstOrDefaultAsync(t => t.IdLop == IdLop && t.Ngay == Ngay);
            if (tkb == null)
            {
                return BadRequest("Không tìm thấy thời khóa biểu với IdLop và Ngày được cung cấp.");
            }

            // Chuyển đổi sang TkbDto để trả về dữ liệu hợp lý
            var tkbDto = new Tkb
            {
                IdLop = tkb.IdLop,
                Ngay = tkb.Ngay,
                CaHoc = tkb.CaHoc,
                IdMh = tkb.IdMh,
                 
            };
            
            return Ok(tkbDto);
        }
        // PUT: api/Tkbs/IdLop/Ngay
        [HttpPut("{IdLop}/{Ngay}")]
        public async Task<IActionResult> PutTkb(string IdLop, string Ngay, Tkb tkb)
        {
            // Tìm bản ghi Tkb theo IdLop và Ngay
            var existingTkb = await _context.Tkbs
                .Include(t => t.IdMhNavigation)  // Bao gồm thông tin từ MonHoc
                .FirstOrDefaultAsync(t => t.IdLop == IdLop && t.Ngay == Ngay);

            if (existingTkb == null)
            {
                return BadRequest("Không tìm thấy thời khóa biểu với IdLop và Ngày được cung cấp.");
            }

            // Cập nhật thông tin của bản ghi Tkb với dữ liệu mới
            existingTkb.CaHoc = tkb.CaHoc;
            existingTkb.IdMh = tkb.IdMh;

            // Đánh dấu trạng thái modified cho bản ghi Tkb
            _context.Entry(existingTkb).State = EntityState.Modified;
            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {

                // Xử lý lỗi đồng bộ hóa nếu có xung đột
                if (!TkbExists(IdLop, Ngay))
                {
                    return BadRequest ("Thời khóa biểu không tồn tại.");
                }
                else
                {
                    throw;
                }
            }

            return NoContent(); // Trả về mã 204 nếu cập nhật thành công
        }

        // POST: api/Tkbs
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Tkb>> PostTkb(Tkb tkb)
        {
            if (_context.Tkbs == null)
            {
                return BadRequest("Entity set 'QLMamNonContext.Tkbs' is null.");
            }

            // Kiểm tra sự tồn tại của bản ghi với IdLop và Ngay
            if (TkbExists(tkb.IdLop, tkb.Ngay))
            {
                return Conflict("Thời khóa biểu đã tồn tại với IdLop và Ngày đã cho.");
            }

            // Thêm bản ghi mới vào DbSet
            _context.Tkbs.Add(tkb);
            try
            {
                // Lưu thay đổi vào cơ sở dữ liệu
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                // Xử lý lỗi nếu có xung đột trong việc thêm bản ghi
                throw;
            }

            // Trả về kết quả 201 Created cùng với thông tin bản ghi vừa thêm
            return CreatedAtAction("GetTkb", new { IdLop = tkb.IdLop, Ngay = tkb.Ngay }, tkb);
        }

        // DELETE: api/Tkbs/IdLop/Ngay
        [HttpDelete("{IdLop}/{Ngay}")]
        public async Task<IActionResult> DeleteTkb(string IdLop, string Ngay)
        {
            if (_context.Tkbs == null)
            {
                return BadRequest("Dữ liệu không tồn tại.");
            }

            // Tìm bản ghi Tkb theo IdLop và Ngay
            var tkb = await _context.Tkbs
                .FirstOrDefaultAsync(t => t.IdLop == IdLop && t.Ngay == Ngay);

            if (tkb == null)
            {
                return BadRequest("Không tìm thấy thời khóa biểu với IdLop và Ngày được cung cấp.");
            }

            // Xóa bản ghi Tkb
            _context.Tkbs.Remove(tkb);
            await _context.SaveChangesAsync();

            return NoContent(); // Trả về mã 204 nếu xóa thành công
        }

        private bool TkbExists(string IdLop, string Ngay)
        {
            // Kiểm tra xem có tồn tại bản ghi Tkb nào với IdLop và Ngay đã cho hay không
            return _context.Tkbs.Any(t => t.IdLop == IdLop && t.Ngay == Ngay);
        }
    }
}