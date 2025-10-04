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
        public int Id { get; set; }
        public string Nombre { get; set; } = "";
        public int GrupoMuscularId { get; set; }   // FK a Grupo_Muscular
        public string GrupoMuscularNombre { get; set; } = ""; // opcional, para joins
        public int CreadoPor { get; set; }         // FK a Usuarios.id_usuario
        public string Imagen { get; set; } = "";   // ruta de imagen

        // ❌ Series/Repeticiones/Descanso se eliminan -> están en DetalleRutina
    }
}

