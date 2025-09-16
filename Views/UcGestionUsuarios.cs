using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using GymManager.Models;
using GymManager.Controllers;

namespace GymManager.Views
{
    public partial class UcGestionUsuarios : UserControl
    {
        // Instancia del controlador de usuarios
        private UsuarioController controller = new UsuarioController();

        // Variable auxiliar para guardar el ID del usuario seleccionado
        private int? idSeleccionado = null;

        // Constructor del UserControl
        public UcGestionUsuarios()
        {
            InitializeComponent();  // Carga el diseño

            CargarUsuarios();       // Muestra los usuarios en la tabla al iniciar
        }

        //  Método para actualizar el DataGridView con los datos actuales
        private void CargarUsuarios()
        {
            dgvUsuarios.DataSource = null;                            // Limpia el origen anterior
            dgvUsuarios.DataSource = controller.ObtenerTodos();      // Asigna la nueva lista
        }

        //  Limpia los campos del formulario
        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            cmbRol.SelectedIndex = -1;
            idSeleccionado = null;
        }

        //  Evento: Al hacer clic en "Agregar"
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Validación básica
            if (txtNombre.Text == "" || txtEmail.Text == "" || txtPassword.Text == "" || cmbRol.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor completá todos los campos.");
                return;
            }

            // Crear un nuevo usuario
            var nuevoUsuario = new Usuario
            {
                Nombre = txtNombre.Text,
                Email = txtEmail.Text,
                Password = txtPassword.Text,
                Rol = (Rol)cmbRol.SelectedIndex  // Convierte índice en enum
            };

            controller.Agregar(nuevoUsuario);     // Agrega a la "base"
            MessageBox.Show("Usuario agregado correctamente.");

            LimpiarCampos();
            CargarUsuarios(); // Refresca el DataGridView
        }

        // 📌 Evento: Al hacer clic en "Editar"
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == null)
            {
                MessageBox.Show("Seleccioná un usuario de la lista.");
                return;
            }

            var usuarioEditado = new Usuario
            {
                Id = idSeleccionado.Value,
                Nombre = txtNombre.Text,
                Email = txtEmail.Text,
                Password = txtPassword.Text,
                Rol = (Rol)cmbRol.SelectedIndex
            };

            controller.Editar(usuarioEditado);   // Edita el usuario en memoria
            MessageBox.Show("Usuario editado correctamente.");

            LimpiarCampos();
            CargarUsuarios();
        }

        //  Evento: Al hacer clic en "Eliminar"
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == null)
            {
                MessageBox.Show("Seleccioná un usuario primero.");
                return;
            }

            controller.Eliminar(idSeleccionado.Value);
            MessageBox.Show("Usuario eliminado.");

            LimpiarCampos();
            CargarUsuarios();
        }

        // 🖱 Evento: Cuando cambia la fila seleccionada en el DataGridView
        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null || dgvUsuarios.CurrentRow.DataBoundItem == null)
                return;

            // Convierte la fila seleccionada en un objeto Usuario
            var usuario = (Usuario)dgvUsuarios.CurrentRow.DataBoundItem;

            // Muestra los datos en los campos
            idSeleccionado = usuario.Id;
            txtNombre.Text = usuario.Nombre;
            txtEmail.Text = usuario.Email;
            txtPassword.Text = usuario.Password;
            cmbRol.SelectedIndex = (int)usuario.Rol;
        }
    }
}
