// ------------------------------------------------------------
// NOMBRE DEL ARCHIVO: UcGestionUsuarios.cs
// PROPÓSITO: Control visual para gestionar los usuarios desde
//             el panel de administración del sistema GymManager.
// AUTOR: Bruno Gamba
// ------------------------------------------------------------

// Espacios de nombres necesarios
using GymManager.Controllers;      // Permite usar el controlador de Usuarios
using GymManager.Models;           // Permite usar la clase Usuario y el enum Rol
using System;
using System.Collections.Generic;  // List<T>
using System.Drawing;              // Colores y fuentes para interfaz
using System.Linq;                 // Métodos de búsqueda y filtrado
using System.Windows.Forms;        // Controles gráficos de WinForms

namespace GymManager.Views
{
    // ------------------------------------------------------------
    // CLASE PARCIAL: UcGestionUsuarios
    // ------------------------------------------------------------
    // Representa la interfaz visual del panel “Gestión de Usuarios”.
    // Permite agregar, editar, eliminar y buscar usuarios.
    // ------------------------------------------------------------
    public partial class UcGestionUsuarios : UserControl
    {
        // ============================================================
        // CAMPOS PRINCIPALES
        // ============================================================

        private UsuarioController controller = new UsuarioController(); // Controlador que maneja la BD
        private List<Usuario> usuariosCache;                            // Lista temporal con los usuarios cargados
        private int idSeleccionado = 0;                                 // Guarda el ID del usuario seleccionado

                                                                        // CAMPO: placeholderBuscar
                                                                        // Guarda SIEMPRE el texto de placeholder vigente según el modo
                                                                        // elegido en el combo (Nombre, Rol o ID). Así, los eventos
                                                                        // Enter/Leave y el filtro pueden saber cuándo NO filtrar.
                                                                        // ------------------------------------------------------------
        private string placeholderBuscar = "Nombre de usuario"; // Valor por defecto



        // ============================================================
        // CONSTRUCTOR
        // ============================================================
        public UcGestionUsuarios()
        {
            InitializeComponent();     // Inicializa los componentes visuales
            CargarUsuarios();          // Carga los usuarios desde la base de datos
            FormatearGrid();           // Aplica formato visual a la tabla
            ConfigurarComboBusqueda(); // Configura las opciones del combo de búsqueda
            ConfigurarPlaceholder();   // Prepara el texto guía del buscador
            EstilizarBotones();        // Asigna colores y estilos a los botones
            ActualizarPlaceholderBusqueda(); // Establece el placeholder inicial

        }

        // ============================================================
        // MÉTODO: CargarUsuarios
        // ------------------------------------------------------------
        // Obtiene los usuarios activos desde la BD y los muestra
        // en el DataGridView.
        // ============================================================
        private void CargarUsuarios()
        {
            // Se obtienen todos los usuarios activos
            usuariosCache = controller.ObtenerTodos();

            // Se limpia la fuente de datos actual (por si existía algo anterior)
            dgvUsuarios.DataSource = null;

            // Se asigna la lista actualizada al DataGridView
            dgvUsuarios.DataSource = usuariosCache;

            // Por seguridad, ocultamos la columna de contraseñas
            if (dgvUsuarios.Columns["Password"] != null)
                dgvUsuarios.Columns["Password"].Visible = false;
        }

        // ============================================================
        // MÉTODO: FormatearGrid
        // ------------------------------------------------------------
        // Ajusta el diseño y estilo de la grilla para una mejor lectura.
        // ============================================================
        private void FormatearGrid()
        {
            // Cambiamos los encabezados a nombres más legibles
            if (dgvUsuarios.Columns["IdUsuario"] != null)
                dgvUsuarios.Columns["IdUsuario"].HeaderText = "ID";
            if (dgvUsuarios.Columns["Nombre"] != null)
                dgvUsuarios.Columns["Nombre"].HeaderText = "Nombre";
            if (dgvUsuarios.Columns["Apellido"] != null)
                dgvUsuarios.Columns["Apellido"].HeaderText = "Apellido";
            if (dgvUsuarios.Columns["Email"] != null)
                dgvUsuarios.Columns["Email"].HeaderText = "Correo Electrónico";
            if (dgvUsuarios.Columns["Rol"] != null)
                dgvUsuarios.Columns["Rol"].HeaderText = "Rol";

            // Configuración visual
            dgvUsuarios.AlternatingRowsDefaultCellStyle.BackColor = Color.LightGray; // Filas intercaladas en gris
            dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;  // Las columnas ocupan todo el ancho
            dgvUsuarios.RowTemplate.Height = 30;                                    // Altura de cada fila
            dgvUsuarios.Font = new Font("Segoe UI", 10, FontStyle.Regular);         // Fuente uniforme
        }

        // ============================================================
        // MÉTODO: LimpiarCampos
        // ------------------------------------------------------------
        // Vacía los inputs del formulario para dejarlo en blanco.
        // ============================================================
        private void LimpiarCampos()
        {
            txtNombre.Text = "";
            txtApellido.Text = "";
            txtEmail.Text = "";
            txtPassword.Text = "";
            cmbRol.SelectedIndex = -1;
            idSeleccionado = 0; // Resetea la variable de selección
        }

        // ============================================================
        // BOTÓN: Agregar Usuario
        // ------------------------------------------------------------
        // Crea un nuevo usuario y lo guarda en la base de datos.
        // ============================================================
        private void btnAgregar_Click(object sender, EventArgs e)
        {
            // Validamos que todos los campos estén completos
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

            // Confirmación antes de agregar
            var confirmar = MessageBox.Show(
                $"¿Desea agregar al usuario {txtNombre.Text} {txtApellido.Text}?",
                "Confirmar alta",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmar != DialogResult.Yes)
                return; // Si el usuario cancela, no hace nada

            // Construimos el nuevo usuario con los datos ingresados
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
                // Enviamos los datos al controlador para guardarlos en la BD
                controller.Agregar(nuevoUsuario);

                MessageBox.Show("Usuario agregado correctamente.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Refrescamos la lista y limpiamos el formulario
                CargarUsuarios();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                // Si ocurre un error (duplicado, conexión, etc.)
                MessageBox.Show("Error al agregar el usuario:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // BOTÓN: Editar Usuario
        // ------------------------------------------------------------
        // Actualiza los datos del usuario seleccionado en la grilla.
        // ============================================================
        private void btnEditar_Click(object sender, EventArgs e)
        {
            // Verifica que haya un usuario seleccionado
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Seleccioná un usuario primero.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Confirmación antes de aplicar cambios
            var confirmar = MessageBox.Show(
                $"¿Desea editar el usuario {txtNombre.Text} {txtApellido.Text}?",
                "Confirmar edición",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (confirmar != DialogResult.Yes)
                return;

            // Creamos el objeto con los nuevos valores
            var usuarioEditado = new Usuario
            {
                IdUsuario = idSeleccionado,
                Nombre = txtNombre.Text.Trim(),
                Apellido = txtApellido.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Password = txtPassword.Text.Trim(), // si está vacío, el controlador lo ignora
                Rol = (Rol)cmbRol.SelectedIndex
            };

            try
            {
                controller.Editar(usuarioEditado);

                MessageBox.Show("Usuario editado correctamente.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Actualizamos la lista y limpiamos
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
        // BOTÓN: Eliminar Usuario
        // ------------------------------------------------------------
        // Desactiva (baja lógica) al usuario seleccionado.
        // ============================================================
        private void btnEliminar_Click(object sender, EventArgs e)
        {
            if (idSeleccionado == 0)
            {
                MessageBox.Show("Seleccioná un usuario para eliminar.",
                    "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Confirmación antes de eliminar
            var confirmar = MessageBox.Show(
                $"¿Desea eliminar al usuario {txtNombre.Text} {txtApellido.Text}?",
                "Confirmar eliminación",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning);

            if (confirmar != DialogResult.Yes)
                return;

            try
            {
                // Llamamos al método del controlador para realizar la baja lógica
                controller.Eliminar(idSeleccionado);

                MessageBox.Show("Usuario eliminado correctamente.",
                    "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Actualizamos la vista
                CargarUsuarios();
                LimpiarCampos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar el usuario:\n" + ex.Message,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // EVENTO: Selección de fila en la grilla
        // ------------------------------------------------------------
        // Carga los datos del usuario seleccionado en los campos
        // de texto para poder editarlos.
        // ============================================================
        private void dgvUsuarios_SelectionChanged(object sender, EventArgs e)
        {
            // Si no hay fila seleccionada, salimos
            if (dgvUsuarios.CurrentRow == null || dgvUsuarios.CurrentRow.DataBoundItem == null)
                return;

            // Obtenemos el usuario seleccionado
            var usuario = (Usuario)dgvUsuarios.CurrentRow.DataBoundItem;

            // Guardamos el ID para futuras operaciones
            idSeleccionado = usuario.IdUsuario;

            // Mostramos los datos en los campos
            txtNombre.Text = usuario.Nombre;
            txtApellido.Text = usuario.Apellido;
            txtEmail.Text = usuario.Email;
            txtPassword.Text = ""; // Nunca mostramos la contraseña real
            cmbRol.SelectedIndex = (int)usuario.Rol;

            // Deshabilitamos la eliminación si es un administrador
            btnEliminar.Enabled = usuario.Rol != Rol.Administrador;
        }

        // ------------------------------------------------------------
        // EVENTO: txtBuscar_TextChanged
        // - Filtra según el modo y el texto
        // - IGNORA el texto cuando es el placeholder actual
        // ------------------------------------------------------------
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            // Toma el texto tal cual (sin forzar a minúscula aún)
            string q = txtBuscar.Text.Trim();

            // Si el TextBox está mostrando el placeholder actual,
            // o está vacío, NO filtramos: mostramos toda la lista.
            if (string.IsNullOrEmpty(q) ||
                q.Equals(placeholderBuscar, StringComparison.OrdinalIgnoreCase))
            {
                dgvUsuarios.DataSource = usuariosCache;
                return; // Salimos temprano para no disparar filtros falsos
            }

            // Ya podemos trabajar en minúsculas para comparar
            string ql = q.ToLowerInvariant();

            // Determina el modo de búsqueda actual
            string modo = cboBuscarPor.SelectedItem?.ToString() ?? "Nombre";

            // Lista temporal para los resultados
            List<Usuario> filtrados = new List<Usuario>();

            // Aplica el filtro según el modo elegido
            switch (modo)
            {
                case "Rol":
                    // Compara el nombre del enum con el texto buscado
                    filtrados = usuariosCache.FindAll(u => u.Rol.ToString().ToLower().Contains(ql));
                    break;

                case "ID":
                    // Si el usuario escribió un número válido, filtra por igualdad de ID
                    if (int.TryParse(q, out int idBuscado))
                        filtrados = usuariosCache.FindAll(u => u.IdUsuario == idBuscado);
                    break;

                default: // "Nombre"
                         // Busca por nombre, apellido o email
                    filtrados = usuariosCache.FindAll(u =>
                        u.Nombre.ToLower().Contains(ql) ||
                        u.Apellido.ToLower().Contains(ql) ||
                        u.Email.ToLower().Contains(ql));
                    break;
            }

            // Refresca la grilla con el resultado
            dgvUsuarios.DataSource = filtrados;

            // Por seguridad, ocultamos siempre la contraseña si existiera la columna
            if (dgvUsuarios.Columns["Password"] != null)
                dgvUsuarios.Columns["Password"].Visible = false;
        }


        // ------------------------------------------------------------
        // MÉTODO: ConfigurarPlaceholder()
        // - Conecta los eventos Enter/Leave del TextBox de búsqueda
        // - Al entrar: si está el placeholder → borra y pone negro
        // - Al salir: si quedó vacío → restaura el placeholder gris
        // ------------------------------------------------------------
        private void ConfigurarPlaceholder()
        {
            // Al recibir foco el TextBox...
            txtBuscar.Enter += (s, e) =>
            {
                // Si lo que se ve es el placeholder, lo limpiamos
                if (txtBuscar.Text == placeholderBuscar)
                {
                    txtBuscar.Text = "";           // Borra el texto guía
                    txtBuscar.ForeColor = Color.Black; // Pinta en negro para escribir
                }
            };

            // Al perder foco el TextBox...
            txtBuscar.Leave += (s, e) =>
            {
                // Si quedó vacío o solo espacios, restauramos el placeholder
                if (string.IsNullOrWhiteSpace(txtBuscar.Text))
                {
                    txtBuscar.ForeColor = Color.Gray;   // Vuelve a gris
                    txtBuscar.Text = placeholderBuscar; // Muestra el texto guía actual
                }
            };
        }


        // ------------------------------------------------------------
        // CONFIGURAR COMBO DE BÚSQUEDA
        // - Carga las opciones "Nombre", "Rol" e "ID"
        // - Escucha el cambio de selección para actualizar el placeholder
        // ------------------------------------------------------------
        // ------------------------------------------------------------
        // CONFIGURAR COMBO DE BÚSQUEDA
        // - Carga las opciones "Nombre", "Rol" e "ID"
        // - Escucha el cambio de selección para actualizar el placeholder
        // ------------------------------------------------------------
        private void ConfigurarComboBusqueda()
        {
            // Limpia opciones previas por si ya se cargaron antes
            cboBuscarPor.Items.Clear();

            // Agrega las tres opciones de búsqueda disponibles
            cboBuscarPor.Items.AddRange(new object[] { "Nombre", "Rol", "ID" });

            // Selecciona por defecto "Nombre"
            cboBuscarPor.SelectedIndex = 0;

            // Cada vez que el usuario cambie el modo de búsqueda...
            cboBuscarPor.SelectedIndexChanged += (s, e) =>
            {
                // ...actualizamos el placeholder del textbox de búsqueda.
                // Nota: por UX, al cambiar el modo se muestra el placeholder
                // del modo nuevo y se "resetea" el texto a ese placeholder.
                ActualizarPlaceholderBusqueda();
            };
        }



        /// ------------------------------------------------------------
        // MÉTODO: ActualizarPlaceholderBusqueda()
        // - Define el texto del placeholder según el modo seleccionado.
        // - Escribe el placeholder en el TextBox y lo pinta en gris.
        // ------------------------------------------------------------
        private void ActualizarPlaceholderBusqueda()
        {
            // Obtiene el modo actual del combo, o "Nombre" si por algún
            // motivo no hay nada seleccionado (fallback seguro).
            string modo = cboBuscarPor.SelectedItem?.ToString() ?? "Nombre";

            // Decide el texto de placeholder según el modo
            switch (modo)
            {
                case "Rol":
                    placeholderBuscar = "Tipo de rol";      // Ej.: "Administrador"
                    break;
                case "ID":
                    placeholderBuscar = "Número de ID";     // Ej.: "12"
                    break;
                default:
                    placeholderBuscar = "Nombre de usuario"; // Ej.: "Juan", "Pérez"
                    break;
            }

            // Pone el placeholder en el TextBox y lo muestra en gris
            txtBuscar.ForeColor = Color.Gray;
            txtBuscar.Text = placeholderBuscar;
        }



        private void EstilizarBotones()
        {
            // Asignamos estilo visual a los botones
            Button[] botones = { btnAgregar, btnEditar, btnEliminar, btnLimpiar };
            foreach (var b in botones)
            {
                b.FlatStyle = FlatStyle.Flat;       // Estilo plano
                b.UseVisualStyleBackColor = false;  // Colores personalizados
            }

            // Colores específicos para cada botón
            btnAgregar.BackColor = Color.FromArgb(46, 204, 113);  // Verde
            btnEditar.BackColor = Color.Gold;                     // Amarillo
            btnEliminar.BackColor = Color.FromArgb(231, 76, 60);  // Rojo
            btnLimpiar.BackColor = Color.RoyalBlue;               // Azul

            // Texto en color blanco
            btnAgregar.ForeColor = btnEliminar.ForeColor = btnLimpiar.ForeColor = Color.White;
        }

        // ============================================================
        // BOTÓN: Limpiar
        // ------------------------------------------------------------
        // Borra todos los campos y pide confirmación.
        // ============================================================
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

        // ============================================================
        // VALIDACIONES DE ENTRADA DE TEXTO
        // ============================================================

        // Permite solo letras en el campo "Nombre"
        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
                e.Handled = true;
        }

        // Permite solo letras en el campo "Apellido"
        private void txtApellido_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsLetter(e.KeyChar) && e.KeyChar != ' ')
                e.Handled = true;
        }
    }
}
