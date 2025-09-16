using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Views
{
    public partial class UcProfesorDashboard : UserControl
    {
        public UcProfesorDashboard()
        {
            InitializeComponent();

            // Configurar los tres DataGridView al cargar
            ConfigurarGrid(dgvHombres);
            ConfigurarGrid(dgvMujeres);
            ConfigurarGrid(dgvDeportistas);
        }

        /// <summary>
        /// Configura estilos generales para todos los DataGridView
        /// </summary>
        private void ConfigurarGrid(DataGridView dgv)
        {
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            // Estilo de encabezado
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Estilo de celdas
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9);
        }

        /// <summary>
        /// Genera ejemplos de rutina para hombres
        /// </summary>
        private void btnGenerarHombres_Click(object sender, EventArgs e)
        {
            dgvHombres.Rows.Clear();
            dgvHombres.Rows.Add("Press banca", 3, 10, "60 s");
            dgvHombres.Rows.Add("Sentadillas", 4, 8, "90 s");
        }

        /// <summary>
        /// Genera ejemplos de rutina para mujeres
        /// </summary>
        private void btnGenerarMujeres_Click(object sender, EventArgs e)
        {
            dgvMujeres.Rows.Clear();
            dgvMujeres.Rows.Add("Peso muerto", 3, 12, "60 s");
            dgvMujeres.Rows.Add("Zancadas", 4, 10, "90 s");
        }

        /// <summary>
        /// Genera ejemplos de rutina para deportistas
        /// </summary>
        private void btnGenerarDeportistas_Click(object sender, EventArgs e)
        {
            dgvDeportistas.Rows.Clear();
            dgvDeportistas.Rows.Add("Burpees", 3, 15, "45 s");
            dgvDeportistas.Rows.Add("Plancha", 3, 1, "30 s"); // 1 = 1 minuto
        }
    }
}
