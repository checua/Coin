namespace Coin.Models
{
    public class Usuario
    {
        public int IdUsuario { get; set; }
        public string NickName { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string CuentaCLABE { get; set; }
        public string ContrasenaHash { get; set; }
        public DateTime FechaRegistro { get; set; }
        public decimal SaldoDisponible { get; set; }
    }
}
