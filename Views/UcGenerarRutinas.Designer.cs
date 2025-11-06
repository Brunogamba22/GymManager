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
        private Button btnLimpiarHombres, btnGuardarHombres;
        private Button btnLimpiarMujeres, btnGuardarMujeres;
        private Button btnLimpiarDeportistas, btnGuardarDeportistas;

        // --- 🔥 VUELVEN LOS CHECKEDLISTBOX ---
        private CheckedListBox chkListHombres, chkListMujeres, chkListDeportistas;

        // --- Controles de Objetivo ---
        private ComboBox cmbObjetivoHombres, cmbObjetivoMujeres, cmbObjetivoDeportistas;
        private Label lblObjetivoHombres, lblObjetivoMujeres, lblObjetivoDeportistas;

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

            this.cmbObjetivoHombres = new ComboBox();
            this.cmbObjetivoMujeres = new ComboBox();
            this.cmbObjetivoDeportistas = new ComboBox();
            this.lblObjetivoHombres = new Label();
            this.lblObjetivoMujeres = new Label();
            this.lblObjetivoDeportistas = new Label();

            this.chkListHombres = new CheckedListBox();
            this.chkListMujeres = new CheckedListBox();
            this.chkListDeportistas = new CheckedListBox();

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
            SetupRutinaPanel(panelHombres, "RUTINA PARA HOMBRES", dgvHombres = new DataGridView(), btnGenerarHombres = new Button(), lblTituloHombres = new Label(), chkListHombres, cmbObjetivoHombres, lblObjetivoHombres);

            this.panelMujeres = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(20) };
            SetupRutinaPanel(panelMujeres, "RUTINA PARA MUJERES", dgvMujeres = new DataGridView(), btnGenerarMujeres = new Button(), lblTituloMujeres = new Label(), chkListMujeres, cmbObjetivoMujeres, lblObjetivoMujeres);

            this.panelDeportistas = new Panel { Dock = DockStyle.Fill, BackColor = Color.White, Padding = new Padding(20) };
            SetupRutinaPanel(panelDeportistas, "RUTINA PARA DEPORTISTAS", dgvDeportistas = new DataGridView(), btnGenerarDeportistas = new Button(), lblTituloDeportistas = new Label(), chkListDeportistas, cmbObjetivoDeportistas, lblObjetivoDeportistas);

            this.contentPanel.Controls.AddRange(new Control[] { panelHombres, panelMujeres, panelDeportistas });
        }

        // --- 🔥 MODIFICADO: La firma vuelve a aceptar CheckedListBox ---
        private void SetupRutinaPanel(Panel panel, string titulo, DataGridView dgv, Button btnGenerar, Label lblTitulo, CheckedListBox chkListGrupos, ComboBox cmbObjetivo, Label lblObjetivo)
        {
            lblTitulo.Text = titulo;
            lblTitulo.Dock = DockStyle.Top;
            lblTitulo.Height = 35;
            lblTitulo.Font = new Font("Segoe UI", 12, FontStyle.Bold);
            lblTitulo.ForeColor = textColor;
            lblTitulo.TextAlign = ContentAlignment.MiddleLeft;

            var panelObjetivo = new Panel { Dock = DockStyle.Top, Height = 40, Padding = new Padding(0, 5, 0, 5) };

            cmbObjetivo.Name = "cmbObjetivo" + panel.Name;
            cmbObjetivo.Size = new Size(200, 25);
            cmbObjetivo.Font = new Font("Segoe UI", 10);
            cmbObjetivo.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbObjetivo.Dock = DockStyle.Left;

            lblObjetivo.Text = "Seleccionar Objetivo:";
            lblObjetivo.Font = new Font("Segoe UI", 10);
            lblObjetivo.ForeColor = textColor;
            lblObjetivo.Dock = DockStyle.Left;
            lblObjetivo.TextAlign = ContentAlignment.MiddleLeft;
            lblObjetivo.Padding = new Padding(0, 0, 5, 0);
            lblObjetivo.AutoSize = true;

            panelObjetivo.Controls.Add(cmbObjetivo);
            panelObjetivo.Controls.Add(lblObjetivo);

            var panelSelector = new Panel { Dock = DockStyle.Top, Height = 110, Padding = new Padding(0, 5, 0, 10) };
            var lblSelector = new Label { Text = "Seleccionar Grupos Musculares:", Dock = DockStyle.Top, Font = new Font("Segoe UI", 10), ForeColor = textColor, Height = 25 };

            var pnlListContainer = new Panel();
            pnlListContainer.Dock = DockStyle.Fill;
            pnlListContainer.Padding = new Padding(1);
            pnlListContainer.BackColor = Color.FromArgb(248, 249, 250);
            pnlListContainer.BorderStyle = BorderStyle.FixedSingle;

            chkListGrupos.Dock = DockStyle.Fill;
            chkListGrupos.Font = new Font("Segoe UI", 10);
            chkListGrupos.CheckOnClick = true;
            chkListGrupos.BorderStyle = BorderStyle.None;
            chkListGrupos.BackColor = Color.White;
            chkListGrupos.HorizontalScrollbar = false;
            chkListGrupos.SelectedIndexChanged += (sender, e) => {
                chkListGrupos.ClearSelected();
            };
            chkListGrupos.ItemCheck += (sender, e) => OnGrupoMuscular_ItemCheck(chkListGrupos, btnGenerar);

            pnlListContainer.Controls.Add(chkListGrupos);

            // --- 🔥 CORRECCIÓN DE ORDEN AQUÍ 🔥 ---
            // 1. Añadir el panel (Fill) primero.
            panelSelector.Controls.Add(pnlListContainer);
            // 2. Añadir el label (Top) después.
            panelSelector.Controls.Add(lblSelector);
            // 3. ¡LA LÍNEA "BringToFront()" FUE ELIMINADA!
            // --- FIN CORRECCIÓN ---

            var lblEstadoVacio = new Label { Text = "Marcá los grupos musculares que querés entrenar y hacé clic en 'Generar Rutina'.", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleCenter, Font = new Font("Segoe UI", 10), ForeColor = Color.LightGray };

            // ... (El resto de tu código para dgv, panelBotones, etc. no cambia) ...
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
            dgv.DefaultCellStyle = new DataGridViewCellStyle { SelectionBackColor = Color.White, SelectionForeColor = textColor, Font = new Font("Segoe UI", 9) };
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250);
            dgv.EnableHeadersVisualStyles = false;
            dgv.Columns.Add("Ejercicio", "EJERCICIO");
            dgv.Columns.Add("Series", "SERIES");
            dgv.Columns.Add("Repeticiones", "REPETICIONES");
            dgv.Columns.Add("Carga", "CARGA (%)");
            dgv.Columns[0].FillWeight = 40;
            dgv.Columns["Ejercicio"].ReadOnly = true;
            dgv.Columns["Series"].ReadOnly = true;
            dgv.Columns["Repeticiones"].ReadOnly = true;
            dgv.Columns["Carga"].ReadOnly = true;

            var panelBotones = new TableLayoutPanel { Dock = DockStyle.Bottom, Height = 60, Padding = new Padding(0, 10, 0, 0), ColumnCount = 3, RowCount = 1 };
            panelBotones.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            panelBotones.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            panelBotones.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));

            btnGenerar.Text = "GENERAR RUTINA";
            btnGenerar.Size = new Size(160, 45);
            btnGenerar.Enabled = false;

            var panelAccionesDerecha = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.LeftToRight, WrapContents = false, AutoSize = true };
            var btnGuardar = new Button { Text = "💾 GUARDAR", Enabled = false, Size = new Size(120, 45) };
            var btnLimpiar = new Button { Text = "🗑️ LIMPIAR", Enabled = false, Size = new Size(110, 45) };

            if (panel == panelHombres)
            {
                btnGenerar.Click += btnGenerarHombres_Click;
                btnGuardar.Click += btnGuardarHombres_Click;
                btnLimpiar.Click += btnLimpiarHombres_Click;
                btnGuardarHombres = btnGuardar;
                btnLimpiarHombres = btnLimpiar;
                StyleButton(btnGenerar, primaryColor);
            }
            else if (panel == panelMujeres)
            {
                btnGenerar.Click += btnGenerarMujeres_Click;
                btnGuardar.Click += btnGuardarMujeres_Click;
                btnLimpiar.Click += btnLimpiarMujeres_Click;
                btnGuardarMujeres = btnGuardar;
                btnLimpiarMujeres = btnLimpiar;
                StyleButton(btnGenerar, secondaryColor);
            }
            else
            {
                btnGenerar.Click += btnGenerarDeportistas_Click;
                btnGuardar.Click += btnGuardarDeportistas_Click;
                btnLimpiar.Click += btnLimpiarDeportistas_Click;
                btnGuardarDeportistas = btnGuardar;
                btnLimpiarDeportistas = btnLimpiar;
                StyleButton(btnGenerar, successColor);
            }

            StyleButton(btnGuardar, successColor);
            StyleButton(btnLimpiar, dangerColor);

            panelAccionesDerecha.Controls.AddRange(new Control[] { btnGuardar, btnLimpiar });
            panelBotones.Controls.Add(btnGenerar, 0, 0);
            panelBotones.Controls.Add(panelAccionesDerecha, 2, 0);

            panel.Controls.Add(dgv);
            panel.Controls.Add(lblEstadoVacio);
            panel.Controls.Add(panelBotones);
            panel.Controls.Add(panelSelector);
            panel.Controls.Add(panelObjetivo);
            panel.Controls.Add(lblTitulo);

            dgv.BringToFront();
        }
    }
}