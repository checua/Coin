namespace Coin.Models
{
    public class Apuesta
    {
        public int IdApuesta { get; set; }
        public int IdJugador1 { get; set; }
        public int IdJugador2 { get; set; }
        public decimal MontoApostado { get; set; }
        public DateTime FechaApuesta { get; set; }
        public string Resultado { get; set; }
        public int? Ganador { get; set; }
    }
}
