using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymManager.Models;
using GymManager.Utils;

namespace GymManager
{
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            //Simulacion de login
            Sesion.Actual = new Usuario
            {
                Id = 1,
                Nombre = "MC Bruninho",
                Email = "mc@gmail.com",
                Rol = Rol.Administrador
            };

            Application.Run(new Forms.FrmMain());
        }
    }
}
