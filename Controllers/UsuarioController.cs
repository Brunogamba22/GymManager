// ------------------------------------------------------------
// NOMBRE DEL ARCHIVO: UsuarioController.cs
// PROPÓSITO: Gestionar las operaciones CRUD y login del módulo "Usuarios"
// AUTOR: Bruno Gamba (proyecto GymManager)
// ------------------------------------------------------------

// Espacios de nombres requeridos
using GymManager.Models;              // Contiene las clases del modelo (Usuario, Rol)
using GymManager.Utils;               // Contiene la clase Conexion y PasswordHelper
using System;                         // Funcionalidades básicas (excepciones, tipos, etc.)
using System.Collections.Generic;     // Permite el uso de List<T>
using System.Data.SqlClient;          // Librería ADO.NET para conexión con SQL Server
using System.Windows.Forms;

namespace GymManager.Controllers
{
    // ------------------------------------------------------------
    // CLASE: UsuarioController
    // ------------------------------------------------------------
    // Esta clase gestiona todas las operaciones sobre los usuarios:
    // listar, agregar, editar, eliminar (baja lógica) y login.
    // ------------------------------------------------------------
    public class UsuarioController
    {
        // ============================================================
        // MÉTODO: ObtenerTodos()
        // ------------------------------------------------------------
        // Retorna una lista con todos los usuarios activos de la base
        // de datos. Este método se usa para llenar la grilla del panel
        // de administración.
        // ============================================================
        public List<Usuario> ObtenerTodos()
        {
            // Lista donde se guardarán los usuarios leídos desde la BD
            var lista = new List<Usuario>();

            // Abrimos la conexión con SQL Server
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open(); // Inicia la conexión

                // Consulta SQL para obtener los datos principales
                string query = @"
                    SELECT 
                        u.id_usuario,
                        u.nombre,
                        u.apellido,
                        u.email,
                        u.password,
                        r.tipo_rol,
                        u.activo
                    FROM dbo.Usuarios u
                    INNER JOIN dbo.Roles r ON u.id_rol = r.id_rol
                    ORDER BY u.apellido, u.nombre;";

                // Ejecutamos el comando SQL
                using (SqlCommand cmd = new SqlCommand(query, conn))
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    // Recorremos cada registro devuelto
                    while (reader.Read())
                    {
                        // Creamos un nuevo objeto Usuario con los datos del registro
                        var u = new Usuario
                        {
                            IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
                            Nombre = reader["nombre"].ToString(),
                            Apellido = reader["apellido"].ToString(),
                            Email = reader["email"].ToString(),
                            Rol = (Rol)Enum.Parse(typeof(Rol), reader["tipo_rol"].ToString(), true),
                            Activo = Convert.ToBoolean(reader["activo"])

                        };

                        // Agregamos el usuario a la lista
                        lista.Add(u);
                    }
                }
            }

            // Retornamos la lista de usuarios activos
            return lista;
        }

        // ============================================================
        // MÉTODO: Agregar(Usuario u)
        // ------------------------------------------------------------
        // Inserta un nuevo usuario en la base de datos.
        // Antes de agregarlo, valida que el email no esté repetido.
        // ============================================================
        public void Agregar(Usuario u)
        {
            // Se abre una conexión a la base de datos
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open(); // Activa la conexión

                // Verificamos si ya existe un usuario con el mismo email
                string checkQuery = "SELECT COUNT(*) FROM dbo.Usuarios WHERE email = @Email";
                using (SqlCommand checkCmd = new SqlCommand(checkQuery, conn))
                {
                    checkCmd.Parameters.AddWithValue("@Email", u.Email); // Parámetro seguro (evita SQL Injection)
                    int count = (int)checkCmd.ExecuteScalar(); // Devuelve la cantidad de coincidencias

                    // Si ya existe un usuario con ese email, lanzamos excepción controlada
                    if (count > 0)
                        throw new InvalidOperationException("Ya existe un usuario con ese correo electrónico.");
                }

                // Si el email no está duplicado, procedemos a insertar el nuevo usuario
                string insertQuery = @"
                    INSERT INTO dbo.Usuarios (nombre, apellido, email, password, id_rol, Activo)
                    VALUES (@Nombre, @Apellido, @Email, @Password, @IdRol, 1);";

                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    // Cargamos los parámetros con los valores del objeto Usuario
                    cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", u.Apellido);
                    cmd.Parameters.AddWithValue("@Email", u.Email);

                    // Antes de guardar, se aplica un hash a la contraseña por seguridad
                    cmd.Parameters.AddWithValue("@Password", PasswordHelper.HashPassword(u.Password));

                    // El Enum Rol comienza en 0, mientras que en la BD los ID comienzan en 1
                    cmd.Parameters.AddWithValue("@IdRol", (int)u.Rol + 1);

                    // Ejecutamos el comando INSERT
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ============================================================
        // MÉTODO: Editar(Usuario u)
        // ------------------------------------------------------------
        // Actualiza los datos de un usuario existente.
        // Si el campo contraseña está vacío, no se modifica.
        // ============================================================
        public void Editar(Usuario u)
        {
            // Validamos que tenga un ID válido
            if (u.IdUsuario <= 0)
                throw new ArgumentException("ID de usuario inválido.");

            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open(); // Abre la conexión

                // Construimos dos versiones del UPDATE: una con password y otra sin password
                string query = string.IsNullOrWhiteSpace(u.Password)
                    ? @"UPDATE dbo.Usuarios
                        SET nombre = @Nombre,
                            apellido = @Apellido,
                            email = @Email,
                            id_rol = @IdRol
                        WHERE id_usuario = @IdUsuario AND Activo = 1;"
                    : @"UPDATE dbo.Usuarios
                        SET nombre = @Nombre,
                            apellido = @Apellido,
                            email = @Email,
                            password = @Password,
                            id_rol = @IdRol
                        WHERE id_usuario = @IdUsuario AND Activo = 1;";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Asignamos los parámetros comunes
                    cmd.Parameters.AddWithValue("@IdUsuario", u.IdUsuario);
                    cmd.Parameters.AddWithValue("@Nombre", u.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", u.Apellido);
                    cmd.Parameters.AddWithValue("@Email", u.Email);
                    cmd.Parameters.AddWithValue("@IdRol", (int)u.Rol + 1);

                    // Solo agregamos el parámetro Password si el usuario cambió la clave
                    if (!string.IsNullOrWhiteSpace(u.Password))
                        cmd.Parameters.AddWithValue("@Password", PasswordHelper.HashPassword(u.Password));

                    // Ejecutamos la actualización
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ============================================================
        // MÉTODO: Eliminar(int idUsuario)
        // ------------------------------------------------------------
        // Realiza una baja lógica del usuario (Activo = 0).
        // Si el usuario es el último administrador, no lo desactiva.
        // ============================================================
        public void Eliminar(int idUsuario)
        {
            // Validamos el ID
            if (idUsuario <= 0)
                throw new ArgumentException("ID de usuario inválido.");

            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // Iniciamos una transacción para asegurar consistencia
                using (var tx = conn.BeginTransaction())
                {
                    // Obtenemos el rol del usuario a eliminar
                    var getRol = new SqlCommand(
                        "SELECT id_rol FROM dbo.Usuarios WHERE id_usuario=@id AND Activo=1", conn, tx);
                    getRol.Parameters.AddWithValue("@id", idUsuario);
                    var rolObj = getRol.ExecuteScalar();

                    // Si no existe el usuario o ya está inactivo
                    if (rolObj == null)
                        throw new InvalidOperationException("El usuario no existe o ya está inactivo.");

                    int idRol = Convert.ToInt32(rolObj);

                    // Evitamos eliminar al último administrador activo
                    if (idRol == 1) // Rol 1 = Administrador
                    {
                        var countAdmins = new SqlCommand(
                            "SELECT COUNT(*) FROM dbo.Usuarios WHERE id_rol = 1 AND Activo=1", conn, tx);
                        int admins = (int)countAdmins.ExecuteScalar();
                        if (admins <= 1)
                            throw new InvalidOperationException("No se puede eliminar el último administrador activo.");
                    }

                    // Ejecutamos la baja lógica
                    var upd = new SqlCommand(
                        "UPDATE dbo.Usuarios SET Activo = 0 WHERE id_usuario=@id", conn, tx);
                    upd.Parameters.AddWithValue("@id", idUsuario);
                    upd.ExecuteNonQuery();

                    // Confirmamos la transacción
                    tx.Commit();
                }
            }
        }

        // ------------------------------------------------------------
        // MÉTODO: Reactivar(int idUsuario)
        // Reactiva un usuario que estaba dado de baja (Activo = 0)
        // ------------------------------------------------------------
        public void Reactivar(int idUsuario)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = "UPDATE dbo.Usuarios SET Activo = 1 WHERE id_usuario = @id";

                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@id", idUsuario);
                    cmd.ExecuteNonQuery();
                }
            }
        }


        // ============================================================
        // MÉTODO: Login()
        // ------------------------------------------------------------
        // Verifica las credenciales del usuario y devuelve su objeto
        // si la autenticación es correcta.
        // ============================================================
        public Usuario Login(string email, string password)
        {
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // Consulta SQL para obtener el usuario por su correo
                string query = @"
                SELECT 
                    u.id_usuario,
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
                        // Si el usuario existe en la BD
                        if (reader.Read())
                        {
                            // Hash almacenado en la base de datos
                            string storedHash = reader["password"].ToString().Trim();

                            // Hash generado con la contraseña ingresada
                            string enteredHash = PasswordHelper.HashPassword(password);

                            // Verificamos si coinciden
                            if (PasswordHelper.VerifyPassword(password, storedHash))
                            {
                                return new Usuario
                                {
                                    IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
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

            // Si no se encontró o no coincide la contraseña
            return null;
        }

        // ============================================================
        // MÉTODO: ObtenerPorId()
        // ------------------------------------------------------------
        // Devuelve los datos completos de un usuario específico.
        // ============================================================
        public Usuario ObtenerPorId(int idUsuario)
        {
            Usuario u = null; // Valor por defecto

            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // Consulta SQL que trae los datos del usuario por su ID
                string query = @"
                    SELECT 
                        u.id_usuario,
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
                            // Mapeamos el registro al objeto Usuario
                            u = new Usuario
                            {
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")),
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

            return u;
        }
        // ------------------------------------------------------------
        // MÉTODO: ObtenerPorEmail()
        // 📌 Devuelve el usuario activo que tenga el email indicado.
        // Si no lo encuentra, retorna "null".
        // ------------------------------------------------------------
        public Usuario ObtenerPorEmail(string email)
        {
            Usuario u = null; // Inicializamos el objeto en null por si no se encuentra

            // Abrimos conexión a la base de datos
            using (SqlConnection conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open(); // Iniciamos la conexión

                // Consulta SQL que busca el usuario por su email (único)
                string query = @"
            SELECT 
                u.id_usuario,          -- ID único del usuario
                u.nombre,              -- Nombre
                u.apellido,            -- Apellido
                u.email,               -- Correo electrónico
                u.password,            -- Contraseña (encriptada)
                r.tipo_rol             -- Nombre del rol (Administrador, Profesor, etc.)
            FROM dbo.Usuarios u
            INNER JOIN dbo.Roles r ON u.id_rol = r.id_rol
            WHERE u.email = @Email AND u.Activo = 1;";

                // Creamos el comando SQL
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    // Agregamos el parámetro con el email recibido
                    cmd.Parameters.AddWithValue("@Email", email);

                    // Ejecutamos el comando y obtenemos un lector de datos
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        // Si se encontró un registro...
                        if (reader.Read())
                        {
                            // Mapeamos los valores de la base al objeto Usuario
                            u = new Usuario
                            {
                                IdUsuario = reader.GetInt32(reader.GetOrdinal("id_usuario")), // ID
                                Nombre = reader["nombre"].ToString(),                         // Nombre
                                Apellido = reader["apellido"].ToString(),                     // Apellido
                                Email = reader["email"].ToString(),                           // Email
                                Password = reader["password"].ToString(),                     // Hash de contraseña
                                Rol = (Rol)Enum.Parse(typeof(Rol), reader["tipo_rol"].ToString(), true) // Enum del rol
                            };
                        }
                    }
                }
            }

            // Devolvemos el usuario encontrado o null si no existe
            return u;
        }


    }
}
