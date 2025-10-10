using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManager.Models
{
    public enum Rol
    {
        Administrador,
        Profesor,
        Recepcionista
    }

    public class Usuario
    {
        public int IdUsuario { get; set; }   // PK identity real en BD
       
        public string Nombre { get; set; } = "";
        public string Apellido { get; set; } = "";
        public string Email { get; set; } = "";
        public string Password { get; set; } = "";
        public Rol Rol { get; set; }
        public bool Activo { get; set; } // Indica si el usuario está activo o no

    }

}

