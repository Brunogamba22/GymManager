using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Views
{
    public partial class UcGenerarRutinas : UserControl
    {
        // Colores personalizados
        private Color primaryColor = Color.FromArgb(46, 134, 171);    // #2E86AB
        private Color secondaryColor = Color.FromArgb(162, 59, 114);  // #A23B72
        private Color successColor = Color.FromArgb(28, 167, 69);     // #28A745
        private Color backgroundColor = Color.FromArgb(248, 249, 250); // #F8F9FA
        private Color textColor = Color.FromArgb(33, 37, 41);         // #212529
        private Color borderColor = Color.FromArgb(222, 226, 230);    // #DEE2E6
        private Color tabHoverColor = Color.FromArgb(240, 240, 240);  // Gris muy claro

        private Panel[] tabPanels;
        private Label[] tabLabels;

        public UcGenerarRutinas()
        {
            InitializeComponent();
            ApplyModernStyles();
            ConfigurarGrids();
            SetupTabSystem();
        }

        private void ApplyModernStyles()
        {
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI", 9, FontStyle.Regular);
        }

        private void SetupTabSystem()
        {
            // Inicializar arrays
            tabPanels = new Panel[] { panelHombres, panelMujeres, panelDeportistas };
            tabLabels = new Label[] { lblTabHombres, lblTabMujeres, lblTabDeportistas };

            // Mostrar solo el primer panel al inicio
            ShowTab(0);
        }

        private void ShowTab(int tabIndex)
        {
            // Ocultar todos los paneles
            foreach (var panel in tabPanels)
            {
                panel.Visible = false;
            }

            // Resetear estilo de todas las tabs
            foreach (var label in tabLabels)
            {
                label.BackColor = Color.White;
                label.ForeColor = textColor;
                label.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            }

            // Mostrar panel seleccionado y aplicar estilo activo a la tab
            tabPanels[tabIndex].Visible = true;
            tabLabels[tabIndex].BackColor = GetTabColor(tabIndex);
            tabLabels[tabIndex].ForeColor = Color.White;
            tabLabels[tabIndex].Font = new Font("Segoe UI", 10, FontStyle.Bold);
        }

        private Color GetTabColor(int tabIndex)
        {
            return tabIndex switch
            {
                0 => primaryColor,    // Hombres - Azul
                1 => secondaryColor,  // Mujeres - Magenta
                2 => successColor,    // Deportistas - Verde
                _ => primaryColor
            };
        }

        private void ConfigurarGrids()
        {
            ConfigurarGrid(dgvHombres);
            ConfigurarGrid(dgvMujeres);
            ConfigurarGrid(dgvDeportistas);
        }

        private void ConfigurarGrid(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(240, 240, 240);
            dgv.DefaultCellStyle.SelectionForeColor = textColor;

            // Estilo de encabezados
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = textColor;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersHeight = 40;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Estilo de celdas
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = textColor;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.Padding = new Padding(5);

            // Estilo de filas alternadas
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = borderColor;
        }

        private void StyleButton(Button btn, Color bgColor)
        {
            btn.BackColor = bgColor;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(20, 10, 20, 10);

            // Efectos hover
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(bgColor, 0.1f);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(bgColor, 0.2f);
        }

        // Eventos para las tabs
        private void lblTabHombres_Click(object sender, EventArgs e) => ShowTab(0);
        private void lblTabMujeres_Click(object sender, EventArgs e) => ShowTab(1);
        private void lblTabDeportistas_Click(object sender, EventArgs e) => ShowTab(2);

        // Eventos hover para tabs
        private void TabLabel_MouseEnter(object sender, EventArgs e)
        {
            var label = (Label)sender;
            if (!label.BackColor.Equals(Color.White)) return;

            label.BackColor = tabHoverColor;
            label.Cursor = Cursors.Hand;
        }

        private void TabLabel_MouseLeave(object sender, EventArgs e)
        {
            var label = (Label)sender;
            if (!label.BackColor.Equals(tabHoverColor)) return;

            label.BackColor = Color.White;
        }

        // Métodos de generación
        private void btnGenerarHombres_Click(object sender, EventArgs e)
        {
            dgvHombres.Rows.Clear();
            dgvHombres.Rows.Add("Press banca", 3, 10, "60 s");
            dgvHombres.Rows.Add("Sentadillas", 4, 8, "90 s");
            dgvHombres.Rows.Add("Dominadas", 3, 8, "75 s");
            dgvHombres.Rows.Add("Press militar", 3, 10, "60 s");
            MessageBox.Show("✅ Rutina para HOMBRES generada exitosamente", "Generación Exitosa",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnGenerarMujeres_Click(object sender, EventArgs e)
        {
            dgvMujeres.Rows.Clear();
            dgvMujeres.Rows.Add("Peso muerto", 3, 12, "60 s");
            dgvMujeres.Rows.Add("Zancadas", 4, 10, "90 s");
            dgvMujeres.Rows.Add("Hip thrust", 4, 12, "60 s");
            dgvMujeres.Rows.Add("Elevación de pelvis", 3, 15, "45 s");
            MessageBox.Show("✅ Rutina para MUJERES generada exitosamente", "Generación Exitosa",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnGenerarDeportistas_Click(object sender, EventArgs e)
        {
            dgvDeportistas.Rows.Clear();
            dgvDeportistas.Rows.Add("Burpees", 3, 15, "45 s");
            dgvDeportistas.Rows.Add("Plancha", 3, 1, "30 s");
            dgvDeportistas.Rows.Add("Saltos de caja", 4, 10, "60 s");
            dgvDeportistas.Rows.Add("Mountain climbers", 3, 20, "45 s");
            MessageBox.Show("✅ Rutina para DEPORTISTAS generada exitosamente", "Generación Exitosa",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}