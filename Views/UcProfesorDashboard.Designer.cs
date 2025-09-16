namespace GymManager.Views
{
    partial class UcProfesorDashboard
    {
        private System.ComponentModel.IContainer components = null;

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgvHombres;
        private System.Windows.Forms.DataGridView dgvMujeres;
        private System.Windows.Forms.DataGridView dgvDeportistas;
        private System.Windows.Forms.Button btnGenerarHombres;
        private System.Windows.Forms.Button btnGenerarMujeres;
        private System.Windows.Forms.Button btnGenerarDeportistas;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null)) { components.Dispose(); }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvHombres = new System.Windows.Forms.DataGridView();
            this.btnGenerarHombres = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvMujeres = new System.Windows.Forms.DataGridView();
            this.btnGenerarMujeres = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvDeportistas = new System.Windows.Forms.DataGridView();
            this.btnGenerarDeportistas = new System.Windows.Forms.Button();

            this.tableLayoutPanel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvHombres)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMujeres)).BeginInit();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeportistas)).BeginInit();
            this.SuspendLayout();

            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(800, 600);

            // 
            // groupBox1 - HOMBRES
            // 
            this.groupBox1.Controls.Add(this.dgvHombres);
            this.groupBox1.Controls.Add(this.btnGenerarHombres);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Text = "Rutina para HOMBRES";

            // dgvHombres
            this.dgvHombres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvHombres.ReadOnly = true;
            this.dgvHombres.AllowUserToAddRows = false;
            this.dgvHombres.AllowUserToDeleteRows = false;
            this.dgvHombres.RowHeadersVisible = false;
            this.dgvHombres.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHombres.Columns.Add("Ejercicio", "Ejercicio");
            this.dgvHombres.Columns.Add("Series", "Series");
            this.dgvHombres.Columns.Add("Repeticiones", "Repeticiones");
            this.dgvHombres.Columns.Add("Descanso", "Descanso (s)");

            // btnGenerarHombres
            this.btnGenerarHombres.Text = "Generar";
            this.btnGenerarHombres.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnGenerarHombres.Click += new System.EventHandler(this.btnGenerarHombres_Click);

            // 
            // groupBox2 - MUJERES
            // 
            this.groupBox2.Controls.Add(this.dgvMujeres);
            this.groupBox2.Controls.Add(this.btnGenerarMujeres);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Text = "Rutina para MUJERES";

            // dgvMujeres
            this.dgvMujeres.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMujeres.ReadOnly = true;
            this.dgvMujeres.AllowUserToAddRows = false;
            this.dgvMujeres.AllowUserToDeleteRows = false;
            this.dgvMujeres.RowHeadersVisible = false;
            this.dgvMujeres.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMujeres.Columns.Add("Ejercicio", "Ejercicio");
            this.dgvMujeres.Columns.Add("Series", "Series");
            this.dgvMujeres.Columns.Add("Repeticiones", "Repeticiones");
            this.dgvMujeres.Columns.Add("Descanso", "Descanso (s)");

            // btnGenerarMujeres
            this.btnGenerarMujeres.Text = "Generar";
            this.btnGenerarMujeres.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnGenerarMujeres.Click += new System.EventHandler(this.btnGenerarMujeres_Click);

            // 
            // groupBox3 - DEPORTISTAS
            // 
            this.groupBox3.Controls.Add(this.dgvDeportistas);
            this.groupBox3.Controls.Add(this.btnGenerarDeportistas);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Text = "Rutina para DEPORTISTAS";

            // dgvDeportistas
            this.dgvDeportistas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvDeportistas.ReadOnly = true;
            this.dgvDeportistas.AllowUserToAddRows = false;
            this.dgvDeportistas.AllowUserToDeleteRows = false;
            this.dgvDeportistas.RowHeadersVisible = false;
            this.dgvDeportistas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDeportistas.Columns.Add("Ejercicio", "Ejercicio");
            this.dgvDeportistas.Columns.Add("Series", "Series");
            this.dgvDeportistas.Columns.Add("Repeticiones", "Repeticiones");
            this.dgvDeportistas.Columns.Add("Descanso", "Descanso (s)");

            // btnGenerarDeportistas
            this.btnGenerarDeportistas.Text = "Generar";
            this.btnGenerarDeportistas.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnGenerarDeportistas.Click += new System.EventHandler(this.btnGenerarDeportistas_Click);

            // 
            // UcProfesorDashboard
            // 
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UcProfesorDashboard";
            this.Size = new System.Drawing.Size(800, 600);

            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHombres)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMujeres)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeportistas)).EndInit();
            this.ResumeLayout(false);
        }
    }
}
