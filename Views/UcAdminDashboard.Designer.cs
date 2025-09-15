using System.Windows.Forms;

namespace GymManager.Views
{
    partial class UcAdminDashboard
    {
        private System.ComponentModel.IContainer components = null;
        private Panel panelContenidoAdmin;
        private Button btnGestionEjercicios;
        private Label lblTitulo;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.panelContenidoAdmin = new Panel();
            this.btnGestionEjercicios = new Button();
            this.lblTitulo = new Label();
            this.SuspendLayout();

            // 
            // lblTitulo
            // 
            this.lblTitulo.Text = "Panel de Administración";
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 20);
            this.lblTitulo.AutoSize = true;

            // 
            // btnGestionEjercicios
            // 
            this.btnGestionEjercicios.Text = "Gestionar Ejercicios";
            this.btnGestionEjercicios.Location = new System.Drawing.Point(20, 60);
            this.btnGestionEjercicios.Size = new System.Drawing.Size(180, 40);
            this.btnGestionEjercicios.Click += new System.EventHandler(this.btnGestionEjercicios_Click);

            // 
            // panelContenidoAdmin
            // 
            this.panelContenidoAdmin.Location = new System.Drawing.Point(20, 120);
            this.panelContenidoAdmin.Size = new System.Drawing.Size(600, 350);
            this.panelContenidoAdmin.BorderStyle = BorderStyle.FixedSingle;

            // 
            // UcAdminDashboard
            // 
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.btnGestionEjercicios);
            this.Controls.Add(this.panelContenidoAdmin);
            this.Size = new System.Drawing.Size(650, 500);
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}

