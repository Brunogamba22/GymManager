using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using GymManager.Models;
using GymManager.Utils;


namespace GymManager.Forms
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void CargarContenido(UserControl uc)
        {
            panelContenido.Controls.Clear();   // Limpia el área central
            uc.Dock = DockStyle.Fill;        // Que ocupe todo el espacio
            panelContenido.Controls.Add(uc);   // Inserta el UserControl
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            if (Sesion.Actual == null)
            {
                MessageBox.Show("No hay sesión iniciada");
                this.Close();
                return;
            }

            //Mensaje dinámico en el header
            lblBienvenida.Text = $"Bienvenido {Sesion.Actual.Nombre} ({Sesion.Actual.Rol})";

            //Cargar el UserControl según el rol
            switch (Sesion.Actual.Rol)
            {
                case Rol.Administrador:
                    CargarContenido(new Views.UcAdminDashboard());
                    break;
                case Rol.Profesor:
                    CargarContenido(new Views.UcProfesorDashboard());
                    break;
                case Rol.Recepcionista:
                    CargarContenido(new Views.UcRecepcionistaDashboard());
                    break;
            }
        }

    }
}
