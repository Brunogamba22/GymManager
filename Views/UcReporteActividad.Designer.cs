using System.Windows.Forms;
using System.Drawing;

namespace GymManager.Views
{
    partial class UcReporteActividad
    {
        private System.ComponentModel.IContainer components = null;
        private Panel pnlFiltros;
        private Label lblFechaDesde;
        private DateTimePicker dtpFechaDesde;
        private Label lblFechaHasta;
        private DateTimePicker dtpFechaHasta;
        private Button btnGenerarReporte;

        // --- Paneles para los "Cards" ---
        private TableLayoutPanel tlpCards;
        private Panel pnlCardTotal;
        private Panel pnlCardNuevas;
        private Panel pnlCardEditadas;

        // --- Labels para los números ---
        private Label lblTotalNumero;
        private Label lblTotalTexto;
        private Label lblNuevasNumero;
        private Label lblNuevasTexto;
        private Label lblEditadasNumero;
        private Label lblEditadasTexto;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.pnlFiltros = new Panel();
            this.lblFechaDesde = new Label();
            this.dtpFechaDesde = new DateTimePicker();
            this.lblFechaHasta = new Label();
            this.dtpFechaHasta = new DateTimePicker();
            this.btnGenerarReporte = new Button();
            this.tlpCards = new TableLayoutPanel();
            this.pnlCardTotal = new Panel();
            this.pnlCardNuevas = new Panel();
            this.pnlCardEditadas = new Panel();
            this.lblTotalNumero = new Label();
            this.lblTotalTexto = new Label();
            this.lblNuevasNumero = new Label();
            this.lblNuevasTexto = new Label();
            this.lblEditadasNumero = new Label();
            this.lblEditadasTexto = new Label();

            this.SuspendLayout();

            // Panel Filtros
            this.pnlFiltros.Dock = DockStyle.Top;
            this.pnlFiltros.Height = 65;
            this.pnlFiltros.Padding = new Padding(20, 10, 20, 10);
            this.pnlFiltros.BackColor = Color.White;
            this.pnlFiltros.Font = new Font("Segoe UI", 9.5f);

            int currentLeft = 10, spacing = 10, labelSpacing = 5, topRowY = 15;
            this.lblFechaDesde.Text = "Desde:"; this.lblFechaDesde.AutoSize = true;
            this.lblFechaDesde.Location = new Point(currentLeft, topRowY + 2);
            currentLeft = lblFechaDesde.Right + labelSpacing;
            this.dtpFechaDesde.Format = DateTimePickerFormat.Short; this.dtpFechaDesde.Size = new Size(110, 25);
            this.dtpFechaDesde.Location = new Point(currentLeft, topRowY);
            currentLeft = dtpFechaDesde.Right + spacing;
            this.lblFechaHasta.Text = "Hasta:"; this.lblFechaHasta.AutoSize = true;
            this.lblFechaHasta.Location = new Point(currentLeft, topRowY + 2);
            currentLeft = lblFechaHasta.Right + labelSpacing;
            this.dtpFechaHasta.Format = DateTimePickerFormat.Short; this.dtpFechaHasta.Size = new Size(110, 25);
            this.dtpFechaHasta.Location = new Point(currentLeft, topRowY);
            currentLeft = dtpFechaHasta.Right + spacing;
            this.btnGenerarReporte.Text = "GENERAR REPORTE"; this.btnGenerarReporte.Size = new Size(160, 35);
            this.btnGenerarReporte.Location = new Point(currentLeft, topRowY - 5);

            this.pnlFiltros.Controls.Add(lblFechaDesde);
            this.pnlFiltros.Controls.Add(dtpFechaDesde);
            this.pnlFiltros.Controls.Add(lblFechaHasta);
            this.pnlFiltros.Controls.Add(dtpFechaHasta);
            this.pnlFiltros.Controls.Add(btnGenerarReporte);

            // TableLayoutPanel (para centrar los cards)
            this.tlpCards.Dock = DockStyle.Fill;
            this.tlpCards.Padding = new Padding(20);
            this.tlpCards.BackColor = Color.FromArgb(248, 249, 250);
            this.tlpCards.ColumnCount = 3;
            this.tlpCards.RowCount = 1;
            this.tlpCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            this.tlpCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            this.tlpCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));

            // Card Total
            this.pnlCardTotal.BackColor = Color.White; this.pnlCardTotal.Dock = DockStyle.Fill;
            this.pnlCardTotal.Padding = new Padding(20); this.pnlCardTotal.Margin = new Padding(10);
            this.lblTotalNumero.Text = "0"; this.lblTotalNumero.Dock = DockStyle.Fill;
            this.lblTotalNumero.Font = new Font("Segoe UI", 36, FontStyle.Bold);
            this.lblTotalNumero.ForeColor = Color.FromArgb(46, 134, 171); // primaryColor
            this.lblTotalNumero.TextAlign = ContentAlignment.MiddleCenter;
            this.lblTotalTexto.Text = "Rutinas Totales"; this.lblTotalTexto.Dock = DockStyle.Bottom;
            this.lblTotalTexto.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            this.lblTotalTexto.TextAlign = ContentAlignment.MiddleCenter; this.lblTotalTexto.Height = 30;
            this.pnlCardTotal.Controls.Add(lblTotalNumero); this.pnlCardTotal.Controls.Add(lblTotalTexto);

            // Card Nuevas
            this.pnlCardNuevas.BackColor = Color.White; this.pnlCardNuevas.Dock = DockStyle.Fill;
            this.pnlCardNuevas.Padding = new Padding(20); this.pnlCardNuevas.Margin = new Padding(10);
            this.lblNuevasNumero.Text = "0"; this.lblNuevasNumero.Dock = DockStyle.Fill;
            this.lblNuevasNumero.Font = new Font("Segoe UI", 36, FontStyle.Bold);
            this.lblNuevasNumero.ForeColor = Color.FromArgb(40, 167, 69); // successColor
            this.lblNuevasNumero.TextAlign = ContentAlignment.MiddleCenter;
            this.lblNuevasTexto.Text = "Rutinas Sin Edicion"; this.lblNuevasTexto.Dock = DockStyle.Bottom;
            this.lblNuevasTexto.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            this.lblNuevasTexto.TextAlign = ContentAlignment.MiddleCenter; this.lblNuevasTexto.Height = 30;
            this.pnlCardNuevas.Controls.Add(lblNuevasNumero); this.pnlCardNuevas.Controls.Add(lblNuevasTexto);

            // Card Editadas
            this.pnlCardEditadas.BackColor = Color.White; this.pnlCardEditadas.Dock = DockStyle.Fill;
            this.pnlCardEditadas.Padding = new Padding(20); this.pnlCardEditadas.Margin = new Padding(10);
            this.lblEditadasNumero.Text = "0"; this.lblEditadasNumero.Dock = DockStyle.Fill;
            this.lblEditadasNumero.Font = new Font("Segoe UI", 36, FontStyle.Bold);
            this.lblEditadasNumero.ForeColor = Color.FromArgb(255, 193, 7); // warningColor
            this.lblEditadasNumero.TextAlign = ContentAlignment.MiddleCenter;
            this.lblEditadasTexto.Text = "Rutinas Editadas"; this.lblEditadasTexto.Dock = DockStyle.Bottom;
            this.lblEditadasTexto.Font = new Font("Segoe UI", 12, FontStyle.Regular);
            this.lblEditadasTexto.TextAlign = ContentAlignment.MiddleCenter; this.lblEditadasTexto.Height = 30;
            this.pnlCardEditadas.Controls.Add(lblEditadasNumero); this.pnlCardEditadas.Controls.Add(lblEditadasTexto);

            this.tlpCards.Controls.Add(pnlCardTotal, 0, 0);
            this.tlpCards.Controls.Add(pnlCardNuevas, 1, 0);
            this.tlpCards.Controls.Add(pnlCardEditadas, 2, 0); 

            // Ensamblado
            this.Controls.Add(this.tlpCards);
            this.Controls.Add(this.pnlFiltros);
            this.ResumeLayout(false);
        }
    }
}