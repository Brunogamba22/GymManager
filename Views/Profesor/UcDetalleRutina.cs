using GymManager.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Views
{
    public partial class UcDetalleRutina : UserControl
    {
        // Colores personalizados
        private Color primaryColor = Color.FromArgb(46, 134, 171);
        private Color backgroundColor = Color.FromArgb(248, 249, 250);
        private Color successColor = Color.FromArgb(40, 167, 69);
        private Color textColor = Color.FromArgb(33, 37, 41);

        // Evento para cuando se cierra el detalle
        public event EventHandler OnCerrarDetalle;

        public UcDetalleRutina()
        {
            InitializeComponent();
            ApplyModernStyles();
            ConfigurarGrid();
        }

        private void ApplyModernStyles()
        {
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI", 9);
        }

        private void ConfigurarGrid()
        {
            dgvEjercicios.BackgroundColor = Color.White;
            dgvEjercicios.BorderStyle = BorderStyle.None;
            dgvEjercicios.EnableHeadersVisualStyles = false;
            dgvEjercicios.AllowUserToAddRows = false;
            dgvEjercicios.AllowUserToDeleteRows = false;
            dgvEjercicios.ReadOnly = true;
            dgvEjercicios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvEjercicios.RowHeadersVisible = false;
            dgvEjercicios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEjercicios.RowTemplate.Height = 35;

            dgvEjercicios.AllowUserToResizeColumns = false;
            dgvEjercicios.AllowUserToResizeRows = false;
            dgvEjercicios.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Estilo de encabezados
            dgvEjercicios.ColumnHeadersDefaultCellStyle.BackColor = primaryColor;
            dgvEjercicios.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvEjercicios.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvEjercicios.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvEjercicios.ColumnHeadersHeight = 40;

            // Estilo de celdas
            dgvEjercicios.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvEjercicios.DefaultCellStyle.BackColor = Color.White;
            dgvEjercicios.DefaultCellStyle.ForeColor = textColor;
            dgvEjercicios.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvEjercicios.DefaultCellStyle.Padding = new Padding(5);

            // Filas alternadas
            dgvEjercicios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
        }

        // Método para cargar los datos de la rutina
        public void CargarRutina(string nombreRutina, string tipoRutina, string profesor,
                               DateTime fecha, List<RutinaSimulador.EjercicioRutina> ejercicios)
        {
            lblTitulo.Text = nombreRutina;
            lblDetalles.Text = $"🏷️ {tipoRutina} | 👤 {profesor} | 📅 {fecha:dd/MM/yyyy HH:mm}";
            lblContador.Text = $"📊 Total de ejercicios: {ejercicios.Count}";

            // Limpiar y cargar ejercicios
            dgvEjercicios.Rows.Clear();
            foreach (var ejercicio in ejercicios)
            {
                dgvEjercicios.Rows.Add(
                    ejercicio.Nombre,
                    ejercicio.Series,
                    ejercicio.Repeticiones,
                    $"{ejercicio.Descanso} s"
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

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            // 🔥 DISPARAR EVENTO PARA QUE EL PADRE SEPA QUE SE CERRÓ
            OnCerrarDetalle?.Invoke(this, EventArgs.Empty);
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("✅ Función de impresión habilitada", "Imprimir",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("✅ Función de exportación habilitada", "Exportar",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}