namespace Coin.Models
{
    public class CuentaInterna
    {
        public int IdCuenta { get; set; }
        public int IdUsuario { get; set; }
        public decimal Saldo { get; set; }
        public DateTime FechaUltimaActualizacion { get; set; }
    }
}
