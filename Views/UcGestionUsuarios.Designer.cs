using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Views
{
    partial class UcGestionUsuarios
    {
        private System.ComponentModel.IContainer components = null;

        // ------------------------------------------------------------
        // 🧩 DECLARACIÓN DE CONTROLES
        // ------------------------------------------------------------
        private TextBox txtNombre;             // Campo para nombre del usuario
        private TextBox txtApellido;           // Campo para apellido del usuario
        private TextBox txtEmail;              // Campo para correo electrónico
        private TextBox txtPassword;           // Campo para contraseña (oculta con asteriscos)
        private ComboBox cmbRol;               // Combo para seleccionar el rol del usuario
        private Button btnAgregar;             // Botón para agregar un nuevo usuario
        private Button btnEditar;              // Botón para editar un usuario existente
        private Button btnEliminar;            // Botón para eliminar (baja lógica)
        private Button btnLimpiar;             // Botón para limpiar los campos del formulario
        private DataGridView dgvUsuarios;      // Tabla donde se listan los usuarios
        private TextBox txtBuscar;             // Campo de texto para búsqueda
        private Label lblTitulo;               // Título del formulario
        private Label lblNombre;               // Etiqueta "Nombre"
        private Label lblApellido;             // Etiqueta "Apellido"
        private Label lblEmail;                // Etiqueta "Email"
        private Label lblPassword;             // Etiqueta "Contraseña"
        private Label lblRol;                  // Etiqueta "Rol"
        private Label lblBuscar;               // Etiqueta "Buscar:"
        private ComboBox cboBuscarPor;         // Combo desplegable: buscar por Nombre, Rol o ID


        // ------------------------------------------------------------
        // MÉTODO Dispose: libera los recursos utilizados
        // ------------------------------------------------------------
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose(); // Libera la memoria de los componentes
            }
            base.Dispose(disposing);
        }

        // ------------------------------------------------------------
        // MÉTODO DE INICIALIZACIÓN DE COMPONENTES (Diseño visual)
        // ------------------------------------------------------------
        private void InitializeComponent()
        {
            this.txtNombre = new TextBox();
            this.txtApellido = new TextBox();
            this.txtEmail = new TextBox();
            this.txtPassword = new TextBox();
            this.cmbRol = new ComboBox();
            this.btnAgregar = new Button();
            this.btnEditar = new Button();
            this.btnEliminar = new Button();
            this.btnLimpiar = new Button();
            this.dgvUsuarios = new DataGridView();
            this.txtBuscar = new TextBox();
            this.lblTitulo = new Label();
            this.lblNombre = new Label();
            this.lblApellido = new Label();
            this.lblEmail = new Label();
            this.lblPassword = new Label();
            this.lblRol = new Label();
            this.lblBuscar = new Label();
            this.cboBuscarPor = new ComboBox();

            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).BeginInit();
            this.SuspendLayout();

            // ============================================================
            // 🔹 TÍTULO PRINCIPAL
            // ============================================================
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblTitulo.Location = new Point(25, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new Size(260, 30);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "👥 Gestión de Usuarios";

            // ============================================================
            // 🔹 LABELS E INPUTS DE FORMULARIO
            // ============================================================

            // Nombre
            this.lblNombre.Location = new Point(30, 70);
            this.lblNombre.Size = new Size(70, 20);
            this.lblNombre.Text = "Nombre:";
            this.txtNombre.Location = new Point(110, 70);
            this.txtNombre.Size = new Size(220, 23);
            this.txtNombre.KeyPress += new KeyPressEventHandler(this.txtNombre_KeyPress);

            // Apellido
            this.lblApellido.Location = new Point(30, 105);
            this.lblApellido.Size = new Size(70, 20);
            this.lblApellido.Text = "Apellido:";
            this.txtApellido.Location = new Point(110, 105);
            this.txtApellido.Size = new Size(220, 23);
            this.txtApellido.KeyPress += new KeyPressEventHandler(this.txtApellido_KeyPress);

            // Email
            this.lblEmail.Location = new Point(30, 140);
            this.lblEmail.Size = new Size(70, 20);
            this.lblEmail.Text = "Email:";
            this.txtEmail.Location = new Point(110, 140);
            this.txtEmail.Size = new Size(220, 23);

            // Contraseña
            this.lblPassword.Location = new Point(30, 175);
            this.lblPassword.Size = new Size(80, 20);
            this.lblPassword.Text = "Contraseña:";
            this.txtPassword.Location = new Point(110, 175);
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new Size(220, 23);

            // Rol
            this.lblRol.Location = new Point(30, 210);
            this.lblRol.Size = new Size(60, 20);
            this.lblRol.Text = "Rol:";
            this.cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbRol.Items.AddRange(new object[] { "Administrador", "Profesor", "Recepcionista" });
            this.cmbRol.Location = new Point(110, 210);
            this.cmbRol.Size = new Size(220, 23);

            // ============================================================
            // 🔹 BOTONES DE ACCIÓN
            // ============================================================

            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.Location = new Point(360, 70);
            this.btnAgregar.Size = new Size(100, 28);
            this.btnAgregar.Click += new EventHandler(this.btnAgregar_Click);

            this.btnEditar.Text = "Editar";
            this.btnEditar.Location = new Point(360, 110);
            this.btnEditar.Size = new Size(100, 28);
            this.btnEditar.Click += new EventHandler(this.btnEditar_Click);

            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.Location = new Point(360, 150);
            this.btnLimpiar.Size = new Size(100, 28);
            this.btnLimpiar.Click += new EventHandler(this.btnLimpiar_Click);

            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.Location = new Point(360, 190);
            this.btnEliminar.Size = new Size(100, 28);
            this.btnEliminar.Click += new EventHandler(this.btnEliminar_Click);

            // ============================================================
            // 🔹 BUSCADOR (Campo + Combo)
            // ============================================================

            this.lblBuscar.Location = new Point(30, 255);
            this.lblBuscar.Size = new Size(60, 20);
            this.lblBuscar.Text = "Buscar:";

            // Combo “Buscar por”
            this.cboBuscarPor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboBuscarPor.Items.AddRange(new object[] { "Nombre", "Rol", "ID" });
            this.cboBuscarPor.Location = new Point(90, 253);
            this.cboBuscarPor.Size = new Size(120, 23);

            // Campo de texto de búsqueda
            this.txtBuscar.Location = new Point(220, 253);
            this.txtBuscar.Size = new Size(240, 23);
            this.txtBuscar.TextChanged += new EventHandler(this.txtBuscar_TextChanged);

            // ============================================================
            // 🔹 TABLA DE USUARIOS
            // ============================================================

            this.dgvUsuarios.AllowUserToAddRows = false;
            this.dgvUsuarios.Anchor = (AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right);
            this.dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsuarios.BackgroundColor = Color.White;
            this.dgvUsuarios.Location = new Point(30, 290);
            this.dgvUsuarios.Name = "dgvUsuarios";
            this.dgvUsuarios.ReadOnly = true;
            this.dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsuarios.Size = new Size(720, 330);

            // Evento para selección y clic en celda “Estado”
            this.dgvUsuarios.SelectionChanged += new EventHandler(this.dgvUsuarios_SelectionChanged);
            this.dgvUsuarios.CellClick += new DataGridViewCellEventHandler(this.dgvUsuarios_CellClick);

            // ============================================================
            // 🔹 CONFIGURACIÓN FINAL DEL USERCONTROL
            // ============================================================

            this.BackColor = Color.White;
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblNombre);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.lblApellido);
            this.Controls.Add(this.txtApellido);
            this.Controls.Add(this.lblEmail);
            this.Controls.Add(this.txtEmail);
            this.Controls.Add(this.lblPassword);
            this.Controls.Add(this.txtPassword);
            this.Controls.Add(this.lblRol);
            this.Controls.Add(this.cmbRol);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.lblBuscar);
            this.Controls.Add(this.cboBuscarPor);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.dgvUsuarios);

            this.Name = "UcGestionUsuarios";
            this.Size = new Size(780, 650);

            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
