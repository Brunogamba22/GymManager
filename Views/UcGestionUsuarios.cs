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

        // Dni del usuario seleccionado (clave lógica)
        private string dniSeleccionado = null;

        public UcGestionUsuarios()
        {
            InitializeComponent();
            CargarUsuarios();
            FormatearGrid();
            ConfigurarPlaceholder();
            EstilizarBotones();
        }

        private List<Usuario> usuariosCache;

        private void CargarUsuarios()
        {
            usuariosCache = controller.ObtenerTodos();
            dgvUsuarios.DataSource = null;
            dgvUsuarios.DataSource = usuariosCache;

            if (dgvUsuarios.Columns["Password"] != null)
                dgvUsuarios.Columns["Password"].Visible = false;
        }

        private void FormatearGrid()
        {
            if (dgvUsuarios.Columns["IdUsuario"] != null)
                dgvUsuarios.Columns["IdUsuario"].HeaderText = "ID Usuario";
            if (dgvUsuarios.Columns["Dni"] != null)
                dgvUsuarios.Columns["Dni"].HeaderText = "DNI";
            if (dgvUsuarios.Columns["Nombre"] != null)
                dgvUsuarios.Columns["Nombre"].HeaderText = "Nombre";
            if (dgvUsuarios.Columns["Apellido"] != null)
                dgvUsuarios.Columns["Apellido"].HeaderText = "Apellido";
            if (dgvUsuarios.Columns["Email"] != null)
                dgvUsuarios.Columns["Email"].HeaderText = "Correo Electrónico";
            if (dgvUsuarios.Columns["Rol"] != null)
                dgvUsuarios.Columns["Rol"].HeaderText = "Rol";

            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            cmbRol.SelectedIndex = -1;
            dniSeleccionado = null;
        }

        // ------------------------------------------------------------
        // AGREGAR USUARIO
        // ------------------------------------------------------------
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                cmbRol.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor completá todos los campos.");
                return;
            }

            var nuevoUsuario = new Usuario
            {
                Dni = Guid.NewGuid().ToString().Substring(0, 8), // DNI simulado
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                Rol = (Rol)cmbRol.SelectedIndex
            };

            try
            {
                controller.Agregar(nuevoUsuario);
                MessageBox.Show("Usuario agregado correctamente.");
                LimpiarCampos();
                CargarUsuarios();
            }
            catch (InvalidOperationException ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error al agregar el usuario: " + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ------------------------------------------------------------
        // EDITAR USUARIO
        // ------------------------------------------------------------
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (dniSeleccionado == null)
            {
                MessageBox.Show("Seleccioná un usuario de la lista.");
                return;
            }

            var usuarioEditado = new Usuario
            {
                Dni = dniSeleccionado, // se usa el DNI como identificador lógico
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Password = txtPassword.Text.Trim(),
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

        // ------------------------------------------------------------
        // ELIMINAR USUARIO
        // ------------------------------------------------------------
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow?.DataBoundItem is Usuario uSel && uSel.Rol == Rol.Administrador)
            {
                MessageBox.Show("No se puede eliminar un usuario con rol Administrador.");
                return;
            }

            if (dniSeleccionado == null)
            {
                MessageBox.Show("Seleccioná un usuario primero.");
                return;
            }

            try
            {
                controller.Eliminar(dniSeleccionado);
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

        // ------------------------------------------------------------
        // VALIDACIONES DE ENTRADA
        // ------------------------------------------------------------
        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
                e.Handled = true;
        }

        private void txtApellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
                e.Handled = true;
        }

        // ------------------------------------------------------------
        // SELECCIÓN EN LA GRILLA
        // ------------------------------------------------------------
        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null || dgvUsuarios.CurrentRow.DataBoundItem == null)
                return;

            var usuario = (Usuario)dgvUsuarios.CurrentRow.DataBoundItem;

            dniSeleccionado = usuario.Dni; // ahora usamos Dni
            txtNombre.Text = usuario.Nombre;
            txtApellido.Text = usuario.Apellido;
            txtEmail.Text = usuario.Email;
            txtPassword.Text = usuario.Password;
            cmbRol.SelectedIndex = (int)usuario.Rol;

            btnEliminar.Enabled = usuario.Rol != Rol.Administrador;
        }

        // ------------------------------------------------------------
        // BÚSQUEDA EN TIEMPO REAL
        // ------------------------------------------------------------
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string q = txtBuscar.Text.Trim().ToLower();

            if (string.IsNullOrEmpty(q) || q == "nombre usuario")
            {
                dgvUsuarios.DataSource = usuariosCache;
            }
            else
            {
                var filtrados = usuariosCache.FindAll(u =>
                    (u.Nombre != null && u.Nombre.ToLower().Contains(q)) ||
                    (u.Apellido != null && u.Apellido.ToLower().Contains(q)) ||
                    (u.Email != null && u.Email.ToLower().Contains(q)) ||
                    (u.Dni != null && u.Dni.ToLower().Contains(q))
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

        private void EstilizarBotones()
        {
            Button[] botones = { btnAgregar, btnEditar, btnEliminar, btnLimpiar };
            foreach (var b in botones)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.UseVisualStyleBackColor = false;
            }

            btnAgregar.BackColor = Color.FromArgb(46, 204, 113);
            btnAgregar.ForeColor = Color.White;

            btnEditar.BackColor = Color.Gold;
            btnEditar.ForeColor = Color.Black;

            btnEliminar.BackColor = Color.FromArgb(231, 76, 60);
            btnEliminar.ForeColor = Color.White;

            btnLimpiar.BackColor = Color.RoyalBlue;
            btnLimpiar.ForeColor = Color.White;
        }
    }
}
