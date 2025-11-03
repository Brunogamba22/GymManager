using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Views
{
    partial class UcGestionEjercicios
    {
        private System.ComponentModel.IContainer components = null;

        // Controles de la interfaz
        private Label lblTitulo;
        private TextBox txtNombre;
        private ComboBox cmbMusculo;
        private TextBox txtImagen;
        private TextBox txtBuscar;
        private Label lblBuscar;
        private Label lblImagen;
        private Button btnAgregar;
        private Button btnEditar;
        private Button btnEliminar;
        private Button btnLimpiar;
        private DataGridView dgvEjercicios;
        private PictureBox pictureBoxEjercicio;
        private Button btnSeleccionarImagen;
        private ComboBox cmbTipoBusqueda;
        private Label lblTipoBusqueda;
        private ComboBox cmbEstado;
        private Label lblEstado;

        // Paneles para agrupar controles
        private Panel panelFormulario;
        private Panel panelBusqueda;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new Label();
            this.txtNombre = new TextBox();
            this.cmbMusculo = new ComboBox();
            this.txtImagen = new TextBox();
            this.lblImagen = new Label();
            this.txtBuscar = new TextBox();
            this.lblBuscar = new Label();
            this.btnAgregar = new Button();
            this.btnEditar = new Button();
            this.btnEliminar = new Button();
            this.btnLimpiar = new Button();
            this.btnSeleccionarImagen = new Button();
            this.dgvEjercicios = new DataGridView();
            this.pictureBoxEjercicio = new PictureBox();
            this.cmbTipoBusqueda = new ComboBox();
            this.lblTipoBusqueda = new Label();
            this.cmbEstado = new ComboBox();
            this.lblEstado = new Label();
            this.panelFormulario = new Panel();
            this.panelBusqueda = new Panel();

            ((System.ComponentModel.ISupportInitialize)(this.dgvEjercicios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEjercicio)).BeginInit();
            this.panelFormulario.SuspendLayout();
            this.panelBusqueda.SuspendLayout();
            this.SuspendLayout();

            // ==================== TÍTULO ====================
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
            this.lblTitulo.Location = new Point(25, 20);
            this.lblTitulo.Text = "Gestión de Ejercicios";
            this.lblTitulo.ForeColor = Color.FromArgb(52, 73, 94);

            // ==================== PANEL FORMULARIO ====================
            this.panelFormulario.BackColor = Color.White;
            this.panelFormulario.BorderStyle = BorderStyle.FixedSingle;
            this.panelFormulario.Location = new Point(25, 70);
            this.panelFormulario.Size = new Size(350, 220);

            // txtNombre
            this.txtNombre.Font = new Font("Segoe UI", 10F);
            this.txtNombre.Location = new Point(20, 30);
            this.txtNombre.Size = new Size(300, 26);
            this.txtNombre.BorderStyle = BorderStyle.FixedSingle;
            this.txtNombre.KeyPress += new KeyPressEventHandler(this.txtNombre_KeyPress);

            // cmbMusculo
            this.cmbMusculo.Font = new Font("Segoe UI", 10F);
            this.cmbMusculo.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMusculo.Location = new Point(20, 75);
            this.cmbMusculo.Size = new Size(300, 28);
            this.cmbMusculo.FlatStyle = FlatStyle.Flat;

            // lblImagen
            this.lblImagen.AutoSize = true;
            this.lblImagen.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblImagen.Location = new Point(20, 120);
            this.lblImagen.Text = "Ruta de imagen:";
            this.lblImagen.ForeColor = Color.FromArgb(52, 73, 94);

            // txtImagen
            this.txtImagen.Font = new Font("Segoe UI", 10F);
            this.txtImagen.Location = new Point(20, 140);
            this.txtImagen.Size = new Size(195, 26);
            this.txtImagen.BorderStyle = BorderStyle.FixedSingle;

            // btnSeleccionarImagen
            this.btnSeleccionarImagen.Text = "Seleccionar...";
            this.btnSeleccionarImagen.Location = new Point(225, 140);
            this.btnSeleccionarImagen.Size = new Size(95, 26);
            this.btnSeleccionarImagen.Click += new EventHandler(this.btnSeleccionarImagen_Click);

            // ==================== PICTURE BOX ====================
            this.pictureBoxEjercicio.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBoxEjercicio.Location = new Point(400, 70);
            this.pictureBoxEjercicio.Size = new Size(180, 180);
            this.pictureBoxEjercicio.SizeMode = PictureBoxSizeMode.Zoom;
            this.pictureBoxEjercicio.BackColor = Color.White;

            // ==================== BOTONES ====================
            this.btnAgregar.Location = new Point(600, 70);
            this.btnEditar.Location = new Point(600, 115);
            this.btnEliminar.Location = new Point(600, 160);
            this.btnLimpiar.Location = new Point(600, 205);
            this.btnAgregar.Size = this.btnEditar.Size = this.btnEliminar.Size = this.btnLimpiar.Size = new Size(100, 35);

            this.btnAgregar.Text = "Agregar";
            this.btnEditar.Text = "Editar";
            this.btnEliminar.Text = "Eliminar";
            this.btnLimpiar.Text = "Limpiar";

            this.btnAgregar.Click += new EventHandler(this.btnAgregar_Click);
            this.btnEditar.Click += new EventHandler(this.btnEditar_Click);
            this.btnEliminar.Click += new EventHandler(this.btnEliminar_Click);
            this.btnLimpiar.Click += new EventHandler(this.btnLimpiar_Click);

            // ==================== PANEL BÚSQUEDA ====================
            this.panelBusqueda.BackColor = Color.White;
            this.panelBusqueda.BorderStyle = BorderStyle.FixedSingle;
            this.panelBusqueda.Location = new Point(25, 310);
            this.panelBusqueda.Size = new Size(860, 50);

            // lblTipoBusqueda
            this.lblTipoBusqueda.AutoSize = true;
            this.lblTipoBusqueda.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblTipoBusqueda.Location = new Point(20, 15);
            this.lblTipoBusqueda.Text = "Buscar por:";
            this.lblTipoBusqueda.ForeColor = Color.FromArgb(52, 73, 94);

            // cmbTipoBusqueda
            this.cmbTipoBusqueda.Font = new Font("Segoe UI", 10F);
            this.cmbTipoBusqueda.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbTipoBusqueda.Location = new Point(95, 12);
            this.cmbTipoBusqueda.Size = new Size(150, 25);
            this.cmbTipoBusqueda.FlatStyle = FlatStyle.Flat;
            this.cmbTipoBusqueda.Items.AddRange(new object[] {
                    "Todos",
                    "ID",
                    "Nombre",
                    "Grupo Muscular"
                });
            this.cmbTipoBusqueda.SelectedIndex = 0;
            this.cmbTipoBusqueda.SelectedIndexChanged += new EventHandler(this.cmbTipoBusqueda_SelectedIndexChanged);

            // lblBuscar
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblBuscar.Location = new Point(260, 15);
            this.lblBuscar.Text = "Buscar:";
            this.lblBuscar.ForeColor = Color.FromArgb(52, 73, 94);

            // txtBuscar
            this.txtBuscar.Font = new Font("Segoe UI", 10F);
            this.txtBuscar.Location = new Point(310, 12);
            this.txtBuscar.Size = new Size(200, 25);
            this.txtBuscar.BorderStyle = BorderStyle.FixedSingle;
            this.txtBuscar.TextChanged += new EventHandler(this.txtBuscar_TextChanged);
            this.txtBuscar.KeyPress += new KeyPressEventHandler(this.txtBuscar_KeyPress);

            // lblEstado
            this.lblEstado.AutoSize = true;
            this.lblEstado.Font = new Font("Segoe UI", 9F, FontStyle.Regular);
            this.lblEstado.Location = new Point(530, 15);
            this.lblEstado.Text = "Estado:";
            this.lblEstado.ForeColor = Color.FromArgb(52, 73, 94);

            // cmbEstado
            this.cmbEstado.Font = new Font("Segoe UI", 10F);
            this.cmbEstado.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbEstado.Location = new Point(585, 12);
            this.cmbEstado.Size = new Size(120, 25);
            this.cmbEstado.FlatStyle = FlatStyle.Flat;
            this.cmbEstado.Items.AddRange(new object[] { "Todos", "Activos", "Inactivos" });
            this.cmbEstado.SelectedIndex = 1;
            this.cmbEstado.SelectedIndexChanged += (s, e) => RefrescarGrid();

            // ==================== DATA GRID VIEW ====================
            this.dgvEjercicios.Location = new Point(25, 375);
            this.dgvEjercicios.Size = new Size(860, 230);
            this.dgvEjercicios.ReadOnly = true;
            this.dgvEjercicios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvEjercicios.SelectionChanged += new EventHandler(this.dgvEjercicios_SelectionChanged);
            this.dgvEjercicios.CellMouseEnter += dgvEjercicios_CellMouseEnter;
            this.dgvEjercicios.CellClick += new DataGridViewCellEventHandler(this.dgvEjercicios_CellClick);

            // ==================== USER CONTROL ====================
            this.Controls.AddRange(new Control[] {
                    lblTitulo,
                    panelFormulario,
                    pictureBoxEjercicio,
                    btnAgregar, btnEditar, btnEliminar, btnLimpiar,
                    panelBusqueda,
                    dgvEjercicios
                });

            // Agregar controles al panel formulario
            this.panelFormulario.Controls.AddRange(new Control[] {
                    txtNombre, cmbMusculo, lblImagen, txtImagen, btnSeleccionarImagen
                });

            // Agregar controles al panel búsqueda
            this.panelBusqueda.Controls.AddRange(new Control[] {
                    lblTipoBusqueda, cmbTipoBusqueda, lblBuscar, txtBuscar, lblEstado, cmbEstado
                });

            this.Size = new Size(900, 630);
            this.BackColor = Color.FromArgb(245, 247, 250);

            ((System.ComponentModel.ISupportInitialize)(this.dgvEjercicios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEjercicio)).EndInit();
            this.panelFormulario.ResumeLayout(false);
            this.panelFormulario.PerformLayout();
            this.panelBusqueda.ResumeLayout(false);
            this.panelBusqueda.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}