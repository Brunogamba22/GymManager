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
            ConfigurarComboMusculos();   // carga el combo de músculos con placeholder
            EstilizarBotones();          // aplica colores y estilos a los botones
            RefrescarGrid();             // llena la grilla con los ejercicios actuales

            // placeholders en campos de texto
            AplicarPlaceholder(txtNombre, "Nombre del ejercicio");
            AplicarPlaceholder(txtSeries, "Series");
            AplicarPlaceholder(txtRepeticiones, "Repeticiones");
            AplicarPlaceholder(txtDescanso, "Descanso (segundos)");
        }

        // Refresca la grilla con los datos actualizados
        private void RefrescarGrid()
        {
            dgvEjercicios.DataSource = null;
            dgvEjercicios.DataSource = controller.ObtenerTodos();
        }

        // Botón AGREGAR
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) || cmbMusculo.SelectedIndex == 0)
            {
                MessageBox.Show("Completa el nombre y selecciona un músculo válido.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Crea un nuevo objeto Ejercicio desde los campos del formulario
            var nuevo = new Ejercicio
            {
                Nombre = txtNombre.Text,
                Musculo = cmbMusculo.SelectedItem.ToString(),
                Series = int.Parse(txtSeries.Text),
                Repeticiones = txtRepeticiones.Text,  // queda como string
                Descanso = txtDescanso.Text
            };

            controller.Agregar(nuevo);
            RefrescarGrid();
            LimpiarCampos();
            MessageBox.Show("¡Ejercicio agregado correctamente!", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Validación: el nombre solo admite letras y espacios
        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true; // Bloquea el ingreso
            }
        }

        // Validación: series y repeticiones solo admiten números
        private void txtSeries_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Bloquea el ingreso
            }
        }
        private void txtRepeticiones_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true; // Bloquea el ingreso
            }
        }

        // Botón EDITAR
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
            ejercicio.Series = int.Parse(txtSeries.Text);
            ejercicio.Repeticiones = txtRepeticiones.Text;  // queda como string
            ejercicio.Descanso = txtDescanso.Text;

            controller.Editar(ejercicio);
            RefrescarGrid();
            LimpiarCampos();
            MessageBox.Show("¡Ejercicio editado correctamente!", "Éxito",
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Botón ELIMINAR (eliminación lógica en el controlador)
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

        // Botón LIMPIAR
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
        }

        // Cuando seleccionás una fila de la grilla, llena los campos de edición
        private void dgvEjercicios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEjercicios.CurrentRow == null) return;

            var ejercicio = (Ejercicio)dgvEjercicios.CurrentRow.DataBoundItem;
            txtNombre.Text = ejercicio.Nombre;
            txtSeries.Text = ejercicio.Series.ToString();
            txtRepeticiones.Text = ejercicio.Repeticiones.ToString();
            txtDescanso.Text = ejercicio.Descanso;

            int idx = cmbMusculo.FindStringExact(ejercicio.Musculo ?? "");
            cmbMusculo.SelectedIndex = (idx >= 0) ? idx : 0;

            txtNombre.ForeColor = Color.Black;
            txtSeries.ForeColor = Color.Black;
            txtRepeticiones.ForeColor = Color.Black;
            txtDescanso.ForeColor = Color.Black;
            cmbMusculo.ForeColor = (cmbMusculo.SelectedIndex == 0) ? Color.Gray : Color.Black;
        }

        // Limpia los campos
        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtSeries.Text = "";
            txtRepeticiones.Text = "";
            txtDescanso.Text = "";
            cmbMusculo.SelectedIndex = 0;
            cmbMusculo.ForeColor = Color.Gray;
            dgvEjercicios.ClearSelection();
            txtNombre.Focus();
        }

        // Búsqueda dinámica en la grilla
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            var lista = controller.ObtenerTodos();
            var filtrados = lista.FindAll(x =>
                x.Nombre.IndexOf(txtBuscar.Text, StringComparison.OrdinalIgnoreCase) >= 0 ||
                x.Musculo.IndexOf(txtBuscar.Text, StringComparison.OrdinalIgnoreCase) >= 0);

            dgvEjercicios.DataSource = null;
            dgvEjercicios.DataSource = filtrados;
        }

        // Método reutilizable para aplicar placeholder a TextBox
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

        // Carga el combo de músculos
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

        // Aplica colores y estilos a los botones
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
