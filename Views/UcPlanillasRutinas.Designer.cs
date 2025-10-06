using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Views
{
    partial class UcPlanillasRutinas
    {
        private System.ComponentModel.IContainer components = null;

        private Panel mainPanel;
        private Panel headerPanel;
        private Panel contentPanel;
        private SplitContainer splitContainer;
        private DataGridView dgvPlanillas;
        private Panel panelDetalles;
        private Button btnExportar;
        private Label lblTitulo;
        private Label lblDescripcion;

        // Columnas del DataGridView
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colProfesor;
        private DataGridViewTextBoxColumn colFecha;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.mainPanel = new Panel();
            this.headerPanel = new Panel();
            this.contentPanel = new Panel();
            this.splitContainer = new SplitContainer();
            this.dgvPlanillas = new DataGridView();
            this.panelDetalles = new Panel();
            this.btnExportar = new Button();
            this.lblTitulo = new Label();
            this.lblDescripcion = new Label();

            this.SuspendLayout();
            this.Size = new Size(1000, 700);

            // Main Panel
            this.mainPanel.Dock = DockStyle.Fill;
            this.mainPanel.BackColor = Color.FromArgb(248, 249, 250);
            this.mainPanel.Padding = new Padding(20);

            // Header Panel
            this.headerPanel.Dock = DockStyle.Top;
            this.headerPanel.Height = 100;
            this.headerPanel.BackColor = Color.White;
            this.headerPanel.Padding = new Padding(25, 15, 25, 15);

            // Content Panel
            this.contentPanel.Dock = DockStyle.Fill;
            this.contentPanel.BackColor = Color.Transparent;

            // Split Container
            this.splitContainer.Dock = DockStyle.Fill;
            this.splitContainer.Orientation = Orientation.Horizontal;
            this.splitContainer.SplitterDistance = 250; // Altura de la lista
            this.splitContainer.SplitterWidth = 8;
            this.splitContainer.Panel1.BackColor = Color.Transparent;
            this.splitContainer.Panel2.BackColor = Color.Transparent;

            // DataGridView
            this.dgvPlanillas.Dock = DockStyle.Fill;
            this.dgvPlanillas.Margin = new Padding(0, 10, 0, 0);

            // Panel de Detalles
            this.panelDetalles.Dock = DockStyle.Fill;
            this.panelDetalles.AutoScroll = true;
            this.panelDetalles.Padding = new Padding(10);

            // Botón Exportar
            this.btnExportar.Text = "📤 EXPORTAR PLANILLA";
            this.btnExportar.Size = new Size(180, 45);
            this.btnExportar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;

            // Título
            this.lblTitulo.Text = "📊 HISTORIAL DE PLANILLAS";
            this.lblTitulo.Dock = DockStyle.Top;
            this.lblTitulo.Height = 40;
            this.lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            this.lblTitulo.ForeColor = Color.FromArgb(46, 134, 171);
            this.lblTitulo.TextAlign = ContentAlignment.MiddleLeft;

            // Descripción
            this.lblDescripcion.Text = "Selecciona una planilla para ver los detalles completos de las rutinas generadas.";
            this.lblDescripcion.Dock = DockStyle.Top;
            this.lblDescripcion.Height = 30;
            this.lblDescripcion.Font = new Font("Segoe UI", 9.5f);
            this.lblDescripcion.ForeColor = Color.FromArgb(100, 100, 100);
            this.lblDescripcion.TextAlign = ContentAlignment.MiddleLeft;

            // Configurar columnas del DataGridView
            this.colNombre = new DataGridViewTextBoxColumn();
            this.colNombre.HeaderText = "NOMBRE DE LA RUTINA";
            this.colNombre.Name = "colNombre";
            this.colNombre.FillWeight = 40;

            this.colProfesor = new DataGridViewTextBoxColumn();
            this.colProfesor.HeaderText = "PROFESOR";
            this.colProfesor.Name = "colProfesor";
            this.colProfesor.FillWeight = 30;

            this.colFecha = new DataGridViewTextBoxColumn();
            this.colFecha.HeaderText = "FECHA CREACIÓN";
            this.colFecha.Name = "colFecha";
            this.colFecha.FillWeight = 30;

            this.dgvPlanillas.Columns.AddRange(new DataGridViewColumn[] {
                this.colNombre,
                this.colProfesor,
                this.colFecha
            });

            // Agregar controles a los paneles
            this.headerPanel.Controls.Add(lblDescripcion);
            this.headerPanel.Controls.Add(lblTitulo);

            var panelLista = new Panel();
            panelLista.Dock = DockStyle.Fill;
            panelLista.Padding = new Padding(0, 10, 0, 0);
            panelLista.Controls.Add(dgvPlanillas);

            this.splitContainer.Panel1.Controls.Add(panelLista);
            this.splitContainer.Panel2.Controls.Add(panelDetalles);

            var panelContentWithButton = new Panel();
            panelContentWithButton.Dock = DockStyle.Fill;
            panelContentWithButton.Controls.Add(splitContainer);
            panelContentWithButton.Controls.Add(btnExportar);

            this.contentPanel.Controls.Add(panelContentWithButton);

            this.mainPanel.Controls.Add(contentPanel);
            this.mainPanel.Controls.Add(headerPanel);

            this.Controls.Add(mainPanel);

            // Eventos
            this.btnExportar.Click += new EventHandler(btnExportar_Click);

            // Aplicar estilos después de la inicialización
            this.Load += (sender, e) => {
                StyleButton(btnExportar, successColor);
                btnExportar.Location = new Point(
                    panelContentWithButton.Width - btnExportar.Width - 20,
                    panelContentWithButton.Height - btnExportar.Height - 20
                );
            };

            this.ResumeLayout(false);
        }
    }
}