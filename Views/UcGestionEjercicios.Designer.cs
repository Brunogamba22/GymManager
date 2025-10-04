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
            this.dgvEjercicios = new DataGridView();

            ((System.ComponentModel.ISupportInitialize)(this.dgvEjercicios)).BeginInit();
            this.SuspendLayout();

            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
            this.lblTitulo.Location = new Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new Size(193, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Gestión de Ejercicios";

            // 
            // txtNombre
            // 
            this.txtNombre.Location = new Point(25, 60);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new Size(220, 23);
            this.txtNombre.TabIndex = 1;
            this.txtNombre.KeyPress += new KeyPressEventHandler(this.txtNombre_KeyPress);

            // 
            // cmbMusculo
            // 
            this.cmbMusculo.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbMusculo.Location = new Point(25, 95);
            this.cmbMusculo.Name = "cmbMusculo";
            this.cmbMusculo.Size = new Size(220, 23);
            this.cmbMusculo.TabIndex = 2;

            // 
            // lblImagen
            // 
            this.lblImagen.AutoSize = true;
            this.lblImagen.Location = new Point(25, 130);
            this.lblImagen.Name = "lblImagen";
            this.lblImagen.Size = new Size(94, 15);
            this.lblImagen.TabIndex = 3;
            this.lblImagen.Text = "Ruta de imagen:";

            // 
            // txtImagen
            // 
            this.txtImagen.Location = new Point(25, 150);
            this.txtImagen.Name = "txtImagen";
            this.txtImagen.Size = new Size(220, 23);
            this.txtImagen.TabIndex = 4;

            // 
            // btnAgregar
            // 
            this.btnAgregar.Location = new Point(270, 55);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new Size(100, 30);
            this.btnAgregar.TabIndex = 5;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);

            // 
            // btnEditar
            // 
            this.btnEditar.Location = new Point(270, 90);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new Size(100, 30);
            this.btnEditar.TabIndex = 6;
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);

            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new Point(270, 125);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new Size(100, 30);
            this.btnEliminar.TabIndex = 7;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);

            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new Point(270, 160);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new Size(100, 30);
            this.btnLimpiar.TabIndex = 8;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.UseVisualStyleBackColor = true;
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);

            // 
            // lblBuscar
            // 
            this.lblBuscar.AutoSize = true;
            this.lblBuscar.Location = new Point(25, 200);
            this.lblBuscar.Name = "lblBuscar";
            this.lblBuscar.Size = new Size(45, 15);
            this.lblBuscar.TabIndex = 9;
            this.lblBuscar.Text = "Buscar:";

            // 
            // txtBuscar
            // 
            this.txtBuscar.Location = new Point(75, 197);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new Size(170, 23);
            this.txtBuscar.TabIndex = 10;
            this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);

            // 
            // dgvEjercicios
            // 
            this.dgvEjercicios.AllowUserToAddRows = false;
            this.dgvEjercicios.AllowUserToDeleteRows = false;
            this.dgvEjercicios.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEjercicios.BackgroundColor = Color.White;
            this.dgvEjercicios.Location = new Point(25, 235);
            this.dgvEjercicios.MultiSelect = false;
            this.dgvEjercicios.Name = "dgvEjercicios";
            this.dgvEjercicios.ReadOnly = true;
            this.dgvEjercicios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvEjercicios.Size = new Size(700, 280);
            this.dgvEjercicios.TabIndex = 11;
            this.dgvEjercicios.SelectionChanged += new System.EventHandler(this.dgvEjercicios_SelectionChanged);

            // 
            // UcGestionEjercicios
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.WhiteSmoke;
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.cmbMusculo);
            this.Controls.Add(this.lblImagen);
            this.Controls.Add(this.txtImagen);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.lblBuscar);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.dgvEjercicios);
            this.Name = "UcGestionEjercicios";
            this.Size = new Size(760, 540);

            ((System.ComponentModel.ISupportInitialize)(this.dgvEjercicios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
