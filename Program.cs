using GymManager.Controllers;
using GymManager.Models;
using GymManager.Utils;
using System;
using System.Windows.Forms;

namespace GymManager
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var usuarioController = new UsuarioController();

            // Helper local para crear un usuario por DNI si no existe
            void CrearSiNoExiste(string dni, string nombre, string apellido, string email, string password, Rol rol)
            {
                if (!usuarioController.ExisteUsuario(dni))
                {
                    var u = new Usuario
                    {
                        Dni = dni,                // 👈 CLAVE LÓGICA (string)
                        Nombre = nombre,
                        Apellido = apellido,
                        Email = email,
                        Password = password,     // El controller lo hashea al guardar
                        Rol = rol
                    };
                    usuarioController.Agregar(u);
                }
            }

            // Seed básico (solo la primera vez)
            CrearSiNoExiste("99999999", "Admin", "Principal", "admin@gmail.com", "1234", Rol.Administrador);
            CrearSiNoExiste("99999998", "Profesor", "Principal", "profe@gmail.com", "1234", Rol.Profesor);
            CrearSiNoExiste("99999997", "Recepcionista", "Principal", "recep@gmail.com", "1234", Rol.Recepcionista);

            // Iniciar app: Login real
            Application.Run(new Forms.FrmLogin());
            // Application.Run(new Forms.FrmMain()); // si querés saltar el login en desarrollo
        }
    }
}
