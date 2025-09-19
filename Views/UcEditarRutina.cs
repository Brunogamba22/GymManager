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
    public partial class UcEditarRutina : UserControl
    {
        public UcEditarRutina()
        {
            InitializeComponent();
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (dgvRutinas.Rows.Count == 0)
            {
                MessageBox.Show("No hay rutinas para editar.");
                return;
            }

            // Validación básica
            foreach (DataGridViewRow row in dgvRutinas.Rows)
            {
                if (row.IsNewRow) continue;
                if (string.IsNullOrWhiteSpace(row.Cells["colEjercicio"].Value?.ToString()))
                {
                    MessageBox.Show("Hay ejercicios vacíos. Corrige antes de guardar.");
                    return;
                }
            }

            // Simulación de guardado
            MessageBox.Show("Rutina actualizada con éxito.");
        }
    }
}
