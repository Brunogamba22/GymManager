using System.Windows.Forms;
using System.Drawing;
// 💡 AÑADIR ESTE USING
using System.Windows.Forms.DataVisualization.Charting;

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
        private TableLayoutPanel tlpCards; // Sigue siendo la fila de arriba
        private Panel pnlCardTotal;
        private Panel pnlCardNuevas;
        private Panel pnlCardEditadas;

        // --- Etiquetas para los números (SOLO LAS 3 DE ARRIBA) ---
        private Label lblTotalNumero;
        private Label lblNuevasNumero;
        private Label lblEditadasNumero;

        // --- 💡 NUEVO CONTROL DE GRÁFICO ---
        private Chart chartGeneros;

        // ❌ Ya no necesitamos los labels de Hombres, Mujeres, etc.

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

            // 💡 INICIALIZAR EL GRÁFICO
            this.chartGeneros = new Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chartGeneros)).BeginInit();

            this.SuspendLayout();

            // ================================
            // PANEL DE FILTROS (Sin cambios)
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
            // TABLA DE CARDS (MODIFICADA)
            // ================================
            this.tlpCards.Dock = DockStyle.Top; // 💡 Cambiado
            this.tlpCards.Height = 160;          // 💡 Altura fija para la fila de tarjetas
            this.tlpCards.Padding = new Padding(20, 15, 20, 15);
            this.tlpCards.BackColor = Color.FromArgb(248, 249, 250);
            this.tlpCards.ColumnCount = 3;
            this.tlpCards.RowCount = 1;          // 💡 Solo 1 fila
            this.tlpCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            this.tlpCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            this.tlpCards.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33.33f));
            this.tlpCards.RowStyles.Add(new RowStyle(SizeType.Percent, 100f)); // 💡

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

            // ❌ Ya no se agregan las tarjetas de Hombres, Mujeres, etc.

            // ================================
            // 💡 GRÁFICO DE GÉNEROS (NUEVO)
            // ================================
            this.chartGeneros.Dock = DockStyle.Fill;
            this.chartGeneros.Location = new Point(0, 0);
            this.chartGeneros.Name = "chartGeneros";
            this.chartGeneros.BackColor = Color.FromArgb(248, 249, 250);
            this.chartGeneros.BorderlineColor = Color.Transparent;

            // ================================
            // ENSAMBLAR TODO
            // ================================

            // 💡 El orden de "Add" es importante para el Docking
            this.Controls.Add(this.chartGeneros);  // 1. Ocupa todo el fondo
            this.Controls.Add(this.tlpCards);      // 2. Se pone encima, arriba
            this.Controls.Add(this.pnlFiltros);    // 3. Se pone encima de todo, arriba

            ((System.ComponentModel.ISupportInitialize)(this.chartGeneros)).EndInit();
            this.ResumeLayout(false);
        }

        // --- MÉTODO CrearCard (MODIFICADO) ---
        // Hice el texto del número más grande y el título más pequeño
        // para que coincida con el estilo profesional.
        private Panel CrearCard(out Label lblNumero, string texto, Color color)
        {
            Panel card = new Panel
            {
                BackColor = Color.White,
                Dock = DockStyle.Fill,
                Padding = new Padding(10),
                Margin = new Padding(10)
                // Aquí podrías añadir un borde, sombra, etc.
            };

            // 1. Crear un TableLayoutPanel para alinear el contenido
            TableLayoutPanel tlp = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2,
                BackColor = Color.Transparent
            };

            // 2. Definir las filas: 70% para el número, 30% para el texto
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 70F));
            tlp.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));

            // 3. Crear el Label del Número (ahora se centra en la Fila 1)
            lblNumero = new Label
            {
                Text = "0",
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 32, FontStyle.Bold),
                ForeColor = color,
                // Se centrará perfectamente en su celda (Fila 1)
                TextAlign = ContentAlignment.MiddleCenter
            };

            // 4. Crear el Label del Texto (ahora se alinea arriba en la Fila 2)
            Label lblTexto = new Label
            {
                Text = texto,
                Dock = DockStyle.Fill,
                Font = new Font("Segoe UI", 11, FontStyle.Regular),
                ForeColor = Color.DimGray,
                // Lo alineamos arriba para que esté pegado al número
                TextAlign = ContentAlignment.TopCenter
            };

            // 5. Añadir los labels al TableLayoutPanel
            tlp.Controls.Add(lblNumero, 0, 0);
            tlp.Controls.Add(lblTexto, 0, 1);

            // 6. Añadir el TableLayoutPanel a la tarjeta
            card.Controls.Add(tlp);
            return card;
        }
    }
}