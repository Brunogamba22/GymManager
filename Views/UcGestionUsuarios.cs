using GymManager.Controllers;
using GymManager.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

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
            ConfigurarPlaceholder();
        }

        private List<Usuario> usuariosCache;

        private void CargarUsuarios()
        {
            usuariosCache = controller.ObtenerTodos(); // guardamos en memoria
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = usuariosCache; //  usamos la lista cacheada


            // Ocultar columna Password si existe
            if (dgvUsuarios.Columns["Password"] != null)
                dgvUsuarios.Columns["Password"].Visible = false;
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

            try
            {
                controller.Agregar(nuevoUsuario);
                MessageBox.Show("Usuario agregado correctamente.");
                LimpiarCampos();
                CargarUsuarios();
            }
            catch (InvalidOperationException ex) // cuando ya existe
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex) // cualquier otro error inesperado
            {
                MessageBox.Show("Ocurrió un error al agregar el usuario: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

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

            try
            {
                controller.Editar(usuarioEditado);
                MessageBox.Show("Usuario editado correctamente.");
                LimpiarCampos();
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al editar: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

            try
            {
                controller.Eliminar(idSeleccionado);
                MessageBox.Show("Usuario eliminado.");
                LimpiarCampos();
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al eliminar: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            LimpiarCampos();
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

            //  No permitir eliminar administradores
            btnEliminar.Enabled = usuario.Rol != Rol.Administrador;
        }

        //buscador
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string q = txtBuscar.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(q) || q == "nombre usuario") //  ignorar placeholder
            {
                dgvUsuarios.DataSource = usuariosCache;
            }
            else
            {
                var filtrados = usuariosCache.FindAll(u =>
                 (u.Nombre != null && u.Nombre.ToLower().Contains(q)) ||
                 (u.Apellido != null && u.Apellido.ToLower().Contains(q)) ||
                 (u.Email != null && u.Email.ToLower().Contains(q))
                 );

                dgvUsuarios.DataSource = filtrados;
            }

            if (dgvUsuarios.Columns["Password"] != null)
                dgvUsuarios.Columns["Password"].Visible = false;
        }




        private void ConfigurarPlaceholder()
        {
            txtBuscar.ForeColor = Color.Gray;
            txtBuscar.Text = "Nombre usuario";

            txtBuscar.Enter += (s, e) =>
            {
                if (txtBuscar.Text == "Nombre usuario")
                {
                    txtBuscar.Text = "";
                    txtBuscar.ForeColor = Color.Black;
                }
            };

            txtBuscar.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtBuscar.Text))
                {
                    txtBuscar.Text = "Nombre usuario";
                    txtBuscar.ForeColor = Color.Gray;
                }
            };
        }


    }
}
