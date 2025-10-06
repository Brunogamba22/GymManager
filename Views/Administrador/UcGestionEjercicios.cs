using GymManager.Controllers;
using GymManager.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Drawing;
using System.Windows.Forms;
using GymManager.Utils;

namespace GymManager.Views
{
    public partial class UcGestionEjercicios : UserControl
    {
        private EjercicioController controller = new EjercicioController();

        // Lista auxiliar para vincular el ComboBox de grupos musculares
        private Dictionary<string, int> gruposMusculares = new Dictionary<string, int>();

        public UcGestionEjercicios()
        {
            InitializeComponent();
            CargarGruposMusculares();    // carga el combo con los grupos desde la BD
            EstilizarBotones();          // aplica colores y estilos
            RefrescarGrid();             // llena la grilla

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



        // Validación: el nombre solo admite letras y espacios
        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Evita números o símbolos
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true; // bloquea el ingreso de caracteres inválidos
            }
        }


        // ------------------------------------------------------------
        // Refresca la grilla con los datos actualizados
        // ------------------------------------------------------------
        private void RefrescarGrid()
        {
            dgvEjercicios.DataSource = null;
            dgvEjercicios.DataSource = controller.ObtenerTodos();

            // Ajusta nombres de columnas
            dgvEjercicios.Columns["Id"].HeaderText = "ID";
            dgvEjercicios.Columns["Nombre"].HeaderText = "Ejercicio";
            dgvEjercicios.Columns["GrupoMuscularNombre"].HeaderText = "Grupo Muscular";
            dgvEjercicios.Columns["Imagen"].HeaderText = "Imagen";
            dgvEjercicios.Columns["CreadoPor"].HeaderText = "ID Creador";

            dgvEjercicios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvEjercicios.ClearSelection();
        }

        // ------------------------------------------------------------
        // Botón AGREGAR
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
                    Imagen = string.IsNullOrWhiteSpace(txtImagen.Text) ? null : txtImagen.Text.Trim(),
                    CreadoPor = Sesion.Actual.IdUsuario // 👈 tomamos el usuario logueado
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
        // Botón EDITAR
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
                var ejercicio = (Ejercicio)dgvEjercicios.CurrentRow.DataBoundItem;
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

        // ------------------------------------------------------------
        // Botón ELIMINAR
        // ------------------------------------------------------------
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvEjercicios.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un ejercicio primero.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var ejercicio = (Ejercicio)dgvEjercicios.CurrentRow.DataBoundItem;

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

        // ------------------------------------------------------------
        // Botón LIMPIAR
        // ------------------------------------------------------------
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        // ------------------------------------------------------------
        // Rellena campos al seleccionar una fila
        // ------------------------------------------------------------
        private void dgvEjercicios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEjercicios.CurrentRow == null) return;

            var ejercicio = (Ejercicio)dgvEjercicios.CurrentRow.DataBoundItem;
            txtNombre.Text = ejercicio.Nombre;
            txtImagen.Text = ejercicio.Imagen ?? "";

            int idx = cmbMusculo.FindStringExact(ejercicio.GrupoMuscularNombre ?? "");
            cmbMusculo.SelectedIndex = (idx >= 0) ? idx : 0;

            txtNombre.ForeColor = Color.Black;
            txtImagen.ForeColor = Color.Black;
        }

        // ------------------------------------------------------------
        // Limpia los campos del formulario
        // ------------------------------------------------------------
        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtImagen.Text = "";
            cmbMusculo.SelectedIndex = 0;
            cmbMusculo.ForeColor = Color.Gray;
            dgvEjercicios.ClearSelection();
            txtNombre.Focus();
        }

        // ------------------------------------------------------------
        // Búsqueda dinámica
        // ------------------------------------------------------------
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            var lista = controller.ObtenerTodos();
            var filtro = txtBuscar.Text.ToLower();

            var filtrados = lista.FindAll(x =>
                x.Nombre.ToLower().Contains(filtro) ||
                x.GrupoMuscularNombre.ToLower().Contains(filtro));

            dgvEjercicios.DataSource = null;
            dgvEjercicios.DataSource = filtrados;
        }

        // ------------------------------------------------------------
        // Placeholder visual para campos de texto
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
        // Estiliza los botones
        // ------------------------------------------------------------
        private void EstilizarBotones()
        {
            Button[] botones = { btnAgregar, btnEditar, btnEliminar, btnLimpiar };
            foreach (var b in botones)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.UseVisualStyleBackColor = false;
            }

            btnAgregar.BackColor = Color.FromArgb(46, 204, 113); // Verde
            btnAgregar.ForeColor = Color.White;

            btnEditar.BackColor = Color.Gold;
            btnEditar.ForeColor = Color.Black;

            btnEliminar.BackColor = Color.FromArgb(231, 76, 60); // Rojo
            btnEliminar.ForeColor = Color.White;

            btnLimpiar.BackColor = Color.RoyalBlue; // Azul
            btnLimpiar.ForeColor = Color.White;
        }
    }
}
