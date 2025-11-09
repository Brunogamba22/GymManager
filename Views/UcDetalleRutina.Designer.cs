using System.Drawing;
using System.Windows.Forms;
using System;
using System.Collections.Generic;
using GymManager.Models;

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
        private Button btnModoTV;


        private DataGridViewTextBoxColumn colEjercicio;
        private DataGridViewTextBoxColumn colSeries;
        private DataGridViewTextBoxColumn colRepeticiones;
        private DataGridViewTextBoxColumn colCarga; 
        private DataGridViewTextBoxColumn colDescanso;

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
            this.btnModoTV = new Button(); // 🖥️ Nuevo botón

            this.colEjercicio = new DataGridViewTextBoxColumn();
            this.colSeries = new DataGridViewTextBoxColumn();
            this.colRepeticiones = new DataGridViewTextBoxColumn();
            this.colCarga = new DataGridViewTextBoxColumn();

            this.SuspendLayout();
            this.Size = new Size(900, 650);

            // MAIN PANEL
            this.mainPanel.Dock = DockStyle.Fill;
            this.mainPanel.BackColor = backgroundColor;
            this.mainPanel.Padding = new Padding(20);

            // HEADER
            this.headerPanel.Dock = DockStyle.Top;
            this.headerPanel.Height = 120;
            this.headerPanel.BackColor = Color.White;
            this.headerPanel.Padding = new Padding(25, 20, 25, 15);

            // CONTENT
            this.contentPanel.Dock = DockStyle.Fill;
            this.contentPanel.Padding = new Padding(0, 10, 0, 10);

            // FOOTER
            this.footerPanel.Dock = DockStyle.Bottom;
            this.footerPanel.Height = 70;
            this.footerPanel.BackColor = Color.Transparent;
            this.footerPanel.Padding = new Padding(20, 10, 25, 10);

            // TITULO
            this.lblTitulo.Text = "Detalles de Rutina";
            this.lblTitulo.Dock = DockStyle.Top;
            this.lblTitulo.Height = 35;
            this.lblTitulo.Font = new Font("Segoe UI", 16, FontStyle.Bold);
            this.lblTitulo.ForeColor = primaryColor;
            this.lblTitulo.TextAlign = ContentAlignment.MiddleLeft;

            // DETALLES
            this.lblDetalles.Text = "Información de la rutina";
            this.lblDetalles.Dock = DockStyle.Top;
            this.lblDetalles.Height = 25;
            this.lblDetalles.Font = new Font("Segoe UI", 10);
            this.lblDetalles.ForeColor = Color.FromArgb(100, 100, 100);
            this.lblDetalles.TextAlign = ContentAlignment.MiddleLeft;

            // CONTADOR
            this.lblContador.Text = "Ejercicios";
            this.lblContador.Dock = DockStyle.Top;
            this.lblContador.Height = 25;
            this.lblContador.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.lblContador.ForeColor = successColor;
            this.lblContador.TextAlign = ContentAlignment.MiddleLeft;
            this.lblContador.Padding = new Padding(0, 5, 0, 0);

            // DATAGRIDVIEW
            this.dgvEjercicios.Dock = DockStyle.Fill;
            this.dgvEjercicios.Margin = new Padding(0, 5, 0, 0);
            this.dgvEjercicios.Columns.AddRange(new DataGridViewColumn[]
            {
        this.colEjercicio,
        this.colSeries,
        this.colRepeticiones,
        this.colCarga
            });

            // COLUMNAS
            this.colEjercicio.HeaderText = "EJERCICIO";
            this.colEjercicio.Name = "colEjercicio";      
            this.colEjercicio.FillWeight = 35;

            this.colSeries.HeaderText = "SERIES";
            this.colSeries.Name = "colSeries";          
            this.colSeries.FillWeight = 15;

            this.colRepeticiones.HeaderText = "REPETICIONES";
            this.colRepeticiones.Name = "colRepeticiones";
            this.colRepeticiones.FillWeight = 15;

            this.colCarga.HeaderText = "CARGA (%)";
            this.colCarga.Name = "colCarga";             
            this.colCarga.FillWeight = 15;

            // BOTONES BASE
            int btnW = 150, btnH = 40;

            this.btnExportar.Text = "📤 EXPORTAR";
            this.btnExportar.Size = new Size(btnW, btnH);
            StyleButton(btnExportar, primaryColor);
            this.btnExportar.Click += btnExportar_Click;

            this.btnImprimir.Text = "🖨️ IMPRIMIR";
            this.btnImprimir.Size = new Size(btnW, btnH);
            StyleButton(btnImprimir, successColor);
            this.btnImprimir.Click += btnImprimir_Click;

            this.btnModoTV.Text = "🖥️ MODO TV";
            this.btnModoTV.Size = new Size(btnW, btnH);
            StyleButton(btnModoTV, Color.FromArgb(52, 73, 94));
            this.btnModoTV.Click += (s, e) =>
            {
                if (RutinaActual == null)
                {
                    MessageBox.Show("No hay rutina cargada para mostrar en modo TV.",
                        "Sin datos", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                var detalles = new List<DetalleRutina>();
                foreach (DataGridViewRow row in dgvEjercicios.Rows)
                {
                    if (row.Cells["colEjercicio"].Value == null) continue;
                    detalles.Add(new DetalleRutina
                    {
                        EjercicioNombre = row.Cells["colEjercicio"].Value.ToString(),
                        Series = Convert.ToInt32(row.Cells["colSeries"].Value ?? 0),
                        Repeticiones = Convert.ToInt32(row.Cells["colRepeticiones"].Value ?? 0),
                        Carga = row.Cells["colCarga"].Value?.ToString()
                    });
                }

                FormTV form = new FormTV(
                    RutinaActual.NombreProfesor,
                    RutinaActual.Nombre,
                    RutinaActual.NombreGenero,
                    detalles
                );
                form.Show();
            };

            this.btnCerrar.Text = "❌ CERRAR";
            this.btnCerrar.Size = new Size(btnW, btnH);
            StyleButton(btnCerrar, Color.FromArgb(108, 117, 125));
            this.btnCerrar.Click += btnCerrar_Click;

            // AGREGAR BOTONES AL FOOTER
            this.footerPanel.Controls.Add(btnCerrar);
            this.footerPanel.Controls.Add(btnExportar);
            this.footerPanel.Controls.Add(btnImprimir);
            this.footerPanel.Controls.Add(btnModoTV);

            // 📏 Reposicionar dinámicamente al redimensionar
            this.footerPanel.Resize += (s, e) =>
            {
                int spacing = 10;
                int bottom = (footerPanel.Height - btnCerrar.Height) / 2;
                int marginRight = 25;

                btnModoTV.Location = new Point(footerPanel.Width - btnModoTV.Width - marginRight, bottom);
                btnImprimir.Location = new Point(btnModoTV.Left - btnImprimir.Width - spacing, bottom);
                btnExportar.Location = new Point(btnImprimir.Left - btnExportar.Width - spacing, bottom);
                btnCerrar.Location = new Point(marginRight, bottom);
            };



            // ARMADO FINAL
            this.headerPanel.Controls.Add(lblContador);
            this.headerPanel.Controls.Add(lblDetalles);
            this.headerPanel.Controls.Add(lblTitulo);
            this.contentPanel.Controls.Add(dgvEjercicios);
            this.mainPanel.Controls.Add(contentPanel);
            this.mainPanel.Controls.Add(footerPanel);
            this.mainPanel.Controls.Add(headerPanel);
            this.Controls.Add(mainPanel);

            this.ResumeLayout(false);
        }
    }
}