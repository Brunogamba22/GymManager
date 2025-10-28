using System;
using System.Drawing;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GymManager.Views
{
    partial class UcReportes
    {
        private Label lblTitulo;
        private Label lblTotalUsuarios;
        private Label lblTotalEjercicios;
        private Label lblUsuariosTxt;
        private Label lblEjerciciosTxt;
        private Chart chartUsuarios;
        private Chart chartEjercicios;
        private Panel cardUsuarios;
        private Panel cardEjercicios;
        private Button btnBackup; // 🔹 Botón de Backup

        private void InitializeComponent()
        {
            ChartArea areaUsuarios = new ChartArea("ChartArea1");
            ChartArea areaEjercicios = new ChartArea("ChartArea2");
            Legend legendUsuarios = new Legend("LegendUsuarios");
            Legend legendEjercicios = new Legend("LegendEjercicios");

            // ============================================================
            // 🧾 TÍTULO
            // ============================================================
            lblTitulo = new Label
            {
                Text = "📊 Reportes del Sistema",
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Location = new Point(30, 20),
                AutoSize = true
            };

            // ============================================================
            // 💾 BOTÓN BACKUP
            // ============================================================
            btnBackup = new Button
            {
                Name = "btnBackup",
                Text = "💾  Crear Backup",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                Size = new Size(150, 38),
                BackColor = Color.FromArgb(54, 162, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Cursor = Cursors.Hand,
                Location = new Point(900, 20), // 👈 más arriba y más a la derecha
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnBackup.FlatAppearance.BorderSize = 0;
            btnBackup.Click += new EventHandler(this.BtnBackup_Click);

            // ============================================================
            // 🧍 CARD USUARIOS
            // ============================================================
            cardUsuarios = new Panel
            {
                BackColor = Color.White,
                Size = new Size(260, 100),
                Location = new Point(40, 65),
                BorderStyle = BorderStyle.FixedSingle
            };

            lblUsuariosTxt = new Label
            {
                Text = "Total de usuarios",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(15, 10)
            };

            lblTotalUsuarios = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 30, FontStyle.Bold),
                ForeColor = Color.FromArgb(54, 162, 235),
                AutoSize = true,
                Location = new Point(25, 40)
            };

            var iconUsuarios = new Label
            {
                Text = "👥",
                Font = new Font("Segoe UI Emoji", 34, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(190, 38)
            };

            cardUsuarios.Controls.AddRange(new Control[] { lblUsuariosTxt, lblTotalUsuarios, iconUsuarios });

            // ============================================================
            // 💪 CARD EJERCICIOS
            // ============================================================
            cardEjercicios = new Panel
            {
                BackColor = Color.White,
                Size = new Size(260, 100),
                Location = new Point(320, 65),
                BorderStyle = BorderStyle.FixedSingle
            };

            lblEjerciciosTxt = new Label
            {
                Text = "Total de ejercicios",
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                ForeColor = Color.Gray,
                AutoSize = true,
                Location = new Point(15, 10)
            };

            lblTotalEjercicios = new Label
            {
                Text = "0",
                Font = new Font("Segoe UI", 30, FontStyle.Bold),
                ForeColor = Color.FromArgb(255, 159, 64),
                AutoSize = true,
                Location = new Point(25, 40)
            };

            var iconEjercicios = new Label
            {
                Text = "💪",
                Font = new Font("Segoe UI Emoji", 34, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(190, 38)
            };

            cardEjercicios.Controls.AddRange(new Control[] { lblEjerciciosTxt, lblTotalEjercicios, iconEjercicios });

            // ============================================================
            // 🥧 GRÁFICO USUARIOS (torta)
            // ============================================================
            chartUsuarios = new Chart
            {
                Location = new Point(40, 190),
                Size = new Size(510, 350),
                BackColor = Color.White
            };
            chartUsuarios.ChartAreas.Add(areaUsuarios);
            chartUsuarios.Legends.Add(legendUsuarios);

            // ============================================================
            // 📊 GRÁFICO EJERCICIOS (barras)
            // ============================================================
            chartEjercicios = new Chart
            {
                Location = new Point(560, 190),
                Size = new Size(530, 350),
                BackColor = Color.White
            };
            chartEjercicios.ChartAreas.Add(areaEjercicios);
            chartEjercicios.Legends.Add(legendEjercicios);

            // ============================================================
            // 🧱 USERCONTROL GENERAL
            // ============================================================
            this.BackColor = Color.FromArgb(245, 247, 250);
            this.Controls.Add(lblTitulo);
            this.Controls.Add(cardUsuarios);
            this.Controls.Add(cardEjercicios);
            this.Controls.Add(chartUsuarios);
            this.Controls.Add(chartEjercicios);
            this.Controls.Add(btnBackup); // 🔹 botón agregado al panel
            this.Name = "UcReportes";
            this.Size = new Size(1100, 600);
            this.Load += new EventHandler(this.UcReportes_Load);
        }
    }
}
