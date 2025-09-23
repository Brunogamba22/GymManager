using System.Windows.Forms;

namespace GymManager.Views
{
    /// <summary>
    /// Código generado por el Diseñador de Visual Studio
    /// para definir la interfaz gráfica (controles, posiciones y estilos).
    /// </summary>
    partial class UcAdminDashboard
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelContenidoAdmin;    // Panel donde se cargan dinámicamente las vistas hijas.
        private Button btnGestionEjercicios;  // Botón para ir a la gestión de ejercicios.
        private Label lblTitulo;              // Etiqueta del título principal.

        /// <summary>
        /// Libera recursos utilizados por los componentes gráficos.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Inicializa y configura todos los controles de la interfaz.
        /// Este método es autogenerado por el Diseñador de Visual Studio.
        /// </summary>
        private void InitializeComponent()
        {
            this.panelContenidoAdmin = new Panel();
            this.btnGestionEjercicios = new Button();
            this.lblTitulo = new Label();
            this.SuspendLayout();

            // 
            // lblTitulo
            // 
            this.lblTitulo.Text = "Panel de Administración"; // Texto que se muestra en pantalla.
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 20); // Posición en el formulario.
            this.lblTitulo.AutoSize = true; // Ajusta el tamaño al texto.

            // 
            // btnGestionEjercicios
            // 
            this.btnGestionEjercicios.Text = "Gestionar Ejercicios"; // Texto del botón.
            this.btnGestionEjercicios.Location = new System.Drawing.Point(20, 60); // Posición en pantalla.
            this.btnGestionEjercicios.Size = new System.Drawing.Size(180, 40); // Tamaño del botón.
            this.btnGestionEjercicios.Click += new System.EventHandler(this.btnGestionEjercicios_Click); // Asocia el evento click.

            // 
            // panelContenidoAdmin
            // 
            this.panelContenidoAdmin.Location = new System.Drawing.Point(20, 120); // Posición.
            this.panelContenidoAdmin.Size = new System.Drawing.Size(600, 350);     // Tamaño.
            this.panelContenidoAdmin.BorderStyle = BorderStyle.FixedSingle;        // Borde para diferenciarlo.

            // 
            // UcAdminDashboard
            // 
            this.Controls.Add(this.lblTitulo);            // Agrega el título al control.
            this.Controls.Add(this.btnGestionEjercicios); // Agrega el botón.
            this.Controls.Add(this.panelContenidoAdmin);  // Agrega el panel.
            this.Size = new System.Drawing.Size(650, 500); // Tamaño total del UserControl.
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
