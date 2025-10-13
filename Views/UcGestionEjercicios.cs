using GymManager.Controllers;
using GymManager.Models;
using GymManager.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;                    // <- para FirstOrDefault / Where
using System.Windows.Forms;
using System.IO;                      // <- File.Exists
using System.Drawing.Drawing2D;       // <- GraphicsPath

namespace GymManager.Views
{
    public partial class UcGestionEjercicios : UserControl
    {
        private readonly EjercicioController controller = new EjercicioController();
        private readonly Dictionary<string, int> gruposMusculares = new Dictionary<string, int>();
        private string imagenSeleccionada = null;

        // Respaldo de los objetos reales para mapear con la grilla (que usa DataTable)
        private List<Ejercicio> listaActual = new List<Ejercicio>();

        public UcGestionEjercicios()
        {
            InitializeComponent();
            CargarGruposMusculares();
            EstilizarBotones();
            RefrescarGrid();

            AplicarPlaceholder(txtNombre, "Nombre del ejercicio");
            AplicarPlaceholder(txtImagen, "Ruta o nombre del archivo de imagen");
        }

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

                    // Asegura que existan "Cardio" y "Abdominales"
                    string checkQuery =
                        "IF NOT EXISTS (SELECT 1 FROM Grupo_Muscular WHERE nombre IN ('Cardio','Abdominales')) " +
                        "BEGIN " +
                        "IF NOT EXISTS (SELECT 1 FROM Grupo_Muscular WHERE nombre = 'Cardio') INSERT INTO Grupo_Muscular (nombre) VALUES ('Cardio'); " +
                        "IF NOT EXISTS (SELECT 1 FROM Grupo_Muscular WHERE nombre = 'Abdominales') INSERT INTO Grupo_Muscular (nombre) VALUES ('Abdominales'); " +
                        "END";
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
        // Seleccionar imagen desde el explorador
        // ------------------------------------------------------------
        private void btnSeleccionarImagen_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Archivos de imagen|*.jpg;*.jpeg;*.png;*.gif";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    imagenSeleccionada = ofd.FileName;
                    txtImagen.Text = imagenSeleccionada;
                    pictureBoxEjercicio.Image = Image.FromFile(imagenSeleccionada);
                    pictureBoxEjercicio.SizeMode = PictureBoxSizeMode.Zoom;
                }
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
            listaActual = controller.ObtenerTodos();   // lista real de modelos
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
                    if (!string.IsNullOrWhiteSpace(e.Imagen) && File.Exists(e.Imagen))
                    {
                        using (var temp = Image.FromFile(e.Imagen))
                            img = new Bitmap(temp, new Size(70, 70));
                    }
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
                    Imagen = (imagenSeleccionada ?? txtImagen.Text)?.Trim(),
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
                ejercicio.Imagen = (imagenSeleccionada ?? txtImagen.Text)?.Trim();

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
        // Handler del botón Limpiar
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

            if (!string.IsNullOrWhiteSpace(ejercicio.Imagen) && File.Exists(ejercicio.Imagen))
            {
                try
                {
                    pictureBoxEjercicio.Image = Image.FromFile(ejercicio.Imagen);
                    pictureBoxEjercicio.SizeMode = PictureBoxSizeMode.Zoom;
                }
                catch { pictureBoxEjercicio.Image = null; }
            }
            else pictureBoxEjercicio.Image = null;

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
            imagenSeleccionada = null;
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
