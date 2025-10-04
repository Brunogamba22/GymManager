using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManager.Models
{
    public class DetalleRutina
    {
        public int IdDetalle { get; set; }
        public int IdRutina { get; set; }
        public int IdEjercicio { get; set; }
        public int Series { get; set; }
        public int Repeticiones { get; set; }
        public double? Carga { get; set; }
        public int Descanso { get; set; } // en segundos
    }

}
