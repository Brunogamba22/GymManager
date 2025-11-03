using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Views
{
    partial class UcGestionUsuarios
    {
        private System.ComponentModel.IContainer components = null;

        private TextBox txtNombre;
        private TextBox txtApellido;
        private TextBox txtEmail;
        private TextBox txtPassword;
        private ComboBox cmbRol;
        private Button btnAgregar;
        private Button btnEditar;
        private Button btnEliminar;
        private Button btnLimpiar;
        private DataGridView dgvUsuarios;
        private TextBox txtBuscar;
        private Label lblTitulo;
        private Label lblNombre;
        private Label lblApellido;
        private Label lblEmail;
        private Label lblPassword;
        private Label lblRol;
        private Label lblBuscar;
        private ComboBox cboBuscarPor;

        // Paneles para agrupar controles
        private Panel panelFormulario;
        private Panel panelBusqueda;

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
            this.panelFormulario = new Panel();
            this.panelBusqueda = new Panel();

            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).BeginInit();
            this.panelFormulario.SuspendLayout();
            this.panelBusqueda.SuspendLayout();
            this.SuspendLayout();

            // ==================== TÍTULO ====================
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblTitulo.Location = new Point(25, 20);
            this.lblTitulo.Text = "👥 Gestión de Usuarios";
            this.lblTitulo.ForeColor = Color.FromArgb(52, 73, 94);

            // ==================== PANEL FORMULARIO ====================
            this.panelFormulario.BackColor = Color.White;
            this.panelFormulario.BorderStyle = BorderStyle.FixedSingle;
            this.panelFormulario.Location = new Point(25, 65);
            this.panelFormulario.Size = new Size(380, 180); // 🔹 ANCHO AUMENTADO

            // 🔹 AJUSTES DE POSICIÓN Y TAMAÑO PARA QUE ENTRE "CONTRASEÑA"
            this.lblNombre.Location = new Point(20, 20);
            this.lblNombre.Size = new Size(65, 20); // 🔹 TAMAÑO FIJO
            this.lblNombre.Text = "Nombre:";
            this.lblNombre.ForeColor = Color.FromArgb(52, 73, 94);

            this.txtNombre.Location = new Point(95, 20); // 🔹 POSICIÓN AJUSTADA
            this.txtNombre.Size = new Size(250, 23); // 🔹 ANCHO AUMENTADO
            this.txtNombre.BorderStyle = BorderStyle.FixedSingle;
            this.txtNombre.KeyPress += new KeyPressEventHandler(this.txtNombre_KeyPress);

            this.lblApellido.Location = new Point(20, 55);
            this.lblApellido.Size = new Size(65, 20);
            this.lblApellido.Text = "Apellido:";
            this.lblApellido.ForeColor = Color.FromArgb(52, 73, 94);

            this.txtApellido.Location = new Point(95, 55);
            this.txtApellido.Size = new Size(250, 23);
            this.txtApellido.BorderStyle = BorderStyle.FixedSingle;
            this.txtApellido.KeyPress += new KeyPressEventHandler(this.txtApellido_KeyPress);

            this.lblEmail.Location = new Point(20, 90);
            this.lblEmail.Size = new Size(65, 20);
            this.lblEmail.Text = "Email:";
            this.lblEmail.ForeColor = Color.FromArgb(52, 73, 94);

            this.txtEmail.Location = new Point(95, 90);
            this.txtEmail.Size = new Size(250, 23);
            this.txtEmail.BorderStyle = BorderStyle.FixedSingle;

            this.lblPassword.Location = new Point(20, 125);
            this.lblPassword.Size = new Size(70, 20); // 🔹 TAMAÑO AJUSTADO
            this.lblPassword.Text = "Contraseña:";
            this.lblPassword.ForeColor = Color.FromArgb(52, 73, 94);

            this.txtPassword.Location = new Point(95, 125); // 🔹 POSICIÓN ALINEADA
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new Size(250, 23); // 🔹 ANCHO AUMENTADO
            this.txtPassword.BorderStyle = BorderStyle.FixedSingle;

            this.lblRol.Location = new Point(20, 160);
            this.lblRol.Size = new Size(65, 20);
            this.lblRol.Text = "Rol:";
            this.lblRol.ForeColor = Color.FromArgb(52, 73, 94);

            this.cmbRol.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbRol.Items.AddRange(new object[] { "Administrador", "Profesor", "Recepcionista" });
            this.cmbRol.Location = new Point(95, 160);
            this.cmbRol.Size = new Size(250, 23); // 🔹 ANCHO AUMENTADO
            this.cmbRol.FlatStyle = FlatStyle.Flat;

            // ==================== BOTONES ====================
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.Location = new Point(420, 75); // 🔹 POSICIÓN AJUSTADA
            this.btnAgregar.Size = new Size(100, 35);

            this.btnEditar.Text = "Editar";
            this.btnEditar.Location = new Point(420, 120);
            this.btnEditar.Size = new Size(100, 35);

            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.Location = new Point(420, 165);
            this.btnLimpiar.Size = new Size(100, 35);

            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.Location = new Point(420, 210);
            this.btnEliminar.Size = new Size(100, 35);

            this.btnAgregar.Click += new EventHandler(this.btnAgregar_Click);
            this.btnEditar.Click += new EventHandler(this.btnEditar_Click);
            this.btnLimpiar.Click += new EventHandler(this.btnLimpiar_Click);
            this.btnEliminar.Click += new EventHandler(this.btnEliminar_Click);

            // ==================== PANEL BÚSQUEDA ====================
            this.panelBusqueda.BackColor = Color.White;
            this.panelBusqueda.BorderStyle = BorderStyle.FixedSingle;
            this.panelBusqueda.Location = new Point(25, 260); // 🔹 POSICIÓN MÁS ARRIBA
            this.panelBusqueda.Size = new Size(860, 45);

            this.lblBuscar.Location = new Point(20, 12);
            this.lblBuscar.Size = new Size(60, 20);
            this.lblBuscar.Text = "Buscar:";
            this.lblBuscar.ForeColor = Color.FromArgb(52, 73, 94);

            this.cboBuscarPor.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cboBuscarPor.Items.AddRange(new object[] { "Nombre", "Rol", "ID" });
            this.cboBuscarPor.Location = new Point(85, 10);
            this.cboBuscarPor.Size = new Size(120, 23);
            this.cboBuscarPor.FlatStyle = FlatStyle.Flat;

            this.txtBuscar.Location = new Point(215, 10);
            this.txtBuscar.Size = new Size(240, 23);
            this.txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            this.txtBuscar.TextChanged += new EventHandler(this.txtBuscar_TextChanged);

            // ==================== DATA GRID VIEW ====================
            this.dgvUsuarios.Location = new Point(25, 320); // 🔹 POSICIÓN MÁS ARRIBA
            this.dgvUsuarios.Size = new Size(860, 250); // 🔹 ALTURA REDUCIDA
            this.dgvUsuarios.ReadOnly = true;
            this.dgvUsuarios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvUsuarios.RowTemplate.Height = 30; // 🔹 FILAS MÁS COMPACTAS
            this.dgvUsuarios.SelectionChanged += new EventHandler(this.dgvUsuarios_SelectionChanged);
            this.dgvUsuarios.CellClick += new DataGridViewCellEventHandler(this.dgvUsuarios_CellClick);

            // ==================== USER CONTROL ====================
            this.Controls.AddRange(new Control[] {
                    lblTitulo,
                    panelFormulario,
                    btnAgregar, btnEditar, btnEliminar, btnLimpiar,
                    panelBusqueda,
                    dgvUsuarios
                });

            // Agregar controles al panel formulario
            this.panelFormulario.Controls.AddRange(new Control[] {
                    lblNombre, txtNombre,
                    lblApellido, txtApellido,
                    lblEmail, txtEmail,
                    lblPassword, txtPassword,
                    lblRol, cmbRol
                });

            // Agregar controles al panel búsqueda
            this.panelBusqueda.Controls.AddRange(new Control[] {
                    lblBuscar, cboBuscarPor, txtBuscar
                });

            this.Size = new Size(900, 600); // 🔹 ALTURA TOTAL REDUCIDA
            this.BackColor = Color.FromArgb(245, 247, 250);

            ((System.ComponentModel.ISupportInitialize)(this.dgvUsuarios)).EndInit();
            this.panelFormulario.ResumeLayout(false);
            this.panelFormulario.PerformLayout();
            this.panelBusqueda.ResumeLayout(false);
            this.panelBusqueda.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}