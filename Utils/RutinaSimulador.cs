using System;
using System.Collections.Generic;
using System.Linq;
using GymManager.Models;

namespace GymManager.Utils
{
    // Clase estática que simula la generación de rutinas de ejercicios.
    // Se usa solo para pruebas cuando no queremos depender de la base de datos real.
    public static class RutinaSimulador
    {
        // 🧩 Lista simulada de ejercicios disponibles.
        // En la aplicación real, esto vendría de la base de datos (tabla Ejercicios).
        private static List<EjercicioSimulado> listaEjerciciosSimulados = new List<EjercicioSimulado>
        {
            // Cargamos una lista estática de ejercicios con su grupo muscular y detalles.
            new EjercicioSimulado { Id = 1, Nombre = "Press banca", GrupoMuscular = "Pectoral", Series = 3, Repeticiones = "10", DescansoSegundos = 60 },
            new EjercicioSimulado { Id = 2, Nombre = "Sentadillas", GrupoMuscular = "Piernas", Series = 4, Repeticiones = "12", DescansoSegundos = 90 },
            new EjercicioSimulado { Id = 3, Nombre = "Dominadas", GrupoMuscular = "Espalda", Series = 3, Repeticiones = "8", DescansoSegundos = 75 },
            new EjercicioSimulado { Id = 4, Nombre = "Press militar", GrupoMuscular = "Hombros", Series = 3, Repeticiones = "10", DescansoSegundos = 60 },
            new EjercicioSimulado { Id = 5, Nombre = "Curl de bíceps", GrupoMuscular = "Brazos", Series = 3, Repeticiones = "12", DescansoSegundos = 45 },
            new EjercicioSimulado { Id = 6, Nombre = "Peso muerto", GrupoMuscular = "Espalda", Series = 4, Repeticiones = "8", DescansoSegundos = 120 },
            new EjercicioSimulado { Id = 7, Nombre = "Fondos en paralelas", GrupoMuscular = "Tríceps", Series = 3, Repeticiones = "12", DescansoSegundos = 60 },
            new EjercicioSimulado { Id = 8, Nombre = "Zancadas", GrupoMuscular = "Piernas", Series = 3, Repeticiones = "12", DescansoSegundos = 75 },
            new EjercicioSimulado { Id = 9, Nombre = "Hip thrust", GrupoMuscular = "Glúteos", Series = 4, Repeticiones = "10", DescansoSegundos = 90 },
            new EjercicioSimulado { Id = 10, Nombre = "Burpees", GrupoMuscular = "Full Body", Series = 3, Repeticiones = "15", DescansoSegundos = 45 },
            new EjercicioSimulado { Id = 11, Nombre = "Plancha", GrupoMuscular = "Core", Series = 3, Repeticiones = "60", DescansoSegundos = 30 },
            new EjercicioSimulado { Id = 12, Nombre = "Mountain climbers", GrupoMuscular = "Core", Series = 3, Repeticiones = "20", DescansoSegundos = 30 }
        };

        // ------------------------------------------------------------
        // 📦 Clase auxiliar: representa un ejercicio simulado.
        // ------------------------------------------------------------
        public class EjercicioSimulado
        {
            public int Id { get; set; }                   // Identificador único
            public string Nombre { get; set; } = "";      // Nombre del ejercicio
            public string GrupoMuscular { get; set; } = "";// Grupo muscular trabajado
            public int Series { get; set; }               // Cantidad de series
            public string Repeticiones { get; set; } = "";// Rango o número de repeticiones
            public int DescansoSegundos { get; set; }     // Descanso entre series (en segundos)
        }

        // ------------------------------------------------------------
        // 📦 Clase EjercicioRutina: lo que se muestra al generar una rutina
        // ------------------------------------------------------------
        public class EjercicioRutina
        {
            public string Nombre { get; set; } = "";      // Nombre del ejercicio
            public int Series { get; set; }               // Cantidad de series
            public string Repeticiones { get; set; } = "";// Repeticiones
            public int DescansoSegundos { get; set; }     // Descanso (segundos)
            public string GrupoMuscular { get; set; } = "";// Grupo muscular
        }

        // ------------------------------------------------------------
        // 🧠 Genera una rutina según el tipo de usuario
        // ------------------------------------------------------------
        public static List<EjercicioRutina> GenerarRutina(string tipoRutina)
        {
            // Lista que se devolverá con los ejercicios generados
            List<EjercicioRutina> listaRutinaGenerada = new List<EjercicioRutina>();

            // Objeto para generar números aleatorios (para variedad)
            Random generadorAleatorio = new Random();

            // Selección del tipo de rutina mediante expresión switch
            return tipoRutina.ToLower() switch
            {
                "hombres" => GenerarRutinaHombres(generadorAleatorio),
                "mujeres" => GenerarRutinaMujeres(generadorAleatorio),
                "deportistas" => GenerarRutinaDeportistas(generadorAleatorio),
                _ => GenerarRutinaGeneral(generadorAleatorio)
            };
        }

        // ------------------------------------------------------------
        // 💪 Rutina para hombres (fuerza y masa muscular)
        // ------------------------------------------------------------
        private static List<EjercicioRutina> GenerarRutinaHombres(Random generadorAleatorio)
        {
            // Selecciona ejercicios de pecho, espalda y piernas
            var ejerciciosFuerza = listaEjerciciosSimulados
                .Where(e => e.GrupoMuscular == "Pectoral" || e.GrupoMuscular == "Espalda" || e.GrupoMuscular == "Piernas")
                .OrderBy(x => generadorAleatorio.Next())
                .Take(4)
                .ToList();

            // Crea la lista de ejercicios personalizados para la rutina
            return ejerciciosFuerza.Select(e => new EjercicioRutina
            {
                Nombre = e.Nombre,
                Series = generadorAleatorio.Next(3, 5), // Entre 3 y 4 series
                Repeticiones = generadorAleatorio.Next(6, 12).ToString(), // 6 a 11 repeticiones
                DescansoSegundos = generadorAleatorio.Next(60, 91), // 60-90 segundos
                GrupoMuscular = e.GrupoMuscular
            }).ToList();
        }

        // ------------------------------------------------------------
        // 🧘 Rutina para mujeres (piernas, glúteos y core)
        // ------------------------------------------------------------
        private static List<EjercicioRutina> GenerarRutinaMujeres(Random generadorAleatorio)
        {
            var ejerciciosMujeres = listaEjerciciosSimulados
                .Where(e => e.GrupoMuscular == "Glúteos" || e.GrupoMuscular == "Piernas" || e.GrupoMuscular == "Core")
                .OrderBy(x => generadorAleatorio.Next())
                .Take(4)
                .ToList();

            return ejerciciosMujeres.Select(e => new EjercicioRutina
            {
                Nombre = e.Nombre,
                Series = generadorAleatorio.Next(3, 4),
                Repeticiones = generadorAleatorio.Next(10, 16).ToString(),
                DescansoSegundos = generadorAleatorio.Next(45, 76),
                GrupoMuscular = e.GrupoMuscular
            }).ToList();
        }

        // ------------------------------------------------------------
        // 🏃 Rutina para deportistas (resistencia y cuerpo completo)
        // ------------------------------------------------------------
        private static List<EjercicioRutina> GenerarRutinaDeportistas(Random generadorAleatorio)
        {
            var ejerciciosDeportistas = listaEjerciciosSimulados
                .Where(e => e.GrupoMuscular == "Full Body" || e.GrupoMuscular == "Core" || e.GrupoMuscular == "Piernas")
                .OrderBy(x => generadorAleatorio.Next())
                .Take(4)
                .ToList();

            return ejerciciosDeportistas.Select(e => new EjercicioRutina
            {
                Nombre = e.Nombre,
                Series = generadorAleatorio.Next(3, 5),
                Repeticiones = generadorAleatorio.Next(12, 21).ToString(),
                DescansoSegundos = generadorAleatorio.Next(30, 61),
                GrupoMuscular = e.GrupoMuscular
            }).ToList();
        }

        // ------------------------------------------------------------
        // 🏋 Rutina general (mezcla de ejercicios)
        // ------------------------------------------------------------
        private static List<EjercicioRutina> GenerarRutinaGeneral(Random generadorAleatorio)
        {
            var ejerciciosGenerales = listaEjerciciosSimulados
                .OrderBy(x => generadorAleatorio.Next())
                .Take(4)
                .ToList();

            return ejerciciosGenerales.Select(e => new EjercicioRutina
            {
                Nombre = e.Nombre,
                Series = generadorAleatorio.Next(3, 4),
                Repeticiones = generadorAleatorio.Next(8, 15).ToString(),
                DescansoSegundos = generadorAleatorio.Next(45, 76),
                GrupoMuscular = e.GrupoMuscular
            }).ToList();
        }

        // ------------------------------------------------------------
        // ➕ Agregar ejercicio manualmente a la lista simulada
        // ------------------------------------------------------------
        public static void AgregarEjercicio(EjercicioSimulado nuevoEjercicio)
        {
            nuevoEjercicio.Id = listaEjerciciosSimulados.Count + 1; // Asigna ID incremental
            listaEjerciciosSimulados.Add(nuevoEjercicio);            // Lo agrega a la lista
        }

        // ------------------------------------------------------------
        // 🔍 Obtener todos los ejercicios disponibles (simulados)
        // ------------------------------------------------------------
        public static List<EjercicioSimulado> ObtenerEjerciciosDisponibles()
        {
            return listaEjerciciosSimulados; // Devuelve la lista completa
        }
    }
}
