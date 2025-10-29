// ------------------------------------------------------------
// NOMBRE DEL ARCHIVO: UcGestionUsuarios.cs
// PROPÓSITO: Control visual para gestionar los usuarios desde
//             el panel de administración del sistema GymManager.
// AUTOR: Bruno Gamba
// ------------------------------------------------------------

using GymManager.Controllers;
using GymManager.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace GymManager.Views
{
    public partial class UcGestionUsuarios : UserControl
    {
        // ============================================================
        // CAMPOS PRINCIPALES
        // ============================================================

        private UsuarioController controller = new UsuarioController(); // Controlador de BD
        private List<Usuario> usuariosCache;                            // Cache temporal de usuarios
        private int idSeleccionado = 0;                                 // ID del usuario seleccionado
        private string placeholderBuscar = "Nombre de usuario";         // Placeholder por defecto


        // ============================================================
        // CONSTRUCTOR
        // ============================================================
        public UcGestionUsuarios()
        {
            InitializeComponent();
            CargarUsuarios();          // Carga todos los usuarios
            ConfigurarComboBusqueda(); // Combo "Buscar por"
            ConfigurarPlaceholder();   // Placeholder dinámico
            EstilizarBotones();        // Estilos visuales
            ActualizarPlaceholderBusqueda();
        }


        // ============================================================
        // MÉTODO: CargarUsuarios
        // ------------------------------------------------------------
        // Carga todos los usuarios (activos e inactivos) y muestra
        // su estado visual (verde/rojo) directamente.
        // ============================================================
        // ============================================================
        // MÉTODO: CargarUsuarios
        // ------------------------------------------------------------
        // Muestra todos los usuarios (activos e inactivos) y aplica
        // colores al estado mediante el evento CellFormatting.
        // ============================================================
        private void CargarUsuarios()
        {
            // 1️⃣ Obtenemos la lista completa desde la base
            usuariosCache = controller.ObtenerTodos();

            // 2️⃣ Limpiamos cualquier fuente anterior
            dgvUsuarios.DataSource = null;
            dgvUsuarios.Columns.Clear();

            // 3️⃣ Asignamos la nueva lista
            dgvUsuarios.DataSource = usuariosCache;

            // 4️⃣ Ocultamos columnas que no queremos mostrar directamente
            if (dgvUsuarios.Columns["Password"] != null)
                dgvUsuarios.Columns["Password"].Visible = false;

            if (dgvUsuarios.Columns["Activo"] != null)
                dgvUsuarios.Columns["Activo"].Visible = false;

            // 5️⃣ Cambiamos encabezados
            if (dgvUsuarios.Columns["IdUsuario"] != null)
                dgvUsuarios.Columns["IdUsuario"].HeaderText = "ID";
            if (dgvUsuarios.Columns["Email"] != null)
                dgvUsuarios.Columns["Email"].HeaderText = "Correo";

            // 6️⃣ Agregamos una columna visual para el estado (si no existe)
            if (!dgvUsuarios.Columns.Contains("Estado"))
            {
                var colEstado = new DataGridViewTextBoxColumn
                {
                    Name = "Estado",
                    HeaderText = "Estado",
                    ReadOnly = true
                };
                dgvUsuarios.Columns.Add(colEstado);
            }

            // 7️⃣ Evento que se dispara cada vez que se dibuja una celda
            dgvUsuarios.CellFormatting -= DgvUsuarios_CellFormatting; // Evita duplicar
            dgvUsuarios.CellFormatting += DgvUsuarios_CellFormatting;

            // 8️⃣ Ajustes visuales
            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray;
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.RowTemplate.Height = 30;
            dgvUsuarios.Font = new Font("Segoe UI", 10, FontStyle.Regular);
        }

        // ============================================================
        // EVENTO: DgvUsuarios_CellFormatting
        // ------------------------------------------------------------
        // Aplica los valores y colores del estado dinámicamente.
        // ============================================================
        private void DgvUsuarios_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvUsuarios.Columns[e.ColumnIndex].Name == "Estado" && e.RowIndex >= 0)
            {
                var usuario = dgvUsuarios.Rows[e.RowIndex].DataBoundItem as Usuario;
                if (usuario != null)
                {
                    e.Value = usuario.Activo ? "Activo" : "Inactivo";
                    e.CellStyle.ForeColor = usuario.Activo ? Color.Green : Color.Red;
                    e.CellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
                }
            }
        }



        // ============================================================
        // EVENTO: dgvUsuarios_CellClick
        // ------------------------------------------------------------
        // Permite reactivar un usuario haciendo clic directamente
        // sobre la celda "Inactivo".
        // ============================================================
        private void dgvUsuarios_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0)
                return;

            if (dgvUsuarios.Columns[e.ColumnIndex].Name == "Estado")
            {
                var usuario = dgvUsuarios.Rows[e.RowIndex].DataBoundItem as Usuario;

                if (usuario != null && !usuario.Activo)
                {
                    var confirmar = MessageBox.Show(
                        $"¿Desea activar al usuario {usuario.Nombre} {usuario.Apellido}?",
                        "Confirmar reactivación",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question);

                    if (confirmar == DialogResult.Yes)
                    {
                        try
                        {
                            controller.Reactivar(usuario.IdUsuario);
                            MessageBox.Show("Usuario activado correctamente.",
                                "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            CargarUsuarios(); // Refrescamos tabla

                            // 🔁 Refrescar torta de reportes si está abierta
                            foreach (Control ctrl in this.Parent.Controls)
                            {
                                if (ctrl is GymManager.Views.UcReportes reportes)
                                {
                                    reportes.RefrescarGraficos();
                                    break;
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Error al activar el usuario:\n" + ex.Message,
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
        }



        // ============================================================
        // MÉTODO: LimpiarCampos
        // ------------------------------------------------------------
        // Deja en blanco todos los campos del formulario.
        // ============================================================
        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            cmbRol.SelectedIndex = -1;
            idSeleccionado = 0;
        }


        // ============================================================
        // BOTÓN: Agregar
        // ============================================================
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text) ||
                string.IsNullOrWhiteSpace(txtApellido.Text) ||
                string.IsNullOrWhiteSpace(txtEmail.Text) ||
                string.IsNullOrWhiteSpace(txtPassword.Text) ||
                cmbRol.SelectedIndex == -1)
            {
                MessageBox.Show("Por favor completá todos los campos antes de agregar.",
                    "Campos incompletos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var confirmar = MessageBox.Show(
                $"¿Desea agregar al usuario {txtNombre.Text} {txtApellido.Text}?",
                "Confirmar alta", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmar != DialogResult.Yes)
                return;

            var nuevoUsuario = new Usuario
            {
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                Rol = (Rol)cmbRol.SelectedIndex
            };

            try
            {
                controller.Agregar(nuevoUsuario);
                MessageBox.Show("Usuario agregado correctamente.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarUsuarios();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al agregar el usuario:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // ============================================================
        // BOTÓN: Editar
        // ============================================================
        private void btnEditar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Seleccioná un usuario primero.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmar = MessageBox.Show(
                $"¿Desea editar el usuario {txtNombre.Text} {txtApellido.Text}?",
                "Confirmar edición", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirmar != DialogResult.Yes)
                return;

            var usuarioEditado = new Usuario
            {
                IdUsuario = idSeleccionado,
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Password = txtPassword.Text.Trim(),
                Rol = (Rol)cmbRol.SelectedIndex
            };

            try
            {
                controller.Editar(usuarioEditado);
                MessageBox.Show("Usuario editado correctamente.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarUsuarios();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al editar el usuario:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // ============================================================
        // BOTÓN: Eliminar (baja lógica)
        // ============================================================
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Seleccioná un usuario para eliminar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirmar = MessageBox.Show(
                $"¿Desea eliminar al usuario {txtNombre.Text} {txtApellido.Text}?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (confirmar != DialogResult.Yes)
                return;

            try
            {
                controller.Eliminar(idSeleccionado);
                MessageBox.Show("Usuario eliminado correctamente.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                CargarUsuarios();
                LimpiarCampos();

                // 🔁 Refrescar torta de reportes si está abierta
                foreach (Control ctrl in this.Parent.Controls)
                {
                    if (ctrl is GymManager.Views.UcReportes reportes)
                    {
                        reportes.RefrescarGraficos();
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el usuario:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // ============================================================
        // EVENTO: Selección de fila en la grilla
        // ============================================================
        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvUsuarios.CurrentRow == null || dgvUsuarios.CurrentRow.DataBoundItem == null)
                return;

            var usuario = (Usuario)dgvUsuarios.CurrentRow.DataBoundItem;

            idSeleccionado = usuario.IdUsuario;
            txtNombre.Text = usuario.Nombre;
            txtApellido.Text = usuario.Apellido;
            txtEmail.Text = usuario.Email;
            txtPassword.Text = "";
            cmbRol.SelectedIndex = (int)usuario.Rol;

            btnEliminar.Enabled = usuario.Rol != Rol.Administrador;
        }


        // ============================================================
        // BÚSQUEDA DINÁMICA
        // ============================================================
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            string q = txtBuscar.Text.Trim();

            if (string.IsNullOrEmpty(q) ||
                q.Equals(placeholderBuscar, StringComparison.OrdinalIgnoreCase))
            {
                dgvUsuarios.DataSource = usuariosCache;
                return;
            }

            string ql = q.ToLowerInvariant();
            string modo = cboBuscarPor.SelectedItem?.ToString() ?? "Nombre";

            List<Usuario> filtrados = new List<Usuario>();

            switch (modo)
            {
                case "Rol":
                    filtrados = usuariosCache.FindAll(u => u.Rol.ToString().ToLower().Contains(ql));
                    break;

                case "ID":
                    if (int.TryParse(q, out int idBuscado))
                        filtrados = usuariosCache.FindAll(u => u.IdUsuario == idBuscado);
                    break;

                default:
                    filtrados = usuariosCache.FindAll(u =>
                        u.Nombre.ToLower().Contains(ql) ||
                        u.Apellido.ToLower().Contains(ql) ||
                        u.Email.ToLower().Contains(ql));
                    break;
            }

            dgvUsuarios.DataSource = filtrados;
            if (dgvUsuarios.Columns["Password"] != null)
                dgvUsuarios.Columns["Password"].Visible = false;
        }


        // ============================================================
        // PLACEHOLDER Y BUSCADOR
        // ============================================================
        private void ConfigurarPlaceholder()
        {
            txtBuscar.Enter += (s, e) =>
            {
                if (txtBuscar.Text == placeholderBuscar)
                {
                    txtBuscar.Text = "";
                    txtBuscar.ForeColor = Color.Black;
                }
            };

            txtBuscar.Leave += (s, e) =>
            {
                if (string.IsNullOrWhiteSpace(txtBuscar.Text))
                {
                    txtBuscar.ForeColor = Color.Gray;
                    txtBuscar.Text = placeholderBuscar;
                }
            };
        }

        private void ConfigurarComboBusqueda()
        {
            cboBuscarPor.Items.Clear();
            cboBuscarPor.Items.AddRange(new object[] { "Nombre", "Rol", "ID" });
            cboBuscarPor.SelectedIndex = 0;

            cboBuscarPor.SelectedIndexChanged += (s, e) =>
            {
                ActualizarPlaceholderBusqueda();
            };
        }

        private void ActualizarPlaceholderBusqueda()
        {
            string modo = cboBuscarPor.SelectedItem?.ToString() ?? "Nombre";

            placeholderBuscar = modo switch
            {
                "Rol" => "Tipo de rol",
                "ID" => "Número de ID",
                _ => "Nombre de usuario"
            };

            txtBuscar.ForeColor = Color.Gray;
            txtBuscar.Text = placeholderBuscar;
        }


        // ============================================================
        // ESTILO DE BOTONES
        // ============================================================
        private void EstilizarBotones()
        {
            Button[] botones = { btnAgregar, btnEditar, btnEliminar, btnLimpiar };
            foreach (var b in botones)
            {
                b.FlatStyle = FlatStyle.Flat;
                b.UseVisualStyleBackColor = false;
            }

            btnAgregar.BackColor = Color.FromArgb(46, 204, 113);
            btnEditar.BackColor = Color.Gold;
            btnEliminar.BackColor = Color.FromArgb(231, 76, 60);
            btnLimpiar.BackColor = Color.RoyalBlue;

            btnAgregar.ForeColor = btnEliminar.ForeColor = btnLimpiar.ForeColor = Color.White;
        }


        // ============================================================
        // VALIDACIONES DE TEXTO
        // ============================================================
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

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            DialogResult r = MessageBox.Show(
                "¿Desea limpiar todos los campos del formulario?",
                "Confirmar limpieza",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (r == DialogResult.Yes)
            {
                LimpiarCampos();
                MessageBox.Show("Formulario limpiado correctamente.",
                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

    }
}
