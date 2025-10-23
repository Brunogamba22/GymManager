using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Views
{
    partial class UcEditarRutina
    {
        private System.ComponentModel.IContainer components = null;

        private Panel mainPanel;
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

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.mainPanel = new Panel();
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
            var centerPanel = new Panel();

            // ((System.ComponentModel.ISupportInitialize)(this.dgvRutinas)).BeginInit(); // Si usas el diseñador visual, esto puede estar
            this.SuspendLayout();
            this.Size = new Size(900, 600);

            // Main Panel
            this.mainPanel.Dock = DockStyle.Fill;
            this.mainPanel.Padding = new Padding(20);

            // Header Panel
            this.headerPanel.Dock = DockStyle.Top;
            this.headerPanel.Height = 90;
            this.headerPanel.BackColor = Color.White;
            this.headerPanel.Padding = new Padding(25, 15, 25, 10);

            // Content Panel
            this.contentPanel.Dock = DockStyle.Fill;
            this.contentPanel.Padding = new Padding(0, 10, 0, 10);

            // Footer Panel
            this.footerPanel.Dock = DockStyle.Bottom;
            this.footerPanel.Height = 70;

            // Título
            this.lblTitulo.Text = "✏️ EDITAR RUTINA";
            this.lblTitulo.Dock = DockStyle.Top;
            this.lblTitulo.Height = 35;
            this.lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            this.lblTitulo.TextAlign = ContentAlignment.MiddleLeft;

            // Descripción
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

            // Botones de Acción
            this.btnAgregarEjercicio.Text = "➕ AGREGAR";
            this.btnAgregarEjercicio.Size = new Size(120, 35);
            this.btnAgregarEjercicio.Location = new Point(0, 0);
            this.btnEliminarEjercicio.Text = "➖ ELIMINAR";
            this.btnEliminarEjercicio.Size = new Size(120, 35);
            this.btnEliminarEjercicio.Location = new Point(125, 0);
            this.btnLimpiarTodo.Text = "🧹 LIMPIAR";
            this.btnLimpiarTodo.Size = new Size(120, 35);
            this.btnLimpiarTodo.Location = new Point(250, 0);

            // DataGridView
            this.dgvRutinas.Dock = DockStyle.Fill;
            this.dgvRutinas.Margin = new Padding(0, 5, 0, 0);
            // Columnas se definen aquí
            dgvRutinas.Columns.Add("Ejercicio", "EJERCICIO");
            dgvRutinas.Columns.Add("Series", "SERIES");
            dgvRutinas.Columns.Add("Repeticiones", "REPETICIONES");
            dgvRutinas.Columns.Add("Descanso", "DESCANSO (s)");
            dgvRutinas.Columns["Ejercicio"].FillWeight = 40;
            dgvRutinas.Columns["Series"].FillWeight = 20;
            dgvRutinas.Columns["Repeticiones"].FillWeight = 20;
            dgvRutinas.Columns["Descanso"].FillWeight = 20;

            // Botón Guardar
            this.btnGuardar.Text = "💾 GUARDAR CAMBIOS";
            this.btnGuardar.Size = new Size(200, 45);
            this.btnGuardar.Anchor = AnchorStyles.None;

            // Ensamblar controles
            this.panelAcciones.Controls.AddRange(new Control[] { btnAgregarEjercicio, btnEliminarEjercicio, btnLimpiarTodo });
            this.headerPanel.Controls.AddRange(new Control[] { lblDescripcion, lblTitulo });
            this.contentPanel.Controls.AddRange(new Control[] { dgvRutinas, panelAcciones });
            centerPanel.Dock = DockStyle.Fill;
            centerPanel.Controls.Add(btnGuardar);
            this.footerPanel.Controls.Add(centerPanel);
            this.mainPanel.Controls.AddRange(new Control[] { contentPanel, footerPanel, headerPanel });
            this.Controls.Add(mainPanel);

            // Asignar Eventos (SOLO LA ASIGNACIÓN +=)
            this.btnGuardar.Click += new EventHandler(btnGuardar_Click);
            this.btnAgregarEjercicio.Click += new EventHandler(btnAgregarEjercicio_Click);
            this.btnEliminarEjercicio.Click += new EventHandler(btnEliminarEjercicio_Click);
            this.btnLimpiarTodo.Click += new EventHandler(btnLimpiarTodo_Click);
            this.Load += new EventHandler(UcEditarRutina_Load);

            // Centrar botón Guardar después del layout
            this.Load += (sender, e) => {
                btnGuardar.Location = new Point((centerPanel.Width - btnGuardar.Width) / 2, (centerPanel.Height - btnGuardar.Height) / 2);
            };

            // ((System.ComponentModel.ISupportInitialize)(this.dgvRutinas)).EndInit(); // Si usas el diseñador visual
            this.ResumeLayout(false);
        }

        // NO HAY IMPLEMENTACIÓN DE MÉTODOS AQUÍ
    }
}