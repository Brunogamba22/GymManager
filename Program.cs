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

            // Crear admin inicial si no existe
            var usuarioController = new UsuarioController();

            if (!usuarioController.ExisteUsuario("99999999"))
            {
                var admin = new Usuario
                {
                    Id = "99999999",
                    Nombre = "Admin",
                    Apellido = "Principal",
                    Email = "admin@gmail.com",
                    Password = "1234",
                    Rol = Rol.Administrador
                };
                usuarioController.Agregar(admin);
            }

            if (!usuarioController.ExisteUsuario("99999998"))
            {
                var profesor = new Usuario
                {
                    Id = "99999998",
                    Nombre = "Profesor",
                    Apellido = "Principal",
                    Email = "profe@gmail.com",
                    Password = "1234",
                    Rol = Rol.Profesor
                };
                usuarioController.Agregar(profesor);
            }

            if (!usuarioController.ExisteUsuario("99999997"))
            {
                var recepcionista = new Usuario
                {
                    Id = "99999997",
                    Nombre = "Recepcionista",
                    Apellido = "Principal",
                    Email = "recep@gmail.com",
                    Password = "1234",
                    Rol = Rol.Recepcionista
                };
                usuarioController.Agregar(recepcionista);
            }



            // Si querés probar el Login real, usá FrmLogin
            Application.Run(new Forms.FrmLogin());
            // Application.Run(new Forms.FrmMain());
        }
    }
}
