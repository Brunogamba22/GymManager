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
        public List<DetalleRutina> ObtenerPorRutina(int idRutina)
        {
            var detalles = new List<DetalleRutina>();
            try
            {
                using (var conn = new SqlConnection(Conexion.Cadena))
                {
                    conn.Open();
                    // Modifica el SELECT para incluir 'Carga' y eliminar 'Descanso'
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
                            while (reader.Read())
                            {
                                var detalle = new DetalleRutina
                                {
                                    IdDetalle = Convert.ToInt32(reader["IdDetalleRutina"]),
                                    IdRutina = Convert.ToInt32(reader["IdRutina"]),
                                    IdEjercicio = Convert.ToInt32(reader["IdEjercicio"]),
                                    EjercicioNombre = reader["EjercicioNombre"].ToString(),
                                    Series = Convert.ToInt32(reader["Series"]),
                                    Repeticiones = Convert.ToInt32(reader["Repeticiones"]),
                                    // Descanso = Convert.ToInt32(reader["Descanso"]) // ¡ELIMINA ESTA LÍNEA!

                                    // Lee Carga, manejando el posible valor NULL
                                    Carga = reader["Carga"] != DBNull.Value ? Convert.ToInt32(reader["Carga"]) : (int?)null
                                };
                                detalles.Add(detalle);
                            }
                        }
                    }
                }
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