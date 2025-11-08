using GymManager.Utils;
using System;
using GymManager.Models;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManager.Controllers
{
    /// <summary>
    /// Se encarga de generar reportes y exportaciones
    /// (PDF, Excel, impresión de rutinas).
    /// </summary>
    internal class ReporteController
    {

        /// <summary>
        /// Obtiene la cantidad de ejercicios por grupo muscular
        /// para un profesor específico en un rango de fechas.
        /// </summary>
        public List<ReporteProfesor> ObtenerBalanceGruposMusculares(int idProfesor, DateTime fechaDesde, DateTime fechaHasta)
        {
            var lista = new List<ReporteProfesor>();

            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // Esta consulta une 4 tablas para contar los ejercicios
                // por grupo muscular de las rutinas creadas por el profesor.
                string query = @"
                SELECT 
                    gm.nombre AS GrupoMuscular,
                    COUNT(dr.id_detalle) AS Conteo
                FROM DetalleRutina dr
                INNER JOIN Ejercicios e ON dr.id_ejercicio = e.id_ejercicio
                INNER JOIN Grupo_Muscular gm ON e.id_grupo_muscular = gm.id_grupo_muscular
                INNER JOIN Rutina r ON dr.id_rutina = r.id_rutina
                WHERE 
                    r.creadaPor = @idProfesor
                    AND r.fecha BETWEEN @fechaDesde AND @fechaHasta
                GROUP BY 
                    gm.nombre
                ORDER BY 
                    Conteo DESC;
                   ";

                using (var cmd = new SqlCommand(query, conn))
                {
                    // Pasamos los parámetros
                    cmd.Parameters.AddWithValue("@idProfesor", idProfesor);
                    cmd.Parameters.AddWithValue("@fechaDesde", fechaDesde);
                    cmd.Parameters.AddWithValue("@fechaHasta", fechaHasta.AddDays(1).AddSeconds(-1)); // Para incluir todo el día 'hasta'

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ReporteProfesor
                            {
                                GrupoMuscular = reader.GetString(reader.GetOrdinal("GrupoMuscular")),
                                Conteo = reader.GetInt32(reader.GetOrdinal("Conteo"))
                            });
                        }
                    }
                }
            }
            return lista;
        }

        public ReporteActividadItem ObtenerReporteActividad(int idProfesor, DateTime fechaDesde, DateTime fechaHasta)
        {
            var reporte = new ReporteActividadItem();

            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // 1️⃣ Conteo de nuevas / editadas
                string queryBase = @"
                    SELECT 
                        esEditada,
                        COUNT(*) AS Conteo
                    FROM Rutina
                    WHERE 
                        creadaPor = @idProfesor
                        AND CONVERT(date, fecha) BETWEEN @fechaDesde AND @fechaHasta
                    GROUP BY esEditada;";

                using (var cmd = new SqlCommand(queryBase, conn))
                {
                    cmd.Parameters.AddWithValue("@idProfesor", idProfesor);
                    cmd.Parameters.AddWithValue("@fechaDesde", fechaDesde.Date);
                    cmd.Parameters.AddWithValue("@fechaHasta", fechaHasta.Date);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            bool esEditada = false;
                            var valor = reader["esEditada"];
                            if (valor != DBNull.Value && (valor is bool b && b || valor is int i && i == 1))
                                esEditada = true;

                            int conteo = reader.GetInt32(reader.GetOrdinal("Conteo"));
                            if (esEditada)
                                reporte.RutinasEditadas = conteo;
                            else
                                reporte.RutinasNuevas = conteo;
                        }
                    }
                }

                // 2️⃣ Conteo por género
                string queryGeneros = @"
                SELECT g.nombre AS Genero, COUNT(*) AS Conteo
                FROM Rutina r
                INNER JOIN Genero g ON r.id_genero = g.id_genero
                WHERE 
                    r.creadaPor = @idProfesor
                    AND CONVERT(date, r.fecha) BETWEEN @fechaDesde AND @fechaHasta
                GROUP BY g.nombre;";

                using (var cmd = new SqlCommand(queryGeneros, conn))
                {
                    cmd.Parameters.AddWithValue("@idProfesor", idProfesor);
                    cmd.Parameters.AddWithValue("@fechaDesde", fechaDesde.Date);
                    cmd.Parameters.AddWithValue("@fechaHasta", fechaHasta.Date);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string genero = reader["Genero"].ToString().ToLower();
                            int conteo = reader.GetInt32(reader.GetOrdinal("Conteo"));

                            if (genero.Contains("masculino") || genero.Contains("hombre"))
                                reporte.RutinasHombres = conteo;
                            else if (genero.Contains("femenino") || genero.Contains("mujer"))
                                reporte.RutinasMujeres = conteo;
                            else if (genero.Contains("deport"))
                                reporte.RutinasDeportistas = conteo;
                        }
                    }
                }
            }

            // 3️⃣ Total general
            reporte.TotalRutinas = reporte.RutinasNuevas + reporte.RutinasEditadas;
            return reporte;
        }


        // =========================================================
        // 🔥 MÉTODO NUEVO: Para el Tooltip del Gráfico de Torta
        // =========================================================
        public List<ReportePopularidad> ObtenerTop5EjerciciosPorGrupo(int idProfesor, DateTime fechaDesde, DateTime fechaHasta, string nombreGrupo)
        {
            var lista = new List<ReportePopularidad>();
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();
                string query = @"
            SELECT TOP 5 
                e.nombre AS EjercicioNombre,
                COUNT(dr.id_detalle) AS Conteo
            FROM DetalleRutina dr
            INNER JOIN Rutina r ON dr.id_rutina = r.id_rutina
            INNER JOIN Ejercicios e ON dr.id_ejercicio = e.id_ejercicio
            INNER JOIN Grupo_Muscular gm ON e.id_grupo_muscular = gm.id_grupo_muscular
            WHERE
                r.creadaPor = @idProfesor
                AND CONVERT(date, r.fecha) BETWEEN @fechaDesde AND @fechaHasta
                AND gm.nombre = @nombreGrupo  -- <-- El filtro clave por grupo
            GROUP BY 
                e.nombre
            ORDER BY 
                Conteo DESC;
        ";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idProfesor", idProfesor);
                    cmd.Parameters.AddWithValue("@fechaDesde", fechaDesde.Date);
                    cmd.Parameters.AddWithValue("@fechaHasta", fechaHasta.Date);
                    cmd.Parameters.AddWithValue("@nombreGrupo", nombreGrupo);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new ReportePopularidad
                            {
                                EjercicioNombre = reader.GetString(reader.GetOrdinal("EjercicioNombre")),
                                Conteo = reader.GetInt32(reader.GetOrdinal("Conteo"))
                            });
                        }
                    }
                }
            }
            return lista;
        }
    }
}
