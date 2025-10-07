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
        private TextBox txtPassword;           // Campo para contraseña
        private ComboBox cmbRol;               // Combo para seleccionar rol (Admin, Profesor, Recepcionista)
        private Button btnAgregar;             // Botón para agregar usuario
        private Button btnEditar;              // Botón para editar usuario
        private Button btnEliminar;            // Botón para eliminar usuario
        private Button btnLimpiar;             // Botón para limpiar formulario
        private DataGridView dgvUsuarios;      // Grilla donde se listan los usuarios
        private TextBox txtBuscar;             // Campo de búsqueda
        private Label lblTitulo;               // Título principal del formulario
        private Label lblNombre;               // Etiqueta “Nombre”
        private Label lblApellido;             // Etiqueta “Apellido”
        private Label lblEmail;                // Etiqueta “Email”
        private Label lblPassword;             // Etiqueta “Contraseña”
        private Label lblRol;                  // Etiqueta “Rol”
        private Label lblBuscar;               // Etiqueta “Buscar :”

        // ------------------------------------------------------------
        // MÉTODO Dispose: libera recursos de los controles
        // ------------------------------------------------------------
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose(); // Libera la memoria usada por los componentes
            }
            base.Dispose(disposing);
        }

        // ------------------------------------------------------------
        // MÉTODO DE INICIALIZACIÓN DE COMPONENTES (diseño visual)
        // ------------------------------------------------------------
        private void InitializeComponent()
        {
            // Inicialización de los controles
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

            // ------------------------------------------------------------
            // CONFIGURACIÓN GENERAL DEL CONTROL PRINCIPAL
            // ------------------------------------------------------------
            this.BackColor = System.Drawing.Color.White;      // Fondo blanco
            this.Name = "UcGestionUsuarios";                  // Nombre lógico del control
            this.Size = new System.Drawing.Size(600, 536);    // Tamaño del control

            // ------------------------------------------------------------
            // 🔹 LABEL TÍTULO
            // ------------------------------------------------------------
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14.25F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(25, 27);
            this.lblTitulo.Text = "👥 Gestión de Usuarios";

            // ------------------------------------------------------------
            // 🔹 LABEL NOMBRE
            // ------------------------------------------------------------
            this.lblNombre.Text = "Nombre:";
            this.lblNombre.Location = new System.Drawing.Point(23, 74);
            this.lblNombre.Size = new System.Drawing.Size(74, 23);

            // 🔹 CAMPO NOMBRE
            this.txtNombre.Location = new System.Drawing.Point(103, 74);
            this.txtNombre.Size = new System.Drawing.Size(200, 20);
            this.txtNombre.KeyPress += new KeyPressEventHandler(this.txtNombre_KeyPress);

            // ------------------------------------------------------------
            // 🔹 LABEL APELLIDO
            // ------------------------------------------------------------
            this.lblApellido.Text = "Apellido:";
            this.lblApellido.Location = new System.Drawing.Point(23, 104);
            this.lblApellido.Size = new System.Drawing.Size(74, 23);

            // 🔹 CAMPO APELLIDO
            this.txtApellido.Location = new System.Drawing.Point(103, 104);
            this.txtApellido.Size = new System.Drawing.Size(200, 20);
            this.txtApellido.KeyPress += new KeyPressEventHandler(this.txtApellido_KeyPress);

            // ------------------------------------------------------------
            // 🔹 LABEL EMAIL
            // ------------------------------------------------------------
            this.lblEmail.Text = "Email:";
            this.lblEmail.Location = new System.Drawing.Point(23, 134);
            this.lblEmail.Size = new System.Drawing.Size(60, 23);

            // 🔹 CAMPO EMAIL
            this.txtEmail.Location = new System.Drawing.Point(103, 134);
            this.txtEmail.Size = new System.Drawing.Size(200, 20);

            // ------------------------------------------------------------
            // 🔹 LABEL PASSWORD
            // ------------------------------------------------------------
            this.lblPassword.Text = "Contraseña:";
            this.lblPassword.Location = new System.Drawing.Point(23, 164);
            this.lblPassword.Size = new System.Drawing.Size(74, 23);

            // 🔹 CAMPO PASSWORD
            this.txtPassword.Location = new System.Drawing.Point(103, 164);
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(200, 20);

            // ------------------------------------------------------------
            // 🔹 LABEL ROL
            // ------------------------------------------------------------
            this.lblRol.Text = "Rol:";
            this.lblRol.Location = new System.Drawing.Point(23, 194);
            this.lblRol.Size = new System.Drawing.Size(60, 23);

            // 🔹 COMBO ROL
            this.cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbRol.Items.AddRange(new object[] { "Administrador", "Profesor", "Recepcionista" });
            this.cmbRol.Location = new System.Drawing.Point(103, 194);
            this.cmbRol.Size = new System.Drawing.Size(200, 21);

            // ------------------------------------------------------------
            // 🔹 BOTONES CRUD
            // ------------------------------------------------------------
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.Location = new System.Drawing.Point(333, 74);
            this.btnAgregar.Size = new System.Drawing.Size(75, 23);
            this.btnAgregar.Click += new EventHandler(this.btnAgregar_Click);

            this.btnEditar.Text = "Editar";
            this.btnEditar.Location = new System.Drawing.Point(333, 114);
            this.btnEditar.Size = new System.Drawing.Size(75, 23);
            this.btnEditar.Click += new EventHandler(this.btnEditar_Click);

            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.Location = new System.Drawing.Point(333, 194);
            this.btnEliminar.Size = new System.Drawing.Size(75, 23);
            this.btnEliminar.Click += new EventHandler(this.btnEliminar_Click);

            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.Location = new System.Drawing.Point(333, 159);
            this.btnLimpiar.Size = new System.Drawing.Size(75, 23);
            this.btnLimpiar.Click += new EventHandler(this.btnLimpiar_Click);

            // ------------------------------------------------------------
            // 🔹 LABEL BUSCAR
            // ------------------------------------------------------------
            this.lblBuscar.Text = "Buscar :";
            this.lblBuscar.Location = new System.Drawing.Point(23, 243);
            this.lblBuscar.AutoSize = true;

            // 🔹 CAMPO BUSCAR
            this.txtBuscar.Location = new System.Drawing.Point(75, 240);
            this.txtBuscar.Size = new System.Drawing.Size(130, 20);
            this.txtBuscar.TextChanged += new EventHandler(this.txtBuscar_TextChanged);

            // ------------------------------------------------------------
            // 🔹 GRILLA DE USUARIOS
            // ------------------------------------------------------------
            this.dgvUsuarios.AllowUserToAddRows = false;
            this.dgvUsuarios.ReadOnly = true;
            this.dgvUsuarios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsuarios.Location = new System.Drawing.Point(26, 266);
            this.dgvUsuarios.Size = new System.Drawing.Size(400, 250);
            this.dgvUsuarios.SelectionChanged += new EventHandler(this.dgvUsuarios_SelectionChanged);

            // ------------------------------------------------------------
            // 🔹 AGREGAR TODOS LOS CONTROLES AL FORM
            // ------------------------------------------------------------
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
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.lblBuscar);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.dgvUsuarios);
        }
    }
}
