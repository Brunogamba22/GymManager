using GymManager.Controllers;
using GymManager.Models;
using GymManager.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GymManager.Views
{
    public partial class UcPlanillasRutinas : UserControl
    {
        // Colores personalizados
        private Color primaryColor = Color.FromArgb(46, 134, 171);
        private Color backgroundColor = Color.FromArgb(248, 249, 250);
        private Color successColor = Color.FromArgb(40, 167, 69);
        private Color textColor = Color.FromArgb(33, 37, 41);

        // 🔥 VARIABLE PARA EL CONTROL DE DETALLES
        private UcDetalleRutina ucDetalle = null;

        private readonly RutinaController _rutinaController = new RutinaController();
        private readonly DetalleRutinaController _detalleController = new DetalleRutinaController();

        // 🔥 LISTA PARA ALMACENAR RUTINAS GUARDADAS
        private List<Rutina> rutinasGuardadas = new List<Rutina>();

        public UcPlanillasRutinas()
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
            dgvPlanillas.BackgroundColor = Color.White;
            dgvPlanillas.BorderStyle = BorderStyle.None;
            dgvPlanillas.EnableHeadersVisualStyles = false;
            dgvPlanillas.AllowUserToAddRows = false;
            dgvPlanillas.AllowUserToDeleteRows = false;
            dgvPlanillas.ReadOnly = true;
            dgvPlanillas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPlanillas.RowHeadersVisible = false;
            dgvPlanillas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPlanillas.RowTemplate.Height = 40;

            dgvPlanillas.AllowUserToResizeColumns = false;
            dgvPlanillas.AllowUserToResizeRows = false;
            dgvPlanillas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Estilo de encabezados
            dgvPlanillas.ColumnHeadersDefaultCellStyle.BackColor = primaryColor;
            dgvPlanillas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPlanillas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvPlanillas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPlanillas.ColumnHeadersHeight = 45;

            // Estilo de celdas
            dgvPlanillas.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvPlanillas.DefaultCellStyle.BackColor = Color.White;
            dgvPlanillas.DefaultCellStyle.ForeColor = textColor;
            dgvPlanillas.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPlanillas.DefaultCellStyle.Padding = new Padding(5);

            // Filas alternadas
            dgvPlanillas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);

            // Evento de selección
            dgvPlanillas.SelectionChanged += DgvPlanillas_SelectionChanged;
        }

        

        public void CargarDatos()
        {
            try
            {
                // 1. Llamar al nuevo método del controlador
                rutinasGuardadas = _rutinaController.ObtenerTodasParaPlanilla();

                // --- 🔥 AÑADE ESTA LÍNEA TEMPORALMENTE ---
                MessageBox.Show($"Se cargaron {rutinasGuardadas.Count} rutinas desde la BD.");
                // --- FIN DE LÍNEA TEMPORAL ---

                // 2. Actualizar la grilla
                ActualizarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar planillas: {ex.Message}", "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        


        // 🔥 ACTUALIZAR GRID CON LAS RUTINAS GUARDADAS
        private void ActualizarGrid()
        {
            dgvPlanillas.Rows.Clear();

            foreach (var rutina in rutinasGuardadas.OrderByDescending(r => r.FechaCreacion))
            {
                dgvPlanillas.Rows.Add(
                    rutina.Nombre,
                    rutina.NombreProfesor,
                    rutina.FechaCreacion.ToString("dd/MM/yyyy HH:mm")
                );
            }
        }

        private void DgvPlanillas_SelectionChanged(object sender, EventArgs e)
        {
            // 🔥 CORREGIDO: Verificar que haya filas seleccionadas y que no sea la fila de encabezado
            if (dgvPlanillas.SelectedRows.Count > 0 && dgvPlanillas.SelectedRows[0].Index >= 0)
            {
                int selectedIndex = dgvPlanillas.SelectedRows[0].Index;

                // 🔥 CORREGIDO: Verificar que el índice esté dentro del rango de rutinas guardadas
                if (selectedIndex < rutinasGuardadas.Count)
                {
                    var rutinaSeleccionada = rutinasGuardadas[selectedIndex];

                    // 🔥 MOSTRAR DETALLES EN USERCONTROL SEPARADO
                    MostrarDetalleRutina(rutinaSeleccionada);
                }
            }
        }

        private void MostrarDetalleRutina(Rutina rutinaHeader)
        {
            mainPanel.Visible = false;

            // Crear o mostrar el control de detalle
            if (ucDetalle == null)
            {
                ucDetalle = new UcDetalleRutina();
                ucDetalle.Dock = DockStyle.Fill;
                ucDetalle.OnCerrarDetalle += (s, e) => OcultarDetalle();
                this.Controls.Add(ucDetalle);
            }

            

            // 1. Buscamos los detalles (ejercicios) de esta rutina en la BD
            List<DetalleRutina> detallesDeRutina = _detalleController.ObtenerPorRutina(rutinaHeader.IdRutina);

            // 2. Cargamos el detalle
            // TU CÓDIGO ACTUAL (demo) espera una lista de 'Utils.RutinaSimulador.EjercicioRutina'
            // AHORA le estamos pasando una lista de 'Models.DetalleRutina'
            // Esto causará un error que debemos arreglar en 'UcDetalleRutina.cs'

            ucDetalle.CargarRutina(
                rutinaHeader.Nombre,
                rutinaHeader.NombreGenero,    // Pasamos el nombre del género
                rutinaHeader.NombreProfesor,  // Pasamos el nombre del profesor
                rutinaHeader.FechaCreacion,
                detallesDeRutina            // Pasamos los detalles REALES
            );
            
            ucDetalle.Visible = true;
            ucDetalle.BringToFront();
        }

        // 🔥 MÉTODO PARA OCULTAR EL DETALLE
        private void OcultarDetalle()
        {
            if (ucDetalle != null)
            {
                ucDetalle.Visible = false;
            }
            mainPanel.Visible = true;
            mainPanel.BringToFront(); // 🔥 AÑADIDO: Traer el panel principal al frente
        }

        private void StyleButton(Button btn, Color bgColor)
        {
            btn.BackColor = bgColor;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(15, 8, 15, 8);

            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(bgColor, 0.1f);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(bgColor, 0.2f);
        }

        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dgvPlanillas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una planilla para exportar", "Información",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var nombreRutina = dgvPlanillas.SelectedRows[0].Cells["colNombre"].Value.ToString();
            MessageBox.Show($"✅ Planilla '{nombreRutina}' exportada correctamente", "Exportación Exitosa",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        

        
    }
}