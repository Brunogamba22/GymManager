using System.Windows.Forms;

namespace GymManager.Views
{
    partial class UcGestionEjercicios
    {
        private System.ComponentModel.IContainer components = null;

        // Declaración de controles
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.TextBox txtMusculo;
        private System.Windows.Forms.TextBox txtDescripcion;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.DataGridView dgvEjercicios;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary> 
        /// Diseño de la interfaz visual del UserControl
        /// </summary>
        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.txtMusculo = new System.Windows.Forms.TextBox();
            this.txtDescripcion = new System.Windows.Forms.TextBox();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.dgvEjercicios = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEjercicios)).BeginInit();
            this.SuspendLayout();

            // 
            // lblTitulo
            // Título principal del panel
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(250, 25);
            this.lblTitulo.Text = "Gestión de Ejercicios";

            // 
            // txtNombre
            // Campo para ingresar el nombre del ejercicio
            this.txtNombre.Location = new System.Drawing.Point(25, 60);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(200, 20);
            //this.txtNombre.PlaceholderText = "Nombre del ejercicio";

            // 
            // txtMusculo
            // Campo para ingresar el músculo trabajado
            this.txtMusculo.Location = new System.Drawing.Point(25, 90);
            this.txtMusculo.Name = "txtMusculo";
            this.txtMusculo.Size = new System.Drawing.Size(200, 20);
            //this.txtMusculo.PlaceholderText = "Músculo trabajado";

            // 
            // txtDescripcion
            // Campo para ingresar una descripción del ejercicio
            this.txtDescripcion.Location = new System.Drawing.Point(25, 120);
            this.txtDescripcion.Multiline = true;
            this.txtDescripcion.Name = "txtDescripcion";
            this.txtDescripcion.Size = new System.Drawing.Size(200, 60);
            //this.txtDescripcion.PlaceholderText = "Descripción";

            // 
            // btnAgregar
            // Botón para agregar un nuevo ejercicio
            this.btnAgregar.Location = new System.Drawing.Point(250, 60);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(100, 25);
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.UseVisualStyleBackColor = true;
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);

            // 
            // btnEditar
            // Botón para editar el ejercicio seleccionado
            this.btnEditar.Location = new System.Drawing.Point(250, 90);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(100, 25);
            this.btnEditar.Text = "Editar";
            this.btnEditar.UseVisualStyleBackColor = true;
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);

            // 
            // btnEliminar
            // Botón para eliminar el ejercicio seleccionado
            this.btnEliminar.Location = new System.Drawing.Point(250, 120);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(100, 25);
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.UseVisualStyleBackColor = true;
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);

            // 
            // dgvEjercicios
            // Tabla que muestra todos los ejercicios
            this.dgvEjercicios.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvEjercicios.Location = new System.Drawing.Point(25, 200);
            this.dgvEjercicios.Name = "dgvEjercicios";
            this.dgvEjercicios.Size = new System.Drawing.Size(500, 200);
            this.dgvEjercicios.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            this.dgvEjercicios.MultiSelect = false;
            this.dgvEjercicios.ReadOnly = true;
            this.dgvEjercicios.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEjercicios_SelectionChanged);

            // 
            // UcGestionEjercicios
            // Configuración general del UserControl
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.txtMusculo);
            this.Controls.Add(this.txtDescripcion);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.dgvEjercicios);
            this.Name = "UcGestionEjercicios";
            this.Size = new System.Drawing.Size(600, 420);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEjercicios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
