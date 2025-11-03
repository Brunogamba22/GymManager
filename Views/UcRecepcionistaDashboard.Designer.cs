using System.Windows.Forms;
using System.Drawing;

namespace GymManager.Views
{
    partial class UcRecepcionistaDashboard
    {
        private System.ComponentModel.IContainer components = null;

        // --- Controles Principales ---
        private Panel mainPanel;        // Panel que contiene todo
        private Panel pnlControles;     // Panel para filtros y botones
        private DataGridView dgvPlanillas; // Grilla de resultados (Nombre correcto)

        // --- Controles de Filtro (Modo Interactivo) ---
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

        // --- Controles Opcionales ---
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
            this.dgvPlanillas = new DataGridView(); // Nombre corregido

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
            // Panel de Controles (Superior)
            // =========================================================
            this.pnlControles.Dock = DockStyle.Top;
            this.pnlControles.Height = 85;
            this.pnlControles.Padding = new Padding(15, 10, 15, 10);
            this.pnlControles.BackColor = Color.White;
            this.pnlControles.Font = new Font("Segoe UI", 9.5f);

            int spacing = 10;
            int labelSpacing = 5;
            int topRowY = 8;
            int bottomRowY = 45;
            int currentLeft = 10;

            // --- Fila 1: Fechas y Profesor ---
            this.lblFiltroFecha.Text = "Día:";
            this.lblFiltroFecha.AutoSize = true;
            this.lblFiltroFecha.Location = new Point(currentLeft, topRowY + 2);
            currentLeft = lblFiltroFecha.Right + labelSpacing;

            this.dtpFecha.Format = DateTimePickerFormat.Short;
            this.dtpFecha.Size = new Size(110, 25);
            this.dtpFecha.Location = new Point(currentLeft, topRowY);
            currentLeft = dtpFecha.Right + spacing;

            this.lblFiltroProfesor.Text = "Profesor:";
            this.lblFiltroProfesor.AutoSize = true;
            this.lblFiltroProfesor.Location = new Point(currentLeft, topRowY + 2);
            currentLeft = lblFiltroProfesor.Right + labelSpacing;

            this.cmbProfesor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbProfesor.Size = new Size(160, 25);
            this.cmbProfesor.Location = new Point(currentLeft, topRowY);
            currentLeft = cmbProfesor.Right + spacing;

            // --- Fila 2: Género, Checkbox y Botones ---
            currentLeft = 10; // Reiniciamos el 'X' para la segunda fila

            this.lblFiltroGenero.Text = "Género:";
            this.lblFiltroGenero.AutoSize = true;
            this.lblFiltroGenero.Location = new Point(currentLeft, bottomRowY + 2);
            currentLeft = lblFiltroGenero.Right + labelSpacing; // 3

            this.cmbGenero.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbGenero.Size = new Size(110, 25);
            this.cmbGenero.Location = new Point(currentLeft, bottomRowY);
            currentLeft = cmbGenero.Right + (spacing * 2); // 8*2 = 16

            this.chkSoloEditadas.Text = "Mostrar solo editadas";
            this.chkSoloEditadas.AutoSize = true;
            this.chkSoloEditadas.Location = new Point(currentLeft, bottomRowY + 2);

            // --- 🔥 CORRECCIÓN AQUÍ: Actualizamos 'currentLeft' ---
            currentLeft = chkSoloEditadas.Right + (spacing * 2); // 8*2 = 16

            // Botón Filtrar (ya NO usa Anchor, usa currentLeft)
            this.btnFiltrar.Text = "🔍 FILTRAR";
            this.btnFiltrar.Size = new Size(120, 35);
            this.btnFiltrar.Location = new Point(currentLeft, bottomRowY - 5);

            // --- 🔥 CORRECCIÓN AQUÍ: Actualizamos 'currentLeft' ---
            currentLeft = btnFiltrar.Right + spacing; // 8

            // Botón Limpiar Filtros (ya NO usa Anchor, usa currentLeft)
            this.btnLimpiarFiltros.Text = "🧹 LIMPIAR";
            this.btnLimpiarFiltros.Size = new Size(120, 35);
            this.btnLimpiarFiltros.Location = new Point(currentLeft, bottomRowY - 5);

            // --- Botones de Acción (Alineados a la derecha) ---
            this.btnModoTV.Text = "📺 INICIAR MODO TV";
            this.btnModoTV.Size = new Size(160, 35);
            this.btnModoTV.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.btnExportar.Text = "📤 EXPORTAR";
            this.btnExportar.Size = new Size(130, 35);
            this.btnExportar.Anchor = AnchorStyles.Top | AnchorStyles.Right;

            this.btnImprimir.Text = "🖨️ IMPRIMIR";
            this.btnImprimir.Size = new Size(130, 35);
            this.btnImprimir.Anchor = AnchorStyles.Top | AnchorStyles.Right;

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
            // DataGridView (Principal)
            // =========================================================
            this.dgvPlanillas.Dock = DockStyle.Fill;
            this.dgvPlanillas.Margin = new Padding(0, 5, 0, 0);

            // --- 🔥 CORRECCIÓN: Definición de Columnas para RUTINAS ---
            this.colNombre.HeaderText = "NOMBRE DE LA RUTINA";
            this.colNombre.Name = "colNombre";
            this.colNombre.FillWeight = 35;
            this.colProfesor.HeaderText = "PROFESOR";
            this.colProfesor.Name = "colProfesor";
            this.colProfesor.FillWeight = 25;
            this.colFecha.HeaderText = "FECHA CREACIÓN";
            this.colFecha.Name = "colFecha";
            this.colFecha.FillWeight = 20;
            this.colGenero.HeaderText = "GÉNERO";
            this.colGenero.Name = "colGenero";
            this.colGenero.FillWeight = 20;

            this.dgvPlanillas.Columns.AddRange(new DataGridViewColumn[] {
                this.colNombre, this.colProfesor, this.colGenero, this.colFecha
            });
            // --- FIN CORRECCIÓN ---

            // =========================================================
            // Ensamblado Final
            // =========================================================
            this.BackColor = Color.FromArgb(248, 249, 250);

            this.mainPanel.Dock = DockStyle.Fill;
            this.mainPanel.Controls.Add(this.dgvPlanillas); // Grilla
            this.mainPanel.Controls.Add(this.pnlControles); // Panel de filtros

            this.Controls.Add(this.mainPanel);

            // Redimensionar botones
            this.pnlControles.Resize += (sender, e) => {
                btnModoTV.Location = new Point(pnlControles.Width - 175, 8);
                btnExportar.Location = new Point(btnModoTV.Left - 140, 8);
                btnImprimir.Location = new Point(btnExportar.Left - 140, 8);
            };

            ((System.ComponentModel.ISupportInitialize)(this.dgvPlanillas)).EndInit();
            this.mainPanel.ResumeLayout(false);
            this.pnlControles.ResumeLayout(false);
            this.pnlControles.PerformLayout();
            this.ResumeLayout(false);
        }
    }
}