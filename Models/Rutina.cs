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

        // Mapea la columna 'fecha' de la BD
        public DateTime FechaCreacion { get; set; }

        // Mapea la columna 'creadaPor' (FK a Usuarios)
        public int CreadaPor { get; set; }

        // Mapea la columna 'id_genero' (FK a Genero)
        public int IdGenero { get; set; }

        public bool EsEditada { get; set; }

        // Las propiedades 'Tipo' y 'Activo' no existen en tu
        // diagrama de BD, por eso las quitamos.
        // (Estas no existen en la tabla 'Rutina', se llenan con consultas)
        public string NombreProfesor { get; set; } = "";
        public string NombreGenero { get; set; } = "";
    }
}