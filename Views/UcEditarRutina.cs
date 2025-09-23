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
    /// <summary>
    /// Vista para editar una rutina de ejercicios.
    /// Permite modificar ejercicios, series, repeticiones y descansos.
    /// </summary>
    public partial class UcEditarRutina : UserControl
    {
        /// <summary>
        /// Constructor del UserControl.
        /// Inicializa todos los componentes gráficos definidos en el diseñador.
        /// </summary>
        public UcEditarRutina()
        {
            InitializeComponent(); // Configura la interfaz (DataGridView y botón).
        }

        /// <summary>
        /// Evento al hacer clic en el botón "Guardar".
        /// Realiza validaciones básicas y simula el guardado de la rutina.
        /// </summary>
        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // Si no hay filas cargadas en la grilla, no se puede guardar nada.
            if (dgvRutinas.Rows.Count == 0)
            {
                MessageBox.Show("No hay rutinas para editar.");
                return;
            }

            // Validación básica sobre las filas ingresadas.
            foreach (DataGridViewRow row in dgvRutinas.Rows)
            {
                if (row.IsNewRow) continue; // Ignora la fila vacía para nuevas entradas.

                // Si la celda de ejercicio está vacía, se cancela el guardado.
                if (string.IsNullOrWhiteSpace(row.Cells["colEjercicio"].Value?.ToString()))
                {
                    MessageBox.Show("Hay ejercicios vacíos. Corrige antes de guardar.");
                    return;
                }
            }

            // Simulación de guardado (en el futuro puede conectarse a la BD).
            MessageBox.Show("Rutina actualizada con éxito.");
        }
    }
}
