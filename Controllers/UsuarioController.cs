using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GymManager.Models;
using GymManager.Utils;

namespace GymManager.Controllers
{
    public class UsuarioController
    {
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
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Usuario u = new Usuario
                        {
                            Id = reader["dni"].ToString(),
                            Nombre = reader["nombre"].ToString(),
                            Apellido = reader["apellido"].ToString(),
                            Email = reader["email"].ToString(),
                            Password = reader["password"].ToString(), // ⚠️ guardado como hash
                            Rol = (Rol)Enum.Parse(typeof(Rol), reader["tipo_rol"].ToString(), true)
                        };

                        lista.Add(u);
                    }
                }
            }

            return lista;
        }

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

                    // Guardamos la contraseña hasheada
                    cmd.Parameters.AddWithValue("@Password", PasswordHelper.HashPassword(u.Password));

                    cmd.Parameters.AddWithValue("@IdRol", (int)u.Rol + 1);
                    cmd.ExecuteNonQuery();
                }
            }
        }

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

                    // Re-hasheamos por si cambió la contraseña
                    cmd.Parameters.AddWithValue("@Password", PasswordHelper.HashPassword(u.Password));

                    cmd.Parameters.AddWithValue("@IdRol", (int)u.Rol + 1);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void Eliminar(string dni)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();
                using (var tx = conn.BeginTransaction())
                {
                    var getRol = new SqlCommand(
                        "SELECT id_rol FROM dbo.Usuarios WHERE dni=@dni", conn, tx);
                    getRol.Parameters.AddWithValue("@dni", dni);
                    var rolObj = getRol.ExecuteScalar();

                    if (rolObj == null)
                        throw new InvalidOperationException("El usuario no existe.");

                    int idRol = Convert.ToInt32(rolObj);

                    if (idRol == 1)
                    {
                        var countAdmins = new SqlCommand(
                            "SELECT COUNT(*) FROM dbo.Usuarios WHERE id_rol = 1", conn, tx);
                        int admins = (int)countAdmins.ExecuteScalar();
                        if (admins <= 1)
                            throw new InvalidOperationException("No se puede eliminar el último Administrador.");
                    }

                    var del = new SqlCommand(
                        "DELETE FROM dbo.Usuarios WHERE dni=@dni", conn, tx);
                    del.Parameters.AddWithValue("@dni", dni);
                    del.ExecuteNonQuery();

                    tx.Commit();
                }
            }
        }

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
                    WHERE u.email = @Email";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedHash = reader["password"].ToString();

                            // Verificamos contraseña contra el hash guardado
                            if (PasswordHelper.VerifyPassword(password, storedHash))
                            {
                                return new Usuario
                                {
                                    Id = reader["dni"].ToString(),
                                    Nombre = reader["nombre"].ToString(),
                                    Apellido = reader["apellido"].ToString(),
                                    Email = reader["email"].ToString(),
                                    Password = storedHash,
                                    Rol = (Rol)Enum.Parse(typeof(Rol), reader["tipo_rol"].ToString(), true)
                                };
                            }
                        }
                    }
                }
            }



            return null;
        }

        public bool ExisteUsuario(string dni)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();
                string query = "SELECT COUNT(*) FROM dbo.Usuarios WHERE dni = @Dni";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Dni", dni);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

    }
}
