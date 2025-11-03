using GymManager.Controllers;
using GymManager.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace GymManager.Views
{
    public partial class UcGestionUsuarios : UserControl
    {
        // ============================================================
        // CAMPOS PRINCIPALES
        // ============================================================
        private UsuarioController controller = new UsuarioController();
        private List<Usuario> usuariosCache;
        private int idSeleccionado = 0;
        private string placeholderBuscar = "Nombre de usuario";

        // Colores modernos para la aplicación
        private readonly Color ColorPrimario = Color.FromArgb(41, 128, 185);
        private readonly Color ColorSecundario = Color.FromArgb(52, 152, 219);
        private readonly Color ColorExito = Color.FromArgb(46, 204, 113);
        private readonly Color ColorAdvertencia = Color.FromArgb(241, 196, 15);
        private readonly Color ColorPeligro = Color.FromArgb(231, 76, 60);
        private readonly Color ColorFondo = Color.FromArgb(245, 247, 250);
        private readonly Color ColorBorde = Color.FromArgb(224, 230, 237);
        private readonly Color ColorTexto = Color.FromArgb(52, 73, 94);

        public UcGestionUsuarios()
        {
            InitializeComponent();
            AplicarEstiloModerno();
            CargarUsuarios();
            ConfigurarComboBusqueda();
            ConfigurarPlaceholder();
            EstilizarBotones();
            ActualizarPlaceholderBusqueda();
        }

        // ============================================================
        // MÉTODOS DE ESTILO MODERNO
        // ============================================================
        private void AplicarEstiloModerno()
        {
            this.BackColor = ColorFondo;
            this.Font = new Font("Segoe UI", 9F, FontStyle.Regular);

            // Estilo del título
            lblTitulo.ForeColor = ColorTexto;
            lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            lblTitulo.BackColor = Color.Transparent;

            // Estilo de labels
            foreach (Control ctrl in this.Controls)
            {
                if (ctrl is Label label)
                {
                    label.ForeColor = ColorTexto;
                    label.BackColor = Color.Transparent;
                }
            }

            // Estilo de controles
            EstilizarControles();
        }

        private void EstilizarControles()
        {
            // Estilo para TextBox
            EstilizarTextBox(txtNombre);
            EstilizarTextBox(txtApellido);
            EstilizarTextBox(txtEmail);
            EstilizarTextBox(txtPassword);
            EstilizarTextBox(txtBuscar);

            // Estilo para ComboBox
            EstilizarComboBox(cmbRol);
            EstilizarComboBox(cboBuscarPor);
        }

        private void EstilizarTextBox(TextBox txt)
        {
            txt.BorderStyle = BorderStyle.FixedSingle;
            txt.BackColor = Color.White;
            txt.ForeColor = ColorTexto;
            txt.Font = new Font("Segoe UI", 9F);
        }

        private void EstilizarComboBox(ComboBox cmb)
        {
            cmb.FlatStyle = FlatStyle.Flat;
            cmb.BackColor = Color.White;
            cmb.ForeColor = ColorTexto;
            cmb.Font = new Font("Segoe UI", 9F);
        }

        // ============================================================
        // MÉTODO: CargarUsuarios
        // ============================================================
        private void CargarUsuarios()
        {
            usuariosCache = controller.ObtenerTodos();
            dgvUsuarios.DataSource = null;
            dgvUsuarios.Columns.Clear();
            dgvUsuarios.DataSource = usuariosCache;

            if (dgvUsuarios.Columns["Password"] != null)
                dgvUsuarios.Columns["Password"].Visible = false;

            if (dgvUsuarios.Columns["Activo"] != null)
                dgvUsuarios.Columns["Activo"].Visible = false;

            if (dgvUsuarios.Columns["IdUsuario"] != null)
                dgvUsuarios.Columns["IdUsuario"].HeaderText = "ID";
            if (dgvUsuarios.Columns["Email"] != null)
                dgvUsuarios.Columns["Email"].HeaderText = "Correo";

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

            dgvUsuarios.CellFormatting -= DgvUsuarios_CellFormatting;
            dgvUsuarios.CellFormatting += DgvUsuarios_CellFormatting;

            EstilizarGrilla();
        }

        private void EstilizarGrilla()
        {
            var g = dgvUsuarios;

            g.BorderStyle = BorderStyle.None;
            g.BackgroundColor = Color.White;
            g.EnableHeadersVisualStyles = false;
            g.GridColor = ColorBorde;

            // Encabezados modernos
            g.ColumnHeadersDefaultCellStyle.BackColor = ColorPrimario;
            g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            g.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            g.ColumnHeadersHeight = 40;
            g.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Filas
            g.DefaultCellStyle.BackColor = Color.White;
            g.DefaultCellStyle.ForeColor = ColorTexto;
            g.DefaultCellStyle.SelectionBackColor = Color.FromArgb(232, 244, 253);
            g.DefaultCellStyle.SelectionForeColor = ColorTexto;
            g.DefaultCellStyle.Font = new Font("Segoe UI", 9F);
            g.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);

            // Configuración general
            g.RowHeadersVisible = false;
            g.AllowUserToAddRows = false;
            g.AllowUserToResizeRows = false;
            g.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            g.RowTemplate.Height = 35;
        }

        private void DgvUsuarios_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dgvUsuarios.Columns[e.ColumnIndex].Name == "Estado" && e.RowIndex >= 0)
            {
                var usuario = dgvUsuarios.Rows[e.RowIndex].DataBoundItem as Usuario;
                if (usuario != null)
                {
                    e.Value = usuario.Activo ? "Activo" : "Inactivo";
                    e.CellStyle.ForeColor = usuario.Activo ? Color.Green : Color.Red;
                    e.CellStyle.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                }
            }
        }

        // ============================================================
        // EVENTO: dgvUsuarios_CellClick
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

                            CargarUsuarios();

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
                    txtBuscar.ForeColor = ColorTexto;
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
                b.FlatAppearance.BorderSize = 0;
                b.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
                b.ForeColor = Color.White;
                b.Size = new Size(100, 35);
                b.Cursor = Cursors.Hand;

                // Efecto hover moderno
                b.MouseEnter += (s, e) => {
                    b.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(b.BackColor, 0.1f);
                };
                b.MouseLeave += (s, e) => {
                    b.FlatAppearance.MouseOverBackColor = b.BackColor;
                };

                // Bordes redondeados
                int radius = 6;
                b.Paint += (s, e) =>
                {
                    var rect = new Rectangle(0, 0, b.Width, b.Height);
                    using (var path = new GraphicsPath())
                    {
                        int d = radius * 2;
                        path.AddArc(rect.X, rect.Y, d, d, 180, 90);
                        path.AddArc(rect.Right - d, rect.Y, d, d, 270, 90);
                        path.AddArc(rect.Right - d, rect.Bottom - d, d, d, 0, 90);
                        path.AddArc(rect.X, rect.Bottom - d, d, d, 90, 90);
                        path.CloseFigure();
                        b.Region = new Region(path);
                    }
                };
            }

            btnAgregar.BackColor = ColorExito;
            btnEditar.BackColor = ColorSecundario;
            btnEliminar.BackColor = ColorPeligro;
            btnLimpiar.BackColor = Color.FromArgb(149, 165, 166);
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