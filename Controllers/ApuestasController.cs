using Coin.Data;
using Coin.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Coin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApuestasController : ControllerBase
    {
        private readonly CoinDbContext _context;

        public ApuestasController(CoinDbContext context)
        {
            _context = context;
        }

        // GET: api/apuestas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Apuesta>>> GetApuestas()
        {
            return await _context.COIN_Apuestas.ToListAsync();
        }

        // GET: api/apuestas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Apuesta>> GetApuesta(int id)
        {
            var apuesta = await _context.COIN_Apuestas.FindAsync(id);

            if (apuesta == null)
            {
                return NotFound();
            }

            return apuesta;
        }

        // POST: api/apuestas
        [Authorize]
        [HttpPost]
        public async Task<ActionResult<Apuesta>> PostApuesta(Apuesta apuesta)
        {
            // Validar que el usuario autenticado esté relacionado con la apuesta
            if (User.Identity.Name != apuesta.IdJugador1.ToString() && !User.IsInRole("Admin"))
            {
                return Forbid();
            }

            _context.COIN_Apuestas.Add(apuesta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetApuesta", new { id = apuesta.IdApuesta }, apuesta);
        }

        // PUT: api/apuestas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutApuesta(int id, Apuesta apuesta)
        {
            if (id != apuesta.IdApuesta)
            {
                return BadRequest();
            }

            _context.Entry(apuesta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ApuestaExists(id))
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

        // DELETE: api/apuestas/5
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteApuesta(int id)
        {
            var apuesta = await _context.COIN_Apuestas.FindAsync(id);
            if (apuesta == null)
            {
                return NotFound();
            }

            _context.COIN_Apuestas.Remove(apuesta);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ApuestaExists(int id)
        {
            return _context.COIN_Apuestas.Any(e => e.IdApuesta == id);
        }
    }
}

