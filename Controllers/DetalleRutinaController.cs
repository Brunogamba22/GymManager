using GymManager.Models;
using GymManager.Utils;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;

namespace GymManager.Controllers
{
    // ... Controlador para gestionar los detalles ...
    public class DetalleRutinaController
    {
        // ... MÉTODO: ObtenerPorRutina() ...
        // En DetalleRutinaController.cs

        public List<DetalleRutina> ObtenerPorRutina(int idRutina)
        {
            var detalles = new List<DetalleRutina>();
            try
            {
                using (var conn = new SqlConnection(Conexion.Cadena))
                {
                    conn.Open();
                    // La consulta SQL está bien (usa alias)
                    var query = @"SELECT dr.id_detalle AS IdDetalle,
                             dr.id_rutina AS IdRutina,
                             dr.id_ejercicio AS IdEjercicio,
                             e.nombre AS EjercicioNombre,
                             dr.series AS Series,
                             dr.repeticiones AS Repeticiones,
                             dr.carga AS Carga
                      FROM DetalleRutina dr
                      INNER JOIN Ejercicios e ON dr.id_ejercicio = e.id_ejercicio
                      WHERE dr.id_rutina = @IdRutina";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdRutina", idRutina);
                        using (var reader = cmd.ExecuteReader())
                        {
                            // =========================================================
                            // 🔥 CORRECCIÓN: Usar GetOrdinal para leer los datos 🔥
                            // =========================================================
                            while (reader.Read())
                            {
                                var detalle = new DetalleRutina
                                {
                                    // Usamos GetOrdinal con el nombre del ALIAS de la consulta SQL
                                    IdDetalle = reader.GetInt32(reader.GetOrdinal("IdDetalle")),
                                    IdRutina = reader.GetInt32(reader.GetOrdinal("IdRutina")),
                                    IdEjercicio = reader.GetInt32(reader.GetOrdinal("IdEjercicio")),
                                    EjercicioNombre = reader.GetString(reader.GetOrdinal("EjercicioNombre")),
                                    Series = reader.GetInt32(reader.GetOrdinal("Series")),
                                    Repeticiones = reader.GetInt32(reader.GetOrdinal("Repeticiones")),
                                    Carga = reader.IsDBNull(reader.GetOrdinal("Carga")) ? (double?)null : reader.GetDouble(reader.GetOrdinal("Carga"))
                                };
                                detalles.Add(detalle);
                            }
                            
                        }
                    }
                }
            }
            catch (IndexOutOfRangeException ioorex) // Captura específica por si GetOrdinal falla
            {
                throw new Exception($"Error al leer columna de detalles: Columna no encontrada ({ioorex.Message})", ioorex);
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener detalles de rutina por ID de rutina: " + ex.Message, ex);
            }
            return detalles;
        }

        // ... MÉTODO: Agregar() ...
#nullable enable
        public void Agregar(DetalleRutina d)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = @"
                INSERT INTO DetalleRutina 
                    (id_rutina, id_ejercicio, series, repeticiones, carga)
                VALUES 
                    (@idRutina, @idEjercicio, @series, @reps, @carga);";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idRutina", d.IdRutina);
                    cmd.Parameters.AddWithValue("@idEjercicio", d.IdEjercicio);
                    cmd.Parameters.AddWithValue("@series", d.Series);
                    cmd.Parameters.AddWithValue("@reps", d.Repeticiones);
                    cmd.Parameters.AddWithValue("@carga", (object?)d.Carga ?? DBNull.Value);
                    

                    cmd.ExecuteNonQuery();
                }
            }
        }
#nullable disable

        // ... MÉTODO: Editar() ...
        public void Editar(DetalleRutina d)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = @"
                UPDATE DetalleRutina
                SET series = @series,
                    repeticiones = @reps,
                    carga = @carga,
                WHERE id_detalle = @idDetalle;";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@series", d.Series);
                    cmd.Parameters.AddWithValue("@reps", d.Repeticiones);
                    cmd.Parameters.AddWithValue("@carga", d.Carga.HasValue ? d.Carga.Value : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@idDetalle", d.IdDetalle);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ... MÉTODO: Eliminar() ...
        public void Eliminar(int idDetalle)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();
                string query = "DELETE FROM DetalleRutina WHERE id_detalle = @idDetalle;";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idDetalle", idDetalle);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ... MÉTODO: EliminarPorRutina() ...
        public void EliminarPorRutina(int idRutina)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();
                string query = "DELETE FROM DetalleRutina WHERE id_rutina = @idRutina;";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idRutina", idRutina);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}