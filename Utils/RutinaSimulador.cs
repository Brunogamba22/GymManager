using System;
using System.Collections.Generic;
using System.Linq;
using GymManager.Models;

namespace GymManager.Utils
{
    public static class RutinaSimulador
    {
        // Base de datos simulada de ejercicios (en un caso real, esto vendría de la BD)
        private static List<Ejercicio> _ejerciciosDisponibles = new List<Ejercicio>
        {
            new Ejercicio { Id = 1, Nombre = "Press banca", Musculo = "Pectoral", Descripcion = "Ejercicio para pectorales" },
            new Ejercicio { Id = 2, Nombre = "Sentadillas", Musculo = "Piernas", Descripcion = "Ejercicio para piernas" },
            new Ejercicio { Id = 3, Nombre = "Dominadas", Musculo = "Espalda", Descripcion = "Ejercicio para espalda" },
            new Ejercicio { Id = 4, Nombre = "Press militar", Musculo = "Hombros", Descripcion = "Ejercicio para hombros" },
            new Ejercicio { Id = 5, Nombre = "Curl de bíceps", Musculo = "Brazos", Descripcion = "Ejercicio para bíceps" },
            new Ejercicio { Id = 6, Nombre = "Peso muerto", Musculo = "Espalda", Descripcion = "Ejercicio para espalda baja" },
            new Ejercicio { Id = 7, Nombre = "Fondos en paralelas", Musculo = "Tríceps", Descripcion = "Ejercicio para tríceps" },
            new Ejercicio { Id = 8, Nombre = "Zancadas", Musculo = "Piernas", Descripcion = "Ejercicio para piernas" },
            new Ejercicio { Id = 9, Nombre = "Hip thrust", Musculo = "Glúteos", Descripcion = "Ejercicio para glúteos" },
            new Ejercicio { Id = 10, Nombre = "Burpees", Musculo = "Full Body", Descripcion = "Ejercicio cardiovascular" },
            new Ejercicio { Id = 11, Nombre = "Plancha", Musculo = "Core", Descripcion = "Ejercicio para abdomen" },
            new Ejercicio { Id = 12, Nombre = "Mountain climbers", Musculo = "Core", Descripcion = "Ejercicio cardiovascular" }
        };

        // Clase para representar un ejercicio en una rutina (con series y repeticiones)
        public class EjercicioRutina
        {
            public string Nombre { get; set; } = "";
            public int Series { get; set; }
            public int Repeticiones { get; set; }
            public int Descanso { get; set; }
            public string Musculo { get; set; } = "";
        }

        public static List<EjercicioRutina> GenerarRutina(string tipo)
        {
            var rutina = new List<EjercicioRutina>();
            var random = new Random();

            return tipo.ToLower() switch
            {
                "hombres" => GenerarRutinaHombres(random),
                "mujeres" => GenerarRutinaMujeres(random),
                "deportistas" => GenerarRutinaDeportistas(random),
                _ => GenerarRutinaGeneral(random)
            };
        }

        private static List<EjercicioRutina> GenerarRutinaHombres(Random random)
        {
            // Rutina para hombres: enfoque en fuerza y masa muscular
            var ejerciciosFuerza = _ejerciciosDisponibles
                .Where(e => e.Musculo == "Pectoral" || e.Musculo == "Espalda" || e.Musculo == "Piernas")
                .OrderBy(x => random.Next())
                .Take(4)
                .ToList();

            return ejerciciosFuerza.Select(e => new EjercicioRutina
            {
                Nombre = e.Nombre,
                Series = random.Next(3, 5), // 3-4 series
                Repeticiones = random.Next(6, 12), // 6-11 repeticiones
                Descanso = random.Next(60, 91), // 60-90 segundos
                Musculo = e.Musculo
            }).ToList();
        }

        private static List<EjercicioRutina> GenerarRutinaMujeres(Random random)
        {
            // Rutina para mujeres: enfoque en glúteos, piernas y core
            var ejerciciosMujeres = _ejerciciosDisponibles
                .Where(e => e.Musculo == "Glúteos" || e.Musculo == "Piernas" || e.Musculo == "Core")
                .OrderBy(x => random.Next())
                .Take(4)
                .ToList();

            return ejerciciosMujeres.Select(e => new EjercicioRutina
            {
                Nombre = e.Nombre,
                Series = random.Next(3, 4), // 3 series
                Repeticiones = random.Next(10, 16), // 10-15 repeticiones
                Descanso = random.Next(45, 76), // 45-75 segundos
                Musculo = e.Musculo
            }).ToList();
        }

        private static List<EjercicioRutina> GenerarRutinaDeportistas(Random random)
        {
            // Rutina para deportistas: enfoque en resistencia y full body
            var ejerciciosDeportistas = _ejerciciosDisponibles
                .Where(e => e.Musculo == "Full Body" || e.Musculo == "Core" || e.Musculo == "Piernas")
                .OrderBy(x => random.Next())
                .Take(4)
                .ToList();

            return ejerciciosDeportistas.Select(e => new EjercicioRutina
            {
                Nombre = e.Nombre,
                Series = random.Next(3, 5), // 3-4 series
                Repeticiones = random.Next(12, 21), // 12-20 repeticiones
                Descanso = random.Next(30, 61), // 30-60 segundos
                Musculo = e.Musculo
            }).ToList();
        }

        private static List<EjercicioRutina> GenerarRutinaGeneral(Random random)
        {
            // Rutina general: mezcla de todos los ejercicios
            var ejerciciosGenerales = _ejerciciosDisponibles
                .OrderBy(x => random.Next())
                .Take(4)
                .ToList();

            return ejerciciosGenerales.Select(e => new EjercicioRutina
            {
                Nombre = e.Nombre,
                Series = random.Next(3, 4),
                Repeticiones = random.Next(8, 15),
                Descanso = random.Next(45, 76),
                Musculo = e.Musculo
            }).ToList();
        }

        // Método para agregar más ejercicios (simula lo que haría el Admin)
        public static void AgregarEjercicio(Ejercicio nuevoEjercicio)
        {
            nuevoEjercicio.Id = _ejerciciosDisponibles.Count + 1;
            _ejerciciosDisponibles.Add(nuevoEjercicio);
        }

        // Método para obtener todos los ejercicios disponibles
        public static List<Ejercicio> ObtenerEjerciciosDisponibles()
        {
            return _ejerciciosDisponibles;
        }
    }
}