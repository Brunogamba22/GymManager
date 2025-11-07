using System.Windows.Forms;
using System.Drawing;

namespace GymManager.Views
{
    partial class UcReportesDashboard
    {
        private System.ComponentModel.IContainer components = null;
        private TabControl tabControlReportes;
        private TabPage tabBalance;
        private TabPage tabActividad;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tabControlReportes = new TabControl();
            this.tabBalance = new TabPage();
            this.tabActividad = new TabPage();

            this.SuspendLayout();

            // TabControl
            this.tabControlReportes.Dock = DockStyle.Fill;
            this.tabControlReportes.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            this.tabControlReportes.ItemSize = new Size(200, 40); // Pestañas más grandes
            this.tabControlReportes.SizeMode = TabSizeMode.Fixed;

            // Pestaña 1: Balance
            this.tabBalance.Text = "📊 Balance de Grupos";
            this.tabBalance.Padding = new Padding(3);
            this.tabBalance.UseVisualStyleBackColor = true;

            // Pestaña 2: Actividad
            this.tabActividad.Text = "📈 Actividad del Profesor";
            this.tabActividad.Padding = new Padding(3);
            this.tabActividad.UseVisualStyleBackColor = true;

            this.tabControlReportes.Controls.Add(this.tabBalance);
            this.tabControlReportes.Controls.Add(this.tabActividad);

            // UserControl
            this.Controls.Add(this.tabControlReportes);
            this.Name = "UcReportesDashboard";
            this.Size = new Size(900, 600);

            this.ResumeLayout(false);
        }
    }
}