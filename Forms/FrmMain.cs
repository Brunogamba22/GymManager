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

    }
}
