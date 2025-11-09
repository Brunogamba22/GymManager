using GymManager.Controllers;
using GymManager.Forms;
using GymManager.Models;
using GymManager.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TextBox;

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
        private Color hoverColor = Color.FromArgb(232, 243, 252);

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

        // --- NUEVO: Controlador y Listas para el menú ---
        private readonly GrupoMuscularController _grupoMuscularController = new GrupoMuscularController();
        private List<GrupoMuscular> _listaGruposBD = new List<GrupoMuscular>();
        private List<Ejercicio> _listaEjerciciosBD = new List<Ejercicio>();

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

            // --- NUEVO: Cargar datos y poblar menú ---
            try
            {
                // 1. Cargar datos de la BD
                _listaGruposBD = _grupoMuscularController.ObtenerTodos();
                _listaEjerciciosBD = _ejercicioController.ObtenerTodos(); // Usa el método del Paso 1

                // 2. Construir el menú con los datos cargados
                PoblarMenuEjercicios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar listas de ejercicios para el menú: " + ex.Message);
            }
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
                    detalle.Carga?.ToString() ?? ""
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
                    if (idEjercicioReal == 0) { /*...*/ return; }

                    // --- 🔥 CORRECCIÓN: Leer int, int, string de la grilla ---

                    // Validar 'Series' (que debe ser int)
                    if (!int.TryParse(row.Cells["Series"].Value?.ToString(), out int totalSeries) || totalSeries <= 0)
                    {
                        MessageBox.Show($"El valor de 'Series' para '{nombreEjercicio}' debe ser un número positivo.", "Dato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Validar 'Repeticiones' (que debe ser int)
                    if (!int.TryParse(row.Cells["Repeticiones"].Value?.ToString(), out int totalRepes) || totalRepes <= 0)
                    {
                        MessageBox.Show($"El valor de 'Repeticiones' para '{nombreEjercicio}' debe ser un número positivo.", "Dato Inválido", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Leer 'Carga' (que es un string y puede estar vacío)
                    string cargaString = row.Cells["Carga"].Value?.ToString() ?? "";

                    

                    rutinaModificada.Add(new DetalleRutina
                    {
                        IdEjercicio = idEjercicioReal,
                        EjercicioNombre = nombreEjercicio,
                        Series = totalSeries,
                        Repeticiones = totalRepes,
                        Carga = cargaString
                    });
                    // --- FIN CORRECCIÓN ---
                }
                if (rutinaModificada.Count == 0) { /* ... */ return; }

                // Mapeo entre las pestañas del sistema y los nombres reales en la BD
                string nombreGenero = _tipoRutinaActual switch
                {
                    "Hombres" => "Masculino",
                    "Mujeres" => "Femenino",
                    "Deportistas" => "Deportistas",
                    _ => "Masculino"
                };

                // Buscar el ID real en la lista cargada
                var generoEncontrado = _listaDeGeneros
                    .FirstOrDefault(g => g.Nombre.Equals(nombreGenero, StringComparison.OrdinalIgnoreCase));

                if (generoEncontrado == null)
                    throw new Exception($"No se encontró el género '{nombreGenero}' en la base de datos.");

                int idGeneroParaGuardar = generoEncontrado.Id;

                // Crear el encabezado con el género correcto
                string nombreRutina = $"Rutina Editada {nombreGenero} - {DateTime.Now:dd/MM/yyyy}";
                int nuevoIdRutina = _rutinaController.CrearEncabezadoRutina(nombreGenero, Sesion.Actual.IdUsuario, nombreRutina, idGeneroParaGuardar, true);

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
            _frmMain?.MostrarEditarRutina();
        }

        // ====================================================================
        // MÉTODOS DE ESTILO Y CONFIGURACIÓN (SIN DUPLICADOS)
        // ====================================================================

        private void ApplyModernStyles() { this.BackColor = backgroundColor; this.Font = new Font("Segoe UI", 9); }
        private void ConfigurarGrid()
        {
            dgvRutinas.BackgroundColor = Color.White;
            dgvRutinas.BorderStyle = BorderStyle.None;
            dgvRutinas.EnableHeadersVisualStyles = false;
            dgvRutinas.AllowUserToAddRows = false;
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
            dgvRutinas.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRutinas.DefaultCellStyle.Padding = new Padding(5);

            // 🔥 COLORES ACTUALIZADOS
            dgvRutinas.DefaultCellStyle.BackColor = Color.White;
            dgvRutinas.DefaultCellStyle.ForeColor = textColor;

            // 💡 Color de selección visible y suave
            dgvRutinas.DefaultCellStyle.SelectionBackColor = Color.FromArgb(208, 236, 255);  // Azul muy claro
            dgvRutinas.DefaultCellStyle.SelectionForeColor = Color.Black;

            // 🔹 Fila alternada más visible
            dgvRutinas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(245, 247, 250);

            // Hover (mantiene tu estilo actual)
            dgvRutinas.CellMouseEnter += dgvRutinas_CellMouseEnter;
            dgvRutinas.CellMouseLeave += dgvRutinas_CellMouseLeave;

            dgvRutinas.Columns["Ejercicio"].ReadOnly = true;
            dgvRutinas.Columns["Series"].ReadOnly = false;
            dgvRutinas.Columns["Repeticiones"].ReadOnly = false;
            dgvRutinas.Columns["Carga"].ReadOnly = false;
        }

        private void ConfigurarMenuContextual()
        { /* ... Tu código de ConfigurarMenuContextual ... */
            menuEjercicios = new ContextMenuStrip { BackColor = Color.White, Font = new Font("Segoe UI", 9), ShowImageMargin = false, AutoSize = true };
        }
            
        private void AgregarEjercicioPersonalizado()
        { /* ... Tu código de AgregarEjercicioPersonalizado ... */
            dgvRutinas.Rows.Add("", "3", "10", "");
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

        // NUEVO: Método para construir el menú dinámicamente
        private void PoblarMenuEjercicios()
        {
            menuEjercicios.Items.Clear(); // Limpiamos por si acaso

            // 1. Opción "Ejercicio Personalizado"
            ToolStripMenuItem itemPersonalizado = new ToolStripMenuItem("🎯 EJERCICIO PERSONALIZADO")
            {
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = successColor
            };
            itemPersonalizado.Click += (s, e) => AgregarEjercicioPersonalizado();
            menuEjercicios.Items.Add(itemPersonalizado);
            menuEjercicios.Items.Add(new ToolStripSeparator());

            // 2. Loop por Grupos Musculares
            foreach (var grupo in _listaGruposBD.OrderBy(g => g.Nombre))
            {
                ToolStripMenuItem itemGrupo = new ToolStripMenuItem(grupo.Nombre.ToUpper());
                itemGrupo.ForeColor = primaryColor;
                itemGrupo.Font = new Font("Segoe UI", 9, FontStyle.Bold);

                // 3. Filtrar ejercicios para este grupo (usando LINQ)
                var ejerciciosDelGrupo = _listaEjerciciosBD
                                          .Where(ej => ej.GrupoMuscularId == grupo.Id)
                                          .OrderBy(ej => ej.Nombre);

                if (ejerciciosDelGrupo.Any())
                {
                    // 4. Loop por Ejercicios y agregarlos al sub-menú del grupo
                    foreach (var ejercicio in ejerciciosDelGrupo)
                    {
                        ToolStripMenuItem itemEjercicio = new ToolStripMenuItem(ejercicio.Nombre);

                        // Guardamos el objeto Ejercicio completo en el Tag
                        itemEjercicio.Tag = ejercicio;

                        // Asignamos el handler
                        itemEjercicio.Click += ItemEjercicio_Click;

                        itemGrupo.DropDownItems.Add(itemEjercicio);
                    }
                }
                else
                {
                    itemGrupo.Enabled = false; // Deshabilitar grupo si no tiene ejercicios
                }

                menuEjercicios.Items.Add(itemGrupo);
            }
        }

        // NUEVO: Handler para cuando se hace clic en un ejercicio
        private void ItemEjercicio_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem itemClickeado = sender as ToolStripMenuItem;
            if (itemClickeado?.Tag is Ejercicio ejercicioSeleccionado)
            {
                // Buscar grupo muscular del ejercicio
                var grupo = _listaGruposBD.FirstOrDefault(g => g.Id == ejercicioSeleccionado.GrupoMuscularId);
                string nombreGrupo = grupo?.Nombre.ToLower() ?? "";

                // Determinar si usa carga
                bool usaCarga = !(nombreGrupo.Contains("abdomen") ||
                                  nombreGrupo.Contains("core") ||
                                  nombreGrupo.Contains("cardio"));

                // Asignar carga por defecto
                string cargaString = usaCarga ? "75%" : "";

                // Añadir a la grilla con valores por defecto
                dgvRutinas.Rows.Add(
                    ejercicioSeleccionado.Nombre,
                    "3",        // Series por defecto
                    "10",       // Repeticiones por defecto
                    cargaString // ✅ Ahora incluye la carga
                );

                // Seleccionar automáticamente la nueva fila
                if (dgvRutinas.Rows.Count > 0)
                {
                    int lastRow = dgvRutinas.Rows.Count - 1;
                    dgvRutinas.ClearSelection();
                    dgvRutinas.Rows[lastRow].Selected = true;
                    dgvRutinas.FirstDisplayedScrollingRowIndex = lastRow;
                }
            }
        }


        // --- 🔥 AÑADE ESTOS DOS MÉTODOS COMPLETOS a UcEditarRutina.cs ---

        private void dgvRutinas_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            // Asegurarse de que no sea la fila de encabezado (índice -1)
            if (e.RowIndex >= 0)
            {
                var row = dgvRutinas.Rows[e.RowIndex];
                // No sobrescribas el color si ya está seleccionada
                if (!row.Selected)
                    row.DefaultCellStyle.BackColor = hoverColor;
            }
        }

        private void dgvRutinas_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            // Asegurarse de que no sea la fila de encabezado
            if (e.RowIndex >= 0)
            {
                var row = dgvRutinas.Rows[e.RowIndex];
                if (!row.Selected)
                {
                    // Restaurar color base (par/impar)
                    row.DefaultCellStyle.BackColor = e.RowIndex % 2 == 0
                        ? dgvRutinas.DefaultCellStyle.BackColor
                        : dgvRutinas.AlternatingRowsDefaultCellStyle.BackColor;
                }
            }
        }
    }
}