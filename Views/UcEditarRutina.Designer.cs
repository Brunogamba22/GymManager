using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Views
{
    partial class UcEditarRutina
    {
        private System.ComponentModel.IContainer components = null;

        // --- Panel 1: Controles de EDICIÓN (Tus controles existentes) ---
        private Panel pnlEdicion; // Panel principal que agrupa la EDICIÓN
        private Panel headerPanel;
        private Panel contentPanel;
        private Panel footerPanel;
        private DataGridView dgvRutinas;
        private Button btnGuardar;
        private Button btnAgregarEjercicio;
        private Button btnEliminarEjercicio;
        private Button btnLimpiarTodo;
        private Label lblTitulo;
        private Label lblDescripcion;
        private Panel panelAcciones;
        private Button btnVolver; // NUEVO: Botón para volver a la selección

        // --- Panel 2: Controles de SELECCIÓN (NUEVOS) ---
        private Panel pnlSeleccion;
        private Label lblTituloSeleccion;
        private Label lblDescripcionSeleccion;
        private Button btnSeleccionarRutinaHombre;
        private Button btnSeleccionarRutinaMujer;
        private Button btnSeleccionarRutinaDeportista;
        private Panel panelBotonesSeleccion;


        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // --- INICIALIZACIÓN DE COMPONENTES ---
            // Panel 1 (Edición)
            this.pnlEdicion = new Panel();
            this.headerPanel = new Panel();
            this.contentPanel = new Panel();
            this.footerPanel = new Panel();
            this.dgvRutinas = new DataGridView();
            this.btnGuardar = new Button();
            this.btnAgregarEjercicio = new Button();
            this.btnEliminarEjercicio = new Button();
            this.btnLimpiarTodo = new Button();
            this.lblTitulo = new Label();
            this.lblDescripcion = new Label();
            this.panelAcciones = new Panel();
            this.btnVolver = new Button(); // Nuevo
            var centerPanelEdicion = new Panel(); // Para centrar botón Guardar

            // Panel 2 (Selección)
            this.pnlSeleccion = new Panel();
            this.lblTituloSeleccion = new Label();
            this.lblDescripcionSeleccion = new Label();
            this.panelBotonesSeleccion = new Panel();
            this.btnSeleccionarRutinaHombre = new Button();
            this.btnSeleccionarRutinaMujer = new Button();
            this.btnSeleccionarRutinaDeportista = new Button();

            this.SuspendLayout();
            this.Size = new Size(900, 600);

            // =========================================================
            // CONFIGURACIÓN DEL PANEL DE SELECCIÓN (pnlSeleccion)
            // =========================================================
            this.pnlSeleccion.Dock = DockStyle.Fill;
            this.pnlSeleccion.Padding = new Padding(20);
            this.pnlSeleccion.Name = "pnlSeleccion";

            // Título (Selección)
            this.lblTituloSeleccion.Text = "Seleccionar Rutina";
            this.lblTituloSeleccion.Dock = DockStyle.Top;
            this.lblTituloSeleccion.Height = 40;
            this.lblTituloSeleccion.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            this.lblTituloSeleccion.TextAlign = ContentAlignment.BottomLeft;
            this.lblTituloSeleccion.Padding = new Padding(5, 0, 0, 0);

            // Descripción (Selección)
            this.lblDescripcionSeleccion.Text = "Elige qué rutina generada deseas modificar.";
            this.lblDescripcionSeleccion.Dock = DockStyle.Top;
            this.lblDescripcionSeleccion.Height = 30;
            this.lblDescripcionSeleccion.Font = new Font("Segoe UI", 9);
            this.lblDescripcionSeleccion.ForeColor = Color.FromArgb(100, 100, 100);
            this.lblDescripcionSeleccion.TextAlign = ContentAlignment.TopLeft;
            this.lblDescripcionSeleccion.Padding = new Padding(5, 0, 0, 0);

            // Panel para los botones (Selección)
            this.panelBotonesSeleccion.Dock = DockStyle.Fill;
            this.panelBotonesSeleccion.Padding = new Padding(5, 30, 5, 5);

            // Botones de Selección
            this.btnSeleccionarRutinaHombre.Text = "HOMBRES (Buscando...)";
            this.btnSeleccionarRutinaHombre.Dock = DockStyle.Top;
            this.btnSeleccionarRutinaHombre.Height = 50;
            this.btnSeleccionarRutinaHombre.Margin = new Padding(10, 10, 10, 10);

            this.btnSeleccionarRutinaMujer.Text = "MUJERES (Buscando...)";
            this.btnSeleccionarRutinaMujer.Dock = DockStyle.Top;
            this.btnSeleccionarRutinaMujer.Height = 50;
            this.btnSeleccionarRutinaMujer.Margin = new Padding(10, 10, 10, 10);

            this.btnSeleccionarRutinaDeportista.Text = "DEPORTISTAS (Buscando...)";
            this.btnSeleccionarRutinaDeportista.Dock = DockStyle.Top;
            this.btnSeleccionarRutinaDeportista.Height = 50;
            this.btnSeleccionarRutinaDeportista.Margin = new Padding(10, 10, 10, 10);

            // Ensamblar Panel de Selección
            this.panelBotonesSeleccion.Controls.Add(this.btnSeleccionarRutinaDeportista);
            this.panelBotonesSeleccion.Controls.Add(new Panel { Height = 15, Dock = DockStyle.Top }); // Espaciador
            this.panelBotonesSeleccion.Controls.Add(this.btnSeleccionarRutinaMujer);
            this.panelBotonesSeleccion.Controls.Add(new Panel { Height = 15, Dock = DockStyle.Top }); // Espaciador
            this.panelBotonesSeleccion.Controls.Add(this.btnSeleccionarRutinaHombre);
            this.pnlSeleccion.Controls.Add(this.panelBotonesSeleccion);
            this.pnlSeleccion.Controls.Add(this.lblDescripcionSeleccion);
            this.pnlSeleccion.Controls.Add(this.lblTituloSeleccion);


            // =========================================================
            // CONFIGURACIÓN DEL PANEL DE EDICIÓN (pnlEdicion)
            // =========================================================
            this.pnlEdicion.Dock = DockStyle.Fill;
            this.pnlEdicion.Padding = new Padding(20);
            this.pnlEdicion.Name = "pnlEdicion";

            // Header Panel (Edición)
            this.headerPanel.Dock = DockStyle.Top;
            this.headerPanel.Height = 90;
            this.headerPanel.BackColor = Color.White;
            this.headerPanel.Padding = new Padding(25, 15, 25, 10);

            // Content Panel (Edición)
            this.contentPanel.Dock = DockStyle.Fill;
            this.contentPanel.Padding = new Padding(0, 10, 0, 10);

            // Footer Panel (Edición)
            this.footerPanel.Dock = DockStyle.Bottom;
            this.footerPanel.Height = 70;
            this.footerPanel.Padding = new Padding(10);

            // Título (Edición)
            this.lblTitulo.Text = "✏️ EDITAR RUTINA";
            this.lblTitulo.Dock = DockStyle.Top;
            this.lblTitulo.Height = 35;
            this.lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            this.lblTitulo.TextAlign = ContentAlignment.MiddleLeft;

            // Descripción (Edición)
            this.lblDescripcion.Text = "Modifica los ejercicios, series, repeticiones...";
            this.lblDescripcion.Dock = DockStyle.Top;
            this.lblDescripcion.Height = 25;
            this.lblDescripcion.Font = new Font("Segoe UI", 9);
            this.lblDescripcion.ForeColor = Color.FromArgb(100, 100, 100);
            this.lblDescripcion.TextAlign = ContentAlignment.MiddleLeft;

            // Panel de Acciones (Agregar, Eliminar, Limpiar)
            this.panelAcciones.Dock = DockStyle.Top;
            this.panelAcciones.Height = 45;
            this.panelAcciones.Padding = new Padding(0, 0, 0, 5);

            // Botones de Acción (Edición)
            this.btnAgregarEjercicio.Text = "➕ AGREGAR";
            this.btnAgregarEjercicio.Size = new Size(120, 35);
            this.btnAgregarEjercicio.Location = new Point(0, 0);
            this.btnEliminarEjercicio.Text = "➖ ELIMINAR";
            this.btnEliminarEjercicio.Size = new Size(120, 35);
            this.btnEliminarEjercicio.Location = new Point(125, 0);
            this.btnLimpiarTodo.Text = "🧹 LIMPIAR";
            this.btnLimpiarTodo.Size = new Size(120, 35);
            this.btnLimpiarTodo.Location = new Point(250, 0);

            // DataGridView (Edición)
            this.dgvRutinas.Dock = DockStyle.Fill;
            this.dgvRutinas.Margin = new Padding(0, 5, 0, 0);
            dgvRutinas.Columns.Add("Ejercicio", "EJERCICIO");
            dgvRutinas.Columns.Add("Series", "SERIES");
            dgvRutinas.Columns.Add("Repeticiones", "REPETICIONES");
            dgvRutinas.Columns.Add("Carga", "CARGA (%)");
            dgvRutinas.Columns["Ejercicio"].FillWeight = 40;
            dgvRutinas.Columns["Series"].FillWeight = 15;
            dgvRutinas.Columns["Repeticiones"].FillWeight = 15;
            dgvRutinas.Columns["Carga"].FillWeight = 20;

            // Botón Guardar (Edición)
            this.btnGuardar.Text = "💾 GUARDAR CAMBIOS";
            this.btnGuardar.Size = new Size(200, 45);
            this.btnGuardar.Anchor = AnchorStyles.None;

            // Botón Volver (NUEVO - Edición)
            this.btnVolver.Text = "VOLVER A SELECCIÓN";
            this.btnVolver.Size = new Size(180, 45);
            this.btnVolver.Dock = DockStyle.Left;

            // Panel para centrar botón Guardar (Edición)
            centerPanelEdicion.Dock = DockStyle.Fill;
            centerPanelEdicion.Controls.Add(btnGuardar);

            // Ensamblar Panel de Edición
            this.panelAcciones.Controls.AddRange(new Control[] { btnAgregarEjercicio, btnEliminarEjercicio, btnLimpiarTodo });
            this.headerPanel.Controls.AddRange(new Control[] { lblDescripcion, lblTitulo });
            this.contentPanel.Controls.AddRange(new Control[] { dgvRutinas, panelAcciones });
            this.footerPanel.Controls.Add(centerPanelEdicion);
            this.footerPanel.Controls.Add(btnVolver); // Añade el botón Volver
            this.pnlEdicion.Controls.AddRange(new Control[] { contentPanel, footerPanel, headerPanel });

            // =========================================================
            // ENSAMBLAR EL USER CONTROL
            // =========================================================
            // Agregamos AMBOS paneles al UserControl.
            // La lógica en UcEditarRutina.cs controlará cuál es visible.
            this.Controls.Add(pnlEdicion);
            this.Controls.Add(pnlSeleccion);

            // =========================================================
            // ASIGNAR EVENTOS
            // =========================================================
            // Eventos del Panel de Edición
            this.btnGuardar.Click += new EventHandler(btnGuardar_Click);
            this.btnAgregarEjercicio.Click += new EventHandler(btnAgregarEjercicio_Click);
            this.btnEliminarEjercicio.Click += new EventHandler(btnEliminarEjercicio_Click);
            this.btnLimpiarTodo.Click += new EventHandler(btnLimpiarTodo_Click);
            this.btnVolver.Click += new EventHandler(btnVolver_Click); // Nuevo
            this.Load += new EventHandler(UcEditarRutina_Load);

            // Eventos del Panel de Selección (Nuevos)
            this.btnSeleccionarRutinaHombre.Click += new EventHandler(btnSeleccionarRutinaHombre_Click);
            this.btnSeleccionarRutinaMujer.Click += new EventHandler(btnSeleccionarRutinaMujer_Click);
            this.btnSeleccionarRutinaDeportista.Click += new EventHandler(btnSeleccionarRutinaDeportista_Click);

            // Centrar botón Guardar (Edición)
            this.Load += (sender, e) => {
                btnGuardar.Location = new Point((centerPanelEdicion.Width - btnGuardar.Width) / 2, (centerPanelEdicion.Height - btnGuardar.Height) / 2);
            };

            this.ResumeLayout(false);
        }
    }
}