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
            // 1. Inicializa el reporte con ceros
            var reporte = new ReporteActividadItem();

            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();
                string query = @"
            SELECT 
                esEditada,
                COUNT(*) AS Conteo
            FROM Rutina
            WHERE 
                creadaPor = @idProfesor
                AND CONVERT(date, fecha) BETWEEN @fechaDesde AND @fechaHasta
            GROUP BY 
                esEditada;
        ";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idProfesor", idProfesor);
                    cmd.Parameters.AddWithValue("@fechaDesde", fechaDesde.Date);
                    cmd.Parameters.AddWithValue("@fechaHasta", fechaHasta.Date);

                    using (var reader = cmd.ExecuteReader())
                    {
                        // 2. Lee los resultados (puede devolver 0, 1 o 2 filas)
                        while (reader.Read())
                        {
                            // Lee el valor 'bit' (booleano) de la BD
                            bool esEditada = false;
                            var valor = reader["esEditada"];
                            if (valor != DBNull.Value && (valor is bool b && b == true || valor is int i && i == 1))
                            {
                                esEditada = true;
                            }

                            int conteo = reader.GetInt32(reader.GetOrdinal("Conteo"));

                            // 3. Asigna el conteo a la propiedad correcta
                            if (esEditada)
                                reporte.RutinasEditadas = conteo;
                            else
                                reporte.RutinasNuevas = conteo;
                        }
                    }
                }
            }

            // 4. Calcula el total y devuelve el objeto
            reporte.TotalRutinas = reporte.RutinasNuevas + reporte.RutinasEditadas;
            return reporte;
        }
    }
}
