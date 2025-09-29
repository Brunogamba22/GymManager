// Referencias a ADO.NET y a tus modelos/utilidades
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using GymManager.Models;
using GymManager.Utils;

namespace GymManager.Controllers
{
    public class UsuarioController
    {
        // ------------------------------------------------------------
        // LEE TODOS LOS USUARIOS ACTIVOS
        // ------------------------------------------------------------
        public List<Usuario> ObtenerTodos()
        {
            // Lista de salida
            List<Usuario> lista = new List<Usuario>();

            // using = cierra la conexión aunque haya excepción
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open(); // abre la conexión

                // Consulta: trae sólo Activo = 1 (baja lógica aplicada)
                string query = @"
                    SELECT u.dni, u.nombre, u.apellido, u.email, 
                           u.password, r.tipo_rol
                    FROM dbo.Usuarios u
                    INNER JOIN dbo.Roles r ON u.id_rol = r.id_rol
                    WHERE u.Activo = 1";  // 👈 filtro de “sólo activos”

                // Ejecuta el SELECT y recorre el cursor
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        // Mapea cada fila del datareader a tu modelo Usuario
                        Usuario u = new Usuario
                        {
                            Id = reader["dni"].ToString(),
                            Nombre = reader["nombre"].ToString(),
                            Apellido = reader["apellido"].ToString(),
                            Email = reader["email"].ToString(),
                            Password = reader["password"].ToString(), // hash almacenado
                            // Convierte tipo_rol (string) a enum Rol, sin importar mayúsculas
                            Rol = (Rol)Enum.Parse(typeof(Rol), reader["tipo_rol"].ToString(), true)
                        };

                        lista.Add(u); // agrega a la lista de salida
                    }
                }
            }

            return lista; // devuelve sólo usuarios activos
        }

        // ------------------------------------------------------------
        // ALTA DE USUARIO (siempre lo da de alta con Activo = 1)
        // ------------------------------------------------------------
        public void Agregar(Usuario u)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // 1) Validación de unicidad por DNI o Email
                string checkQuery = "SELECT COUNT(*) FROM dbo.Usuarios WHERE dni = @Dni OR email = @Email";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    // Parámetros para evitar SQL Injection
                    checkCmd.Parameters.AddWithValue("@Dni", u.Id);
                    checkCmd.Parameters.AddWithValue("@Email", u.Email);

                    int count = (int)checkCmd.ExecuteScalar(); // devuelve 0..n
                    if (count > 0)
                    {
                        // Señaliza a la UI que hay duplicados
                        throw new InvalidOperationException("El usuario ya existe con ese DNI o Email.");
                    }
                }

                // 2) Inserta el usuario. Activo queda en 1 explícitamente.
                string query = @"
                    INSERT INTO dbo.Usuarios (dni, nombre, apellido, email, password, id_rol, Activo)
                    VALUES (@Dni, @Nombre, @Apellido, @Email, @Password, @IdRol, 1)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Dni", u.Id);
                    cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", u.Apellido);
                    cmd.Parameters.AddWithValue("@Email", u.Email);

                    // Hashea la contraseña antes de guardar
                    cmd.Parameters.AddWithValue("@Password", PasswordHelper.HashPassword(u.Password));

                    // En BD: 1=Admin, 2=Profesor, 3=Recepcionista
                    // En enum: 0=Admin, 1=Profesor, 2=Recepcionista -> por eso +1
                    cmd.Parameters.AddWithValue("@IdRol", (int)u.Rol + 1);

                    cmd.ExecuteNonQuery(); // ejecuta el INSERT
                }
            }
        }

        // ------------------------------------------------------------
        // MODIFICACIÓN DE USUARIO (sólo si está Activo)
        // ------------------------------------------------------------
        public void Editar(Usuario u)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // Actualiza datos del usuario. Protegemos que esté activo.
                string query = @"
                    UPDATE dbo.Usuarios 
                    SET nombre = @Nombre, apellido = @Apellido, email = @Email, 
                        password = @Password, id_rol = @IdRol
                    WHERE dni = @Dni AND Activo = 1"; // 👈 no edita si está inactivo

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Dni", u.Id);
                    cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", u.Apellido);
                    cmd.Parameters.AddWithValue("@Email", u.Email);

                    // Re-hash por si cambió la contraseña
                    cmd.Parameters.AddWithValue("@Password", PasswordHelper.HashPassword(u.Password));
                    cmd.Parameters.AddWithValue("@IdRol", (int)u.Rol + 1);

                    cmd.ExecuteNonQuery(); // ejecuta el UPDATE
                }
            }
        }

        // ------------------------------------------------------------
        // BAJA LÓGICA (no borra, deja Activo = 0)
        // Protege que no se desactive el último Administrador.
        // ------------------------------------------------------------
        public void Eliminar(string dni)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // Usamos transacción por si algo falla (todo o nada)
                using (var tx = conn.BeginTransaction())
                {
                    // Obtiene el rol del usuario, sólo si está activo
                    var getRol = new SqlCommand(
                        "SELECT id_rol FROM dbo.Usuarios WHERE dni=@dni AND Activo=1", conn, tx);
                    getRol.Parameters.AddWithValue("@dni", dni);
                    var rolObj = getRol.ExecuteScalar();

                    // Si no existe o ya está inactivo, avisamos
                    if (rolObj == null)
                        throw new InvalidOperationException("El usuario no existe o ya está inactivo.");

                    int idRol = Convert.ToInt32(rolObj);

                    // Si es Admin (id_rol = 1), verificamos no dejar 0 admins activos
                    if (idRol == 1)
                    {
                        var countAdmins = new SqlCommand(
                            "SELECT COUNT(*) FROM dbo.Usuarios WHERE id_rol = 1 AND Activo=1", conn, tx);
                        int admins = (int)countAdmins.ExecuteScalar();
                        if (admins <= 1)
                            throw new InvalidOperationException("No se puede desactivar el último Administrador.");
                    }

                    // Baja lógica: Activo = 0 (NO se borra el registro)
                    var upd = new SqlCommand(
                        "UPDATE dbo.Usuarios SET Activo = 0 WHERE dni=@dni", conn, tx);
                    upd.Parameters.AddWithValue("@dni", dni);
                    upd.ExecuteNonQuery();

                    tx.Commit(); // confirma la transacción
                }
            }
        }

        // ------------------------------------------------------------
        // LOGIN: sólo permite ingresar si el usuario está Activo
        // ------------------------------------------------------------
        public Usuario Login(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // Busca por email y además exige Activo = 1
                string query = @"
                    SELECT u.dni, u.nombre, u.apellido, u.email, 
                           u.password, r.tipo_rol
                    FROM dbo.Usuarios u
                    INNER JOIN dbo.Roles r ON u.id_rol = r.id_rol
                    WHERE u.email = @Email AND u.Activo = 1"; // 👈 filtro de activo

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read()) // encontró el email y está activo
                        {
                            string storedHash = reader["password"].ToString();

                            // Compara password ingresado vs. hash en BD
                            if (PasswordHelper.VerifyPassword(password, storedHash))
                            {
                                // Construye el objeto Usuario que devolverá
                                return new Usuario
                                {
                                    Id = reader["dni"].ToString(),
                                    Nombre = reader["nombre"].ToString(),
                                    Apellido = reader["apellido"].ToString(),
                                    Email = reader["email"].ToString(),
                                    Password = storedHash, // queda el hash
                                    Rol = (Rol)Enum.Parse(typeof(Rol), reader["tipo_rol"].ToString(), true)
                                };
                            }
                        }
                    }
                }
            }

            // Si no pasa validaciones, devolvemos null (login inválido)
            return null;
        }

        // ------------------------------------------------------------
        // EXISTE USUARIO (sólo cuenta activos)
        // ------------------------------------------------------------
        public bool ExisteUsuario(string dni)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // Cuenta usuarios con ese DNI que estén activos
                string query = "SELECT COUNT(*) FROM dbo.Usuarios WHERE dni = @Dni AND Activo = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Dni", dni);
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0; // true si existe
                }
            }
        }
    }
}
