using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GymManager.Controllers;
using GymManager.Models;

namespace GymManager.Views
{
    public partial class UcReportes : UserControl
    {
        private readonly UsuarioController controladorUsuarios = new UsuarioController();
        private readonly EjercicioController controladorEjercicios = new EjercicioController();

        public UcReportes()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(245, 247, 250); // fondo gris claro
        }

        private void UcReportes_Load(object sender, EventArgs e)
        {
            CargarDatosGenerales();
            ConfigurarGraficos();
            ConfigurarCards();
            CargarGraficoUsuarios();
            CargarGraficoEjercicios();
        }

        // ============================================================
        // 🔹 DATOS GENERALES
        // ============================================================
        private void CargarDatosGenerales()
        {
            int totalUsuarios = controladorUsuarios.ObtenerTodos().Count;
            int totalEjercicios = controladorEjercicios.ObtenerTodos().Count;

            lblTotalUsuarios.Text = totalUsuarios.ToString();
            lblTotalEjercicios.Text = totalEjercicios.ToString();
        }

        // ============================================================
        // 🔹 CONFIGURACIÓN GENERAL DE GRÁFICOS
        // ============================================================
        private void ConfigurarGraficos()
        {
            // Gráfico usuarios
            chartUsuarios.ChartAreas[0].BackColor = Color.White;
            chartUsuarios.BackColor = Color.White;
            chartUsuarios.Legends[0].Docking = Docking.Right;
            chartUsuarios.Legends[0].Font = new Font("Segoe UI", 8);
            chartUsuarios.Legends[0].BackColor = Color.White;

            // Gráfico ejercicios
            var area = chartEjercicios.ChartAreas[0];
            area.BackColor = Color.White;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 9);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 9);
            area.AxisX.Title = "Grupos musculares";
            area.AxisY.Title = "Cantidad";
            area.Area3DStyle.Enable3D = false;
            area.AxisX.Interval = 1; // ✅ mostrar todas las etiquetas
        }

        // ============================================================
        // 🥧 GRÁFICO USUARIOS POR ROL
        // ============================================================
        private void CargarGraficoUsuarios()
        {
            var lista = controladorUsuarios.ObtenerTodos();

            int admins = lista.Count(u => u.Rol == Rol.Administrador);
            int profes = lista.Count(u => u.Rol == Rol.Profesor);
            int receps = lista.Count(u => u.Rol == Rol.Recepcionista);

            chartUsuarios.Series.Clear();

            var serie = new Series("Usuarios")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                LabelForeColor = Color.Black
            };

            serie.Points.AddXY("Administradores", admins);
            serie.Points.AddXY("Profesores", profes);
            serie.Points.AddXY("Recepcionistas", receps);

            serie.Points[0].Color = Color.FromArgb(54, 162, 235);
            serie.Points[1].Color = Color.FromArgb(255, 206, 86);
            serie.Points[2].Color = Color.FromArgb(255, 99, 132);

            serie["PieDrawingStyle"] = "Concave";
            serie["PieLabelStyle"] = "Outside";
            serie.SmartLabelStyle.Enabled = true;

            chartUsuarios.Series.Add(serie);
        }

        // ============================================================
        // 📊 GRÁFICO EJERCICIOS POR GRUPO MUSCULAR (real)
        // ============================================================
       private void CargarGraficoEjercicios()
{
    var lista = controladorEjercicios.ObtenerTodos();

    // Agrupar por el nombre del grupo muscular
    var grupos = lista
        .GroupBy(e => string.IsNullOrWhiteSpace(e.GrupoMuscularNombre) ? "Sin grupo" : e.GrupoMuscularNombre)
        .Select(g => new { Grupo = g.Key, Cantidad = g.Count() })
        .OrderBy(g => g.Grupo)
        .ToList();

    chartEjercicios.Series.Clear();

    var serie = new Series("Ejercicios")
    {
        ChartType = SeriesChartType.Column,
        IsValueShownAsLabel = true,
        Font = new Font("Segoe UI", 9, FontStyle.Bold),
        LabelForeColor = Color.Black
    };

    foreach (var grupo in grupos)
    {
        int idx = serie.Points.AddXY(grupo.Grupo, grupo.Cantidad);
        serie.Points[idx].Color = Color.FromArgb(54, 162, 235);
    }

    serie["PointWidth"] = "0.55";
    chartEjercicios.Series.Add(serie);

    // Configurar área
    var area = chartEjercicios.ChartAreas[0];
    area.AxisX.Interval = 1;
    area.AxisX.LabelStyle.Angle = -30; // para leer mejor si hay muchos grupos
}


        // ============================================================
        // 🧱 CONFIGURACIÓN DE LAS CARDS
        // ============================================================
        private void ConfigurarCards()
        {
            // --- CARD USUARIOS
            lblUsuariosTxt.Text = "Total de usuarios";
            lblUsuariosTxt.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblTotalUsuarios.Font = new Font("Segoe UI", 30, FontStyle.Bold);
            lblTotalUsuarios.ForeColor = Color.FromArgb(54, 162, 235);
            lblTotalUsuarios.Location = new Point(25, 45);

            // Limpia cualquier ícono previo
            cardUsuarios.Controls.OfType<Label>()
                .Where(l => l.Text == "👥").ToList()
                .ForEach(l => cardUsuarios.Controls.Remove(l));

            var iconUsuarios = new Label
            {
                Text = "👥",
                Font = new Font("Segoe UI Emoji", 34),
                AutoSize = true,
                Location = new Point(190, 40)
            };
            cardUsuarios.Controls.Add(iconUsuarios);

            // --- CARD EJERCICIOS
            lblEjerciciosTxt.Text = "Total de ejercicios";
            lblEjerciciosTxt.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblTotalEjercicios.Font = new Font("Segoe UI", 30, FontStyle.Bold);
            lblTotalEjercicios.ForeColor = Color.FromArgb(255, 159, 64);
            lblTotalEjercicios.Location = new Point(25, 45);

            // Limpia cualquier ícono previo
            cardEjercicios.Controls.OfType<Label>()
                .Where(l => l.Text == "💪").ToList()
                .ForEach(l => cardEjercicios.Controls.Remove(l));

            var iconEjercicios = new Label
            {
                Text = "💪",
                Font = new Font("Segoe UI Emoji", 34),
                AutoSize = true,
                Location = new Point(190, 40)
            };
            cardEjercicios.Controls.Add(iconEjercicios);
        }

    }
}
