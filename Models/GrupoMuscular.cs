using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManager.Models
{
    // Coincide con la tabla Grupo_Muscular
    public class GrupoMuscular
    {
        public int Id { get; set; }              // id_grupo_muscular
        public string Nombre { get; set; } = ""; // nombre

        public override string ToString() => Nombre;
    }
}
