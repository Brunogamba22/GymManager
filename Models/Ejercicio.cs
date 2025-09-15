using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManager.Models
{
    // Clase que representa un ejercicio del gimnasio
    // Será usado tanto por el administrador (alta/baja) como por el profesor (rutinas)
    public class Ejercicio
    {
        public int Id { get; set; }               // Identificador único
        public string Nombre { get; set; } = "";  // Nombre del ejercicio
        public string Musculo { get; set; } = ""; // Músculo trabajado
        public string Descripcion { get; set; } = ""; // Descripción opcional
    }
}

