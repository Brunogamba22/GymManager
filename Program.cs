using GymManager.Controllers;  // Controladores (lógica de negocio)
using GymManager.Models;       // Modelos de datos (Usuario, Rol, etc.)
using GymManager.Utils;        // Utilidades (conexión, helpers)
using System;
using System.Windows.Forms;

namespace GymManager
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            // ------------------------------------------------------------
            // CONFIGURACIÓN INICIAL DEL APLICATIVO
            // ------------------------------------------------------------
            Application.EnableVisualStyles();                       // Usa estilos visuales modernos
            Application.SetCompatibleTextRenderingDefault(false);    // Mejora la compatibilidad del renderizado de texto

            // Instancia del controlador que maneja los usuarios
            var usuarioController = new UsuarioController();

            // ------------------------------------------------------------
            // 🔹 Helper local: Crear usuario por email si no existe
            // (Ya no usamos DNI como identificador)
            // ------------------------------------------------------------
            void CrearSiNoExiste(string email, string nombre, string apellido, string password, Rol rol)
            {
                // Consulta si ya hay un usuario con ese email
                Usuario existente = usuarioController.ObtenerPorEmail(email);

                // Si no existe, lo creamos
                if (existente == null)
                {
                    var nuevo = new Usuario
                    {
                        Nombre = nombre,
                        Apellido = apellido,
                        Email = email,
                        Password = password, // Se hashea automáticamente en el controlador
                        Rol = rol
                    };

                    // Lo insertamos en la BD
                    usuarioController.Agregar(nuevo);
                }
            }

            // ------------------------------------------------------------
            // 🧠 SEED DE USUARIOS (solo se ejecuta la primera vez)
            // ------------------------------------------------------------
            // Se crean 3 usuarios base para iniciar sesión por primera vez
            CrearSiNoExiste("admin@gmail.com", "Admin", "Principal", "1234", Rol.Administrador);
            CrearSiNoExiste("profe@gmail.com", "Profesor", "Principal", "1234", Rol.Profesor);
            CrearSiNoExiste("recep@gmail.com", "Recepcionista", "Principal", "1234", Rol.Recepcionista);

            // ------------------------------------------------------------
            // 🚀 INICIO DE LA APLICACIÓN
            // ------------------------------------------------------------
            // Muestra el formulario de login como pantalla inicial
            Application.Run(new Forms.FrmLogin());
            // Si querés probar sin login, podés usar:
            // Application.Run(new Forms.FrmMain());
        }
    }
}
