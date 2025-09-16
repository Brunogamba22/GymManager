using System.Collections.Generic;
using System.Linq;
using GymManager.Models;

namespace GymManager.Controllers
{
    /// <summary>
    /// Maneja todo lo relacionado a los usuarios del sistema:
    /// login, creación, edición y listado.
    /// </summary>
    public class UsuarioController
    {
        // Lista en memoria que actúa como una base de datos temporal
        private List<Usuario> usuarios = new List<Usuario>
        {
            new Usuario { Id = 1, Nombre = "Admin", Email = "admin@gmail.com", Password = "1234", Rol = Rol.Administrador },
            new Usuario { Id = 2, Nombre = "Profesor Juan", Email = "profe@gmail.com", Password = "1234", Rol = Rol.Profesor },
            new Usuario { Id = 3, Nombre = "Recepcionista Ana", Email = "recep@gmail.com", Password = "1234", Rol = Rol.Recepcionista }
        };

        // ID incremental para nuevos usuarios
        private int proximoId = 4;

        /// <summary>
        /// Devuelve una copia de todos los usuarios
        /// </summary>
        public List<Usuario> ObtenerTodos()
        {
            return usuarios.ToList();
        }

        /// <summary>
        /// Agrega un nuevo usuario a la lista
        /// </summary>
        public void Agregar(Usuario u)
        {
            u.Id = proximoId++;
            usuarios.Add(u);
        }

        /// <summary>
        /// Edita un usuario existente según su ID
        /// </summary>
        public void Editar(Usuario u)
        {
            var existente = usuarios.FirstOrDefault(x => x.Id == u.Id);
            if (existente != null)
            {
                existente.Nombre = u.Nombre;
                existente.Email = u.Email;
                existente.Password = u.Password;
                existente.Rol = u.Rol;
            }
        }

        /// <summary>
        /// Elimina un usuario por su ID
        /// </summary>
        public void Eliminar(int id)
        {
            var usuario = usuarios.FirstOrDefault(u => u.Id == id);
            if (usuario != null)
                usuarios.Remove(usuario);
        }

        /// <summary>
        /// Verifica las credenciales y devuelve el usuario si son correctas
        /// </summary>
        public Usuario? Login(string email, string password)
        {
            return usuarios.FirstOrDefault(u => u.Email == email && u.Password == password);
        }
    }
}
