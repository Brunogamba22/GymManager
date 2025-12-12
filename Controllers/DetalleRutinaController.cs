using GymManager.Models;
using GymManager.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GymManager.Controllers
{
    public class DetalleRutinaController
    {
        // =========================================================
        // MÉTODO: ObtenerPorRutina()
        // =========================================================
        public List<DetalleRutina> ObtenerPorRutina(int idRutina)
        {
            var detalles = new List<DetalleRutina>();

            try
            {
                using (var conn = new SqlConnection(Conexion.Cadena))
                {
                    conn.Open();

                    string query = @"
                        SELECT 
                            dr.id_detalle AS IdDetalle,
                            dr.id_rutina AS IdRutina,
                            dr.id_ejercicio AS IdEjercicio,
                            e.nombre AS EjercicioNombre,
                            dr.series AS Series,
                            dr.repeticiones AS Repeticiones,
                            dr.carga AS Carga,
                            e.imagen AS Imagen,                    
                            gm.nombre AS GrupoMuscularNombre
                        FROM DetalleRutina dr
                        INNER JOIN Ejercicios e ON dr.id_ejercicio = e.id_ejercicio
                        INNER JOIN Grupo_Muscular gm ON e.id_grupo_muscular = gm.id_grupo_muscular
                        WHERE dr.id_rutina = @IdRutina;";

                    using (var cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@IdRutina", idRutina);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var detalle = new DetalleRutina
                                {
                                    IdDetalle = reader.GetInt32(reader.GetOrdinal("IdDetalle")),
                                    IdRutina = reader.GetInt32(reader.GetOrdinal("IdRutina")),
                                    IdEjercicio = reader.GetInt32(reader.GetOrdinal("IdEjercicio")),
                                    EjercicioNombre = reader.GetString(reader.GetOrdinal("EjercicioNombre")),
                                    Series = reader.GetInt32(reader.GetOrdinal("Series")),
                                    Repeticiones = reader.GetInt32(reader.GetOrdinal("Repeticiones")),
                                    Carga = reader.IsDBNull(reader.GetOrdinal("Carga")) ? "" : reader.GetString(reader.GetOrdinal("Carga")),
                                    Imagen = reader.IsDBNull(reader.GetOrdinal("Imagen")) ? "" : reader.GetString(reader.GetOrdinal("Imagen")),
                                    GrupoMuscular = reader.IsDBNull(reader.GetOrdinal("GrupoMuscularNombre")) ? "" : reader.GetString(reader.GetOrdinal("GrupoMuscularNombre"))
                                };
                                detalles.Add(detalle);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al obtener detalles de la rutina: " + ex.Message, ex);
            }

            return detalles;
        }

        // =========================================================
        // MÉTODO: Agregar()
        // =========================================================
        public void Agregar(DetalleRutina d)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = @"
                    INSERT INTO DetalleRutina (id_rutina, id_ejercicio, series, repeticiones, carga)
                    VALUES (@idRutina, @idEjercicio, @series, @reps, @carga);";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idRutina", d.IdRutina);
                    cmd.Parameters.AddWithValue("@idEjercicio", d.IdEjercicio);
                    cmd.Parameters.AddWithValue("@series", d.Series);
                    cmd.Parameters.AddWithValue("@reps", d.Repeticiones);
                    cmd.Parameters.AddWithValue("@carga", (object)d.Carga ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // =========================================================
        // MÉTODO: Editar()
        // =========================================================
        public void Editar(DetalleRutina d)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = @"
                    UPDATE DetalleRutina
                    SET series = @series,
                        repeticiones = @reps,
                        carga = @carga
                    WHERE id_detalle = @idDetalle;";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@series", d.Series);
                    cmd.Parameters.AddWithValue("@reps", d.Repeticiones);
                    cmd.Parameters.AddWithValue("@carga", (object)d.Carga ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@idDetalle", d.IdDetalle);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // =========================================================
        // MÉTODO: Eliminar()
        // =========================================================
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

        // =========================================================
        // MÉTODO: EliminarPorRutina()
        // =========================================================
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
