using System;
using System.Drawing;
using System.Windows.Forms;
using GymManager.Models;
using GymManager.Utils;
using GymManager.Views;

namespace GymManager.Forms
{
    public partial class FrmMain : Form
    {
        // Variables para mantener las instancias de los UserControls
        private UcGenerarRutinas ucGenerarRutinas;
        private UcEditarRutina ucEditarRutina;
        private UcPlanillasRutinas ucPlanillasRutinas;

        public FrmMain()
        {
            InitializeComponent();
            
            // Inicializar los UserControls una sola vez
            InicializarUserControls();
        }

        private void InicializarUserControls()
        {
            // Crear las instancias una sola vez (no cada vez que se hace clic)
            ucGenerarRutinas = new UcGenerarRutinas();
            ucEditarRutina = new UcEditarRutina();
            ucPlanillasRutinas = new UcPlanillasRutinas();
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            if (Sesion.Actual == null)
            {
                MessageBox.Show("No hay sesión iniciada");
                this.Close();
                return;
            }
            var globalColor = Color.FromArgb(45, 52, 70);

            panelNavbar.BackColor = globalColor;
            panelHeader.BackColor = globalColor;
            panelFooter.BackColor = globalColor;

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

            
            CargarNavbar(Sesion.Actual.Rol);
            MostrarDashboard(Sesion.Actual.Rol);
        }

        private void CargarNavbar(Rol rol)
        {
            panelNavbar.Controls.Clear();

            // Botón "Inicio"
            AgregarBotonNav("Inicio", () => MostrarDashboard(rol), DockStyle.Top);

            // ---- Opciones según rol ----
            if (rol == Rol.Profesor)
            {
                AgregarBotonNav("Generar Rutinas", () => MostrarGenerarRutinas(), DockStyle.Top);
                AgregarBotonNav("Editar Rutina", () => MostrarEditarRutina(), DockStyle.Top);
                AgregarBotonNav("Planillas", () => MostrarPlanillas(), DockStyle.Top);
            }

            if (rol == Rol.Administrador)
            {
                AgregarBotonNav("Usuarios", () => CargarVista(new Views.UcGestionUsuarios()), DockStyle.Top);
                AgregarBotonNav("Ejercicios", () => CargarVista(new Views.UcGestionEjercicios()), DockStyle.Top);
                AgregarBotonNav("Reportes", () => CargarVista(new Views.UcReportes()), DockStyle.Top);
            }

            if (rol == Rol.Recepcionista)
            {
                AgregarBotonNav("Rutina", () => CargarVista(new Views.UcRecepcionistaDashboard()), DockStyle.Top);
                //AgregarBotonNav("Imprimir rutina", () => MessageBox.Show("Aquí se imprimiría la rutina"), DockStyle.Top);
                //AgregarBotonNav("Exportar", () => MessageBox.Show("Aquí se exportaría la rutina"), DockStyle.Top);
            }

            // Botón "Cerrar sesión"
            AgregarBotonNav("Cerrar sesión", () =>
            {
                Sesion.Cerrar();
                Application.Restart();
            }, DockStyle.Bottom);
        }

        // MÉTODOS ESPECÍFICOS PARA CADA VISTA (mantienen el estado)
        private void MostrarGenerarRutinas()
        {
            panelContenido.Controls.Clear();
            ucGenerarRutinas.Dock = DockStyle.Fill;

            // RESTAURAR LAS RUTINAS AL MOSTRAR LA VISTA
            ucGenerarRutinas.RestaurarRutinas();

            panelContenido.Controls.Add(ucGenerarRutinas);
        }

        private void MostrarEditarRutina()
        {
            panelContenido.Controls.Clear();
            ucEditarRutina.Dock = DockStyle.Fill;
            panelContenido.Controls.Add(ucEditarRutina);
        }

        private void MostrarPlanillas()
        {
            panelContenido.Controls.Clear();
            ucPlanillasRutinas.Dock = DockStyle.Fill;
            panelContenido.Controls.Add(ucPlanillasRutinas);
        }

        // Método genérico para otras vistas (no mantiene estado)
        private void CargarVista(UserControl vista)
        {
            panelContenido.Controls.Clear();
            vista.Dock = DockStyle.Fill;
            panelContenido.Controls.Add(vista);
        }

        private void AgregarBotonNav(string texto, Action onClick, DockStyle dockPos)
        {
            var btn = new Button
            {
                Text = texto,
                Dock = dockPos,
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
                Image = Properties.Resources.Logo_gymM13,
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill
            };

            panelContenido.Controls.Add(logo);
            panelContenido.Controls.Add(lbl);
        }
    }
}