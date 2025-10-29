using System.Windows.Forms;
using System.Drawing;

namespace GymManager.Views
{
    partial class UcPlanillasRutinas
    {
        private System.ComponentModel.IContainer components = null;

        // --- Controles Principales ---
        private Panel mainPanel;        // Panel base
        private Panel headerPanel;      // Título y Descripción
        private Panel contentPanel;     // Contiene Filtros y Grid
        private Panel pnlFiltros;       // Panel para los controles de filtro
        private DataGridView dgvPlanillas; // Grilla de resultados

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
        private Button btnLimpiarFiltros; // Botón nuevo

        // --- Controles de Footer (Botón Exportar) ---
        private Panel footerPanel;      // Panel para el botón Exportar
        private Button btnExportar;

        // --- Columnas del DataGridView ---
        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colProfesor;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colGenero; // Columna nueva


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
            this.colFecha = new DataGridViewTextBoxColumn();
            this.colGenero = new DataGridViewTextBoxColumn(); // Nueva

            this.SuspendLayout();

            // Main Panel
            this.mainPanel.Dock = DockStyle.Fill;
            this.mainPanel.BackColor = Color.FromArgb(248, 249, 250); // backgroundColor
            this.mainPanel.Padding = new Padding(20);

            // Header Panel
            this.headerPanel.Dock = DockStyle.Top;
            this.headerPanel.Height = 90; // Más compacto
            this.headerPanel.BackColor = Color.White;
            this.headerPanel.Padding = new Padding(25, 15, 25, 10); // Menos padding abajo

            // Título
            this.lblTitulo.Text = "📊 HISTORIAL DE PLANILLAS";
            this.lblTitulo.Dock = DockStyle.Top;
            this.lblTitulo.Height = 35; // Ajustado
            this.lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            this.lblTitulo.ForeColor = Color.FromArgb(46, 134, 171); // primaryColor
            this.lblTitulo.TextAlign = ContentAlignment.MiddleLeft;

            // Descripción
            this.lblDescripcion.Text = "Busca y selecciona una planilla para ver sus detalles.";
            this.lblDescripcion.Dock = DockStyle.Top;
            this.lblDescripcion.Height = 25; // Ajustado
            this.lblDescripcion.Font = new Font("Segoe UI", 9.5f);
            this.lblDescripcion.ForeColor = Color.FromArgb(100, 100, 100);
            this.lblDescripcion.TextAlign = ContentAlignment.MiddleLeft;

            // Content Panel (Filtros + Grid)
            this.contentPanel.Dock = DockStyle.Fill;
            this.contentPanel.BackColor = Color.Transparent;

            // Panel Filtros
            this.pnlFiltros.Dock = DockStyle.Top;
            this.pnlFiltros.Height = 55; // Altura para los filtros
            this.pnlFiltros.Padding = new Padding(0, 10, 0, 10);
            this.pnlFiltros.BackColor = Color.White; // Fondo blanco para filtros
                                                     // Controles de Filtro (alineación precisa)
            int currentLeft = 15;
            int topBase = 15;

            // === Fecha Desde ===
            lblFiltroFechaDesde.Text = "Desde:";
            lblFiltroFechaDesde.AutoSize = true;
            pnlFiltros.Controls.Add(lblFiltroFechaDesde);
            lblFiltroFechaDesde.Location = new Point(currentLeft, topBase + 4);

            dtpFechaDesde.Format = DateTimePickerFormat.Short;
            dtpFechaDesde.Size = new Size(110, 25);
            pnlFiltros.Controls.Add(dtpFechaDesde);
            dtpFechaDesde.Location = new Point(lblFiltroFechaDesde.Left + lblFiltroFechaDesde.PreferredWidth + 4, topBase);

            // Ajustamos el siguiente control
            currentLeft = dtpFechaDesde.Right + 20;

            // === Fecha Hasta ===
            lblFiltroFechaHasta.Text = "Hasta:";
            lblFiltroFechaHasta.AutoSize = true;
            pnlFiltros.Controls.Add(lblFiltroFechaHasta);
            lblFiltroFechaHasta.Location = new Point(currentLeft, topBase + 4);

            dtpFechaHasta.Format = DateTimePickerFormat.Short;
            dtpFechaHasta.Size = new Size(110, 25);
            pnlFiltros.Controls.Add(dtpFechaHasta);
            dtpFechaHasta.Location = new Point(lblFiltroFechaHasta.Left + lblFiltroFechaHasta.PreferredWidth + 4, topBase);

            currentLeft = dtpFechaHasta.Right + 20;

            // === Género ===
            lblFiltroGenero.Text = "Género:";
            lblFiltroGenero.AutoSize = true;
            pnlFiltros.Controls.Add(lblFiltroGenero);
            lblFiltroGenero.Location = new Point(currentLeft, topBase + 4);

            cmbGenero.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbGenero.Size = new Size(130, 25);
            pnlFiltros.Controls.Add(cmbGenero);
            cmbGenero.Location = new Point(lblFiltroGenero.Left + lblFiltroGenero.PreferredWidth + 4, topBase);

            currentLeft = cmbGenero.Right + 20;

            // === Botones ===
            btnFiltrar.Text = "🔍 FILTRAR";
            btnFiltrar.Size = new Size(120, 35);
            btnFiltrar.Location = new Point(currentLeft, topBase - 2);
            pnlFiltros.Controls.Add(btnFiltrar);

            currentLeft = btnFiltrar.Right + 10;

            btnLimpiarFiltros.Text = "🧹 LIMPIAR";
            btnLimpiarFiltros.Size = new Size(120, 35);
            btnLimpiarFiltros.Location = new Point(currentLeft, topBase - 2);
            pnlFiltros.Controls.Add(btnLimpiarFiltros);


            // Agregar controles al pnlFiltros
            this.pnlFiltros.Controls.Add(lblFiltroFechaDesde);
            this.pnlFiltros.Controls.Add(dtpFechaDesde);
            this.pnlFiltros.Controls.Add(lblFiltroFechaHasta);
            this.pnlFiltros.Controls.Add(dtpFechaHasta);
            this.pnlFiltros.Controls.Add(lblFiltroGenero);
            this.pnlFiltros.Controls.Add(cmbGenero);
            this.pnlFiltros.Controls.Add(btnFiltrar);
            this.pnlFiltros.Controls.Add(btnLimpiarFiltros);


            // DataGridView (debajo de filtros)
            this.dgvPlanillas.Dock = DockStyle.Fill;
            this.dgvPlanillas.Margin = new Padding(0, 5, 0, 0); // Espacio entre filtros y grid

            // Columnas del DataGridView
            this.colNombre.HeaderText = "NOMBRE DE LA RUTINA";
            this.colNombre.Name = "colNombre";
            this.colNombre.FillWeight = 35; // Ajustado

            this.colProfesor.HeaderText = "PROFESOR";
            this.colProfesor.Name = "colProfesor";
            this.colProfesor.FillWeight = 25; // Ajustado

            this.colFecha.HeaderText = "FECHA CREACIÓN";
            this.colFecha.Name = "colFecha";
            this.colFecha.FillWeight = 20; // Ajustado

            this.colGenero.HeaderText = "GÉNERO"; // Nueva
            this.colGenero.Name = "colGenero";
            this.colGenero.FillWeight = 20; // Nueva

            this.dgvPlanillas.Columns.AddRange(new DataGridViewColumn[] {
                this.colNombre,
                this.colProfesor,
                this.colGenero, // Nueva
                this.colFecha
            });

            // Footer Panel (para Exportar)
            this.footerPanel.Dock = DockStyle.Bottom;
            this.footerPanel.Height = 60; // Más compacto
            this.footerPanel.BackColor = Color.Transparent;
            this.footerPanel.Padding = new Padding(0, 0, 25, 10); // Padding para alinear botón

            // Botón Exportar
            this.btnExportar.Text = "📤 EXPORTAR";
            this.btnExportar.Size = new Size(150, 40); // Ajustado
            this.btnExportar.Anchor = AnchorStyles.Bottom | AnchorStyles.Right; // Anclado a la derecha

            // Ensamblar controles
            this.headerPanel.Controls.Add(lblDescripcion);
            this.headerPanel.Controls.Add(lblTitulo);

            this.contentPanel.Controls.Add(dgvPlanillas); // Grid primero (para que ocupe el espacio restante)
            this.contentPanel.Controls.Add(pnlFiltros);   // Filtros arriba

            this.footerPanel.Controls.Add(btnExportar);

            this.mainPanel.Controls.Add(contentPanel);
            this.mainPanel.Controls.Add(footerPanel);
            this.mainPanel.Controls.Add(headerPanel);

            this.Controls.Add(mainPanel);

            // Eventos (se asignarán en el .cs)

            this.ResumeLayout(false);
        }
    }
}