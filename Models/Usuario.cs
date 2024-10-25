using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Coin.Models
{
    public class Usuario : IdentityUser
    {
        [Required]
        [MaxLength(50)]
        public string NickName { get; set; }

        [MaxLength(15)]
        public string Telefono { get; set; }

        [MaxLength(18)]
        public string CuentaCLABE { get; set; }

        [Required]
        public decimal SaldoDisponible { get; set; }

        [MaxLength(50)]
        public string Rol { get; set; } // Puede ser "Admin" o "User"

        [Required]
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
    }
}
