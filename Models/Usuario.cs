using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManager.Models
{
    // Los roles posibles en el sistema (definidos en tu ERS).
    // Se usa para controlar qué ve y qué puede hacer cada usuario.
    public enum Rol
    {
        Administrador,
        Profesor,
        Recepcionista
    }

    // Representa a un usuario del sistema (ya registrado o logueado).
    // Es una "clase modelo" que se corresponde con la tabla de Usuarios en la BD.
    public class Usuario
    {
        // Identificador único del usuario (clave primaria en la BD).
        public int Id { get; set; }

        // Nombre del usuario (por ejemplo "Juan Pérez").
        public string Nombre { get; set; } = "";

        // Correo electrónico del usuario (también se podría usar como login).
        public string Email { get; set; } = "";

        //contraseña del usuario
        public string Password { get; set; } = "";

        // Rol asignado (Admin / Profesor / Recepcionista).
        // Determina los permisos que tendrá dentro de la aplicación.
        public Rol Rol { get; set; }
    }
}

