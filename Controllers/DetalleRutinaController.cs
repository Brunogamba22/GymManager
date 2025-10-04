using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GymManager.Models;
using GymManager.Utils;

namespace GymManager.Controllers
{
    // ------------------------------------------------------------
    // Controlador para gestionar los detalles de cada rutina
    // (Series, Repeticiones, Carga y Descanso)
    // ------------------------------------------------------------
    public class DetalleRutinaController
    {
        // ------------------------------------------------------------
        // MÉTODO: ObtenerPorRutina()
        // Devuelve todos los ejercicios (detalles) de una rutina específica
        // ------------------------------------------------------------
        public List<DetalleRutina> ObtenerPorRutina(int idRutina)
        {
            var lista = new List<DetalleRutina>();

            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = @"
                    SELECT 
                        d.id_detalle,
                        d.id_rutina,
                        d.id_ejercicio,
                        e.nombre AS nombre_ejercicio,
                        g.nombre AS grupo_muscular,
                        d.series,
                        d.repeticiones,
                        d.carga,
                        d.descanso
                    FROM DetalleRutina d
                    INNER JOIN Ejercicios e ON d.id_ejercicio = e.id_ejercicio
                    INNER JOIN Grupo_Muscular g ON e.id_grupo_muscular = g.id_grupo_muscular
                    WHERE d.id_rutina = @idRutina
                    ORDER BY d.id_detalle;";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idRutina", idRutina);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var detalle = new DetalleRutina
                            {
                                IdDetalle = reader.GetInt32(reader.GetOrdinal("id_detalle")),
                                IdRutina = reader.GetInt32(reader.GetOrdinal("id_rutina")),
                                IdEjercicio = reader.GetInt32(reader.GetOrdinal("id_ejercicio")),
                                Series = reader.GetInt32(reader.GetOrdinal("series")),
                                Repeticiones = reader.GetInt32(reader.GetOrdinal("repeticiones")),
                                Carga = reader.IsDBNull(reader.GetOrdinal("carga"))
                                            ? (double?)null
                                            : reader.GetDouble(reader.GetOrdinal("carga")),
                                Descanso = reader.IsDBNull(reader.GetOrdinal("descanso"))
                                            ? 0
                                            : reader.GetInt32(reader.GetOrdinal("descanso"))
                            };

                            lista.Add(detalle);
                        }
                    }
                }
            }

            return lista;
        }

        // ------------------------------------------------------------
        // MÉTODO: Agregar()
        // Inserta un nuevo detalle (ejercicio) dentro de una rutina
        // ------------------------------------------------------------
#nullable enable
        public void Agregar(DetalleRutina d)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = @"
            INSERT INTO DetalleRutina 
                (id_rutina, id_ejercicio, series, repeticiones, carga, descanso)
            VALUES 
                (@idRutina, @idEjercicio, @series, @reps, @carga, @descanso);";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idRutina", d.IdRutina);
                    cmd.Parameters.AddWithValue("@idEjercicio", d.IdEjercicio);
                    cmd.Parameters.AddWithValue("@series", d.Series);
                    cmd.Parameters.AddWithValue("@reps", d.Repeticiones);
                    cmd.Parameters.AddWithValue("@carga", (object?)d.Carga ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@descanso", d.Descanso);

                    cmd.ExecuteNonQuery();
                }
            }
        }
#nullable disable


        // ------------------------------------------------------------
        // MÉTODO: Editar()
        // Permite modificar un detalle de rutina existente
        // ------------------------------------------------------------
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
                descanso = @descanso
            WHERE id_detalle = @idDetalle;";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@series", d.Series);
                    cmd.Parameters.AddWithValue("@reps", d.Repeticiones);
                    cmd.Parameters.AddWithValue("@carga", d.Carga.HasValue ? d.Carga.Value : (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@descanso", d.Descanso);
                    cmd.Parameters.AddWithValue("@idDetalle", d.IdDetalle);

                    cmd.ExecuteNonQuery();
                }
            }
        }


        // ------------------------------------------------------------
        // MÉTODO: Eliminar()
        // Elimina un ejercicio asociado a una rutina
        // ------------------------------------------------------------
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

        // ------------------------------------------------------------
        // MÉTODO: EliminarPorRutina()
        // Elimina todos los detalles asociados a una rutina (ej: al borrar una rutina completa)
        // ------------------------------------------------------------
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
