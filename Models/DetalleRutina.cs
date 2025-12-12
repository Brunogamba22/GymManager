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
        public string Carga { get; set; } = "";
        //public int Descanso { get; set; } // en segundos
        // AGREGA ESTA PROPIEDAD:
        public string EjercicioNombre { get; set; } = "";

        // AGREGAR ESTAS DOS PROPIEDADES NUEVAS:
        public string Imagen { get; set; } = "";          // Nombre del archivo GIF
        public string GrupoMuscular { get; set; } = "";   // Para buscar en subcarpetas
    }

}
