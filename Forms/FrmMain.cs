// Importa las bibliotecas necesarias del sistema y del proyecto
using System;
using System.Drawing;
using System.Windows.Forms;

// Importa las clases del proyecto
using GymManager.Models;  // Acceso a modelos como Usuario, Rol, etc.
using GymManager.Utils;   // Acceso a la clase Sesion

namespace GymManager.Forms
{
    // Clase principal del formulario que se muestra después del login
    public partial class FrmMain : Form
    {
        // Constructor del formulario
        public FrmMain()
        {
            InitializeComponent(); // Inicializa todos los controles del formulario (desde el diseñador)
        }

        // Evento que se ejecuta al cargar el formulario
        private void FrmMain_Load(object sender, EventArgs e)
        {
            // Si no hay usuario logueado, se cierra la aplicación
            if (Sesion.Actual == null)
            {
                MessageBox.Show("No hay sesión iniciada");
                this.Close();
                return;
            }

            // --- Estética del panel lateral ---
            panelNavbar.BackColor = Color.FromArgb(45, 52, 70); // gris azulado oscuro

            // Logo/título arriba
            Label lblTitulo = new Label
            {
                Text = "🏋️ GymManager",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelNavbar.Controls.Add(lblTitulo);

            // Mensaje de bienvenida en el encabezado superior
            lblBienvenida.Text = $"Bienvenido {Sesion.Actual.Nombre} ({Sesion.Actual.Rol})";

            // Carga el menú lateral dependiendo del rol
            CargarNavbar(Sesion.Actual.Rol);

            // Carga el dashboard principal inicial según rol
            MostrarDashboard(Sesion.Actual.Rol);
        }

        // Crea el menú lateral dinámicamente según el rol
        private void CargarNavbar(Rol rol)
        {
            panelNavbar.Controls.Clear();  // Limpia el menú anterior

            // Botón "Inicio" (arriba de todo)
            AgregarBotonNav("Inicio", () => MostrarDashboard(rol), DockStyle.Top);

            // ---- Opciones según rol ----
            if (rol == Rol.Profesor)
            {
                AgregarBotonNav("Generar Rutinas", () => CargarVista(new Views.UcGenerarRutinas()), DockStyle.Top);
                AgregarBotonNav("Editar Rutina", () => CargarVista(new Views.UcEditarRutina()), DockStyle.Top);
                AgregarBotonNav("Planillas", () => CargarVista(new Views.UcPlanillasRutinas()), DockStyle.Top);
            }

            if (rol == Rol.Administrador)
            {
                AgregarBotonNav("Usuarios", () => CargarVista(new Views.UcGestionUsuarios()), DockStyle.Top);
                AgregarBotonNav("Ejercicios", () => CargarVista(new Views.UcAdminDashboard()), DockStyle.Top);
                AgregarBotonNav("Reportes", () => CargarVista(new Views.UcReportes()), DockStyle.Top);
            }

            if (rol == Rol.Recepcionista)
            {
                AgregarBotonNav("Rutina", () => CargarVista(new Views.UcRecepcionistaDashboard()), DockStyle.Top);
                AgregarBotonNav("Imprimir rutina", () => MessageBox.Show("Aquí se imprimiría la rutina"), DockStyle.Top);
                AgregarBotonNav("Exportar", () => MessageBox.Show("Aquí se exportaría la rutina"), DockStyle.Top);
            }

            // ---- Botón "Cerrar sesión" (abajo SIEMPRE) ----
            AgregarBotonNav("Cerrar sesión", () =>
            {
                Sesion.Cerrar();
                Application.Restart();
            }, DockStyle.Bottom);
        }

        // Modificado para permitir elegir posición (Top o Bottom)
        private void AgregarBotonNav(string texto, Action onClick, DockStyle dockPos)
        {
            var btn = new Button
            {
                Text = texto,
                Dock = dockPos,   // ahora podés elegir Top o Bottom
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = texto == "Cerrar sesión" ? Color.DarkRed : Color.FromArgb(45, 62, 80),
                ForeColor = Color.White
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (s, e) => onClick();

            panelNavbar.Controls.Add(btn);
        }


        private void MostrarDashboard(Rol rol)
        {
            panelContenido.Controls.Clear();

            Label lbl = new Label
            {
                Text = $"Bienvenido {Sesion.Actual.Nombre}, seleccioná una opción del menú.",
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.DimGray,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 60
            };

            PictureBox logo = new PictureBox
            {
                Image = Properties.Resources.Logo_gymM13, // agregá una imagen a Resources
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill
            };

            panelContenido.Controls.Add(logo);
            panelContenido.Controls.Add(lbl);
        }


        // Carga una vista (UserControl) en el panel principal
        private void CargarVista(UserControl vista)
        {
            panelContenido.Controls.Clear();
            vista.Dock = DockStyle.Fill;
            panelContenido.Controls.Add(vista);
        }
    }
}
