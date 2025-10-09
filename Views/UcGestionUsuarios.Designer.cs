using System;
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
        private ComboBox cboBuscarPor;         // Combo desplegable: buscar por Nombre o Rol

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
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.txtApellido = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.cmbRol = new System.Windows.Forms.ComboBox();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.btnLimpiar = new System.Windows.Forms.Button();
            this.dgvUsuarios = new System.Windows.Forms.DataGridView();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblNombre = new System.Windows.Forms.Label();
            this.lblApellido = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblRol = new System.Windows.Forms.Label();
            this.lblBuscar = new System.Windows.Forms.Label();
            this.cboBuscarPor = new System.Windows.Forms.ComboBox();

            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).BeginInit();
            this.SuspendLayout();

            // ============================================================
            // 🔹 TÍTULO PRINCIPAL
            // ============================================================
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(25, 15);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(260, 30);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "👥 Gestión de Usuarios";

            // ============================================================
            // 🔹 LABELS E INPUTS DE FORMULARIO
            // ============================================================

            // Nombre
            this.lblNombre.Location = new System.Drawing.Point(30, 70);
            this.lblNombre.Size = new System.Drawing.Size(70, 20);
            this.lblNombre.Text = "Nombre:";
            this.txtNombre.Location = new System.Drawing.Point(110, 70);
            this.txtNombre.Size = new System.Drawing.Size(220, 23);
            this.txtNombre.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNombre_KeyPress);

            // Apellido
            this.lblApellido.Location = new System.Drawing.Point(30, 105);
            this.lblApellido.Size = new System.Drawing.Size(70, 20);
            this.lblApellido.Text = "Apellido:";
            this.txtApellido.Location = new System.Drawing.Point(110, 105);
            this.txtApellido.Size = new System.Drawing.Size(220, 23);
            this.txtApellido.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtApellido_KeyPress);

            // Email
            this.lblEmail.Location = new System.Drawing.Point(30, 140);
            this.lblEmail.Size = new System.Drawing.Size(70, 20);
            this.lblEmail.Text = "Email:";
            this.txtEmail.Location = new System.Drawing.Point(110, 140);
            this.txtEmail.Size = new System.Drawing.Size(220, 23);

            // Contraseña
            this.lblPassword.Location = new System.Drawing.Point(30, 175);
            this.lblPassword.Size = new System.Drawing.Size(80, 20);
            this.lblPassword.Text = "Contraseña:";
            this.txtPassword.Location = new System.Drawing.Point(110, 175);
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(220, 23);

            // Rol
            this.lblRol.Location = new System.Drawing.Point(30, 210);
            this.lblRol.Size = new System.Drawing.Size(60, 20);
            this.lblRol.Text = "Rol:";
            this.cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbRol.Items.AddRange(new object[] { "Administrador", "Profesor", "Recepcionista" });
            this.cmbRol.Location = new System.Drawing.Point(110, 210);
            this.cmbRol.Size = new System.Drawing.Size(220, 23);

            // ============================================================
            // 🔹 BOTONES DE ACCIÓN
            // ============================================================

            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.Location = new System.Drawing.Point(360, 70);
            this.btnAgregar.Size = new System.Drawing.Size(100, 28);
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);

            this.btnEditar.Text = "Editar";
            this.btnEditar.Location = new System.Drawing.Point(360, 110);
            this.btnEditar.Size = new System.Drawing.Size(100, 28);
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);

            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.Location = new System.Drawing.Point(360, 150);
            this.btnLimpiar.Size = new System.Drawing.Size(100, 28);
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);

            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.Location = new System.Drawing.Point(360, 190);
            this.btnEliminar.Size = new System.Drawing.Size(100, 28);
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);

            // ============================================================
            // 🔹 BUSCADOR (Campo + Combo)
            // ============================================================

            this.lblBuscar.Location = new System.Drawing.Point(30, 255);
            this.lblBuscar.Size = new System.Drawing.Size(60, 20);
            this.lblBuscar.Text = "Buscar:";

            // Combo “Buscar por”
            this.cboBuscarPor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboBuscarPor.Items.AddRange(new object[] { "Nombre", "Rol" });
            this.cboBuscarPor.Location = new System.Drawing.Point(90, 253);
            this.cboBuscarPor.Size = new System.Drawing.Size(120, 23);

            // Campo de texto de búsqueda
            this.txtBuscar.Location = new System.Drawing.Point(220, 253);
            this.txtBuscar.Size = new System.Drawing.Size(240, 23);
            this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);

            // ============================================================
            // 🔹 TABLA DE USUARIOS
            // ============================================================

            this.dgvUsuarios.AllowUserToAddRows = false;
            this.dgvUsuarios.Anchor = ((AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right));
            this.dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsuarios.BackgroundColor = System.Drawing.Color.White;
            this.dgvUsuarios.Location = new System.Drawing.Point(30, 290);
            this.dgvUsuarios.Name = "dgvUsuarios";
            this.dgvUsuarios.ReadOnly = true;
            this.dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsuarios.Size = new System.Drawing.Size(720, 330);
            this.dgvUsuarios.SelectionChanged += new System.EventHandler(this.dgvUsuarios_SelectionChanged);

            // ============================================================
            // 🔹 CONFIGURACIÓN FINAL DEL USERCONTROL
            // ============================================================

            this.BackColor = System.Drawing.Color.White;
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
            this.Size = new System.Drawing.Size(780, 650); // Aumentamos tamaño del panel

            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
