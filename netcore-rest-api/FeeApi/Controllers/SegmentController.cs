#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FeeApi.Models;

namespace FeeApi.Controllers
{
    [Route("api/new-segment")]
    [ApiController]
    public class SegmentController : ControllerBase
    {
        private readonly FeeContext _context;

        public SegmentController(FeeContext context)
        {
            _context = context;
        }

        // GET: api/Segment
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Segment>>> GetSegment()
        {
            return await _context.Segments.ToListAsync();
        }

        // GET: api/Segment/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Segment>> GetSegment(int id)
        {
            var segment = await _context.Segments.FindAsync(id);

            if (segment == null)
            {
                return NotFound();
            }

            return segment;
        }

        // PUT: api/Segment/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSegment(int id, Segment segment)
        {
            if (id != segment.Id)
            {
                return BadRequest();
            }

            _context.Entry(segment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SegmentExists(id))
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

        // POST: api/Segment
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Segment>> PostSegment(Segment segment)
        {
            _context.Segments.Add(segment);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSegment", new { id = segment.Id }, segment);
        }

        // DELETE: api/Segment/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSegment(int id)
        {
            var segment = await _context.Segments.FindAsync(id);
            if (segment == null)
            {
                return NotFound();
            }

            _context.Segments.Remove(segment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SegmentExists(int id)
        {
            return _context.Segments.Any(e => e.Id == id);
        }
    }
}
