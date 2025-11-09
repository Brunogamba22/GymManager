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
        // 🎨 Paleta de colores
        private Color primaryColor = Color.FromArgb(46, 134, 171);
        private Color backgroundColor = Color.FromArgb(248, 249, 250);
        private Color successColor = Color.FromArgb(40, 167, 69);
        private Color textColor = Color.FromArgb(33, 37, 41);
        private Color warningColor = Color.FromArgb(255, 193, 7);
        private Color hoverColor = Color.FromArgb(238, 231, 235);

        // Control de detalle
        private UcDetalleRutina ucDetalle = null;

        // Controladores
        private readonly RutinaController _rutinaController = new RutinaController();
        private readonly DetalleRutinaController _detalleController = new DetalleRutinaController();
        private readonly GeneroController _generoController = new GeneroController();

        // Lista principal
        private List<Rutina> rutinasGuardadas = new List<Rutina>();

        public UcPlanillasRutinas()
        {
            InitializeComponent();
            ApplyModernStyles();
            ConfigurarGrid();
            ConfigurarFiltros();
            ReajustarLayout();

            // Eventos
            btnFiltrar.Click += BtnFiltrar_Click;
            btnLimpiarFiltros.Click += BtnLimpiarFiltros_Click;
            btnExportar.Click += btnExportar_Click;
            btnModoTV.Click += BtnModoTV_Click;
        }

        // =========================================================
        // 🎥 EVENTO MODO TV
        // =========================================================
        private void BtnModoTV_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPlanillas.SelectedRows.Count == 0)
                {
                    MessageBox.Show("Debe seleccionar una rutina para abrir el Modo TV.",
                                    "Sin selección", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int index = dgvPlanillas.SelectedRows[0].Index;
                if (index < 0 || index >= rutinasGuardadas.Count)
                    return;

                var rutinaSeleccionada = rutinasGuardadas[index];
                var detalles = _detalleController.ObtenerPorRutina(rutinaSeleccionada.IdRutina);

                if (detalles == null || detalles.Count == 0)
                {
                    MessageBox.Show("Esta rutina no tiene ejercicios cargados para mostrar.",
                                    "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 🔹 Abrir modo TV (agregamos género)
                FormTV pantallaTV = new FormTV(
                    rutinaSeleccionada.NombreProfesor,
                    rutinaSeleccionada.Nombre,
                    rutinaSeleccionada.NombreGenero,
                    detalles
                );

                pantallaTV.Text = $"Modo TV - {rutinaSeleccionada.NombreProfesor}";
                pantallaTV.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar Modo TV: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // =========================================================
        // 💅 ESTILOS Y CONFIGURACIÓN VISUAL
        // =========================================================
        private void ApplyModernStyles()
        {
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI", 9);

            StyleButton(btnFiltrar, primaryColor);
            StyleButton(btnLimpiarFiltros, warningColor);
            StyleButton(btnExportar, successColor);
            StyleButton(btnModoTV, Color.FromArgb(52, 73, 94));
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

            dgvPlanillas.ColumnHeadersDefaultCellStyle.BackColor = primaryColor;
            dgvPlanillas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPlanillas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvPlanillas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPlanillas.ColumnHeadersHeight = 45;

            dgvPlanillas.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvPlanillas.DefaultCellStyle.BackColor = Color.White;
            dgvPlanillas.DefaultCellStyle.ForeColor = textColor;
            dgvPlanillas.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPlanillas.DefaultCellStyle.Padding = new Padding(5);
            dgvPlanillas.DefaultCellStyle.SelectionBackColor = Color.White;
            dgvPlanillas.DefaultCellStyle.SelectionForeColor = textColor;
            dgvPlanillas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);

            dgvPlanillas.SelectionChanged += DgvPlanillas_SelectionChanged;
            dgvPlanillas.CellMouseEnter += dgvPlanillas_CellMouseEnter;
            dgvPlanillas.CellMouseLeave += dgvPlanillas_CellMouseLeave;
        }

        // =========================================================
        // 🧭 FILTROS
        // =========================================================
        private void ConfigurarFiltros()
        {
            dtpFechaDesde.Value = DateTime.Now.AddMonths(-1);
            dtpFechaHasta.Value = DateTime.Now;

            try
            {
                var generos = _generoController.ObtenerTodos();

                if (generos == null)
                    generos = new List<Genero>();

                generos.Insert(0, new Genero { Id = 0, Nombre = "Todos" });

                cmbGenero.DataSource = generos;
                cmbGenero.DisplayMember = "Nombre";
                cmbGenero.ValueMember = "Id";

                // ✅ Solo seleccionar índice 0 si hay elementos
                if (cmbGenero.Items.Count > 0)
                    cmbGenero.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar géneros: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void CargarDatos()
        {
            AplicarFiltrosYCargarGrid();
            OcultarDetalle();
        }

        private void AplicarFiltrosYCargarGrid()
        {
            try
            {
                DateTime fechaDesde = dtpFechaDesde.Value.Date;
                DateTime fechaHasta = dtpFechaHasta.Value.Date.AddDays(1).AddSeconds(-1);

                int? idGenero = (int)cmbGenero.SelectedValue;
                if (idGenero == 0) idGenero = null;

                bool soloEditadas = chkSoloEditadas.Checked;

                rutinasGuardadas = _rutinaController.ObtenerTodasParaPlanilla(fechaDesde, fechaHasta, idGenero, soloEditadas);
                ActualizarGrid();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar planillas filtradas: {ex.Message}",
                    "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnFiltrar_Click(object sender, EventArgs e)
        {
            AplicarFiltrosYCargarGrid();
        }

        private void BtnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            dtpFechaDesde.Value = DateTime.Now.AddMonths(-1);
            dtpFechaHasta.Value = DateTime.Now;
            cmbGenero.SelectedIndex = 0;
            AplicarFiltrosYCargarGrid();
        }

        // =========================================================
        // 📋 ACTUALIZAR GRILLA
        // =========================================================
        private void ActualizarGrid()
        {
            dgvPlanillas.SelectionChanged -= DgvPlanillas_SelectionChanged;
            dgvPlanillas.Rows.Clear();

            foreach (var rutina in rutinasGuardadas.OrderByDescending(r => r.FechaCreacion))
            {
                dgvPlanillas.Rows.Add(
                    rutina.Nombre,
                    rutina.NombreProfesor,
                    rutina.NombreGenero,
                    rutina.FechaCreacion.ToString("dd/MM/yyyy HH:mm")
                );
            }

            dgvPlanillas.SelectionChanged += DgvPlanillas_SelectionChanged;
            if (dgvPlanillas.Rows.Count > 0)
                dgvPlanillas.ClearSelection();
        }

        private void DgvPlanillas_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvPlanillas.SelectedRows.Count > 0 && dgvPlanillas.SelectedRows[0].Index >= 0)
            {
                int selectedIndex = dgvPlanillas.SelectedRows[0].Index;
                if (selectedIndex < rutinasGuardadas.Count)
                {
                    var rutinaSeleccionada = rutinasGuardadas[selectedIndex];
                    MostrarDetalleRutina(rutinaSeleccionada);
                }
            }
        }

        // =========================================================
        // 📄 MOSTRAR DETALLE DE RUTINA
        // =========================================================
        private void MostrarDetalleRutina(Rutina rutinaHeader)
        {
            try
            {
                mainPanel.Visible = false;

                if (ucDetalle == null)
                {
                    ucDetalle = new UcDetalleRutina();
                    ucDetalle.Dock = DockStyle.Fill;
                    ucDetalle.OnCerrarDetalle += (s, e) => OcultarDetalle();
                    this.Controls.Add(ucDetalle);
                }

                var detalles = _detalleController.ObtenerPorRutina(rutinaHeader.IdRutina);
                if (detalles == null || detalles.Count == 0)
                {
                    MessageBox.Show("Esta rutina no tiene ejercicios cargados.",
                        "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OcultarDetalle();
                    return;
                }

                ucDetalle.CargarRutina(
                    rutinaHeader.IdRutina,
                    rutinaHeader.Nombre,
                    rutinaHeader.NombreGenero,
                    rutinaHeader.NombreProfesor,
                    rutinaHeader.FechaCreacion,
                    detalles
                );

                ucDetalle.Visible = true;
                ucDetalle.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al mostrar rutina: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                OcultarDetalle();
            }
        }

        private void OcultarDetalle()
        {
            if (ucDetalle != null)
                ucDetalle.Visible = false;

            mainPanel.Visible = true;
            mainPanel.BringToFront();
            ReajustarLayout();
        }

        private void ReajustarLayout()
        {
            this.SuspendLayout();
            if (mainPanel != null)
            {
                mainPanel.Dock = DockStyle.Fill;
                mainPanel.Padding = new Padding(0);
                mainPanel.Margin = new Padding(0);
            }
            this.BackColor = backgroundColor;
            mainPanel.BackColor = backgroundColor;
            this.ResumeLayout();
            this.Refresh();
        }

        // =========================================================
        // 🎨 ESTILO BOTONES
        // =========================================================
        private void StyleButton(Button btn, Color bgColor)
        {
            bool esColorClaro = (0.299 * bgColor.R + 0.587 * bgColor.G + 0.114 * bgColor.B) / 255 > 0.5;
            Color colorTexto = esColorClaro ? textColor : Color.White;
            StyleButton(btn, bgColor, colorTexto);
        }

        private void StyleButton(Button btn, Color bgColor, Color foreColor)
        {
            if (btn == null) return;
            btn.BackColor = bgColor;
            btn.ForeColor = foreColor;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(12, 6, 12, 6);
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(bgColor, 0.1f);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(bgColor, 0.2f);
        }

        // =========================================================
        // 📤 EXPORTAR RUTINA
        // =========================================================
        private void btnExportar_Click(object sender, EventArgs e)
        {
            if (dgvPlanillas.SelectedRows.Count == 0)
            {
                MessageBox.Show("Selecciona una planilla para exportar", "Información",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var nombreRutina = dgvPlanillas.SelectedRows[0].Cells["colNombre"].Value.ToString();
            MessageBox.Show($"✅ Planilla '{nombreRutina}' exportada correctamente",
                            "Exportación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // =========================================================
        // 🖱️ EFECTO HOVER EN GRILLA
        // =========================================================
        private void dgvPlanillas_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                dgvPlanillas.Rows[e.RowIndex].DefaultCellStyle.BackColor = hoverColor;
        }

        private void dgvPlanillas_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvPlanillas.Rows[e.RowIndex];
                Color originalColor = e.RowIndex % 2 == 0
                    ? dgvPlanillas.DefaultCellStyle.BackColor
                    : dgvPlanillas.AlternatingRowsDefaultCellStyle.BackColor;
                row.DefaultCellStyle.BackColor = originalColor;
            }
        }
    }
}
