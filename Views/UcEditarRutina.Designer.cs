using System.Windows.Forms;

namespace GymManager.Views
{
    partial class UcEditarRutina
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvRutinas;
        private System.Windows.Forms.Button btnGuardar;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEjercicio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSeries;
        private System.Windows.Forms.DataGridViewTextBoxColumn colRepeticiones;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescanso;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvRutinas = new System.Windows.Forms.DataGridView();
            this.colEjercicio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSeries = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colRepeticiones = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescanso = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnGuardar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvRutinas)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvRutinas
            // 
            this.dgvRutinas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvRutinas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colEjercicio,
            this.colSeries,
            this.colRepeticiones,
            this.colDescanso});
            this.dgvRutinas.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvRutinas.Location = new System.Drawing.Point(0, 0);
            this.dgvRutinas.Name = "dgvRutinas";
            this.dgvRutinas.Size = new System.Drawing.Size(1077, 300);
            this.dgvRutinas.TabIndex = 0;
            // 
            // colEjercicio
            // 
            this.colEjercicio.Name = "Ejercicio";
            // 
            // colSeries
            // 
            this.colSeries.Name = "Series";
            // 
            // colRepeticiones
            // 
            this.colRepeticiones.Name = "Repeticiones";
            // 
            // colDescanso
            // 
            this.colDescanso.Name = "Descanso";
            // 
            // btnGuardar
            // 
            this.btnGuardar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnGuardar.Location = new System.Drawing.Point(0, 612);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(1077, 40);
            this.btnGuardar.TabIndex = 1;
            this.btnGuardar.Text = "Guardar Cambios";
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click);
            // 
            // UcEditarRutina
            // 
            this.Controls.Add(this.dgvRutinas);
            this.Controls.Add(this.btnGuardar);
            this.Name = "UcEditarRutina";
            this.Size = new System.Drawing.Size(1077, 652);
            ((System.ComponentModel.ISupportInitialize)(this.dgvRutinas)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
