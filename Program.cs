using System;
using System.Windows.Forms;
using GymManager.Models;
using GymManager.Utils;

namespace GymManager
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // ⚠️ SIMULACIÓN DE LOGIN (usuario ficticio)
            Sesion.Actual = new Usuario
            {
                Id = "12345678",                  // Ahora es string (dni)
                Nombre = "MC Bruninho",
                Email = "mc@gmail.com",
                Rol = Rol.Profesor
            };

            // Si querés probar el Login real, usá FrmLogin
            Application.Run(new Forms.FrmLogin());
            // Application.Run(new Forms.FrmMain());
        }
    }
}
