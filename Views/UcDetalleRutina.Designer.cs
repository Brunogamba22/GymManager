using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Views
{
    partial class UcDetalleRutina
    {
        private System.ComponentModel.IContainer components = null;

        private Panel mainPanel;
        private Panel headerPanel;
        private Panel contentPanel;
        private Panel footerPanel;
        private DataGridView dgvEjercicios;
        private Label lblTitulo;
        private Label lblDetalles;
        private Label lblContador;
        private Button btnCerrar;
        private Button btnImprimir;
        private Button btnExportar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.mainPanel = new Panel();
            this.headerPanel = new Panel();
            this.contentPanel = new Panel();
            this.footerPanel = new Panel();
            this.dgvEjercicios = new DataGridView();
            this.lblTitulo = new Label();
            this.lblDetalles = new Label();
            this.lblContador = new Label();
            this.btnCerrar = new Button();
            this.btnImprimir = new Button();
            this.btnExportar = new Button();

            this.SuspendLayout();
            this.Size = new Size(900, 650);

            // Main Panel
            this.mainPanel.Dock = DockStyle.Fill;
            this.mainPanel.BackColor = backgroundColor;
            this.mainPanel.Padding = new Padding(20);

            // Header Panel
            this.headerPanel.Dock = DockStyle.Top;
            this.headerPanel.Height = 120;
            this.headerPanel.BackColor = Color.White;
            this.headerPanel.Padding = new Padding(25, 20, 25, 15);

            // Content Panel
            this.contentPanel.Dock = DockStyle.Fill;
            this.contentPanel.BackColor = Color.Transparent;
            this.contentPanel.Padding = new Padding(0, 10, 0, 10);

            // Footer Panel
            this.footerPanel.Dock = DockStyle.Bottom;
            this.footerPanel.Height = 70;
            this.footerPanel.BackColor = Color.Transparent;

            // Título
            this.lblTitulo.Text = "Detalles de Rutina";
            this.lblTitulo.Dock = DockStyle.Top;
            this.lblTitulo.Height = 35;
            this.lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            this.lblTitulo.ForeColor = primaryColor;
            this.lblTitulo.TextAlign = ContentAlignment.MiddleLeft;

            // Detalles
            this.lblDetalles.Text = "Información de la rutina";
            this.lblDetalles.Dock = DockStyle.Top;
            this.lblDetalles.Height = 25;
            this.lblDetalles.Font = new Font("Segoe UI", 10);
            this.lblDetalles.ForeColor = Color.FromArgb(100, 100, 100);
            this.lblDetalles.TextAlign = ContentAlignment.MiddleLeft;

            // Contador
            this.lblContador.Text = "Ejercicios";
            this.lblContador.Dock = DockStyle.Top;
            this.lblContador.Height = 25;
            this.lblContador.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.lblContador.ForeColor = successColor;
            this.lblContador.TextAlign = ContentAlignment.MiddleLeft;
            this.lblContador.Padding = new Padding(0, 5, 0, 0);

            // DataGridView
            this.dgvEjercicios.Dock = DockStyle.Fill;
            this.dgvEjercicios.Margin = new Padding(0, 5, 0, 0);

            // Configurar columnas del DataGridView
            dgvEjercicios.Columns.Add("Ejercicio", "EJERCICIO");
            dgvEjercicios.Columns.Add("Series", "SERIES");
            dgvEjercicios.Columns.Add("Repeticiones", "REPETICIONES");
            dgvEjercicios.Columns.Add("Descanso", "DESCANSO (s)");

            // Botón Cerrar
            this.btnCerrar.Text = "❌ CERRAR";
            this.btnCerrar.Size = new Size(120, 40);

            // Botón Imprimir
            this.btnImprimir.Text = "🖨️ IMPRIMIR";
            this.btnImprimir.Size = new Size(120, 40);

            // Botón Exportar
            this.btnExportar.Text = "📤 EXPORTAR";
            this.btnExportar.Size = new Size(120, 40);

            // Agregar controles a los paneles
            this.headerPanel.Controls.Add(lblContador);
            this.headerPanel.Controls.Add(lblDetalles);
            this.headerPanel.Controls.Add(lblTitulo);

            this.contentPanel.Controls.Add(dgvEjercicios);

            // Panel de botones
            var panelBotones = new Panel();
            panelBotones.Dock = DockStyle.Fill;
            panelBotones.Controls.Add(btnExportar);
            panelBotones.Controls.Add(btnImprimir);
            panelBotones.Controls.Add(btnCerrar);
            this.footerPanel.Controls.Add(panelBotones);

            this.mainPanel.Controls.Add(contentPanel);
            this.mainPanel.Controls.Add(footerPanel);
            this.mainPanel.Controls.Add(headerPanel);

            this.Controls.Add(mainPanel);

            // Eventos
            this.btnCerrar.Click += new System.EventHandler(btnCerrar_Click);
            this.btnImprimir.Click += new System.EventHandler(btnImprimir_Click);
            this.btnExportar.Click += new System.EventHandler(btnExportar_Click);

            // Aplicar estilos después de la inicialización
            this.Load += (sender, e) => {
                StyleButton(btnCerrar, Color.FromArgb(108, 117, 125));
                StyleButton(btnImprimir, successColor);
                StyleButton(btnExportar, primaryColor);

                // Posicionar botones
                btnCerrar.Location = new Point(panelBotones.Width - btnCerrar.Width - 10,
                                             (panelBotones.Height - btnCerrar.Height) / 2);
                btnImprimir.Location = new Point(btnCerrar.Left - btnImprimir.Width - 10,
                                               (panelBotones.Height - btnImprimir.Height) / 2);
                btnExportar.Location = new Point(btnImprimir.Left - btnExportar.Width - 10,
                                               (panelBotones.Height - btnExportar.Height) / 2);
            };

            this.ResumeLayout(false);
        }
    }
}