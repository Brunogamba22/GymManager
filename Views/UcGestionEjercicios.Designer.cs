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

            ((System.ComponentModel.ISupportInitialize)(this.dgvEjercicios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEjercicio)).BeginInit();
            this.SuspendLayout();

            // lblTitulo
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new Font("Segoe UI", 16F, FontStyle.Bold);
            this.lblTitulo.Location = new Point(25, 20);
            this.lblTitulo.Text = "Gestión de Ejercicios";

            // txtNombre
            this.txtNombre.Font = new Font("Segoe UI", 10F);
            this.txtNombre.Location = new Point(30, 70);
            this.txtNombre.Size = new Size(260, 25);
            this.txtNombre.KeyPress += new KeyPressEventHandler(this.txtNombre_KeyPress);

            // cmbMusculo
            this.cmbMusculo.Font = new Font("Segoe UI", 10F);
            this.cmbMusculo.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMusculo.Location = new Point(30, 105);
            this.cmbMusculo.Size = new Size(260, 25);

            // lblImagen
            this.lblImagen.AutoSize = true;
            this.lblImagen.Font = new Font("Segoe UI", 9F);
            this.lblImagen.Location = new Point(30, 140);
            this.lblImagen.Text = "Ruta de imagen:";

            // txtImagen
            this.txtImagen.Font = new Font("Segoe UI", 10F);
            this.txtImagen.Location = new Point(30, 160);
            this.txtImagen.Size = new Size(260, 25);

            // btnSeleccionarImagen
            this.btnSeleccionarImagen.Text = "Seleccionar...";
            this.btnSeleccionarImagen.Location = new Point(300, 160);
            this.btnSeleccionarImagen.Size = new Size(90, 28);
            this.btnSeleccionarImagen.Click += new EventHandler(this.btnSeleccionarImagen_Click);

            // pictureBoxEjercicio
            this.pictureBoxEjercicio.BorderStyle = BorderStyle.FixedSingle;
            this.pictureBoxEjercicio.Location = new Point(420, 60);
            this.pictureBoxEjercicio.Size = new Size(150, 120);
            this.pictureBoxEjercicio.SizeMode = PictureBoxSizeMode.Zoom;

            // Botones
            this.btnAgregar.Location = new Point(600, 60);
            this.btnEditar.Location = new Point(600, 100);
            this.btnEliminar.Location = new Point(600, 140);
            this.btnLimpiar.Location = new Point(600, 180);
            this.btnAgregar.Size = this.btnEditar.Size = this.btnEliminar.Size = this.btnLimpiar.Size = new Size(120, 35);

            this.btnAgregar.Text = "Agregar";
            this.btnEditar.Text = "Editar";
            this.btnEliminar.Text = "Eliminar";
            this.btnLimpiar.Text = "Limpiar";

            this.btnAgregar.Click += new EventHandler(this.btnAgregar_Click);
            this.btnEditar.Click += new EventHandler(this.btnEditar_Click);
            this.btnEliminar.Click += new EventHandler(this.btnEliminar_Click);
            this.btnLimpiar.Click += new EventHandler(this.btnLimpiar_Click);

            // Buscar
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.Font = new Font("Segoe UI", 9F);
            this.lblBuscar.Location = new Point(30, 205);
            this.lblBuscar.Text = "Buscar:";

            this.txtBuscar.Font = new Font("Segoe UI", 10F);
            this.txtBuscar.Location = new Point(90, 202);
            this.txtBuscar.Size = new Size(200, 25);
            this.txtBuscar.TextChanged += new EventHandler(this.txtBuscar_TextChanged);

            // DataGridView
            this.dgvEjercicios.Location = new Point(30, 240);
            this.dgvEjercicios.Size = new Size(740, 320);
            this.dgvEjercicios.ReadOnly = true;
            this.dgvEjercicios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvEjercicios.SelectionChanged += new EventHandler(this.dgvEjercicios_SelectionChanged);

            //
            this.dgvEjercicios.CellMouseEnter += dgvEjercicios_CellMouseEnter;

            // UserControl
            this.BackColor = Color.WhiteSmoke;
            this.Controls.AddRange(new Control[] {
        lblTitulo, txtNombre, cmbMusculo, lblImagen, txtImagen, btnSeleccionarImagen,
        pictureBoxEjercicio, btnAgregar, btnEditar, btnEliminar, btnLimpiar,
        lblBuscar, txtBuscar, dgvEjercicios
    });
            this.Size = new Size(800, 600);

            ((System.ComponentModel.ISupportInitialize)(this.dgvEjercicios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxEjercicio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

    }
}
