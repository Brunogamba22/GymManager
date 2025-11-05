using GymManager.Models;
using GymManager.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

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




        public List<Rutina> ObtenerTodasParaPlanilla(DateTime? fechaDesde = null, DateTime? fechaHasta = null, int? idGenero = null, bool? soloEditadas = null, int? idProfesor = null)
        {
            var lista = new List<Rutina>();

            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = @"
            SELECT 
                r.id_rutina, r.nombre, r.fecha, r.creadaPor, r.id_genero, r.esEditada,
                u.nombre AS nombreProfesor,
                g.nombre AS nombreGenero
            FROM Rutina r
            INNER JOIN Usuarios u ON r.creadaPor = u.id_usuario
            INNER JOIN Genero g ON r.id_genero = g.id_genero
            WHERE 1=1";

                var parameters = new List<SqlParameter>();

                // =========================================================
                // 🔥 CORRECCIÓN CRÍTICA DE FILTRO DE FECHA
                // =========================================================

                // Si se proporciona fechaDesde, filtramos desde el inicio de ese día.
                if (fechaDesde.HasValue)
                {
                    // Convertimos la fecha a la medianoche (00:00:00) del día seleccionado
                    query += " AND r.fecha >= @fechaInicioDia";
                    parameters.Add(new SqlParameter("@fechaInicioDia", fechaDesde.Value.Date));
                }

                // Si se proporciona fechaHasta, filtramos hasta el FINAL de ese día.
                if (fechaHasta.HasValue)
                {
                    // Calculamos el final del día: el inicio del día siguiente (2025-11-06 00:00:00)
                    DateTime fechaFinDia = fechaHasta.Value.Date.AddDays(1);
                    query += " AND r.fecha < @fechaFinDia";
                    parameters.Add(new SqlParameter("@fechaFinDia", fechaFinDia));
                }
                // Si ambos son null (por el checkbox 'Todas las fechas' en el UI),
                // no se agrega ningún filtro de fecha, logrando el efecto 'TODOS'.

                // =========================================================
                // FIN CORRECCIÓN CRÍTICA
                // =========================================================

                if (idGenero.HasValue && idGenero.Value > 0)
                {
                    query += " AND r.id_genero = @idGenero";
                    parameters.Add(new SqlParameter("@idGenero", idGenero.Value));
                }


                if (soloEditadas.HasValue && soloEditadas.Value == true)
                {
                    query += " AND r.esEditada = 1"; // Filtra en la BD
                }

                if (idProfesor.HasValue && idProfesor.Value > 0)
                {
                    query += " AND r.creadaPor = @idProfesor";
                    parameters.Add(new SqlParameter("@idProfesor", idProfesor.Value));
                }


                query += " ORDER BY r.fecha DESC;";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddRange(parameters.ToArray());

                    using (var reader = cmd.ExecuteReader())
                    {
                        // ... (Tu lógica de lectura del reader está perfecta) ...
                        while (reader.Read())
                        {
                            bool esEditada = false;
                            var valor = reader["esEditada"];
                            if (valor != DBNull.Value)
                            {
                                if (valor is bool b) esEditada = b;
                                else if (valor is byte by) esEditada = by == 1;
                                else if (valor is int i) esEditada = i == 1;
                                else if (valor is string s) esEditada = s == "1" || s.Equals("true", StringComparison.OrdinalIgnoreCase);
                            }

                            lista.Add(new Rutina
                            {
                                IdRutina = reader.GetInt32(reader.GetOrdinal("id_rutina")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                FechaCreacion = reader.GetDateTime(reader.GetOrdinal("fecha")),
                                CreadaPor = reader.GetInt32(reader.GetOrdinal("creadaPor")),
                                IdGenero = reader.GetInt32(reader.GetOrdinal("id_genero")),
                                EsEditada = esEditada,
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
