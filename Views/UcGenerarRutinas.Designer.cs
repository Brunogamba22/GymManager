using System.Drawing;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using GymManager.Models;

namespace GymManager.Views
{
    partial class UcGenerarRutinas
    {
        private System.ComponentModel.IContainer components = null;

        // Controles principales
        private Panel mainPanel, headerPanel, contentPanel, tabsContainer;
        private Label lblTabHombres, lblTabMujeres, lblTabDeportistas;
        private Panel panelHombres, panelMujeres, panelDeportistas;
        private DataGridView dgvHombres, dgvMujeres, dgvDeportistas;
        private Button btnGenerarHombres, btnGenerarMujeres, btnGenerarDeportistas;
        private Label lblTituloHombres, lblTituloMujeres, lblTituloDeportistas;

        // Controles para selección múltiple
        private CheckedListBox chkListHombres, chkListMujeres, chkListDeportistas;

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

            SetupMainPanels();
            SetupTabs();
            SetupContentPanels();

            this.SuspendLayout();
            this.Controls.Add(this.mainPanel);
            this.Name = "UcGenerarRutinas";
            this.Size = new System.Drawing.Size(900, 700);
            this.BackColor = Color.FromArgb(248, 249, 250);
            this.ResumeLayout(false);
        }

        private void SetupMainPanels()
        {
            this.mainPanel.Dock = DockStyle.Fill;
            this.mainPanel.BackColor = backgroundColor;
            this.mainPanel.Padding = new Padding(20);

            this.headerPanel.Dock = DockStyle.Top;
            this.headerPanel.Height = 60;
            this.headerPanel.BackColor = Color.White;
            this.headerPanel.Padding = new Padding(20, 15, 20, 0);

            this.contentPanel.Dock = DockStyle.Fill;
            this.contentPanel.BackColor = Color.Transparent;

            this.mainPanel.Controls.Add(this.contentPanel);
            this.mainPanel.Controls.Add(this.headerPanel);
        }

        private void SetupTabs()
        {
            this.tabsContainer.Dock = DockStyle.Top;
            this.tabsContainer.Height = 45;
            this.tabsContainer.BackColor = Color.White;

            this.lblTabHombres = new Label { Text = "🏋️ HOMBRES", Size = new Size(120, 45), Location = new Point(0, 0), TextAlign = ContentAlignment.MiddleCenter, Cursor = Cursors.Hand };
            this.lblTabHombres.Click += lblTabHombres_Click;
            this.lblTabHombres.MouseEnter += TabLabel_MouseEnter;
            this.lblTabHombres.MouseLeave += TabLabel_MouseLeave;

            this.lblTabMujeres = new Label { Text = "💪 MUJERES", Size = new Size(120, 45), Location = new Point(120, 0), TextAlign = ContentAlignment.MiddleCenter, Cursor = Cursors.Hand };
            this.lblTabMujeres.Click += lblTabMujeres_Click;
            this.lblTabMujeres.MouseEnter += TabLabel_MouseEnter;
            this.lblTabMujeres.MouseLeave += TabLabel_MouseLeave;

            this.lblTabDeportistas = new Label { Text = "⚡ DEPORTISTAS", Size = new Size(140, 45), Location = new Point(240, 0), TextAlign = ContentAlignment.MiddleCenter, Cursor = Cursors.Hand };
            this.lblTabDeportistas.Click += lblTabDeportistas_Click;
            this.lblTabDeportistas.MouseEnter += TabLabel_MouseEnter;
            this.lblTabDeportistas.MouseLeave += TabLabel_MouseLeave;

            this.tabsContainer.Controls.AddRange(new Control[] { lblTabHombres, lblTabMujeres, lblTabDeportistas });
            this.headerPanel.Controls.Add(this.tabsContainer);
        }

        private void SetupContentPanels()
        {
            this.panelHombres = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(20) };
            SetupRutinaPanel(panelHombres, "RUTINA PARA HOMBRES", dgvHombres = new DataGridView(), btnGenerarHombres = new Button(), lblTituloHombres = new Label(), chkListHombres = new CheckedListBox());

            this.panelMujeres = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(20) };
            SetupRutinaPanel(panelMujeres, "RUTINA PARA MUJERES", dgvMujeres = new DataGridView(), btnGenerarMujeres = new Button(), lblTituloMujeres = new Label(), chkListMujeres = new CheckedListBox());

            this.panelDeportistas = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(20) };
            SetupRutinaPanel(panelDeportistas, "RUTINA PARA DEPORTISTAS", dgvDeportistas = new DataGridView(), btnGenerarDeportistas = new Button(), lblTituloDeportistas = new Label(), chkListDeportistas = new CheckedListBox());

            this.contentPanel.Controls.AddRange(new Control[] { panelHombres, panelMujeres, panelDeportistas });
        }

        private void SetupRutinaPanel(Panel panel, string titulo, DataGridView dgv, Button btnGenerar, Label lblTitulo, CheckedListBox chkListGrupos)
        {
            lblTitulo.Text = titulo;
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Height = 35;
            lblTitulo.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTitulo.ForeColor = textColor;
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;

            var panelSelector = new Panel { Dock = DockStyle.Top, Height = 130, Padding = new Padding(0, 5, 0, 10) };
            var lblSelector = new Label { Text = "Seleccionar Grupos Musculares:", Dock = DockStyle.Top, Font = new Font("Segoe UI", 10), ForeColor = textColor, Height = 25 };

            chkListGrupos.Dock = DockStyle.Fill;
            chkListGrupos.Font = new Font("Segoe UI", 10);
            chkListGrupos.CheckOnClick = true;
            chkListGrupos.BorderStyle = BorderStyle.FixedSingle;
            // chkListGrupos.SelectionMode = SelectionMode.None; // <-- REMOVÉ ESTA LÍNEA O COMENTALA
            chkListGrupos.ItemCheck += (sender, e) => OnGrupoMuscular_ItemCheck(chkListGrupos, btnGenerar);
            chkListGrupos.SelectedIndexChanged += (sender, e) => { chkListGrupos.ClearSelected(); }; // <-- ¡AGREGÁ ESTA LÍNEA MÁGICA!
            panelSelector.Controls.AddRange(new Control[] { chkListGrupos, lblSelector });

            var lblEstadoVacio = new Label { Text = "Marcá los grupos musculares que querés entrenar y hacé clic en 'Generar Rutina'.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 10), ForeColor = Color.LightGray };

            dgv.Visible = false;
            dgv.Dock = DockStyle.Fill;
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.GridColor = borderColor;
            dgv.RowHeadersVisible = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgv.ColumnHeadersDefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.FromArgb(248, 249, 250), ForeColor = textColor, Font = new Font("Segoe UI", 9, FontStyle.Bold), Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = new Padding(10, 0, 10, 0) };
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dgv.ColumnHeadersHeight = 40;
            dgv.DefaultCellStyle = new DataGridViewCellStyle { BackColor = Color.White, ForeColor = textColor, Font = new Font("Segoe UI", 9), SelectionBackColor = primaryColor, SelectionForeColor = Color.White, Alignment = DataGridViewContentAlignment.MiddleLeft, Padding = new Padding(10, 0, 10, 0) };
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dgv.EnableHeadersVisualStyles = false;
            dgv.Columns.Add("Ejercicio", "EJERCICIO");
            dgv.Columns.Add("Series", "SERIES");
            dgv.Columns.Add("Repeticiones", "REPETICIONES");
            dgv.Columns.Add("Descanso", "DESCANSO (s)");
            dgv.Columns[0].FillWeight = 40;

            var panelBotones = new TableLayoutPanel { Dock = DockStyle.Bottom, Height = 60, Padding = new Padding(0, 10, 0, 0), ColumnCount = 3, RowCount = 1 };
            panelBotones.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            panelBotones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            panelBotones.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            btnGenerar.Text = "GENERAR RUTINA";
            btnGenerar.Size = new Size(160, 45);
            btnGenerar.Enabled = false;

            var panelAccionesDerecha = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, AutoSize = true };
            var btnEditar = new Button { Text = "✏️ EDITAR", Enabled = false, Size = new Size(110, 45) };
            var btnGuardar = new Button { Text = "💾 GUARDAR", Enabled = false, Size = new Size(120, 45) };
            var btnLimpiar = new Button { Text = "🗑️ LIMPIAR", Enabled = false, Size = new Size(110, 45) };

            if (panel == panelHombres)
            {
                btnGenerar.Click += btnGenerarHombres_Click;
                btnEditar.Click += btnEditarHombres_Click;
                btnGuardar.Click += btnGuardarHombres_Click;
                btnLimpiar.Click += btnLimpiarHombres_Click;
                btnEditarHombres = btnEditar;
                btnGuardarHombres = btnGuardar;
                btnLimpiarHombres = btnLimpiar;
                StyleButton(btnGenerar, primaryColor);
            }
            else if (panel == panelMujeres)
            {
                btnGenerar.Click += btnGenerarMujeres_Click;
                btnEditar.Click += btnEditarMujeres_Click;
                btnGuardar.Click += btnGuardarMujeres_Click;
                btnLimpiar.Click += btnLimpiarMujeres_Click;
                btnEditarMujeres = btnEditar;
                btnGuardarMujeres = btnGuardar;
                btnLimpiarMujeres = btnLimpiar;
                StyleButton(btnGenerar, secondaryColor);
            }
            else
            {
                btnGenerar.Click += btnGenerarDeportistas_Click;
                btnEditar.Click += btnEditarDeportistas_Click;
                btnGuardar.Click += btnGuardarDeportistas_Click;
                btnLimpiar.Click += btnLimpiarDeportistas_Click;
                btnEditarDeportistas = btnEditar;
                btnGuardarDeportistas = btnGuardar;
                btnLimpiarDeportistas = btnLimpiar;
                StyleButton(btnGenerar, successColor);
            }

            StyleButton(btnEditar, warningColor);
            StyleButton(btnGuardar, successColor);
            StyleButton(btnLimpiar, dangerColor);

            panelAccionesDerecha.Controls.AddRange(new Control[] { btnEditar, btnGuardar, btnLimpiar });
            panelBotones.Controls.Add(btnGenerar, 0, 0);
            panelBotones.Controls.Add(panelAccionesDerecha, 2, 0);

            panel.Controls.Add(dgv);
            panel.Controls.Add(lblEstadoVacio);
            panel.Controls.Add(panelBotones);
            panel.Controls.Add(panelSelector);
            panel.Controls.Add(lblTitulo);

            dgv.BringToFront();
        }
    }
}