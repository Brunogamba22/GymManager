using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Views
{
    public partial class UcPlanillasRutinas : UserControl
    {
        // Colores personalizados
        private Color primaryColor = Color.FromArgb(46, 134, 171);
        private Color backgroundColor = Color.FromArgb(248, 249, 250);
        private Color successColor = Color.FromArgb(40, 167, 69);
        private Color textColor = Color.FromArgb(33, 37, 41);

        public UcPlanillasRutinas()
        {
            InitializeComponent();
            ApplyModernStyles();
            ConfigurarGrid();
            CargarPlanillasDemo();
        }

        private void ApplyModernStyles()
        {
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI", 9);
        }

        private void ConfigurarGrid()
        {
            dgvPlanillas.BackgroundColor = Color.White;
            dgvPlanillas.BorderStyle = BorderStyle.None;
            dgvPlanillas.EnableHeadersVisualStyles = false;
            dgvPlanillas.AllowUserToAddRows = false;
            dgvPlanillas.AllowUserToDeleteRows = false;
            dgvPlanillas.ReadOnly = true;
            dgvPlanillas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPlanillas.RowHeadersVisible = false;
            dgvPlanillas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPlanillas.RowTemplate.Height = 40;

            dgvPlanillas.AllowUserToResizeColumns = false;  // ← Esto evita que se redimensionen columnas
            dgvPlanillas.AllowUserToResizeRows = false;     // ← Esto evita que se redimensionen filas
            dgvPlanillas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Estilo de encabezados
            dgvPlanillas.ColumnHeadersDefaultCellStyle.BackColor = primaryColor;
            dgvPlanillas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPlanillas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvPlanillas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPlanillas.ColumnHeadersHeight = 45;

            // Estilo de celdas
            dgvPlanillas.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvPlanillas.DefaultCellStyle.BackColor = Color.White;
            dgvPlanillas.DefaultCellStyle.ForeColor = textColor;
            dgvPlanillas.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPlanillas.DefaultCellStyle.Padding = new Padding(5);

            // Filas alternadas
            dgvPlanillas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);

            // Evento de selección
            dgvPlanillas.SelectionChanged += DgvPlanillas_SelectionChanged;
        }

        private void CargarPlanillasDemo()
        {
            dgvPlanillas.Rows.Clear();

            // Datos simulados más realistas
            dgvPlanillas.Rows.Add("Rutina Hombres - Fuerza", "Juan Pérez", DateTime.Now.AddDays(-5).ToString("dd/MM/yyyy"));
            dgvPlanillas.Rows.Add("Rutina Mujeres - Glúteos", "María Gómez", DateTime.Now.AddDays(-3).ToString("dd/MM/yyyy"));
            dgvPlanillas.Rows.Add("Rutina Deportistas - Crossfit", "Carlos López", DateTime.Now.AddDays(-1).ToString("dd/MM/yyyy"));
            dgvPlanillas.Rows.Add("Rutina General - Cardio", "Ana Rodríguez", DateTime.Now.AddDays(-7).ToString("dd/MM/yyyy"));
            dgvPlanillas.Rows.Add("Rutina Avanzada - Fuerza", "Pedro Martínez", DateTime.Now.AddDays(-2).ToString("dd/MM/yyyy"));
        }

        private void DgvPlanillas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPlanillas.SelectedRows.Count > 0)
            {
                var selectedRow = dgvPlanillas.SelectedRows[0];
                MostrarDetallesPlanilla(selectedRow);
            }
        }

        private void MostrarDetallesPlanilla(DataGridViewRow planilla)
        {
            // Limpiar detalles anteriores
            panelDetalles.Controls.Clear();

            var nombrePlanilla = planilla.Cells["colNombre"].Value.ToString();
            var profesor = planilla.Cells["colProfesor"].Value.ToString();
            var fecha = planilla.Cells["colFecha"].Value.ToString();

            // Crear contenedor de detalles
            var detallesPanel = new Panel();
            detallesPanel.Dock = DockStyle.Fill;
            detallesPanel.AutoScroll = true;
            detallesPanel.Padding = new Padding(20);

            // Header de detalles
            var lblTituloDetalles = new Label();
            lblTituloDetalles.Text = $"📋 {nombrePlanilla}";
            lblTituloDetalles.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            lblTituloDetalles.ForeColor = primaryColor;
            lblTituloDetalles.Height = 40;
            lblTituloDetalles.Dock = DockStyle.Top;

            var lblInfo = new Label();
            lblInfo.Text = $"👤 Profesor: {profesor} | 📅 Fecha: {fecha}";
            lblInfo.Font = new Font("Segoe UI", 10);
            lblInfo.ForeColor = Color.FromArgb(100, 100, 100);
            lblInfo.Height = 30;
            lblInfo.Dock = DockStyle.Top;

            // Contenedor de rutinas
            var rutinasPanel = new Panel();
            rutinasPanel.Dock = DockStyle.Fill;
            rutinasPanel.Padding = new Padding(0, 20, 0, 0);

            // Agregar rutinas según el tipo de planilla
            if (nombrePlanilla.Contains("Hombres"))
            {
                AgregarRutinaPanel(rutinasPanel, "🏋️ RUTINA HOMBRES", GetRutinaHombres());
            }
            else if (nombrePlanilla.Contains("Mujeres"))
            {
                AgregarRutinaPanel(rutinasPanel, "💪 RUTINA MUJERES", GetRutinaMujeres());
            }
            else if (nombrePlanilla.Contains("Deportistas"))
            {
                AgregarRutinaPanel(rutinasPanel, "⚡ RUTINA DEPORTISTAS", GetRutinaDeportistas());
            }
            else
            {
                // Planilla general - mostrar todas las rutinas
                AgregarRutinaPanel(rutinasPanel, "🏋️ RUTINA HOMBRES", GetRutinaHombres());
                AgregarRutinaPanel(rutinasPanel, "💪 RUTINA MUJERES", GetRutinaMujeres());
                AgregarRutinaPanel(rutinasPanel, "⚡ RUTINA DEPORTISTAS", GetRutinaDeportistas());
            }

            // Agregar controles al panel de detalles
            detallesPanel.Controls.Add(rutinasPanel);
            detallesPanel.Controls.Add(lblInfo);
            detallesPanel.Controls.Add(lblTituloDetalles);

            panelDetalles.Controls.Add(detallesPanel);
        }

        private void AgregarRutinaPanel(Panel parent, string titulo, string[,] ejercicios)
        {
            var rutinaPanel = new Panel();
            rutinaPanel.Dock = DockStyle.Top;
            rutinaPanel.BackColor = Color.White;
            rutinaPanel.Padding = new Padding(15);
            rutinaPanel.Margin = new Padding(0, 0, 0, 15);

            // Título de la rutina
            var lblTituloRutina = new Label();
            lblTituloRutina.Text = titulo;
            lblTituloRutina.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTituloRutina.ForeColor = primaryColor;
            lblTituloRutina.Height = 30;
            lblTituloRutina.Dock = DockStyle.Top;

            // Grid de ejercicios
            var ejerciciosGrid = new DataGridView();
            ejerciciosGrid.Dock = DockStyle.Top;
            ejerciciosGrid.Height = ejercicios.GetLength(0) * 35 + 45;
            ejerciciosGrid.ReadOnly = true;
            ejerciciosGrid.AllowUserToAddRows = false;
            ejerciciosGrid.RowHeadersVisible = false;
            ejerciciosGrid.BackgroundColor = Color.White;
            ejerciciosGrid.BorderStyle = BorderStyle.None;

            // Configurar columnas
            ejerciciosGrid.Columns.Add("Ejercicio", "EJERCICIO");
            ejerciciosGrid.Columns.Add("Series", "SERIES");
            ejerciciosGrid.Columns.Add("Repeticiones", "REPETICIONES");
            ejerciciosGrid.Columns.Add("Descanso", "DESCANSO (s)");

            // Estilo del grid
            ejerciciosGrid.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            ejerciciosGrid.ColumnHeadersDefaultCellStyle.ForeColor = textColor;
            ejerciciosGrid.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);

            // Agregar ejercicios
            for (int i = 0; i < ejercicios.GetLength(0); i++)
            {
                ejerciciosGrid.Rows.Add(ejercicios[i, 0], ejercicios[i, 1], ejercicios[i, 2], ejercicios[i, 3]);
            }

            rutinaPanel.Controls.Add(ejerciciosGrid);
            rutinaPanel.Controls.Add(lblTituloRutina);
            parent.Controls.Add(rutinaPanel);
        }

        private string[,] GetRutinaHombres()
        {
            return new string[,]
            {
                {"Press banca", "3", "10", "60"},
                {"Sentadillas", "4", "8", "90"},
                {"Dominadas", "3", "8", "75"},
                {"Press militar", "3", "10", "60"}
            };
        }

        private string[,] GetRutinaMujeres()
        {
            return new string[,]
            {
                {"Peso muerto", "3", "12", "60"},
                {"Zancadas", "4", "10", "90"},
                {"Hip thrust", "4", "12", "60"},
                {"Elevación de pelvis", "3", "15", "45"}
            };
        }

        private string[,] GetRutinaDeportistas()
        {
            return new string[,]
            {
                {"Burpees", "3", "15", "45"},
                {"Plancha", "3", "1", "30"},
                {"Saltos de caja", "4", "10", "60"},
                {"Mountain climbers", "3", "20", "45"}
            };
        }

        private void StyleButton(Button btn, Color bgColor)
        {
            btn.BackColor = bgColor;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(15, 8, 15, 8);

            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(bgColor, 0.1f);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(bgColor, 0.2f);
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dgvPlanillas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una planilla para exportar", "Información",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var nombreRutina = dgvPlanillas.SelectedRows[0].Cells["colNombre"].Value.ToString();
            MessageBox.Show($"✅ Planilla '{nombreRutina}' exportada correctamente", "Exportación Exitosa",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}