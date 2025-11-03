using System.Windows.Forms;
using System.Drawing;

namespace GymManager.Views
{
    partial class UcRecepcionistaDashboard
    {
        private System.ComponentModel.IContainer components = null;

        // --- Controles Principales ---
        private Panel mainPanel;
        private Panel pnlControles;
        private DataGridView dgvPlanillas;

        // --- Controles de Filtro ---
        private Label lblFiltroFecha;
        private DateTimePicker dtpFecha;
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

        // --- Columnas del DataGridView ---
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
            // Panel de Controles (Superior) - MEJORADO
            // =========================================================
            this.pnlControles.Dock = DockStyle.Top;
            this.pnlControles.Height = 90;
            this.pnlControles.Padding = new Padding(20, 15, 20, 15);
            this.pnlControles.BackColor = Color.White;
            this.pnlControles.Font = new Font("Segoe UI", 9f);

            int spacing = 12;
            int labelSpacing = 6;
            int topRowY = 12;
            int bottomRowY = 48;
            int currentLeft = 20;

            // --- Fila 1: Fecha y Profesor ---
            this.lblFiltroFecha.Text = "Día:";
            this.lblFiltroFecha.AutoSize = true;
            this.lblFiltroFecha.Location = new Point(currentLeft, topRowY + 3);
            this.lblFiltroFecha.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblFiltroFecha.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            currentLeft = lblFiltroFecha.Right + labelSpacing;

            this.dtpFecha.Format = DateTimePickerFormat.Short;
            this.dtpFecha.Size = new Size(115, 23);
            this.dtpFecha.Location = new Point(currentLeft, topRowY);
            this.dtpFecha.Font = new Font("Segoe UI", 9f);
            currentLeft = dtpFecha.Right + spacing;

            this.lblFiltroProfesor.Text = "Profesor:";
            this.lblFiltroProfesor.AutoSize = true;
            this.lblFiltroProfesor.Location = new Point(currentLeft, topRowY + 3);
            this.lblFiltroProfesor.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblFiltroProfesor.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            currentLeft = lblFiltroProfesor.Right + labelSpacing;

            this.cmbProfesor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbProfesor.Size = new Size(170, 23);
            this.cmbProfesor.Location = new Point(currentLeft, topRowY);
            this.cmbProfesor.Font = new Font("Segoe UI", 9f);
            this.cmbProfesor.FlatStyle = FlatStyle.Flat;
            currentLeft = cmbProfesor.Right + spacing;

            // --- Fila 2: Género, Checkbox y Botones ---
            currentLeft = 20;

            this.lblFiltroGenero.Text = "Género:";
            this.lblFiltroGenero.AutoSize = true;
            this.lblFiltroGenero.Location = new Point(currentLeft, bottomRowY + 3);
            this.lblFiltroGenero.ForeColor = Color.FromArgb(52, 73, 94);
            this.lblFiltroGenero.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            currentLeft = lblFiltroGenero.Right + labelSpacing;

            this.cmbGenero.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbGenero.Size = new Size(120, 23);
            this.cmbGenero.Location = new Point(currentLeft, bottomRowY);
            this.cmbGenero.Font = new Font("Segoe UI", 9f);
            this.cmbGenero.FlatStyle = FlatStyle.Flat;
            currentLeft = cmbGenero.Right + (spacing * 2);

            this.chkSoloEditadas.Text = "Mostrar solo editadas";
            this.chkSoloEditadas.AutoSize = true;
            this.chkSoloEditadas.Location = new Point(currentLeft, bottomRowY + 3);
            this.chkSoloEditadas.ForeColor = Color.FromArgb(52, 73, 94);
            this.chkSoloEditadas.Font = new Font("Segoe UI", 9f);
            currentLeft = chkSoloEditadas.Right + (spacing * 2);

            // Botón Filtrar
            this.btnFiltrar.Text = "🔍 FILTRAR";
            this.btnFiltrar.Size = new Size(120, 35);
            this.btnFiltrar.Location = new Point(currentLeft, bottomRowY - 3);
            currentLeft = btnFiltrar.Right + spacing;

            // Botón Limpiar Filtros
            this.btnLimpiarFiltros.Text = "🧹 LIMPIAR";
            this.btnLimpiarFiltros.Size = new Size(120, 35);
            this.btnLimpiarFiltros.Location = new Point(currentLeft, bottomRowY - 3);

            // --- Botones de Acción (Alineados a la derecha) ---
            this.btnImprimir.Text = "🖨️ IMPRIMIR";
            this.btnImprimir.Size = new Size(130, 35);
            this.btnImprimir.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.btnExportar.Text = "📤 EXPORTAR";
            this.btnExportar.Size = new Size(130, 35);
            this.btnExportar.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.btnModoTV.Text = "📺 MODO TV";
            this.btnModoTV.Size = new Size(140, 35);
            this.btnModoTV.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            // Agregar controles a pnlControles
            this.pnlControles.Controls.Add(lblFiltroFecha);
            this.pnlControles.Controls.Add(dtpFecha);
            this.pnlControles.Controls.Add(lblFiltroProfesor);
            this.pnlControles.Controls.Add(cmbProfesor);
            this.pnlControles.Controls.Add(lblFiltroGenero);
            this.pnlControles.Controls.Add(cmbGenero);
            this.pnlControles.Controls.Add(chkSoloEditadas);
            this.pnlControles.Controls.Add(btnFiltrar);
            this.pnlControles.Controls.Add(btnLimpiarFiltros);
            this.pnlControles.Controls.Add(btnImprimir);
            this.pnlControles.Controls.Add(btnExportar);
            this.pnlControles.Controls.Add(btnModoTV);

            // =========================================================
            // DataGridView (Principal) - MEJORADO
            // =========================================================
            this.dgvPlanillas.Dock = DockStyle.Fill;
            this.dgvPlanillas.Margin = new Padding(0, 5, 0, 0);
            this.dgvPlanillas.BackgroundColor = Color.White;
            this.dgvPlanillas.BorderStyle = BorderStyle.None;
            this.dgvPlanillas.GridColor = Color.FromArgb(224, 230, 237);
            this.dgvPlanillas.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            this.dgvPlanillas.EnableHeadersVisualStyles = false;
            this.dgvPlanillas.AllowUserToAddRows = false;
            this.dgvPlanillas.AllowUserToDeleteRows = false;
            this.dgvPlanillas.ReadOnly = true;
            this.dgvPlanillas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvPlanillas.RowHeadersVisible = false;
            this.dgvPlanillas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPlanillas.RowTemplate.Height = 50;
            this.dgvPlanillas.AllowUserToResizeColumns = false;
            this.dgvPlanillas.AllowUserToResizeRows = false;
            this.dgvPlanillas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Encabezados
            this.dgvPlanillas.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(41, 128, 185);
            this.dgvPlanillas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            this.dgvPlanillas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            this.dgvPlanillas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dgvPlanillas.ColumnHeadersHeight = 45;

            // Celdas
            this.dgvPlanillas.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
            this.dgvPlanillas.DefaultCellStyle.BackColor = Color.White;
            this.dgvPlanillas.DefaultCellStyle.ForeColor = Color.FromArgb(52, 73, 94);
            this.dgvPlanillas.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            this.dgvPlanillas.DefaultCellStyle.Padding = new Padding(8);
            this.dgvPlanillas.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 244, 253);
            this.dgvPlanillas.DefaultCellStyle.SelectionForeColor = Color.FromArgb(52, 73, 94);
            this.dgvPlanillas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);

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

            // Redimensionar botones de acción (derecha)
            this.pnlControles.Resize += (sender, e) => {
                btnModoTV.Location = new Point(pnlControles.Width - 160, 15);
                btnExportar.Location = new Point(btnModoTV.Left - 145, 15);
                btnImprimir.Location = new Point(btnExportar.Left - 145, 15);
            };

            ((System.ComponentModel.ISupportInitialize)(this.dgvPlanillas)).EndInit();
            this.mainPanel.ResumeLayout(false);
            this.pnlControles.ResumeLayout(false);
            this.pnlControles.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}