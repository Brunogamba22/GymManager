using System.Drawing;

namespace GymManager.Forms
{
    partial class FrmMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.panelHeader = new System.Windows.Forms.Panel();
            this.lblBienvenida = new System.Windows.Forms.Label();
            this.panelFooter = new System.Windows.Forms.Panel();
            this.panelNavbar = new System.Windows.Forms.Panel();
            this.panelContenido = new System.Windows.Forms.Panel();
            this.panelHeader.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelHeader
            // 
            // panelHeader
            this.panelHeader.BackColor = Color.FromArgb(52, 73, 90); // azul más moderno
            this.panelHeader.Controls.Add(this.lblBienvenida);
            this.panelHeader.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelHeader.Location = new System.Drawing.Point(0, 0);
            this.panelHeader.Name = "panelHeader";
            this.panelHeader.Size = new System.Drawing.Size(1200, 60);
            this.panelHeader.TabIndex = 0;
            // 
            // lblBienvenida
            // 
            this.lblBienvenida.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lblBienvenida.Font = new System.Drawing.Font("Segoe UI", 14F, System.Drawing.FontStyle.Bold);
            this.lblBienvenida.ForeColor = System.Drawing.Color.White;
            this.lblBienvenida.Location = new System.Drawing.Point(0, 0);
            this.lblBienvenida.Name = "lblBienvenida";
            this.lblBienvenida.Size = new System.Drawing.Size(1200, 60);
            this.lblBienvenida.TabIndex = 0;
            this.lblBienvenida.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panelFooter
            // 
            this.panelFooter.BackColor = System.Drawing.Color.FromArgb(52, 73, 94); // Gris oscuro
            this.panelFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelFooter.Location = new System.Drawing.Point(0, 680);
            this.panelFooter.Name = "panelFooter";
            this.panelFooter.Size = new System.Drawing.Size(1200, 30);
            this.panelFooter.TabIndex = 1;
            // 
            // panelNavbar
            // 
            this.panelNavbar.BackColor = System.Drawing.Color.FromArgb(45, 52, 70); // Gris azulado oscuro
            this.panelNavbar.Dock = System.Windows.Forms.DockStyle.Left;
            this.panelNavbar.Location = new System.Drawing.Point(0, 60);
            this.panelNavbar.Name = "panelNavbar";
            this.panelNavbar.Size = new System.Drawing.Size(220, 620);
            this.panelNavbar.TabIndex = 2;
            // 
            // panelContenido
            // 
            this.panelContenido.BackColor = System.Drawing.Color.White;
            this.panelContenido.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelContenido.Location = new System.Drawing.Point(220, 60);
            this.panelContenido.Name = "panelContenido";
            this.panelContenido.Size = new System.Drawing.Size(980, 620);
            this.panelContenido.TabIndex = 3;
            // 
            // FrmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1200, 710);
            this.Controls.Add(this.panelContenido);
            this.Controls.Add(this.panelNavbar);
            this.Controls.Add(this.panelFooter);
            this.Controls.Add(this.panelHeader);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "FrmMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "GymManager";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.FrmMain_Load);
            this.panelHeader.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelHeader;
        private System.Windows.Forms.Panel panelFooter;
        private System.Windows.Forms.Panel panelNavbar;
        private System.Windows.Forms.Panel panelContenido;
        private System.Windows.Forms.Label lblBienvenida;
    }
}
