namespace GymManager.Views
{
    partial class UcProfesorDashboard
    {
        /// <summary> 
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }



        #region Código generado por el Diseñador de componentes

        /// <summary> 
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.dgvHombres = new System.Windows.Forms.DataGridView();
            this.colEjercicio = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSeries = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colReps = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDescanso = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.dgvMujeres = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnGenerarMujeres = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.dgvDeportistas = new System.Windows.Forms.DataGridView();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.btnGenerarDeportistas = new System.Windows.Forms.Button();
            this.btnGenerarHombres = new System.Windows.Forms.Button();
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
            this.tableLayoutPanel1.AutoSize = true;
            this.tableLayoutPanel1.ColumnCount = 1;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel1.Controls.Add(this.groupBox1, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.groupBox2, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.groupBox3, 0, 2);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 3;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(815, 567);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnGenerarHombres);
            this.groupBox1.Controls.Add(this.dgvHombres);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox1.Location = new System.Drawing.Point(3, 3);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(809, 183);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Rutina para Hombres";
            this.groupBox1.Enter += new System.EventHandler(this.groupBox1_Enter);
            // 
            // dgvHombres
            // 
            this.dgvHombres.AllowUserToAddRows = false;
            this.dgvHombres.AllowUserToDeleteRows = false;
            this.dgvHombres.AllowUserToResizeRows = false;
            this.dgvHombres.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvHombres.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvHombres.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colEjercicio,
            this.colSeries,
            this.colReps,
            this.colDescanso});
            this.dgvHombres.Dock = System.Windows.Forms.DockStyle.Top;
            this.dgvHombres.Location = new System.Drawing.Point(3, 16);
            this.dgvHombres.Name = "dgvHombres";
            this.dgvHombres.ReadOnly = true;
            this.dgvHombres.RowHeadersVisible = false;
            this.dgvHombres.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvHombres.Size = new System.Drawing.Size(803, 138);
            this.dgvHombres.TabIndex = 1;
            this.dgvHombres.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvHombres_CellContentClick);
            // 
            // colEjercicio
            // 
            this.colEjercicio.HeaderText = "Ejercicio";
            this.colEjercicio.Name = "colEjercicio";
            this.colEjercicio.ReadOnly = true;
            // 
            // colSeries
            // 
            this.colSeries.HeaderText = "Series";
            this.colSeries.Name = "colSeries";
            this.colSeries.ReadOnly = true;
            // 
            // colReps
            // 
            this.colReps.HeaderText = "Repeticiones";
            this.colReps.Name = "colReps";
            this.colReps.ReadOnly = true;
            // 
            // colDescanso
            // 
            this.colDescanso.HeaderText = "Descanso (s)";
            this.colDescanso.Name = "colDescanso";
            this.colDescanso.ReadOnly = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.dgvMujeres);
            this.groupBox2.Controls.Add(this.btnGenerarMujeres);
            this.groupBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox2.Location = new System.Drawing.Point(3, 192);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(809, 183);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Rutina para MUJERES";
            // 
            // dgvMujeres
            // 
            this.dgvMujeres.AllowUserToAddRows = false;
            this.dgvMujeres.AllowUserToDeleteRows = false;
            this.dgvMujeres.AllowUserToResizeColumns = false;
            this.dgvMujeres.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvMujeres.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMujeres.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn3,
            this.dataGridViewTextBoxColumn4});
            this.dgvMujeres.Location = new System.Drawing.Point(6, 19);
            this.dgvMujeres.Name = "dgvMujeres";
            this.dgvMujeres.ReadOnly = true;
            this.dgvMujeres.RowHeadersVisible = false;
            this.dgvMujeres.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvMujeres.Size = new System.Drawing.Size(725, 144);
            this.dgvMujeres.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.HeaderText = "Ejercicio";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.HeaderText = "Series";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn3
            // 
            this.dataGridViewTextBoxColumn3.HeaderText = "Repticiones";
            this.dataGridViewTextBoxColumn3.Name = "dataGridViewTextBoxColumn3";
            this.dataGridViewTextBoxColumn3.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.HeaderText = "Descanso (s)";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            // 
            // btnGenerarMujeres
            // 
            this.btnGenerarMujeres.Location = new System.Drawing.Point(737, 160);
            this.btnGenerarMujeres.Name = "btnGenerarMujeres";
            this.btnGenerarMujeres.Size = new System.Drawing.Size(75, 23);
            this.btnGenerarMujeres.TabIndex = 0;
            this.btnGenerarMujeres.Text = "Generar";
            this.btnGenerarMujeres.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.dgvDeportistas);
            this.groupBox3.Controls.Add(this.btnGenerarDeportistas);
            this.groupBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.groupBox3.Location = new System.Drawing.Point(3, 381);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(809, 183);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Rutinas para DEPORTISTAS";
            // 
            // dgvDeportistas
            // 
            this.dgvDeportistas.AllowUserToAddRows = false;
            this.dgvDeportistas.AllowUserToDeleteRows = false;
            this.dgvDeportistas.AllowUserToResizeColumns = false;
            this.dgvDeportistas.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvDeportistas.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvDeportistas.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewTextBoxColumn8});
            this.dgvDeportistas.Location = new System.Drawing.Point(6, 19);
            this.dgvDeportistas.Name = "dgvDeportistas";
            this.dgvDeportistas.ReadOnly = true;
            this.dgvDeportistas.RowHeadersVisible = false;
            this.dgvDeportistas.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvDeportistas.Size = new System.Drawing.Size(722, 158);
            this.dgvDeportistas.TabIndex = 1;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.HeaderText = "Ejercicios";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.HeaderText = "Series";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.HeaderText = "Repeticiones";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.HeaderText = "Descanso (s)";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            // 
            // btnGenerarDeportistas
            // 
            this.btnGenerarDeportistas.Location = new System.Drawing.Point(737, 163);
            this.btnGenerarDeportistas.Name = "btnGenerarDeportistas";
            this.btnGenerarDeportistas.Size = new System.Drawing.Size(75, 23);
            this.btnGenerarDeportistas.TabIndex = 0;
            this.btnGenerarDeportistas.Text = "Generar";
            this.btnGenerarDeportistas.UseVisualStyleBackColor = true;
            // 
            // btnGenerarHombres
            // 
            this.btnGenerarHombres.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.btnGenerarHombres.Location = new System.Drawing.Point(3, 157);
            this.btnGenerarHombres.Name = "btnGenerarHombres";
            this.btnGenerarHombres.Size = new System.Drawing.Size(803, 23);
            this.btnGenerarHombres.TabIndex = 0;
            this.btnGenerarHombres.Text = "Generar";
            this.btnGenerarHombres.UseVisualStyleBackColor = true;
            this.btnGenerarHombres.Click += new System.EventHandler(this.btnGenerarHombres_Click_1);
            // 
            // UcProfesorDashboard
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tableLayoutPanel1);
            this.Name = "UcProfesorDashboard";
            this.Size = new System.Drawing.Size(815, 567);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvHombres)).EndInit();
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMujeres)).EndInit();
            this.groupBox3.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvDeportistas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.DataGridView dgvHombres;
        private System.Windows.Forms.DataGridView dgvMujeres;
        private System.Windows.Forms.Button btnGenerarMujeres;
        private System.Windows.Forms.Button btnGenerarDeportistas;
        private System.Windows.Forms.DataGridView dgvDeportistas;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEjercicio;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSeries;
        private System.Windows.Forms.DataGridViewTextBoxColumn colReps;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDescanso;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn3;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.Button btnGenerarHombres;
    }
}
