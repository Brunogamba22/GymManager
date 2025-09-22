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
    public partial class UcRecepcionistaDashboard : UserControl
    {
        public UcRecepcionistaDashboard()
        {
            InitializeComponent();
            ConfigurarGrid();
            CargarRutina(DateTime.Today);
        }

        // Configuración visual de la grilla
        private void ConfigurarGrid()
        {
            dgvRutina.AllowUserToAddRows = false;
            dgvRutina.AllowUserToDeleteRows = false;
            dgvRutina.ReadOnly = true;
            dgvRutina.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRutina.RowHeadersVisible = false;
            dgvRutina.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgvRutina.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvRutina.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRutina.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
        }

        // Carga la rutina según la fecha
        private void CargarRutina(DateTime fecha)
        {
            dgvRutina.Rows.Clear();

            // Ejemplo estático (después podrías conectarlo a BD o lógica del controlador)
            dgvRutina.Rows.Add("Sentadillas", "4", "12", "60 s");
            dgvRutina.Rows.Add("Press banca", "4", "10", "90 s");
            dgvRutina.Rows.Add("Plancha abdominal", "3", "30 seg", "30 s");

            lblTitulo.Text = $"Rutina del día {fecha:dd/MM/yyyy}";
        }

        private void btnPantallaCompleta_Click(object sender, EventArgs e)
        {
            Form frm = new Form();
            frm.Text = "Rutina - Pantalla Completa";
            frm.WindowState = FormWindowState.Maximized;

            DataGridView copia = new DataGridView();
            copia.Dock = DockStyle.Fill;
            copia.DataSource = dgvRutina.DataSource ?? null;

            frm.Controls.Add(copia);
            frm.ShowDialog();
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Aquí se exportaría la rutina (PDF, Excel, etc.)", "Exportar", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Aquí se enviaría la rutina a impresión", "Imprimir", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cmbFecha_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Cuando cambie la fecha, recargamos
            if (DateTime.TryParse(cmbFecha.SelectedItem.ToString(), out DateTime fecha))
            {
                CargarRutina(fecha);
            }
        }
    }
}
