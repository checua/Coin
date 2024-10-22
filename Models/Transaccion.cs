namespace Coin.Models
{
    public class Transaccion
    {
        public int IdTransaccion { get; set; }
        public int IdUsuario { get; set; }
        public string TipoTransaccion { get; set; }
        public decimal Monto { get; set; }
        public DateTime FechaTransaccion { get; set; }
        public string Estado { get; set; }
    }
}
