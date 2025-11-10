using System.Windows.Forms;
using System.Drawing;

namespace GymManager.Views
{
    partial class UcPlanillasRutinas
    {
        private System.ComponentModel.IContainer components = null;

        // --- Controles Principales ---
        private Panel mainPanel;
        private Panel headerPanel;
        private Panel contentPanel;
        private Panel pnlFiltros;
        private DataGridView dgvPlanillas;
        private CheckBox chkSoloEditadas;

        // --- Controles de Header ---
        private Label lblTitulo;
        private Label lblDescripcion;

        // --- Controles de Filtro ---
        private Label lblFiltroFechaDesde;
        private DateTimePicker dtpFechaDesde;
        private Label lblFiltroFechaHasta;
        private DateTimePicker dtpFechaHasta;
        private Label lblFiltroGenero;
        private ComboBox cmbGenero;
        private Button btnFiltrar;
        private Button btnLimpiarFiltros;

        // --- Controles de Footer ---
        private Panel footerPanel;
        private Button btnExportar;
        

        // --- Columnas del DataGridView ---
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colProfesor;
        private DataGridViewTextBoxColumn colGenero;
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
            this.pnlFiltros = new Panel();
            this.dgvPlanillas = new DataGridView();
            this.footerPanel = new Panel();

            this.lblTitulo = new Label();
            this.lblDescripcion = new Label();

            this.lblFiltroFechaDesde = new Label();
            this.dtpFechaDesde = new DateTimePicker();
            this.lblFiltroFechaHasta = new Label();
            this.dtpFechaHasta = new DateTimePicker();
            this.lblFiltroGenero = new Label();
            this.cmbGenero = new ComboBox();
            this.btnFiltrar = new Button();
            this.btnLimpiarFiltros = new Button();

            this.btnExportar = new Button();
            

            this.colNombre = new DataGridViewTextBoxColumn();
            this.colProfesor = new DataGridViewTextBoxColumn();
            this.colGenero = new DataGridViewTextBoxColumn();
            this.colFecha = new DataGridViewTextBoxColumn();

            this.SuspendLayout();

            // MAIN PANEL
            this.mainPanel.Dock = DockStyle.Fill;
            this.mainPanel.BackColor = Color.FromArgb(248, 249, 250);
            this.mainPanel.Padding = new Padding(20);

            // HEADER PANEL
            this.headerPanel.Dock = DockStyle.Top;
            this.headerPanel.Height = 90;
            this.headerPanel.BackColor = Color.White;
            this.headerPanel.Padding = new Padding(25, 15, 25, 10);

            // TÍTULO
            this.lblTitulo.Text = "📊 HISTORIAL DE PLANILLAS";
            this.lblTitulo.Dock = DockStyle.Top;
            this.lblTitulo.Height = 35;
            this.lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            this.lblTitulo.ForeColor = Color.FromArgb(46, 134, 171);
            this.lblTitulo.TextAlign = ContentAlignment.MiddleLeft;

            // DESCRIPCIÓN
            this.lblDescripcion.Text = "Busca y selecciona una planilla para ver sus detalles.";
            this.lblDescripcion.Dock = DockStyle.Top;
            this.lblDescripcion.Height = 25;
            this.lblDescripcion.Font = new Font("Segoe UI", 9.5f);
            this.lblDescripcion.ForeColor = Color.FromArgb(100, 100, 100);
            this.lblDescripcion.TextAlign = ContentAlignment.MiddleLeft;

            // CONTENT PANEL
            this.contentPanel.Dock = DockStyle.Fill;
            this.contentPanel.BackColor = Color.Transparent;

            // PANEL FILTROS
            this.pnlFiltros.Dock = DockStyle.Top;
            this.pnlFiltros.Height = 55;
            this.pnlFiltros.Padding = new Padding(0, 10, 0, 10);
            this.pnlFiltros.BackColor = Color.White;

            int currentLeft = 15;
            int topBase = 15;

            // --- FILTRO FECHA DESDE ---
            lblFiltroFechaDesde.Text = "Desde:";
            lblFiltroFechaDesde.AutoSize = true;
            pnlFiltros.Controls.Add(lblFiltroFechaDesde);
            lblFiltroFechaDesde.Location = new Point(currentLeft, topBase + 4);

            dtpFechaDesde.Format = DateTimePickerFormat.Short;
            dtpFechaDesde.Size = new Size(110, 25);
            pnlFiltros.Controls.Add(dtpFechaDesde);
            dtpFechaDesde.Location = new Point(lblFiltroFechaDesde.Right + 5, topBase);

            currentLeft = dtpFechaDesde.Right + 20;

            // --- FILTRO FECHA HASTA ---
            lblFiltroFechaHasta.Text = "Hasta:";
            lblFiltroFechaHasta.AutoSize = true;
            pnlFiltros.Controls.Add(lblFiltroFechaHasta);
            lblFiltroFechaHasta.Location = new Point(currentLeft, topBase + 4);

            dtpFechaHasta.Format = DateTimePickerFormat.Short;
            dtpFechaHasta.Size = new Size(110, 25);
            pnlFiltros.Controls.Add(dtpFechaHasta);
            dtpFechaHasta.Location = new Point(lblFiltroFechaHasta.Right + 5, topBase);

            currentLeft = dtpFechaHasta.Right + 20;

            // --- GÉNERO ---
            lblFiltroGenero.Text = "Género:";
            lblFiltroGenero.AutoSize = true;
            pnlFiltros.Controls.Add(lblFiltroGenero);
            lblFiltroGenero.Location = new Point(currentLeft, topBase + 4);

            cmbGenero.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGenero.Size = new Size(130, 25);
            pnlFiltros.Controls.Add(cmbGenero);
            cmbGenero.Location = new Point(lblFiltroGenero.Right + 5, topBase);

            currentLeft = cmbGenero.Right + 20;

            // --- CHECK SOLO EDITADAS ---
            chkSoloEditadas = new CheckBox();
            chkSoloEditadas.Text = "Mostrar solo editadas";
            chkSoloEditadas.AutoSize = true;
            chkSoloEditadas.Font = new Font("Segoe UI", 9);
            chkSoloEditadas.ForeColor = Color.FromArgb(33, 37, 41);
            chkSoloEditadas.Cursor = Cursors.Hand;
            pnlFiltros.Controls.Add(chkSoloEditadas);
            chkSoloEditadas.Location = new Point(currentLeft, topBase + 3);

            currentLeft = chkSoloEditadas.Right + 20;

            // --- BOTONES DE FILTRO ---
            btnFiltrar.Text = "🔍 FILTRAR";
            btnFiltrar.Size = new Size(120, 35);
            btnFiltrar.Location = new Point(currentLeft, topBase - 2);
            pnlFiltros.Controls.Add(btnFiltrar);

            currentLeft = btnFiltrar.Right + 10;

            btnLimpiarFiltros.Text = "🧹 LIMPIAR";
            btnLimpiarFiltros.Size = new Size(120, 35);
            btnLimpiarFiltros.Location = new Point(currentLeft, topBase - 2);
            pnlFiltros.Controls.Add(btnLimpiarFiltros);

            // --- DATAGRIDVIEW ---
            dgvPlanillas.Dock = DockStyle.Fill;
            dgvPlanillas.Margin = new Padding(0, 5, 0, 0);

            colNombre.HeaderText = "NOMBRE DE LA RUTINA";
            colNombre.Name = "colNombre";
            colNombre.FillWeight = 35;

            colProfesor.HeaderText = "PROFESOR";
            colProfesor.Name = "colProfesor";
            colProfesor.FillWeight = 25;

            colGenero.HeaderText = "GÉNERO";
            colGenero.Name = "colGenero";
            colGenero.FillWeight = 20;

            colFecha.HeaderText = "FECHA CREACIÓN";
            colFecha.Name = "colFecha";
            colFecha.FillWeight = 20;

            dgvPlanillas.Columns.AddRange(new DataGridViewColumn[]
            {
                colNombre,
                colProfesor,
                colGenero,
                colFecha
            });

            // --- FOOTER PANEL ---
            footerPanel.Dock = DockStyle.Bottom;
            footerPanel.Height = 70;
            footerPanel.BackColor = Color.Transparent;
            footerPanel.Padding = new Padding(20, 10, 25, 10);

            // --- BOTÓN EXPORTAR ---
            btnExportar.Text = "📤 EXPORTAR";
            btnExportar.Size = new Size(150, 40);
            btnExportar.BackColor = Color.FromArgb(41, 128, 185);
            btnExportar.ForeColor = Color.White;
            btnExportar.FlatStyle = FlatStyle.Flat;
            btnExportar.FlatAppearance.BorderSize = 0;
            btnExportar.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnExportar.Cursor = Cursors.Hand;

            

            
            // --- ARMADO ---
           
            footerPanel.Controls.Add(btnExportar);


            // 📏 Reposicionar dinámicamente los botones
            footerPanel.Resize += (s, e) =>
            {
                int bottom = 10;

                // 📍 Centrar horizontalmente el botón Exportar
                btnExportar.Location = new Point(
                    (footerPanel.Width - btnExportar.Width) / 2,
                    bottom
                );
            };

            // =========================================================
            // 🔧 ENSAMBLADO DE LA ESTRUCTURA VISUAL
            // =========================================================

            // 1️⃣ Agregar los subpaneles a sus contenedores
            contentPanel.Controls.Add(dgvPlanillas);
            contentPanel.Controls.Add(pnlFiltros);

            headerPanel.Controls.Add(lblDescripcion);
            headerPanel.Controls.Add(lblTitulo);

            // 2️⃣ Agregar secciones principales al mainPanel
            mainPanel.Controls.Add(contentPanel);
            mainPanel.Controls.Add(footerPanel);
            mainPanel.Controls.Add(headerPanel);

            // 3️⃣ Agregar el mainPanel al UserControl
            this.Controls.Add(mainPanel);

            // 4️⃣ Configuración general del UserControl
            this.Dock = DockStyle.Fill;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.Font = new Font("Segoe UI", 9);

            // =========================================================
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
