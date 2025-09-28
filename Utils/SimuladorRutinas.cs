using System;
using System.Collections.Generic;

namespace GymManager.Utils
{
    public static class SimuladorRutinas
    {
        private static Random random = new Random();

        // Estructura de un ejercicio
        public class EjercicioSimulado
        {
            public string Nombre { get; set; }
            public int Series { get; set; }
            public int Repeticiones { get; set; }
            public string Descanso { get; set; }
        }

        // Diccionario de rutinas predefinidas
        private static Dictionary<string, List<List<EjercicioSimulado>>> rutinas = new Dictionary<string, List<List<EjercicioSimulado>>>
        {
            ["Hombres"] = new List<List<EjercicioSimulado>>
            {
                new List<EjercicioSimulado>
                {
                    new EjercicioSimulado { Nombre = "Press banca", Series = 3, Repeticiones = 10, Descanso = "60 s" },
                    new EjercicioSimulado { Nombre = "Sentadillas", Series = 4, Repeticiones = 8, Descanso = "90 s" },
                    new EjercicioSimulado { Nombre = "Dominadas", Series = 3, Repeticiones = 8, Descanso = "75 s" }
                },
                new List<EjercicioSimulado>
                {
                    new EjercicioSimulado { Nombre = "Peso muerto", Series = 4, Repeticiones = 6, Descanso = "120 s" },
                    new EjercicioSimulado { Nombre = "Remo con barra", Series = 3, Repeticiones = 10, Descanso = "75 s" },
                    new EjercicioSimulado { Nombre = "Fondos", Series = 3, Repeticiones = 12, Descanso = "60 s" }
                }
            },

            ["Mujeres"] = new List<List<EjercicioSimulado>>
            {
                new List<EjercicioSimulado>
                {
                    new EjercicioSimulado { Nombre = "Hip thrust", Series = 4, Repeticiones = 12, Descanso = "60 s" },
                    new EjercicioSimulado { Nombre = "Zancadas", Series = 3, Repeticiones = 10, Descanso = "75 s" },
                    new EjercicioSimulado { Nombre = "Elevación de pelvis", Series = 3, Repeticiones = 15, Descanso = "45 s" }
                },
                new List<EjercicioSimulado>
                {
                    new EjercicioSimulado { Nombre = "Peso muerto rumano", Series = 4, Repeticiones = 10, Descanso = "90 s" },
                    new EjercicioSimulado { Nombre = "Puente de glúteo", Series = 3, Repeticiones = 20, Descanso = "45 s" },
                    new EjercicioSimulado { Nombre = "Abducciones con banda", Series = 3, Repeticiones = 15, Descanso = "30 s" }
                }
            },

            ["Deportistas"] = new List<List<EjercicioSimulado>>
            {
                new List<EjercicioSimulado>
                {
                    new EjercicioSimulado { Nombre = "Burpees", Series = 3, Repeticiones = 15, Descanso = "45 s" },
                    new EjercicioSimulado { Nombre = "Plancha", Series = 3, Repeticiones = 1, Descanso = "30 s" },
                    new EjercicioSimulado { Nombre = "Mountain climbers", Series = 3, Repeticiones = 20, Descanso = "45 s" }
                },
                new List<EjercicioSimulado>
                {
                    new EjercicioSimulado { Nombre = "Saltos de caja", Series = 4, Repeticiones = 10, Descanso = "60 s" },
                    new EjercicioSimulado { Nombre = "Sprints", Series = 6, Repeticiones = 30, Descanso = "90 s" },
                    new EjercicioSimulado { Nombre = "Flexiones explosivas", Series = 3, Repeticiones = 12, Descanso = "60 s" }
                }
            }
        };

        // Método para devolver una rutina aleatoria según tipo
        public static List<EjercicioSimulado> ObtenerRutina(string tipo)
        {
            if (!rutinas.ContainsKey(tipo)) return new List<EjercicioSimulado>();

            var opciones = rutinas[tipo];
            int index = random.Next(opciones.Count);
            return opciones[index];
        }
    }
}
