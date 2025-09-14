using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GymManager.Models;

namespace GymManager.Controllers
{
    /// <summary>
    /// Maneja todo lo relacionado a los usuarios del sistema:
    /// login, creación, edición y listado.
    /// </summary>
    public class UsuarioController
    {
        // Lista de usuarios de prueba
        private List<Usuario> usuarios = new List<Usuario>
        {
            new Usuario { Id = 1, Nombre = "Admin", Email = "admin@gmail.com", Rol = Rol.Administrador },
            new Usuario { Id = 2, Nombre = "Profesor Juan", Email = "profe@gmail.com", Rol = Rol.Profesor },
            new Usuario { Id = 3, Nombre = "Recepcionista Ana", Email = "recep@gmail.com", Rol = Rol.Recepcionista }
        };

        public Usuario? Login(string email, string password)
        {
            // Por simplicidad, todos usan la contraseña "1234"
            return usuarios.FirstOrDefault(u => u.Email == email && password == "1234");
        }

    }
}
