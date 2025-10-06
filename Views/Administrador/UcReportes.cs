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
        private UsuarioController usuarioController = new UsuarioController();
        private EjercicioController ejercicioController = new EjercicioController();

        public UcReportes()
        {
            InitializeComponent();
        }

        private void UcReportes_Load(object sender, EventArgs e)
        {
            // Total de ejercicios
            int totalEjercicios = ejercicioController.ObtenerTodos().Count;
            lblTotalEjercicios.Text = totalEjercicios.ToString();

            // Usuarios por rol
            var usuarios = usuarioController.ObtenerTodos();
            int admins = usuarios.Count(u => u.Rol == Rol.Administrador);
            int profes = usuarios.Count(u => u.Rol == Rol.Profesor);
            int receps = usuarios.Count(u => u.Rol == Rol.Recepcionista);

            lblTotalUsuarios.Text = usuarios.Count.ToString();

            // 🔹 Etiquetas extra para mostrar detalle
            lblUsuariosTxt.Text = "Total de usuarios:";
            lblEjerciciosTxt.Text = "Total de ejercicios:";

            // Puedes agregar labels extra al diseño si querés que se vean como KPIs
            // Ejemplo:
            // lblAdmins.Text = $"Administradores: {admins}";
            // lblProfes.Text = $"Profesores: {profes}";
            // lblRecepcionistas.Text = $"Recepcionistas: {receps}";

            // ---  Gráfico de torta (Usuarios por rol) ---
            // --- Gráfico de torta ---
            chartUsuarios.Series.Clear();
            var serieUsuarios = new Series("Usuarios")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                Label = "#VALX\n#VAL (#PERCENT{P0})", // 🔹 Ej: Profesores 7 (44%)
                Font = new Font("Segoe UI", 8, FontStyle.Bold),
                LabelForeColor = Color.Black
            };

            serieUsuarios.Points.AddXY("Administradores", admins);
            serieUsuarios.Points.AddXY("Profesores", profes);
            serieUsuarios.Points.AddXY("Recepcionistas", receps);

            chartUsuarios.Series.Add(serieUsuarios);
            chartUsuarios.Legends[0].Docking = Docking.Right;

            // Etiquetas afuera y más legibles
            serieUsuarios["PieLabelStyle"] = "Outside";
            serieUsuarios.SmartLabelStyle.Enabled = true;

            // --- Gráfico de barras ---
            chartEjercicios.Series.Clear();
            var serieEjercicios = new Series("Ejercicios")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true,
                LabelForeColor = Color.Black
            };

            // Agrupar ejercicios por músculo
            var ejercicios = ejercicioController.ObtenerTodos()
                                .GroupBy(e => e.Musculo)
                                .Select(g => new { Musculo = g.Key, Cantidad = g.Count() });

            foreach (var item in ejercicios)
            {
                serieEjercicios.Points.AddXY(item.Musculo, item.Cantidad);
            }

            chartEjercicios.Series.Add(serieEjercicios);

            // Mejorar legibilidad de eje X
            chartEjercicios.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
            chartEjercicios.ChartAreas[0].AxisX.Interval = 1;
            chartEjercicios.ChartAreas[0].AxisY.Title = "Cantidad";
            chartEjercicios.ChartAreas[0].AxisX.Title = "Músculos";

        }

    }
}
