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

        // ============================================================
        // 🔹 CONSTRUCTOR
        // ============================================================
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
            int totalUsuarios = controladorUsuarios.ObtenerTodos()
                                                    .Count(u => u.Activo); // solo activos
            int totalEjercicios = controladorEjercicios.ObtenerTodos().Count;

            lblTotalUsuarios.Text = totalUsuarios.ToString();
            lblTotalEjercicios.Text = totalEjercicios.ToString();
        }


        // ============================================================
        // 🔹 CONFIGURACIÓN DE GRÁFICOS
        // ============================================================
        private void ConfigurarGraficos()
        {
            // Usuarios
            chartUsuarios.ChartAreas[0].BackColor = Color.White;
            chartUsuarios.BackColor = Color.White;
            chartUsuarios.Legends[0].Docking = Docking.Right;
            chartUsuarios.Legends[0].Font = new Font("Segoe UI", 8);
            chartUsuarios.Legends[0].BackColor = Color.White;

            // Ejercicios
            var area = chartEjercicios.ChartAreas[0];
            area.BackColor = Color.White;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 9);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 9);
            area.AxisX.Title = "Grupos musculares";
            area.AxisY.Title = "Cantidad";
            area.AxisX.Interval = 1;
        }

        // ============================================================
        // 🥧 GRÁFICO USUARIOS (ACTUALIZADO Y FUNCIONANDO)
        // ============================================================
        private void CargarGraficoUsuarios()
        {
            var lista = controladorUsuarios.ObtenerTodos()
                                           .Where(u => u.Activo) // solo activos
                                           .ToList();

            int admins = lista.Count(u => u.Rol == Rol.Administrador);
            int profes = lista.Count(u => u.Rol == Rol.Profesor);
            int receps = lista.Count(u => u.Rol == Rol.Recepcionista);

            // Colores modernos y contrastantes
            Color colorAdmin = Color.FromArgb(75, 192, 192); // Teal/Cian
            Color colorProfesor = Color.FromArgb(255, 159, 64); // Naranja
            Color colorRecepcionista = Color.FromArgb(153, 102, 255); // Púrpura

            chartUsuarios.Series.Clear();
            var serie = new Series("Usuarios")
            {
                ChartType = SeriesChartType.Doughnut, // 1. Estilo Doughnut
                IsValueShownAsLabel = true,

                // 2. Propiedades que pueden ir en el inicializador:
                LabelForeColor = Color.White, // Color blanco para contrastar
                LegendText = "#VALX", // **CORREGIDO:** Propiedad de la leyenda

                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BorderColor = Color.White,
                BorderWidth = 2
            };

            serie.Label = "#VALY";

            // Agregamos los puntos y asignamos los colores
            serie.Points.AddXY("Administradores", admins);
            serie.Points.AddXY("Profesores", profes);
            serie.Points.AddXY("Recepcionistas", receps);

            serie.Points[0].Color = colorAdmin;
            serie.Points[1].Color = colorProfesor;
            serie.Points[2].Color = colorRecepcionista;

            // 4. Estilos visuales y posición de la etiqueta
            serie["PieDrawingStyle"] = "SoftEdge";
            serie["DoughnutRadius"] = "50";
            serie["PieLabelStyle"] = "Inside"; // Posiciona el número dentro

            chartUsuarios.Series.Add(serie);

            // 5. Estilo 3D sutil
            chartUsuarios.ChartAreas[0].Area3DStyle.Enable3D = true;
            chartUsuarios.ChartAreas[0].Area3DStyle.Perspective = 10;
            chartUsuarios.ChartAreas[0].Area3DStyle.Rotation = 0;
            chartUsuarios.ChartAreas[0].Area3DStyle.Inclination = 40;
        }

        // ============================================================
        // 📊 GRÁFICO EJERCICIOS (ACTUALIZADO)
        // ============================================================
        private void CargarGraficoEjercicios()
        {
            var lista = controladorEjercicios.ObtenerTodos();
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
                LabelForeColor = Color.Black,

                // **MEJORA VISUAL 1: Ajuste de Borde y Apariencia**
                BorderWidth = 1, // Borde delgado para definir la barra
                BorderColor = Color.FromArgb(40, 100, 200), // Un azul oscuro para el borde
                ShadowOffset = 0 // Quitamos sombras si las hubiera por defecto
            };

            // **Color principal para las barras (Teal más oscuro)**
            Color colorBarra = Color.FromArgb(75, 192, 192);

            foreach (var grupo in grupos)
            {
                int idx = serie.Points.AddXY(grupo.Grupo, grupo.Cantidad);

                // Asignamos el color moderno
                serie.Points[idx].Color = colorBarra;

                // Opcional: Se puede usar un color más suave para el fondo de la columna si se necesita contraste con la etiqueta
                // serie.Points[idx].BackSecondaryColor = Color.FromArgb(75, 192, 192); 
            }

            // **MEJORA VISUAL 2: Ancho de la barra**
            serie["PointWidth"] = "0.7"; // Aumentamos ligeramente el ancho para que se vean más robustas
            chartEjercicios.Series.Add(serie);

            var area = chartEjercicios.ChartAreas[0];
            area.AxisX.Interval = 1;

            // **MEJORA VISUAL 3: Angulo de las etiquetas X**
            // Si la lista de grupos es muy larga, -30 grados puede ser mucho. Probamos con -45 para mejor legibilidad.
            area.AxisX.LabelStyle.Angle = -45;

            // **MEJORA VISUAL 4: Ajuste del eje Y**
            area.AxisY.Minimum = 0; // Aseguramos que la base sea 0 para evitar distorsión visual
            area.AxisY.Interval = 2; // Mostrar las líneas de la cuadrícula cada 2 unidades para un eje más limpio
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

            var iconEjercicios = new Label
            {
                Text = "💪",
                Font = new Font("Segoe UI Emoji", 34),
                AutoSize = true,
                Location = new Point(190, 40)
            };
            cardEjercicios.Controls.Add(iconEjercicios);
        }


        // ============================================================
        // 🔁 MÉTODO PÚBLICO PARA REFRESCAR LOS GRÁFICOS Y CONTADORES
        // ============================================================
        public void RefrescarGraficos()
        {
            // Vuelve a contar usuarios activos y ejercicios
            CargarDatosGenerales();
            CargarGraficoUsuarios();
            CargarGraficoEjercicios();
        }

    }
}
