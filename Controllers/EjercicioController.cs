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
        // ------------------------------------------------------------
        public List<Ejercicio> ObtenerTodos()
        {
            var lista = new List<Ejercicio>();

            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // Trae solo los ejercicios activos
                string query = @"
                    SELECT id_ejercicio, nombre, musculo, descripcion
                    FROM Ejercicios
                    WHERE Activo = 1"; // 👈 filtro de baja lógica

                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ejercicio = new Ejercicio
                        {
                            Id = reader.GetInt32(0),              // id_ejercicio
                            Nombre = reader.GetString(1),             // nombre
                            Musculo = reader.GetString(2),             // musculo
                            Descripcion = reader.IsDBNull(3) ? "" : reader.GetString(3) // puede venir NULL
                        };

                        lista.Add(ejercicio);
                    }
                }
            }

            return lista;
        }

        // ------------------------------------------------------------
        // ALTA DE EJERCICIO (siempre lo da de alta con Activo = 1)
        // ------------------------------------------------------------
        public void Agregar(Ejercicio e)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = @"
                    INSERT INTO Ejercicios (nombre, musculo, descripcion, Activo)
                    VALUES (@n, @m, @d, 1)"; // 👈 explícitamente activo

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@n", e.Nombre ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@m", string.IsNullOrEmpty(e.Musculo) ? (object)DBNull.Value : e.Musculo);
                    cmd.Parameters.AddWithValue("@d", string.IsNullOrEmpty(e.Descripcion) ? (object)DBNull.Value : e.Descripcion);

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
                    SET nombre = @n, musculo = @m, descripcion = @d
                    WHERE id_ejercicio = @id AND Activo = 1"; // 👈 no edita si está inactivo

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@n", e.Nombre);
                    cmd.Parameters.AddWithValue("@m", e.Musculo);
                    cmd.Parameters.AddWithValue("@d", e.Descripcion ?? (object)DBNull.Value);
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

                string query = "UPDATE Ejercicios SET Activo = 0 WHERE id_ejercicio = @id"; // 👈 baja lógica

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
