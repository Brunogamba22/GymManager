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
    /// <summary>
    /// UserControl principal del Administrador.
    /// Sirve como panel de inicio para acceder a distintas gestiones
    /// dentro del sistema (ej: gestión de ejercicios).
    /// Forma parte de la capa de Vista (MVC).
    /// </summary>
    public partial class UcAdminDashboard : UserControl
    {
        /// <summary>
        /// Constructor del UserControl.
        /// Inicializa los componentes gráficos y asocia el evento Load.
        /// </summary>
        public UcAdminDashboard()
        {
            InitializeComponent(); // Inicializa los elementos creados en el diseñador.

            // Se suscribe el método "UcAdminDashboard_Load"
            // al evento "Load" del UserControl.
            this.Load += UcAdminDashboard_Load;
        }

        /// <summary>
        /// Evento que se ejecuta al cargarse el UserControl.
        /// Ideal para inicializar datos o estados visuales.
        /// </summary>
        private void UcAdminDashboard_Load(object sender, EventArgs e)
        {
            // Opcional: aquí se podrían cargar datos iniciales del panel.
        }

        /// <summary>
        /// Método que carga dinámicamente otro UserControl
        /// dentro del panel de contenido del Administrador.
        /// </summary>
        private void CargarVista(UserControl vista)
        {
            panelContenidoAdmin.Controls.Clear();       // Limpia el panel antes de insertar algo nuevo.
            vista.Dock = DockStyle.Fill;                // Hace que la vista ocupe todo el espacio disponible.
            panelContenidoAdmin.Controls.Add(vista);    // Agrega la vista al panel.
        }

        /// <summary>
        /// Evento al hacer clic en el botón "Gestionar Ejercicios".
        /// Carga el UserControl de gestión de ejercicios dentro del panel.
        /// </summary>
        private void btnGestionEjercicios_Click(object sender, EventArgs e)
        {
            CargarVista(new UcGestionEjercicios()); // Muestra la vista de ejercicios.
        }

        // Aquí se pueden agregar más métodos para otros botones o funcionalidades futuras.
    }
}
