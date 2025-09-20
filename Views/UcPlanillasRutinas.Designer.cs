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

            // dgvPlanillas
            this.dgvPlanillas.AllowUserToAddRows = false;
            this.dgvPlanillas.AllowUserToDeleteRows = false;
            this.dgvPlanillas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvPlanillas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colNombre,
                this.colProfesor,
                this.colFecha});
            this.dgvPlanillas.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvPlanillas.Height = 300;
            this.dgvPlanillas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvPlanillas.MultiSelect = false;
            this.dgvPlanillas.ReadOnly = true;

            // Columnas
            this.colNombre.HeaderText = "Nombre de la Rutina";
            this.colProfesor.HeaderText = "Profesor";
            this.colFecha.HeaderText = "Fecha Creación";

            // btnExportar
            this.btnExportar.Text = "Exportar Rutina";
            this.btnExportar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnExportar.Height = 40;
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);

            // UcPlanillasRutinas
            this.Controls.Add(this.dgvPlanillas);
            this.Controls.Add(this.btnExportar);
            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Size = new System.Drawing.Size(800, 400);

            ((System.ComponentModel.ISupportInitialize)(this.dgvPlanillas)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
