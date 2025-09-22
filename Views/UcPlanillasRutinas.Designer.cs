namespace GymManager.Views
{
    partial class UcPlanillasRutinas
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvPlanillas;
        private System.Windows.Forms.Button btnExportar;
        private System.Windows.Forms.DataGridViewTextBoxColumn colNombre;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProfesor;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFecha;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.dgvPlanillas = new System.Windows.Forms.DataGridView();
            this.colNombre = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProfesor = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFecha = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnExportar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlanillas)).BeginInit();
            this.SuspendLayout();
            // 
            // dgvPlanillas
            // 
            this.dgvPlanillas.AllowUserToAddRows = false;
            this.dgvPlanillas.AllowUserToDeleteRows = false;
            this.dgvPlanillas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPlanillas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colNombre,
            this.colProfesor,
            this.colFecha});
            this.dgvPlanillas.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvPlanillas.Location = new System.Drawing.Point(0, 0);
            this.dgvPlanillas.MultiSelect = false;
            this.dgvPlanillas.Name = "dgvPlanillas";
            this.dgvPlanillas.ReadOnly = true;
            this.dgvPlanillas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPlanillas.Size = new System.Drawing.Size(927, 300);
            this.dgvPlanillas.TabIndex = 0;
            // 
            // colNombre
            // 
            this.colNombre.HeaderText = "Nombre de la Rutina";
            this.colNombre.Name = "colNombre";
            this.colNombre.ReadOnly = true;
            // 
            // colProfesor
            // 
            this.colProfesor.HeaderText = "Profesor";
            this.colProfesor.Name = "colProfesor";
            this.colProfesor.ReadOnly = true;
            // 
            // colFecha
            // 
            this.colFecha.HeaderText = "Fecha Creación";
            this.colFecha.Name = "colFecha";
            this.colFecha.ReadOnly = true;
            // 
            // btnExportar
            // 
            this.btnExportar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnExportar.Location = new System.Drawing.Point(0, 473);
            this.btnExportar.Name = "btnExportar";
            this.btnExportar.Size = new System.Drawing.Size(927, 40);
            this.btnExportar.TabIndex = 1;
            this.btnExportar.Text = "Exportar Rutina";
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);
            // 
            // UcPlanillasRutinas
            // 
            this.Controls.Add(this.dgvPlanillas);
            this.Controls.Add(this.btnExportar);
            this.Name = "UcPlanillasRutinas";
            this.Size = new System.Drawing.Size(927, 513);
            ((System.ComponentModel.ISupportInitialize)(this.dgvPlanillas)).EndInit();
            this.ResumeLayout(false);

        }
    }
}
