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

        // Carga un UserControl en el panel principal (centro de la pantalla)
        private void CargarContenido(UserControl uc)
        {
            panelContenido.Controls.Clear();  // Limpia el contenido anterior
            uc.Dock = DockStyle.Fill;         // Hace que el nuevo UserControl ocupe todo el espacio
            panelContenido.Controls.Add(uc);  // Agrega el nuevo UserControl al panel
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

            // Mensaje de bienvenida en el encabezado
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

            // Botón "Inicio" (siempre visible)
            AgregarBotonNav("Inicio", () => MostrarDashboard(rol));

            // Si es profesor, muestra opciones relacionadas a rutinas
            if (rol == Rol.Profesor)
            {
                AgregarBotonNav("Generar Rutinas", () => CargarVista(new Views.UcProfesorDashboard()));

                AgregarBotonNav("Editar Rutina", () =>
                {
                    // De momento mostramos un placeholder hasta crear el UserControl
                    //MessageBox.Show("Aquí se cargará la vista de edición de rutinas");
                    CargarVista(new Views.UcEditarRutina());
                });

                AgregarBotonNav("Planillas", () =>
                {
                    // Placeholder de planillas
                    MessageBox.Show("Aquí se cargará la vista de planillas");
                    // Ejemplo futuro: CargarVista(new Views.UcPlanillasRutinas());
                });
            }


            // ---- Administrador ----
            if (rol == Rol.Administrador)
            {
                AgregarBotonNav("Ejercicios", () => CargarVista(new Views.UcAdminDashboard()));
                AgregarBotonNav("Usuarios", () => CargarVista(new Views.UcGestionUsuarios()));
                AgregarBotonNav("Reportes", () => MessageBox.Show("Vista de reportes (a crear)"));
            }

            // ---- Recepcionista ----
            if (rol == Rol.Recepcionista)
            {
                AgregarBotonNav("Turnos", () => MessageBox.Show("Vista de turnos (a crear)"));
            }

            // ---- Botón para cerrar sesión (siempre visible) ----
            AgregarBotonNav("Cerrar sesión", () =>
            {
                Sesion.Cerrar();
                Application.Restart();
            });
        }

        // Crea un botón en el navbar con su acción correspondiente
        private void AgregarBotonNav(string texto, Action onClick)
        {
            var btn = new Button
            {
                Text = texto,
                Dock = DockStyle.Top,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = Color.RoyalBlue,
                ForeColor = Color.White
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (s, e) => onClick();

            panelNavbar.Controls.Add(btn);
        }

        // Muestra el dashboard principal según el rol
        private void MostrarDashboard(Rol rol)
        {
            switch (rol)
            {
                case Rol.Administrador:
                    CargarVista(new Views.UcAdminDashboard());
                    break;
                case Rol.Profesor:
                    CargarVista(new Views.UcProfesorDashboard());
                    break;
                case Rol.Recepcionista:
                    CargarVista(new Views.UcRecepcionistaDashboard());
                    break;
            }
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
