using System.Windows.Forms;
using System.Drawing;
using System.Windows.Forms.DataVisualization.Charting;


namespace GymManager.Views
{
    partial class UcReportes
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitulo;
        private Label lblTotalEjercicios;
        private Label lblTotalUsuarios;
        private Label lblEjerciciosTxt;
        private Label lblUsuariosTxt;
        private Chart chartUsuarios;
        private Chart chartEjercicios;
        private Label lblProfes;
        private Label lblReceps;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblTotalEjercicios = new System.Windows.Forms.Label();
            this.lblTotalUsuarios = new System.Windows.Forms.Label();
            this.lblEjerciciosTxt = new System.Windows.Forms.Label();
            this.lblUsuariosTxt = new System.Windows.Forms.Label();
            this.chartUsuarios = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.chartEjercicios = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblProfes = new System.Windows.Forms.Label();
            this.lblReceps = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chartUsuarios)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartEjercicios)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(223, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "📊 Reportes del Sistema";
            // 
            // lblTotalEjercicios
            // 
            this.lblTotalEjercicios.AutoSize = true;
            this.lblTotalEjercicios.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalEjercicios.Location = new System.Drawing.Point(180, 98);
            this.lblTotalEjercicios.Name = "lblTotalEjercicios";
            this.lblTotalEjercicios.Size = new System.Drawing.Size(0, 19);
            this.lblTotalEjercicios.TabIndex = 4;
            // 
            // lblTotalUsuarios
            // 
            this.lblTotalUsuarios.AutoSize = true;
            this.lblTotalUsuarios.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalUsuarios.Location = new System.Drawing.Point(180, 68);
            this.lblTotalUsuarios.Name = "lblTotalUsuarios";
            this.lblTotalUsuarios.Size = new System.Drawing.Size(0, 19);
            this.lblTotalUsuarios.TabIndex = 2;
            // 
            // lblEjerciciosTxt
            // 
            this.lblEjerciciosTxt.AutoSize = true;
            this.lblEjerciciosTxt.Location = new System.Drawing.Point(40, 100);
            this.lblEjerciciosTxt.Name = "lblEjerciciosTxt";
            this.lblEjerciciosTxt.Size = new System.Drawing.Size(96, 15);
            this.lblEjerciciosTxt.TabIndex = 3;
            this.lblEjerciciosTxt.Text = "Total de ejercicios:";
            // 
            // lblUsuariosTxt
            // 
            this.lblUsuariosTxt.AutoSize = true;
            this.lblUsuariosTxt.Location = new System.Drawing.Point(40, 70);
            this.lblUsuariosTxt.Name = "lblUsuariosTxt";
            this.lblUsuariosTxt.Size = new System.Drawing.Size(91, 15);
            this.lblUsuariosTxt.TabIndex = 1;
            this.lblUsuariosTxt.Text = "Total de usuarios:";
            // 
            // chartUsuarios
            // 
            chartArea1.Name = "ChartArea1";
            this.chartUsuarios.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend";
            this.chartUsuarios.Legends.Add(legend1);
            this.chartUsuarios.Location = new System.Drawing.Point(40, 150);
            this.chartUsuarios.Name = "chartUsuarios";
            this.chartUsuarios.Size = new System.Drawing.Size(450, 350);
            this.chartUsuarios.TabIndex = 5;
            // 
            // chartEjercicios
            // 
            chartArea2.Name = "ChartArea1";
            this.chartEjercicios.ChartAreas.Add(chartArea2);
            this.chartEjercicios.Location = new System.Drawing.Point(520, 150);
            this.chartEjercicios.Name = "chartEjercicios";
            this.chartEjercicios.Size = new System.Drawing.Size(500, 350);
            this.chartEjercicios.TabIndex = 6;
            // 
            // lblProfes
            // 
            this.lblProfes.AutoSize = true;
            this.lblProfes.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblProfes.Location = new System.Drawing.Point(40, 150);
            this.lblProfes.Name = "lblProfes";
            this.lblProfes.Size = new System.Drawing.Size(74, 15);
            this.lblProfes.TabIndex = 8;
            this.lblProfes.Text = "Profesores: 0";
            // 
            // lblReceps
            // 
            this.lblReceps.AutoSize = true;
            this.lblReceps.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblReceps.Location = new System.Drawing.Point(40, 170);
            this.lblReceps.Name = "lblReceps";
            this.lblReceps.Size = new System.Drawing.Size(100, 15);
            this.lblReceps.TabIndex = 9;
            this.lblReceps.Text = "Recepcionistas: 0";
            // 
            // UcReportes
            // 
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblUsuariosTxt);
            this.Controls.Add(this.lblTotalUsuarios);
            this.Controls.Add(this.lblEjerciciosTxt);
            this.Controls.Add(this.lblTotalEjercicios);
            this.Controls.Add(this.chartUsuarios);
            this.Controls.Add(this.chartEjercicios);
            this.Controls.Add(this.lblProfes);
            this.Controls.Add(this.lblReceps);
            this.Name = "UcReportes";
            this.Size = new System.Drawing.Size(1050, 550);
            this.Load += new System.EventHandler(this.UcReportes_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chartUsuarios)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chartEjercicios)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
