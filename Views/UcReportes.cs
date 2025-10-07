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
        // Controladores para acceder a los datos de usuarios y ejercicios
        private UsuarioController controladorUsuarios = new UsuarioController();
        private EjercicioController controladorEjercicios = new EjercicioController();

        // ------------------------------------------------------------
        // 🔹 CONSTRUCTOR DEL USER CONTROL
        // ------------------------------------------------------------
        public UcReportes()
        {
            InitializeComponent(); // Inicializa los componentes visuales del formulario
        }

        // ------------------------------------------------------------
        // 🚀 EVENTO LOAD: se ejecuta cuando se carga el control por primera vez
        // ------------------------------------------------------------
        private void UcReportes_Load(object sender, EventArgs e)
        {
            // ============================================================
            // 📊 SECCIÓN 1 — ESTADÍSTICAS GENERALES
            // ============================================================

            // Obtiene el total de ejercicios existentes en la BD
            int totalEjercicios = controladorEjercicios.ObtenerTodos().Count;
            lblTotalEjercicios.Text = totalEjercicios.ToString(); // Muestra el número en el label

            // Obtiene todos los usuarios activos desde el controlador
            var listaUsuarios = controladorUsuarios.ObtenerTodos();

            // Contamos usuarios por tipo de rol
            int cantidadAdmins = listaUsuarios.Count(u => u.Rol == Rol.Administrador);
            int cantidadProfes = listaUsuarios.Count(u => u.Rol == Rol.Profesor);
            int cantidadReceps = listaUsuarios.Count(u => u.Rol == Rol.Recepcionista);

            // Muestra el total general de usuarios
            lblTotalUsuarios.Text = listaUsuarios.Count.ToString();

            // Etiquetas informativas (opcional para KPIs visuales)
            lblUsuariosTxt.Text = "Total de usuarios:";
            lblEjerciciosTxt.Text = "Total de ejercicios:";

            // ============================================================
            // 🥧 SECCIÓN 2 — GRÁFICO DE TORTA: USUARIOS POR ROL
            // ============================================================

            chartUsuarios.Series.Clear(); // Limpia cualquier serie previa del gráfico

            // Crea una nueva serie para representar los roles
            var serieUsuarios = new Series("Usuarios")
            {
                ChartType = SeriesChartType.Pie,                   // Tipo de gráfico: torta
                IsValueShownAsLabel = true,                        // Muestra los valores en cada porción
                Label = "#VALX\n#VAL (#PERCENT{P0})",              // Ejemplo: Profesores 5 (33%)
                Font = new Font("Segoe UI", 8, FontStyle.Bold),     // Fuente de las etiquetas
                LabelForeColor = Color.Black                       // Color del texto de etiquetas
            };

            // Agrega los valores (X = nombre del rol, Y = cantidad)
            serieUsuarios.Points.AddXY("Administradores", cantidadAdmins);
            serieUsuarios.Points.AddXY("Profesores", cantidadProfes);
            serieUsuarios.Points.AddXY("Recepcionistas", cantidadReceps);

            // Agrega la serie al gráfico
            chartUsuarios.Series.Add(serieUsuarios);

            // Ubica la leyenda a la derecha del gráfico
            chartUsuarios.Legends[0].Docking = Docking.Right;

            // Configuración visual de las etiquetas de la torta
            serieUsuarios["PieLabelStyle"] = "Outside"; // Muestra las etiquetas fuera de las porciones
            serieUsuarios.SmartLabelStyle.Enabled = true; // Evita superposición de texto

            // ============================================================
            // 📊 SECCIÓN 3 — GRÁFICO DE BARRAS: EJERCICIOS POR MÚSCULO
            // ============================================================

            chartEjercicios.Series.Clear(); // Limpia series previas

            // Nueva serie para las barras
            var serieEjercicios = new Series("Ejercicios")
            {
                ChartType = SeriesChartType.Column,    // Tipo de gráfico: columnas
                IsValueShownAsLabel = true,            // Muestra los valores encima de las barras
                LabelForeColor = Color.Black           // Texto negro
            };

            // ⚠️ En el modelo actual, Ejercicio ya no tiene propiedad 'Musculo'.
            // Por eso generamos un valor simulado o usamos el nombre del grupo muscular si existe.

            var listaEjercicios = controladorEjercicios.ObtenerTodos();

            // Creamos un agrupamiento temporal:
            // si el modelo Ejercicio tiene IdGrupoMuscular, podemos usarlo para agrupar;
            // en caso contrario, simulamos un campo genérico.
            var ejerciciosAgrupados = listaEjercicios
                .GroupBy(e =>
                    e.GetType().GetProperty("GrupoMuscular") != null  // Si el modelo tiene esa propiedad
                        ? e.GetType().GetProperty("GrupoMuscular").GetValue(e, null)?.ToString() ?? "Sin grupo"
                        : "General"                                   // Si no, agrupamos bajo "General"
                )
                .Select(g => new
                {
                    Grupo = g.Key,          // Nombre del grupo o categoría
                    Cantidad = g.Count()    // Total de ejercicios en ese grupo
                });

            // Agregamos los datos al gráfico de barras
            foreach (var grupo in ejerciciosAgrupados)
            {
                serieEjercicios.Points.AddXY(grupo.Grupo, grupo.Cantidad);
            }

            // Agrega la serie de ejercicios al gráfico principal
            chartEjercicios.Series.Add(serieEjercicios);

            // Configuración visual del gráfico
            chartEjercicios.ChartAreas[0].AxisX.LabelStyle.Angle = -45; // Rota etiquetas X para mejor lectura
            chartEjercicios.ChartAreas[0].AxisX.Interval = 1;           // Muestra todas las etiquetas
            chartEjercicios.ChartAreas[0].AxisY.Title = "Cantidad";     // Título del eje Y
            chartEjercicios.ChartAreas[0].AxisX.Title = "Grupos musculares"; // Título del eje X
        }
    }
}
