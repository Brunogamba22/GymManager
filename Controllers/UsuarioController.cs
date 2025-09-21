using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using GymManager.Models;
using GymManager.Utils; // Usamos Conexion.cs para obtener la cadena de conexión

namespace GymManager.Controllers
{
    /// <summary>
    /// Controlador que maneja el acceso a la tabla Usuarios desde la base de datos.
    /// </summary>
    public class UsuarioController
    {
        /// <summary>
        /// Devuelve una lista con todos los usuarios de la base de datos.
        /// </summary>
        public List<Usuario> ObtenerTodos()
        {
            List<Usuario> lista = new List<Usuario>();

            // Usamos un using para manejar automáticamente la conexión
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = "SELECT * FROM Usuarios";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Leemos cada fila y la convertimos en objeto Usuario
                        while (reader.Read())
                        {
                            Usuario u = new Usuario
                            {
                                Id = (int)reader["Id"],
                                Nombre = reader["Nombre"].ToString(),
                                Email = reader["Email"].ToString(),
                                Password = reader["Password"].ToString(),
                                Rol = (Rol)Enum.Parse(typeof(Rol), reader["Rol"].ToString())

                            };

                            lista.Add(u);
                        }
                    }
                }
            }

            return lista;
        }

        /// <summary>
        /// Inserta un nuevo usuario en la base de datos.
        /// </summary>
        public void Agregar(Usuario u)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = "INSERT INTO Usuarios (Nombre, Email, Password, Rol) VALUES (@Nombre, @Email, @Password, @Rol)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
                    cmd.Parameters.AddWithValue("@Email", u.Email);
                    cmd.Parameters.AddWithValue("@Password", u.Password);
                    cmd.Parameters.AddWithValue("@Rol", u.Rol.ToString());

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Actualiza los datos de un usuario existente.
        /// </summary>
        public void Editar(Usuario u)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = "UPDATE Usuarios SET Nombre = @Nombre, Email = @Email, Password = @Password, Rol = @Rol WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
                    cmd.Parameters.AddWithValue("@Email", u.Email);
                    cmd.Parameters.AddWithValue("@Password", u.Password);
                    cmd.Parameters.AddWithValue("@Rol", u.Rol.ToString());
                    cmd.Parameters.AddWithValue("@Id", u.Id);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Elimina un usuario de la base de datos por ID.
        /// </summary>
        public void Eliminar(int id)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = "DELETE FROM Usuarios WHERE Id = @Id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Busca un usuario por email y contraseña para login.
        /// </summary>
        public Usuario? Login(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            { 
                conn.Open();

                string query = "SELECT * FROM Usuarios WHERE Email = @Email AND Password = @Password";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@Password", password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                Id = (int)reader["Id"],
                                Nombre = reader["Nombre"].ToString(),
                                Email = reader["Email"].ToString(),
                                Password = reader["Password"].ToString(),
                                Rol = (Rol)Enum.Parse(typeof(Rol), reader["Rol"].ToString())

                            };
                        }
                    }
                }
            }

            return null; // Si no se encontró el usuario
        }
    }
}
