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

        // --- Paneles para las "Cards" ---
        private TableLayoutPanel tlpCards;
        private Panel pnlCardTotal;
        private Panel pnlCardNuevas;
        private Panel pnlCardEditadas;

        // --- Etiquetas para los números ---
        private Label lblTotalNumero;
        private Label lblTotalTexto;
        private Label lblNuevasNumero;
        private Label lblNuevasTexto;
        private Label lblEditadasNumero;
        private Label lblEditadasTexto;
        private Label lblHombresNumero;
        private Label lblMujeresNumero;
        private Label lblDeportistasNumero;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
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

            // ================================
            // PANEL DE FILTROS
            // ================================
            this.pnlFiltros.Dock = DockStyle.Top;
            this.pnlFiltros.Height = 65;
            this.pnlFiltros.Padding = new Padding(20, 10, 20, 10);
            this.pnlFiltros.BackColor = Color.White;
            this.pnlFiltros.Font = new Font("Segoe UI", 9.5f);

            int currentLeft = 10, spacing = 10, labelSpacing = 5, topRowY = 15;

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

            this.btnGenerarReporte.Text = "GENERAR INFORME";
            this.btnGenerarReporte.Size = new Size(160, 35);
            this.btnGenerarReporte.Location = new Point(currentLeft, topRowY - 5);

            this.pnlFiltros.Controls.Add(lblFechaDesde);
            this.pnlFiltros.Controls.Add(dtpFechaDesde);
            this.pnlFiltros.Controls.Add(lblFechaHasta);
            this.pnlFiltros.Controls.Add(dtpFechaHasta);
            this.pnlFiltros.Controls.Add(btnGenerarReporte);

            // ================================
            // TABLA DE CARDS
            // ================================
            this.tlpCards.Dock = DockStyle.Fill;
            this.tlpCards.Padding = new Padding(20);
            this.tlpCards.BackColor = Color.FromArgb(248, 249, 250);
            this.tlpCards.ColumnCount = 3;
            this.tlpCards.RowCount = 2;
            this.tlpCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            this.tlpCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            this.tlpCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));

            // --- Crear las tarjetas ---
            Panel pnlCardHombres = CrearCard(out lblHombresNumero, "Rutinas Hombres", Color.FromArgb(46, 134, 171));
            Panel pnlCardMujeres = CrearCard(out lblMujeresNumero, "Rutinas Mujeres", Color.FromArgb(162, 59, 114));
            Panel pnlCardDeportistas = CrearCard(out lblDeportistasNumero, "Rutinas Deportistas", Color.FromArgb(28, 167, 69));

            // --- Tarjeta Total ---
            this.pnlCardTotal = CrearCard(out lblTotalNumero, "Rutinas Totales", Color.FromArgb(46, 134, 171));
            // --- Tarjeta Nuevas ---
            this.pnlCardNuevas = CrearCard(out lblNuevasNumero, "Rutinas Sin Edición", Color.FromArgb(40, 167, 69));
            // --- Tarjeta Editadas ---
            this.pnlCardEditadas = CrearCard(out lblEditadasNumero, "Rutinas Editadas", Color.FromArgb(255, 193, 7));

            // --- Agregar al diseño ---
            this.tlpCards.Controls.Add(pnlCardTotal, 0, 0);
            this.tlpCards.Controls.Add(pnlCardNuevas, 1, 0);
            this.tlpCards.Controls.Add(pnlCardEditadas, 2, 0);
            this.tlpCards.Controls.Add(pnlCardHombres, 0, 1);
            this.tlpCards.Controls.Add(pnlCardMujeres, 1, 1);
            this.tlpCards.Controls.Add(pnlCardDeportistas, 2, 1);

            // ================================
            // ENSAMBLAR TODO
            // ================================
            this.Controls.Add(this.tlpCards);
            this.Controls.Add(this.pnlFiltros);
            this.ResumeLayout(false);
        }

        private Panel CrearCard(out Label lblNumero, string texto, Color color)
        {
            Panel card = new Panel
            {
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Padding = new Padding(20),
                Margin = new Padding(10)
            };

            lblNumero = new Label
            {
                Text = "0",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 36, FontStyle.Bold),
                ForeColor = color,
                TextAlign = ContentAlignment.MiddleCenter
            };

            Label lblTexto = new Label
            {
                Text = texto,
                Dock = DockStyle.Bottom,
                Font = new Font("Segoe UI", 12, FontStyle.Regular),
                Height = 30,
                TextAlign = ContentAlignment.MiddleCenter
            };

            card.Controls.Add(lblNumero);
            card.Controls.Add(lblTexto);
            return card;
        }
    }
}
