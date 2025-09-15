// Importa las bibliotecas necesarias del sistema y del proyecto
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

// Importa las clases del proyecto
using GymManager.Models;  // Acceso a modelos como Usuario, Rol, etc.
using GymManager.Utils;   // Acceso a la clase Sesion

// Namespace principal del formulario
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
                MessageBox.Show("No hay sesión iniciada");  // Muestra mensaje
                this.Close();                               // Cierra el formulario
                return;                                     // Sale del método
            }

            // Muestra mensaje de bienvenida en el encabezado
            lblBienvenida.Text = $"Bienvenido {Sesion.Actual.Nombre} ({Sesion.Actual.Rol})";

            // Carga el menú lateral dependiendo del rol
            CargarNavbar(Sesion.Actual.Rol);

            // Carga el contenido principal (dashboard) según el rol
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

        // Crea el menú lateral (navbar) dinámicamente según el rol
        private void CargarNavbar(Rol rol)
        {
            panelNavbar.Controls.Clear();  // Limpia el menú anterior

            // Botón "Inicio" (siempre visible)
            AgregarBotonNav("Inicio", () => MostrarDashboard(rol));

            // Si es profesor, muestra acceso a rutinas
            if (rol == Rol.Profesor)
            {
                AgregarBotonNav("Rutinas", () => CargarVista(new Views.UcProfesorDashboard()));
                // Ejemplo de otras vistas posibles:
                // AgregarBotonNav("Alumnos", () => CargarVista(new Views.UcAlumnos()));
            }

            // Si es administrador, muestra opciones de usuarios y reportes
            if (rol == Rol.Administrador)
            {
                AgregarBotonNav("Ejercicios", () => CargarVista(new Views.UcAdminDashboard()));
                AgregarBotonNav("Usuarios", () => MessageBox.Show("Vista de usuarios (a crear)"));
                AgregarBotonNav("Reportes", () => MessageBox.Show("Vista de reportes (a crear)"));
            }

            // Si es recepcionista, muestra opción de turnos
            if (rol == Rol.Recepcionista)
            {
                AgregarBotonNav("Turnos", () => MessageBox.Show("Vista de turnos (a crear)"));
            }

            // Botón para cerrar sesión (siempre visible)
            AgregarBotonNav("Cerrar sesión", () =>
            {
                Utils.Sesion.Cerrar();     // Cierra la sesión actual
                Application.Restart();     // Reinicia la app y vuelve al login
            });
        }

        // Crea un botón en el navbar con su acción correspondiente
        private void AgregarBotonNav(string texto, Action onClick)
        {
            var btn = new Button();                          // Crea el botón
            btn.Text = texto;                                // Texto visible del botón
            btn.Dock = DockStyle.Top;                        // Se apila verticalmente
            btn.Height = 50;                                 // Altura del botón
            btn.FlatStyle = FlatStyle.Flat;                  // Estilo plano
            btn.FlatAppearance.BorderSize = 0;               // Sin borde
            btn.BackColor = Color.RoyalBlue;                 // Color de fondo
            btn.ForeColor = Color.White;                     // Color de texto
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold); // Fuente
            btn.Click += (s, e) => onClick();                // Evento al hacer clic

            panelNavbar.Controls.Add(btn);                   // Agrega el botón al panel lateral
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
            panelContenido.Controls.Clear();   // Limpia la vista anterior
            vista.Dock = DockStyle.Fill;       // Ajusta tamaño al panel
            panelContenido.Controls.Add(vista); // Agrega la nueva vista
        }
    }
}
