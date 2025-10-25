using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GymManager.Controllers;
using GymManager.Models;
using GymManager.Utils;
using GymManager.Forms;

namespace GymManager.Views
{
    public partial class UcEditarRutina : UserControl
    {
        // --- Colores ---
        private Color primaryColor = Color.FromArgb(46, 134, 171);
        private Color backgroundColor = Color.FromArgb(248, 249, 250);
        private Color successColor = Color.FromArgb(40, 167, 69);
        private Color warningColor = Color.FromArgb(255, 193, 7);
        private Color dangerColor = Color.FromArgb(220, 53, 69);
        private Color textColor = Color.FromArgb(33, 37, 41);
        private Color lightGrayColor = Color.FromArgb(220, 220, 220);

        // --- Rutinas disponibles (Encabezados) ---
        private List<DetalleRutina> _rutinaHombreGenerada;
        private List<DetalleRutina> _rutinaMujerGenerada;
        private List<DetalleRutina> _rutinaDeportistaGenerada;

        // --- Variables para la rutina en edición ---
        private List<DetalleRutina> _rutinaActualParaEditar;
        private string _tipoRutinaActual = "";

        // --- Controladores ---
        private readonly RutinaController _rutinaController = new RutinaController();
        private readonly DetalleRutinaController _detalleRutinaController = new DetalleRutinaController();
        private readonly GeneroController _generoController = new GeneroController();
        private readonly EjercicioController _ejercicioController = new EjercicioController();
        private List<Genero> _listaDeGeneros = new List<Genero>();

        private FrmMain _frmMain;

        private ContextMenuStrip menuEjercicios;

        public UcEditarRutina()
        {
            InitializeComponent();
            ApplyModernStyles();
            ConfigurarGrid();
            ConfigurarMenuContextual();

            try { _listaDeGeneros = _generoController.ObtenerTodos(); }
            catch (Exception ex) { MessageBox.Show("Error al cargar géneros: " + ex.Message); }
        }

        private void UcEditarRutina_Load(object sender, EventArgs e)
        {
            // Guardamos la referencia al formulario principal
            _frmMain = this.ParentForm as FrmMain;

            // Aplicar estilos
            StyleButton(btnGuardar, successColor);
            StyleButton(btnAgregarEjercicio, primaryColor);
            StyleButton(btnEliminarEjercicio, dangerColor);
            StyleButton(btnLimpiarTodo, warningColor);
            StyleButton(btnVolver, lightGrayColor, textColor);
            StyleButton(btnSeleccionarRutinaHombre, primaryColor);
            StyleButton(btnSeleccionarRutinaMujer, primaryColor);
            StyleButton(btnSeleccionarRutinaDeportista, primaryColor);

            // NO mostramos ningún panel por defecto.
            pnlEdicion.Visible = false;
            pnlSeleccion.Visible = false;
        }

        

        private void MostrarPanelEdicion(bool mostrar)
        {
            pnlEdicion.Visible = mostrar;
            pnlSeleccion.Visible = !mostrar;

            if (!mostrar)
            {
                dgvRutinas.Rows.Clear();
                _rutinaActualParaEditar = null;
                _tipoRutinaActual = "";
                lblTitulo.Text = "✏️ EDITAR RUTINA";
            }
        }

        // ====================================================================
        // EVENTOS DEL PANEL DE SELECCIÓN
        // ====================================================================

        private void btnSeleccionarRutinaHombre_Click(object sender, EventArgs e)
        {
            CargarRutinaGeneradaParaEditar(_rutinaHombreGenerada, "Hombres");
        }

        private void btnSeleccionarRutinaMujer_Click(object sender, EventArgs e)
        {
            CargarRutinaGeneradaParaEditar(_rutinaMujerGenerada, "Mujeres");
        }

        private void btnSeleccionarRutinaDeportista_Click(object sender, EventArgs e)
        {
            CargarRutinaGeneradaParaEditar(_rutinaDeportistaGenerada, "Deportistas");
        }

        

        // ====================================================================
        // MÉTODOS PÚBLICOS (Los llamará FrmMain)
        // ====================================================================

        /// <summary>
        /// (ATAJO) Carga una rutina generada (en memoria) directamente en el editor.
        /// </summary>
        public void CargarRutinaGeneradaParaEditar(List<DetalleRutina> rutina, string tipoRutina)
        {
            CargarRutinaParaEditar(rutina, tipoRutina);
            MostrarPanelEdicion(true); // true = mostrar panel de edición
        }

        /// <summary>
        /// (RESET) Muestra el panel de selección y actualiza la disponibilidad
        /// basado en las listas de UcGenerarRutinas.
        /// </summary>
        public void ActualizarYMostrarPanelSeleccion(List<DetalleRutina> hombres, List<DetalleRutina> mujeres, List<DetalleRutina> deportistas)
        {
            // 1. Guardar las listas
            _rutinaHombreGenerada = hombres;
            _rutinaMujerGenerada = mujeres;
            _rutinaDeportistaGenerada = deportistas;

            // 2. Habilitar/Deshabilitar botones
            btnSeleccionarRutinaHombre.Enabled = (hombres != null && hombres.Count > 0);
            btnSeleccionarRutinaMujer.Enabled = (mujeres != null && mujeres.Count > 0);
            btnSeleccionarRutinaDeportista.Enabled = (deportistas != null && deportistas.Count > 0);

            // 3. Actualizar texto de botones
            btnSeleccionarRutinaHombre.Text = btnSeleccionarRutinaHombre.Enabled ? "HOMBRES (Generada)" : "HOMBRES (No generada)";
            btnSeleccionarRutinaMujer.Text = btnSeleccionarRutinaMujer.Enabled ? "MUJERES (Generada)" : "MUJERES (No generada)";
            btnSeleccionarRutinaDeportista.Text = btnSeleccionarRutinaDeportista.Enabled ? "DEPORTISTAS (Generada)" : "DEPORTISTAS (No generada)";

            // 4. Mostrar el panel de selección
            MostrarPanelEdicion(false); // false = mostrar selección
        }


        // ====================================================================
        // LÓGICA DE EDICIÓN
        // ====================================================================

        // ESTE MÉTODO ES PRIVADO. FrmMain NO debe llamarlo directamente.
        private void CargarRutinaParaEditar(List<DetalleRutina> rutina, string tipoRutina)
        {
            _rutinaActualParaEditar = rutina;
            _tipoRutinaActual = tipoRutina;
            dgvRutinas.Rows.Clear();

            if (_rutinaActualParaEditar == null) return;

            foreach (var detalle in _rutinaActualParaEditar)
            {
                dgvRutinas.Rows.Add(
                    detalle.EjercicioNombre,
                    detalle.Series,
                    detalle.Repeticiones,
                    detalle.Descanso
                );
            }

            lblTitulo.Text = $"✏️ EDITAR RUTINA - {tipoRutina}";
            lblDescripcion.Text = $"Modificá series, repeticiones o agregá/quitá ejercicios.";
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            int rowCount = dgvRutinas.Rows.Count - (dgvRutinas.AllowUserToAddRows ? 1 : 0);
            if (rowCount <= 0) { /* ... */ return; }
            for (int i = 0; i < rowCount; i++)
            {
                var row = dgvRutinas.Rows[i];
                if (string.IsNullOrWhiteSpace(row.Cells["Ejercicio"].Value?.ToString())) { /* ... */ return; }
                if (!int.TryParse(row.Cells["Series"].Value?.ToString(), out _) ||
                    !int.TryParse(row.Cells["Repeticiones"].Value?.ToString(), out _) ||
                    !int.TryParse(row.Cells["Descanso"].Value?.ToString(), out _)) { /* ... */ return; }
            }
            GuardarRutinaEditadaEnBD();
        }

        private void GuardarRutinaEditadaEnBD()
        {
            try
            {
                if (Sesion.Actual == null) throw new InvalidOperationException("No hay un usuario logueado.");
                var rutinaModificada = new List<DetalleRutina>();
                int rowCount = dgvRutinas.Rows.Count - (dgvRutinas.AllowUserToAddRows ? 1 : 0);
                for (int i = 0; i < rowCount; i++)
                {
                    var row = dgvRutinas.Rows[i];
                    string nombreEjercicio = row.Cells["Ejercicio"].Value?.ToString();
                    if (string.IsNullOrEmpty(nombreEjercicio)) continue;
                    var ejercicioDb = _ejercicioController.ObtenerPorNombre(nombreEjercicio);
                    int idEjercicioReal = ejercicioDb?.Id ?? 0;
                    if (idEjercicioReal == 0) { /* ... */ return; }
                    rutinaModificada.Add(new DetalleRutina
                    {
                        IdEjercicio = idEjercicioReal,
                        EjercicioNombre = nombreEjercicio,
                        Series = Convert.ToInt32(row.Cells["Series"].Value),
                        Repeticiones = Convert.ToInt32(row.Cells["Repeticiones"].Value),
                        Descanso = Convert.ToInt32(row.Cells["Descanso"].Value)
                    });
                }
                if (rutinaModificada.Count == 0) { /* ... */ return; }

                var generoEncontrado = _listaDeGeneros.FirstOrDefault(g => g.Nombre.Equals(_tipoRutinaActual, StringComparison.OrdinalIgnoreCase));
                int idGeneroParaGuardar = generoEncontrado?.Id ?? 1;
                string nombreRutina = $"Rutina Editada {_tipoRutinaActual} - {DateTime.Now:dd/MM/yyyy}";
                int nuevoIdRutina = _rutinaController.CrearEncabezadoRutina(_tipoRutinaActual, Sesion.Actual.IdUsuario, nombreRutina, idGeneroParaGuardar);
                foreach (var detalle in rutinaModificada)
                {
                    detalle.IdRutina = nuevoIdRutina;
                    _rutinaController.AgregarDetalle(detalle);
                }
                MessageBox.Show("Rutina modificada guardada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                // 1. Avisa a FrmMain que limpie la lista generada
                _frmMain?.LimpiarRutinaGeneradaEnPanel(_tipoRutinaActual);

                // 2. Pide a FrmMain que refresque este panel de selección
                //    (para que el botón ahora aparezca como "No generada")
                _frmMain?.MostrarEditarRutina();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la rutina: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnVolver_Click(object sender, EventArgs e)
        {
            int rowCount = dgvRutinas.Rows.Count - (dgvRutinas.AllowUserToAddRows ? 1 : 0);
            if (rowCount > 0)
            {
                var confirm = MessageBox.Show(
                    "Tienes cambios sin guardar. ¿Estás seguro de que quieres volver? Se perderán las modificaciones.",
                    "Descartar Cambios",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);
                if (confirm == DialogResult.No) return;
            }
            MostrarPanelEdicion(false);
        }

        // ====================================================================
        // MÉTODOS DE ESTILO Y CONFIGURACIÓN (SIN DUPLICADOS)
        // ====================================================================

        private void ApplyModernStyles() { this.BackColor = backgroundColor; this.Font = new Font("Segoe UI", 9); }
        private void ConfigurarGrid()
        { /* ... Tu código de ConfigurarGrid ... */
            dgvRutinas.BackgroundColor = Color.White;
            dgvRutinas.BorderStyle = BorderStyle.None;
            dgvRutinas.EnableHeadersVisualStyles = false;
            dgvRutinas.AllowUserToAddRows = true;
            dgvRutinas.AllowUserToDeleteRows = true;
            dgvRutinas.ReadOnly = false;
            dgvRutinas.AllowUserToResizeColumns = false;
            dgvRutinas.AllowUserToResizeRows = false;
            dgvRutinas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvRutinas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRutinas.RowHeadersVisible = false;
            dgvRutinas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRutinas.RowTemplate.Height = 35;
            dgvRutinas.ColumnHeadersDefaultCellStyle.BackColor = primaryColor;
            dgvRutinas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRutinas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvRutinas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRutinas.ColumnHeadersHeight = 40;
            dgvRutinas.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvRutinas.DefaultCellStyle.BackColor = Color.White;
            dgvRutinas.DefaultCellStyle.ForeColor = textColor;
            dgvRutinas.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRutinas.DefaultCellStyle.Padding = new Padding(5);
            dgvRutinas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgvRutinas.Columns["Ejercicio"].ReadOnly = false;
            dgvRutinas.Columns["Series"].ReadOnly = false;
            dgvRutinas.Columns["Repeticiones"].ReadOnly = false;
            dgvRutinas.Columns["Descanso"].ReadOnly = false;
        }
        private void ConfigurarMenuContextual()
        { /* ... Tu código de ConfigurarMenuContextual ... */
            menuEjercicios = new ContextMenuStrip { BackColor = Color.White, Font = new Font("Segoe UI", 9), ShowImageMargin = false };
            ToolStripMenuItem itemPersonalizado = new ToolStripMenuItem("🎯 EJERCICIO PERSONALIZADO")
            {
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = successColor
            };
            itemPersonalizado.Click += (s, e) => AgregarEjercicioPersonalizado();
            menuEjercicios.Items.Add(itemPersonalizado);
        }
        private void AgregarEjercicioPersonalizado()
        { /* ... Tu código de AgregarEjercicioPersonalizado ... */
            dgvRutinas.Rows.Add("", "3", "10", "60");
            if (dgvRutinas.Rows.Count > 0)
            {
                int lastRow = dgvRutinas.Rows.Count - (dgvRutinas.AllowUserToAddRows ? 2 : 1);
                if (lastRow >= 0)
                {
                    dgvRutinas.CurrentCell = dgvRutinas.Rows[lastRow].Cells["Ejercicio"];
                    dgvRutinas.BeginEdit(true);
                }
            }
        }

        // ESTA ES LA VERSIÓN CORRECTA Y ÚNICA DE LOS MÉTODOS DE ESTILO
        private void StyleButton(Button btn, Color bgColor, Color foreColor)
        {
            StyleButton(btn, bgColor);
            btn.ForeColor = foreColor;
            btn.FlatAppearance.BorderColor = lightGrayColor;
            btn.FlatAppearance.BorderSize = 1;
        }

        private void StyleButton(Button btn, Color bgColor)
        {
            if (btn == null) return;
            btn.BackColor = bgColor;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(12, 6, 12, 6);
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(bgColor, 0.1f);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(bgColor, 0.2f);

            btn.EnabledChanged += (s, e) =>
            {
                if (btn.Enabled)
                {
                    btn.BackColor = bgColor;
                    btn.ForeColor = (bgColor == lightGrayColor) ? textColor : Color.White;
                }
                else
                {
                    btn.BackColor = Color.FromArgb(230, 230, 230);
                    btn.ForeColor = Color.FromArgb(150, 150, 150);
                }
            };
            if (!btn.Enabled)
            {
                btn.BackColor = Color.FromArgb(230, 230, 230);
                btn.ForeColor = Color.FromArgb(150, 150, 150);
            }
        }

        private void btnAgregarEjercicio_Click(object sender, EventArgs e) { menuEjercicios?.Show(btnAgregarEjercicio, new Point(0, btnAgregarEjercicio.Height)); }
        private void btnEliminarEjercicio_Click(object sender, EventArgs e)
        { /* ... Tu código de btnEliminar ... */
            if (dgvRutinas.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvRutinas.SelectedRows)
                {
                    if (!row.IsNewRow) dgvRutinas.Rows.Remove(row);
                }
            }
        }
        private void btnLimpiarTodo_Click(object sender, EventArgs e)
        { /* ... Tu código de btnLimpiar ... */
            if (MessageBox.Show("¿Seguro que quieres borrar todos los ejercicios de esta rutina?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                dgvRutinas.Rows.Clear();
        }

        // --- LOS MÉTODOS DUPLICADOS FUERON ELIMINADOS DE AQUÍ ---
    }
}