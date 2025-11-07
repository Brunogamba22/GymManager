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


namespace GymManager.Views
{
    public partial class UcRecepcionistaDashboard : UserControl
    {
        // Colores modernos actualizados
        private Color primaryColor = Color.FromArgb(41, 128, 185);
        private Color backgroundColor = Color.FromArgb(245, 247, 250);
        private Color successColor = Color.FromArgb(46, 204, 113);    // Imprimir
        private Color infoColor = Color.FromArgb(52, 152, 219);      // Exportar
        private Color darkColor = Color.FromArgb(52, 73, 94);        // TV
        private Color textColor = Color.FromArgb(52, 73, 94);
        private Color warningColor = Color.FromArgb(241, 196, 15);    // Limpiar
        private Color hoverColor = Color.FromArgb(232, 244, 253);
        private Color gridLineColor = Color.FromArgb(224, 230, 237);
        private Color panelColor = Color.White;

        private UcDetalleRutina ucDetalle = null;

        // Controladores
        private readonly RutinaController _rutinaController = new RutinaController();
        private readonly DetalleRutinaController _detalleController = new DetalleRutinaController();
        private readonly GeneroController _generoController = new GeneroController();
        private readonly UsuarioController _usuarioController = new UsuarioController();

        private List<Rutina> rutinasGuardadas = new List<Rutina>();

        // NOTA: chkTodasLasFechas se define en el Designer.cs

        public UcRecepcionistaDashboard()
        {
            InitializeComponent();
            ApplyModernStyles();
            ConfigurarGrid();
            ConfigurarFiltros();

            btnFiltrar.Click += BtnFiltrar_Click;
            btnImprimir.Click += BtnImprimir_Click;
            btnExportar.Click += BtnExportar_Click;
            btnModoTV.Click += BtnModoTV_Click;
            btnLimpiarFiltros.Click += BtnLimpiarFiltros_Click;

            // Permite deshabilitar el DateTimePicker si se marcan todas las fechas
            chkTodasLasFechas.CheckedChanged += (s, e) => { dtpFecha.Enabled = !chkTodasLasFechas.Checked; };

            dgvPlanillas.CellDoubleClick += DgvPlanillas_CellDoubleClick;
        }

        private void ApplyModernStyles()
        {
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI", 9);

            // Aplicar estilos modernos a todos los botones
            StyleButton(btnFiltrar, primaryColor);
            StyleButton(btnImprimir, successColor);
            StyleButton(btnExportar, infoColor);
            StyleButton(btnModoTV, darkColor);
            StyleButton(btnLimpiarFiltros, warningColor);

            // Estilizar combobox y datetimepicker
            EstilizarCombobox(cmbProfesor);
            EstilizarCombobox(cmbGenero);
            EstilizarDateTimePicker(dtpFecha);

            // Estilizar checkbox
            if (chkSoloEditadas != null)
            {
                chkSoloEditadas.ForeColor = textColor;
                chkSoloEditadas.Font = new Font("Segoe UI", 9);
            }
            if (chkTodasLasFechas != null)
            {
                chkTodasLasFechas.ForeColor = textColor;
                chkTodasLasFechas.Font = new Font("Segoe UI", 9);
            }

            // Estilizar labels
            EstilizarLabels();
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
            dgvPlanillas.AllowUserToAddRows = false;
            dgvPlanillas.AllowUserToDeleteRows = false;
            dgvPlanillas.ReadOnly = true;
            dgvPlanillas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvPlanillas.RowHeadersVisible = false;
            dgvPlanillas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvPlanillas.RowTemplate.Height = 50;
            dgvPlanillas.AllowUserToResizeColumns = false;
            dgvPlanillas.AllowUserToResizeRows = false;
            dgvPlanillas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Encabezados modernos
            dgvPlanillas.ColumnHeadersDefaultCellStyle.BackColor = primaryColor;
            dgvPlanillas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvPlanillas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            dgvPlanillas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPlanillas.ColumnHeadersHeight = 45;

            // Celdas
            dgvPlanillas.DefaultCellStyle.Font = new Font("Segoe UI", 10f);
            dgvPlanillas.DefaultCellStyle.BackColor = Color.White;
            dgvPlanillas.DefaultCellStyle.ForeColor = textColor;
            dgvPlanillas.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvPlanillas.DefaultCellStyle.Padding = new Padding(8);
            dgvPlanillas.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 244, 253);
            dgvPlanillas.DefaultCellStyle.SelectionForeColor = textColor;
            dgvPlanillas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);

            dgvPlanillas.SelectionChanged += DgvPlanillas_SelectionChanged;
            dgvPlanillas.CellMouseEnter += dgvPlanillas_CellMouseEnter;
            dgvPlanillas.CellMouseLeave += dgvPlanillas_CellMouseLeave;
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

        // =========================================================
        // LÓGICA DE CARGA Y FILTRADO (MODIFICADO)
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
            // Limpieza de filtros
            dtpFecha.Value = DateTime.Now.Date;
            chkTodasLasFechas.Checked = false; // Limpiamos el filtro de fecha
            cmbProfesor.SelectedIndex = 0;
            cmbGenero.SelectedIndex = 0;
            if (chkSoloEditadas != null) chkSoloEditadas.Checked = false;

            CargarGrillaConFiltros();
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
        }

        // =========================================================
        // LÓGICA DE NAVEGACIÓN Y ACCIONES
        // =========================================================

        private void DgvPlanillas_SelectionChanged(object sender, EventArgs e)
        {
            HabilitarBotonesDeAccion(dgvPlanillas.SelectedRows.Count > 0);
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
                    // Se asume que ucDetalle es un UserControl ya creado.
                    ucDetalle = new UcDetalleRutina();
                    ucDetalle.Dock = DockStyle.Fill;
                    ucDetalle.OnCerrarDetalle += (s, ev) => OcultarDetalle();
                    this.Controls.Add(ucDetalle);
                }

                List<DetalleRutina> detallesDeRutina = _detalleController.ObtenerPorRutina(rutinaHeader.IdRutina);

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
                MessageBox.Show($"No se pudieron cargar los detalles: {ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private void ExportarRutina(Rutina rutina)
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
                    tabla.AddCell(new iTextSharp.text.pdf.PdfPCell(new iTextSharp.text.Phrase(d.Carga.HasValue ? d.Carga.ToString() : "-", fNormal)) { HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER });
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

        private void ImprimirRutina(Rutina rutina)
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
                        g.DrawString(d.Carga.HasValue ? $"{d.Carga}%" : "-", fTexto, Brushes.Black, xInicio + 540, y + 6);

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


        //---------------------------------------------------------------------

        private List<DetalleRutina> ObtenerDetallesPorGenero(DateTime fecha, string nombreGenero)
        {
            // 1. Encontrar el Género por Nombre
            var genero = _generoController.ObtenerTodos().FirstOrDefault(g =>
                g.Nombre.Equals(nombreGenero, StringComparison.OrdinalIgnoreCase) ||
                // Añadimos lógica flexible si los nombres en la DB son Masc./Fem.
                (nombreGenero == "Hombre" && g.Nombre.StartsWith("Masculino", StringComparison.OrdinalIgnoreCase)) ||
                (nombreGenero == "Mujer" && g.Nombre.StartsWith("Femenino", StringComparison.OrdinalIgnoreCase))
            );

            if (genero == null) return new List<DetalleRutina>();

            // 2. Obtener la ÚLTIMA rutina creada para ese género en esa fecha
            // Usamos la fecha como filtro (sin el filtro de 'todas las fechas')
            var rutinas = _rutinaController.ObtenerTodasParaPlanilla(fecha, fecha, genero.Id);

            if (rutinas.Count > 0)
            {
                // Tomamos la rutina más reciente.
                var rutinaActiva = rutinas.OrderByDescending(r => r.FechaCreacion).First();

                // 3. Obtener los Detalles de la Rutina Activa
                return _detalleController.ObtenerPorRutina(rutinaActiva.IdRutina);
            }

            return new List<DetalleRutina>();
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
                dgvPlanillas.Rows[e.RowIndex].DefaultCellStyle.BackColor = hoverColor;
        }

        private void dgvPlanillas_CellMouseLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dgvPlanillas.Rows[e.RowIndex];
                Color originalColor = (e.RowIndex % 2 == 0)
                    ? dgvPlanillas.DefaultCellStyle.BackColor
                    : dgvPlanillas.AlternatingRowsDefaultCellStyle.BackColor;
                row.DefaultCellStyle.BackColor = originalColor;
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
        // 🎨 Colores y fuentes base para el modo TV
        private Color tvBackColor = Color.FromArgb(33, 37, 41);
        private Color tvGridColor = Color.FromArgb(52, 58, 64);
        private Color tvHeaderColor = Color.FromArgb(41, 128, 185);
        private Color tvTitleColor = Color.FromArgb(241, 196, 15);
        private Font tvGridFont = new Font("Segoe UI", 11f);
        private Font tvHeaderFont = new Font("Segoe UI", 12f, FontStyle.Bold);
        private Font tvTitleFont = new Font("Segoe UI", 28f, FontStyle.Bold);

        // 🔹 Controles principales
        private Label lblTituloTV;
        private DataGridView dgvRutina;

        // ========================================================================
        // 🔹 CONSTRUCTOR PRINCIPAL
        // ========================================================================
        public FormTV(string nombreProfesor, string nombreRutina, List<DetalleRutina> detalles)
        {
            // Inicializa la estructura visual del formulario
            InitializeForm();

            // 🔹 Título de la ventana (arriba en la barra de título)
            this.Text = $"Modo TV - {nombreProfesor}";

            // 🔹 Texto visible grande dentro de la pantalla
            lblTituloTV.Text = $"RUTINA DEL PROFESOR: {nombreProfesor.ToUpper()}";

            // 🔹 Llenar la grilla con los ejercicios de la rutina
            CargarRutina(detalles, nombreRutina);
        }

        // ========================================================================
        // 🔹 CONFIGURACIÓN VISUAL DEL FORMULARIO
        // ========================================================================
        private void InitializeForm()
        {
            // --- Configuración general del formulario ---
            this.BackColor = tvBackColor;
            this.WindowState = FormWindowState.Maximized;  // Abre en pantalla completa
            this.FormBorderStyle = FormBorderStyle.Sizable; // Permite minimizar / mover
            this.MinimizeBox = true;
            this.MaximizeBox = true;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.KeyPreview = true;  // Permite detectar teclas (como ESC)
            this.KeyDown += (s, e) => { if (e.KeyCode == Keys.Escape) this.Close(); };

            // --- Diseño general con TableLayoutPanel (2 filas) ---
            TableLayoutPanel tlpMain = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                BackColor = tvBackColor,
                RowCount = 2,
                ColumnCount = 1
            };
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Absolute, 100));  // fila título
            tlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100));   // fila tabla
            this.Controls.Add(tlpMain);

            // --- Título principal (grande arriba) ---
            lblTituloTV = new Label
            {
                Dock = DockStyle.Fill,
                Font = tvTitleFont,
                ForeColor = tvTitleColor,
                Text = "RUTINA DEL PROFESOR",
                TextAlign = ContentAlignment.MiddleCenter
            };
            tlpMain.Controls.Add(lblTituloTV, 0, 0);

            // --- DataGridView donde se muestran los ejercicios ---
            dgvRutina = CrearGrillaTV();
            tlpMain.Controls.Add(dgvRutina, 0, 1);
        }

        // ========================================================================
        // 🔹 CONFIGURACIÓN DE LA GRILLA DE EJERCICIOS
        // ========================================================================
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
                RowTemplate = { Height = 60 },
                AllowUserToResizeRows = false,
                ColumnHeadersHeight = 50
            };

            // --- Estilo de encabezado ---
            dgv.ColumnHeadersDefaultCellStyle.BackColor = tvHeaderColor;
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgv.ColumnHeadersDefaultCellStyle.Font = tvHeaderFont;
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            // --- Estilo de filas ---
            dgv.DefaultCellStyle.Font = tvGridFont;
            dgv.DefaultCellStyle.BackColor = tvBackColor;
            dgv.DefaultCellStyle.ForeColor = Color.White;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.SelectionBackColor = tvBackColor;
            dgv.DefaultCellStyle.SelectionForeColor = Color.White;
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(40, 44, 52);

            // --- Columnas ---
            dgv.Columns.Add("EJERCICIO", "EJERCICIO");
            dgv.Columns.Add("SERIES", "SERIES");
            dgv.Columns.Add("REPS", "REPS");
            dgv.Columns.Add("CARGA", "CARGA %");

            return dgv;
        }

        // ========================================================================
        // 🔹 CARGA DE DATOS EN LA GRILLA
        // ========================================================================
        private void CargarRutina(List<DetalleRutina> detalles, string nombreRutina)
        {
            dgvRutina.Rows.Clear();

            // Mostrar mensaje si la rutina no tiene ejercicios
            if (detalles == null || detalles.Count == 0)
            {
                dgvRutina.Rows.Add("Sin ejercicios disponibles", "", "", "");
                return;
            }

            // Agregar cada ejercicio como una fila en la grilla
            foreach (var d in detalles)
            {
                dgvRutina.Rows.Add(
                    d.EjercicioNombre,
                    d.Series,
                    d.Repeticiones,
                    d.Carga?.ToString() ?? "-"
                );
            }

            // Ajustar el tamaño de las columnas automáticamente
            dgvRutina.AutoResizeColumns();
        }
    }

}




