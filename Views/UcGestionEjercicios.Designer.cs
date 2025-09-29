using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Views
{
    partial class UcGestionEjercicios
    {
        private System.ComponentModel.IContainer components = null;

        // Controles de la interfaz.
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.Button btnAgregar;
        private System.Windows.Forms.Button btnEditar;
        private System.Windows.Forms.Button btnEliminar;
        private System.Windows.Forms.DataGridView dgvEjercicios;
        private System.Windows.Forms.TextBox txtBuscar;
        private Label label1;

        private System.Windows.Forms.TextBox txtSeries;
        private System.Windows.Forms.TextBox txtRepeticiones;
        private System.Windows.Forms.TextBox txtDescanso;
        private System.Windows.Forms.Label lblSeries;
        private System.Windows.Forms.Label lblRepeticiones;
        private System.Windows.Forms.Label lblDescanso;

        private System.Windows.Forms.Button btnLimpiar;
        private ComboBox cmbMusculo;

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
            this.lblTitulo = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.btnAgregar = new System.Windows.Forms.Button();
            this.btnEditar = new System.Windows.Forms.Button();
            this.btnEliminar = new System.Windows.Forms.Button();
            this.dgvEjercicios = new System.Windows.Forms.DataGridView();
            this.cmbMusculo = new System.Windows.Forms.ComboBox();
            this.txtBuscar = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSeries = new System.Windows.Forms.TextBox();
            this.txtRepeticiones = new System.Windows.Forms.TextBox();
            this.txtDescanso = new System.Windows.Forms.TextBox();
            this.lblSeries = new System.Windows.Forms.Label();
            this.lblRepeticiones = new System.Windows.Forms.Label();
            this.lblDescanso = new System.Windows.Forms.Label();
            this.btnLimpiar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvEjercicios)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(193, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "Gestión de Ejercicios";
            // 
            // txtNombre
            // 
            this.txtNombre.Location = new System.Drawing.Point(25, 55);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(200, 20);
            this.txtNombre.TabIndex = 1;
            this.txtNombre.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNombre_KeyPress);
            // 
            // btnAgregar
            // 
            this.btnAgregar.Location = new System.Drawing.Point(286, 50);
            this.btnAgregar.Name = "btnAgregar";
            this.btnAgregar.Size = new System.Drawing.Size(100, 25);
            this.btnAgregar.TabIndex = 4;
            this.btnAgregar.Text = "Agregar";
            this.btnAgregar.Click += new System.EventHandler(this.btnAgregar_Click);
            // 
            // btnEditar
            // 
            this.btnEditar.Location = new System.Drawing.Point(286, 80);
            this.btnEditar.Name = "btnEditar";
            this.btnEditar.Size = new System.Drawing.Size(100, 25);
            this.btnEditar.TabIndex = 5;
            this.btnEditar.Text = "Editar";
            this.btnEditar.Click += new System.EventHandler(this.btnEditar_Click);
            // 
            // btnEliminar
            // 
            this.btnEliminar.Location = new System.Drawing.Point(286, 145);
            this.btnEliminar.Name = "btnEliminar";
            this.btnEliminar.Size = new System.Drawing.Size(100, 25);
            this.btnEliminar.TabIndex = 6;
            this.btnEliminar.Text = "Eliminar";
            this.btnEliminar.Click += new System.EventHandler(this.btnEliminar_Click);
            // 
            // dgvEjercicios
            // 
            this.dgvEjercicios.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvEjercicios.Location = new System.Drawing.Point(25, 240);
            this.dgvEjercicios.MultiSelect = false;
            this.dgvEjercicios.Name = "dgvEjercicios";
            this.dgvEjercicios.ReadOnly = true;
            this.dgvEjercicios.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvEjercicios.Size = new System.Drawing.Size(700, 300);
            this.dgvEjercicios.TabIndex = 12;
            this.dgvEjercicios.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvEjercicios_SelectionChanged);
            // 
            // cmbMusculo
            // 
            this.cmbMusculo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMusculo.Items.AddRange(new object[] {
            "Seleccione un músculo",
            "Pecho",
            "Espalda",
            "Hombros",
            "Brazos",
            "Cuádriceps",
            "Isquiotibiales",
            "Glúteos",
            "Pantorrillas",
            "Abdomen"});
            this.cmbMusculo.Location = new System.Drawing.Point(25, 85);
            this.cmbMusculo.Name = "cmbMusculo";
            this.cmbMusculo.Size = new System.Drawing.Size(200, 21);
            this.cmbMusculo.TabIndex = 2;
            // 
            // txtBuscar
            // 
            this.txtBuscar.Location = new System.Drawing.Point(68, 209);
            this.txtBuscar.Name = "txtBuscar";
            this.txtBuscar.Size = new System.Drawing.Size(164, 20);
            this.txtBuscar.TabIndex = 13;
            this.txtBuscar.TextChanged += new System.EventHandler(this.txtBuscar_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(22, 212);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "Buscar";
            // 
            // txtSeries
            // 
            this.txtSeries.Location = new System.Drawing.Point(100, 112);
            this.txtSeries.Name = "txtSeries";
            this.txtSeries.Size = new System.Drawing.Size(125, 20);
            this.txtSeries.TabIndex = 4;
            this.txtSeries.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtSeries_KeyPress);
            // 
            // txtRepeticiones
            // 
            this.txtRepeticiones.Location = new System.Drawing.Point(100, 142);
            this.txtRepeticiones.Name = "txtRepeticiones";
            this.txtRepeticiones.Size = new System.Drawing.Size(125, 20);
            this.txtRepeticiones.TabIndex = 6;
            // 
            // txtDescanso
            // 
            this.txtDescanso.Location = new System.Drawing.Point(100, 172);
            this.txtDescanso.Name = "txtDescanso";
            this.txtDescanso.Size = new System.Drawing.Size(125, 20);
            this.txtDescanso.TabIndex = 8;
            // 
            // lblSeries
            // 
            this.lblSeries.Location = new System.Drawing.Point(25, 115);
            this.lblSeries.Name = "lblSeries";
            this.lblSeries.Size = new System.Drawing.Size(50, 23);
            this.lblSeries.TabIndex = 3;
            this.lblSeries.Text = "Series:";
            // 
            // lblRepeticiones
            // 
            this.lblRepeticiones.Location = new System.Drawing.Point(25, 145);
            this.lblRepeticiones.Name = "lblRepeticiones";
            this.lblRepeticiones.Size = new System.Drawing.Size(69, 23);
            this.lblRepeticiones.TabIndex = 5;
            this.lblRepeticiones.Text = "Repeticiones:";
            // 
            // lblDescanso
            // 
            this.lblDescanso.Location = new System.Drawing.Point(25, 175);
            this.lblDescanso.Name = "lblDescanso";
            this.lblDescanso.Size = new System.Drawing.Size(69, 23);
            this.lblDescanso.TabIndex = 7;
            this.lblDescanso.Text = "Descanso:";
            // 
            // btnLimpiar
            // 
            this.btnLimpiar.Location = new System.Drawing.Point(286, 110);
            this.btnLimpiar.Name = "btnLimpiar";
            this.btnLimpiar.Size = new System.Drawing.Size(100, 25);
            this.btnLimpiar.TabIndex = 11;
            this.btnLimpiar.Text = "Limpiar";
            this.btnLimpiar.Click += new System.EventHandler(this.btnLimpiar_Click);
            // 
            // UcGestionEjercicios
            // 
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.txtNombre);
            this.Controls.Add(this.cmbMusculo);
            this.Controls.Add(this.lblSeries);
            this.Controls.Add(this.txtSeries);
            this.Controls.Add(this.lblRepeticiones);
            this.Controls.Add(this.txtRepeticiones);
            this.Controls.Add(this.lblDescanso);
            this.Controls.Add(this.txtDescanso);
            this.Controls.Add(this.btnAgregar);
            this.Controls.Add(this.btnEditar);
            this.Controls.Add(this.btnEliminar);
            this.Controls.Add(this.btnLimpiar);
            this.Controls.Add(this.dgvEjercicios);
            this.Controls.Add(this.txtBuscar);
            this.Controls.Add(this.label1);
            this.Name = "UcGestionEjercicios";
            this.Size = new System.Drawing.Size(600, 536);
            ((System.ComponentModel.ISupportInitialize)(this.dgvEjercicios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
