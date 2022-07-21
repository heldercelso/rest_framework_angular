#nullable disable
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FeeApi.Models;

namespace FeeApi.Controllers
{
    // [Route("api/[controller]")]
    [ApiController]
    // [EnableCors("MyAllowSpecificOrigins")]
    public class FeeItemsController : ControllerBase
    {
        private readonly FeeContext _context;

        public FeeItemsController(FeeContext context)
        {
            _context = context;
        }

        // GET: api/FeeItems
        [HttpGet("api/fees-charged")]
        public async Task<ActionResult<IEnumerable<FeeItemToReturn>>> GetFeeItems([FromQuery] string segment, [FromQuery] string client)
        {
            IEnumerable<FeeItem> resList = new FeeItem[] {}.AsQueryable();
            IEnumerable<FeeItemToReturn> resReturn = new FeeItemToReturn[] {}.AsQueryable();
            //var feeItemList = _context.FeeItems.Include(n => n.clientF);
            // var feeItem = new FeeItem();

            if (!string.IsNullOrEmpty(client) && !string.IsNullOrEmpty(segment)) {
                if (client.All(char.IsDigit)) {
                    resList = await _context.FeeItems.Where(x => x.clientF.cpf.ToString().Contains(client) && x.clientF.segmentF.name.ToLower().Contains(segment.ToLower())).ToListAsync();
                    // return await _context.FeeItems.Where(x => x.clientF.cpf.ToString().Contains(client) && x.clientF.segment.ToLower().Contains(segment.ToLower())).ToListAsync();
                } else {
                    resList =  await _context.FeeItems.Where(x => x.clientF.name.ToLower().Contains(client.ToLower()) && x.clientF.segmentF.name.ToLower().Contains(segment.ToLower())).ToListAsync();
                    // return await _context.FeeItems.Where(x => x.clientF.name.ToLower().Contains(client.ToLower()) && x.clientF.segment.ToLower().Contains(segment.ToLower())).ToListAsync();
                }
            } else {
                if (!string.IsNullOrEmpty(segment)) {
                    resList =  await _context.FeeItems.Where(x => x.clientF.segmentF.name.ToLower().Contains(segment.ToLower())).ToListAsync();
                    // return await _context.FeeItems.Where(x => x.clientF.segment.ToLower().Contains(segment.ToLower())).ToListAsync();
                } else {
                    if (!string.IsNullOrEmpty(client)) {
                        if (client.All(char.IsDigit)) {
                            resList =  await _context.FeeItems.Where(x => x.clientF.cpf.ToString().Contains(client)).ToListAsync();
                            // return await _context.FeeItems.Where(x => x.clientF.cpf.ToString().Contains(client)).ToListAsync();
                        } else {
                            resList =  await _context.FeeItems.Where(x => x.clientF.name.ToString().Contains(client)).ToListAsync();
                        }
                    } else {
                        var applicationDbContext = _context.FeeItems.Include(n => n.clientF);
                        resList = await applicationDbContext.ToListAsync();
                    }
                }
            }

            foreach (var feeItem in resList.ToList()) {
                var feeItemReturn = new FeeItemToReturn();
                var clientF = await _context.Clients.FindAsync(feeItem.client);
                var segmentF = await _context.Segments.FindAsync(clientF.segment);
                feeItemReturn.Id = feeItem.Id;
                feeItemReturn.client = feeItem.client;
                feeItemReturn.client_name = clientF.name;
                feeItemReturn.client_segment = segmentF.name;
                feeItemReturn.segment_fee = segmentF.fee;
                feeItemReturn.source_currency_amount = feeItem.source_currency_amount;
                feeItemReturn.conversion_result = feeItem.conversion_result;
                feeItemReturn.fee = feeItem.fee;
                feeItemReturn.formula = feeItem.formula;
                resReturn = resReturn.Append(feeItemReturn);
                
            }
            return resReturn.ToList();

            // var applicationDbContext = _context.FeeItems.Include(n => n.clientF);
            // return await applicationDbContext.ToListAsync();
        }

        // GET: api/FeeItems/5
        [HttpGet("api/fees-charged/{cpf}/{id}")]
        // [HttpGet("api/[controller]")]
        public async Task<ActionResult<FeeItemToReturn>> GetFeeItem(string cpf, int id)
        {
            var feeItemList = await _context.FeeItems.Where(x => x.client == cpf && x.Id == id).ToListAsync();
            var feeItem = new FeeItem();

            if (!feeItemList.Any())
            {
                return NotFound();
            } else {
                feeItem = feeItemList.First();
            }

            var clientF = await _context.Clients.FindAsync(feeItem.client);
            var segmentF = await _context.Segments.FindAsync(clientF.segment);

            // new JsonResult(feeItem, "client_name": "Teste");
            var feeItemReturn = new FeeItemToReturn();
            feeItemReturn.Id = feeItem.Id;
            feeItemReturn.client = feeItem.client;
            feeItemReturn.client_name = clientF.name;
            feeItemReturn.client_segment = segmentF.name;
            feeItemReturn.segment_fee = segmentF.fee;
            feeItemReturn.source_currency_amount = feeItem.source_currency_amount;
            feeItemReturn.conversion_result = feeItem.conversion_result;
            feeItemReturn.fee = feeItem.fee;
            feeItemReturn.formula = feeItem.formula;

            return feeItemReturn;
        }

        // PUT: api/FeeItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("api/fees-charged/{cpf}/{id}")]
        public async Task<IActionResult> PutFeeItem(string cpf, int id, FeeItem upFeeItem)
        {
            var feeItemList = await _context.FeeItems.Where(x => x.client == cpf && x.Id == id).ToListAsync();
            var feeItem = new FeeItem();
            if (!feeItemList.Any())
            {
                return NotFound();
            } else {
                feeItem = feeItemList.First();
            }
            FeeUtils utils = new FeeUtils();
            // var res = await utils.get_current_fee1();
            feeItem.fee = await utils.get_current_fee();//Utils.FeeUtils.get_current_fee();
            feeItem.source_currency_amount = upFeeItem.source_currency_amount;
            feeItem.conversion_result = (feeItem.source_currency_amount * feeItem.fee) * (1 + 0.3);//feeItem.client.segment.fee)
            _context.Entry(feeItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FeeItemExists(cpf, id))
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

        // POST: api/FeeItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("api/fees-charged")]
        public async Task<ActionResult<FeeItem>> PostFeeItem(FeeItem feeItem)
        {
            FeeUtils utils = new FeeUtils();
            // var res = await utils.get_current_fee1();
            feeItem.fee = await utils.get_current_fee();//Utils.FeeUtils.get_current_fee();
            feeItem.conversion_result = (feeItem.source_currency_amount * feeItem.fee) * (1 + 0.3);//feeItem.client.segment.fee)
            var clientF = await _context.Clients.FindAsync(feeItem.client);
            var segmentF = await _context.Segments.FindAsync(clientF.segment);
            
            if (clientF == null)
            {
                return NotFound();
            }
            // var User = _context.Clients.Where(u => u.Id == feeItem.clientId).First();
            clientF.segmentF = segmentF;
            feeItem.clientF = clientF;
            
            _context.FeeItems.Add(feeItem);
            
            await _context.SaveChangesAsync();
            

            return CreatedAtAction(nameof(PostFeeItem), new { id = feeItem.Id }, feeItem);
        }

        // DELETE: api/FeeItems/5
        [HttpDelete("api/fees-charged/{cpf}/{id}")]
        public async Task<IActionResult> DeleteFeeItem(string cpf, int id)
        {
            var feeItemList = await _context.FeeItems.Where(x => x.client == cpf && x.Id == id).ToListAsync();
            var feeItem = new FeeItem();
            if (!feeItemList.Any())
            {
                return NotFound();
            } else {
                feeItem = feeItemList.First();
            }

            _context.FeeItems.Remove(feeItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool FeeItemExists(string cpf, int id)
        {
            return _context.FeeItems.Any(e => e.client == cpf && e.Id == id);
        }
        public class FeeItemToReturn
        {
            public int Id { get; set; }
            public string client { get; set; }
            public string client_name { get; set; }
            public string client_segment { get; set; }
            public double segment_fee { get; set; }
            public double source_currency_amount { get; set; }
            public double conversion_result { get; set; }
            public double fee { get; set; }
            public string formula { get; set; }
        }
    }
}
