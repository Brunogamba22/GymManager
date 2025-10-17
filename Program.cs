using System;
using System.Windows.Forms;
using GymManager.Utils;

namespace GymManager
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            string hash = PasswordHelper.HashPassword("1234");
            MessageBox.Show($"Hash generado por el código:\n{hash}", "Debug Hash");

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Forms.FrmLogin());
        }
    }
}
