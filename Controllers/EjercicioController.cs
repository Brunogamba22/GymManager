using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GymManager.Models;
using GymManager.Utils;

namespace GymManager.Controllers
{
    public class EjercicioController
    {
        // ------------------------------------------------------------
        // LEE TODOS LOS EJERCICIOS ACTIVOS (JOIN con Grupo_Muscular)
        // ------------------------------------------------------------
        public List<Ejercicio> ObtenerTodos()
        {
            var lista = new List<Ejercicio>();

            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = @"
                    SELECT 
                        e.id_ejercicio,
                        e.nombre,
                        g.id_grupo_muscular,
                        g.nombre AS grupo_muscular,
                        e.creadoPor,
                        e.imagen
                    FROM Ejercicios e
                    INNER JOIN Grupo_Muscular g ON e.id_grupo_muscular = g.id_grupo_muscular
                    WHERE e.Activo = 1
                    ORDER BY e.nombre;";

                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ejercicio = new Ejercicio
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id_ejercicio")),
                            Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                            GrupoMuscularId = reader.GetInt32(reader.GetOrdinal("id_grupo_muscular")),
                            GrupoMuscularNombre = reader.GetString(reader.GetOrdinal("grupo_muscular")),
                            CreadoPor = reader.GetInt32(reader.GetOrdinal("creadoPor")),
                            Imagen = reader.IsDBNull(reader.GetOrdinal("imagen"))
                                ? string.Empty
                                : reader.GetString(reader.GetOrdinal("imagen"))
                        };

                        lista.Add(ejercicio);
                    }
                }
            }

            return lista;
        }

        // ------------------------------------------------------------
        // ALTA DE EJERCICIO (siempre con Activo = 1)
        // ------------------------------------------------------------
        public void Agregar(Ejercicio e)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = @"
                    INSERT INTO Ejercicios (nombre, id_grupo_muscular, creadoPor, imagen, Activo)
                    VALUES (@n, @g, @c, @img, 1)";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@n", e.Nombre ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@g", e.GrupoMuscularId);
                    cmd.Parameters.AddWithValue("@c", e.CreadoPor);
                    cmd.Parameters.AddWithValue("@img", string.IsNullOrEmpty(e.Imagen) ? (object)DBNull.Value : e.Imagen);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ------------------------------------------------------------
        // MODIFICACIÓN DE EJERCICIO (solo si está Activo)
        // ------------------------------------------------------------
        public void Editar(Ejercicio e)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = @"
                    UPDATE Ejercicios
                    SET 
                        nombre = @n, 
                        id_grupo_muscular = @g, 
                        creadoPor = @c, 
                        imagen = @img
                    WHERE id_ejercicio = @id AND Activo = 1;";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@n", e.Nombre);
                    cmd.Parameters.AddWithValue("@g", e.GrupoMuscularId);
                    cmd.Parameters.AddWithValue("@c", e.CreadoPor);
                    cmd.Parameters.AddWithValue("@img", string.IsNullOrEmpty(e.Imagen) ? (object)DBNull.Value : e.Imagen);
                    cmd.Parameters.AddWithValue("@id", e.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ------------------------------------------------------------
        // BAJA LÓGICA DEL EJERCICIO (Activo = 0)
        // ------------------------------------------------------------
        public void Eliminar(int id)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = "UPDATE Ejercicios SET Activo = 0 WHERE id_ejercicio = @id;";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ------------------------------------------------------------
        // OBTENER UNO POR ID (para editar o ver detalle)
        // ------------------------------------------------------------
        public Ejercicio ObtenerPorId(int id)
        {
            Ejercicio ejercicio = null;

            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = @"
                    SELECT 
                        e.id_ejercicio,
                        e.nombre,
                        g.id_grupo_muscular,
                        g.nombre AS grupo_muscular,
                        e.creadoPor,
                        e.imagen
                    FROM Ejercicios e
                    INNER JOIN Grupo_Muscular g ON e.id_grupo_muscular = g.id_grupo_muscular
                    WHERE e.id_ejercicio = @id AND e.Activo = 1;";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            ejercicio = new Ejercicio
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id_ejercicio")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                GrupoMuscularId = reader.GetInt32(reader.GetOrdinal("id_grupo_muscular")),
                                GrupoMuscularNombre = reader.GetString(reader.GetOrdinal("grupo_muscular")),
                                CreadoPor = reader.GetInt32(reader.GetOrdinal("creadoPor")),
                                Imagen = reader.IsDBNull(reader.GetOrdinal("imagen"))
                                    ? string.Empty
                                    : reader.GetString(reader.GetOrdinal("imagen"))
                            };
                        }
                    }
                }
            }

            return ejercicio;
        }
    }
}
