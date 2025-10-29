using GymManager.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using GymManager.Models;

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

            dgvEjercicios.DefaultCellStyle.SelectionBackColor = Color.White; // Same as normal background
            dgvEjercicios.DefaultCellStyle.SelectionForeColor = textColor;

            // Filas alternadas
            dgvEjercicios.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
        }

        // =========================================================
        // MÉTODO ACTUALIZADO: Acepta el modelo real
        // =========================================================
        /// <summary>
        /// Carga los datos de una rutina real desde la base de datos.
        /// </summary>
        public void CargarRutina(string nombreRutina, string tipoRutina, string profesor,
                                     DateTime fecha, List<DetalleRutina> ejercicios) // <-- CAMBIO DE TIPO
        {
            // 🔹 Título principal
            lblTitulo.Text = nombreRutina;

            // 🔹 Subtítulo con tipo, profesor y fecha
            lblDetalles.Text = $"🏷️ {tipoRutina.ToUpper()} | 👤 {profesor} | 📅 {fecha:dd/MM/yyyy HH:mm}";

            // 🔹 Contador total de ejercicios
            lblContador.Text = $"📊 Total de ejercicios: {ejercicios.Count}";

            // 🔹 Limpiar cualquier contenido previo de la grilla
            dgvEjercicios.Rows.Clear();

            // 🔹 Agregar cada ejercicio como fila en el DataGridView
            foreach (var ejercicio in ejercicios)
            {
                dgvEjercicios.Rows.Add(
                    ejercicio.EjercicioNombre,        // Nombre del ejercicio (del JOIN)
                    ejercicio.Series,               // Cantidad de series
                    ejercicio.Repeticiones,         // Cantidad de repeticiones
                    ejercicio.Carga?.ToString() ?? "" // Carga (o vacío si es null)
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
            //DISPARAR EVENTO PARA QUE EL PADRE SEPA QUE SE CERRÓ
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