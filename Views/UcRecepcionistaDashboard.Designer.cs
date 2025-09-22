using System;

namespace GymManager.Views
{
    partial class UcRecepcionistaDashboard
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblTitulo;
        private System.Windows.Forms.ComboBox cmbFecha;
        private System.Windows.Forms.DataGridView dgvRutina;
        private System.Windows.Forms.Button btnPantallaCompleta;
        private System.Windows.Forms.Button btnExportar;
        private System.Windows.Forms.Button btnImprimir;

        private System.Windows.Forms.DataGridViewTextBoxColumn colEjercicio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSeries;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReps;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescanso;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) { components.Dispose(); }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.cmbFecha = new System.Windows.Forms.ComboBox();
            this.dgvRutina = new System.Windows.Forms.DataGridView();
            this.colEjercicio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSeries = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReps = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescanso = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnPantallaCompleta = new System.Windows.Forms.Button();
            this.btnExportar = new System.Windows.Forms.Button();
            this.btnImprimir = new System.Windows.Forms.Button();

            ((System.ComponentModel.ISupportInitialize)(this.dgvRutina)).BeginInit();
            this.SuspendLayout();

            // lblTitulo
            this.lblTitulo.Text = "Rutina Diaria";
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Dock = System.Windows.Forms.DockStyle.Top;
            this.lblTitulo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            // cmbFecha
            this.cmbFecha.Dock = System.Windows.Forms.DockStyle.Top;
            this.cmbFecha.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbFecha.Items.AddRange(new object[] {
                DateTime.Today.ToShortDateString(),
                DateTime.Today.AddDays(-1).ToShortDateString(),
                DateTime.Today.AddDays(-2).ToShortDateString()
            });
            this.cmbFecha.SelectedIndexChanged += new System.EventHandler(this.cmbFecha_SelectedIndexChanged);

            // dgvRutina
            this.dgvRutina.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                this.colEjercicio,
                this.colSeries,
                this.colReps,
                this.colDescanso});
            this.dgvRutina.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvRutina.Height = 250;

            // Columnas
            this.colEjercicio.HeaderText = "Ejercicio";
            this.colSeries.HeaderText = "Series";
            this.colReps.HeaderText = "Reps";
            this.colDescanso.HeaderText = "Descanso";

            // btnPantallaCompleta
            this.btnPantallaCompleta.Text = "Pantalla Completa";
            this.btnPantallaCompleta.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnPantallaCompleta.Click += new System.EventHandler(this.btnPantallaCompleta_Click);

            // btnExportar
            this.btnExportar.Text = "Exportar";
            this.btnExportar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnExportar.Click += new System.EventHandler(this.btnExportar_Click);

            // btnImprimir
            this.btnImprimir.Text = "Imprimir";
            this.btnImprimir.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnImprimir.Click += new System.EventHandler(this.btnImprimir_Click);

            // UcRecepcionistaDashboard
            this.Controls.Add(this.dgvRutina);
            this.Controls.Add(this.cmbFecha);
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.btnImprimir);
            this.Controls.Add(this.btnExportar);
            this.Controls.Add(this.btnPantallaCompleta);

            this.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ResumeLayout(false);

            ((System.ComponentModel.ISupportInitialize)(this.dgvRutina)).EndInit();
        }
    }
}
