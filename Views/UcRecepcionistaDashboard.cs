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
    public partial class UcRecepcionistaDashboard : UserControl
    {
        // Colores
        private Color primaryColor = Color.FromArgb(46, 134, 171);
        private Color backgroundColor = Color.FromArgb(248, 249, 250);
        private Color successColor = Color.FromArgb(40, 167, 69); // Imprimir
        private Color infoColor = Color.FromArgb(23, 162, 184);   // Exportar
        private Color darkColor = Color.FromArgb(52, 58, 64);    // TV
        private Color textColor = Color.FromArgb(33, 37, 41);
        private Color warningColor = Color.FromArgb(255, 193, 7); // Limpiar
        private Color hoverColor = Color.FromArgb(228, 231, 235);
        private Color gridLineColor = Color.FromArgb(235, 237, 240);

        // --- 🔥 MODIFICADO: Panel de detalle ---
        private UcDetalleRutina ucDetalle = null;

        // Controladores
        private readonly RutinaController _rutinaController = new RutinaController();
        private readonly DetalleRutinaController _detalleController = new DetalleRutinaController();
        private readonly GeneroController _generoController = new GeneroController();
        private readonly UsuarioController _usuarioController = new UsuarioController();

        // --- 🔥 MODIFICADO: Lista de Encabezados de Rutina ---
        private List<Rutina> rutinasGuardadas = new List<Rutina>();

        public UcRecepcionistaDashboard()
        {
            InitializeComponent();
            ApplyModernStyles();
            ConfigurarGrid();
            ConfigurarFiltros();

            // Asignar eventos
            btnFiltrar.Click += BtnFiltrar_Click;
            btnImprimir.Click += BtnImprimir_Click;
            btnExportar.Click += BtnExportar_Click;
            btnModoTV.Click += BtnModoTV_Click;
            btnLimpiarFiltros.Click += BtnLimpiarFiltros_Click;

            // --- 🔥 NUEVO: Evento Doble Clic para ver detalle ---
            dgvPlanillas.CellDoubleClick += DgvPlanillas_CellDoubleClick;
        }

        private void ApplyModernStyles()
        {
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI", 9);

            StyleButton(btnFiltrar, primaryColor);
            StyleButton(btnImprimir, successColor);
            StyleButton(btnExportar, infoColor);
            StyleButton(btnModoTV, darkColor);
            StyleButton(btnLimpiarFiltros, warningColor);
        }

        private void ConfigurarGrid()
        {
            // --- 🔥 CORRECCIÓN: Usa dgvPlanillas ---
            dgvPlanillas.BackgroundColor = Color.White;
            dgvPlanillas.BorderStyle = BorderStyle.None;
            dgvPlanillas.GridColor = gridLineColor;
            dgvPlanillas.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPlanillas.EnableHeadersVisualStyles = false;
            dgvPlanillas.AllowUserToAddRows = false;
            dgvPlanillas.AllowUserToDeleteRows = false;
            dgvPlanillas.ReadOnly = true;
            dgvPlanillas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPlanillas.RowHeadersVisible = false;
            dgvPlanillas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPlanillas.RowTemplate.Height = 55;
            dgvPlanillas.AllowUserToResizeColumns = false;
            dgvPlanillas.AllowUserToResizeRows = false;
            dgvPlanillas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgvPlanillas.ColumnHeadersDefaultCellStyle.BackColor = Color.White;
            dgvPlanillas.ColumnHeadersDefaultCellStyle.ForeColor = textColor;
            dgvPlanillas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 11f, FontStyle.Bold);
            dgvPlanillas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPlanillas.ColumnHeadersHeight = 50;

            dgvPlanillas.DefaultCellStyle.Font = new Font("Segoe UI", 12f);
            dgvPlanillas.DefaultCellStyle.BackColor = Color.White;
            dgvPlanillas.DefaultCellStyle.ForeColor = textColor;
            dgvPlanillas.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPlanillas.DefaultCellStyle.Padding = new Padding(5);
            dgvPlanillas.DefaultCellStyle.SelectionBackColor = primaryColor;
            dgvPlanillas.DefaultCellStyle.SelectionForeColor = Color.White;
            dgvPlanillas.AlternatingRowsDefaultCellStyle.BackColor = backgroundColor;

            dgvPlanillas.SelectionChanged += DgvPlanillas_SelectionChanged;
            dgvPlanillas.CellMouseEnter += dgvPlanillas_CellMouseEnter;
            dgvPlanillas.CellMouseLeave += dgvPlanillas_CellMouseLeave;
        }

        private void ConfigurarFiltros()
        {
            // ... (Tu código de ConfigurarFiltros es correcto, se queda igual) ...
            dtpFecha.Value = DateTime.Now.Date;
            try
            {
                var generos = _generoController.ObtenerTodos();
                generos.Insert(0, new Genero { Id = 0, Nombre = "Todas" });
                cmbGenero.DataSource = generos;
                cmbGenero.DisplayMember = "Nombre";
                cmbGenero.ValueMember = "Id";
                cmbGenero.SelectedIndex = 0;
            }
            catch (Exception ex) { MessageBox.Show("Error al cargar géneros: " + ex.Message); }

            try
            {
                var profesores = _usuarioController.ObtenerProfesores();
                profesores.Insert(0, new Usuario { IdUsuario = 0, Apellido = "Todos" });
                cmbProfesor.DataSource = profesores;
                cmbProfesor.DisplayMember = "FullName";
                cmbProfesor.ValueMember = "IdUsuario";
                cmbProfesor.SelectedIndex = 0;
            }
            catch (Exception ex) { MessageBox.Show("Error al cargar profesores: " + ex.Message); }
        }

        // =========================================================
        // LÓGICA DE CARGA Y FILTRADO (MODO INTERACTIVO)
        // =========================================================

        public void CargarDatos()
        {
            CargarGrillaConFiltros();
            OcultarDetalle();
        }

        private void BtnFiltrar_Click(object sender, EventArgs e)
        {
            CargarGrillaConFiltros();
        }

        private void BtnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            dtpFecha.Value = DateTime.Now.Date;
            cmbProfesor.SelectedIndex = 0;
            cmbGenero.SelectedIndex = 0;
            if (chkSoloEditadas != null) chkSoloEditadas.Checked = false;

            CargarGrillaConFiltros();
        }

        private void CargarGrillaConFiltros()
        {
            try
            {
                DateTime fecha = dtpFecha.Value.Date;
                int? idGenero = (int)cmbGenero.SelectedValue;
                int? idProfesor = (int)cmbProfesor.SelectedValue;
                bool soloEditadas = (chkSoloEditadas != null) && chkSoloEditadas.Checked;

                if (idGenero == 0) idGenero = null;
                if (idProfesor == 0) idProfesor = null;

                // --- 🔥 CORRECCIÓN: Solo buscamos los encabezados ---
                rutinasGuardadas = _rutinaController.ObtenerTodasParaPlanilla(
                    fecha, fecha, idGenero, soloEditadas, idProfesor);

                if (rutinasGuardadas.Count == 0)
                {
                    MessageBox.Show("No se encontraron rutinas para los filtros seleccionados.", "Sin Resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvPlanillas.Rows.Clear();
                    HabilitarBotonesDeAccion(false);
                    return;
                }

                // Actualizamos la grilla con los ENCABEZADOS
                ActualizarGrid(rutinasGuardadas);
                HabilitarBotonesDeAccion(false); // Botones se habilitan al seleccionar
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar planilla: {ex.Message}", "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                HabilitarBotonesDeAccion(false);
            }
        }

        private void HabilitarBotonesDeAccion(bool habilitar)
        {
            btnImprimir.Enabled = habilitar;
            btnExportar.Enabled = habilitar;
        }

        // --- 🔥 CORRECCIÓN: Este método ahora recibe la lista de Rutinas (encabezados) ---
        private void ActualizarGrid(List<Rutina> rutinas)
        {
            dgvPlanillas.SelectionChanged -= DgvPlanillas_SelectionChanged; // Desactivar evento
            dgvPlanillas.Rows.Clear();

            foreach (var rutina in rutinas) // Ya no es 'detalles'
            {
                dgvPlanillas.Rows.Add(
                    rutina.Nombre,
                    rutina.NombreProfesor,
                    rutina.NombreGenero,
                    rutina.FechaCreacion.ToString("dd/MM/yyyy HH:mm")
                );
            }

            dgvPlanillas.SelectionChanged += DgvPlanillas_SelectionChanged; // Reactivar evento
            if (dgvPlanillas.Rows.Count > 0)
            {
                dgvPlanillas.ClearSelection(); // Limpiar selección inicial
            }
        }

        // =========================================================
        // LÓGICA DE NAVEGACIÓN Y ACCIONES
        // =========================================================

        private void DgvPlanillas_SelectionChanged(object sender, EventArgs e)
        {
            // Habilitar botones si hay una fila seleccionada
            HabilitarBotonesDeAccion(dgvPlanillas.SelectedRows.Count > 0);
        }

        // --- 🔥 NUEVO: Mostrar detalle al hacer DOBLE CLIC ---
        private void DgvPlanillas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) // Asegurarse de que no sea el encabezado
            {
                if (e.RowIndex < rutinasGuardadas.Count)
                {
                    // Obtener la rutina (encabezado) de la fila
                    var rutinaSeleccionada = rutinasGuardadas[e.RowIndex];
                    // Mostrar el panel de detalle
                    MostrarDetalleRutina(rutinaSeleccionada);
                }
            }
        }

        private void MostrarDetalleRutina(Rutina rutinaHeader)
        {
            try
            {
                // Ocultar la vista principal
                mainPanel.Visible = false;

                if (ucDetalle == null)
                {
                    ucDetalle = new UcDetalleRutina();
                    ucDetalle.Dock = DockStyle.Fill;
                    ucDetalle.OnCerrarDetalle += (s, e) => OcultarDetalle();
                    this.Controls.Add(ucDetalle);
                }

                // Buscar los detalles (ejercicios) SÓLO cuando se piden
                List<DetalleRutina> detallesDeRutina = _detalleController.ObtenerPorRutina(rutinaHeader.IdRutina);

                // Cargar el panel de detalle
                ucDetalle.CargarRutina(
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
                MessageBox.Show($"No se pudieron cargar los detalles: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                OcultarDetalle();
            }
        }

        private void OcultarDetalle()
        {
            if (ucDetalle != null) ucDetalle.Visible = false;
            mainPanel.Visible = true;
            mainPanel.BringToFront();
        }

        // =========================================================
        // BOTONES DE ACCIÓN (Imprimir, Exportar, Modo TV)
        // =========================================================

        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            MessageBox.Show("✅ Rutina enviada a impresión (Lógica no implementada)", "Imprimir",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnExportar_Click(object sender, EventArgs e)
        {
            var nombreRutina = dgvPlanillas.SelectedRows[0].Cells["colNombre"].Value.ToString();
            MessageBox.Show($"✅ Planilla '{nombreRutina}' exportada (Lógica no implementada)", "Exportación Exitosa",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnModoTV_Click(object sender, EventArgs e)
        {
            try
            {
                DateTime fechaSeleccionada = dtpFecha.Value.Date;

                // Buscar las 3 rutinas del día
                var detallesM = ObtenerDetallesPorGenero(fechaSeleccionada, "Masculino");
                var detallesF = ObtenerDetallesPorGenero(fechaSeleccionada, "Femenino");
                var detallesD = ObtenerDetallesPorGenero(fechaSeleccionada, "Deportistas");

                FormTV pantallaTV = new FormTV(fechaSeleccionada, detallesM, detallesF, detallesD);
                pantallaTV.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar Modo TV: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private List<DetalleRutina> ObtenerDetallesPorGenero(DateTime fecha, string nombreGenero)
        {
            var genero = _generoController.ObtenerTodos().FirstOrDefault(g => g.Nombre.Equals(nombreGenero, StringComparison.OrdinalIgnoreCase));
            if (genero == null) return new List<DetalleRutina>();

            var rutinas = _rutinaController.ObtenerTodasParaPlanilla(fecha, fecha, genero.Id);

            if (rutinas.Count > 0)
            {
                return _detalleController.ObtenerPorRutina(rutinas.First().IdRutina);
            }

            return new List<DetalleRutina>();
        }

        // =========================================================
        // MÉTODOS DE ESTILO Y HOVER (Sin cambios)
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
            btn.Font = new Font("Segoe UI", 9.5f, FontStyle.Bold); // Fuente ligeramente más grande
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(12, 6, 12, 6);
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(bgColor, 0.1f);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(bgColor, 0.2f);

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
            if (!btn.Enabled)
            {
                btn.BackColor = Color.FromArgb(220, 220, 220);
                btn.ForeColor = Color.FromArgb(150, 150, 150);
            }
        }

        private void dgvPlanillas_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                dgvPlanillas.Rows[e.RowIndex].DefaultCellStyle.BackColor = hoverColor;
        }

        private void dgvPlanillas_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPlanillas.Rows[e.RowIndex];
                Color originalColor = (e.RowIndex % 2 == 0) ? dgvPlanillas.DefaultCellStyle.BackColor : dgvPlanillas.AlternatingRowsDefaultCellStyle.BackColor;
                row.DefaultCellStyle.BackColor = originalColor;
            }
        }

        // (La clase FormPantallaCompleta ya no es necesaria aquí)
    }

    // ====================================================================
    // 🔥 NUEVA CLASE: FormTV (Pantalla Completa con 3 Grillas) 🔥
    // ====================================================================
    // (Esta clase puede ir al final de tu archivo UcRecepcionistaDashboard.cs)

    public class FormTV : Form
    {
        private Color tvBackColor = Color.FromArgb(33, 37, 41); // Fondo oscuro
        private Color tvGridColor = Color.FromArgb(52, 58, 64); // Bordes
        private Color tvHeaderColor = Color.FromArgb(46, 134, 171); // Azul
        private Color tvTitleColor = Color.FromArgb(255, 193, 7); // Amarillo
        private Font tvGridFont = new Font("Segoe UI", 12f);
        private Font tvHeaderFont = new Font("Segoe UI", 14f, FontStyle.Bold);
        private Font tvTitleFont = new Font("Segoe UI", 16f, FontStyle.Bold);

        public FormTV(DateTime fecha, List<DetalleRutina> detallesM, List<DetalleRutina> detallesF, List<DetalleRutina> detallesD)
        {
            InitializeForm();

            // Llenar las 3 grillas
            lblTituloTV.Text = $"RUTINAS DEL DÍA: {fecha:dd/MM/yyyy}";
            PoblarGrilla(dgvMasculino, detallesM, lblTVMasculino, "MASCULINO");
            PoblarGrilla(dgvFemenino, detallesF, lblTVFemenino, "FEMENINO");
            PoblarGrilla(dgvDeportista, detallesD, lblTVDeportista, "DEPORTISTAS");
        }

        private DataGridView dgvMasculino, dgvFemenino, dgvDeportista;
        private Label lblTituloTV, lblTVMasculino, lblTVFemenino, lblTVDeportista;

        private void PoblarGrilla(DataGridView dgv, List<DetalleRutina> detalles, Label lbl, string titulo)
        {
            lbl.Text = titulo;
            dgv.Rows.Clear();

            if (detalles.Count == 0)
            {
                lbl.Text += " (NO DISPONIBLE)";
                lbl.ForeColor = Color.Gray;
                return;
            }

            lbl.ForeColor = tvTitleColor;
            foreach (var d in detalles)
            {
                dgv.Rows.Add(d.EjercicioNombre, d.Series, d.Repeticiones, d.Carga?.ToString() ?? "");
            }
        }

        private void InitializeForm()
        {
            this.Text = "Modo TV - Presione ESC para salir";
            this.WindowState = FormWindowState.Maximized;
            this.BackColor = tvBackColor;
            this.FormBorderStyle = FormBorderStyle.None;
            this.KeyPreview = true;
            this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) this.Close(); };

            // Panel Principal
            TableLayoutPanel tlpMain = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                BackColor = tvBackColor,
                RowCount = 3,
                ColumnCount = 3
            };
            this.Controls.Add(tlpMain);

            // Definición de Filas
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 60)); // Fila 0: Título principal
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 50)); // Fila 1: Títulos de género
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); // Fila 2: Grillas

            // Definición de Columnas
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            tlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));

            // Fila 0: Título Principal
            lblTituloTV = new Label { Dock = DockStyle.Fill, Font = new Font("Segoe UI", 24, FontStyle.Bold), ForeColor = Color.White, Text = "RUTINAS DEL DÍA", TextAlign = ContentAlignment.MiddleCenter };
            tlpMain.Controls.Add(lblTituloTV, 0, 0);
            tlpMain.SetColumnSpan(lblTituloTV, 3); // Ocupa las 3 columnas

            // Fila 1: Títulos de Género
            lblTVMasculino = new Label { Dock = DockStyle.Fill, Font = tvTitleFont, ForeColor = tvTitleColor, Text = "MASCULINO", TextAlign = ContentAlignment.BottomCenter };
            lblTVFemenino = new Label { Dock = DockStyle.Fill, Font = tvTitleFont, ForeColor = tvTitleColor, Text = "FEMENINO", TextAlign = ContentAlignment.BottomCenter };
            lblTVDeportista = new Label { Dock = DockStyle.Fill, Font = tvTitleFont, ForeColor = tvTitleColor, Text = "DEPORTISTAS", TextAlign = ContentAlignment.BottomCenter };
            tlpMain.Controls.Add(lblTVMasculino, 0, 1);
            tlpMain.Controls.Add(lblTVFemenino, 1, 1);
            tlpMain.Controls.Add(lblTVDeportista, 2, 1);

            // Fila 2: Grillas
            dgvMasculino = CrearGrillaTV();
            dgvFemenino = CrearGrillaTV();
            dgvDeportista = CrearGrillaTV();
            tlpMain.Controls.Add(dgvMasculino, 0, 2);
            tlpMain.Controls.Add(dgvFemenino, 1, 2);
            tlpMain.Controls.Add(dgvDeportista, 2, 2);
        }

        private DataGridView CrearGrillaTV()
        {
            DataGridView dgv = new DataGridView
            {
                Dock = DockStyle.Fill,
                BackgroundColor = tvBackColor,
                BorderStyle = BorderStyle.None,
                GridColor = tvGridColor,
                CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal,
                EnableHeadersVisualStyles = false,
                AllowUserToAddRows = false,
                ReadOnly = true,
                RowHeadersVisible = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
                RowTemplate = { Height = 55 },
                AllowUserToResizeRows = false,
                ColumnHeadersVisible = true,
                ColumnHeadersHeight = 50,
                ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            };

            // Estilos
            dgv.ColumnHeadersDefaultCellStyle.BackColor = tvHeaderColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = tvHeaderFont;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.DefaultCellStyle.Font = tvGridFont;
            dgv.DefaultCellStyle.BackColor = tvBackColor;
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.Padding = new Padding(5);
            dgv.DefaultCellStyle.SelectionBackColor = tvBackColor;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 44, 52); // Gris oscuro alternado

            // Columnas
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "EJERCICIO", Name = "colEj", FillWeight = 40 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "SERIES", Name = "colSer", FillWeight = 15 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "REPS", Name = "colRep", FillWeight = 15 });
            dgv.Columns.Add(new DataGridViewTextBoxColumn { HeaderText = "CARGA %", Name = "colCar", FillWeight = 15 });

            return dgv;
        }
    }
}