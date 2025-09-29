using System;
using System.Collections.Generic;

namespace GymManager.Models.Events
{
    public class RutinaGeneradaEventArgs : EventArgs
    {
        public string TipoRutina { get; set; } = "";
        public string NombreRutina { get; set; } = ""; // 🔥 AÑADIDO: Propiedad faltante
        public List<GymManager.Utils.RutinaSimulador.EjercicioRutina> Ejercicios { get; set; } = new List<GymManager.Utils.RutinaSimulador.EjercicioRutina>();
    }

    // 🔥 NUEVA CLASE PARA ARGUMENTOS DE GUARDADO
    public class RutinaGuardadaEventArgs : EventArgs
    {
        public string TipoRutina { get; set; } = "";
        public string NombreRutina { get; set; } = "";
        public List<GymManager.Utils.RutinaSimulador.EjercicioRutina> Ejercicios { get; set; } = new List<GymManager.Utils.RutinaSimulador.EjercicioRutina>();
        public DateTime FechaCreacion { get; set; } = DateTime.Now;
    }

    public static class EventosRutina
    {
        public static event EventHandler<RutinaGeneradaEventArgs> RutinaGeneradaParaEdicion;

        // 🔥 NUEVO EVENTO PARA GUARDADO
        public static event EventHandler<RutinaGuardadaEventArgs> RutinaGuardada;

        // 🔥 CORREGIDO: Agregar parámetro nombreRutina
        public static void DispararRutinaGenerada(string tipoRutina, string nombreRutina, List<GymManager.Utils.RutinaSimulador.EjercicioRutina> ejercicios)
        {
            RutinaGeneradaParaEdicion?.Invoke(null, new RutinaGeneradaEventArgs
            {
                TipoRutina = tipoRutina,
                NombreRutina = nombreRutina,
                Ejercicios = ejercicios
            });
        }

        // 🔥 NUEVO MÉTODO PARA DISPARAR EVENTO DE GUARDADO
        public static void DispararRutinaGuardada(string nombreRutina, string tipoRutina, DateTime fechaCreacion, List<GymManager.Utils.RutinaSimulador.EjercicioRutina> ejercicios)
        {
            RutinaGuardada?.Invoke(null, new RutinaGuardadaEventArgs
            {
                TipoRutina = tipoRutina,
                Ejercicios = ejercicios,
                NombreRutina = nombreRutina,
                FechaCreacion = fechaCreacion
            });
        }
    }
}