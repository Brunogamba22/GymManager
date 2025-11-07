using GymManager.Controllers;
using GymManager.Models;
using GymManager.Utils;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Views
{
    public partial class UcReporteActividad : UserControl
    {
        // Colores
        private Color primaryColor = Color.FromArgb(46, 134, 171);
        private Color backgroundColor = Color.FromArgb(248, 249, 250);

        // Controladores
        private readonly ReporteController _reporteController = new ReporteController();

        public UcReporteActividad()
        {
            InitializeComponent();
            ApplyModernStyles();

            btnGenerarReporte.Click += BtnGenerarReporte_Click;

            // Configurar fechas por defecto
            dtpFechaDesde.Value = DateTime.Now.AddMonths(-1);
            dtpFechaHasta.Value = DateTime.Now;

            // Cargar datos iniciales
            BtnGenerarReporte_Click(null, null);
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
                if (Sesion.Actual == null) { /* ... */ return; }
                int idProfesor = Sesion.Actual.IdUsuario;
                DateTime fechaDesde = dtpFechaDesde.Value;
                DateTime fechaHasta = dtpFechaHasta.Value;

                // 1. Llamar al controlador
                ReporteActividadItem datos = _reporteController.ObtenerReporteActividad(idProfesor, fechaDesde, fechaHasta);

                // 2. Actualizar los labels
                lblTotalNumero.Text = datos.TotalRutinas.ToString();
                lblNuevasNumero.Text = datos.RutinasNuevas.ToString();
                lblEditadasNumero.Text = datos.RutinasEditadas.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar el reporte: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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