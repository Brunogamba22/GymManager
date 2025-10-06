using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManager.Models
{
    public class Rutina
    {
        public int IdRutina { get; set; }
        public string Nombre { get; set; } = "";
        public string Tipo { get; set; } = ""; // HOMBRES / MUJERES / DEPORTISTAS
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
        public int IdProfesor { get; set; }   // FK al usuario (profesor)
        public bool Activo { get; set; } = true;
    }
}
