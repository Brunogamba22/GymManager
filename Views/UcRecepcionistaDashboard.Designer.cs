using System.Windows.Forms;
using System.Drawing;

namespace GymManager.Views
{
    partial class UcRecepcionistaDashboard
    {
        private System.ComponentModel.IContainer components = null;

        // --- Declaraciones (Mantener esto) ---
        private Panel mainPanel;
        private Panel pnlControles;
        private DataGridView dgvPlanillas;

        private Label lblFiltroFecha;
        private DateTimePicker dtpFecha;
        private CheckBox chkTodasLasFechas;
        private Label lblFiltroProfesor;
        private ComboBox cmbProfesor;
        private Label lblFiltroGenero;
        private ComboBox cmbGenero;
        private Button btnFiltrar;
        private Button btnImprimir;
        private Button btnExportar;
        private Button btnModoTV;
        private CheckBox chkSoloEditadas;
        private Button btnLimpiarFiltros;

        private DataGridViewTextBoxColumn colNombre;
        private DataGridViewTextBoxColumn colProfesor;
        private DataGridViewTextBoxColumn colFecha;
        private DataGridViewTextBoxColumn colGenero;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // Inicialización de Controles
            this.mainPanel = new Panel();
            this.pnlControles = new Panel();
            this.dgvPlanillas = new DataGridView();

            this.lblFiltroFecha = new Label();
            this.dtpFecha = new DateTimePicker();
            this.chkTodasLasFechas = new CheckBox();
            this.lblFiltroProfesor = new Label();
            this.cmbProfesor = new ComboBox();
            this.lblFiltroGenero = new Label();
            this.cmbGenero = new ComboBox();
            this.btnFiltrar = new Button();
            this.btnImprimir = new Button();
            this.btnExportar = new Button();
            this.btnModoTV = new Button();
            this.chkSoloEditadas = new CheckBox();
            this.btnLimpiarFiltros = new Button();

            this.colNombre = new DataGridViewTextBoxColumn();
            this.colProfesor = new DataGridViewTextBoxColumn();
            this.colFecha = new DataGridViewTextBoxColumn();
            this.colGenero = new DataGridViewTextBoxColumn();

            ((System.ComponentModel.ISupportInitialize)(this.dgvPlanillas)).BeginInit();
            this.mainPanel.SuspendLayout();
            this.pnlControles.SuspendLayout();
            this.SuspendLayout();

            // =========================================================
            // Panel de Controles (Superior) - ALINEACIÓN FINAL
            // =========================================================
            this.pnlControles.Dock = DockStyle.Top;
            this.pnlControles.Height = 100;
            this.pnlControles.Padding = new Padding(20, 10, 20, 10);
            this.pnlControles.BackColor = Color.White;
            this.pnlControles.Font = new Font("Segoe UI", 9f);

            int spacing = 15;
            int topRowY = 10;       // Fila 1: Género y Acciones
            int bottomRowY = 50;    // Fila 2: Día/Profesor y Búsqueda
            int currentLeft;
            int buttonHeight = 35;
            int buttonActionY = 5; // Posición vertical más alta para los botones de acción

            // --- Fila 1 (Superior): Género, Checkbox Editadas y Botones de Acción ---
            currentLeft = 20;

            // Género:
            this.lblFiltroGenero.Text = "Género:";
            this.lblFiltroGenero.AutoSize = true;
            this.lblFiltroGenero.Location = new Point(currentLeft, topRowY + 3);
            currentLeft = lblFiltroGenero.Right + 6;

            this.cmbGenero.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbGenero.Size = new Size(110, 23);
            this.cmbGenero.Location = new Point(currentLeft, topRowY);
            currentLeft = cmbGenero.Right + spacing;

            // Checkbox Solo Editadas:
            this.chkSoloEditadas.Text = "Mostrar solo editadas";
            this.chkSoloEditadas.AutoSize = true;
            this.chkSoloEditadas.Location = new Point(currentLeft, topRowY + 3);

            // --- Fila 2 (Inferior): Día, Profesor y Botones de Búsqueda ---
            currentLeft = 20; // Reiniciamos a la izquierda para la segunda fila

            // Día:
            this.lblFiltroFecha.Text = "Día:";
            this.lblFiltroFecha.AutoSize = true;
            this.lblFiltroFecha.Location = new Point(currentLeft, bottomRowY + 3);
            currentLeft = lblFiltroFecha.Right + 6;

            this.dtpFecha.Format = DateTimePickerFormat.Short;
            this.dtpFecha.Size = new Size(110, 23);
            this.dtpFecha.Location = new Point(currentLeft, bottomRowY);
            currentLeft = dtpFecha.Right + 5;

            // Checkbox Todas las Fechas:
            this.chkTodasLasFechas.Text = "Todas las fechas";
            this.chkTodasLasFechas.AutoSize = true;
            this.chkTodasLasFechas.Location = new Point(currentLeft, bottomRowY + 3);
            currentLeft = chkTodasLasFechas.Right + spacing;


            // Profesor:
            this.lblFiltroProfesor.Text = "Profesor:";
            this.lblFiltroProfesor.AutoSize = true;
            this.lblFiltroProfesor.Location = new Point(currentLeft, bottomRowY + 3);
            currentLeft = lblFiltroProfesor.Right + 6;

            this.cmbProfesor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbProfesor.Size = new Size(170, 23);
            this.cmbProfesor.Location = new Point(currentLeft, bottomRowY);
            currentLeft = cmbProfesor.Right + spacing * 2;


            // Botón Filtrar
            this.btnFiltrar.Text = "FILTRAR";
            this.btnFiltrar.Size = new Size(110, buttonHeight);
            this.btnFiltrar.Location = new Point(currentLeft, bottomRowY - 3);
            currentLeft = btnFiltrar.Right + 8;

            // Botón Limpiar Filtros
            this.btnLimpiarFiltros.Text = "LIMPIAR";
            this.btnLimpiarFiltros.Size = new Size(110, buttonHeight);
            this.btnLimpiarFiltros.Location = new Point(currentLeft, bottomRowY - 3);


            // --- Botones de Acción (Alineados a la derecha en la Fila 1) ---

            this.btnModoTV.Text = "MODO TV";
            this.btnModoTV.Size = new Size(110, buttonHeight);
            this.btnModoTV.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.btnExportar.Text = "EXPORTAR";
            this.btnExportar.Size = new Size(110, buttonHeight);
            this.btnExportar.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.btnImprimir.Text = "IMPRIMIR";
            this.btnImprimir.Size = new Size(110, buttonHeight);
            this.btnImprimir.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // Agregar Controles
            this.pnlControles.Controls.AddRange(new Control[] {
                // Fila 1 - Izquierda
                lblFiltroGenero, cmbGenero, chkSoloEditadas, 
                
                // Fila 1 - Derecha (posicionados por Resize)
                btnImprimir, btnExportar, btnModoTV, 

                // Fila 2 - Izquierda a Derecha
                lblFiltroFecha, dtpFecha, chkTodasLasFechas,
                lblFiltroProfesor, cmbProfesor,
                btnFiltrar, btnLimpiarFiltros
            });

            // Lógica de Posicionamiento de Botones de Acción (Fila 1)
            this.pnlControles.Resize += (sender, e) => {
                int rightMargin = 20;
                int buttonY = buttonActionY; // Posicionado más alto (5)
                int buttonSpacing = 8;

                int currentRight = pnlControles.Width - rightMargin;

                // 1. MODO TV
                btnModoTV.Location = new Point(currentRight - btnModoTV.Width, buttonY);
                currentRight = btnModoTV.Left - buttonSpacing;

                // 2. EXPORTAR 
                btnExportar.Location = new Point(currentRight - btnExportar.Width, buttonY);
                currentRight = btnExportar.Left - buttonSpacing;

                // 3. IMPRIMIR
                btnImprimir.Location = new Point(currentRight - btnImprimir.Width, buttonY);
            };

            // Inicializamos la posición de los botones al cargar el componente
            this.pnlControles.SizeChanged += (sender, e) => {
                if (pnlControles.Width > 0)
                {
                    int rightMargin = 20;
                    int buttonY = buttonActionY;
                    int buttonSpacing = 8;

                    int currentRight = pnlControles.Width - rightMargin;

                    btnModoTV.Location = new Point(currentRight - btnModoTV.Width, buttonY);
                    currentRight = btnModoTV.Left - buttonSpacing;

                    btnExportar.Location = new Point(currentRight - btnExportar.Width, buttonY);
                    currentRight = btnExportar.Left - buttonSpacing;

                    btnImprimir.Location = new Point(currentRight - btnImprimir.Width, buttonY);
                }
            };


            // =========================================================
            // DataGridView (Principal)
            // =========================================================
            this.dgvPlanillas.Dock = DockStyle.Fill;
            this.dgvPlanillas.Margin = new Padding(0);

            // ... (Resto de la configuración del DataGridView sin cambios)

            // Columnas
            this.colNombre.HeaderText = "NOMBRE DE LA RUTINA";
            this.colNombre.Name = "colNombre";
            this.colNombre.FillWeight = 35;

            this.colProfesor.HeaderText = "PROFESOR";
            this.colProfesor.Name = "colProfesor";
            this.colProfesor.FillWeight = 25;

            this.colGenero.HeaderText = "GÉNERO";
            this.colGenero.Name = "colGenero";
            this.colGenero.FillWeight = 20;

            this.colFecha.HeaderText = "FECHA CREACIÓN";
            this.colFecha.Name = "colFecha";
            this.colFecha.FillWeight = 20;

            this.dgvPlanillas.Columns.AddRange(new DataGridViewColumn[] {
                this.colNombre, this.colProfesor, this.colGenero, this.colFecha
            });

            // =========================================================
            // Ensamblado Final
            // =========================================================
            this.BackColor = Color.FromArgb(245, 247, 250);

            this.mainPanel.Dock = DockStyle.Fill;
            this.mainPanel.Controls.Add(this.dgvPlanillas);
            this.mainPanel.Controls.Add(this.pnlControles);

            this.Controls.Add(this.mainPanel);

            ((System.ComponentModel.ISupportInitialize)(this.dgvPlanillas)).EndInit();
            this.mainPanel.ResumeLayout(false);
            this.pnlControles.ResumeLayout(false);
            this.pnlControles.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}