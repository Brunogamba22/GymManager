using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using GymManager.Controllers;
using GymManager.Models;

namespace GymManager.Views
{
    public partial class UcReportes : UserControl
    {
        private readonly UsuarioController controladorUsuarios = new UsuarioController();
        private readonly EjercicioController controladorEjercicios = new EjercicioController();

        // ============================================================
        // 🔹 CONSTRUCTOR
        // ============================================================
        public UcReportes()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(245, 247, 250); // fondo gris claro
        }

        private void UcReportes_Load(object sender, EventArgs e)
        {
            CargarDatosGenerales();
            ConfigurarGraficos();
            ConfigurarCards();
            CargarGraficoUsuarios();
            CargarGraficoEjercicios();

            // Configurar estilo del botón Backup
            ConfigurarBotonBackup();

            ColocarBackupArribaDerecha();                 //  forzamos posición/visible
            CargarUltimoBackupDesdeCarpeta();   // muestra el último .bak si existe
            this.Resize += (s, ev) => ColocarBackupArribaDerecha();//  que siga anclado



        }

        // ============================================================
        //  CONFIGURAR BOTÓN BACKUP (estilo + evento)
        // ============================================================
        private void ConfigurarBotonBackup()
        {
            btnBackup.Text = "💾  Crear Backup";
            btnBackup.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btnBackup.BackColor = Color.FromArgb(54, 162, 235);
            btnBackup.ForeColor = Color.White;
            btnBackup.FlatStyle = FlatStyle.Flat;
            btnBackup.FlatAppearance.BorderSize = 0;
            btnBackup.Cursor = Cursors.Hand;
            btnBackup.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnBackup.Click += BtnBackup_Click;
            // Tooltip
            // Mostrar texto al pasar el mouse sobre el botón
            var tt = new ToolTip { IsBalloon = false, InitialDelay = 200 };
            tt.SetToolTip(btnBackup, "Crear respaldo de la base de datos (.bak)");

        }

        // ============================================================
        // 🔸 EVENTO CLICK: HACER BACKUP DE LA BASE DE DATOS
        // ============================================================
        private void BtnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                // Ruta por defecto (puede ser externa)
                string defaultFolder = @"C:\Usuarios\Bruno\source\repos\Backups_GymManager\";
                Directory.CreateDirectory(defaultFolder);

                string defaultName = $"GymManagerDB_BACKUP_{DateTime.Now:yyyyMMdd_HHmm}.bak";
                // Diálogo para guardar el archivo .bak
                using (var dlg = new SaveFileDialog())
                {
                    dlg.InitialDirectory = defaultFolder;
                    dlg.FileName = defaultName;
                    dlg.Filter = "SQL Server Backup (*.bak)|*.bak";
                    dlg.Title = "Guardar respaldo de la base de datos";

                    if (dlg.ShowDialog() != DialogResult.OK)
                        return;

                    // Ejecutar respaldo
                    // Realizar el backup full
                    HacerBackupFull(dlg.FileName);
                    // Actualizar la etiqueta del último backup                  
                    ActualizarUltimoBackup(DateTime.Now);

                    // Mostrar mensaje de éxito
                    MessageBox.Show($"✅ Backup completado con éxito.\nArchivo guardado en:\n{dlg.FileName}",
                                    "Respaldo realizado",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);
                }
            }
            
            catch (Exception ex)
            {
                MessageBox.Show("❌ Error al realizar el backup:\n" + ex.Message,
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        // ============================================================
        // 🔸 MÉTODO: REALIZA EL BACKUP FULL
        // ============================================================
        private void HacerBackupFull(string rutaDestino)
        {
            // Usamos la base master para ejecutar el BACKUP
            string cadena = ObtenerCadenaConexionMaster();

            using (var cn = new SqlConnection(cadena))
            using (var cmd = cn.CreateCommand())
            {
                cn.Open();
                cmd.CommandTimeout = 0;
                cmd.CommandText = @"
                    BACKUP DATABASE GymManagerDB
                    TO DISK = @ruta
                    WITH INIT, STATS = 10;";
                cmd.Parameters.AddWithValue("@ruta", rutaDestino);
                cmd.ExecuteNonQuery();
            }
        }
        // ============================================================
        // 🔸 MÉTODOS AUXILIARES BACKUP
        private string GetDefaultBackupFolder()
        {
            // Ajustá si cambiás la carpeta
            return @"C:\Usuarios\Bruno\source\repos\Backups_GymManager\";
        }

        private void CargarUltimoBackupDesdeCarpeta()
        {
            try
            {
                var folder = GetDefaultBackupFolder();
                if (!Directory.Exists(folder))
                {
                    lblUltimoBackup.Text = "Último backup: —";
                    return;
                }

                var ultimoBak = new DirectoryInfo(folder)
                    .GetFiles("*.bak")
                    .OrderByDescending(f => f.LastWriteTime)
                    .FirstOrDefault();

                if (ultimoBak != null)
                {
                    lblUltimoBackup.Text = $"Último backup: {ultimoBak.LastWriteTime:dd/MM/yyyy - HH:mm} hs";
                }
                else
                {
                    lblUltimoBackup.Text = "Último backup: —";
                }
            }
            catch
            {
                lblUltimoBackup.Text = "Último backup: —";
            }
        }
        // Actualiza la etiqueta del último backup con una fecha dada
        private void ActualizarUltimoBackup(DateTime fecha)
        {
            lblUltimoBackup.Text = $"Último backup: {fecha:dd/MM/yyyy - HH:mm} hs";
        }




        private string ObtenerCadenaConexionMaster()
        {
            // Adaptá tu cadena según la configuración del proyecto
            string cs = @"Server=DESKTOP-K765B76\SQLEXPRESS;Database=GymManagerDB;Trusted_Connection=True;";

            // Reemplaza el nombre de base por "master"
            try
            {
                var builder = new SqlConnectionStringBuilder(cs);
                builder.InitialCatalog = "master";
                return builder.ToString();
            }
            catch
            {
                return cs.Replace("Database=GymManagerDB", "Database=master")
                         .Replace("Initial Catalog=GymManagerDB", "Initial Catalog=master");
            }
        }

        // ============================================================
        // 🔹 DATOS GENERALES
        // ============================================================
        private void CargarDatosGenerales()
        {
            int totalUsuarios = controladorUsuarios.ObtenerTodos()
                                                    .Count(u => u.Activo); // solo activos
            int totalEjercicios = controladorEjercicios.ObtenerTodos().Count;

            lblTotalUsuarios.Text = totalUsuarios.ToString();
            lblTotalEjercicios.Text = totalEjercicios.ToString();
        }


        // ============================================================
        // 🔹 CONFIGURACIÓN DE GRÁFICOS
        // ============================================================
        private void ConfigurarGraficos()
        {
            // Usuarios
            chartUsuarios.ChartAreas[0].BackColor = Color.White;
            chartUsuarios.BackColor = Color.White;
            chartUsuarios.Legends[0].Docking = Docking.Right;
            chartUsuarios.Legends[0].Font = new Font("Segoe UI", 8);
            chartUsuarios.Legends[0].BackColor = Color.White;

            // Ejercicios
            var area = chartEjercicios.ChartAreas[0];
            area.BackColor = Color.White;
            area.AxisX.MajorGrid.Enabled = false;
            area.AxisY.MajorGrid.LineColor = Color.LightGray;
            area.AxisX.LabelStyle.Font = new Font("Segoe UI", 9);
            area.AxisY.LabelStyle.Font = new Font("Segoe UI", 9);
            area.AxisX.Title = "Grupos musculares";
            area.AxisY.Title = "Cantidad";
            area.AxisX.Interval = 1;
        }

        // ============================================================
        // 🥧 GRÁFICO USUARIOS
        // ============================================================
        private void CargarGraficoUsuarios()
        {
 
            var lista = controladorUsuarios.ObtenerTodos()
                               .Where(u => u.Activo) // solo activos
                               .ToList();


            int admins = lista.Count(u => u.Rol == Rol.Administrador);
            int profes = lista.Count(u => u.Rol == Rol.Profesor);
            int receps = lista.Count(u => u.Rol == Rol.Recepcionista);

            chartUsuarios.Series.Clear();
            var serie = new Series("Usuarios")
            {
                ChartType = SeriesChartType.Pie,
                IsValueShownAsLabel = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                LabelForeColor = Color.Black
            };

            serie.Points.AddXY("Administradores", admins);
            serie.Points.AddXY("Profesores", profes);
            serie.Points.AddXY("Recepcionistas", receps);

            serie.Points[0].Color = Color.FromArgb(54, 162, 235);
            serie.Points[1].Color = Color.FromArgb(255, 206, 86);
            serie.Points[2].Color = Color.FromArgb(255, 99, 132);
            serie["PieDrawingStyle"] = "Concave";
            serie["PieLabelStyle"] = "Outside";
            serie.SmartLabelStyle.Enabled = true;
            chartUsuarios.Series.Add(serie);
        }

        // ============================================================
        // 📊 GRÁFICO EJERCICIOS
        // ============================================================
        private void CargarGraficoEjercicios()
        {
            var lista = controladorEjercicios.ObtenerTodos();
            var grupos = lista
                .GroupBy(e => string.IsNullOrWhiteSpace(e.GrupoMuscularNombre) ? "Sin grupo" : e.GrupoMuscularNombre)
                .Select(g => new { Grupo = g.Key, Cantidad = g.Count() })
                .OrderBy(g => g.Grupo)
                .ToList();

            chartEjercicios.Series.Clear();
            var serie = new Series("Ejercicios")
            {
                ChartType = SeriesChartType.Column,
                IsValueShownAsLabel = true,
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                LabelForeColor = Color.Black
            };

            foreach (var grupo in grupos)
            {
                int idx = serie.Points.AddXY(grupo.Grupo, grupo.Cantidad);
                serie.Points[idx].Color = Color.FromArgb(54, 162, 235);
            }

            serie["PointWidth"] = "0.55";
            chartEjercicios.Series.Add(serie);

            var area = chartEjercicios.ChartAreas[0];
            area.AxisX.Interval = 1;
            area.AxisX.LabelStyle.Angle = -30;
        }

        // ============================================================
        // 🧱 CONFIGURACIÓN DE LAS CARDS
        // ============================================================
        private void ConfigurarCards()
        {
            // --- CARD USUARIOS
            lblUsuariosTxt.Text = "Total de usuarios";
            lblUsuariosTxt.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblTotalUsuarios.Font = new Font("Segoe UI", 30, FontStyle.Bold);
            lblTotalUsuarios.ForeColor = Color.FromArgb(54, 162, 235);
            lblTotalUsuarios.Location = new Point(25, 45);

            var iconUsuarios = new Label
            {
                Text = "👥",
                Font = new Font("Segoe UI Emoji", 34),
                AutoSize = true,
                Location = new Point(190, 40)
            };
            cardUsuarios.Controls.Add(iconUsuarios);

            // --- CARD EJERCICIOS
            lblEjerciciosTxt.Text = "Total de ejercicios";
            lblEjerciciosTxt.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            lblTotalEjercicios.Font = new Font("Segoe UI", 30, FontStyle.Bold);
            lblTotalEjercicios.ForeColor = Color.FromArgb(255, 159, 64);
            lblTotalEjercicios.Location = new Point(25, 45);

            var iconEjercicios = new Label
            {
                Text = "💪",
                Font = new Font("Segoe UI Emoji", 34),
                AutoSize = true,
                Location = new Point(190, 40)
            };
            cardEjercicios.Controls.Add(iconEjercicios);
        }

        private void ColocarBackupArribaDerecha()
        {
            if (btnBackup == null || lblUltimoBackup == null) return;

            // Margen desde el borde derecho y superior
            const int rightPadding = 60;  //  más espacio desde la derecha 
            const int topPadding = 25;    //  más espacio desde arriba

            // --- Botón ---
            btnBackup.Visible = true;
            btnBackup.Parent = this;
            btnBackup.BringToFront();

            int x = this.ClientSize.Width - btnBackup.Width - rightPadding;
            int y = topPadding;
            btnBackup.Location = new Point(x, y);

            // --- Label debajo del botón ---
            lblUltimoBackup.Visible = true;
            lblUltimoBackup.Parent = this;
            lblUltimoBackup.BringToFront();
            lblUltimoBackup.Location = new Point(
                btnBackup.Left,
                btnBackup.Bottom + 6
            );
        }

        // ============================================================
        // 🔁 MÉTODO PÚBLICO PARA REFRESCAR LOS GRÁFICOS Y CONTADORES
        // ============================================================
        public void RefrescarGraficos()
        {
            // Vuelve a contar usuarios activos y ejercicios
            CargarDatosGenerales();
            CargarGraficoUsuarios();
            CargarGraficoEjercicios();
        }

    }
}
