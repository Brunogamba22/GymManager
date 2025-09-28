using GymManager.Controllers;
using GymManager.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Views
{
    public partial class UcGestionEjercicios : UserControl
    {
        private EjercicioController controller = new EjercicioController();

        public UcGestionEjercicios()
        {
            InitializeComponent();
            ConfigurarComboMusculos();   //  combo con placeholder
            EstilizarBotones();          // colores de botones
            RefrescarGrid();
            AplicarPlaceholder(txtNombre, "Nombre del ejercicio");
            AplicarPlaceholder(txtDescripcion, "Descripción");
        }

        private void RefrescarGrid()
        {
            dgvEjercicios.DataSource = null;
            dgvEjercicios.DataSource = controller.ObtenerTodos();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || cmbMusculo.SelectedIndex == 0)
            {
                MessageBox.Show("Completa el nombre y selecciona un músculo válido.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var nuevo = new Ejercicio
            {
                Nombre = txtNombre.Text,
                Musculo = cmbMusculo.SelectedItem.ToString(),
                Descripcion = txtDescripcion.Text
            };

            controller.Agregar(nuevo);
            RefrescarGrid();
            LimpiarCampos();
            MessageBox.Show("¡Ejercicio agregado correctamente!", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Solo permite letras, teclas de control (ej: borrar) y espacios
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true; // Bloquea el ingreso
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
                MessageBox.Show("Debe seleccionar un músculo válido.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var ejercicio = (Ejercicio)dgvEjercicios.CurrentRow.DataBoundItem;
            ejercicio.Nombre = txtNombre.Text;
            ejercicio.Musculo = cmbMusculo.SelectedItem?.ToString();
            ejercicio.Descripcion = txtDescripcion.Text;

            controller.Editar(ejercicio);
            RefrescarGrid();
            LimpiarCampos();
            MessageBox.Show("¡Ejercicio editado correctamente!", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvEjercicios.CurrentRow == null)
            {
                MessageBox.Show("Selecciona un ejercicio primero.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var ejercicio = (Ejercicio)dgvEjercicios.CurrentRow.DataBoundItem;

            controller.Eliminar(ejercicio.Id);
            RefrescarGrid();
            LimpiarCampos();
            MessageBox.Show("¡Ejercicio eliminado correctamente!", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        private void dgvEjercicios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEjercicios.CurrentRow == null) return;

            var ejercicio = (Ejercicio)dgvEjercicios.CurrentRow.DataBoundItem;
            txtNombre.Text = ejercicio.Nombre;
            txtDescripcion.Text = ejercicio.Descripcion;

            int idx = cmbMusculo.FindStringExact(ejercicio.Musculo ?? "");
            cmbMusculo.SelectedIndex = (idx >= 0) ? idx : 0;

            txtNombre.ForeColor = Color.Black;
            txtDescripcion.ForeColor = Color.Black;
            cmbMusculo.ForeColor = (cmbMusculo.SelectedIndex == 0) ? Color.Gray : Color.Black;
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtDescripcion.Text = "";
            cmbMusculo.SelectedIndex = 0; // vuelve al placeholder
            cmbMusculo.ForeColor = Color.Gray;
            dgvEjercicios.ClearSelection();
            txtNombre.Focus();
        }

        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            var lista = controller.ObtenerTodos();
            var filtrados = lista.FindAll(x =>
                x.Nombre.IndexOf(txtBuscar.Text, StringComparison.OrdinalIgnoreCase) >= 0 ||
                x.Musculo.IndexOf(txtBuscar.Text, StringComparison.OrdinalIgnoreCase) >= 0);

            dgvEjercicios.DataSource = null;
            dgvEjercicios.DataSource = filtrados;
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

        //  Inicializa el combo con placeholder
        private void ConfigurarComboMusculos()
        {
            cmbMusculo.Items.Clear();
            cmbMusculo.Items.AddRange(new object[]
            {
                "Seleccione un músculo",
                "Pecho", "Espalda", "Hombros", "Brazos",
                "Cuádriceps", "Isquiotibiales", "Glúteos",
                "Pantorrillas", "Abdomen"
            });

            cmbMusculo.SelectedIndex = 0;
            cmbMusculo.ForeColor = Color.Gray;

            cmbMusculo.SelectedIndexChanged += (s, e) =>
            {
                cmbMusculo.ForeColor = (cmbMusculo.SelectedIndex == 0) ? Color.Gray : Color.Black;
            };
        }

        // Colores de los botones
        private void EstilizarBotones()
        {
            btnAgregar.FlatStyle = FlatStyle.Flat;
            btnAgregar.UseVisualStyleBackColor = false;
            btnAgregar.BackColor = Color.FromArgb(46, 204, 113); // verde
            btnAgregar.ForeColor = Color.White;

            btnEditar.FlatStyle = FlatStyle.Flat;
            btnEditar.UseVisualStyleBackColor = false;
            btnEditar.BackColor = Color.Gold; // amarillo
            btnEditar.ForeColor = Color.Black;

            btnEliminar.FlatStyle = FlatStyle.Flat;
            btnEliminar.UseVisualStyleBackColor = false;
            btnEliminar.BackColor = Color.FromArgb(231, 76, 60); // rojo
            btnEliminar.ForeColor = Color.White;

            btnLimpiar.FlatStyle = FlatStyle.Flat;
            btnLimpiar.UseVisualStyleBackColor = false;
            btnLimpiar.BackColor = Color.RoyalBlue; // azul
            btnLimpiar.ForeColor = Color.White;
        }
    }
}
