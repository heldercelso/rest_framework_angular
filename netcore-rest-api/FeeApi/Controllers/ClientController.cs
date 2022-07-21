#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FeeApi.Models;

namespace FeeApi.Controllers
{
    [Route("api/new-client")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly FeeContext _context;

        public ClientController(FeeContext context)
        {
            _context = context;
        }

        // GET: api/Client
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            return await _context.Clients.ToListAsync();
        }

        // GET: api/Client/5
        [HttpGet("{cpf}")]
        public async Task<ActionResult<Client>> GetClient(string cpf)
        {
            var client = await _context.Clients.FindAsync(cpf);

            if (client == null)
            {
                return NotFound();
            }

            return client;
        }

        // PUT: api/Client/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{cpf}")]
        public async Task<IActionResult> PutClient(string cpf, Client client)
        {
            if (cpf != client.cpf)
            {
                return BadRequest();
            }

            _context.Entry(client).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ClientExists(cpf))
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

        // POST: api/Client
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Client>> PostClient(Client client)
        {
            // if (string.IsNullOrEmpty(client.name) || string.IsNullOrEmpty(client.segment)) {
            //     HttpResponseMessage response = 
            //         this.Request.CreateErrorResponse(Http StatusCode.BadRequest, "your message");
            //         throw new HttpResponseException(response);
            // }
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetClient", new { cpf = client.cpf }, client);
        }

        // DELETE: api/Client/5
        [HttpDelete("{cpf}")]
        public async Task<IActionResult> DeleteClient(string cpf)
        {
            var client = await _context.Clients.FindAsync(cpf);
            if (client == null)
            {
                return NotFound();
            }

            _context.Clients.Remove(client);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ClientExists(string cpf)
        {
            return _context.Clients.Any(e => e.cpf == cpf);
        }
        public class ErrorMessage
        {
            public string message { get; set; }
        }
    }
}
