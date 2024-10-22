using System.ComponentModel.DataAnnotations;

namespace Coin.Models
{
    public class Apuesta
    {
        public int IdApuesta { get; set; }
        public int IdJugador1 { get; set; }
        public int IdJugador2 { get; set; }
        public decimal MontoApostado { get; set; }
        public DateTime FechaApuesta { get; set; } = DateTime.Now;
        public string? Resultado { get; set; }  // Hacer que el campo Resultado acepte valores nulos
        public int? Ganador { get; set; }  // Puede ser null hasta que se determine un ganador
    }

}
