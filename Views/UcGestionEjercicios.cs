using GymManager.Controllers;
using GymManager.Models;
using GymManager.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;                    // FirstOrDefault / Where
using System.Windows.Forms;
using System.IO;                      // File / Path
using System.Drawing.Drawing2D;       // GraphicsPath
using System.Drawing.Imaging;         // FrameDimension (GIF)

namespace GymManager.Views
{
    public partial class UcGestionEjercicios : UserControl
    {
        private readonly EjercicioController controller = new EjercicioController();
        private readonly Dictionary<string, int> gruposMusculares = new Dictionary<string, int>();

        // Guardamos el ÚLTIMO archivo elegido ya copiado dentro del proyecto, en RUTA RELATIVA (lo que se guarda en BD)
        private string rutaRelativaSeleccionada = null;

        // Respaldo de los objetos reales (la grilla usa DataTable)
        private List<Ejercicio> listaActual = new List<Ejercicio>();

        // ====== Rutas base dentro del proyecto ======
        // En tiempo de ejecución, la base es: <carpeta_del_exe>\Resources\Ejercicios
        // Ej.: ...\GymManager\bin\Debug\net48\Resources\Ejercicios
        private static readonly string AssetsRoot =
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources", "Ejercicios");

        public UcGestionEjercicios()
        {
            InitializeComponent();
            CargarGruposMusculares();
            EstilizarBotones();
            RefrescarGrid();

            AplicarPlaceholder(txtNombre, "Nombre del ejercicio");
            AplicarPlaceholder(txtImagen, "Ruta relativa (p.ej. Pecho/press_banca.gif)");
        }

        // ====================== HELPERS DE RUTAS/IMÁGENES ======================

        /// <summary>
        /// Convierte una ruta RELATIVA (guardada en BD) a ABSOLUTA dentro del proyecto.
        /// Si relative es null/empty, devuelve null.
        /// </summary>
        private static string AbsPath(string relative) =>
            string.IsNullOrWhiteSpace(relative) ? null : Path.Combine(AssetsRoot, relative);

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            // 🔁 Actualiza los frames de todos los GIF activos
            if (pictureBoxEjercicio.Image != null &&
                ImageFormat.Gif.Equals(pictureBoxEjercicio.Image.RawFormat))
            {
                System.Drawing.ImageAnimator.UpdateFrames(pictureBoxEjercicio.Image);
            }
        }


        /// <summary>
        /// Copia el archivo "sourceFullPath" a Resources/Ejercicios/[subfolder opcional],
        /// evitando sobrescribir (agrega sufijo _1, _2, ...), y devuelve la RUTA RELATIVA
        /// que es la que se guarda en la base.
        /// </summary>
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

            // Ruta relativa desde AssetsRoot (esto va a BD)
            return GetRelativePath(AssetsRoot, target);
        }

        // ------------------------------------------------------------
        // Carga una imagen (o GIF animado) y la prepara para ser mostrada
        // sin bloquear el archivo y permitiendo animación.
        // ------------------------------------------------------------
        private static Image LoadImageSafe(string absPath)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(absPath) || !File.Exists(absPath))
                    return null;

                // ⚡ Copiamos el archivo a un temporal (para evitar lock en el original)
                string tempFile = Path.GetTempFileName();
                File.Copy(absPath, tempFile, true);

                // ⚡ Cargamos la imagen desde el archivo temporal (no desde stream)
                Image img = Image.FromFile(tempFile);

                // Si es GIF, permitimos animación
                if (ImageFormat.Gif.Equals(img.RawFormat))
                {
                    System.Drawing.ImageAnimator.Animate(img, (s, e) => { });
                }

                return img; // se mantiene animado y estable
            }
            catch
            {
                return null;
            }
        }


        /// <summary>
        /// Devuelve una miniatura 70x70. Si es GIF animado, toma el primer frame.
        /// </summary>
        private static Image GetThumbnail70(string absPath)
        {
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
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;

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


        // === Helpers para ruta relativa en .NET Framework ===
        private static string EnsureDirSep(string path)
        {
            if (string.IsNullOrEmpty(path)) return path;
            char sep = Path.DirectorySeparatorChar;
            return path.EndsWith(sep.ToString()) ? path : path + sep;
        }

        private static string GetRelativePath(string basePath, string fullPath)
        {
            // Usa URI para calcular ruta relativa (compatible .NET Framework)
            var baseUri = new Uri(EnsureDirSep(basePath));
            var fileUri = new Uri(fullPath);
            var rel = baseUri.MakeRelativeUri(fileUri).ToString();   // usa '/'
            return Uri.UnescapeDataString(rel).Replace('/', Path.DirectorySeparatorChar);
        }


        // =================== FIN HELPERS ===================

        // ------------------------------------------------------------
        // Carga la lista de grupos musculares desde la base de datos
        // ------------------------------------------------------------
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

                    // Pequeña semilla defensiva por si faltan
                    string checkQuery =
                        "IF NOT EXISTS (SELECT 1 FROM Grupo_Muscular WHERE nombre = 'Cardio') INSERT INTO Grupo_Muscular (nombre) VALUES ('Cardio'); " +
                        "IF NOT EXISTS (SELECT 1 FROM Grupo_Muscular WHERE nombre = 'Abdominales') INSERT INTO Grupo_Muscular (nombre) VALUES ('Abdominales'); ";
                    using (var checkCmd = new SqlCommand(checkQuery, conn))
                        checkCmd.ExecuteNonQuery();

                    // Carga todos los grupos
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
                cmbMusculo.ForeColor = (cmbMusculo.SelectedIndex == 0) ? Color.Gray : Color.Black;
            };
        }

        // ------------------------------------------------------------
        // Seleccionar imagen: copia al proyecto y guarda RUTA RELATIVA
        // ------------------------------------------------------------
        private void btnSeleccionarImagen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Imágenes|*.jpg;*.jpeg;*.png;*.gif";
                if (ofd.ShowDialog() != DialogResult.OK) return;

                // Subcarpeta opcional: el grupo muscular elegido (si hay)
                string sub = (cmbMusculo.SelectedIndex > 0) ? cmbMusculo.SelectedItem.ToString() : null;

                // Copiamos al repo y obtenemos ruta RELATIVA (lo que se guarda en BD)
                rutaRelativaSeleccionada = ImportToAssets(ofd.FileName, sub);

                // Mostramos la relativa en el textbox (para que quede claro)
                txtImagen.Text = rutaRelativaSeleccionada;

                // Preview (GIF se anima)
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
            g.GridColor = Color.Gainsboro;

            // Encabezados
            g.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(245, 245, 245);
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.Black;
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            g.ColumnHeadersHeight = 36;

            // Filas
            g.DefaultCellStyle.BackColor = Color.White;
            g.DefaultCellStyle.ForeColor = Color.Black;
            g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 244, 253);
            g.DefaultCellStyle.SelectionForeColor = Color.Black;
            g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);

            // Sin row headers y sin fila “*”
            g.RowHeadersVisible = false;
            g.AllowUserToAddRows = false;
            g.AllowUserToResizeRows = false;

            // Autoajustes
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.RowTemplate.Height = 80;

            // Tamaños mínimos agradables
            if (g.Columns.Contains("ID")) g.Columns["ID"].FillWeight = 8;
            if (g.Columns.Contains("Ejercicio")) g.Columns["Ejercicio"].FillWeight = 40;
            if (g.Columns.Contains("GrupoMuscular")) g.Columns["GrupoMuscular"].FillWeight = 22;
            if (g.Columns.Contains("Imagen"))
            {
                var c = (DataGridViewImageColumn)g.Columns["Imagen"];
                c.ImageLayout = DataGridViewImageCellLayout.Zoom;
                c.FillWeight = 30;
            }
        }


        // Solo letras en nombre
        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
                e.Handled = true;
        }

        // ------------------------------------------------------------
        // Refrescar DataGridView (carga lista real y pinta tabla)
        // ------------------------------------------------------------
        private void RefrescarGrid()
        {
            listaActual = controller.ObtenerTodos();   // lista real de modelos (Imagen = RUTA RELATIVA)
            CargarGridDesde(listaActual);              // pinta la grilla con miniaturas
        }

        // Pinta la grilla desde una lista de Ejercicio
        private void CargarGridDesde(List<Ejercicio> fuente)
        {
            dgvEjercicios.Columns.Clear();

            var tabla = new DataTable();
            tabla.Columns.Add("ID", typeof(int));
            tabla.Columns.Add("Ejercicio", typeof(string));
            tabla.Columns.Add("GrupoMuscular", typeof(string));
            tabla.Columns.Add("Imagen", typeof(Image));   // miniatura

            foreach (var e in fuente)
            {
                Image img = null;
                try
                {
                    // e.Imagen es RELATIVA -> la paso a ABSOLUTA para leer el archivo
                    var abs = AbsPath(e.Imagen);
                    if (!string.IsNullOrWhiteSpace(abs) && File.Exists(abs))
                        img = GetThumbnail70(abs);
                }
                catch { img = null; }

                tabla.Rows.Add(e.Id, e.Nombre, e.GrupoMuscularNombre, img);
            }

            dgvEjercicios.DataSource = tabla;

            // Ajustes visuales
            dgvEjercicios.RowTemplate.Height = 80;
            dgvEjercicios.Columns["ID"].Width = 50;
            dgvEjercicios.Columns["Ejercicio"].Width = 220;
            dgvEjercicios.Columns["GrupoMuscular"].Width = 150;

            var colImg = (DataGridViewImageColumn)dgvEjercicios.Columns["Imagen"];
            colImg.ImageLayout = DataGridViewImageCellLayout.Zoom;

            EstilizarGrilla();

            dgvEjercicios.ClearSelection();

        }

        // ------------------------------------------------------------
        // Agregar
        // ------------------------------------------------------------
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || cmbMusculo.SelectedIndex == 0)
            {
                MessageBox.Show("Completa el nombre y selecciona un grupo muscular válido.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                var nuevo = new Ejercicio
                {
                    Nombre = txtNombre.Text.Trim(),
                    GrupoMuscularId = gruposMusculares[cmbMusculo.SelectedItem.ToString()],
                    // Guardamos SIEMPRE ruta RELATIVA (si viene vacía, queda null)
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

        // ------------------------------------------------------------
        // Editar (usa ID de la fila seleccionada y busca en listaActual)
        // ------------------------------------------------------------
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
                // Si el usuario eligió nueva imagen, ya está en txtImagen como RUTA RELATIVA
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

        // ------------------------------------------------------------
        // Eliminar (usa ID de la fila seleccionada)
        // ------------------------------------------------------------
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

        // Botón Limpiar
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        // ------------------------------------------------------------
        // Selección en la grilla (usa ID -> objeto real)
        // ------------------------------------------------------------
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
                    // 🔸 Liberar la imagen anterior
                    if (pictureBoxEjercicio.Image != null)
                    {
                        pictureBoxEjercicio.Image.Dispose();
                        pictureBoxEjercicio.Image = null;
                    }

                    // 🔸 Cargar y mostrar imagen
                    pictureBoxEjercicio.Image = LoadImageSafe(abs);
                    pictureBoxEjercicio.SizeMode = PictureBoxSizeMode.Zoom;

                    // 🌀 Si es GIF, animarlo
                    if (pictureBoxEjercicio.Image != null &&
                        ImageFormat.Gif.Equals(pictureBoxEjercicio.Image.RawFormat))
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
            catch
            {
                pictureBoxEjercicio.Image = null;
            }

            // Actualizar selección del combo
            int idx = cmbMusculo.FindStringExact(ejercicio.GrupoMuscularNombre ?? "");
            cmbMusculo.SelectedIndex = (idx >= 0) ? idx : 0;

            txtNombre.ForeColor = Color.Black;
            txtImagen.ForeColor = Color.Black;
        }


        // ------------------------------------------------------------
        // Limpiar campos
        // ------------------------------------------------------------
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

        // ------------------------------------------------------------
        // Búsqueda dinámica (filtra lista real y repinta grilla)
        // ------------------------------------------------------------
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            var filtro = txtBuscar.Text.ToLower();
            var filtrados = listaActual
                .Where(x =>
                    x.Nombre.ToLower().Contains(filtro) ||
                    x.GrupoMuscularNombre.ToLower().Contains(filtro))
                .ToList();

            CargarGridDesde(filtrados);
        }

        // ------------------------------------------------------------
        // Placeholder visual
        // ------------------------------------------------------------
        private void AplicarPlaceholder(TextBox txt, string placeholder)
        {
            txt.ForeColor = Color.Gray;
            txt.Text = placeholder;

            txt.Enter += (s, e) =>
            {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
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
                // 🔸 Liberar la imagen anterior para evitar bloqueos
                if (pictureBoxEjercicio.Image != null)
                {
                    pictureBoxEjercicio.Image.Dispose();
                    pictureBoxEjercicio.Image = null;
                }

                // 🔸 Cargar la nueva imagen (puede ser GIF)
                pictureBoxEjercicio.Image = (!string.IsNullOrWhiteSpace(abs) && File.Exists(abs))
                    ? LoadImageSafe(abs)
                    : null;

                pictureBoxEjercicio.SizeMode = PictureBoxSizeMode.Zoom;

                // 🌀 Si la imagen es GIF, activamos animación segura
                if (pictureBoxEjercicio.Image != null &&
                    ImageFormat.Gif.Equals(pictureBoxEjercicio.Image.RawFormat))
                {
                    System.Drawing.ImageAnimator.Animate(pictureBoxEjercicio.Image, (s, e2) =>
                    {
                        pictureBoxEjercicio.Invalidate(); // redibuja el frame actual
                    });
                }
            }
            catch
            {
                pictureBoxEjercicio.Image = null;
            }
        }


        // ------------------------------------------------------------
        // Estilos de botones (compactos y sin APIs raras)
        // ------------------------------------------------------------
        private void EstilizarBotones()
        {
            Button[] botones = { btnAgregar, btnEditar, btnEliminar, btnLimpiar, btnSeleccionarImagen };

            foreach (var b in botones)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.FlatAppearance.BorderSize = 0;
                b.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                b.ForeColor = Color.White;
                b.Size = new Size(95, 32);

                // Bordes redondeados suaves
                int radius = 8;
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

            btnAgregar.BackColor = Color.FromArgb(46, 204, 113);
            btnEditar.BackColor = Color.FromArgb(241, 196, 15);
            btnEliminar.BackColor = Color.FromArgb(231, 76, 60);
            btnLimpiar.BackColor = Color.FromArgb(52, 152, 219);
            btnSeleccionarImagen.BackColor = Color.FromArgb(155, 89, 182);

        }
    }
}
