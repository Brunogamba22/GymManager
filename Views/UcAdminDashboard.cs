using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymManager.Views
{
    public partial class UcAdminDashboard : UserControl
    {
        public UcAdminDashboard()
        {
            InitializeComponent(); // Inicializa componentes del diseñador

            // Evento de carga del control
            this.Load += UcAdminDashboard_Load;

        }

        // Evento que se dispara al cargarse el UserControl
        private void UcAdminDashboard_Load(object sender, EventArgs e)
        {
            // Opcional: cargar datos iniciales o estados
        }

        // Método que carga un UserControl dentro del panel contenedor
        private void CargarVista(UserControl vista)
        {
            panelContenidoAdmin.Controls.Clear();       // Limpia el panel
            vista.Dock = DockStyle.Fill;                // Ocupa todo el espacio
            panelContenidoAdmin.Controls.Add(vista);    // Agrega la vista
        }

        // Evento cuando se hace clic en el botón de gestión de ejercicios
        private void btnGestionEjercicios_Click(object sender, EventArgs e)
        {
            CargarVista(new UcGestionEjercicios()); // Carga el UserControl de ejercicios
        }

        // Aquí podés agregar más métodos para otros botones en el futuro





    }
}
