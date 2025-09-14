using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GymManager.Views
{
    public partial class UcProfesorDashboard : UserControl
    {
        public UcProfesorDashboard()
        {
            InitializeComponent();
            ConfigurarGrid(dgvHombres);
            ConfigurarGrid(dgvMujeres);
            ConfigurarGrid(dgvDeportistas);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

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

        private void btnGenerarHombres_Click(object sender, EventArgs e)
        {
            dgvHombres.Rows.Clear();
            dgvHombres.Rows.Add("Press banca", 3, 10, 60);
            dgvHombres.Rows.Add("Sentadillas", 4, 8, 90);
        }

        private void btnGenerarMujeres_Click(object sender, EventArgs e)
        {
            dgvMujeres.Rows.Clear();
            dgvMujeres.Rows.Add("Peso muerto", 3, 12, 60);
            dgvMujeres.Rows.Add("Zancadas", 4, 10, 90);
        }

        private void btnGenerarDeportistas_Click(object sender, EventArgs e)
        {
            dgvDeportistas.Rows.Clear();
            dgvDeportistas.Rows.Add("Burpees", 3, 15, 45);
            dgvDeportistas.Rows.Add("Plancha", 3, 60, 30);
        }

        private void dgvHombres_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void btnGenerarHombres_Click_1(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
