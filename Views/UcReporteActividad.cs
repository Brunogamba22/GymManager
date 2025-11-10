using GymManager.Controllers;
using GymManager.Models;
using GymManager.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;
// 💡 AÑADIR ESTE USING
using System.Windows.Forms.DataVisualization.Charting;

namespace GymManager.Views
{
    public partial class UcReporteActividad : UserControl
    {
        // Colores
        private Color primaryColor = Color.FromArgb(46, 134, 171);
        private Color backgroundColor = Color.FromArgb(248, 249, 250);

        // 💡 Colores para el gráfico
        private Color colorHombres = Color.FromArgb(46, 134, 171);
        private Color colorMujeres = Color.FromArgb(162, 59, 114);
        private Color colorDeportistas = Color.FromArgb(28, 167, 69);


        // Controladores
        private readonly ReporteController _reporteController = new ReporteController();

        public UcReporteActividad()
        {
            InitializeComponent();

            ApplyModernStyles();

            btnGenerarReporte.Click += BtnGenerarReporte_Click;

            // 💡 Mover la carga de datos al evento Load
            this.Load += UcReporteActividad_Load;

            // Configurar fechas por defecto
            dtpFechaDesde.Value = DateTime.Now.AddMonths(-1);
            dtpFechaHasta.Value = DateTime.Now;

            // ❌ NO cargar datos aquí, el gráfico aún no está listo
            // BtnGenerarReporte_Click(null, null);
        }

        // 💡 MÉTODO LOAD
        private void UcReporteActividad_Load(object sender, EventArgs e)
        {
            // Configurar el estilo del gráfico una sola vez
            ConfigurarGraficoGeneros();

            // Cargar datos iniciales AHORA
            BtnGenerarReporte_Click(null, null);
        }


        // 💡 NUEVO: Configura el estilo del gráfico
        private void ConfigurarGraficoGeneros()
        {
            chartGeneros.Series.Clear();
            chartGeneros.ChartAreas.Clear();

            ChartArea chartArea = new ChartArea("MainArea");
            chartArea.BackColor = Color.Transparent; // Fondo transparente
            chartArea.AxisX.MajorGrid.Enabled = false; // Sin líneas de cuadrícula X
            chartArea.AxisX.MajorTickMark.Enabled = false; // Sin marcas
            chartArea.AxisX.LabelStyle.Font = new Font("Segoe UI", 10f);

            chartArea.AxisY.MajorGrid.LineColor = Color.Gainsboro; // Líneas Y suaves
            chartArea.AxisY.MajorTickMark.Enabled = false;
            chartArea.AxisY.LabelStyle.Enabled = false; // Ocultar etiquetas del eje Y (números)
            chartArea.AxisY.LineColor = Color.Transparent; // Ocultar línea del eje Y

            chartGeneros.ChartAreas.Add(chartArea);

            Series series = new Series("Generos");
            series.ChartType = SeriesChartType.Bar; // Gráfico de barras
            series.IsValueShownAsLabel = true;     // Mostrar el número en la barra
            series.Font = new Font("Segoe UI", 10f, FontStyle.Bold);
            series.BackGradientStyle = GradientStyle.TopBottom;

            chartGeneros.Series.Add(series);

            chartGeneros.Legends.Clear(); // Sin leyenda

            // Añadir título
            Title title = new Title("Distribución por Genero", Docking.Top, new Font("Segoe UI", 14f, FontStyle.Bold), Color.Black);
            title.Alignment = ContentAlignment.TopCenter;
            
            chartGeneros.Titles.Add(title);
        }

        private void ApplyModernStyles()
        {
            this.BackColor = backgroundColor;
            StyleButton(btnGenerarReporte, primaryColor);
        }

        private void BtnGenerarReporte_Click(object sender, EventArgs e)
        {
            try
            {
                if (Sesion.Actual == null)
                {
                    MessageBox.Show("No se ha iniciado sesión.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                int idProfesor = Sesion.Actual.IdUsuario;
                DateTime fechaDesde = dtpFechaDesde.Value;
                DateTime fechaHasta = dtpFechaHasta.Value;

                // Llamar al controlador
                ReporteActividadItem datos = _reporteController.ObtenerReporteActividad(idProfesor, fechaDesde, fechaHasta);

                // --- 1. Actualizar las TARJETAS ---
                lblTotalNumero.Text = datos.TotalRutinas.ToString();
                lblNuevasNumero.Text = datos.RutinasNuevas.ToString();
                lblEditadasNumero.Text = datos.RutinasEditadas.ToString();

                // --- 2. Poblar el GRÁFICO ---
                PoblarGraficoGeneros(datos);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el informe: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // 💡 NUEVO: Rellena el gráfico con datos
        private void PoblarGraficoGeneros(ReporteActividadItem datos)
        {
            var series = chartGeneros.Series["Generos"];
            series.Points.Clear();

            // Añadir puntos de datos
            DataPoint dpHombres = new DataPoint();
            dpHombres.SetValueXY("Hombres", datos.RutinasHombres);
            dpHombres.LabelForeColor = colorHombres;
            dpHombres.Color = colorHombres;
            series.Points.Add(dpHombres);

            DataPoint dpMujeres = new DataPoint();
            dpMujeres.SetValueXY("Mujeres", datos.RutinasMujeres);
            dpMujeres.LabelForeColor = colorMujeres;
            dpMujeres.Color = colorMujeres;
            series.Points.Add(dpMujeres);

            DataPoint dpDeportistas = new DataPoint();
            dpDeportistas.SetValueXY("Deportistas", datos.RutinasDeportistas);
            dpDeportistas.LabelForeColor = colorDeportistas;
            dpDeportistas.Color = colorDeportistas;
            series.Points.Add(dpDeportistas);

            // Aplicar degradado
            foreach (DataPoint pt in series.Points)
            {
                pt.Color = Color.FromArgb(220, pt.Color); // Un poco de transparencia
                pt.BackSecondaryColor = Color.FromArgb(50, pt.Color);
            }
        }


        // (Tu método StyleButton - Sin cambios)
        private void StyleButton(Button boton, Color colorFondo)
        {
            if (boton == null) return;

            boton.BackColor = colorFondo;
            boton.ForeColor = Color.White;
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;
            boton.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            boton.Cursor = Cursors.Hand;
            boton.Padding = new Padding(12, 6, 12, 6);
            boton.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(colorFondo, 0.1f);
            boton.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(colorFondo, 0.2f);
        }
    }
}