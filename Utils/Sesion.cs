using GymManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GymManager.Utils
{
    // Clase estática que guarda la sesión actual del sistema.
    // Como es "static", no hace falta crear un objeto de Sesion, se accede directo: Sesion.Actual.
    public static class Sesion
    {
        // Usuario que está actualmente logueado.
        // Puede ser null si nadie inició sesión todavía.
        public static Usuario? Actual { get; set; }

        // Método helper: permite preguntar rápido si el usuario actual tiene cierto rol.
        // Ejemplo: if (Sesion.Es(Rol.Profesor)) { ... } 
        public static bool Es(Rol rol) => Actual?.Rol == rol;

        // Método para cerrar sesión: "borra" el usuario actual.
        public static void Cerrar()
        {
            Actual = null;
        }
    }
}

