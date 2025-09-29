using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Views
{
    partial class UcRecepcionistaDashboard
    {
        private System.ComponentModel.IContainer components = null;
        private Label lblTitulo;
        private ComboBox cmbFecha;
        private DataGridView dgvRutina;
        private Button btnPantallaCompleta;
        private Button btnExportar;
        private Button btnImprimir;
        private Label lblContadorEjercicios;
        private Panel panelHeader;
        private Panel panelContenido;
        private Panel panelBotones;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) { components.Dispose(); }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblTitulo = new Label();
            this.cmbFecha = new ComboBox();
            this.dgvRutina = new DataGridView();
            this.btnPantallaCompleta = new Button();
            this.btnExportar = new Button();
            this.btnImprimir = new Button();
            this.lblContadorEjercicios = new Label();
            this.panelHeader = new Panel();
            this.panelContenido = new Panel();
            this.panelBotones = new Panel();

            ((System.ComponentModel.ISupportInitialize)(this.dgvRutina)).BeginInit();
            this.panelHeader.SuspendLayout();
            this.panelContenido.SuspendLayout();
            this.panelBotones.SuspendLayout();
            this.SuspendLayout();

            // 
            // panelHeader
            // 
            this.panelHeader.Dock = DockStyle.Top;
            this.panelHeader.Height = 120;
            this.panelHeader.BackColor = Color.White;
            this.panelHeader.Padding = new Padding(30, 20, 30, 15);

            // lblTitulo
            this.lblTitulo.Text = "Rutina Diaria";
            this.lblTitulo.Dock = DockStyle.Top;
            this.lblTitulo.Height = 40;
            this.lblTitulo.Font = new Font("Segoe UI", 18, FontStyle.Bold);
            this.lblTitulo.ForeColor = primaryColor;
            this.lblTitulo.TextAlign = ContentAlignment.MiddleLeft;

            // lblContadorEjercicios
            this.lblContadorEjercicios.Text = "Ejercicios";
            this.lblContadorEjercicios.Dock = DockStyle.Top;
            this.lblContadorEjercicios.Height = 25;
            this.lblContadorEjercicios.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.lblContadorEjercicios.ForeColor = successColor;
            this.lblContadorEjercicios.TextAlign = ContentAlignment.MiddleLeft;
            this.lblContadorEjercicios.Padding = new Padding(0, 5, 0, 0);

            // cmbFecha
            this.cmbFecha.Dock = DockStyle.Top;
            this.cmbFecha.Height = 35;
            this.cmbFecha.Font = new Font("Segoe UI", 10);
            this.cmbFecha.DropDownStyle = ComboBoxStyle.DropDownList;
            this.cmbFecha.FlatStyle = FlatStyle.Flat;
            this.cmbFecha.SelectedIndexChanged += new EventHandler(this.cmbFecha_SelectedIndexChanged);

            // 
            // panelContenido
            // 
            this.panelContenido.Dock = DockStyle.Fill;
            this.panelContenido.BackColor = Color.Transparent;
            this.panelContenido.Padding = new Padding(30, 20, 30, 20);

            // dgvRutina
            this.dgvRutina.Dock = DockStyle.Fill;
            this.dgvRutina.Margin = new Padding(0, 10, 0, 0);

            // 
            // panelBotones
            // 
            this.panelBotones.Dock = DockStyle.Bottom;
            this.panelBotones.Height = 80;
            this.panelBotones.BackColor = Color.Transparent;
            this.panelBotones.Padding = new Padding(30, 15, 30, 15);

            // btnPantallaCompleta
            this.btnPantallaCompleta.Text = "📺 PANTALLA COMPLETA";
            this.btnPantallaCompleta.Size = new Size(160, 45);
            this.btnPantallaCompleta.Click += new EventHandler(this.btnPantallaCompleta_Click);

            // btnExportar
            this.btnExportar.Text = "📤 EXPORTAR PDF";
            this.btnExportar.Size = new Size(140, 45);
            this.btnExportar.Click += new EventHandler(this.btnExportar_Click);

            // btnImprimir
            this.btnImprimir.Text = "🖨️ IMPRIMIR";
            this.btnImprimir.Size = new Size(120, 45);
            this.btnImprimir.Click += new EventHandler(this.btnImprimir_Click);

            // Agregar controles a los paneles
            this.panelHeader.Controls.Add(this.lblContadorEjercicios);
            this.panelHeader.Controls.Add(this.cmbFecha);
            this.panelHeader.Controls.Add(this.lblTitulo);

            this.panelContenido.Controls.Add(this.dgvRutina);

            // Configurar panel de botones
            this.panelBotones.Controls.Add(this.btnPantallaCompleta);
            this.panelBotones.Controls.Add(this.btnExportar);
            this.panelBotones.Controls.Add(this.btnImprimir);

            // Agregar paneles al UserControl
            this.Controls.Add(this.panelContenido);
            this.Controls.Add(this.panelBotones);
            this.Controls.Add(this.panelHeader);

            this.Dock = DockStyle.Fill;

            ((System.ComponentModel.ISupportInitialize)(this.dgvRutina)).EndInit();
            this.panelHeader.ResumeLayout(false);
            this.panelContenido.ResumeLayout(false);
            this.panelBotones.ResumeLayout(false);
            this.ResumeLayout(false);

            // Aplicar estilos después de la inicialización
            this.Load += (sender, e) =>
            {
                StyleButton(btnPantallaCompleta, primaryColor);
                StyleButton(btnExportar, successColor);
                StyleButton(btnImprimir, Color.FromArgb(108, 117, 125));

                // 🔥 USAR EL NUEVO MÉTODO PARA POSICIONAR BOTONES
                ReposicionarBotones();
            };

            // 🔥 NUEVO: Reposicionar botones cuando cambie el tamaño
            this.panelBotones.SizeChanged += (sender, e) => ReposicionarBotones();
            this.SizeChanged += (sender, e) => ReposicionarBotones();
        }
    }
}