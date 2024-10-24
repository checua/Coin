﻿using System.ComponentModel.DataAnnotations;

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
        public DateTime FechaRegistro { get; set; } = DateTime.Now;
        public decimal SaldoDisponible { get; set; }
        public string Rol { get; set; }  // Puede ser "Admin" o "User"
    }

}
