using GymManager.Models.Events;
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

        // 🔥 LISTA PARA ALMACENAR RUTINAS GUARDADAS
        private List<RutinaGuardada> rutinasGuardadas = new List<RutinaGuardada>();

        public UcPlanillasRutinas()
        {
            InitializeComponent();
            ApplyModernStyles();
            ConfigurarGrid();
            CargarPlanillasDemo(); // 🔥 DESCOMENTADO PARA PRUEBAS

            // 🔥 SUSCRIBIRSE AL EVENTO DE RUTINAS GUARDADAS
            EventosRutina.RutinaGuardada += OnRutinaGuardada;
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

        private void CargarPlanillasDemo()
        {
            // 🔥 AGREGAR RUTINAS DE DEMO A LA LISTA
            rutinasGuardadas.Add(new RutinaGuardada
            {
                Nombre = "Rutina Hombres - Fuerza",
                TipoRutina = "HOMBRES",
                Profesor = "Juan Pérez",
                FechaCreacion = DateTime.Now.AddDays(-5),
                Ejercicios = new List<Utils.RutinaSimulador.EjercicioRutina>
                {
                    new Utils.RutinaSimulador.EjercicioRutina { Nombre = "Press banca", Series = 3, Repeticiones = 10, Descanso = 60 },
                    new Utils.RutinaSimulador.EjercicioRutina { Nombre = "Sentadillas", Series = 4, Repeticiones = 8, Descanso = 90 },
                    new Utils.RutinaSimulador.EjercicioRutina { Nombre = "Dominadas", Series = 3, Repeticiones = 8, Descanso = 75 }
                }
            });

            rutinasGuardadas.Add(new RutinaGuardada
            {
                Nombre = "Rutina Mujeres - Glúteos",
                TipoRutina = "MUJERES",
                Profesor = "María Gómez",
                FechaCreacion = DateTime.Now.AddDays(-3),
                Ejercicios = new List<Utils.RutinaSimulador.EjercicioRutina>
                {
                    new Utils.RutinaSimulador.EjercicioRutina { Nombre = "Peso muerto", Series = 3, Repeticiones = 12, Descanso = 60 },
                    new Utils.RutinaSimulador.EjercicioRutina { Nombre = "Zancadas", Series = 4, Repeticiones = 10, Descanso = 90 },
                    new Utils.RutinaSimulador.EjercicioRutina { Nombre = "Hip thrust", Series = 4, Repeticiones = 12, Descanso = 60 }
                }
            });

            ActualizarGrid();
        }

        // 🔥 ACTUALIZAR GRID CON LAS RUTINAS GUARDADAS
        private void ActualizarGrid()
        {
            dgvPlanillas.Rows.Clear();
            foreach (var rutina in rutinasGuardadas.OrderByDescending(r => r.FechaCreacion))
            {
                dgvPlanillas.Rows.Add(
                    rutina.Nombre,
                    rutina.Profesor,
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

        // 🔥 MÉTODO PARA MOSTRAR EL DETALLE
        private void MostrarDetalleRutina(RutinaGuardada rutina)
        {
            // Ocultar la lista de planillas
            mainPanel.Visible = false;

            // Crear o mostrar el control de detalle
            if (ucDetalle == null)
            {
                ucDetalle = new UcDetalleRutina();
                ucDetalle.Dock = DockStyle.Fill;
                ucDetalle.OnCerrarDetalle += (s, e) => OcultarDetalle();
                this.Controls.Add(ucDetalle);
                ucDetalle.BringToFront(); // 🔥 AÑADIDO: Traer al frente
            }

            // Cargar datos en el detalle
            ucDetalle.CargarRutina(
                rutina.Nombre,
                rutina.TipoRutina,
                rutina.Profesor,
                rutina.FechaCreacion,
                rutina.Ejercicios
            );

            ucDetalle.Visible = true;
            ucDetalle.BringToFront(); // 🔥 AÑADIDO: Asegurar que esté al frente
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

        // 🔥 MÉTODO PARA CAPTURAR RUTINAS GUARDADAS DESDE GENERAR RUTINAS
        private void OnRutinaGuardada(object sender, RutinaGuardadaEventArgs e)
        {
            // Simular nombre de profesor (en un sistema real esto vendría del usuario logueado)
            string nombreProfesor = "Profesor Actual";

            var nuevaRutina = new RutinaGuardada
            {
                Nombre = e.NombreRutina,
                TipoRutina = e.TipoRutina,
                Profesor = nombreProfesor,
                FechaCreacion = e.FechaCreacion,
                Ejercicios = new List<Utils.RutinaSimulador.EjercicioRutina>(e.Ejercicios)
            };

            rutinasGuardadas.Add(nuevaRutina);
            ActualizarGrid();

            MessageBox.Show($"✅ Rutina guardada en Planillas: {e.NombreRutina}",
                          "Rutina Guardada", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 🔥 CLASE INTERNA PARA MANEJAR RUTINAS GUARDADAS
        private class RutinaGuardada
        {
            public string Nombre { get; set; } = "";
            public string TipoRutina { get; set; } = "";
            public string Profesor { get; set; } = "";
            public DateTime FechaCreacion { get; set; } = DateTime.Now;
            public List<Utils.RutinaSimulador.EjercicioRutina> Ejercicios { get; set; } = new List<Utils.RutinaSimulador.EjercicioRutina>();
        }
    }
}