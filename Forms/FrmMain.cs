using GymManager.Models;
using GymManager.Utils;
using GymManager.Views;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Forms
{
    public partial class FrmMain : Form
    {
        // Instancias de los UserControls
        private UcGenerarRutinas ucGenerarRutinas;
        private UcEditarRutina ucEditarRutina;
        private UcPlanillasRutinas ucPlanillasRutinas;

        // --- AÑADIDO ---
        // Panel para el Dashboard (Inicio)
        private Panel panelDashboard;

        public FrmMain()
        {
            InitializeComponent();
            // Inicializar los UserControls (PERO NO agregarlos aún)
            // Se agregarán en el FrmMain_Load
            InicializarUserControls();
        }

        private void InicializarUserControls()
        {
            // Solo crear las instancias
            ucGenerarRutinas = new UcGenerarRutinas();
            ucEditarRutina = new UcEditarRutina();
            ucPlanillasRutinas = new UcPlanillasRutinas();

            // --- AÑADIDO: Crear el panel de Dashboard ---
            panelDashboard = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White // O el color de fondo que prefieras
            };
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

            // (Tu código del título de la Navbar aquí...)
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


            // --- AÑADIDO: Llenar el panel de Dashboard ---
            // (Lo hacemos aquí porque necesitamos Sesion.Actual.Nombre)
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
                Image = Properties.Resources.Logo_gymM13, // Asegúrate que este recurso exista
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill
            };
            // Agregamos el logo y label AL panelDashboard
            panelDashboard.Controls.Add(logo);
            panelDashboard.Controls.Add(lbl);

            // --- AÑADIDO: Agregar TODOS los paneles al panelContenido ---
            // Los agregamos aquí, una sola vez.
            ucGenerarRutinas.Dock = DockStyle.Fill;
            ucEditarRutina.Dock = DockStyle.Fill;
            ucPlanillasRutinas.Dock = DockStyle.Fill;
            // panelDashboard ya tiene Dock.Fill de la inicialización

            panelContenido.Controls.Add(ucGenerarRutinas);
            panelContenido.Controls.Add(ucEditarRutina);
            panelContenido.Controls.Add(ucPlanillasRutinas);
            panelContenido.Controls.Add(panelDashboard); // <-- Añadir el dashboard
            // -----------------------------------------------------------

            CargarNavbar(Sesion.Actual.Rol);
            MostrarDashboard(Sesion.Actual.Rol); // <-- Muestra el dashboard al inicio
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
            }

            // Botón "Cerrar sesión"
            AgregarBotonNav("Cerrar sesión", () =>
            {
                Sesion.Cerrar();
                Application.Restart();
            }, DockStyle.Bottom);
        }

        // =========================================================
        // 🔥 MÉTODOS DE NAVEGACIÓN CORREGIDOS (Usan BringToFront) 🔥
        // =========================================================

        private void MostrarGenerarRutinas()
        {
            ucGenerarRutinas.BringToFront();
        }

        public void MostrarEditarRutina()
        {
            // 1. Obtener las listas generadas (en memoria) desde ucGenerarRutinas
            var listaHombres = ucGenerarRutinas.rutinaHombres;
            var listaMujeres = ucGenerarRutinas.rutinaMujeres;
            var listaDeportistas = ucGenerarRutinas.rutinaDeportistas;

            // 2. Pasarlas al panel de edición para que actualice sus botones
            ucEditarRutina.ActualizarYMostrarPanelSeleccion(listaHombres, listaMujeres, listaDeportistas);

            // 3. Mostrar el panel de edición
            ucEditarRutina.BringToFront();
        }

        public void MostrarPlanillas()
        {
            ucPlanillasRutinas.BringToFront();
        }

        // Este método para Admin/Recepcionista usa el patrón Clear/Add
        // Esto romperá la navegación si vuelves a "Generar Rutina"
        // (Considera crear UserControls únicos también para estas vistas)
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

        public void LimpiarRutinaGeneradaEnPanel(string tipoRutina)
        {
            ucGenerarRutinas?.LimpiarRutinaGenerada(tipoRutina);
        }

        private void MostrarDashboard(Rol rol)
        {
            // Ya no necesita Clear() ni Add()
            // Solo trae el panel del dashboard al frente.
            panelDashboard.BringToFront();
        }
    }
}