using System;
using System.Collections.Generic;
using System.Data;
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

            // Creamos la conexión SQL usando la cadena configurada en App.config
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open(); // Abrimos la conexión

                // Consulta SQL para traer todos los ejercicios
                string query = "SELECT * FROM Ejercicios";

                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader()) // Ejecutamos y leemos los resultados
                {
                    while (reader.Read())
                    {
                        var ejercicio = new Ejercicio
                        {
                            Id = reader.GetInt32(0),                 // Columna 0: Id
                            Nombre = reader.GetString(1),            // Columna 1: Nombre
                            Musculo = reader.GetString(2),           // Columna 2: Musculo
                            Descripcion = reader.GetString(3)        // Columna 3: Descripcion
                        };

                        lista.Add(ejercicio); // Lo agregamos a la lista
                    }
                }
            }

            return lista; // Devolvemos la lista completa
        }

        // Método para agregar un nuevo ejercicio
        public void Agregar(Ejercicio e)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // Consulta con parámetros para evitar SQL Injection
                string query = "INSERT INTO Ejercicios (Nombre, Musculo, Descripcion) VALUES (@n, @m, @d)";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@n", e.Nombre);
                    cmd.Parameters.AddWithValue("@m", e.Musculo);
                    cmd.Parameters.AddWithValue("@d", e.Descripcion);

                    cmd.ExecuteNonQuery(); // Ejecutamos sin esperar resultados
                }
            }
        }

        // Método para editar un ejercicio existente
        public void Editar(Ejercicio e)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = "UPDATE Ejercicios SET Nombre = @n, Musculo = @m, Descripcion = @d WHERE Id = @id";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@n", e.Nombre);
                    cmd.Parameters.AddWithValue("@m", e.Musculo);
                    cmd.Parameters.AddWithValue("@d", e.Descripcion);
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

                string query = "DELETE FROM Ejercicios WHERE Id = @id";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
