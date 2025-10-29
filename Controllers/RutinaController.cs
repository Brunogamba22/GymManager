using GymManager.Models;
using GymManager.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GymManager.Controllers
{
    public class RutinaController
    {
        public int CrearEncabezadoRutina(string tipoRutina, int idProfesor, string nombre, int idGenero)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // Tu base de datos no tiene fecha_creacion ni tipo, adapté la consulta a tu esquema
                string query = @"
                    INSERT INTO Rutina (nombre, fecha, creadaPor, id_genero)
                    OUTPUT INSERTED.id_rutina
                    VALUES (@nombre, GETDATE(), @creadaPor, @idGenero);";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@creadaPor", idProfesor);
                    cmd.Parameters.AddWithValue("@idGenero", idGenero);

                    return (int)cmd.ExecuteScalar(); // Devuelve el ID recién creado
                }
            }
        }

        public void AgregarDetalle(DetalleRutina detalle)
        {
            try
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
            catch (Exception ex)
            {
                throw new Exception("Error al agregar detalle de rutina: " + ex.Message, ex);
            }
        }

        // ====================================================================
        //  MÉTODO NUEVO REQUERIDO POR UcEditarRutina 
        // ====================================================================
        /// <summary>
        /// Busca el encabezado de la última rutina creada para un género específico.
        /// </summary>
        /// <returns>Un objeto Rutina, o null si no se encuentra.</returns>
        public Rutina ObtenerUltimaRutinaPorGenero(int idGenero)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // Asume que la columna 'fecha' se mapea a 'FechaCreacion' en tu modelo Rutina
                string query = @"
                    SELECT TOP 1 id_rutina, nombre, fecha, creadaPor, id_genero
                    FROM Rutina
                    WHERE id_genero = @idGenero
                    ORDER BY fecha DESC;";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idGenero", idGenero);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Asumimos que tu modelo Rutina.cs tiene estas propiedades
                            return new Rutina
                            {
                                IdRutina = reader.GetInt32(reader.GetOrdinal("id_rutina")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("fecha")),
                                CreadaPor = reader.GetInt32(reader.GetOrdinal("creadaPor")),
                                IdGenero = reader.GetInt32(reader.GetOrdinal("id_genero"))
                            };
                        }
                    }
                }
            }
            return null; // No se encontró rutina para ese género
        }

        
        // =========================================================
        // 🔥 MÉTODO ACTUALIZADO (para el panel Planillas con filtros) 🔥
        // =========================================================
        /// <summary>
        /// Obtiene todos los encabezados de rutinas guardadas, opcionalmente filtrados
        /// por fecha y/o género. Incluye nombre de profesor y género.
        /// </summary>
        public List<Rutina> ObtenerTodasParaPlanilla(DateTime? fechaDesde = null, DateTime? fechaHasta = null, int? idGenero = null)
        {
            var lista = new List<Rutina>();
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // Base de la consulta
                string query = @"
                SELECT 
                    r.id_rutina, r.nombre, r.fecha, r.creadaPor, r.id_genero,
                    u.nombre AS nombreProfesor,
                    g.nombre AS nombreGenero
                FROM Rutina r
                INNER JOIN Usuarios u ON r.creadaPor = u.id_usuario
                INNER JOIN Genero g ON r.id_genero = g.id_genero
                WHERE 1=1 "; // Condición base para añadir AND fácilmente

                var parameters = new List<SqlParameter>();

                // Añadir filtro de fecha desde (si se proporcionó)
                if (fechaDesde.HasValue)
                {
                    // Comparamos solo la parte de la fecha (ignorando la hora)
                    query += " AND CONVERT(date, r.fecha) >= @fechaDesde ";
                    parameters.Add(new SqlParameter("@fechaDesde", fechaDesde.Value.Date));
                }

                // Añadir filtro de fecha hasta (si se proporcionó)
                if (fechaHasta.HasValue)
                {
                    // Comparamos solo la parte de la fecha
                    query += " AND CONVERT(date, r.fecha) <= @fechaHasta ";
                    parameters.Add(new SqlParameter("@fechaHasta", fechaHasta.Value.Date));
                }

                // Añadir filtro de género (si se proporcionó y es mayor a 0)
                if (idGenero.HasValue && idGenero.Value > 0)
                {
                    query += " AND r.id_genero = @idGenero ";
                    parameters.Add(new SqlParameter("@idGenero", idGenero.Value));
                }

                query += " ORDER BY r.fecha DESC"; // Ordenar siempre

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