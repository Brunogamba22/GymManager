using GymManager.Controllers;
using GymManager.Models;
using GymManager.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace GymManager.Views
{
    public partial class UcGestionEjercicios : UserControl
    {
        private readonly EjercicioController controller = new EjercicioController();
        private readonly Dictionary<string, int> gruposMusculares = new Dictionary<string, int>();
        private string rutaRelativaSeleccionada = null;
        private List<Ejercicio> listaActual = new List<Ejercicio>();

        private static readonly string AssetsRoot =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Ejercicios");

        // Colores modernos para la aplicación
        private readonly Color ColorPrimario = Color.FromArgb(41, 128, 185);     // Azul profesional
        private readonly Color ColorSecundario = Color.FromArgb(52, 152, 219);   // Azul claro
        private readonly Color ColorExito = Color.FromArgb(46, 204, 113);        // Verde
        private readonly Color ColorAdvertencia = Color.FromArgb(241, 196, 15);  // Amarillo
        private readonly Color ColorPeligro = Color.FromArgb(231, 76, 60);       // Rojo
        private readonly Color ColorFondo = Color.FromArgb(245, 247, 250);       // Gris muy claro
        private readonly Color ColorBorde = Color.FromArgb(224, 230, 237);       // Gris borde
        private readonly Color ColorTexto = Color.FromArgb(52, 73, 94);          // Gris oscuro texto

        public UcGestionEjercicios()
        {
            InitializeComponent();
            AplicarEstiloModerno();
            CargarGruposMusculares();
            EstilizarBotones();

            cmbTipoBusqueda.Items.Clear();
            cmbTipoBusqueda.Items.AddRange(new object[] { "Todos", "ID", "Nombre", "Grupo Muscular" });
            cmbTipoBusqueda.SelectedIndex = 0;
            CargarComboEstado();
            RefrescarGrid();

            AplicarPlaceholder(txtNombre, "Nombre del ejercicio");
            AplicarPlaceholder(txtImagen, "Ruta relativa (p.ej. Pecho/press_banca.gif)");
        }

        // ====================== MÉTODOS DE ESTILO MODERNO ======================

        private void AplicarEstiloModerno()
        {
            this.BackColor = ColorFondo;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            // Estilo del título
            lblTitulo.ForeColor = ColorTexto;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.BackColor = Color.Transparent;

            // Estilo de labels
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Label label)
                {
                    label.ForeColor = ColorTexto;
                    label.BackColor = Color.Transparent;
                }
            }

            // Estilo de grupos/paneles
            EstilizarControles();
        }

        private void EstilizarControles()
        {
            // Estilo para TextBox
            EstilizarTextBox(txtNombre);
            EstilizarTextBox(txtImagen);
            EstilizarTextBox(txtBuscar);

            // Estilo para ComboBox
            EstilizarComboBox(cmbMusculo);
            EstilizarComboBox(cmbTipoBusqueda);
            EstilizarComboBox(cmbEstado);

            // Estilo para PictureBox - SOLO CORREGIDO
            pictureBoxEjercicio.BorderStyle = BorderStyle.FixedSingle;
            pictureBoxEjercicio.BackColor = Color.White;
        }

        private void EstilizarTextBox(TextBox txt)
        {
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.BackColor = Color.White;
            txt.ForeColor = ColorTexto;
            txt.Font = new Font("Segoe UI", 9F);
        }

        private void EstilizarComboBox(ComboBox cmb)
        {
            cmb.FlatStyle = FlatStyle.Flat;
            cmb.BackColor = Color.White;
            cmb.ForeColor = ColorTexto;
            cmb.Font = new Font("Segoe UI", 9F);
        }

        // ====================== HELPERS DE RUTAS/IMÁGENES ======================

        private static string AbsPath(string relative) =>
            string.IsNullOrWhiteSpace(relative) ? null : Path.Combine(AssetsRoot, relative);

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            if (pictureBoxEjercicio.Image != null &&
                ImageFormat.Gif.Equals(pictureBoxEjercicio.Image.RawFormat))
            {
                System.Drawing.ImageAnimator.UpdateFrames(pictureBoxEjercicio.Image);
            }
        }

        private static string ImportToAssets(string sourceFullPath, string subfolder = null)
        {
            Directory.CreateDirectory(AssetsRoot);

            string destFolder = string.IsNullOrWhiteSpace(subfolder)
                                ? AssetsRoot
                                : Path.Combine(AssetsRoot, subfolder);

            Directory.CreateDirectory(destFolder);

            string fileName = Path.GetFileName(sourceFullPath);
            string name = Path.GetFileNameWithoutExtension(fileName);
            string ext = Path.GetExtension(fileName);
            string target = Path.Combine(destFolder, fileName);

            int i = 1;
            while (File.Exists(target))
                target = Path.Combine(destFolder, $"{name}_{i++}{ext}");

            File.Copy(sourceFullPath, target, overwrite: false);
            return GetRelativePath(AssetsRoot, target);
        }

        private static Image LoadImageSafe(string absPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(absPath) || !File.Exists(absPath))
                    return null;

                string tempFile = Path.GetTempFileName();
                File.Copy(absPath, tempFile, true);
                Image img = Image.FromFile(tempFile);

                if (ImageFormat.Gif.Equals(img.RawFormat))
                {
                    System.Drawing.ImageAnimator.Animate(img, (s, e) => { });
                }

                return img;
            }
            catch
            {
                return null;
            }
        }

        private static Image GetThumbnail70(string absPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(absPath) || !File.Exists(absPath))
                    return null;

                using (var fs = new FileStream(absPath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                using (var src = Image.FromStream(fs))
                {
                    bool esGif = ImageFormat.Gif.Equals(src.RawFormat);
                    if (esGif && src.FrameDimensionsList?.Length > 0)
                    {
                        var dim = new FrameDimension(src.FrameDimensionsList[0]);
                        if (src.GetFrameCount(FrameDimension.Time) > 0)
                            src.SelectActiveFrame(FrameDimension.Time, 0);
                    }

                    var thumb = new Bitmap(70, 70);
                    using (var g = Graphics.FromImage(thumb))
                    {
                        g.Clear(Color.Transparent);
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;

                        var ratio = Math.Min(70f / src.Width, 70f / src.Height);
                        var w = (int)(src.Width * ratio);
                        var h = (int)(src.Height * ratio);
                        var x = (70 - w) / 2;
                        var y = (70 - h) / 2;

                        g.DrawImage(src, new Rectangle(x, y, w, h));
                    }

                    return thumb;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("⚠️ Error al generar miniatura: " + ex.Message);
                return null;
            }
        }

        private static string EnsureDirSep(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            char sep = Path.DirectorySeparatorChar;
            return path.EndsWith(sep.ToString()) ? path : path + sep;
        }

        private static string GetRelativePath(string basePath, string fullPath)
        {
            var baseUri = new Uri(EnsureDirSep(basePath));
            var fileUri = new Uri(fullPath);
            var rel = baseUri.MakeRelativeUri(fileUri).ToString();
            return Uri.UnescapeDataString(rel).Replace('/', Path.DirectorySeparatorChar);
        }

        // ====================== MÉTODOS PRINCIPALES ======================

        private void CargarGruposMusculares()
        {
            cmbMusculo.Items.Clear();
            cmbMusculo.Items.Add("Seleccione un grupo muscular");
            gruposMusculares.Clear();

            try
            {
                using (var conn = new SqlConnection(Conexion.Cadena))
                {
                    conn.Open();

                    string checkQuery =
                        "IF NOT EXISTS (SELECT 1 FROM Grupo_Muscular WHERE nombre = 'Cardio') INSERT INTO Grupo_Muscular (nombre) VALUES ('Cardio'); " +
                        "IF NOT EXISTS (SELECT 1 FROM Grupo_Muscular WHERE nombre = 'Abdominales') INSERT INTO Grupo_Muscular (nombre) VALUES ('Abdominales'); ";
                    using (var checkCmd = new SqlCommand(checkQuery, conn))
                        checkCmd.ExecuteNonQuery();

                    string query = "SELECT id_grupo_muscular, nombre FROM Grupo_Muscular ORDER BY nombre";
                    using (var cmd = new SqlCommand(query, conn))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int id = reader.GetInt32(0);
                            string nombre = reader.GetString(1);
                            gruposMusculares[nombre] = id;
                            cmbMusculo.Items.Add(nombre);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar los grupos musculares: " + ex.Message);
            }

            cmbMusculo.SelectedIndex = 0;
            cmbMusculo.ForeColor = Color.Gray;
            cmbMusculo.SelectedIndexChanged += (s, e) =>
            {
                cmbMusculo.ForeColor = (cmbMusculo.SelectedIndex == 0) ? Color.Gray : ColorTexto;
            };
        }

        private void btnSeleccionarImagen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.gif";
                if (ofd.ShowDialog() != DialogResult.OK) return;

                string sub = (cmbMusculo.SelectedIndex > 0) ? cmbMusculo.SelectedItem.ToString() : null;
                rutaRelativaSeleccionada = ImportToAssets(ofd.FileName, sub);
                txtImagen.Text = rutaRelativaSeleccionada;

                var abs = AbsPath(rutaRelativaSeleccionada);
                if (abs != null && File.Exists(abs))
                {
                    pictureBoxEjercicio.Image = LoadImageSafe(abs);
                    pictureBoxEjercicio.SizeMode = PictureBoxSizeMode.Zoom;
                }
            }
        }

        private void EstilizarGrilla()
        {
            var g = dgvEjercicios;

            g.BorderStyle = BorderStyle.None;
            g.BackgroundColor = Color.White;
            g.EnableHeadersVisualStyles = false;
            g.GridColor = ColorBorde;

            // Encabezados modernos
            g.ColumnHeadersDefaultCellStyle.BackColor = ColorPrimario;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            g.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            g.ColumnHeadersHeight = 40;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Filas
            g.DefaultCellStyle.BackColor = Color.White;
            g.DefaultCellStyle.ForeColor = ColorTexto;
            g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 244, 253);
            g.DefaultCellStyle.SelectionForeColor = ColorTexto;
            g.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);

            // Configuración general
            g.RowHeadersVisible = false;
            g.AllowUserToAddRows = false;
            g.AllowUserToResizeRows = false;
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.RowTemplate.Height = 70;

            // Tamaños de columnas
            if (g.Columns.Contains("ID")) g.Columns["ID"].FillWeight = 8;
            if (g.Columns.Contains("Ejercicio")) g.Columns["Ejercicio"].FillWeight = 40;
            if (g.Columns.Contains("GrupoMuscular")) g.Columns["GrupoMuscular"].FillWeight = 22;
            if (g.Columns.Contains("Estado")) g.Columns["Estado"].FillWeight = 15;
            if (g.Columns.Contains("Imagen"))
            {
                var c = (DataGridViewImageColumn)g.Columns["Imagen"];
                c.ImageLayout = DataGridViewImageCellLayout.Zoom;
                c.FillWeight = 15;
            }
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
                e.Handled = true;
        }

        private void CargarComboEstado()
        {
            cmbEstado.Items.Clear();
            cmbEstado.Items.Add("Todos");
            cmbEstado.Items.Add("Activos");
            cmbEstado.Items.Add("Inactivos");
            cmbEstado.SelectedIndex = 1;
        }

        private void RefrescarGrid()
        {
            bool? estado = null;
            if (cmbEstado.SelectedIndex == 1) estado = true;
            if (cmbEstado.SelectedIndex == 2) estado = false;

            listaActual = controller.ObtenerTodos(estado);
            CargarGridDesde(listaActual);
        }

        private void CargarGridDesde(List<Ejercicio> fuente)
        {
            dgvEjercicios.Columns.Clear();

            var tabla = new DataTable();
            tabla.Columns.Add("ID", typeof(int));
            tabla.Columns.Add("Ejercicio", typeof(string));
            tabla.Columns.Add("GrupoMuscular", typeof(string));
            tabla.Columns.Add("Estado", typeof(string));
            tabla.Columns.Add("Imagen", typeof(Image));

            foreach (var e in fuente)
            {
                Image img = null;
                try
                {
                    var abs = AbsPath(e.Imagen);
                    if (!string.IsNullOrWhiteSpace(abs) && File.Exists(abs))
                        img = GetThumbnail70(abs);
                }
                catch { img = null; }

                string estadoStr = e.Activo ? "Activo" : "Inactivo";
                tabla.Rows.Add(e.Id, e.Nombre, e.GrupoMuscularNombre, estadoStr, img);
            }

            dgvEjercicios.DataSource = tabla;
            dgvEjercicios.RowTemplate.Height = 70;

            // Ajustes de columnas
            if (dgvEjercicios.Columns.Contains("Estado"))
            {
                dgvEjercicios.Columns["Estado"].Width = 90;
                dgvEjercicios.Columns["Estado"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            if (dgvEjercicios.Columns.Contains("GrupoMuscular"))
            {
                dgvEjercicios.Columns["GrupoMuscular"].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            }

            var colImg = (DataGridViewImageColumn)dgvEjercicios.Columns["Imagen"];
            colImg.ImageLayout = DataGridViewImageCellLayout.Zoom;

            EstilizarGrilla();

            // Colorear estados
            foreach (DataGridViewRow row in dgvEjercicios.Rows)
            {
                if (row.Cells["Estado"].Value?.ToString() == "Inactivo")
                {
                    row.DefaultCellStyle.ForeColor = Color.Gray;
                    row.DefaultCellStyle.BackColor = Color.FromArgb(252, 252, 252);
                }
            }

            dgvEjercicios.ClearSelection();

            if (fuente == null || fuente.Count == 0)
                pictureBoxEjercicio.Image = null;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || txtNombre.ForeColor == Color.Gray)
            {
                MessageBox.Show("Debes ingresar un nombre para el ejercicio.",
                    "Campo incompleto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtNombre.Focus();
                return;
            }

            if (cmbMusculo.SelectedIndex <= 0)
            {
                MessageBox.Show("Debes seleccionar un grupo muscular antes de agregar el ejercicio.",
                    "Campo incompleto", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                cmbMusculo.Focus();
                return;
            }

            try
            {
                var nuevo = new Ejercicio
                {
                    Nombre = txtNombre.Text.Trim(),
                    GrupoMuscularId = gruposMusculares[cmbMusculo.SelectedItem.ToString()],
                    Imagen = string.IsNullOrWhiteSpace(txtImagen.Text) ? null : txtImagen.Text.Trim(),
                    CreadoPor = Sesion.Actual.IdUsuario
                };

                controller.Agregar(nuevo);
                RefrescarGrid();
                LimpiarCampos();

                MessageBox.Show("¡Ejercicio agregado correctamente!", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el ejercicio: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvEjercicios.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un ejercicio primero.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (cmbMusculo.SelectedIndex == 0)
            {
                MessageBox.Show("Debes seleccionar un grupo muscular válido.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                int id = Convert.ToInt32(dgvEjercicios.CurrentRow.Cells["ID"].Value);
                var ejercicio = listaActual.FirstOrDefault(x => x.Id == id);
                if (ejercicio == null) return;

                ejercicio.Nombre = txtNombre.Text.Trim();
                ejercicio.GrupoMuscularId = gruposMusculares[cmbMusculo.SelectedItem.ToString()];
                ejercicio.Imagen = string.IsNullOrWhiteSpace(txtImagen.Text) ? null : txtImagen.Text.Trim();

                controller.Editar(ejercicio);
                RefrescarGrid();
                LimpiarCampos();

                MessageBox.Show("¡Ejercicio editado correctamente!", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar el ejercicio: " + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvEjercicios.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un ejercicio primero.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            int id = Convert.ToInt32(dgvEjercicios.CurrentRow.Cells["ID"].Value);
            var ejercicio = listaActual.FirstOrDefault(x => x.Id == id);
            if (ejercicio == null) return;

            var confirm = MessageBox.Show($"¿Seguro que deseas eliminar '{ejercicio.Nombre}'?",
                "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                controller.Eliminar(ejercicio.Id);
                RefrescarGrid();
                LimpiarCampos();
                MessageBox.Show("¡Ejercicio eliminado correctamente!", "Éxito",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void dgvEjercicios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEjercicios.CurrentRow == null) return;

            int id = Convert.ToInt32(dgvEjercicios.CurrentRow.Cells["ID"].Value);
            var ejercicio = listaActual.FirstOrDefault(x => x.Id == id);
            if (ejercicio == null) return;

            txtNombre.Text = ejercicio.Nombre;
            txtImagen.Text = ejercicio.Imagen ?? "";

            var abs = AbsPath(ejercicio.Imagen);

            try
            {
                if (!string.IsNullOrWhiteSpace(abs) && File.Exists(abs))
                {
                    if (pictureBoxEjercicio.Image != null)
                    {
                        try
                        {
                            var temp = pictureBoxEjercicio.Image;
                            pictureBoxEjercicio.Image = null;
                            temp.Dispose();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine("⚠️ No se pudo liberar la imagen anterior: " + ex.Message);
                        }
                    }

                    pictureBoxEjercicio.Image = LoadImageSafe(abs);
                    pictureBoxEjercicio.SizeMode = PictureBoxSizeMode.Zoom;

                    if (pictureBoxEjercicio.Image == null ||
                        pictureBoxEjercicio.Image.Width == 0 ||
                        pictureBoxEjercicio.Image.Height == 0)
                    {
                        pictureBoxEjercicio.Image = null;
                        return;
                    }

                    if (ImageFormat.Gif.Equals(pictureBoxEjercicio.Image.RawFormat))
                    {
                        System.Drawing.ImageAnimator.Animate(pictureBoxEjercicio.Image, (s, e2) =>
                        {
                            pictureBoxEjercicio.Invalidate();
                        });
                    }
                }
                else
                {
                    pictureBoxEjercicio.Image = null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("⚠️ Error al mostrar la imagen: " + ex.Message);
                pictureBoxEjercicio.Image = null;
            }

            int idx = cmbMusculo.FindStringExact(ejercicio.GrupoMuscularNombre ?? "");
            cmbMusculo.SelectedIndex = (idx >= 0) ? idx : 0;

            txtNombre.ForeColor = ColorTexto;
            txtImagen.ForeColor = ColorTexto;
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtImagen.Text = "";
            cmbMusculo.SelectedIndex = 0;
            cmbMusculo.ForeColor = Color.Gray;
            pictureBoxEjercicio.Image = null;
            rutaRelativaSeleccionada = null;
            dgvEjercicios.ClearSelection();
            txtNombre.Focus();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string filtro = txtBuscar.Text.Trim().ToLower();
            string tipo = cmbTipoBusqueda.SelectedItem?.ToString() ?? "Todos";

            List<Ejercicio> filtrados;

            if (string.IsNullOrWhiteSpace(filtro))
            {
                filtrados = listaActual;
            }
            else
            {
                switch (tipo)
                {
                    case "ID":
                        filtrados = listaActual
                            .Where(x => x.Id.ToString().Contains(filtro))
                            .ToList();
                        break;

                    case "Nombre":
                        filtrados = listaActual
                            .Where(x => x.Nombre.ToLower().Contains(filtro))
                            .ToList();
                        break;

                    case "Grupo Muscular":
                        filtrados = listaActual
                            .Where(x => x.GrupoMuscularNombre.ToLower().Contains(filtro))
                            .ToList();
                        break;

                    default:
                        filtrados = listaActual
                            .Where(x =>
                                x.Id.ToString().Contains(filtro) ||
                                x.Nombre.ToLower().Contains(filtro) ||
                                x.GrupoMuscularNombre.ToLower().Contains(filtro))
                            .ToList();
                        break;
                }
            }

            CargarGridDesde(filtrados);

            if (filtrados.Count == 0)
            {
                pictureBoxEjercicio.Image = null;
            }
        }

        private void txtBuscar_KeyPress(object sender, KeyPressEventArgs e)
        {
            string tipo = cmbTipoBusqueda.SelectedItem?.ToString() ?? "Todos";

            if (tipo == "ID")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
                    e.Handled = true;
            }
            else if (tipo == "Nombre" || tipo == "Grupo Muscular")
            {
                if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
                    e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void AplicarPlaceholder(TextBox txt, string placeholder)
        {
            txt.ForeColor = Color.Gray;
            txt.Text = placeholder;

            txt.Enter += (s, e) =>
            {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = ColorTexto;
                }
            };

            txt.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = placeholder;
                    txt.ForeColor = Color.Gray;
                }
            };
        }

        private void dgvEjercicios_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            int id = Convert.ToInt32(dgvEjercicios.Rows[e.RowIndex].Cells["ID"].Value);
            var ej = listaActual.FirstOrDefault(x => x.Id == id);
            var abs = AbsPath(ej?.Imagen);

            try
            {
                if (pictureBoxEjercicio.Image != null)
                {
                    try
                    {
                        var temp = pictureBoxEjercicio.Image;
                        pictureBoxEjercicio.Image = null;
                        temp.Dispose();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("⚠️ No se pudo liberar la imagen anterior: " + ex.Message);
                    }
                }

                pictureBoxEjercicio.Image = (!string.IsNullOrWhiteSpace(abs) && File.Exists(abs))
                    ? LoadImageSafe(abs)
                    : null;

                pictureBoxEjercicio.SizeMode = PictureBoxSizeMode.Zoom;

                if (pictureBoxEjercicio.Image != null &&
                    ImageFormat.Gif.Equals(pictureBoxEjercicio.Image.RawFormat))
                {
                    System.Drawing.ImageAnimator.Animate(pictureBoxEjercicio.Image, (s, e2) =>
                    {
                        pictureBoxEjercicio.Invalidate();
                    });
                }
            }
            catch
            {
                pictureBoxEjercicio.Image = null;
            }
        }

        private void EstilizarBotones()
        {
            Button[] botones = { btnAgregar, btnEditar, btnEliminar, btnLimpiar, btnSeleccionarImagen };

            foreach (var b in botones)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                b.ForeColor = Color.White;
                b.Size = new Size(100, 35);
                b.Cursor = Cursors.Hand;

                // Efecto hover moderno
                b.MouseEnter += (s, e) => {
                    b.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(b.BackColor, 0.1f);
                };
                b.MouseLeave += (s, e) => {
                    b.FlatAppearance.MouseOverBackColor = b.BackColor;
                };

                // Bordes redondeados
                int radius = 6;
                b.Paint += (s, e) =>
                {
                    var rect = new Rectangle(0, 0, b.Width, b.Height);
                    using (var path = new GraphicsPath())
                    {
                        int d = radius * 2;
                        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                        path.CloseFigure();
                        b.Region = new Region(path);
                    }
                };
            }

            btnAgregar.BackColor = ColorExito;
            btnEditar.BackColor = ColorSecundario;
            btnEliminar.BackColor = ColorPeligro;
            btnLimpiar.BackColor = Color.FromArgb(149, 165, 166);
            btnSeleccionarImagen.BackColor = ColorPrimario;
        }

        private void cmbTipoBusqueda_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtBuscar.Text = "";
            txtBuscar.Focus();
        }

        private void dgvEjercicios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dgvEjercicios.Columns[e.ColumnIndex].Name == "Estado")
            {
                int id = Convert.ToInt32(dgvEjercicios.Rows[e.RowIndex].Cells["ID"].Value);
                var ejercicio = listaActual.FirstOrDefault(x => x.Id == id);

                if (ejercicio != null && !ejercicio.Activo)
                {
                    var confirmar = MessageBox.Show(
                        $"¿Deseas reactivar el ejercicio '{ejercicio.Nombre}'?",
                        "Confirmar reactivación",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question
                    );

                    if (confirmar == DialogResult.Yes)
                    {
                        controller.Reactivar(ejercicio.Id);
                        RefrescarGrid();
                    }
                }
            }
        }
    }
}