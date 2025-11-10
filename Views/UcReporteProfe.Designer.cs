using System;
using System.Drawing;
using System.Windows.Forms;
// --- 🔥 IMPORTANTE: Añade el using del Charting ---
using System.Windows.Forms.DataVisualization.Charting;

namespace GymManager.Views

{
    partial class UcReporteProfe
    {
        private System.ComponentModel.IContainer components = null;
        private Panel pnlHeader;
        private Label lblTitulo;
        private Panel pnlFiltros;
        private Label lblFechaDesde;
        private DateTimePicker dtpFechaDesde;
        private Label lblFechaHasta;
        private DateTimePicker dtpFechaHasta;
        private Button btnGenerarReporte;
        private Panel pnlChart;
        private Chart chartBalance; // <-- El Gráfico

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            

            // --- Definición del Chart ---
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.Windows.Forms.DataVisualization.Charting.Title title1 = new System.Windows.Forms.DataVisualization.Charting.Title();

            this.pnlHeader = new Panel();
            this.lblTitulo = new Label();
            this.pnlFiltros = new Panel();
            this.lblFechaDesde = new Label();
            this.dtpFechaDesde = new DateTimePicker();
            this.lblFechaHasta = new Label();
            this.dtpFechaHasta = new DateTimePicker();
            this.btnGenerarReporte = new Button();
            this.pnlChart = new Panel();
            this.chartBalance = new System.Windows.Forms.DataVisualization.Charting.Chart();

            Console.WriteLine($"[DEBUG] ChartBalance init: Height={this.chartBalance.Height}, Width={this.chartBalance.Width}");

            ((System.ComponentModel.ISupportInitialize)(this.chartBalance)).BeginInit();
            this.SuspendLayout();

            // Panel Header
            this.pnlHeader.Dock = DockStyle.Top;
            this.pnlHeader.Height = 60;
            this.pnlHeader.BackColor = Color.White;
            this.pnlHeader.Padding = new Padding(20, 0, 0, 0);

            this.lblTitulo.Text = "Reporte: Balance de Grupos Musculares";
            this.lblTitulo.Dock = DockStyle.Fill;
            this.lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            this.lblTitulo.ForeColor = Color.FromArgb(46, 134, 171);
            this.lblTitulo.TextAlign = ContentAlignment.MiddleLeft;
            this.pnlHeader.Controls.Add(lblTitulo);

            // Panel Filtros
            this.pnlFiltros.Dock = DockStyle.Top;
            this.pnlFiltros.Height = 65;
            this.pnlFiltros.Padding = new Padding(20, 10, 20, 10);
            this.pnlFiltros.BackColor = Color.White;
            this.pnlFiltros.Font = new Font("Segoe UI", 9.5f);

            int currentLeft = 10;
            int spacing = 10;
            int labelSpacing = 5;
            int topRowY = 15;

            this.lblFechaDesde.Text = "Desde:";
            this.lblFechaDesde.AutoSize = true;
            this.lblFechaDesde.Location = new Point(currentLeft, topRowY + 2);
            currentLeft = lblFechaDesde.Right + labelSpacing;

            this.dtpFechaDesde.Format = DateTimePickerFormat.Short;
            this.dtpFechaDesde.Size = new Size(110, 25);
            this.dtpFechaDesde.Location = new Point(currentLeft, topRowY);
            currentLeft = dtpFechaDesde.Right + spacing;

            this.lblFechaHasta.Text = "Hasta:";
            this.lblFechaHasta.AutoSize = true;
            this.lblFechaHasta.Location = new Point(currentLeft, topRowY + 2);
            currentLeft = lblFechaHasta.Right + labelSpacing;

            this.dtpFechaHasta.Format = DateTimePickerFormat.Short;
            this.dtpFechaHasta.Size = new Size(110, 25);
            this.dtpFechaHasta.Location = new Point(currentLeft, topRowY);
            currentLeft = dtpFechaHasta.Right + spacing;

            this.btnGenerarReporte.Text = "GENERAR REPORTE";
            this.btnGenerarReporte.Size = new Size(160, 35);
            this.btnGenerarReporte.Location = new Point(currentLeft, topRowY - 5);

            this.pnlFiltros.Controls.Add(lblFechaDesde);
            this.pnlFiltros.Controls.Add(dtpFechaDesde);
            this.pnlFiltros.Controls.Add(lblFechaHasta);
            this.pnlFiltros.Controls.Add(dtpFechaHasta);
            this.pnlFiltros.Controls.Add(btnGenerarReporte);

            // Panel Chart
            this.pnlChart.Dock = DockStyle.Fill;
            this.pnlChart.Padding = new Padding(20, 10, 20, 20);
            this.pnlChart.BackColor = Color.FromArgb(248, 249, 250);

            // Chart
            chartArea1.Name = "ChartArea1";
            chartArea1.BackColor = Color.White;
            this.chartBalance.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            legend1.Docking = Docking.Right;
            legend1.Alignment = StringAlignment.Center;
            legend1.Font = new Font("Segoe UI", 9);
            this.chartBalance.Legends.Add(legend1);
            this.chartBalance.Dock = DockStyle.Fill;
            this.chartBalance.Name = "chartBalance";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Default";
            series1.ChartType = SeriesChartType.Doughnut; // Gráfico de Torta/Dona
            series1.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            series1.Label = "#PERCENT{P0}"; // Mostrar porcentaje
            series1.LabelForeColor = Color.White;
            
            this.chartBalance.Series.Add(series1);
            title1.Name = "Title1";
            title1.Text = "Balance de Grupos Musculares";
            title1.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            this.chartBalance.Titles.Add(title1);

            this.pnlChart.Controls.Add(this.chartBalance);

            // Ensamblado
            this.Controls.Add(this.pnlChart);
            this.Controls.Add(this.pnlFiltros);
            this.Controls.Add(this.pnlHeader);

            ((System.ComponentModel.ISupportInitialize)(this.chartBalance)).EndInit();
            this.ResumeLayout(false);
        }

    }
}
