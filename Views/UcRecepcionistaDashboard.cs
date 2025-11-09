using GymManager.Controllers;
using GymManager.Models;
using GymManager.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Globalization;


namespace GymManager.Views
{
    public partial class UcRecepcionistaDashboard : UserControl
    {
        private Color primaryColor = Color.FromArgb(41, 128, 185);
        private Color backgroundColor = Color.FromArgb(245, 247, 250);
        private Color successColor = Color.FromArgb(46, 204, 113);    // Imprimir
        private Color infoColor = Color.FromArgb(52, 152, 219);      // Exportar
        private Color darkColor = Color.FromArgb(52, 73, 94);        // Modo TV
        private Color warningColor = Color.FromArgb(241, 196, 15);   // Limpiar
        private Color textColor = Color.FromArgb(52, 73, 94);
        private Color hoverColor = Color.FromArgb(232, 244, 253);
        private Color gridLineColor = Color.FromArgb(224, 230, 237);
        private Color panelColor = Color.White;
        // Selección (más oscuro)
        private readonly Color selectedRowColor = Color.FromArgb(45, 92, 130); // azul oscuro
        private readonly Color selectedRowText = Color.White;
        private readonly Color oddRowColor = Color.FromArgb(250, 250, 250);
        private readonly Color evenRowColor = Color.White;

        private UcDetalleRutina ucDetalle = null;

        // Controladores
        private readonly RutinaController _rutinaController = new RutinaController();
        private readonly DetalleRutinaController _detalleController = new DetalleRutinaController();
        private readonly GeneroController _generoController = new GeneroController();
        private readonly UsuarioController _usuarioController = new UsuarioController();

        private List<Rutina> rutinasGuardadas = new List<Rutina>();

        private Panel pnlHeader;

        // NOTA: chkTodasLasFechas se define en el Designer.cs

        // 🔹 Notificación visual
        private Label lblNotificacion;
        public UcRecepcionistaDashboard()
        {
            InitializeComponent();

            //  Agregar encabezado superior
            AgregarEncabezadoSuperior();

            //  Ajustar layout general (corrección de hueco)
            ReajustarLayoutRecepcionista();

            //  Ajustar el margen superior del panel principal para no quedar oculto
            mainPanel.Dock = DockStyle.Fill;
            mainPanel.Padding = new Padding(0, pnlHeader.Height, 0, 0);

            // Crear la etiqueta flotante de notificación
            CrearEtiquetaNotificacion();
            ApplyModernStyles();
            ConfigurarGrid();
            ConfigurarFiltros();

            btnFiltrar.Click += BtnFiltrar_Click;
            btnImprimir.Click += BtnImprimir_Click;
            btnExportar.Click += BtnExportar_Click;
            btnModoTV.Click += BtnModoTV_Click;
            btnLimpiarFiltros.Click += BtnLimpiarFiltros_Click;
            chkTodasLasFechas.CheckedChanged += (s, e) => { dtpFecha.Enabled = !chkTodasLasFechas.Checked; };
            dgvPlanillas.CellDoubleClick += DgvPlanillas_CellDoubleClick;
            this.BackColor = backgroundColor;
        }



        // =========================================================
        // 🧱 UI MODERNA
        // =========================================================
        private void ApplyModernStyles()
        {
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI", 9);

            // Botones con estilo
            StyleButton(btnFiltrar, primaryColor);
            StyleButton(btnImprimir, successColor);
            StyleButton(btnExportar, infoColor);
            StyleButton(btnModoTV, darkColor);
            StyleButton(btnLimpiarFiltros, warningColor);

            // Hover animado (sin tocar el estilo base)
            foreach (var btn in new[] { btnFiltrar, btnImprimir, btnExportar, btnModoTV, btnLimpiarFiltros })
            {
                btn.MouseEnter += (s, e) => btn.BackColor = ControlPaint.Light(btn.BackColor, 0.2f);
                btn.MouseLeave += (s, e) => ApplyModernStyles(); // restaura color base
            }

            // Comboboxes y DateTimePicker
            EstilizarCombobox(cmbProfesor);
            EstilizarCombobox(cmbGenero);
            EstilizarDateTimePicker(dtpFecha);

            // Labels y CheckBox
            if (chkSoloEditadas != null) chkSoloEditadas.ForeColor = textColor;
            if (chkTodasLasFechas != null) chkTodasLasFechas.ForeColor = textColor;
            EstilizarLabels();
        }

        // 🔹 Agrega título superior fijo
        private void AgregarEncabezadoSuperior()
        {
            if (pnlHeader != null && !pnlHeader.IsDisposed) return;

            pnlHeader = new Panel
            {
                Name = "pnlHeader",
                Dock = DockStyle.Top,
                Height = 48,
                BackColor = primaryColor
            };

            var lblTitulo = new Label
            {
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.White,
                TextAlign = ContentAlignment.MiddleCenter,
                Text = "🏋️‍♂️  Panel del Recepcionista – Gestión de Rutinas"
            };

            pnlHeader.Controls.Add(lblTitulo);

            // 👉 Lo agregamos antes que mainPanel para que no lo pise
            this.Controls.Add(pnlHeader);
            this.Controls.SetChildIndex(pnlHeader, 0);
            this.Controls.SetChildIndex(mainPanel, 1); // mainPanel queda debajo y se ajusta solo
        }

        // 🔹 Notificación visual inferior (desaparece con Timer)
        private void CrearEtiquetaNotificacion()
        {
            if (lblNotificacion != null && !lblNotificacion.IsDisposed) return;

            lblNotificacion = new Label
            {
                AutoSize = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(52, 152, 219),
                Padding = new Padding(10, 5, 10, 5),
                Visible = false,
                Anchor = AnchorStyles.Bottom | AnchorStyles.Right
            };

            this.Controls.Add(lblNotificacion);
            lblNotificacion.BringToFront();

            // Reposicionar en cada resize
            this.Resize += (s, e) => ReposicionarNotificacion();
            ReposicionarNotificacion();
        }

        private void ReposicionarNotificacion()
        {
            if (lblNotificacion == null) return;
            lblNotificacion.Location = new Point(
                this.ClientSize.Width - lblNotificacion.Width - 16,
                this.ClientSize.Height - lblNotificacion.Height - 12
            );
        }

        private void MostrarNotificacion(string mensaje, Color color)
        {
            lblNotificacion.Text = mensaje;
            lblNotificacion.BackColor = color;
            lblNotificacion.Visible = true;
            ReposicionarNotificacion();

            var t = new Timer { Interval = 2000 };
            t.Tick += (s, e) => { lblNotificacion.Visible = false; t.Stop(); t.Dispose(); };
            t.Start();
        }

        private void EstilizarLabels()
        {
            Label[] labels = { lblFiltroFecha, lblFiltroProfesor, lblFiltroGenero };
            foreach (var label in labels)
            {
                if (label != null)
                {
                    label.ForeColor = textColor;
                    label.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                    label.BackColor = Color.Transparent;
                }
            }
        }

        private void EstilizarCombobox(ComboBox cmb)
        {
            if (cmb == null) return;

            cmb.FlatStyle = FlatStyle.Flat;
            cmb.BackColor = Color.White;
            cmb.ForeColor = textColor;
            cmb.Font = new Font("Segoe UI", 9);
        }

        private void EstilizarDateTimePicker(DateTimePicker dtp)
        {
            if (dtp == null) return;

            dtp.Format = DateTimePickerFormat.Short;
            dtp.ShowUpDown = false;
            dtp.CalendarFont = new Font("Segoe UI", 9);
        }

        private void ConfigurarGrid()
        {
            dgvPlanillas.BackgroundColor = Color.White;
            dgvPlanillas.BorderStyle = BorderStyle.None;
            dgvPlanillas.GridColor = gridLineColor;
            dgvPlanillas.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgvPlanillas.EnableHeadersVisualStyles = false;
            dgvPlanillas.ReadOnly = true;
            dgvPlanillas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPlanillas.RowHeadersVisible = false;
            dgvPlanillas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPlanillas.RowTemplate.Height = 50;

            // Encabezados
            dgvPlanillas.ColumnHeadersDefaultCellStyle.BackColor = primaryColor;
            dgvPlanillas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPlanillas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvPlanillas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPlanillas.ColumnHeadersHeight = 45;

            // Filas
            dgvPlanillas.DefaultCellStyle.Font = new Font("Segoe UI", 10);
            dgvPlanillas.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPlanillas.DefaultCellStyle.BackColor = Color.White;
            dgvPlanillas.DefaultCellStyle.ForeColor = textColor;

            dgvPlanillas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);

            // Selección oscura consistente
            dgvPlanillas.DefaultCellStyle.SelectionBackColor = selectedRowColor;
            dgvPlanillas.DefaultCellStyle.SelectionForeColor = selectedRowText;
            dgvPlanillas.AlternatingRowsDefaultCellStyle.SelectionBackColor = selectedRowColor;
            dgvPlanillas.AlternatingRowsDefaultCellStyle.SelectionForeColor = selectedRowText;


            // Animación hover
            dgvPlanillas.CellMouseEnter += (s, e) =>
            {
                if (e.RowIndex >= 0)
                    dgvPlanillas.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.FromArgb(232, 244, 253);
            };
            dgvPlanillas.CellMouseLeave += (s, e) =>
            {
                if (e.RowIndex >= 0)
                {
                    var color = e.RowIndex % 2 == 0 ? Color.White : Color.FromArgb(250, 250, 250);
                    dgvPlanillas.Rows[e.RowIndex].DefaultCellStyle.BackColor = color;
                }
            };
        }

        private void ConfigurarFiltros()
        {
            dtpFecha.Value = DateTime.Now.Date;
            chkTodasLasFechas.Checked = false; // Por defecto, filtrar por fecha actual

            try
            {
                var generos = _generoController.ObtenerTodos();
                generos.Insert(0, new Genero { Id = 0, Nombre = "Todos" });
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
        private void ReajustarLayoutRecepcionista()
        {
            this.SuspendLayout();

            // 🔹 Aseguramos que el encabezado azul esté bien anclado arriba
            if (pnlHeader != null)
            {
                pnlHeader.Dock = DockStyle.Top;
                pnlHeader.Height = 48;
                pnlHeader.BackColor = primaryColor;
            }

            // 🔹 Aseguramos que el panel principal ocupe todo el resto
            if (mainPanel != null)
            {
                mainPanel.Dock = DockStyle.Fill;
                mainPanel.Padding = new Padding(0);
                mainPanel.Margin = new Padding(0);
                mainPanel.BackColor = backgroundColor;
            }

            // 🔹 Ajustamos los colores base para evitar fondo gris del contenedor
            this.BackColor = backgroundColor;

            // 🔹 Nos aseguramos del orden correcto de los controles
            if (pnlHeader != null)
            {
                this.Controls.SetChildIndex(pnlHeader, 0);
                if (mainPanel != null)
                    this.Controls.SetChildIndex(mainPanel, 1);
            }

            this.ResumeLayout();
            this.Refresh();
        }


        // =========================================================
        // LÓGICA DE CARGA Y FILTRADO (MODIFICADO)
        // =========================================================

        public void CargarDatos()
        {
            CargarGrillaConFiltros();
            OcultarDetalle();
        }

        // =========================================================
        // 🧭 FILTROS Y CARGA DE DATOS (idéntico, solo feedback visual)
        // =========================================================
        private void BtnFiltrar_Click(object sender, EventArgs e)
        {
            CargarGrillaConFiltros();
            MostrarNotificacion("✅ Filtro aplicado", Color.FromArgb(52, 152, 219));
        }

        private void BtnLimpiarFiltros_Click(object sender, EventArgs e)
        {
            try
            {
                // 1. Restablecer visualmente los filtros
                cmbProfesor.SelectedIndex = 0;
                cmbGenero.SelectedIndex = 0;

                // 💡 Es crucial que el filtro de fecha quede en "Todas las fechas" o en la fecha actual.
                // Si quieres que la grilla quede vacía, debes dejar los filtros en un estado que NO devuelva rutinas,
                // o forzar CargarGrillaConFiltros a usar un filtro que sepas que no tiene resultados.
                // La convención normal es: Limpiar = Mostrar TODOS los datos disponibles o Ninguno.

                // Opcion A: Mostrar todos los disponibles (Recomendado)
                chkTodasLasFechas.Checked = true;
                if (chkSoloEditadas != null) chkSoloEditadas.Checked = false;
                dtpFecha.Value = DateTime.Now.Date; // El valor real es ignorado por el check

                // 2. Ejecutar la carga (esto llama a CargarGrillaConFiltros)
                CargarGrillaConFiltros();

                // 3. Quitar selección y feedback
                dgvPlanillas.ClearSelection();
                HabilitarBotonesDeAccion(false);
                //MostrarNotificacion("🧹 Filtros limpiados. Mostrando todas las rutinas.", Color.FromArgb(241, 196, 15));

                // Si prefieres que la grilla quede vacía después de limpiar, haz esto:
                 dgvPlanillas.Rows.Clear();
                 MostrarNotificacion("🧹 Grilla vacía.", Color.FromArgb(241, 196, 15));

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al limpiar filtros: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CargarGrillaConFiltros()
        {
            try
            {
                DateTime? fechaInicio = null;
                DateTime? fechaFin = null;

                // Aplicar filtro de fecha SOLO si el CheckBox 'Todas las fechas' NO está marcado
                if (!chkTodasLasFechas.Checked)
                {
                    fechaInicio = dtpFecha.Value.Date;
                    fechaFin = dtpFecha.Value.Date;
                }

                // Conversión de IDs (0 = Todos -> null)
                int? idGenero = (int)cmbGenero.SelectedValue;
                int? idProfesor = (int)cmbProfesor.SelectedValue;
                bool soloEditadas = (chkSoloEditadas != null) && chkSoloEditadas.Checked;

                if (idGenero == 0) idGenero = null;
                if (idProfesor == 0) idProfesor = null;

                // NOTA: Se asume que el método ObtenerTodasParaPlanilla en RutinaController
                // ahora acepta DateTime? (nullables)
                rutinasGuardadas = _rutinaController.ObtenerTodasParaPlanilla(
                    fechaInicio, fechaFin, idGenero, soloEditadas, idProfesor);

                if (rutinasGuardadas.Count == 0)
                {
                    MessageBox.Show("No se encontraron rutinas para los filtros seleccionados.",
                        "Sin Resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    dgvPlanillas.Rows.Clear();
                    HabilitarBotonesDeAccion(false);
                    return;
                }

                ActualizarGrid(rutinasGuardadas);
                HabilitarBotonesDeAccion(false);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al cargar planilla: {ex.Message}",
                    "Error de Base de Datos", MessageBoxButtons.OK, MessageBoxIcon.Error);
                HabilitarBotonesDeAccion(false);
            }
        }

        private void HabilitarBotonesDeAccion(bool habilitar)
        {
            btnImprimir.Enabled = habilitar;
            btnExportar.Enabled = habilitar;
        }

        private void ActualizarGrid(List<Rutina> rutinas)
        {
            dgvPlanillas.SelectionChanged -= DgvPlanillas_SelectionChanged;
            dgvPlanillas.Rows.Clear();

            foreach (var rutina in rutinas)
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
            {
                dgvPlanillas.ClearSelection();
            }
            AplicarEstilosFilas();
        }

        // =========================================================
        // LÓGICA DE NAVEGACIÓN Y ACCIONES
        // =========================================================

        private void DgvPlanillas_SelectionChanged(object sender, EventArgs e)
        {
            HabilitarBotonesDeAccion(dgvPlanillas.SelectedRows.Count > 0);
            AplicarEstilosFilas();
        }

        private void AplicarEstilosFilas()
        {
            foreach (DataGridViewRow row in dgvPlanillas.Rows)
            {
                if (row.Selected)
                {
                    row.DefaultCellStyle.BackColor = selectedRowColor;
                    row.DefaultCellStyle.ForeColor = selectedRowText;
                }
                else
                {
                    row.DefaultCellStyle.BackColor = (row.Index % 2 == 0) ? evenRowColor : oddRowColor;
                    row.DefaultCellStyle.ForeColor = textColor;
                }
            }
        }

        private void DgvPlanillas_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                if (e.RowIndex < rutinasGuardadas.Count)
                {
                    var rutinaSeleccionada = rutinasGuardadas[e.RowIndex];
                    MostrarDetalleRutina(rutinaSeleccionada);
                }
            }
        }

        private void MostrarDetalleRutina(Rutina rutinaHeader)
        {
            try
            {
                mainPanel.Visible = false;

                if (ucDetalle == null)
                {
                    ucDetalle = new UcDetalleRutina();
                    ucDetalle.Dock = DockStyle.Fill;
                    ucDetalle.OnCerrarDetalle += (s, ev) => OcultarDetalle();
                    this.Controls.Add(ucDetalle);
                }

                // 🔹 Obtener los detalles de la rutina seleccionada
                List<DetalleRutina> detallesDeRutina = _detalleController.ObtenerPorRutina(rutinaHeader.IdRutina);

                if (detallesDeRutina == null || detallesDeRutina.Count == 0)
                {
                    MessageBox.Show("Esta rutina no tiene ejercicios cargados para mostrar.",
                                    "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    OcultarDetalle();
                    return;
                }

                // 🔹 Cargar los datos completos en el control de detalle (ahora incluye el ID)
                ucDetalle.CargarRutina(
                    rutinaHeader.IdRutina,              // ✅ ID real de la rutina
                    rutinaHeader.Nombre,                // Nombre de la rutina
                    rutinaHeader.NombreGenero,          // Tipo o género
                    rutinaHeader.NombreProfesor,        // Profesor
                    rutinaHeader.FechaCreacion,         // Fecha
                    detallesDeRutina                    // Lista de ejercicios
                );

                ucDetalle.Visible = true;
                ucDetalle.BringToFront();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"No se pudieron cargar los detalles: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                OcultarDetalle();
            }
        }

        private void OcultarDetalle()
        {
            if (ucDetalle != null)
            {
                ucDetalle.Visible = false;
                ucDetalle.SendToBack();
            }

            if (mainPanel != null)
            {
                // 🔹 Forzamos a repintar correctamente el fondo
                mainPanel.Visible = true;
                mainPanel.Dock = DockStyle.Fill;
                mainPanel.Padding = new Padding(0);
                mainPanel.Margin = new Padding(0);
                mainPanel.BackColor = backgroundColor; // vuelve a aplicar el color del panel principal
                mainPanel.BringToFront();
                mainPanel.Refresh();
            }

            // 🔹 También repintamos el fondo del UserControl
            this.BackColor = backgroundColor;
            this.Refresh();
        }


        // =========================================================
        // BOTONES DE ACCIÓN
        // =========================================================

        private void BtnImprimir_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPlanillas.SelectedRows.Count == 0)
                {
                    MessageBox.Show("⚠️ Debe seleccionar una rutina antes de imprimir.",
                                    "Impresión no disponible",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int index = dgvPlanillas.SelectedRows[0].Index;
                if (index < 0 || index >= rutinasGuardadas.Count)
                {
                    MessageBox.Show("No se pudo identificar la rutina seleccionada.",
                                    "Error de selección", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var rutina = rutinasGuardadas[index];
                ImprimirRutina(rutina);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al intentar imprimir: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnExportar_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvPlanillas.SelectedRows.Count == 0)
                {
                    MessageBox.Show("⚠️ Debe seleccionar una rutina antes de exportar.",
                                    "Exportación no disponible",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int index = dgvPlanillas.SelectedRows[0].Index;
                if (index < 0 || index >= rutinasGuardadas.Count)
                {
                    MessageBox.Show("No se pudo identificar la rutina seleccionada.",
                                    "Error de selección", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var rutina = rutinasGuardadas[index];
                ExportarRutina(rutina);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al intentar exportar: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // 🔹 ExportarRutina (versión PDF completamente funcional)
        // ============================================================
        public void ExportarRutina(Rutina rutina)
        {
            try
            {
                var detalles = _detalleController.ObtenerPorRutina(rutina.IdRutina);
                if (detalles == null || detalles.Count == 0)
                {
                    MessageBox.Show("Esta rutina no tiene ejercicios cargados para exportar.",
                                    "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string ruta = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Desktop),
                    $"Rutina_{SanearNombre(rutina.Nombre)}_{DateTime.Now:yyyyMMdd_HHmm}.pdf"
                );

                // 👉 Usamos nombres completos para evitar ambigüedad
                iTextSharp.text.Document doc = new iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 50, 50, 80, 50);
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(doc, new FileStream(ruta, FileMode.Create));
                doc.Open();

                // Estilos PDF
                var azulGym = new iTextSharp.text.BaseColor(41, 128, 185);
                var fTitulo = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 16, iTextSharp.text.Font.BOLD, azulGym);
                var fSub = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10, iTextSharp.text.Font.BOLD);
                var fNormal = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 10);
                var fPie = new iTextSharp.text.Font(iTextSharp.text.Font.FontFamily.HELVETICA, 8, iTextSharp.text.Font.ITALIC, iTextSharp.text.BaseColor.GRAY);

                // Cabecera
                var titulo = new iTextSharp.text.Paragraph("G Y M   M A N A G E R\nRUTINA DE ENTRENAMIENTO", fTitulo)
                { Alignment = iTextSharp.text.Element.ALIGN_CENTER };
                doc.Add(titulo);
                doc.Add(new iTextSharp.text.Paragraph("\n"));

                doc.Add(new iTextSharp.text.Paragraph($"Rutina: {rutina.Nombre}", fSub));
                doc.Add(new iTextSharp.text.Paragraph($"Profesor: {rutina.NombreProfesor}", fNormal));
                doc.Add(new iTextSharp.text.Paragraph($"Género: {rutina.NombreGenero}", fNormal));
                doc.Add(new iTextSharp.text.Paragraph($"Fecha de creación: {rutina.FechaCreacion:dd/MM/yyyy}", fNormal));
                doc.Add(new iTextSharp.text.Paragraph("\n"));

                // Tabla de ejercicios
                var tabla = new iTextSharp.text.pdf.PdfPTable(4)
                {
                    WidthPercentage = 100
                };
                tabla.SetWidths(new float[] { 3f, 1f, 1f, 1f });

                string[] headers = { "EJERCICIO", "SERIES", "REPS", "CARGA %" };
                foreach (var h in headers)
                {
                    var cell = new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(h, fSub))
                    {
                        BackgroundColor = new iTextSharp.text.BaseColor(235, 242, 250),
                        HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER,
                        Padding = 6
                    };
                    tabla.AddCell(cell);
                }

                foreach (var d in detalles)
                {
                    tabla.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(d.EjercicioNombre, fNormal)) { Padding = 5 });
                    tabla.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(d.Series.ToString(), fNormal)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
                    tabla.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(d.Repeticiones.ToString(), fNormal)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
                    string cargaTextoPdf = string.IsNullOrWhiteSpace(d.Carga) ? "-" : d.Carga;
                    tabla.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(cargaTextoPdf, fNormal)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
                }

                doc.Add(tabla);

                // Pie
                doc.Add(new iTextSharp.text.Paragraph("\nGenerado el " + DateTime.Now.ToString("dd/MM/yyyy HH:mm"), fPie));

                doc.Close();

                MessageBox.Show($"📄 Rutina exportada correctamente en:\n{ruta}",
                                "Exportación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);

                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo(ruta) { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al exportar rutina: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        //---------------------------------------------------------------------
        // Reemplaza caracteres inválidos para nombres de archivo por '_'
        private string SanearNombre(string nombre)
        {
            foreach (char c in Path.GetInvalidFileNameChars())
                nombre = nombre.Replace(c, '_');
            return nombre;
        }

        // 🔢 Contador de páginas (a nivel de clase)
        private int _pageNumber = 0;

        public void ImprimirRutina(Rutina rutina)
        {
            try
            {
                var detalles = _detalleController.ObtenerPorRutina(rutina.IdRutina);
                if (detalles == null || detalles.Count == 0)
                {
                    MessageBox.Show("Esta rutina no tiene ejercicios cargados.",
                                    "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                PrintDocument printDoc = new PrintDocument();

                // 📄 Márgenes / orientación
                printDoc.DefaultPageSettings.Margins = new Margins(60, 60, 80, 60);
                printDoc.DefaultPageSettings.Landscape = false;

                // ✅ Reiniciar contador al iniciar impresión/preview
                printDoc.BeginPrint += (s, e) => { _pageNumber = 0; };

                // 🎨 Estilos / fuentes
                Color colorPrimario = Color.FromArgb(41, 128, 185);
                Color colorTexto = Color.FromArgb(33, 33, 33);
                Color colorLinea = Color.FromArgb(200, 200, 200);

                Font fTitulo = new Font("Segoe UI", 16, FontStyle.Bold);
                Font fSubtitulo = new Font("Segoe UI", 10, FontStyle.Bold);
                Font fTexto = new Font("Segoe UI", 10);
                Font fPie = new Font("Segoe UI", 8, FontStyle.Italic);

                int salto = 26;

                printDoc.PrintPage += (s, e) =>
                {
                    // 🔢 Avanzamos el número de página real
                    _pageNumber++;

                    Graphics g = e.Graphics;
                    int ancho = e.PageBounds.Width;
                    int y;

                    // ===== ENCABEZADO =====
                    g.FillRectangle(new SolidBrush(colorPrimario), 0, 0, ancho, 70);
                    g.DrawString("G Y M   M A N A G E R", new Font("Segoe UI", 12, FontStyle.Bold), Brushes.White,
                        new RectangleF(0, 10, ancho, 30), new StringFormat { Alignment = StringAlignment.Center });
                    g.DrawString("RUTINA DE ENTRENAMIENTO", fTitulo, Brushes.White,
                        new RectangleF(0, 35, ancho, 40), new StringFormat { Alignment = StringAlignment.Center });

                    // ===== DATOS PRINCIPALES =====
                    y = 100;
                    g.DrawString($"Rutina: {rutina.Nombre}", fSubtitulo, Brushes.Black, 80, y); y += salto;
                    g.DrawString($"Profesor: {rutina.NombreProfesor}", fTexto, Brushes.Black, 80, y); y += salto;
                    g.DrawString($"Género: {rutina.NombreGenero}", fTexto, Brushes.Black, 80, y); y += salto;
                    g.DrawString($"Fecha de creación: {rutina.FechaCreacion:dd/MM/yyyy}", fTexto, Brushes.Black, 80, y);
                    y += salto * 2;

                    // ===== TABLA =====
                    int xInicio = 80;
                    int anchoTabla = ancho - 160;
                    int filaAltura = 28;

                    g.FillRectangle(new SolidBrush(Color.FromArgb(235, 242, 250)), xInicio, y, anchoTabla, filaAltura);
                    g.DrawRectangle(Pens.Gray, xInicio, y, anchoTabla, filaAltura);

                    g.DrawString("EJERCICIO", fSubtitulo, Brushes.Black, xInicio + 10, y + 6);
                    g.DrawString("SERIES", fSubtitulo, Brushes.Black, xInicio + 340, y + 6);
                    g.DrawString("REPS", fSubtitulo, Brushes.Black, xInicio + 440, y + 6);
                    g.DrawString("CARGA %", fSubtitulo, Brushes.Black, xInicio + 530, y + 6);
                    y += filaAltura;

                    // Filas (paginación básica)
                    foreach (var d in detalles)
                    {
                        g.DrawString(d.EjercicioNombre, fTexto, new SolidBrush(colorTexto), xInicio + 10, y + 6);
                        g.DrawString(d.Series.ToString(), fTexto, Brushes.Black, xInicio + 345, y + 6);
                        g.DrawString(d.Repeticiones.ToString(), fTexto, Brushes.Black, xInicio + 445, y + 6);
                        //'Carga' es un string. Comprobamos si está vacío.
                        string cargaTexto = string.IsNullOrWhiteSpace(d.Carga) ? "-" : d.Carga;
                        g.DrawString(cargaTexto, fTexto, Brushes.Black, xInicio + 540, y + 6);

                        g.DrawLine(new Pen(colorLinea), xInicio, y + filaAltura, xInicio + anchoTabla, y + filaAltura);
                        y += filaAltura;

                        if (y > e.MarginBounds.Bottom - 60)
                        {
                            // Si tuvieras muchas filas y quisieras continuar,
                            // podrías cortar aquí y setear HasMorePages = true, 
                            // manejando un índice externo de fila. Para este caso
                            // simple, imprimimos en una sola página.
                            break;
                        }
                    }

                    // ===== PIE =====
                    y += 25;
                    g.DrawLine(Pens.Gray, 60, y, ancho - 60, y);

                    string textoPie = $"Generado el {DateTime.Now:dd/MM/yyyy HH:mm}";
                    g.DrawString(textoPie, fPie, Brushes.Gray, 80, y + 10);

                    //  Número de página correcto
                    g.DrawString($"Página {_pageNumber}", fPie, Brushes.Gray, ancho - 180, y + 10);

                    e.HasMorePages = false;
                };

                //  Vista previa
                PrintPreviewDialog preview = new PrintPreviewDialog
                {
                    Document = printDoc,
                    WindowState = FormWindowState.Maximized,
                    ShowIcon = false,
                    Text = "Vista previa - Rutina de Entrenamiento"
                };
                preview.PrintPreviewControl.Zoom = 1.0;
                preview.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al imprimir rutina: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnModoTV_Click(object sender, EventArgs e)
        {
            try
            {
                // 🔹 Verificar que haya una rutina seleccionada
                if (dgvPlanillas.SelectedRows.Count == 0)
                {
                    MessageBox.Show("⚠️ Debe seleccionar una rutina antes de abrir el Modo TV.",
                                    "Modo TV no disponible",
                                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                int index = dgvPlanillas.SelectedRows[0].Index;

                if (index < 0 || index >= rutinasGuardadas.Count)
                {
                    MessageBox.Show("No se pudo identificar la rutina seleccionada.",
                                    "Error de selección", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 🔹 Obtener la rutina seleccionada
                var rutinaSeleccionada = rutinasGuardadas[index];
                var detalles = _detalleController.ObtenerPorRutina(rutinaSeleccionada.IdRutina);

                if (detalles == null || detalles.Count == 0)
                {
                    MessageBox.Show("Esta rutina no tiene ejercicios cargados para mostrar.",
                                    "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }



                // 🔹 Crear y mostrar una nueva ventana FormTV (independiente)
                FormTV pantallaTV = new FormTV(
                    rutinaSeleccionada.NombreProfesor,
                    rutinaSeleccionada.Nombre,
                    rutinaSeleccionada.NombreGenero, // 👈 se envía el tipo de rutina
                    detalles
                );

                pantallaTV.Text = $"Modo TV - {rutinaSeleccionada.NombreProfesor}";
                pantallaTV.Show(); // 👉 así podés abrir varias a la vez
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al iniciar Modo TV: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // =========================================================
        // MÉTODOS DE ESTILO MEJORADOS (Versión actualizada)
        // =========================================================
        private void StyleButton(Button btn, Color bgColor)
        {
            if (btn == null) return;

            bool esColorClaro = (0.299 * bgColor.R + 0.587 * bgColor.G + 0.114 * bgColor.B) / 255 > 0.5;
            Color colorTexto = esColorClaro ? textColor : Color.White;

            StyleButton(btn, bgColor, colorTexto);
        }

        private void StyleButton(Button btn, Color bgColor, Color foreColor)
        {
            if (btn == null) return;

            // --- Tamaño adaptable ---
            btn.AutoSize = true;
            btn.AutoSizeMode = AutoSizeMode.GrowAndShrink;
            btn.MinimumSize = new Size(110, 35);
            btn.UseCompatibleTextRendering = false;
            btn.Padding = new Padding(16, 8, 16, 8);

            // --- Colores base ---
            btn.BackColor = bgColor;
            btn.ForeColor = foreColor;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9f, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;

            // --- Efectos hover ---
            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(bgColor, 0.15f);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(bgColor, 0.25f);

            // --- Bordes redondeados seguros (1px dentro del contenedor) ---
            btn.Paint += (s, e) =>
            {
                var rect = new Rectangle(1, 1, btn.Width - 2, btn.Height - 2);
                using (var path = new GraphicsPath())
                {
                    int radius = 6;
                    int d = radius * 2;
                    path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                    path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                    path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                    path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                    path.CloseFigure();
                    btn.Region = new Region(path);
                }
            };

            // --- Cambio de estilo al deshabilitar ---
            btn.EnabledChanged += (s, e) =>
            {
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

        // ============================================================
        // EVENTOS VISUALES DEL DATAGRIDVIEW
        // ============================================================
        private void dgvPlanillas_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvPlanillas.Rows[e.RowIndex];
                if (!row.Selected) row.DefaultCellStyle.BackColor = hoverColor;
            }
        }

        private void dgvPlanillas_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                var row = dgvPlanillas.Rows[e.RowIndex];
                if (!row.Selected)
                {
                    row.DefaultCellStyle.BackColor = (e.RowIndex % 2 == 0) ? evenRowColor : oddRowColor;
                }
            }
        }
    }

    // ============================================================================
    // 🖥️ FORMULARIO MODO TV (versión mejorada y comentada)
    // ----------------------------------------------------------------------------
    // Muestra la rutina de un profesor en pantalla completa, con opción de 
    // minimizar, abrir múltiples rutinas en paralelo, y cerrar con la tecla ESC.
    // ============================================================================

    public class FormTV : Form
    {
        // 🎨 Colores y fuentes base
        private Color tvBackColor = Color.FromArgb(33, 37, 41);
        private Color tvGridColor = Color.FromArgb(52, 58, 64);
        private Color tvHeaderColor = Color.FromArgb(41, 128, 185);
        private Color tvTitleColor = Color.FromArgb(241, 196, 15);

        // 🔹 Fuentes "base" (se escalan dinámicamente)
        private float baseTitleSize = 28f;   // título
        private float baseSubSize = 14f;   // subtítulo (género)
        private float baseHeaderSize = 12f;  // encabezado de columnas
        private float baseGridSize = 11f;    // filas
        private int baseRowHeight = 60;      // alto de fila
        private int baseHeaderHeight = 56;   // alto encabezado

        private string generoRutina;         // ⬅️ nuevo

        private Label lblTituloTV;
        private Label lblGenero;             // ⬅️ nuevo
        private DataGridView dgvRutina;
        private TableLayoutPanel tlpMain;

        // ✅ NUEVO: ctor principal (4 parámetros)
        public FormTV(string nombreProfesor, string nombreRutina, string genero, List<DetalleRutina> detalles)
        {
            this.generoRutina = genero ?? string.Empty;
            InitializeForm();

            this.Text = $"Modo TV - {nombreProfesor}";
            lblTituloTV.Text = $"RUTINA DEL PROFESOR: {nombreProfesor.ToUpper()}";

            var etiqueta = EtiquetaGenero(this.generoRutina);
            if (etiqueta != null)
            {
                lblGenero.Text = etiqueta;
                lblGenero.Visible = true;
            }
            else
            {
                lblGenero.Visible = false;
            }

            CargarRutina(detalles, nombreRutina);

            ApplyResponsiveScale();
            this.Resize += (s, e) => ApplyResponsiveScale();
        }

        // ✅ COMPATIBILIDAD: ctor antiguo (3 parámetros)
        public FormTV(string nombreProfesor, string nombreRutina, List<DetalleRutina> detalles)
            : this(nombreProfesor, nombreRutina, null, detalles) { }

        private void InitializeForm()
        {
            this.BackColor = tvBackColor;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.MinimizeBox = true;
            this.MaximizeBox = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) this.Close(); };

            tlpMain = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(30),
                BackColor = tvBackColor,
                RowCount = 3,                     // ⬅️ antes 2
                ColumnCount = 1
            };
            tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));      // título
            tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));      // subtítulo (género)
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100));  // grilla
            this.Controls.Add(tlpMain);

            lblTituloTV = new Label
            {
                Dock = DockStyle.Top,
                AutoSize = false,
                ForeColor = tvTitleColor,
                Text = "RUTINA DEL PROFESOR",
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 110,
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            tlpMain.Controls.Add(lblTituloTV, 0, 0);

            // ⬅️ NUEVO: subtítulo para el género
            lblGenero = new Label
            {
                Dock = DockStyle.Top,
                AutoSize = false,
                ForeColor = Color.Gainsboro,
                Text = "",
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 36,
                Visible = false, // se muestra sólo si hay género
                Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right
            };
            tlpMain.Controls.Add(lblGenero, 0, 1);

            dgvRutina = CrearGrillaTV();
            dgvRutina.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tlpMain.Controls.Add(dgvRutina, 0, 2);
        }

        private static string EtiquetaGenero(string genero)
        {
            if (string.IsNullOrWhiteSpace(genero)) return null;

            var g = genero.Trim().ToLower();

            // Variantes comunes
            if (g.Contains("masc") || g.Contains("hombre")) return "PARA: HOMBRES";
            if (g.Contains("fem") || g.Contains("mujer")) return "PARA: MUJERES";
            if (g.Contains("deport")) return "PARA: DEPORTISTAS";

            // Fallback: lo mostramos capitalizado
            try
            {
                var nice = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(g);
                return $"PARA: {nice}";
            }
            catch { return $"PARA: {genero.ToUpper()}"; }
        }


        private DataGridView CrearGrillaTV()
        {
            var dgv = new DataGridView
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
                ColumnHeadersHeight = baseHeaderHeight,
                RowTemplate = { Height = baseRowHeight },
                AllowUserToResizeRows = false
            };

            dgv.ColumnHeadersDefaultCellStyle.BackColor = tvHeaderColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.DefaultCellStyle.BackColor = tvBackColor;
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.SelectionBackColor = tvBackColor; // sin resaltado
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 44, 52);

            var colEj = new DataGridViewTextBoxColumn { Name = "EJERCICIO", HeaderText = "EJERCICIO", FillWeight = 52 };
            var colSe = new DataGridViewTextBoxColumn { Name = "SERIES", HeaderText = "SERIES", FillWeight = 16 };
            var colRe = new DataGridViewTextBoxColumn { Name = "REPS", HeaderText = "REPS", FillWeight = 16 };
            var colCa = new DataGridViewTextBoxColumn { Name = "CARGA", HeaderText = "CARGA %", FillWeight = 16 };
            dgv.Columns.AddRange(colEj, colSe, colRe, colCa);

            return dgv;
        }

        // 📐 Escalado responsivo
        private void ApplyResponsiveScale()
        {
            float factor = Math.Max(1.0f, Math.Min(1.7f, this.ClientSize.Width / 1366f));

            lblTituloTV.Font = new Font("Segoe UI", baseTitleSize * factor, FontStyle.Bold);
            lblGenero.Font = new Font("Segoe UI", baseSubSize * factor, FontStyle.Bold);

            dgvRutina.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", baseHeaderSize * factor, FontStyle.Bold);
            dgvRutina.DefaultCellStyle.Font = new Font("Segoe UI", baseGridSize * factor, FontStyle.Regular);

            dgvRutina.ColumnHeadersHeight = (int)Math.Round(baseHeaderHeight * factor);
            dgvRutina.RowTemplate.Height = (int)Math.Round(baseRowHeight * factor);

            lblTituloTV.Height = (int)Math.Round(90 * factor);
            lblGenero.Height = (int)Math.Round(32 * factor);

            dgvRutina.DefaultCellStyle.Padding = new Padding((int)(8 * factor), (int)(6 * factor), (int)(8 * factor), (int)(6 * factor));
            dgvRutina.AutoResizeColumns();
        }

        private void CargarRutina(List<DetalleRutina> detalles, string nombreRutina)
        {
            dgvRutina.Rows.Clear();

            if (detalles == null || detalles.Count == 0)
            {
                dgvRutina.Rows.Add("Sin ejercicios disponibles", "", "", "");
                return;
            }

            foreach (var d in detalles)
            {
                dgvRutina.Rows.Add(
                    d.EjercicioNombre,
                    d.Series,
                    d.Repeticiones,
                    string.IsNullOrWhiteSpace(d.Carga) ? "-" : d.Carga
                );
            }
            dgvRutina.AutoResizeColumns();
        }
    }


}




