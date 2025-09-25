using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Views
{
    public partial class UcEditarRutina : UserControl
    {
        private Color primaryColor = Color.FromArgb(46, 134, 171);
        private Color backgroundColor = Color.FromArgb(248, 249, 250);
        private Color successColor = Color.FromArgb(40, 167, 69);
        private Color warningColor = Color.FromArgb(255, 193, 7);
        private Color dangerColor = Color.FromArgb(220, 53, 69);

        public UcEditarRutina()
        {
            InitializeComponent();
            ApplyModernStyles();
            ConfigurarGrid();
            LoadSampleData();
        }

        private void ApplyModernStyles()
        {
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI", 9);
        }

        private void ConfigurarGrid()
        {
            // Configuración básica del DataGridView
            dgvRutinas.BackgroundColor = Color.White;
            dgvRutinas.BorderStyle = BorderStyle.None;
            dgvRutinas.EnableHeadersVisualStyles = false;

            // Permitir edición
            dgvRutinas.AllowUserToAddRows = true;
            dgvRutinas.AllowUserToDeleteRows = true;
            dgvRutinas.ReadOnly = false;
            dgvRutinas.AllowUserToResizeColumns = false;  // ← Deshabilitar redimensionamiento
            dgvRutinas.AllowUserToResizeRows = false;
            dgvRutinas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Configuración visual
            dgvRutinas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRutinas.RowHeadersVisible = false;
            dgvRutinas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRutinas.RowTemplate.Height = 35;

            // Estilo de encabezados
            dgvRutinas.ColumnHeadersDefaultCellStyle.BackColor = primaryColor;
            dgvRutinas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRutinas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvRutinas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRutinas.ColumnHeadersHeight = 40;

            // Estilo de celdas
            dgvRutinas.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvRutinas.DefaultCellStyle.BackColor = Color.White;
            dgvRutinas.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dgvRutinas.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRutinas.DefaultCellStyle.Padding = new Padding(5);

            // Filas alternadas
            dgvRutinas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
        }

        private void LoadSampleData()
        {
            // Limpiar datos existentes
            dgvRutinas.Rows.Clear();

            // Agregar datos de ejemplo realistas
            string[,] ejercicios = {
                {"Press banca", "3", "10", "60"},
                {"Sentadillas", "4", "8", "90"},
                {"Dominadas", "3", "8", "75"},
                {"Press militar", "3", "10", "60"},
                {"Curl de bíceps", "3", "12", "45"},
                {"Fondos en paralelas", "3", "10", "60"},
                {"Peso muerto", "4", "6", "120"}
            };

            for (int i = 0; i < ejercicios.GetLength(0); i++)
            {
                dgvRutinas.Rows.Add(
                    ejercicios[i, 0],
                    ejercicios[i, 1],
                    ejercicios[i, 2],
                    ejercicios[i, 3]
                );
            }
        }

        private void StyleButton(Button btn, Color bgColor)
        {
            btn.BackColor = bgColor;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(12, 6, 12, 6);

            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(bgColor, 0.1f);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(bgColor, 0.2f);
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (dgvRutinas.Rows.Count == 0 || (dgvRutinas.Rows.Count == 1 && dgvRutinas.Rows[0].IsNewRow))
            {
                MessageBox.Show("No hay rutinas para guardar.", "Información",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Validar datos
            for (int i = 0; i < dgvRutinas.Rows.Count - 1; i++)
            {
                var row = dgvRutinas.Rows[i];

                if (string.IsNullOrWhiteSpace(row.Cells[0].Value?.ToString()))
                {
                    MessageBox.Show($"El ejercicio en la fila {i + 1} está vacío.", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            MessageBox.Show("✅ Rutina guardada con éxito", "Éxito",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnAgregarEjercicio_Click(object sender, EventArgs e)
        {
            dgvRutinas.Rows.Add("Nuevo ejercicio", "3", "10", "60");
        }

        private void btnEliminarEjercicio_Click(object sender, EventArgs e)
        {
            if (dgvRutinas.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvRutinas.SelectedRows)
                {
                    if (!row.IsNewRow)
                        dgvRutinas.Rows.Remove(row);
                }
            }
            else
            {
                MessageBox.Show("Selecciona un ejercicio para eliminar.", "Información",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLimpiarTodo_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Eliminar todos los ejercicios?", "Confirmar",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
                dgvRutinas.Rows.Clear();
        }
    }
}