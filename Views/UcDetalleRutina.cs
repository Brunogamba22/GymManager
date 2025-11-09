using GymManager.Forms;
using GymManager.Models;
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
        public Rutina RutinaActual { get; private set; }

        private List<DetalleRutina> _detallesActuales;

        public UcDetalleRutina()
        {
            InitializeComponent();
            ApplyModernStyles();
            ConfigurarGrid();

            this.btnEditar.Click += this.btnEditar_Click;
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
        public void CargarRutina(int idRutina, string nombreRutina, string tipoRutina, string profesor,
                         DateTime fecha, List<DetalleRutina> ejercicios)
        {
            // Guardar la rutina actual con su ID real
            RutinaActual = new Rutina
            {
                IdRutina = idRutina,
                Nombre = nombreRutina,
                NombreGenero = tipoRutina,
                NombreProfesor = profesor,
                FechaCreacion = fecha
            };

            _detallesActuales = ejercicios;

            // 🔹 Título principal
            lblTitulo.Text = nombreRutina;

            // 🔹 Subtítulo con detalles
            lblDetalles.Text = $"🏷️ {tipoRutina.ToUpper()} | 👤 {profesor} | 📅 {fecha:dd/MM/yyyy HH:mm}";

            // 🔹 Contador total de ejercicios
            lblContador.Text = $"📊 Total de ejercicios: {ejercicios.Count}";

            // 🔹 Limpiar contenido previo
            dgvEjercicios.Rows.Clear();

            // 🔹 Cargar filas
            foreach (var ejercicio in ejercicios)
            {
                dgvEjercicios.Rows.Add(
                    ejercicio.EjercicioNombre,
                    ejercicio.Series,
                    ejercicio.Repeticiones,
                    ejercicio.Carga?.ToString() ?? ""
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
            try
            {
                if (RutinaActual == null)
                {
                    MessageBox.Show("No hay una rutina cargada para imprimir.",
                                    "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Crear instancia del Dashboard temporalmente solo para usar su método de impresión
                var dash = new UcRecepcionistaDashboard();
                dash.ImprimirRutina(RutinaActual);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al imprimir rutina: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                if (RutinaActual == null)
                {
                    MessageBox.Show("No hay una rutina cargada para exportar.",
                                    "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // Crear instancia del Dashboard temporalmente solo para usar su método de exportación
                var dash = new UcRecepcionistaDashboard();
                dash.ExportarRutina(RutinaActual);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar rutina: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            
            if (RutinaActual == null || _detallesActuales == null)
            {
                MessageBox.Show("No hay una rutina cargada para editar.",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmacion = MessageBox.Show(
                                        "¿Estás seguro de que deseas editar esta rutina?" +
                                        "\n\nSe cargará en el panel de edición.",
                                        "Confirmar Edición", // Título de la ventana
                                        MessageBoxButtons.YesNo, // Botones Sí y No
                                        MessageBoxIcon.Question); // Ícono de pregunta

            // Si el usuario presiona "No", simplemente salimos del método.
            if (confirmacion == DialogResult.No)
            {
                return;
            }

            // Buscar el formulario principal (FrmMain)
            var frmMain = this.ParentForm as FrmMain;
            if (frmMain != null)
            {
                
                // Pedirle a FrmMain que inicie la navegación
                frmMain.NavegarAEditor(RutinaActual, _detallesActuales);

                // Disparar el evento 'Cerrar' para que el panel de 
                // historial sepa que debe ocultar esta vista de detalle.
                OnCerrarDetalle?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                MessageBox.Show("Error fatal: No se pudo encontrar el formulario principal...", "Error");
            }
        }
    }
}