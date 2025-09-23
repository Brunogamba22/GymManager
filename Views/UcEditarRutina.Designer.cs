using System.Windows.Forms;

namespace GymManager.Views
{
    /// <summary>
    /// Código autogenerado por el Diseñador de Visual Studio.
    /// Define la estructura visual: DataGridView para rutinas
    /// y botón para guardar cambios.
    /// </summary>
    partial class UcEditarRutina
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.DataGridView dgvRutinas;     // Grilla de rutinas.
        private System.Windows.Forms.Button btnGuardar;           // Botón para guardar cambios.
        private System.Windows.Forms.DataGridViewTextBoxColumn colEjercicio;     // Columna para nombre del ejercicio.
        private System.Windows.Forms.DataGridViewTextBoxColumn colSeries;        // Columna para cantidad de series.
        private System.Windows.Forms.DataGridViewTextBoxColumn colRepeticiones;  // Columna para repeticiones.
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescanso;      // Columna para descanso.

        /// <summary>
        /// Libera recursos utilizados por los componentes.
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        /// <summary>
        /// Inicializa y configura los controles gráficos de la vista.
        /// </summary>
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
            this.dgvRutinas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill; // Ajusta ancho de columnas automáticamente.
            this.dgvRutinas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colEjercicio,
                this.colSeries,
                this.colRepeticiones,
                this.colDescanso}); // Agrega las columnas definidas.
            this.dgvRutinas.Dock = System.Windows.Forms.DockStyle.Top; // Se ubica en la parte superior.
            this.dgvRutinas.Location = new System.Drawing.Point(0, 0);
            this.dgvRutinas.Name = "dgvRutinas";
            this.dgvRutinas.Size = new System.Drawing.Size(1077, 300);
            this.dgvRutinas.TabIndex = 0;

            // 
            // colEjercicio
            // 
            this.colEjercicio.Name = "Ejercicio"; // Nombre de la columna en la grilla.

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
            this.btnGuardar.Dock = System.Windows.Forms.DockStyle.Bottom; // Se ubica abajo del UserControl.
            this.btnGuardar.Location = new System.Drawing.Point(0, 612);
            this.btnGuardar.Name = "btnGuardar";
            this.btnGuardar.Size = new System.Drawing.Size(1077, 40);
            this.btnGuardar.TabIndex = 1;
            this.btnGuardar.Text = "Guardar Cambios";
            this.btnGuardar.Click += new System.EventHandler(this.btnGuardar_Click); // Evento al hacer clic.

            // 
            // UcEditarRutina
            // 
            this.Controls.Add(this.dgvRutinas);  // Agrega la grilla.
            this.Controls.Add(this.btnGuardar);  // Agrega el botón.
            this.Name = "UcEditarRutina";
            this.Size = new System.Drawing.Size(1077, 652); // Tamaño total del UserControl.
            ((System.ComponentModel.ISupportInitialize)(this.dgvRutinas)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
