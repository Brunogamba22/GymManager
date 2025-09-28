using System;
using System.Collections.Generic;

namespace GymManager.Models.Events
{
    public class RutinaGeneradaEventArgs : EventArgs
    {
        public string TipoRutina { get; set; } = "";
        public List<GymManager.Utils.RutinaSimulador.EjercicioRutina> Ejercicios { get; set; } = new List<GymManager.Utils.RutinaSimulador.EjercicioRutina>();
    }

    public static class EventosRutina
    {
        public static event EventHandler<RutinaGeneradaEventArgs> RutinaGeneradaParaEdicion;

        public static void DispararRutinaGenerada(string tipoRutina, List<GymManager.Utils.RutinaSimulador.EjercicioRutina> ejercicios)
        {
            RutinaGeneradaParaEdicion?.Invoke(null, new RutinaGeneradaEventArgs
            {
                TipoRutina = tipoRutina,
                Ejercicios = ejercicios
            });
        }
    }
}