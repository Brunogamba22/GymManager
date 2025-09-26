using GymManager.Controllers;
using GymManager.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Views
{
    /// <summary>
    /// Vista de gestión de ejercicios.
    /// Permite realizar operaciones CRUD (crear, leer, actualizar, eliminar) 
    /// sobre los ejercicios mediante el controlador correspondiente.
    /// </summary>
    public partial class UcGestionEjercicios : UserControl
    {
        // Controlador que maneja la lógica de negocio para los ejercicios.
        private EjercicioController controller = new EjercicioController();

        /// <summary>
        /// Constructor del UserControl.
        /// Inicializa componentes, refresca la grilla y aplica placeholders.
        /// </summary>
        public UcGestionEjercicios()
        {
            InitializeComponent();
            RefrescarGrid(); // Carga inicial de la lista de ejercicios.

            // Aplica texto por defecto en los campos de entrada.
            AplicarPlaceholder(txtNombre, "Nombre del ejercicio");
            AplicarPlaceholder(txtMusculo, "Músculo trabajado");
            AplicarPlaceholder(txtDescripcion, "Descripción");
        }

        /// <summary>
        /// Refresca los datos del DataGridView con la lista actual de ejercicios.
        /// </summary>
        private void RefrescarGrid()
        {
            dgvEjercicios.DataSource = null; // Limpia la grilla.
            dgvEjercicios.DataSource = controller.ObtenerTodos(); // Carga nueva lista desde el controlador.
        }

        /// <summary>
        /// Botón para agregar un nuevo ejercicio.
        /// Crea un objeto Ejercicio y lo envía al controlador.
        /// </summary>
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            var nuevo = new Ejercicio
            {
                Nombre = txtNombre.Text,
                Musculo = txtMusculo.Text,
                Descripcion = txtDescripcion.Text
            };

            controller.Agregar(nuevo); // Envía el nuevo ejercicio al controlador.
            RefrescarGrid();           // Actualiza la grilla.
            LimpiarCampos();           // Limpia los campos de entrada.
        }

        /// <summary>
        /// Botón para editar el ejercicio seleccionado.
        /// </summary>
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvEjercicios.CurrentRow == null) return; // Si no hay fila seleccionada, salir.

            // Obtiene el ejercicio actualmente seleccionado en la grilla.
            var ejercicio = (Ejercicio)dgvEjercicios.CurrentRow.DataBoundItem;

            // Actualiza sus propiedades con los datos ingresados en los TextBox.
            ejercicio.Nombre = txtNombre.Text;
            ejercicio.Musculo = txtMusculo.Text;
            ejercicio.Descripcion = txtDescripcion.Text;

            controller.Editar(ejercicio); // Llama al controlador para actualizar.
            RefrescarGrid();              // Refresca grilla.
            LimpiarCampos();              // Limpia entrada.
        }

        /// <summary>
        /// Botón para eliminar el ejercicio seleccionado.
        /// </summary>
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvEjercicios.CurrentRow == null) return; // Si no hay selección, salir.

            var ejercicio = (Ejercicio)dgvEjercicios.CurrentRow.DataBoundItem; // Obtiene el ejercicio.

            controller.Eliminar(ejercicio.Id); // Lo elimina por su Id.
            RefrescarGrid();                   // Refresca la grilla.
            LimpiarCampos();                   // Limpia campos.
        }

        /// <summary>
        /// Evento que se dispara al seleccionar una fila en la grilla.
        /// Carga los datos del ejercicio en los TextBox.
        /// </summary>
        private void dgvEjercicios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvEjercicios.CurrentRow == null) return;

            var ejercicio = (Ejercicio)dgvEjercicios.CurrentRow.DataBoundItem;

            txtNombre.Text = ejercicio.Nombre;
            txtMusculo.Text = ejercicio.Musculo;
            txtDescripcion.Text = ejercicio.Descripcion;
        }

        /// <summary>
        /// Limpia los campos de entrada (TextBox).
        /// </summary>
        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtMusculo.Text = "";
            txtDescripcion.Text = "";
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


        /// <summary>
        /// Aplica un texto placeholder a un TextBox
        /// (se muestra en gris cuando está vacío).
        /// </summary>
        private void AplicarPlaceholder(TextBox txt, string placeholder)
        {
            txt.ForeColor = Color.Gray;
            txt.Text = placeholder;

            // Evento al enfocar el campo.
            txt.Enter += (s, e) =>
            {
                if (txt.Text == placeholder)
                {
                    txt.Text = "";
                    txt.ForeColor = Color.Black;
                }
            };

            // Evento al salir del campo.
            txt.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txt.Text))
                {
                    txt.Text = placeholder;
                    txt.ForeColor = Color.Gray;
                }
            };
        }
    }
}
