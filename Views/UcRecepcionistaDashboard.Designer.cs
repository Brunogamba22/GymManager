using System.Windows.Forms;
using System.Drawing;

namespace GymManager.Views
{
    partial class UcRecepcionistaDashboard
    {
        private System.ComponentModel.IContainer components = null;

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
        private FlowLayoutPanel flpAcciones;

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
            this.flpAcciones = new FlowLayoutPanel();

            this.colNombre = new DataGridViewTextBoxColumn();
            this.colProfesor = new DataGridViewTextBoxColumn();
            this.colFecha = new DataGridViewTextBoxColumn();
            this.colGenero = new DataGridViewTextBoxColumn();

            ((System.ComponentModel.ISupportInitialize)(this.dgvPlanillas)).BeginInit();
            this.mainPanel.SuspendLayout();
            this.pnlControles.SuspendLayout();
            this.SuspendLayout();

            // =========================================================
            // PANEL DE CONTROLES
            // =========================================================
            this.pnlControles.Dock = DockStyle.Top;
            this.pnlControles.Height = 120;
            this.pnlControles.Padding = new Padding(20, 10, 20, 10);
            this.pnlControles.BackColor = Color.White;
            this.pnlControles.Font = new Font("Segoe UI", 9f);

            int spacing = 15;
            int topRowY = 12;
            int bottomRowY = 60;
            int currentLeft;
            int buttonHeight = 35;

            // --- Fila 1: Género + Solo Editadas ---
            currentLeft = 20;
            this.lblFiltroGenero.Text = "Género:";
            this.lblFiltroGenero.AutoSize = true;
            this.lblFiltroGenero.Location = new Point(currentLeft, topRowY + 3);
            currentLeft = lblFiltroGenero.Right + 6;

            this.cmbGenero.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbGenero.Size = new Size(110, 23);
            this.cmbGenero.Location = new Point(currentLeft, topRowY);
            currentLeft = cmbGenero.Right + spacing;

            this.chkSoloEditadas.Text = "Mostrar solo editadas";
            this.chkSoloEditadas.AutoSize = true;
            this.chkSoloEditadas.Location = new Point(currentLeft, topRowY + 3);

            // =========================================================
            // CONTENEDOR DERECHA: BOTONES ACCIÓN
            // =========================================================
            this.flpAcciones.AutoSize = true;
            this.flpAcciones.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            this.flpAcciones.FlowDirection = FlowDirection.RightToLeft;
            this.flpAcciones.WrapContents = false;
            this.flpAcciones.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            this.flpAcciones.Location = new Point(this.pnlControles.ClientSize.Width - 20, 8);
            this.flpAcciones.Margin = new Padding(0);
            this.flpAcciones.Padding = new Padding(0);

            // Botones
            this.btnModoTV.Text = "MODO TV";
            this.btnModoTV.AutoSize = true;
            this.btnModoTV.MinimumSize = new Size(110, buttonHeight);
            this.btnModoTV.Margin = new Padding(8, 0, 0, 0);

            this.btnExportar.Text = "EXPORTAR";
            this.btnExportar.AutoSize = true;
            this.btnExportar.MinimumSize = new Size(110, buttonHeight);
            this.btnExportar.Margin = new Padding(8, 0, 0, 0);

            this.btnImprimir.Text = "IMPRIMIR";
            this.btnImprimir.AutoSize = true;
            this.btnImprimir.MinimumSize = new Size(110, buttonHeight);
            this.btnImprimir.Margin = new Padding(8, 0, 0, 0);

            this.flpAcciones.Controls.Add(this.btnModoTV);
            this.flpAcciones.Controls.Add(this.btnExportar);
            this.flpAcciones.Controls.Add(this.btnImprimir);

            this.pnlControles.Resize += (s, e) =>
            {
                this.flpAcciones.Location = new Point(
                    this.pnlControles.ClientSize.Width - this.flpAcciones.Width - 20,
                    8
                );
            };

            // =========================================================
            // Fila 2: Día, Profesor, Botones Filtrar y Limpiar
            // =========================================================
            currentLeft = 20;
            this.lblFiltroFecha.Text = "Día:";
            this.lblFiltroFecha.AutoSize = true;
            this.lblFiltroFecha.Location = new Point(currentLeft, bottomRowY + 3);
            currentLeft = lblFiltroFecha.Right + 6;

            this.dtpFecha.Format = DateTimePickerFormat.Short;
            this.dtpFecha.Size = new Size(110, 23);
            this.dtpFecha.Location = new Point(currentLeft, bottomRowY);
            currentLeft = dtpFecha.Right + 5;

            this.chkTodasLasFechas.Text = "Todas las fechas";
            this.chkTodasLasFechas.AutoSize = true;
            this.chkTodasLasFechas.Location = new Point(currentLeft, bottomRowY + 3);
            currentLeft = chkTodasLasFechas.Right + spacing;

            this.lblFiltroProfesor.Text = "Profesor:";
            this.lblFiltroProfesor.AutoSize = true;
            this.lblFiltroProfesor.Location = new Point(currentLeft, bottomRowY + 3);
            currentLeft = lblFiltroProfesor.Right + 6;

            this.cmbProfesor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbProfesor.Size = new Size(170, 23);
            this.cmbProfesor.Location = new Point(currentLeft, bottomRowY);
            currentLeft = cmbProfesor.Right + spacing * 2;

            this.btnFiltrar.Text = "FILTRAR";
            this.btnFiltrar.Size = new Size(110, buttonHeight);
            this.btnFiltrar.Location = new Point(currentLeft, bottomRowY - 3);
            currentLeft = btnFiltrar.Right + 8;

            this.btnLimpiarFiltros.Text = "LIMPIAR";
            this.btnLimpiarFiltros.Size = new Size(110, buttonHeight);
            this.btnLimpiarFiltros.Location = new Point(currentLeft, bottomRowY - 3);

            // =========================================================
            // Agregar controles al panel
            // =========================================================
            this.pnlControles.Controls.AddRange(new Control[] {
                lblFiltroGenero, cmbGenero, chkSoloEditadas,
                flpAcciones,
                lblFiltroFecha, dtpFecha, chkTodasLasFechas,
                lblFiltroProfesor, cmbProfesor,
                btnFiltrar, btnLimpiarFiltros
            });

            // =========================================================
            // DATAGRIDVIEW
            // =========================================================
            this.dgvPlanillas.Dock = DockStyle.Fill;
            this.dgvPlanillas.Margin = new Padding(0);

            this.colNombre.HeaderText = "NOMBRE DE LA RUTINA";
            this.colProfesor.HeaderText = "PROFESOR";
            this.colGenero.HeaderText = "GÉNERO";
            this.colFecha.HeaderText = "FECHA CREACIÓN";

            this.dgvPlanillas.Columns.AddRange(new DataGridViewColumn[] {
                this.colNombre, this.colProfesor, this.colGenero, this.colFecha
            });

            // =========================================================
            // ENSAMBLADO FINAL
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
