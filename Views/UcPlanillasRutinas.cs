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
    public partial class UcPlanillasRutinas : UserControl
    {
        public UcPlanillasRutinas()
        {
            InitializeComponent();
            CargarPlanillasDemo(); // Cargamos datos de prueba por ahora
        }

        private void CargarPlanillasDemo()
        {
            dgvPlanillas.Rows.Clear();

            // Datos simulados
            dgvPlanillas.Rows.Add("Rutina Hombres - Fuerza", "Juan Pérez", DateTime.Now.AddDays(-5).ToShortDateString());
            dgvPlanillas.Rows.Add("Rutina Mujeres - Glúteos", "María Gómez", DateTime.Now.AddDays(-3).ToShortDateString());
            dgvPlanillas.Rows.Add("Rutina Deportistas - Crossfit", "Carlos López", DateTime.Now.AddDays(-1).ToShortDateString());
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dgvPlanillas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Seleccione una rutina para exportar");
                return;
            }

            var nombreRutina = dgvPlanillas.SelectedRows[0].Cells["columnName"].Value.ToString();
            MessageBox.Show($"Rutina '{nombreRutina}' exportada correctamente (simulación).");
        }
    }
}
