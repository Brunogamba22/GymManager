using GymManager.Controllers;
using GymManager.Models;
using GymManager.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace GymManager.Views
{
    public partial class UcReporteProfe : UserControl
    {
        // Colores
        private Color primaryColor = Color.FromArgb(46, 134, 171);
        private Color backgroundColor = Color.FromArgb(248, 249, 250);
        private Color textColor = Color.FromArgb(33, 37, 41);

        // Controladores
        private readonly ReporteController _reporteController = new ReporteController();

        public UcReporteProfe()
        {
            InitializeComponent();

            
            ApplyModernStyles();

            // Asignar evento al botón
            btnGenerarReporte.Click += BtnGenerarReporte_Click;

            // Configurar fechas por defecto
            dtpFechaDesde.Value = DateTime.Now.AddMonths(-1);
            dtpFechaHasta.Value = DateTime.Now;
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
                // 1. Obtener ID del profesor (desde la Sesión)
                if (Sesion.Actual == null)
                {
                    MessageBox.Show("No se ha iniciado sesión.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                int idProfesor = Sesion.Actual.IdUsuario;

                // 2. Obtener Fechas
                DateTime fechaDesde = dtpFechaDesde.Value;
                DateTime fechaHasta = dtpFechaHasta.Value;

                // 3. Llamar al controlador
                List<ReporteProfesor> datos = _reporteController.ObtenerBalanceGruposMusculares(idProfesor, fechaDesde, fechaHasta);

                // 4. Poblar el gráfico
                PoblarGrafico(datos, idProfesor, fechaDesde, fechaHasta);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PoblarGrafico(List<ReporteProfesor> datos, int idProfesor, DateTime fechaDesde, DateTime fechaHasta)
        {
            chartBalance.Series["Default"].Points.Clear();

            if (datos == null || datos.Count == 0)
            {
                chartBalance.Titles[0].Text = "No se encontraron datos para este rango";
                return;
            }

            chartBalance.Titles[0].Text = "Balance de Grupos Musculares";
            chartBalance.ChartAreas[0].Area3DStyle.Enable3D = true;
            chartBalance.ChartAreas[0].Area3DStyle.Inclination = 15;

            double total = datos.Sum(d => d.Conteo);

            foreach (var item in datos)
            {
                int pointIndex = chartBalance.Series["Default"].Points.AddXY(item.GrupoMuscular, item.Conteo);
                DataPoint point = chartBalance.Series["Default"].Points[pointIndex];

                
                // 🔥 LÓGICA DE TOOLTIP AÑADIDA 🔥
                // 1. Buscar el Top 5 para este grupo muscular
                List<ReportePopularidad> top5 = _reporteController.ObtenerTop5EjerciciosPorGrupo(
                    idProfesor, fechaDesde, fechaHasta, item.GrupoMuscular);

                // 2. Construir el texto del tooltip
                System.Text.StringBuilder tooltipTexto = new System.Text.StringBuilder();
                tooltipTexto.AppendLine($"Top 5 - {item.GrupoMuscular}:"); // Título

                if (top5.Count == 0)
                {
                    tooltipTexto.AppendLine("  (No hay datos de ejercicios)");
                }
                else
                {
                    for (int i = 0; i < top5.Count; i++)
                    {
                        tooltipTexto.AppendLine($"  {i + 1}. {top5[i].EjercicioNombre} ({top5[i].Conteo} usos)");
                    }
                }

                // 3. Asignar el tooltip al "quesito"
                point.ToolTip = tooltipTexto.ToString();

                // =========================================================

                point.LegendText = $"{item.GrupoMuscular} ({item.Conteo})";
                point.Label = (item.Conteo / total).ToString("P0");
            }

            chartBalance.Series["Default"].Label = "";
        }

        // (Tu método StyleButton)
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
