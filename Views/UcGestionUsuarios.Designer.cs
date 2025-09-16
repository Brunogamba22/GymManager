namespace GymManager.Views
{
    partial class UcGestionUsuarios
    {
        private System.ComponentModel.IContainer components = null;

        // Controles
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.ComboBox cmbRol;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.DataGridView dgvUsuarios;
        private System.Windows.Forms.Label lblNombre;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.Label lblRol;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();

            // Inicialización de controles
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.cmbRol = new System.Windows.Forms.ComboBox();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.dgvUsuarios = new System.Windows.Forms.DataGridView();
            this.lblNombre = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblRol = new System.Windows.Forms.Label();

            // Diseño general del UserControl
            this.Size = new System.Drawing.Size(700, 400);
            this.BackColor = System.Drawing.Color.White;

            // Label Nombre
            lblNombre.Text = "Nombre:";
            lblNombre.Location = new System.Drawing.Point(20, 20);

            // TextBox Nombre
            txtNombre.Location = new System.Drawing.Point(100, 20);
            txtNombre.Width = 200;

            // Label Email
            lblEmail.Text = "Email:";
            lblEmail.Location = new System.Drawing.Point(20, 60);

            // TextBox Email
            txtEmail.Location = new System.Drawing.Point(100, 60);
            txtEmail.Width = 200;

            // Label Password
            lblPassword.Text = "Contraseña:";
            lblPassword.Location = new System.Drawing.Point(20, 100);

            // TextBox Password
            txtPassword.Location = new System.Drawing.Point(100, 100);
            txtPassword.Width = 200;
            txtPassword.PasswordChar = '*';

            // Label Rol
            lblRol.Text = "Rol:";
            lblRol.Location = new System.Drawing.Point(20, 140);

            // ComboBox Rol
            cmbRol.Location = new System.Drawing.Point(100, 140);
            cmbRol.Width = 200;
            cmbRol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            cmbRol.Items.AddRange(new object[] { "Administrador", "Profesor", "Recepcionista" });

            // Botón Agregar
            btnAgregar.Text = "Agregar";
            btnAgregar.Location = new System.Drawing.Point(330, 20);
            btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);

            // Botón Editar
            btnEditar.Text = "Editar";
            btnEditar.Location = new System.Drawing.Point(330, 60);
            btnEditar.Click += new System.EventHandler(this.btnEditar_Click);

            // Botón Eliminar
            btnEliminar.Text = "Eliminar";
            btnEliminar.Location = new System.Drawing.Point(330, 100);
            btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);

            // DataGridView Usuarios
            dgvUsuarios.Location = new System.Drawing.Point(20, 200);
            dgvUsuarios.Size = new System.Drawing.Size(650, 180);
            dgvUsuarios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dgvUsuarios.ReadOnly = true;
            dgvUsuarios.AllowUserToAddRows = false;
            dgvUsuarios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            dgvUsuarios.SelectionChanged += new System.EventHandler(this.dgvUsuarios_SelectionChanged);

            // Agregar controles al UserControl
            this.Controls.Add(lblNombre);
            this.Controls.Add(txtNombre);
            this.Controls.Add(lblEmail);
            this.Controls.Add(txtEmail);
            this.Controls.Add(lblPassword);
            this.Controls.Add(txtPassword);
            this.Controls.Add(lblRol);
            this.Controls.Add(cmbRol);
            this.Controls.Add(btnAgregar);
            this.Controls.Add(btnEditar);
            this.Controls.Add(btnEliminar);
            this.Controls.Add(dgvUsuarios);
        }
    }
}
