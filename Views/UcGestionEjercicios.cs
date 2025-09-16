using GymManager.Controllers;
using GymManager.Models;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Views
{
    public partial class UcGestionEjercicios : UserControl
    {
        // Controlador que maneja la lógica de los ejercicios (alta, baja, modificación)
        private EjercicioController controller = new EjercicioController();

        // Constructor
        public UcGestionEjercicios()
        {
            InitializeComponent();
            RefrescarGrid(); // Carga inicial de los datos

            AplicarPlaceholder(txtNombre, "Nombre del ejercicio");
            AplicarPlaceholder(txtMusculo, "Músculo trabajado");
            AplicarPlaceholder(txtDescripcion, "Descripción");

        }

        // Refresca los datos del DataGridView con la lista actual de ejercicios
        private void RefrescarGrid()
        {
            dgvEjercicios.DataSource = null; // Limpia la grilla
            dgvEjercicios.DataSource = controller.ObtenerTodos(); // Carga nueva lista
        }

        // Botón para agregar un nuevo ejercicio
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Crea un nuevo ejercicio con los datos de los TextBox
            var nuevo = new Ejercicio
            {
                Nombre = txtNombre.Text,
                Musculo = txtMusculo.Text,
                Descripcion = txtDescripcion.Text
            };

            controller.Agregar(nuevo); // Lo envía al controlador
            RefrescarGrid();           // Refresca la grilla
            LimpiarCampos();           // Limpia los campos de entrada
        }

        // Botón para editar un ejercicio ya existente
        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Si no hay ninguna fila seleccionada, salir
            if (dgvEjercicios.CurrentRow == null) return;

            // Toma el ejercicio seleccionado en la grilla
            var ejercicio = (Ejercicio)dgvEjercicios.CurrentRow.DataBoundItem;

            // Actualiza los campos del ejercicio con los datos actuales
            ejercicio.Nombre = txtNombre.Text;
            ejercicio.Musculo = txtMusculo.Text;
            ejercicio.Descripcion = txtDescripcion.Text;

            controller.Editar(ejercicio); // Llama al controlador para actualizar
            RefrescarGrid();              // Refresca grilla
            LimpiarCampos();              // Limpia entrada
        }

        // Botón para eliminar un ejercicio seleccionado
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            // Si no hay fila seleccionada, salir
            if (dgvEjercicios.CurrentRow == null) return;

            // Toma el ejercicio seleccionado
            var ejercicio = (Ejercicio)dgvEjercicios.CurrentRow.DataBoundItem;

            controller.Eliminar(ejercicio.Id); // Lo elimina del controlador
            RefrescarGrid();                   // Refresca grilla
            LimpiarCampos();                   // Limpia campos
        }

        // Evento que se dispara cuando se selecciona una fila diferente
        private void dgvEjercicios_SelectionChanged(object sender, EventArgs e)
        {
            // Si no hay fila seleccionada, salir
            if (dgvEjercicios.CurrentRow == null) return;

            // Toma el ejercicio actual y muestra sus datos en los TextBox
            var ejercicio = (Ejercicio)dgvEjercicios.CurrentRow.DataBoundItem;

            txtNombre.Text = ejercicio.Nombre;
            txtMusculo.Text = ejercicio.Musculo;
            txtDescripcion.Text = ejercicio.Descripcion;
        }

        // Limpia los campos de texto del formulario
        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtMusculo.Text = "";
            txtDescripcion.Text = "";
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


    }
}


