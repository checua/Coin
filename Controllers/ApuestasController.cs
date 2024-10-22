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
            // Verificar que ambos jugadores tienen suficiente saldo para realizar la apuesta
            var jugador1 = await _context.COIN_Usuarios.FindAsync(apuesta.IdJugador1);
            var jugador2 = await _context.COIN_Usuarios.FindAsync(apuesta.IdJugador2);

            if (jugador1 == null || jugador2 == null)
            {
                return BadRequest("Uno o ambos jugadores no existen.");
            }

            if (jugador1.SaldoDisponible < apuesta.MontoApostado || jugador2.SaldoDisponible < apuesta.MontoApostado)
            {
                return BadRequest("Uno o ambos jugadores no tienen suficiente saldo para realizar la apuesta.");
            }

            // Si ambos jugadores tienen suficiente saldo, creamos la apuesta
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

        [Authorize(Roles = "Admin")]
        [HttpPost("{id}/finalizar")]
        public async Task<IActionResult> FinalizarApuesta(int id)
        {
            // Buscar la apuesta
            var apuesta = await _context.COIN_Apuestas.FindAsync(id);
            if (apuesta == null)
            {
                return NotFound("La apuesta no existe.");
            }

            // Verificar que la apuesta aún no ha sido finalizada
            if (apuesta.Ganador != null)
            {
                return BadRequest("La apuesta ya ha sido finalizada.");
            }

            // Determinar el ganador aleatoriamente (jugador 1 o jugador 2)
            Random random = new Random();
            int ganador = random.Next(0, 2) == 0 ? apuesta.IdJugador1 : apuesta.IdJugador2;

            // Actualizar el campo "Ganador" en la apuesta
            apuesta.Ganador = ganador;

            // Realizar la transferencia de fondos
            var jugador1 = await _context.COIN_Usuarios.FindAsync(apuesta.IdJugador1);
            var jugador2 = await _context.COIN_Usuarios.FindAsync(apuesta.IdJugador2);

            if (jugador1 == null || jugador2 == null)
            {
                return BadRequest("Uno de los jugadores no existe.");
            }

            // Verificar que ambos jugadores tengan suficiente saldo
            if (jugador1.SaldoDisponible < apuesta.MontoApostado || jugador2.SaldoDisponible < apuesta.MontoApostado)
            {
                return BadRequest("Uno o ambos jugadores no tienen suficiente saldo para completar la apuesta.");
            }

            // Calcular la comisión del 5%
            decimal comision = apuesta.MontoApostado * 0.05m;
            decimal montoGanancia = apuesta.MontoApostado - comision;

            // Transferir el monto de la comisión al sistema y la ganancia al ganador
            if (ganador == apuesta.IdJugador1)
            {
                jugador1.SaldoDisponible += montoGanancia + apuesta.MontoApostado;
                jugador2.SaldoDisponible -= apuesta.MontoApostado;
            }
            else
            {
                jugador2.SaldoDisponible += montoGanancia + apuesta.MontoApostado;
                jugador1.SaldoDisponible -= apuesta.MontoApostado;
            }

            // Crear una nueva entrada en COIN_Sistema para registrar la transacción
            var transaccionSistema = new Sistema
            {
                IdApuesta = apuesta.IdApuesta,
                IdJugador1 = apuesta.IdJugador1,
                IdJugador2 = apuesta.IdJugador2,
                FechaTransaccion = DateTime.Now,
                MontoApostado = apuesta.MontoApostado,
                Comision = comision,
                MontoGanancia = montoGanancia,
                Ganador = apuesta.Ganador,
                Status = "Completada",
                TipoTransaccion = "Comisión Apuesta",
                Detalle = "Comisión del 5% por la apuesta completada"
            };

            _context.COIN_Sistema.Add(transaccionSistema);

            // Guardar los cambios
            _context.Entry(jugador1).State = EntityState.Modified;
            _context.Entry(jugador2).State = EntityState.Modified;
            _context.Entry(apuesta).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(new { ganador, comision, montoGanancia });
        }





        private bool ApuestaExists(int id)
        {
            return _context.COIN_Apuestas.Any(e => e.IdApuesta == id);
        }
    }
}

