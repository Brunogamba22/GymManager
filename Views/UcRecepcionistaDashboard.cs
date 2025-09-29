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
        // Colores personalizados - misma línea del profesor
        private Color primaryColor = Color.FromArgb(46, 134, 171);
        private Color backgroundColor = Color.FromArgb(248, 249, 250);
        private Color cardColor = Color.White;
        private Color successColor = Color.FromArgb(40, 167, 69);
        private Color textColor = Color.FromArgb(33, 37, 41);

        public UcRecepcionistaDashboard()
        {
            InitializeComponent();
            ApplyModernStyles();
            ConfigurarGrid();
            CargarFechasDisponibles();
            CargarRutina(DateTime.Today);
        }

        private void ApplyModernStyles()
        {
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI", 9);
        }

        // Configuración visual moderna de la grilla
        private void ConfigurarGrid()
        {
            dgvRutina.BackgroundColor = cardColor;
            dgvRutina.BorderStyle = BorderStyle.None;
            dgvRutina.EnableHeadersVisualStyles = false;
            dgvRutina.AllowUserToAddRows = false;
            dgvRutina.AllowUserToDeleteRows = false;
            dgvRutina.ReadOnly = true;
            dgvRutina.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRutina.RowHeadersVisible = false;
            dgvRutina.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRutina.RowTemplate.Height = 45;

            // DESHABILITAR REDIMENSIONAMIENTO
            dgvRutina.AllowUserToResizeColumns = false;
            dgvRutina.AllowUserToResizeRows = false;
            dgvRutina.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // CONFIGURAR COLUMNAS PRIMERO
            ConfigurarColumnas();

            // Estilo de encabezados
            dgvRutina.ColumnHeadersDefaultCellStyle.BackColor = primaryColor;
            dgvRutina.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRutina.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvRutina.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRutina.ColumnHeadersHeight = 45;

            // Estilo de celdas
            dgvRutina.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvRutina.DefaultCellStyle.BackColor = cardColor;
            dgvRutina.DefaultCellStyle.ForeColor = textColor;
            dgvRutina.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRutina.DefaultCellStyle.Padding = new Padding(8);

            // Filas alternadas
            dgvRutina.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
        }

        // MÉTODO NUEVO: Configurar columnas primero
        private void ConfigurarColumnas()
        {
            // Limpiar columnas existentes
            dgvRutina.Columns.Clear();

            // Agregar columnas
            dgvRutina.Columns.Add("Ejercicio", "🏋️‍♂️ EJERCICIO");
            dgvRutina.Columns.Add("Series", "📊 SERIES");
            dgvRutina.Columns.Add("Repeticiones", "🔄 REPETICIONES");
            dgvRutina.Columns.Add("Descanso", "⏱️ DESCANSO");

            // Configurar ancho de columnas
            dgvRutina.Columns["Ejercicio"].FillWeight = 40;
            dgvRutina.Columns["Series"].FillWeight = 20;
            dgvRutina.Columns["Repeticiones"].FillWeight = 20;
            dgvRutina.Columns["Descanso"].FillWeight = 20;

            // DESHABILITAR REDIMENSIONAMIENTO POR COLUMNA
            foreach (DataGridViewColumn columna in dgvRutina.Columns)
            {
                columna.Resizable = DataGridViewTriState.False;
            }
        }

        private void CargarFechasDisponibles()
        {
            cmbFecha.Items.Clear();

            // Agregar fechas de ejemplo (en un sistema real vendrían de la base de datos)
            for (int i = 0; i < 7; i++)
            {
                DateTime fecha = DateTime.Today.AddDays(-i);
                cmbFecha.Items.Add(new FechaItem
                {
                    Fecha = fecha,
                    Texto = fecha.ToString("dd/MM/yyyy")
                });
            }

            cmbFecha.SelectedIndex = 0;
        }

        // Carga la rutina según la fecha
        private void CargarRutina(DateTime fecha)
        {
            // CORREGIDO: Limpiar filas pero mantener columnas
            dgvRutina.Rows.Clear();

            // Ejemplo estático (después podrías conectarlo a BD o lógica del controlador)
            dgvRutina.Rows.Add("🏋️‍♂️ Sentadillas", "4", "12", "60 s");
            dgvRutina.Rows.Add("💪 Press banca", "4", "10", "90 s");
            dgvRutina.Rows.Add("🔥 Plancha abdominal", "3", "30 seg", "30 s");
            dgvRutina.Rows.Add("🚴‍♂️ Burpees", "3", "15", "45 s");
            dgvRutina.Rows.Add("🧘‍♂️ Flexiones", "4", "12", "60 s");

            lblTitulo.Text = $"📅 Rutina del día {fecha:dd/MM/yyyy}";
            lblContadorEjercicios.Text = $"📊 Total de ejercicios: {dgvRutina.Rows.Count}";
        }

        private void btnPantallaCompleta_Click(object sender, EventArgs e)
        {
            try
            {
                FormPantallaCompleta pantallaCompleta = new FormPantallaCompleta(dgvRutina, lblTitulo.Text);
                pantallaCompleta.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al abrir pantalla completa: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            MessageBox.Show("✅ Rutina exportada correctamente (PDF)", "Exportar",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnImprimir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("✅ Rutina enviada a impresión", "Imprimir",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cmbFecha_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Cuando cambie la fecha, recargamos
            if (cmbFecha.SelectedItem is FechaItem fechaItem)
            {
                CargarRutina(fechaItem.Fecha);
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
            btn.Padding = new Padding(15, 10, 15, 10);

            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(bgColor, 0.1f);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(bgColor, 0.2f);
        }

        // NUEVO: Método para reposicionar botones cuando cambie el tamaño
        private void ReposicionarBotones()
        {
            int espacioEntreBotones = 15;
            int margenDerecho = 30;

            // Calcular posición X empezando desde la derecha
            int xPos = panelBotones.Width - margenDerecho;

            // Pantalla Completa (más ancho)
            btnPantallaCompleta.Size = new Size(160, 45);
            xPos -= btnPantallaCompleta.Width;
            btnPantallaCompleta.Location = new Point(xPos, (panelBotones.Height - btnPantallaCompleta.Height) / 2);

            // Exportar
            xPos -= espacioEntreBotones;
            btnExportar.Size = new Size(140, 45);
            xPos -= btnExportar.Width;
            btnExportar.Location = new Point(xPos, (panelBotones.Height - btnExportar.Height) / 2);

            // Imprimir
            xPos -= espacioEntreBotones;
            btnImprimir.Size = new Size(120, 45);
            xPos -= btnImprimir.Width;
            btnImprimir.Location = new Point(xPos, (panelBotones.Height - btnImprimir.Height) / 2);
        }

        // Clase para manejar items del combobox con fecha
        private class FechaItem
        {
            public DateTime Fecha { get; set; }
            public string Texto { get; set; } = "";

            public override string ToString()
            {
                return Texto;
            }
        }
    }

    // Form para pantalla completa
    public class FormPantallaCompleta : Form
    {
        private DataGridView dgvOriginal;
        private string titulo;

        public FormPantallaCompleta(DataGridView dgvOriginal, string titulo)
        {
            this.dgvOriginal = dgvOriginal;
            this.titulo = titulo;
            InitializeForm();
        }

        private void InitializeForm()
        {
            this.Text = "Rutina - Pantalla Completa";
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.FormBorderStyle = FormBorderStyle.None;
            this.KeyPreview = true;
            this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) this.Close(); };

            // Panel principal
            Panel mainPanel = new Panel();
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Padding = new Padding(40);
            mainPanel.BackColor = Color.FromArgb(248, 249, 250);

            // Título
            Label lblTituloPantalla = new Label();
            lblTituloPantalla.Text = titulo;
            lblTituloPantalla.Dock = DockStyle.Top;
            lblTituloPantalla.Height = 80;
            lblTituloPantalla.Font = new Font("Segoe UI", 24, FontStyle.Bold);
            lblTituloPantalla.ForeColor = Color.FromArgb(46, 134, 171);
            lblTituloPantalla.TextAlign = ContentAlignment.MiddleCenter;

            // DataGridView para pantalla completa
            DataGridView dgvPantalla = new DataGridView();
            dgvPantalla.Dock = DockStyle.Fill;
            dgvPantalla.BackgroundColor = Color.White;
            dgvPantalla.BorderStyle = BorderStyle.None;
            dgvPantalla.EnableHeadersVisualStyles = false;
            dgvPantalla.AllowUserToAddRows = false;
            dgvPantalla.ReadOnly = true;
            dgvPantalla.RowHeadersVisible = false;
            dgvPantalla.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPantalla.RowTemplate.Height = 60;

            // DESHABILITAR COMPLETAMENTE EL REDIMENSIONAMIENTO
            dgvPantalla.AllowUserToResizeColumns = false;
            dgvPantalla.AllowUserToResizeRows = false;
            dgvPantalla.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // CONFIGURAR COLUMNAS PRIMERO
            dgvPantalla.Columns.Clear();
            foreach (DataGridViewColumn col in dgvOriginal.Columns)
            {
                DataGridViewColumn newCol = (DataGridViewColumn)col.Clone();
                newCol.Resizable = DataGridViewTriState.False; // 🔥 DESHABILITAR REDIMENSION POR COLUMNA
                dgvPantalla.Columns.Add(newCol);
            }

            // Copiar datos
            foreach (DataGridViewRow row in dgvOriginal.Rows)
            {
                if (!row.IsNewRow)
                {
                    int rowIndex = dgvPantalla.Rows.Add();
                    for (int i = 0; i < row.Cells.Count; i++)
                    {
                        dgvPantalla.Rows[rowIndex].Cells[i].Value = row.Cells[i].Value;
                    }
                }
            }

            // Estilos para pantalla grande
            dgvPantalla.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(46, 134, 171);
            dgvPantalla.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPantalla.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 14, FontStyle.Bold);
            dgvPantalla.ColumnHeadersHeight = 60;
            dgvPantalla.DefaultCellStyle.Font = new Font("Segoe UI", 12);
            dgvPantalla.DefaultCellStyle.Padding = new Padding(10);

            // DESHABILITAR LA SELECCIÓN VISUAL (opcional, para mejor experiencia)
            dgvPantalla.DefaultCellStyle.SelectionBackColor = dgvPantalla.DefaultCellStyle.BackColor;
            dgvPantalla.DefaultCellStyle.SelectionForeColor = dgvPantalla.DefaultCellStyle.ForeColor;

            // Instrucción de salida
            Label lblInstruccion = new Label();
            lblInstruccion.Text = "Presiona ESC para salir";
            lblInstruccion.Dock = DockStyle.Bottom;
            lblInstruccion.Height = 40;
            lblInstruccion.Font = new Font("Segoe UI", 10, FontStyle.Italic);
            lblInstruccion.ForeColor = Color.Gray;
            lblInstruccion.TextAlign = ContentAlignment.MiddleCenter;

            mainPanel.Controls.Add(dgvPantalla);
            mainPanel.Controls.Add(lblInstruccion);
            mainPanel.Controls.Add(lblTituloPantalla);

            this.Controls.Add(mainPanel);
        }
    }
}