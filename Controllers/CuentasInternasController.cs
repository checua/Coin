using Coin.Data;
using Coin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CuentasInternasController : ControllerBase
    {
        private readonly CoinDbContext _context;

        public CuentasInternasController(CoinDbContext context)
        {
            _context = context;
        }

        // GET: api/cuentasinternas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CuentaInterna>>> GetCuentasInternas()
        {
            return await _context.COIN_CuentasInternas.ToListAsync();
        }

        // GET: api/cuentasinternas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CuentaInterna>> GetCuentaInterna(int id)
        {
            var cuentaInterna = await _context.COIN_CuentasInternas.FindAsync(id);

            if (cuentaInterna == null)
            {
                return NotFound();
            }

            return cuentaInterna;
        }

        // POST: api/cuentasinternas
        [HttpPost]
        public async Task<ActionResult<CuentaInterna>> PostCuentaInterna(CuentaInterna cuentaInterna)
        {
            _context.COIN_CuentasInternas.Add(cuentaInterna);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCuentaInterna", new { id = cuentaInterna.IdCuenta }, cuentaInterna);
        }

        // PUT: api/cuentasinternas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCuentaInterna(int id, CuentaInterna cuentaInterna)
        {
            if (id != cuentaInterna.IdCuenta)
            {
                return BadRequest();
            }

            _context.Entry(cuentaInterna).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CuentaInternaExists(id))
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

        // DELETE: api/cuentasinternas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCuentaInterna(int id)
        {
            var cuentaInterna = await _context.COIN_CuentasInternas.FindAsync(id);
            if (cuentaInterna == null)
            {
                return NotFound();
            }

            _context.COIN_CuentasInternas.Remove(cuentaInterna);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CuentaInternaExists(int id)
        {
            return _context.COIN_CuentasInternas.Any(e => e.IdCuenta == id);
        }
    }

}
