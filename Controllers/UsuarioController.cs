using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GymManager.Models;
using GymManager.Utils;

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

            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = @"
                    SELECT u.dni, u.nombre, u.apellido, u.email, 
                           u.password, r.tipo_rol
                    FROM dbo.Usuarios u
                    INNER JOIN dbo.Roles r ON u.id_rol = r.id_rol";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            Usuario u = new Usuario
                            {
                                Id = reader["dni"].ToString(),   // dni como string
                                Nombre = reader["nombre"].ToString(),
                                Email = reader["email"].ToString(),
                                Password = reader["password"].ToString(),
                                Rol = (Rol)Enum.Parse(typeof(Rol), reader["tipo_rol"].ToString(), true)
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

                string query = @"
                    INSERT INTO dbo.Usuarios (dni, nombre, apellido, email, password, id_rol)
                    VALUES (@Dni, @Nombre, @Apellido, @Email, @Password, @IdRol)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Dni", u.Id);
                    cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", u.Apellido);
                    cmd.Parameters.AddWithValue("@Email", u.Email);
                    cmd.Parameters.AddWithValue("@Password", u.Password);
                    cmd.Parameters.AddWithValue("@IdRol", (int)u.Rol + 1);
                    // +1 porque en la DB los id_rol arrancan en 1

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

                string query = @"
                    UPDATE dbo.Usuarios 
                    SET nombre = @Nombre, apellido = @Apellido, email = @Email, 
                        password = @Password, id_rol = @IdRol
                    WHERE dni = @Dni";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Dni", u.Id);
                    cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", u.Apellido);
                    cmd.Parameters.AddWithValue("@Email", u.Email);
                    cmd.Parameters.AddWithValue("@Password", u.Password);
                    cmd.Parameters.AddWithValue("@IdRol", (int)u.Rol + 1);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Elimina un usuario de la base de datos por DNI.
        /// </summary>
        public void Eliminar(string dni)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = "DELETE FROM dbo.Usuarios WHERE dni = @Dni";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Dni", dni);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        /// <summary>
        /// Busca un usuario por email y contraseña para login.
        /// </summary>
        public Usuario Login(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = @"
                    SELECT u.dni, u.nombre, u.apellido, u.email, 
                           u.password, r.tipo_rol
                    FROM dbo.Usuarios u
                    INNER JOIN dbo.Roles r ON u.id_rol = r.id_rol
                    WHERE u.email = @Email AND u.password = @Password";

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
                                Id = reader["dni"].ToString(),
                                Nombre = reader["nombre"].ToString(),
                                Email = reader["email"].ToString(),
                                Password = reader["password"].ToString(),
                                Rol = (Rol)Enum.Parse(typeof(Rol), reader["tipo_rol"].ToString(), true)
                            };
                        }
                    }
                }
            }

            return null; // si no se encontró
        }
    }
}
