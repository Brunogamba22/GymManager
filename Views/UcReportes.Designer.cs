using System;
using System.Windows.Forms;
using System.Drawing;

namespace GymManager.Views
{
    partial class UcReportes
    {
        //  Definición de etiquetas (Labels) para mostrar textos y números
        private Label lblTitulo;
        private Label lblEjercicios;
        private Label lblTotalEjercicios;
        private Label lblUsuarios;
        private Label lblAdmins;
        private Label lblProfes;
        private Label lblRecepcionistas;

        private void InitializeComponent()
        {
            this.lblTitulo = new System.Windows.Forms.Label();
            this.lblEjercicios = new System.Windows.Forms.Label();
            this.lblTotalEjercicios = new System.Windows.Forms.Label();
            this.lblUsuarios = new System.Windows.Forms.Label();
            this.lblAdmins = new System.Windows.Forms.Label();
            this.lblProfes = new System.Windows.Forms.Label();
            this.lblRecepcionistas = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lblTitulo
            // 
            this.lblTitulo.AutoSize = true;
            this.lblTitulo.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblTitulo.Location = new System.Drawing.Point(20, 20);
            this.lblTitulo.Name = "lblTitulo";
            this.lblTitulo.Size = new System.Drawing.Size(223, 25);
            this.lblTitulo.TabIndex = 0;
            this.lblTitulo.Text = "📊 Reportes del Sistema";
            // 
            // lblEjercicios
            // 
            this.lblEjercicios.AutoSize = true;
            this.lblEjercicios.Location = new System.Drawing.Point(40, 70);
            this.lblEjercicios.Name = "lblEjercicios";
            this.lblEjercicios.Size = new System.Drawing.Size(96, 13);
            this.lblEjercicios.TabIndex = 1;
            this.lblEjercicios.Text = "Total de ejercicios:";
            // 
            // lblTotalEjercicios
            // 
            this.lblTotalEjercicios.AutoSize = true;
            this.lblTotalEjercicios.Font = new System.Drawing.Font("Segoe UI", 10F, System.Drawing.FontStyle.Bold);
            this.lblTotalEjercicios.Location = new System.Drawing.Point(200, 70);
            this.lblTotalEjercicios.Name = "lblTotalEjercicios";
            this.lblTotalEjercicios.Size = new System.Drawing.Size(0, 19);
            this.lblTotalEjercicios.TabIndex = 2;
            // 
            // lblUsuarios
            // 
            this.lblUsuarios.AutoSize = true;
            this.lblUsuarios.Location = new System.Drawing.Point(40, 120);
            this.lblUsuarios.Name = "lblUsuarios";
            this.lblUsuarios.Size = new System.Drawing.Size(83, 13);
            this.lblUsuarios.TabIndex = 3;
            this.lblUsuarios.Text = "Usuarios por rol:";
            // 
            // lblAdmins
            // 
            this.lblAdmins.AutoSize = true;
            this.lblAdmins.Location = new System.Drawing.Point(60, 150);
            this.lblAdmins.Name = "lblAdmins";
            this.lblAdmins.Size = new System.Drawing.Size(93, 13);
            this.lblAdmins.TabIndex = 4;
            this.lblAdmins.Text = "Administradores: 0";
            // 
            // lblProfes
            // 
            this.lblProfes.AutoSize = true;
            this.lblProfes.Location = new System.Drawing.Point(60, 180);
            this.lblProfes.Name = "lblProfes";
            this.lblProfes.Size = new System.Drawing.Size(69, 13);
            this.lblProfes.TabIndex = 5;
            this.lblProfes.Text = "Profesores: 0";
            // 
            // lblRecepcionistas
            // 
            this.lblRecepcionistas.AutoSize = true;
            this.lblRecepcionistas.Location = new System.Drawing.Point(60, 210);
            this.lblRecepcionistas.Name = "lblRecepcionistas";
            this.lblRecepcionistas.Size = new System.Drawing.Size(92, 13);
            this.lblRecepcionistas.TabIndex = 6;
            this.lblRecepcionistas.Text = "Recepcionistas: 0";
            // 
            // UcReportes
            // 
            this.Controls.Add(this.lblTitulo);
            this.Controls.Add(this.lblEjercicios);
            this.Controls.Add(this.lblTotalEjercicios);
            this.Controls.Add(this.lblUsuarios);
            this.Controls.Add(this.lblAdmins);
            this.Controls.Add(this.lblProfes);
            this.Controls.Add(this.lblRecepcionistas);
            this.Name = "UcReportes";
            this.Size = new System.Drawing.Size(400, 300);
            this.Load += new System.EventHandler(this.UcReportes_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
    }
}
