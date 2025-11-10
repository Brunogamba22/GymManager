using System.Windows.Forms;
using GymManager.Views; // Asegúrate de tener este 'using'
using System.Drawing;   // Asegúrate de tener este 'using'
using System;         // Asegúrate de tener este 'using'

namespace GymManager.Views
{
    public partial class UcReportesDashboard : UserControl
    {
        // Guardar instancias de los reportes
        private UcReporteProfe ucReporteProfe;
        private UcReporteActividad ucReporteActividad;

        public UcReportesDashboard()
        {
            InitializeComponent();

            // --- 🔥 ASIGNAR EL EVENTO LOAD AQUÍ ---
            this.Load += UcReportesDashboard_Load;
        }

        // --- 🔥 LÓGICA MOVIDA AL EVENTO LOAD ---
        private void UcReportesDashboard_Load(object sender, EventArgs e)
        {
            // Cargar los UserControls dentro de las pestañas

            // 1. Cargar Reporte de Balance
            if (ucReporteProfe == null) // Solo crear si no existe
            {
                ucReporteProfe = new UcReporteProfe();
                ucReporteProfe.Size = tabBalance.ClientSize; // fuerza dimensiones válidas
                ucReporteProfe.Dock = DockStyle.Fill;
                tabBalance.Controls.Add(ucReporteProfe);
            }

            // 2. Cargar Reporte de Actividad
            if (ucReporteActividad == null) // Solo crear si no existe
            {
                ucReporteActividad = new UcReporteActividad();
                ucReporteActividad.Size = tabActividad.ClientSize;
                ucReporteActividad.Dock = DockStyle.Fill;
                tabActividad.Controls.Add(ucReporteActividad);
            }
        }
    }
}