using System.Drawing;
using System.Windows.Forms;
using System;

namespace GymManager.Views
{
    partial class UcGenerarRutinas
    {
        private System.ComponentModel.IContainer components = null;

        // Controles principales
        private Panel mainPanel;
        private Panel headerPanel;
        private Panel contentPanel;
        private Panel tabsContainer;

        // Tabs
        private Label lblTabHombres;
        private Label lblTabMujeres;
        private Label lblTabDeportistas;

        // Paneles de contenido
        private Panel panelHombres;
        private Panel panelMujeres;
        private Panel panelDeportistas;

        // DataGridViews
        private DataGridView dgvHombres;
        private DataGridView dgvMujeres;
        private DataGridView dgvDeportistas;

        // Botones
        private Button btnGenerarHombres;
        private Button btnGenerarMujeres;
        private Button btnGenerarDeportistas;

        // Títulos
        private Label lblTituloHombres;
        private Label lblTituloMujeres;
        private Label lblTituloDeportistas;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) { components.Dispose(); }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.mainPanel = new Panel();
            this.headerPanel = new Panel();
            this.tabsContainer = new Panel();
            this.contentPanel = new Panel();

            // Configurar paneles principales
            SetupMainPanels();

            // Configurar tabs
            SetupTabs();

            // Configurar paneles de contenido
            SetupContentPanels();

            this.SuspendLayout();

            // 
            // UcGenerarRutinas
            // 
            this.Controls.Add(this.mainPanel);
            this.Name = "UcGenerarRutinas";
            this.Size = new System.Drawing.Size(900, 700);
            this.BackColor = Color.FromArgb(248, 249, 250);

            this.ResumeLayout(false);

            // Aplicar estilos después de inicializar
            this.Load += (sender, e) =>
            {
                StyleButton(btnGenerarHombres, primaryColor);
                StyleButton(btnGenerarMujeres, secondaryColor);
                StyleButton(btnGenerarDeportistas, successColor);

                // 🔥 APLICAR ESTILOS A LOS BOTONES DE ACCIÓN
                if (btnEditarHombres != null) StyleButton(btnEditarHombres, Color.FromArgb(255, 193, 7)); // Amarillo
                if (btnLimpiarHombres != null) StyleButton(btnLimpiarHombres, dangerColor); // Rojo
                if (btnEditarMujeres != null) StyleButton(btnEditarMujeres, Color.FromArgb(255, 193, 7));
                if (btnLimpiarMujeres != null) StyleButton(btnLimpiarMujeres, dangerColor);
                if (btnEditarDeportistas != null) StyleButton(btnEditarDeportistas, Color.FromArgb(255, 193, 7));
                if (btnLimpiarDeportistas != null) StyleButton(btnLimpiarDeportistas, dangerColor);
            };
        }

        private void SetupMainPanels()
        {
            // Panel principal
            this.mainPanel.Dock = DockStyle.Fill;
            this.mainPanel.BackColor = backgroundColor;
            this.mainPanel.Padding = new Padding(20);

            // Header panel (para las tabs)
            this.headerPanel.Dock = DockStyle.Top;
            this.headerPanel.Height = 60;
            this.headerPanel.BackColor = Color.White;
            this.headerPanel.Padding = new Padding(20, 15, 20, 0);

            // Content panel
            this.contentPanel.Dock = DockStyle.Fill;
            this.contentPanel.BackColor = Color.Transparent;

            // Agregar al main panel
            this.mainPanel.Controls.Add(this.contentPanel);
            this.mainPanel.Controls.Add(this.headerPanel);
        }

        private void SetupTabs()
        {
            this.tabsContainer.Dock = DockStyle.Top;
            this.tabsContainer.Height = 45;
            this.tabsContainer.BackColor = Color.White;

            // Tab Hombres
            this.lblTabHombres = new Label();
            this.lblTabHombres.Text = "🏋️  HOMBRES";
            this.lblTabHombres.Size = new Size(120, 45);
            this.lblTabHombres.Location = new Point(0, 0);
            this.lblTabHombres.TextAlign = ContentAlignment.MiddleCenter;
            this.lblTabHombres.Cursor = Cursors.Hand;
            this.lblTabHombres.Click += lblTabHombres_Click;
            this.lblTabHombres.MouseEnter += TabLabel_MouseEnter;
            this.lblTabHombres.MouseLeave += TabLabel_MouseLeave;

            // Tab Mujeres
            this.lblTabMujeres = new Label();
            this.lblTabMujeres.Text = "💪  MUJERES";
            this.lblTabMujeres.Size = new Size(120, 45);
            this.lblTabMujeres.Location = new Point(120, 0);
            this.lblTabMujeres.TextAlign = ContentAlignment.MiddleCenter;
            this.lblTabMujeres.Cursor = Cursors.Hand;
            this.lblTabMujeres.Click += lblTabMujeres_Click;
            this.lblTabMujeres.MouseEnter += TabLabel_MouseEnter;
            this.lblTabMujeres.MouseLeave += TabLabel_MouseLeave;

            // Tab Deportistas
            this.lblTabDeportistas = new Label();
            this.lblTabDeportistas.Text = "⚡  DEPORTISTAS";
            this.lblTabDeportistas.Size = new Size(140, 45);
            this.lblTabDeportistas.Location = new Point(240, 0);
            this.lblTabDeportistas.TextAlign = ContentAlignment.MiddleCenter;
            this.lblTabDeportistas.Cursor = Cursors.Hand;
            this.lblTabDeportistas.Click += lblTabDeportistas_Click;
            this.lblTabDeportistas.MouseEnter += TabLabel_MouseEnter;
            this.lblTabDeportistas.MouseLeave += TabLabel_MouseLeave;

            // Agregar tabs al container
            this.tabsContainer.Controls.Add(lblTabHombres);
            this.tabsContainer.Controls.Add(lblTabMujeres);
            this.tabsContainer.Controls.Add(lblTabDeportistas);

            // Agregar al header
            this.headerPanel.Controls.Add(this.tabsContainer);
        }

        private void SetupContentPanels()
        {
            // Panel Hombres
            this.panelHombres = new Panel();
            this.panelHombres.Dock = DockStyle.Fill;
            this.panelHombres.BackColor = Color.White;
            this.panelHombres.Padding = new Padding(15);
            SetupRutinaPanel(panelHombres, "RUTINA PARA HOMBRES", dgvHombres = new DataGridView(),
                           btnGenerarHombres = new Button(), lblTituloHombres = new Label());

            // Panel Mujeres
            this.panelMujeres = new Panel();
            this.panelMujeres.Dock = DockStyle.Fill;
            this.panelMujeres.BackColor = Color.White;
            this.panelMujeres.Padding = new Padding(15);
            SetupRutinaPanel(panelMujeres, "RUTINA PARA MUJERES", dgvMujeres = new DataGridView(),
                           btnGenerarMujeres = new Button(), lblTituloMujeres = new Label());

            // Panel Deportistas
            this.panelDeportistas = new Panel();
            this.panelDeportistas.Dock = DockStyle.Fill;
            this.panelDeportistas.BackColor = Color.White;
            this.panelDeportistas.Padding = new Padding(15);
            SetupRutinaPanel(panelDeportistas, "RUTINA PARA DEPORTISTAS", dgvDeportistas = new DataGridView(),
                           btnGenerarDeportistas = new Button(), lblTituloDeportistas = new Label());

            // Agregar al content panel
            this.contentPanel.Controls.Add(panelHombres);
            this.contentPanel.Controls.Add(panelMujeres);
            this.contentPanel.Controls.Add(panelDeportistas);
        }

        private void SetupRutinaPanel(Panel panel, string titulo, DataGridView dgv, Button btnGenerar, Label lblTitulo)
        {
            // Título
            lblTitulo.Text = titulo;
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Height = 40;
            lblTitulo.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTitulo.ForeColor = textColor;
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;

            // DataGridView
            dgv.Dock = DockStyle.Fill;
            dgv.Margin = new Padding(0, 5, 0, 10);

            // Configurar columnas
            dgv.Columns.Add("Ejercicio", "EJERCICIO");
            dgv.Columns.Add("Series", "SERIES");
            dgv.Columns.Add("Repeticiones", "REPETICIONES");
            dgv.Columns.Add("Descanso", "DESCANSO (s)");

            // 🔥 PANEL DE BOTONES (Generar, Editar, Limpiar)
            var panelBotones = new Panel();
            panelBotones.Dock = DockStyle.Bottom;
            panelBotones.Height = 50;
            panelBotones.Padding = new Padding(0, 5, 0, 0);

            // Botón Generar
            btnGenerar.Text = "GENERAR RUTINA";
            btnGenerar.Height = 40;
            btnGenerar.Dock = DockStyle.Left;
            btnGenerar.Width = 150;

            // 🔥 BOTÓN EDITAR
            var btnEditar = new Button();
            btnEditar.Text = "EDITAR";
            btnEditar.Height = 40;
            btnEditar.Dock = DockStyle.Left;
            btnEditar.Width = 100;
            btnEditar.Margin = new Padding(5, 0, 0, 0);
            btnEditar.Enabled = false; // Inicialmente deshabilitado

            // 🔥 BOTÓN LIMPIAR
            var btnLimpiar = new Button();
            btnLimpiar.Text = "LIMPIAR";
            btnLimpiar.Height = 40;
            btnLimpiar.Dock = DockStyle.Left;
            btnLimpiar.Width = 100;
            btnLimpiar.Margin = new Padding(5, 0, 0, 0);
            btnLimpiar.Enabled = false; // Inicialmente deshabilitado

            // 🔥 ASIGNAR LOS BOTONES A LAS VARIABLES GLOBALES SEGÚN EL PANEL
            if (panel == panelHombres)
            {
                btnGenerar.Click += btnGenerarHombres_Click;
                btnEditar.Click += btnEditarHombres_Click;
                btnLimpiar.Click += btnLimpiarHombres_Click;
                btnEditarHombres = btnEditar;
                btnLimpiarHombres = btnLimpiar;
            }
            else if (panel == panelMujeres)
            {
                btnGenerar.Click += btnGenerarMujeres_Click;
                btnEditar.Click += btnEditarMujeres_Click;
                btnLimpiar.Click += btnLimpiarMujeres_Click;
                btnEditarMujeres = btnEditar;
                btnLimpiarMujeres = btnLimpiar;
            }
            else if (panel == panelDeportistas)
            {
                btnGenerar.Click += btnGenerarDeportistas_Click;
                btnEditar.Click += btnEditarDeportistas_Click;
                btnLimpiar.Click += btnLimpiarDeportistas_Click;
                btnEditarDeportistas = btnEditar;
                btnLimpiarDeportistas = btnLimpiar;
            }

            // Agregar botones al panel de botones
            panelBotones.Controls.Add(btnLimpiar);
            panelBotones.Controls.Add(btnEditar);
            panelBotones.Controls.Add(btnGenerar);

            // Agregar controles al panel principal
            panel.Controls.Add(dgv);
            panel.Controls.Add(panelBotones);
            panel.Controls.Add(lblTitulo);
        }
    }
}