using GymManager.Models;
using GymManager.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GymManager.Controllers
{
    public class RutinaController
    {
        public int CrearEncabezadoRutina(string tipoRutina, int idProfesor, string nombre, int idGenero, bool esEditada = false)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = @"
                    INSERT INTO Rutina (nombre, fecha, creadaPor, id_genero, esEditada)
                    VALUES (@nombre, GETDATE(), @creadaPor, @idGenero, @esEditada);
                    SELECT CAST(SCOPE_IDENTITY() AS INT);";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@creadaPor", idProfesor);
                    cmd.Parameters.AddWithValue("@idGenero", idGenero);
                    cmd.Parameters.Add("@esEditada", System.Data.SqlDbType.Bit).Value = esEditada ? 1 : 0;


                    return (int)cmd.ExecuteScalar();
                }
            }
        }

        public void AgregarDetalle(DetalleRutina detalle)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = @"
                    INSERT INTO DetalleRutina (id_rutina, id_ejercicio, series, repeticiones, carga)
                    VALUES (@idRutina, @idEjercicio, @series, @repeticiones, @carga);";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idRutina", detalle.IdRutina);
                    cmd.Parameters.AddWithValue("@idEjercicio", detalle.IdEjercicio);
                    cmd.Parameters.AddWithValue("@series", detalle.Series);
                    cmd.Parameters.AddWithValue("@repeticiones", detalle.Repeticiones);
                    cmd.Parameters.AddWithValue("@carga", (object)detalle.Carga ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Rutina> ObtenerTodasParaPlanilla(DateTime? fechaDesde = null, DateTime? fechaHasta = null, int? idGenero = null)
        {
            var lista = new List<Rutina>();

            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = @"
            SELECT 
                r.id_rutina, 
                r.nombre, 
                r.fecha,
                r.creadaPor, 
                r.id_genero,
                r.esEditada,
                u.nombre AS nombreProfesor,
                g.nombre AS nombreGenero
            FROM Rutina r
            INNER JOIN Usuarios u ON r.creadaPor = u.id_usuario
            INNER JOIN Genero g ON r.id_genero = g.id_genero
            WHERE 1=1";

                var parameters = new List<SqlParameter>();

                if (fechaDesde.HasValue)
                {
                    query += " AND r.fecha >= @fechaDesde";
                    parameters.Add(new SqlParameter("@fechaDesde", fechaDesde.Value.Date));
                }

                if (fechaHasta.HasValue)
                {
                    query += " AND r.fecha <= @fechaHasta";
                    parameters.Add(new SqlParameter("@fechaHasta", fechaHasta.Value.Date));
                }

                if (idGenero.HasValue && idGenero.Value > 0)
                {
                    query += " AND r.id_genero = @idGenero";
                    parameters.Add(new SqlParameter("@idGenero", idGenero.Value));
                }

                query += " ORDER BY r.fecha DESC;";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lista.Add(new Rutina
                            {
                                IdRutina = reader.GetInt32(reader.GetOrdinal("id_rutina")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("fecha")),
                                CreadaPor = reader.GetInt32(reader.GetOrdinal("creadaPor")),
                                IdGenero = reader.GetInt32(reader.GetOrdinal("id_genero")),
                                EsEditada = Convert.ToBoolean(reader["esEditada"]),
                                NombreProfesor = reader.GetString(reader.GetOrdinal("nombreProfesor")),
                                NombreGenero = reader.GetString(reader.GetOrdinal("nombreGenero"))
                            });
                        }
                    }
                }
            }

            return lista;
        }

    }
}
