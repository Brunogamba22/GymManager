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
        private Color warningColor = Color.FromArgb(255, 193, 7);
        private Color hoverColor = Color.FromArgb(238, 231, 235);

        // 🔥 VARIABLE PARA EL CONTROL DE DETALLES
        private UcDetalleRutina ucDetalle = null;

        private readonly RutinaController _rutinaController = new RutinaController();
        private readonly DetalleRutinaController _detalleController = new DetalleRutinaController();
        private readonly GeneroController _generoController = new GeneroController();

        
        // 🔥 LISTA PARA ALMACENAR RUTINAS GUARDADAS
        private List<Rutina> rutinasGuardadas = new List<Rutina>();

        public UcPlanillasRutinas()
        {
            InitializeComponent();
            ApplyModernStyles();
            ConfigurarGrid();
            ConfigurarFiltros();
            ReajustarLayout();

            // Asignar eventos a los botones de filtro
            btnFiltrar.Click += BtnFiltrar_Click;
            btnLimpiarFiltros.Click += BtnLimpiarFiltros_Click;
            btnExportar.Click += btnExportar_Click;

        }

        private void ApplyModernStyles()
        {
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI", 9);

            // Aplicar estilo a botones
            StyleButton(btnFiltrar, primaryColor);
            StyleButton(btnLimpiarFiltros, warningColor);
            StyleButton(btnExportar, successColor);
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


            dgvPlanillas.DefaultCellStyle.SelectionBackColor = Color.White; // Same as normal background
            dgvPlanillas.DefaultCellStyle.SelectionForeColor = textColor;

            // Filas alternadas
            dgvPlanillas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);

            // Evento de selección
            dgvPlanillas.SelectionChanged += DgvPlanillas_SelectionChanged;

            dgvPlanillas.CellMouseEnter += dgvPlanillas_CellMouseEnter;
            dgvPlanillas.CellMouseLeave += dgvPlanillas_CellMouseLeave;
        }

        // =========================================================
        // 🔥 NUEVO: Configuración de Filtros 🔥
        // =========================================================
        private void ConfigurarFiltros()
        {
            // Configurar DatePickers (fechas iniciales opcionales)
            dtpFechaDesde.Value = DateTime.Now.AddMonths(-1); // Ejemplo: último mes
            dtpFechaHasta.Value = DateTime.Now;

            // Cargar ComboBox de Géneros
            try
            {
                var generos = _generoController.ObtenerTodos();
                // Añadir opción "Todos" al principio
                generos.Insert(0, new Genero { Id = 0, Nombre = "Todos" });

                cmbGenero.DataSource = generos;
                cmbGenero.DisplayMember = "Nombre";
                cmbGenero.ValueMember = "Id";
                cmbGenero.SelectedIndex = 0; // Seleccionar "Todos" por defecto
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar géneros para el filtro: " + ex.Message);
                // Podrías deshabilitar el ComboBox si falla la carga
            }
        }



        public void CargarDatos()
        {
            AplicarFiltrosYCargarGrid();
            OcultarDetalle(); // Asegurarse de mostrar la lista
        }

        /// <summary>
        /// Obtiene los valores de los filtros y recarga la grilla.
        /// </summary>
        // En UcPlanillasRutinas.cs

        private void AplicarFiltrosYCargarGrid()
        {
            try
            {
                
                DateTime fechaDesde = dtpFechaDesde.Value.Date; // "03/10/2025 00:00:00"
                DateTime fechaHasta = dtpFechaHasta.Value.Date.AddDays(1).AddSeconds(-1); // "03/11/2025 23:59:59"

                int? idGenero = (int)cmbGenero.SelectedValue;
                if (idGenero == 0) idGenero = null;

                
                bool soloEditadas = chkSoloEditadas.Checked;

                rutinasGuardadas = _rutinaController.ObtenerTodasParaPlanilla(fechaDesde, fechaHasta, idGenero, soloEditadas);

                ActualizarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar planillas filtradas: {ex.Message}", "Error de Base de Datos",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // Evento del botón Filtrar
        private void BtnFiltrar_Click(object sender, EventArgs e)
        {
            AplicarFiltrosYCargarGrid();
        }

        // Evento del botón Limpiar Filtros
        private void BtnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            // Resetear controles a valores por defecto
            dtpFechaDesde.Value = DateTime.Now.AddMonths(-1);
            dtpFechaHasta.Value = DateTime.Now;
            cmbGenero.SelectedIndex = 0; // Seleccionar "Todos"

            // Recargar la grilla sin filtros (o con los filtros reseteados)
            AplicarFiltrosYCargarGrid();
        }

        // 🔥 ACTUALIZAR GRID CON LAS RUTINAS GUARDADAS
        private void ActualizarGrid()
        {
            dgvPlanillas.SelectionChanged -= DgvPlanillas_SelectionChanged;
            dgvPlanillas.Rows.Clear();

            foreach (var rutina in rutinasGuardadas.OrderByDescending(r => r.FechaCreacion))
            {
                dgvPlanillas.Rows.Add(
                    rutina.Nombre,
                    rutina.NombreProfesor,
                    rutina.NombreGenero, // Columna Nueva
                    rutina.FechaCreacion.ToString("dd/MM/yyyy HH:mm")
                );
            }
            dgvPlanillas.SelectionChanged += DgvPlanillas_SelectionChanged;
            if (dgvPlanillas.Rows.Count == 0)
            {
                dgvPlanillas.ClearSelection();
                // Podrías también ocultar el panel de detalles si estaba visible
                // OcultarDetalle(); // Descomenta si quieres volver a la lista si no hay resultados
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
            try
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

                // 🔹 1. Obtener los ejercicios asociados a la rutina seleccionada
                List<DetalleRutina> detallesDeRutina = _detalleController.ObtenerPorRutina(rutinaHeader.IdRutina);

                if (detallesDeRutina == null || detallesDeRutina.Count == 0)
                {
                    MessageBox.Show("Esta rutina no tiene ejercicios cargados para mostrar.",
                                    "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OcultarDetalle();
                    return;
                }

                // 🔹 2. Llamar correctamente al método con 6 parámetros
                ucDetalle.CargarRutina(
                    rutinaHeader.IdRutina,          // ✅ Este parámetro era el que faltaba
                    rutinaHeader.Nombre,
                    rutinaHeader.NombreGenero,
                    rutinaHeader.NombreProfesor,
                    rutinaHeader.FechaCreacion,
                    detallesDeRutina
                );

                ucDetalle.Visible = true;
                ucDetalle.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudieron cargar los detalles para '{rutinaHeader.Nombre}':\n{ex.Message}",
                                "Error al Cargar Detalles", MessageBoxButtons.OK, MessageBoxIcon.Error);
                OcultarDetalle();
            }
        }

        // 🔥 MÉTODO PARA OCULTAR EL DETALLE
        private void OcultarDetalle()
        {
            if (ucDetalle != null)
                ucDetalle.Visible = false;

            mainPanel.Visible = true;
            mainPanel.BringToFront();

            ReajustarLayout(); // ✅ corrige el layout y elimina la franja gris
        }

        private void ReajustarLayout()
        {
            this.SuspendLayout();

            // 🔹 Forzamos que el panel principal ocupe todo el espacio disponible
            if (mainPanel != null)
            {
                mainPanel.Dock = DockStyle.Fill;
                mainPanel.Padding = new Padding(0);
                mainPanel.Margin = new Padding(0);
            }

            // 🔹 Reforzamos que no quede un espacio sin pintar (gris)
            this.BackColor = backgroundColor;
            mainPanel.BackColor = backgroundColor;

            this.ResumeLayout();
            this.Refresh();
        }



        // Sobrecarga para botones con colores claros (como amarillo)
        private void StyleButton(Button btn, Color bgColor)
        {
            // Determinar si el color de fondo es "claro" (ej. amarillo, blanco, etc.)
            // Usamos una fórmula simple de luminancia percibida.
            bool esColorClaro = (0.299 * bgColor.R + 0.587 * bgColor.G + 0.114 * bgColor.B) / 255 > 0.5;

            // Si es claro, usar texto oscuro; si no, usar texto blanco.
            Color colorTexto = esColorClaro ? textColor : Color.White;

            // Llamar a la versión principal del método
            StyleButton(btn, bgColor, colorTexto);
        }

        // Método principal que aplica todos los estilos
        private void StyleButton(Button btn, Color bgColor, Color foreColor)
        {
            if (btn == null) return;
            btn.BackColor = bgColor;
            btn.ForeColor = foreColor; // Color de texto dinámico
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            // Reducimos un poco el padding horizontal para que quepa mejor
            btn.Padding = new Padding(12, 6, 12, 6);
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(bgColor, 0.1f);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(bgColor, 0.2f);

            // Lógica para botón deshabilitado (si la necesitas)
            btn.EnabledChanged += (s, e) => {
                if (!btn.Enabled)
                {
                    btn.BackColor = Color.FromArgb(220, 220, 220);
                    btn.ForeColor = Color.FromArgb(150, 150, 150);
                }
                else
                {
                    btn.BackColor = bgColor;
                    btn.ForeColor = foreColor;
                }
            };
            // Forzar estado inicial si está deshabilitado
            if (!btn.Enabled)
            {
                btn.BackColor = Color.FromArgb(220, 220, 220);
                btn.ForeColor = Color.FromArgb(150, 150, 150);
            }
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


        /// <summary>
        /// Se dispara cuando el mouse entra en CUALQUIER celda de la grilla.
        /// </summary>
        private void dgvPlanillas_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            // Asegurarse de que no sea la fila de encabezado (índice -1)
            if (e.RowIndex >= 0)
            {
                // Resaltar TODA la fila donde entró el mouse
                dgvPlanillas.Rows[e.RowIndex].DefaultCellStyle.BackColor = hoverColor;
            }
        }

        /// <summary>
        /// Se dispara cuando el mouse sale de CUALQUIER celda de la grilla.
        /// </summary>
        private void dgvPlanillas_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            // Asegurarse de que no sea la fila de encabezado
            if (e.RowIndex >= 0)
            {
                // Obtener la fila de la que salió el mouse
                DataGridViewRow row = dgvPlanillas.Rows[e.RowIndex];

                // Determinar el color original (blanco o gris alternado)
                Color originalColor;
                if (e.RowIndex % 2 == 0) // Fila par
                {
                    originalColor = dgvPlanillas.DefaultCellStyle.BackColor; // Normalmente Blanco
                }
                else // Fila impar
                {
                    originalColor = dgvPlanillas.AlternatingRowsDefaultCellStyle.BackColor; // El gris claro alternado
                }

                // Devolver la fila a su color original
                row.DefaultCellStyle.BackColor = originalColor;
            }
        }
        // --- FIN DE MÉTODOS AÑADIDOS ---


    }
}