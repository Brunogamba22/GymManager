using GymManager.Controllers;
using GymManager.Models;
using GymManager.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Printing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;


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


public class FormTV : Form
    {
        // 🎨 Colores principales del modo TV
        private Color tvBackColor = Color.FromArgb(33, 37, 41);     // Fondo oscuro
        private Color tvGridColor = Color.FromArgb(52, 58, 64);     // Color de líneas de la grilla
        private Color tvHeaderColor = Color.FromArgb(41, 128, 185); // Azul encabezado
        private Color tvTitleColor = Color.FromArgb(241, 196, 15);  // Amarillo dorado (título)

        // 🔹 Tamaños base para escalado
        private float baseTitleSize = 34f;    // Título ligeramente más chico
        private float baseSubSize = 15f;      // Subtítulo (género)
        private float baseHeaderSize = 13f;   // Encabezado de columnas
        private float baseGridSize = 12f;     // Texto de filas
        private int baseRowHeight = 58;       // Altura de filas
        private int baseHeaderHeight = 56;    // Altura encabezado

        // 🎬 Variables para el carrusel
        private Timer timerCarrusel;
        private int indiceCarrusel = 0;
        private PictureBox picGifGrande;
        private Label lblInfoEjercicio;
        private Label lblContador;
        private SplitContainer splitContainer;
        private List<DetalleRutina> detallesActuales;

        // 📋 Variables de clase
        private string generoRutina;
        private Label lblTituloTV;
        private Label lblGenero;
        private DataGridView dgvRutina;
        private TableLayoutPanel tlpMain;

        // =====================================================================
        // 🧩 CONSTRUCTOR PRINCIPAL
        // =====================================================================
        public FormTV(string nombreProfesor, string nombreRutina, string genero, List<DetalleRutina> detalles)
        {
            this.detallesActuales = detalles;
            this.generoRutina = genero ?? string.Empty;

            InitializeForm();

            this.Text = $"Modo TV - {nombreProfesor}";
            lblTituloTV.Text = $"RUTINA DEL PROFESOR: {nombreProfesor.ToUpper()}";

            var etiqueta = EtiquetaGenero(this.generoRutina);
            lblGenero.Text = etiqueta ?? "";
            lblGenero.Visible = etiqueta != null;

            CargarRutina(detalles, nombreRutina);
            IniciarCarrusel(detalles);
            ApplyResponsiveScale();

            // Eventos para manejo seguro del redimensionamiento
            this.Resize += (s, e) =>
            {
                if (this.WindowState != FormWindowState.Minimized)
                    ApplyResponsiveScaleSafe();
            };
        }

        public FormTV(string nombreProfesor, string nombreRutina, List<DetalleRutina> detalles)
            : this(nombreProfesor, nombreRutina, null, detalles) { }

        // =====================================================================
        // ⚙️ MÉTODO: InitializeForm
        // =====================================================================
        private void InitializeForm()
        {
            // Configuración base de ventana
            this.BackColor = tvBackColor;
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.Sizable;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;
            this.KeyDown += (s, e) =>
            {
                if (e.KeyCode == Keys.Escape) this.Close();
                if (e.KeyCode == Keys.Space) PausarReanudarCarrusel();
                if (e.KeyCode == Keys.Right) MostrarSiguienteGif();
                if (e.KeyCode == Keys.Left) MostrarGifAnterior();
            };

            // Layout principal
            tlpMain = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20, 10, 20, 20),
                BackColor = tvBackColor,
                RowCount = 3,
                ColumnCount = 1
            };
            tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
            this.Controls.Add(tlpMain);

            // Título y subtítulo
            lblTituloTV = new Label
            {
                Dock = DockStyle.Top,
                AutoSize = false,
                ForeColor = tvTitleColor,
                Text = "RUTINA DEL PROFESOR",
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 90,
                Padding = new Padding(0, 6, 0, 4),
            };
            tlpMain.Controls.Add(lblTituloTV, 0, 0);

            lblGenero = new Label
            {
                Dock = DockStyle.Top,
                AutoSize = false,
                ForeColor = Color.FromArgb(220, 225, 230),
                Text = "",
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 30,
                Padding = new Padding(0, 0, 0, 4),
                Visible = false
            };
            tlpMain.Controls.Add(lblGenero, 0, 1);

            // ============ CREAR SPLIT CONTAINER ============
            splitContainer = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterDistance = 250,
                Panel1 = { BackColor = tvBackColor },
                Panel2 = { BackColor = Color.Black }
            };

            // 🔥 CONFIGURAR TAMAÑOS MÍNIMOS para evitar errores
            splitContainer.Panel1MinSize = 100;
            splitContainer.Panel2MinSize = 100;

            // Panel superior: DataGridView
            dgvRutina = CrearGrillaTV();
            dgvRutina.Dock = DockStyle.Fill;
            splitContainer.Panel1.Controls.Add(dgvRutina);

            // Agregar SplitContainer al layout principal
            tlpMain.Controls.Add(splitContainer, 0, 2);

            // Eventos para manejo seguro del redimensionamiento
            this.ResizeBegin += (s, e) => splitContainer.SuspendLayout();
            this.ResizeEnd += (s, e) => splitContainer.ResumeLayout();
        }

        // =====================================================================
        // 🎬 MÉTODOS PARA EL CARRUSEL
        // =====================================================================
        private void IniciarCarrusel(List<DetalleRutina> detalles)
        {
            if (detalles == null || detalles.Count == 0) return;

            // Limpiar panel anterior si existe
            splitContainer.Panel2.Controls.Clear();

            // Panel principal del carrusel
            var pnlCarrusel = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.Black
            };

            // PictureBox para el GIF grande
            picGifGrande = new PictureBox
            {
                Dock = DockStyle.Fill,
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = Color.Black
            };

            // Panel inferior para información
            var pnlInfo = new Panel
            {
                Dock = DockStyle.Bottom,
                Height = 100,
                BackColor = Color.FromArgb(40, 40, 40, 200) // Semi-transparente
            };

            // Label con información del ejercicio
            lblInfoEjercicio = new Label
            {
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.Transparent,
                Padding = new Padding(0, 10, 0, 0)
            };

            // Label contador (ej: 1/5)
            lblContador = new Label
            {
                Dock = DockStyle.Bottom,
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                ForeColor = Color.LightGray,
                BackColor = Color.Transparent
            };

            // Botones de control (opcional, ocultos por defecto)
            var btnAnterior = new Button
            {
                Text = "◀",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 60, 60),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(60, 60),
                Location = new Point(20, (pnlCarrusel.Height - 60) / 2),
                Visible = false,
                TabStop = false
            };
            btnAnterior.Click += (s, e) => MostrarGifAnterior();

            var btnSiguiente = new Button
            {
                Text = "▶",
                Font = new Font("Arial", 20, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 60, 60),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(60, 60),
                Location = new Point(pnlCarrusel.Width - 80, (pnlCarrusel.Height - 60) / 2),
                Visible = false,
                TabStop = false
            };
            btnSiguiente.Click += (s, e) => MostrarSiguienteGif();

            var btnPausa = new Button
            {
                Text = "⏸",
                Font = new Font("Arial", 16, FontStyle.Bold),
                ForeColor = Color.White,
                BackColor = Color.FromArgb(60, 60, 60),
                FlatStyle = FlatStyle.Flat,
                Size = new Size(50, 50),
                Location = new Point(pnlCarrusel.Width / 2 - 25, 20),
                Visible = false,
                TabStop = false
            };
            btnPausa.Click += (s, e) => PausarReanudarCarrusel();

            // Mostrar controles al pasar el mouse
            pnlCarrusel.MouseEnter += (s, e) =>
            {
                btnAnterior.Visible = true;
                btnSiguiente.Visible = true;
                btnPausa.Visible = true;
            };

            pnlCarrusel.MouseLeave += (s, e) =>
            {
                btnAnterior.Visible = false;
                btnSiguiente.Visible = false;
                btnPausa.Visible = false;
            };

            // Agregar controles
            pnlInfo.Controls.Add(lblInfoEjercicio);
            pnlInfo.Controls.Add(lblContador);

            pnlCarrusel.Controls.Add(picGifGrande);
            pnlCarrusel.Controls.Add(pnlInfo);
            pnlCarrusel.Controls.Add(btnAnterior);
            pnlCarrusel.Controls.Add(btnSiguiente);
            pnlCarrusel.Controls.Add(btnPausa);

            splitContainer.Panel2.Controls.Add(pnlCarrusel);

            // Configurar timer del carrusel (8 segundos por default)
            timerCarrusel = new Timer
            {
                Interval = 8000,
                Enabled = true
            };
            timerCarrusel.Tick += (s, e) => MostrarSiguienteGif();

            // Mostrar primer GIF
            MostrarSiguienteGif();
        }

        private void MostrarSiguienteGif()
        {
            if (detallesActuales == null || detallesActuales.Count == 0) return;

            var detalle = detallesActuales[indiceCarrusel];
            string gifPath = BuscarGif(detalle.Imagen, detalle.GrupoMuscular);

            // Actualizar imagen
            if (!string.IsNullOrEmpty(gifPath) && File.Exists(gifPath))
            {
                try
                {
                    // Liberar imagen anterior si existe
                    if (picGifGrande.Image != null)
                    {
                        picGifGrande.Image.Dispose();
                    }
                    picGifGrande.Image = Image.FromFile(gifPath);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error cargando GIF: {ex.Message}");
                    picGifGrande.Image = null;
                }
            }
            else
            {
                picGifGrande.Image = null;
            }

            // Actualizar información
            string cargaDisplay = string.IsNullOrWhiteSpace(detalle.Carga) ? "Sin carga" : detalle.Carga;
            lblInfoEjercicio.Text = $"{detalle.EjercicioNombre}\n" +
                                   $"Series: {detalle.Series} × Reps: {detalle.Repeticiones} | Carga: {cargaDisplay}";

            lblContador.Text = $"{indiceCarrusel + 1} / {detallesActuales.Count}";

            // Resaltar fila en el DataGridView
            if (dgvRutina.Rows.Count > indiceCarrusel)
            {
                dgvRutina.ClearSelection();
                dgvRutina.Rows[indiceCarrusel].Selected = true;
                dgvRutina.FirstDisplayedScrollingRowIndex = indiceCarrusel;
            }

            // Avanzar índice (circular)
            indiceCarrusel = (indiceCarrusel + 1) % detallesActuales.Count;
        }

        private void MostrarGifAnterior()
        {
            if (detallesActuales == null || detallesActuales.Count == 0) return;

            // Retroceder índice (circular)
            indiceCarrusel = (indiceCarrusel - 1 + detallesActuales.Count) % detallesActuales.Count;
            MostrarSiguienteGif(); // Reutilizar la lógica de mostrar
        }

        private void PausarReanudarCarrusel()
        {
            if (timerCarrusel != null)
            {
                timerCarrusel.Enabled = !timerCarrusel.Enabled;

                // Mostrar indicador de estado
                if (!timerCarrusel.Enabled)
                    lblContador.Text += " ⏸ PAUSADO";
            }
        }

        // =====================================================================
        // 📋 MÉTODO: CargarRutina
        // =====================================================================
        private void CargarRutina(List<DetalleRutina> detalles, string nombreRutina)
        {
            dgvRutina.Rows.Clear();
            detallesActuales = detalles;

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

        // =====================================================================
        // 🔍 MÉTODO: Buscar archivo GIF
        // =====================================================================
        private string BuscarGif(string nombreArchivo, string grupoMuscular)
        {
            try
            {
                if (string.IsNullOrEmpty(nombreArchivo))
                {
                    Debug.WriteLine($"❌ nombreArchivo está vacío");
                    return null;
                }

                string nombreBuscar = Path.GetFileNameWithoutExtension(nombreArchivo).ToLower();
                Debug.WriteLine($"\n🔍 BUSCANDO GIF: '{nombreBuscar}' en grupo: '{grupoMuscular}'");

                string appPath = Application.StartupPath;
                string rutaBaseGifs = Path.Combine(appPath, "Resources", "Ejercicios");

                if (!Directory.Exists(rutaBaseGifs))
                {
                    Debug.WriteLine($"❌ Carpeta base no existe: {rutaBaseGifs}");
                    return null;
                }

                // 1. Primero buscar en carpeta específica del grupo muscular
                string carpetaGrupo = "";
                if (!string.IsNullOrEmpty(grupoMuscular))
                {
                    string grupoLower = grupoMuscular.ToLower();

                    // Mapeo manual de grupos a carpetas (más preciso)
                    if (grupoLower.Contains("bíceps") || grupoLower.Contains("biceps"))
                        carpetaGrupo = Path.Combine(rutaBaseGifs, "Biceps");
                    else if (grupoLower.Contains("pecho"))
                        carpetaGrupo = Path.Combine(rutaBaseGifs, "Pecho");
                    else if (grupoLower.Contains("hombro"))
                        carpetaGrupo = Path.Combine(rutaBaseGifs, "Hombros");
                    else if (grupoLower.Contains("tríceps") || grupoLower.Contains("triceps"))
                        carpetaGrupo = Path.Combine(rutaBaseGifs, "Triceps");
                    else if (grupoLower.Contains("abdominal"))
                        carpetaGrupo = Path.Combine(rutaBaseGifs, "Abdominales");
                    else if (grupoLower.Contains("pierna") || grupoLower.Contains("piema"))
                        carpetaGrupo = Path.Combine(rutaBaseGifs, "Piemas");
                    else if (grupoLower.Contains("espalda"))
                        carpetaGrupo = Path.Combine(rutaBaseGifs, "Espalda");
                    else
                    {
                        // Buscar carpeta por coincidencia
                        string[] carpetas = Directory.GetDirectories(rutaBaseGifs);
                        foreach (string carpeta in carpetas)
                        {
                            string nombreCarpeta = Path.GetFileName(carpeta).ToLower();
                            if (grupoLower.Contains(nombreCarpeta) || nombreCarpeta.Contains(grupoLower))
                            {
                                carpetaGrupo = carpeta;
                                break;
                            }
                        }
                    }
                }

                // Si no se encontró carpeta específica, buscar en todas
                if (string.IsNullOrEmpty(carpetaGrupo))
                {
                    carpetaGrupo = rutaBaseGifs;
                    Debug.WriteLine($"📂 Buscando en TODAS las carpetas");
                }
                else
                {
                    Debug.WriteLine($"📂 Buscando en carpeta: {Path.GetFileName(carpetaGrupo)}");
                }

                // 2. Buscar archivo GIF con coincidencia EXACTA primero
                string[] todosGifs = Directory.GetFiles(carpetaGrupo, "*.gif", SearchOption.AllDirectories);
                Debug.WriteLine($"🎯 Encontrados {todosGifs.Length} GIFs");

                if (todosGifs.Length == 0)
                {
                    Debug.WriteLine($"❌ No hay GIFs en {carpetaGrupo}");
                    return null;
                }

                // Lista de GIFs ya usados para evitar repeticiones
                List<string> gifsUsados = new List<string>();

                // ESTRATEGIA 1: Buscar coincidencia EXACTA (ignorando guiones y espacios)
                foreach (string gifPath in todosGifs)
                {
                    string nombreGif = Path.GetFileNameWithoutExtension(gifPath).ToLower();

                    // Normalizar nombres (quitar guiones, espacios, acentos)
                    string nombreBuscarNormalizado = NormalizarNombre(nombreBuscar);
                    string nombreGifNormalizado = NormalizarNombre(nombreGif);

                    Debug.WriteLine($"   🔎 Comparando: '{nombreBuscarNormalizado}' vs '{nombreGifNormalizado}'");

                    // Coincidencia exacta normalizada
                    if (nombreBuscarNormalizado == nombreGifNormalizado && !gifsUsados.Contains(gifPath))
                    {
                        Debug.WriteLine($"   ✅ COINCIDENCIA EXACTA: {Path.GetFileName(gifPath)}");
                        gifsUsados.Add(gifPath);
                        return gifPath;
                    }

                    // Coincidencia parcial fuerte (80% del nombre)
                    if (CoincidenciaParcialFuerte(nombreBuscarNormalizado, nombreGifNormalizado) && !gifsUsados.Contains(gifPath))
                    {
                        Debug.WriteLine($"   ✅ COINCIDENCIA FUERTE: {Path.GetFileName(gifPath)}");
                        gifsUsados.Add(gifPath);
                        return gifPath;
                    }
                }

                // ESTRATEGIA 2: Buscar por PALABRAS CLAVE principales
                foreach (string gifPath in todosGifs)
                {
                    if (gifsUsados.Contains(gifPath)) continue;

                    string nombreGif = Path.GetFileNameWithoutExtension(gifPath).ToLower();
                    string nombreGifNormalizado = NormalizarNombre(nombreGif);

                    // Extraer palabras clave del ejercicio buscado
                    string[] palabrasClave = ExtraerPalabrasClave(nombreBuscar);

                    int coincidencias = 0;
                    foreach (string palabra in palabrasClave)
                    {
                        if (nombreGifNormalizado.Contains(palabra))
                        {
                            coincidencias++;
                            Debug.WriteLine($"   ✓ Palabra coincidente: '{palabra}'");
                        }
                    }

                    // Si al menos 2 palabras clave coinciden (o 1 si es palabra clave fuerte)
                    if (coincidencias >= 2 || (coincidencias == 1 && EsPalabraClaveFuerte(palabrasClave[0])))
                    {
                        Debug.WriteLine($"   ✅ {coincidencias} palabras coincidentes: {Path.GetFileName(gifPath)}");
                        gifsUsados.Add(gifPath);
                        return gifPath;
                    }
                }

                // ESTRATEGIA 3: Buscar cualquier GIF no usado del mismo grupo
                foreach (string gifPath in todosGifs)
                {
                    if (!gifsUsados.Contains(gifPath))
                    {
                        Debug.WriteLine($"   ⚠️ Usando GIF disponible: {Path.GetFileName(gifPath)}");
                        gifsUsados.Add(gifPath);
                        return gifPath;
                    }
                }

                // Si no hay GIFs disponibles, usar el primero
                Debug.WriteLine($"   ⚠️ Todos los GIFs usados, usando el primero");
                return todosGifs[0];
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"💥 ERROR en BuscarGif: {ex.Message}");
                return null;
            }
        }

        // =====================================================================
        // 🛠️ MÉTODOS AUXILIARES
        // =====================================================================
        private string NormalizarNombre(string nombre)
        {
            if (string.IsNullOrEmpty(nombre)) return "";

            string normalizado = nombre.ToLower()
                .Replace("_", "")
                .Replace(" ", "")
                .Replace("-", "")
                .Replace("á", "a").Replace("é", "e").Replace("í", "i")
                .Replace("ó", "o").Replace("ú", "u").Replace("ñ", "n");

            normalizado = normalizado
                .Replace("de", "")
                .Replace("con", "")
                .Replace("la", "")
                .Replace("las", "")
                .Replace("los", "")
                .Replace("el", "")
                .Replace("un", "")
                .Replace("una", "")
                .Replace("y", "")
                .Replace("en", "");

            return normalizado;
        }

        private bool CoincidenciaParcialFuerte(string nombre1, string nombre2)
        {
            if (nombre1.Contains(nombre2) && nombre2.Length >= nombre1.Length * 0.8)
                return true;

            if (nombre2.Contains(nombre1) && nombre1.Length >= nombre2.Length * 0.8)
                return true;

            return false;
        }

        private string[] ExtraerPalabrasClave(string nombreEjercicio)
        {
            string[] separadores = new string[] { "_", " ", "de", "con", "la", "los", "las", "en", "y", "del" };
            string[] palabras = nombreEjercicio.Split(separadores, StringSplitOptions.RemoveEmptyEntries);

            var palabrasClave = new List<string>();
            foreach (string palabra in palabras)
            {
                if (palabra.Length > 3 && !EsPalabraComun(palabra))
                {
                    palabrasClave.Add(NormalizarNombre(palabra));
                }
            }

            return palabrasClave.ToArray();
        }

        private bool EsPalabraComun(string palabra)
        {
            string[] palabrasComunes = { "brazo", "brazos", "pierna", "piernas", "cuerpo", "cable", "barra",
                               "mancuerna", "mancuernas", "polea", "poleas", "maquina", "máquina" };

            return palabrasComunes.Contains(palabra.ToLower());
        }

        private bool EsPalabraClaveFuerte(string palabra)
        {
            string[] palabrasFuertes = { "curl", "press", "sentadilla", "remo", "prensa", "extension",
                               "flexion", "elevacion", "abduccion", "aduccion", "crunch" };

            return palabrasFuertes.Contains(palabra.ToLower());
        }

        // =====================================================================
        // 🏷️ Etiqueta de género
        // =====================================================================
        private static string EtiquetaGenero(string genero)
        {
            if (string.IsNullOrWhiteSpace(genero)) return null;

            var g = genero.Trim().ToLower();
            if (g.Contains("masc") || g.Contains("hombre")) return "PARA: HOMBRES";
            if (g.Contains("fem") || g.Contains("mujer")) return "PARA: MUJERES";
            if (g.Contains("deport")) return "PARA: DEPORTISTAS";

            try
            {
                var nice = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(g);
                return $"PARA: {nice}";
            }
            catch { return $"PARA: {genero.ToUpper()}"; }
        }

        // =====================================================================
        // 🧱 Crear DataGridView de rutina
        // =====================================================================
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

            // Encabezado visual
            dgv.ColumnHeadersDefaultCellStyle.BackColor = tvHeaderColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // Celdas
            dgv.DefaultCellStyle.BackColor = tvBackColor;
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.SelectionBackColor = tvBackColor;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 44, 52);

            // Columnas visibles
            var colEj = new DataGridViewTextBoxColumn { Name = "EJERCICIO", HeaderText = "EJERCICIO", FillWeight = 50 };
            var colSe = new DataGridViewTextBoxColumn { Name = "SERIES", HeaderText = "SERIES", FillWeight = 16 };
            var colRe = new DataGridViewTextBoxColumn { Name = "REPS", HeaderText = "REPS", FillWeight = 16 };
            var colCa = new DataGridViewTextBoxColumn { Name = "CARGA", HeaderText = "CARGA %", FillWeight = 16 };

            dgv.Columns.AddRange(colEj, colSe, colRe, colCa);
            return dgv;
        }

        // =====================================================================
        // 🔠 Escalado responsivo SEGURO
        // =====================================================================
        private void ApplyResponsiveScale()
        {
            try
            {
                ApplyResponsiveScaleSafe();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en ApplyResponsiveScale: {ex.Message}");
            }
        }

        private void ApplyResponsiveScaleSafe()
        {
            try
            {
                if (this.ClientSize.Width <= 0 || this.ClientSize.Height <= 0)
                    return;

                float factor = Math.Max(0.9f, Math.Min(1.5f, this.ClientSize.Width / 1366f));

                // Escala textos
                lblTituloTV.Font = new Font("Segoe UI", baseTitleSize * factor, FontStyle.Bold);
                lblGenero.Font = new Font("Segoe UI", baseSubSize * factor, FontStyle.Bold);
                dgvRutina.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", baseHeaderSize * factor, FontStyle.Bold);
                dgvRutina.DefaultCellStyle.Font = new Font("Segoe UI", baseGridSize * factor, FontStyle.Regular);

                if (lblInfoEjercicio != null)
                    lblInfoEjercicio.Font = new Font("Segoe UI", 14 * factor, FontStyle.Bold);

                if (lblContador != null)
                    lblContador.Font = new Font("Segoe UI", 10 * factor, FontStyle.Regular);

                // Ajusta alturas
                dgvRutina.ColumnHeadersHeight = (int)Math.Round(baseHeaderHeight * factor);
                dgvRutina.RowTemplate.Height = (int)Math.Round(baseRowHeight * factor);

                // 🔥 CORRECCIÓN SEGURA: Ajustar SplitterDistance
                if (splitContainer != null && splitContainer.Visible && splitContainer.Height > 200)
                {
                    int alturaDisponible = splitContainer.Height;

                    // Calcular nueva distancia (50% para la tabla, 50% para el carrusel)
                    int nuevaDistancia = (int)(alturaDisponible * 0.5);

                    // Limitar valores entre los mínimos permitidos
                    int minDistancia = Math.Max(splitContainer.Panel1MinSize, 100);
                    int maxDistancia = alturaDisponible - Math.Max(splitContainer.Panel2MinSize, 100);

                    // Asegurar que esté dentro de límites válidos
                    if (nuevaDistancia < minDistancia) nuevaDistancia = minDistancia;
                    if (nuevaDistancia > maxDistancia) nuevaDistancia = maxDistancia;

                    // Solo asignar si es diferente y válido
                    if (nuevaDistancia > 0 && nuevaDistancia < alturaDisponible)
                    {
                        splitContainer.SplitterDistance = nuevaDistancia;
                    }
                }

                lblTituloTV.Height = (int)Math.Round(80 * factor);
                lblGenero.Height = (int)Math.Round(28 * factor);

                dgvRutina.DefaultCellStyle.Padding = new Padding((int)(6 * factor), (int)(4 * factor), (int)(6 * factor), (int)(4 * factor));
                dgvRutina.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error en ApplyResponsiveScaleSafe: {ex.Message}");
            }
        }

        // =====================================================================
        // 🧹 Limpieza al cerrar
        // =====================================================================
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (timerCarrusel != null)
            {
                timerCarrusel.Stop();
                timerCarrusel.Dispose();
            }

            if (picGifGrande != null && picGifGrande.Image != null)
            {
                picGifGrande.Image.Dispose();
            }
        }
    }

}



