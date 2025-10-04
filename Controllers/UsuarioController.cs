// ------------------------------------------------------------
// Espacios de nombres necesarios
// ------------------------------------------------------------
using System;                         // Funcionalidades base (excepciones, tipos genéricos, etc.)
using System.Collections.Generic;     // Permite usar List<T>
using System.Data.SqlClient;          // Librería ADO.NET para conexión con SQL Server
using GymManager.Models;              // Referencia a las clases del modelo (Usuario, Rol, etc.)
using GymManager.Utils;               // Referencia a utilidades (ej: Conexion.Cadena, PasswordHelper)

namespace GymManager.Controllers
{
    // ------------------------------------------------------------
    // Clase controladora para gestionar operaciones sobre "Usuarios"
    // ------------------------------------------------------------
    public class UsuarioController
    {
        // ------------------------------------------------------------
        // MÉTODO: ObtenerTodos()
        // Retorna todos los usuarios activos del sistema
        // ------------------------------------------------------------
        public List<Usuario> ObtenerTodos()
        {
            // Crea una lista vacía donde se almacenarán los usuarios leídos desde la BD
            var lista = new List<Usuario>();

            // Abre una conexión con la base de datos
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open(); // Inicia la conexión

                // Consulta SQL: trae los usuarios activos y el nombre del rol asociado
                string query = @"
                    SELECT 
                        u.id_usuario,
                        u.dni,
                        u.nombre,
                        u.apellido,
                        u.email,
                        u.password,
                        r.tipo_rol
                    FROM dbo.Usuarios u
                    INNER JOIN dbo.Roles r ON u.id_rol = r.id_rol
                    WHERE u.Activo = 1
                    ORDER BY u.apellido, u.nombre;";

                // Prepara el comando SQL a ejecutar
                using (SqlCommand cmd = new SqlCommand(query, conn))
                // Ejecuta el comando y obtiene un "cursor" de lectura (DataReader)
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Recorre cada registro devuelto por la consulta
                    while (reader.Read())
                    {
                        // Mapea cada fila de la BD a un objeto "Usuario"
                        var u = new Usuario
                        {
                            IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                            Dni = reader["dni"].ToString(),
                            Nombre = reader["nombre"].ToString(),
                            Apellido = reader["apellido"].ToString(),
                            Email = reader["email"].ToString(),
                            Password = reader["password"].ToString(),
                            // Convierte el nombre del rol (string) en el Enum correspondiente
                            Rol = (Rol)Enum.Parse(typeof(Rol), reader["tipo_rol"].ToString(), true)
                        };

                        // Agrega el usuario leído a la lista
                        lista.Add(u);
                    }
                }
            }

            // Retorna la lista completa de usuarios activos
            return lista;
        }

        // ------------------------------------------------------------
        // MÉTODO: Agregar(Usuario u)
        // Inserta un nuevo usuario en la base de datos
        // ------------------------------------------------------------
        public void Agregar(Usuario u)
        {
            // Abre la conexión con la base de datos
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // Primero verifica que no exista otro usuario con el mismo DNI o Email
                string checkQuery = "SELECT COUNT(*) FROM dbo.Usuarios WHERE dni = @Dni OR email = @Email";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    // Usa parámetros para evitar SQL Injection
                    checkCmd.Parameters.AddWithValue("@Dni", u.Dni);
                    checkCmd.Parameters.AddWithValue("@Email", u.Email);

                    // Ejecuta la consulta y obtiene la cantidad de coincidencias
                    int count = (int)checkCmd.ExecuteScalar();

                    // Si ya existe, lanza una excepción controlada
                    if (count > 0)
                        throw new InvalidOperationException("Ya existe un usuario con ese DNI o Email.");
                }

                // Si no existe, procede a insertar el nuevo registro
                string query = @"
                    INSERT INTO dbo.Usuarios (dni, nombre, apellido, email, password, id_rol, Activo)
                    VALUES (@Dni, @Nombre, @Apellido, @Email, @Password, @IdRol, 1)";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Asigna los valores del modelo a los parámetros SQL
                    cmd.Parameters.AddWithValue("@Dni", u.Dni);
                    cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", u.Apellido);
                    cmd.Parameters.AddWithValue("@Email", u.Email);

                    // Hashea la contraseña antes de guardarla (nunca se almacena en texto plano)
                    cmd.Parameters.AddWithValue("@Password", PasswordHelper.HashPassword(u.Password));

                    // Convierte el valor del Enum Rol al ID numérico correspondiente
                    cmd.Parameters.AddWithValue("@IdRol", (int)u.Rol + 1);

                    // Ejecuta el comando INSERT
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ------------------------------------------------------------
        // MÉTODO: Editar(Usuario u)
        // Modifica los datos de un usuario activo
        // ------------------------------------------------------------
        public void Editar(Usuario u)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // Consulta SQL para actualizar los campos del usuario
                string query = @"
                    UPDATE dbo.Usuarios
                    SET nombre = @Nombre,
                        apellido = @Apellido,
                        email = @Email,
                        password = @Password,
                        id_rol = @IdRol
                    WHERE id_usuario = @IdUsuario AND Activo = 1;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Asigna los valores del modelo a los parámetros
                    cmd.Parameters.AddWithValue("@IdUsuario", u.IdUsuario);
                    cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", u.Apellido);
                    cmd.Parameters.AddWithValue("@Email", u.Email);
                    cmd.Parameters.AddWithValue("@Password", PasswordHelper.HashPassword(u.Password));
                    cmd.Parameters.AddWithValue("@IdRol", (int)u.Rol + 1);

                    // Ejecuta el comando UPDATE
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ------------------------------------------------------------
        // MÉTODO: Eliminar(int idUsuario)
        // Desactiva un usuario (baja lógica, no elimina físicamente)
        // ------------------------------------------------------------
        public void Eliminar(string dni)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                using (var tx = conn.BeginTransaction())
                {
                    var getRol = new SqlCommand(
                        "SELECT id_rol FROM dbo.Usuarios WHERE dni=@dni AND Activo=1", conn, tx);
                    getRol.Parameters.AddWithValue("@dni", dni);
                    var rolObj = getRol.ExecuteScalar();

                    if (rolObj == null)
                        throw new InvalidOperationException("El usuario no existe o ya está inactivo.");

                    int idRol = Convert.ToInt32(rolObj);

                    if (idRol == 1)
                    {
                        var countAdmins = new SqlCommand(
                            "SELECT COUNT(*) FROM dbo.Usuarios WHERE id_rol = 1 AND Activo=1", conn, tx);
                        int admins = (int)countAdmins.ExecuteScalar();
                        if (admins <= 1)
                            throw new InvalidOperationException("No se puede desactivar el último Administrador.");
                    }

                    var upd = new SqlCommand(
                        "UPDATE dbo.Usuarios SET Activo = 0 WHERE dni=@dni", conn, tx);
                    upd.Parameters.AddWithValue("@dni", dni);
                    upd.ExecuteNonQuery();

                    tx.Commit();
                }
            }
        }


        // ------------------------------------------------------------
        // MÉTODO: Login()
        // Verifica las credenciales del usuario y retorna su objeto si es válido
        // ------------------------------------------------------------
        public Usuario Login(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // Consulta SQL: busca por email y valida que esté activo
                string query = @"
                    SELECT 
                        u.id_usuario,
                        u.dni,
                        u.nombre,
                        u.apellido,
                        u.email,
                        u.password,
                        r.tipo_rol
                    FROM dbo.Usuarios u
                    INNER JOIN dbo.Roles r ON u.id_rol = r.id_rol
                    WHERE u.email = @Email AND u.Activo = 1;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Si encuentra el usuario, verifica la contraseña
                        if (reader.Read())
                        {
                            string storedHash = reader["password"].ToString();

                            // Compara el password ingresado con el hash almacenado
                            if (PasswordHelper.VerifyPassword(password, storedHash))
                            {
                                // Si es correcto, retorna el objeto Usuario completo
                                return new Usuario
                                {
                                    IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                                    Dni = reader["dni"].ToString(),
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

            // Si no pasó las validaciones, retorna null (login fallido)
            return null;
        }

        // ------------------------------------------------------------
        // MÉTODO: ExisteUsuario()
        // Verifica si un usuario con determinado DNI ya existe (y está activo)
        // ------------------------------------------------------------
        public bool ExisteUsuario(string dni)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = "SELECT COUNT(*) FROM dbo.Usuarios WHERE dni = @Dni AND Activo = 1";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@Dni", dni);

                    // Devuelve true si encontró coincidencias
                    int count = (int)cmd.ExecuteScalar();
                    return count > 0;
                }
            }
        }

        // ------------------------------------------------------------
        // MÉTODO: ObtenerPorId()
        // Devuelve los datos de un usuario específico (por ID)
        // ------------------------------------------------------------
        public Usuario ObtenerPorId(int idUsuario)
        {
            Usuario u = null;

            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // Consulta SQL: busca usuario por ID
                string query = @"
                    SELECT 
                        u.id_usuario,
                        u.dni,
                        u.nombre,
                        u.apellido,
                        u.email,
                        u.password,
                        r.tipo_rol
                    FROM dbo.Usuarios u
                    INNER JOIN dbo.Roles r ON u.id_rol = r.id_rol
                    WHERE u.id_usuario = @id AND u.Activo = 1;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idUsuario);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Mapea los datos del registro al objeto Usuario
                            u = new Usuario
                            {
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                                Dni = reader["dni"].ToString(),
                                Nombre = reader["nombre"].ToString(),
                                Apellido = reader["apellido"].ToString(),
                                Email = reader["email"].ToString(),
                                Password = reader["password"].ToString(),
                                Rol = (Rol)Enum.Parse(typeof(Rol), reader["tipo_rol"].ToString(), true)
                            };
                        }
                    }
                }
            }

            // Devuelve el usuario (o null si no existe)
            return u;
        }
    }
}
