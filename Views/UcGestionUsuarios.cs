using System;
using System.Windows.Forms;
using GymManager.Models;
using GymManager.Controllers;

namespace GymManager.Views
{
    public partial class UcGestionUsuarios : UserControl
    {
        private UsuarioController controller = new UsuarioController();

        // Ahora es string porque Id = dni
        private string idSeleccionado = null;

        public UcGestionUsuarios()
        {
            InitializeComponent();
            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = controller.ObtenerTodos();
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            cmbRol.SelectedIndex = -1;
            idSeleccionado = null;
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (txtNombre.Text == "" || txtApellido.Text == "" || txtEmail.Text == "" || txtPassword.Text == "" || cmbRol.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor completá todos los campos.");
                return;
            }

            var nuevoUsuario = new Usuario
            {
                Id = Guid.NewGuid().ToString().Substring(0, 8), // dni simulado si no tenés input
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Email = txtEmail.Text,
                Password = txtPassword.Text,
                Rol = (Rol)cmbRol.SelectedIndex
            };

            controller.Agregar(nuevoUsuario);
            MessageBox.Show("Usuario agregado correctamente.");

            LimpiarCampos();
            CargarUsuarios();
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == null)
            {
                MessageBox.Show("Seleccioná un usuario de la lista.");
                return;
            }

            var usuarioEditado = new Usuario
            {
                Id = idSeleccionado, // ahora string
                Nombre = txtNombre.Text,
                Apellido = txtApellido.Text,
                Email = txtEmail.Text,
                Password = txtPassword.Text,
                Rol = (Rol)cmbRol.SelectedIndex
            };

            controller.Editar(usuarioEditado);
            MessageBox.Show("Usuario editado correctamente.");

            LimpiarCampos();
            CargarUsuarios();
        }
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow?.DataBoundItem is Usuario uSel && uSel.Rol == Rol.Administrador)
            {
                MessageBox.Show("No se puede eliminar un usuario con rol Administrador.");
                return;
            }

            if (idSeleccionado == null)
            {
                MessageBox.Show("Seleccioná un usuario primero.");
                return;
            }

            controller.Eliminar(idSeleccionado);
            MessageBox.Show("Usuario eliminado.");
            LimpiarCampos();
            CargarUsuarios();
        }


        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null || dgvUsuarios.CurrentRow.DataBoundItem == null)
                return;

            var usuario = (Usuario)dgvUsuarios.CurrentRow.DataBoundItem;

            idSeleccionado = usuario.Id; // ahora string
            txtNombre.Text = usuario.Nombre;
            txtApellido.Text = usuario.Apellido;
            txtEmail.Text = usuario.Email;
            txtPassword.Text = usuario.Password;
            cmbRol.SelectedIndex = (int)usuario.Rol;

            // 🔒 No permitir eliminar administradores
            btnEliminar.Enabled = usuario.Rol != Rol.Administrador;
        }
    }
}
