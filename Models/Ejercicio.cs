using System;

namespace GymManager.Models
{
    /// <summary>
    /// Representa un ejercicio del gimnasio.
    /// </summary>
    public class Ejercicio
    {
        // PK
        public int Id { get; set; }

        // Datos principales
        public string Nombre { get; set; } = string.Empty;

        // FK a Grupo_Muscular
        public int GrupoMuscularId { get; set; }

        /// <summary>
        /// Nombre del grupo muscular (rellenado vía JOIN; no se persiste).
        /// </summary>
        public string GrupoMuscularNombre { get; set; } = string.Empty;

        // Auditoría / metadatos
        public int CreadoPor { get; set; }

        /// <summary>
        /// Ruta del archivo de imagen (puede ser null o vacío si no se cargó).
        /// </summary>
        public string Imagen { get; set; }  // permite null en .NET Framework

        public bool Activo { get; set; } = true;// Indica si el ejercicio está activo o no


        // Helpers opcionales (no se persisten)
        public bool TieneImagen => !string.IsNullOrWhiteSpace(Imagen);

        public override string ToString() =>
            string.IsNullOrWhiteSpace(GrupoMuscularNombre)
                ? Nombre
                : $"{Nombre} ({GrupoMuscularNombre})";
    }
}
