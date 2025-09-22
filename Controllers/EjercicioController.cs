using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GymManager.Models;
using GymManager.Utils;

namespace GymManager.Controllers
{
    public class EjercicioController
    {
        // Método que obtiene todos los ejercicios desde la base de datos
        public List<Ejercicio> ObtenerTodos()
        {
            var lista = new List<Ejercicio>();

            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // Nombres de columnas exactos según la tabla SQL
                string query = "SELECT id_ejercicio, nombre, musculo, descripcion FROM Ejercicios";

                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var ejercicio = new Ejercicio
                        {
                            Id = reader.GetInt32(0),           // id_ejercicio
                            Nombre = reader.GetString(1),      // nombre
                            Musculo = reader.GetString(2),     // musculo
                            Descripcion = reader.IsDBNull(3) ? "" : reader.GetString(3) // descripcion (puede ser NULL)
                        };

                        lista.Add(ejercicio);
                    }
                }
            }

            return lista;
        }

        // Método para agregar un nuevo ejercicio
        public void Agregar(Ejercicio e)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = "INSERT INTO Ejercicios (nombre, musculo, descripcion) VALUES (@n, @m, @d)";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@n", e.Nombre);
                    cmd.Parameters.AddWithValue("@m", e.Musculo);
                    cmd.Parameters.AddWithValue("@d", e.Descripcion ?? (object)DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        // Método para editar un ejercicio existente
        public void Editar(Ejercicio e)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = "UPDATE Ejercicios SET nombre = @n, musculo = @m, descripcion = @d WHERE id_ejercicio = @id";

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

        // Método para eliminar un ejercicio por su ID
        public void Eliminar(int id)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = "DELETE FROM Ejercicios WHERE id_ejercicio = @id";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
