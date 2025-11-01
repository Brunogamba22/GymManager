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
        // LEE TODOS LOS EJERCICIOS ACTIVOS
        //  - LEFT JOIN defensivo (si falta el grupo, no "desaparece")
        //  - Alias que coinciden con el modelo (Id, Nombre, etc.)
        //  - Imagen es RUTA RELATIVA (puede venir NULL)
        // ------------------------------------------------------------
        public List<Ejercicio> ObtenerTodos()
        {
            var lista = new List<Ejercicio>();

            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                var query = @"
                    SELECT
                        e.id_ejercicio       AS Id,
                        e.nombre             AS Nombre,
                        e.id_grupo_muscular  AS GrupoMuscularId,
                        gm.nombre            AS GrupoMuscularNombre,
                        e.creadoPor          AS CreadoPor,
                        e.imagen             AS Imagen
                    FROM dbo.Ejercicios e
                    LEFT JOIN dbo.Grupo_Muscular gm 
                           ON gm.id_grupo_muscular = e.id_grupo_muscular
                    WHERE e.Activo = 1
                    ORDER BY e.id_ejercicio;";

                using (var cmd = new SqlCommand(query, conn))
                using (var rd = cmd.ExecuteReader())
                {
                    while (rd.Read())
                    {
                        var ej = new Ejercicio
                        {
                            Id = rd.GetInt32(rd.GetOrdinal("Id")),
                            Nombre = rd.GetString(rd.GetOrdinal("Nombre")),
                            GrupoMuscularId = rd.GetInt32(rd.GetOrdinal("GrupoMuscularId")),
                            GrupoMuscularNombre = rd.IsDBNull(rd.GetOrdinal("GrupoMuscularNombre"))
                                                ? string.Empty
                                                : rd.GetString(rd.GetOrdinal("GrupoMuscularNombre")),
                            CreadoPor = rd.GetInt32(rd.GetOrdinal("CreadoPor")),
                            Imagen = rd.IsDBNull(rd.GetOrdinal("Imagen"))
                                                ? null
                                                : rd.GetString(rd.GetOrdinal("Imagen")) // RUTA RELATIVA (p.ej. "Pecho/press_banca.gif")
                        };
                        lista.Add(ej);
                    }
                }
            }

            return lista;
        }

        // ------------------------------------------------------------
        // ALTA DE EJERCICIO (siempre con Activo = 1)
        //  - Guarda Imagen como RUTA RELATIVA o NULL si está vacía
        // ------------------------------------------------------------
        public void Agregar(Ejercicio e)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                var query = @"
                    INSERT INTO dbo.Ejercicios (nombre, id_grupo_muscular, creadoPor, imagen, Activo)
                    VALUES (@n, @g, @c, @img, 1);";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@n", string.IsNullOrWhiteSpace(e.Nombre) ? (object)DBNull.Value : e.Nombre);
                    cmd.Parameters.AddWithValue("@g", e.GrupoMuscularId);
                    cmd.Parameters.AddWithValue("@c", e.CreadoPor);
                    cmd.Parameters.AddWithValue("@img", string.IsNullOrWhiteSpace(e.Imagen) ? (object)DBNull.Value : e.Imagen);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ------------------------------------------------------------
        // MODIFICACIÓN DE EJERCICIO (solo si está Activo)
        //  - Actualiza la ruta relativa o la pone NULL si viene vacía
        // ------------------------------------------------------------
        public void Editar(Ejercicio e)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                var query = @"
                    UPDATE dbo.Ejercicios
                    SET 
                        nombre            = @n,
                        id_grupo_muscular = @g,
                        creadoPor         = @c,
                        imagen            = @img
                    WHERE id_ejercicio   = @id
                      AND Activo         = 1;";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@n", string.IsNullOrWhiteSpace(e.Nombre) ? (object)DBNull.Value : e.Nombre);
                    cmd.Parameters.AddWithValue("@g", e.GrupoMuscularId);
                    cmd.Parameters.AddWithValue("@c", e.CreadoPor);
                    cmd.Parameters.AddWithValue("@img", string.IsNullOrWhiteSpace(e.Imagen) ? (object)DBNull.Value : e.Imagen);
                    cmd.Parameters.AddWithValue("@id", e.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ------------------------------------------------------------
        // BAJA LÓGICA (Activo = 0)
        // ------------------------------------------------------------
        public void Eliminar(int id)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                var query = "UPDATE dbo.Ejercicios SET Activo = 0 WHERE id_ejercicio = @id;";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ------------------------------------------------------------
        // OBTENER UNO POR ID (para editar/ver)
        // ------------------------------------------------------------
        public Ejercicio ObtenerPorId(int id)
        {
            Ejercicio ejercicio = null;

            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                var query = @"
                    SELECT
                        e.id_ejercicio       AS Id,
                        e.nombre             AS Nombre,
                        e.id_grupo_muscular  AS GrupoMuscularId,
                        gm.nombre            AS GrupoMuscularNombre,
                        e.creadoPor          AS CreadoPor,
                        e.imagen             AS Imagen
                    FROM dbo.Ejercicios e
                    LEFT JOIN dbo.Grupo_Muscular gm 
                           ON gm.id_grupo_muscular = e.id_grupo_muscular
                    WHERE e.id_ejercicio = @id AND e.Activo = 1;";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);

                    using (var rd = cmd.ExecuteReader())
                    {
                        if (rd.Read())
                        {
                            ejercicio = new Ejercicio
                            {
                                Id = rd.GetInt32(rd.GetOrdinal("Id")),
                                Nombre = rd.GetString(rd.GetOrdinal("Nombre")),
                                GrupoMuscularId = rd.GetInt32(rd.GetOrdinal("GrupoMuscularId")),
                                GrupoMuscularNombre = rd.IsDBNull(rd.GetOrdinal("GrupoMuscularNombre"))
                                                    ? string.Empty
                                                    : rd.GetString(rd.GetOrdinal("GrupoMuscularNombre")),
                                CreadoPor = rd.GetInt32(rd.GetOrdinal("CreadoPor")),
                                Imagen = rd.IsDBNull(rd.GetOrdinal("Imagen"))
                                                    ? null
                                                    : rd.GetString(rd.GetOrdinal("Imagen"))
                            };
                        }
                    }
                }
            }

            return ejercicio;
        }

        // Dentro de la clase EjercicioController

        public Ejercicio ObtenerPorNombre(string nombreEjercicio)
        {
            Ejercicio ejercicio = null;
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();
                string query = @"
            SELECT e.id_ejercicio, e.nombre, e.id_grupo_muscular, g.nombre AS grupo_muscular, e.creadoPor, e.imagen
            FROM Ejercicios e
            INNER JOIN Grupo_Muscular g ON e.id_grupo_muscular = g.id_grupo_muscular
            WHERE e.nombre = @nombre AND e.Activo = 1;"; // Busca por nombre

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombreEjercicio);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // Solo toma el primero si hay duplicados (debería ser único)
                        {
                            ejercicio = new Ejercicio
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("id_ejercicio")),
                                Nombre = reader.GetString(reader.GetOrdinal("nombre")),
                                GrupoMuscularId = reader.GetInt32(reader.GetOrdinal("id_grupo_muscular")),
                                GrupoMuscularNombre = reader.GetString(reader.GetOrdinal("grupo_muscular")),
                                CreadoPor = reader.GetInt32(reader.GetOrdinal("creadoPor")),
                                Imagen = reader.IsDBNull(reader.GetOrdinal("imagen")) ? string.Empty : reader.GetString(reader.GetOrdinal("imagen"))
                            };
                        }
                    }
                }
            }
            return ejercicio; // Devuelve null si no lo encuentra
        }

        // ------------------------------------------------------------
        // LISTAR POR NOMBRE DE GRUPO (para combos/filtros)
        // ------------------------------------------------------------
        public List<Ejercicio> ObtenerPorGrupoMuscular(string grupoMuscularNombre)
        {
            var lista = new List<Ejercicio>();

            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                var query = @"
                    SELECT
                        e.id_ejercicio       AS Id,
                        e.nombre             AS Nombre,
                        e.id_grupo_muscular  AS GrupoMuscularId,
                        gm.nombre            AS GrupoMuscularNombre,
                        e.creadoPor          AS CreadoPor,
                        e.imagen             AS Imagen
                    FROM dbo.Ejercicios e
                    LEFT JOIN dbo.Grupo_Muscular gm 
                           ON gm.id_grupo_muscular = e.id_grupo_muscular
                    WHERE e.Activo = 1
                      AND gm.nombre = @grupo
                    ORDER BY e.nombre;";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@grupo", grupoMuscularNombre);

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            var ej = new Ejercicio
                            {
                                Id = rd.GetInt32(rd.GetOrdinal("Id")),
                                Nombre = rd.GetString(rd.GetOrdinal("Nombre")),
                                GrupoMuscularId = rd.GetInt32(rd.GetOrdinal("GrupoMuscularId")),
                                GrupoMuscularNombre = rd.IsDBNull(rd.GetOrdinal("GrupoMuscularNombre"))
                                                    ? string.Empty
                                                    : rd.GetString(rd.GetOrdinal("GrupoMuscularNombre")),
                                CreadoPor = rd.GetInt32(rd.GetOrdinal("CreadoPor")),
                                Imagen = rd.IsDBNull(rd.GetOrdinal("Imagen"))
                                                    ? null
                                                    : rd.GetString(rd.GetOrdinal("Imagen"))
                            };
                            lista.Add(ej);
                        }
                    }
                }
            }

            return lista;
        }

        // ------------------------------------------------------------
        // REACTIVAR EJERCICIO (Activo = 1)
        // ------------------------------------------------------------
        public void Reactivar(int id)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();
                var query = "UPDATE dbo.Ejercicios SET Activo = 1 WHERE id_ejercicio = @id;";
                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ------------------------------------------------------------
        // OBTENER TODOS (con filtro opcional por estado)
        // ------------------------------------------------------------
        public List<Ejercicio> ObtenerTodos(bool? soloActivos = null)
        {
            var lista = new List<Ejercicio>();

            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                var query = @"
            SELECT
                e.id_ejercicio       AS Id,
                e.nombre             AS Nombre,
                e.id_grupo_muscular  AS GrupoMuscularId,
                gm.nombre            AS GrupoMuscularNombre,
                e.creadoPor          AS CreadoPor,
                e.imagen             AS Imagen,
                e.Activo             AS Activo
            FROM dbo.Ejercicios e
            LEFT JOIN dbo.Grupo_Muscular gm 
                   ON gm.id_grupo_muscular = e.id_grupo_muscular
            WHERE (@estado IS NULL OR e.Activo = @estado)
            ORDER BY e.id_ejercicio;";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@estado", (object)soloActivos ?? DBNull.Value);

                    using (var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            var ej = new Ejercicio
                            {
                                Id = rd.GetInt32(rd.GetOrdinal("Id")),
                                Nombre = rd.GetString(rd.GetOrdinal("Nombre")),
                                GrupoMuscularId = rd.GetInt32(rd.GetOrdinal("GrupoMuscularId")),
                                GrupoMuscularNombre = rd.IsDBNull(rd.GetOrdinal("GrupoMuscularNombre")) ? string.Empty : rd.GetString(rd.GetOrdinal("GrupoMuscularNombre")),
                                CreadoPor = rd.GetInt32(rd.GetOrdinal("CreadoPor")),
                                Imagen = rd.IsDBNull(rd.GetOrdinal("Imagen")) ? null : rd.GetString(rd.GetOrdinal("Imagen")),
                                Activo = rd.GetBoolean(rd.GetOrdinal("Activo"))
                            };
                            lista.Add(ej);
                        }
                    }
                }
            }

            return lista;
        }




    }
}
