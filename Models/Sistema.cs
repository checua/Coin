using System;

namespace Coin.Models
{
    public class Sistema
    {
        public int IdTransaccion { get; set; }
        public int IdApuesta { get; set; }
        public int IdJugador1 { get; set; }
        public int IdJugador2 { get; set; }
        public DateTime FechaTransaccion { get; set; } = DateTime.Now;
        public decimal MontoApostado { get; set; }
        public decimal Comision { get; set; }
        public decimal MontoGanancia { get; set; }
        public int? Ganador { get; set; } // Puede ser null hasta que se determine un ganador
        public string Status { get; set; }
        public string TipoTransaccion { get; set; }
        public string Detalle { get; set; }
    }
}

