using GymManager.Models;
using GymManager.Utils;
using GymManager.Views;
using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace GymManager.Forms
{
    public partial class FrmMain : Form
    {
        // =========================================================
        // 🔹 INSTANCIAS Y VARIABLES PRINCIPALES
        // =========================================================
        private UcGenerarRutinas ucGenerarRutinas;
        private UcEditarRutina ucEditarRutina;
        private UcPlanillasRutinas ucPlanillasRutinas;

        // Botón global de Backup y su etiqueta informativa
        private Button btnBackup;
        private Label lblUltimoBackup;

        // Panel de bienvenida (Dashboard principal)
        private Panel panelDashboard;

        // =========================================================
        // 🔹 CONSTRUCTOR
        // =========================================================
        public FrmMain()
        {
            InitializeComponent();
            InicializarUserControls(); // Instanciar los UserControls sin agregarlos todavía
        }

        // =========================================================
        // 🔹 CREACIÓN DE LOS USERCONTROLS Y DASHBOARD
        // =========================================================
        private void InicializarUserControls()
        {
            ucGenerarRutinas = new UcGenerarRutinas();
            ucEditarRutina = new UcEditarRutina();
            ucPlanillasRutinas = new UcPlanillasRutinas();

            panelDashboard = new Panel
            {
                Dock = DockStyle.Fill,
                BackColor = Color.White
            };
        }

        // =========================================================
        // 🔹 EVENTO LOAD DEL FORM PRINCIPAL
        // =========================================================
        private void FrmMain_Load(object sender, EventArgs e)
        {
            // Validar sesión
            if (Sesion.Actual == null)
            {
                MessageBox.Show("No hay sesión iniciada");
                this.Close();
                return;
            }

            // Colores globales
            var globalColor = Color.FromArgb(45, 52, 70);
            panelNavbar.BackColor = globalColor;
            panelHeader.BackColor = globalColor;
            panelFooter.BackColor = globalColor;

            // --------------------------------------------------------
            // 🏋️ TÍTULO DEL MENÚ LATERAL
            // --------------------------------------------------------
            Label lblTitulo = new Label
            {
                Text = "🏋️ GymManager",
                ForeColor = Color.White,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                Dock = DockStyle.Top,
                Height = 60,
                TextAlign = ContentAlignment.MiddleCenter
            };
            panelNavbar.Controls.Add(lblTitulo);

            // --------------------------------------------------------
            // 🏠 DASHBOARD DE INICIO (Mensaje + logo)
            // --------------------------------------------------------
            Label lbl = new Label
            {
                Text = $"Bienvenido {Sesion.Actual.Nombre}, seleccioná una opción del menú.",
                Dock = DockStyle.Top,
                Font = new Font("Segoe UI", 14, FontStyle.Bold),
                ForeColor = Color.DimGray,
                TextAlign = ContentAlignment.MiddleCenter,
                Height = 60
            };

            PictureBox logo = new PictureBox
            {
                Image = Properties.Resources.Logo_gymM13,
                SizeMode = PictureBoxSizeMode.Zoom,
                Dock = DockStyle.Fill
            };

            panelDashboard.Controls.Add(logo);
            panelDashboard.Controls.Add(lbl);

            // --------------------------------------------------------
            // 📋 AGREGAR LOS PANELES AL CONTENEDOR CENTRAL
            // --------------------------------------------------------
            ucGenerarRutinas.Dock = DockStyle.Fill;
            ucEditarRutina.Dock = DockStyle.Fill;
            ucPlanillasRutinas.Dock = DockStyle.Fill;

            panelContenido.Controls.Add(ucGenerarRutinas);
            panelContenido.Controls.Add(ucEditarRutina);
            panelContenido.Controls.Add(ucPlanillasRutinas);
            panelContenido.Controls.Add(panelDashboard);

            // =========================================================
            // 💾 BOTÓN GLOBAL DE BACKUP (solo si es ADMINISTRADOR)
            // =========================================================
            if (Sesion.Actual.Rol == Rol.Administrador)
            {
                ConfigurarBotonBackup();          // estilo + evento
                CargarUltimoBackupDesdeCarpeta(); // muestra el último .bak
                ColocarBackupArribaDerecha();     // posiciona correctamente
                panelHeader.Resize += (s, ev) => ColocarBackupArribaDerecha();
            }

            // =========================================================
            // ✅ CARGAR NAVEGACIÓN SEGÚN ROL Y MOSTRAR DASHBOARD
            // =========================================================
            CargarNavbar(Sesion.Actual.Rol);
            MostrarDashboard(Sesion.Actual.Rol);
        }

        // =========================================================
        // 🔹 CONFIGURAR BOTÓN BACKUP (estilo y tooltip)
        // =========================================================
        private void ConfigurarBotonBackup()
        {
            btnBackup = new Button
            {
                Text = "💾  Crear Backup",
                Font = new Font("Segoe UI", 9, FontStyle.Bold),
                BackColor = Color.FromArgb(54, 162, 235),
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Height = 32,
                Width = 150,
                Cursor = Cursors.Hand,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };
            btnBackup.FlatAppearance.BorderSize = 0;
            btnBackup.Click += BtnBackup_Click;

            lblUltimoBackup = new Label
            {
                Text = "Último backup: —",
                Font = new Font("Segoe UI", 8.75f, FontStyle.Regular),
                ForeColor = Color.WhiteSmoke,
                AutoSize = true,
                Anchor = AnchorStyles.Top | AnchorStyles.Right
            };

            // Tooltip (mensaje al pasar el mouse)
            var tt = new ToolTip { IsBalloon = false, InitialDelay = 200 };
            tt.SetToolTip(btnBackup, "Crear respaldo de la base de datos (.bak)");

            panelHeader.Controls.Add(btnBackup);
            panelHeader.Controls.Add(lblUltimoBackup);
        }

        // =========================================================
        // 🔸 EVENTO CLICK: HACER BACKUP DE LA BASE DE DATOS
        // =========================================================
        private void BtnBackup_Click(object sender, EventArgs e)
        {
            try
            {
                string defaultFolder = GetDefaultBackupFolder();
                Directory.CreateDirectory(defaultFolder);

                string defaultName = $"GymManagerDB_BACKUP_{DateTime.Now:yyyyMMdd_HHmm}.bak";

                using (var dlg = new SaveFileDialog())
                {
                    dlg.InitialDirectory = defaultFolder;
                    dlg.FileName = defaultName;
                    dlg.Filter = "SQL Server Backup (*.bak)|*.bak";
                    dlg.Title = "Guardar respaldo de la base de datos";

                    if (dlg.ShowDialog() != DialogResult.OK)
                        return;

                    // Realizar el backup
                    HacerBackupFull(dlg.FileName);
                    ActualizarUltimoBackup(DateTime.Now);

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

        // =========================================================
        // 🔸 MÉTODO: REALIZA EL BACKUP FULL
        // =========================================================
        private void HacerBackupFull(string rutaDestino)
        {
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

        // =========================================================
        // 🔸 AUXILIARES DE BACKUP
        // =========================================================
        private string GetDefaultBackupFolder()
        {
            // 📂 Carpeta por defecto (ajustar si cambia el path)
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

                lblUltimoBackup.Text = ultimoBak != null
                    ? $"Último backup: {ultimoBak.LastWriteTime:dd/MM/yyyy - HH:mm} hs"
                    : "Último backup: —";
            }
            catch
            {
                lblUltimoBackup.Text = "Último backup: —";
            }
        }

        private void ActualizarUltimoBackup(DateTime fecha)
        {
            lblUltimoBackup.Text = $"Último backup: {fecha:dd/MM/yyyy - HH:mm} hs";
        }

        private string ObtenerCadenaConexionMaster()
        {
            // 🔹 Adaptado para tus PCs (Bruno / Joni)
            string cs = @"Server=DESKTOP-K765B76\SQLEXPRESS;Database=GymManagerDB;Trusted_Connection=True;";
            // string cs = @"Server=localhost,1433;Database=GymManagerDB;Trusted_Connection=True;";

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

        private void ColocarBackupArribaDerecha()
        {
            if (btnBackup == null || lblUltimoBackup == null) return;

            const int rightPadding = 30;
            const int topPadding = 10;

            // --- Botón ---
            int x = panelHeader.ClientSize.Width - btnBackup.Width - rightPadding;
            int y = topPadding;
            btnBackup.Location = new Point(x, y);

            // --- Label debajo, centrado respecto al botón ---
            lblUltimoBackup.Location = new Point(
                btnBackup.Left -24,           // 5px desplazado para que no empiece justo en el borde
                btnBackup.Bottom + 1
            );

            lblUltimoBackup.BringToFront();
        }


        // =========================================================
        // 🔹 NAVEGACIÓN / UTILIDADES EXISTENTES
        // =========================================================
        private void CargarNavbar(Rol rol)
        {
            panelNavbar.Controls.Clear();
            AgregarBotonNav("Inicio", () => MostrarDashboard(rol), DockStyle.Top);

            if (rol == Rol.Profesor)
            {
                AgregarBotonNav("Generar Rutinas", MostrarGenerarRutinas, DockStyle.Top);
                AgregarBotonNav("Editar Rutina", MostrarEditarRutina, DockStyle.Top);
                AgregarBotonNav("Planillas", MostrarPlanillas, DockStyle.Top);
            }

            if (rol == Rol.Administrador)
            {
                AgregarBotonNav("Usuarios", () => CargarVista(new UcGestionUsuarios()), DockStyle.Top);
                AgregarBotonNav("Ejercicios", () => CargarVista(new UcGestionEjercicios()), DockStyle.Top);
                AgregarBotonNav("Reportes", () => CargarVista(new UcReportes()), DockStyle.Top);
            }

            if (rol == Rol.Recepcionista)
            {
                AgregarBotonNav("Rutina", () => CargarVista(new UcRecepcionistaDashboard()), DockStyle.Top);
            }

            AgregarBotonNav("Cerrar sesión", () =>
            {
                Sesion.Cerrar();
                Application.Restart();
            }, DockStyle.Bottom);
        }

        private void MostrarGenerarRutinas() => ucGenerarRutinas.BringToFront();

        public void MostrarEditarRutina()
        {
            var listaHombres = ucGenerarRutinas.rutinaHombres;
            var listaMujeres = ucGenerarRutinas.rutinaMujeres;
            var listaDeportistas = ucGenerarRutinas.rutinaDeportistas;

            ucEditarRutina.ActualizarYMostrarPanelSeleccion(listaHombres, listaMujeres, listaDeportistas);
            ucEditarRutina.BringToFront();
        }

        public void MostrarPlanillas()
        {
            ucPlanillasRutinas.BringToFront();
            ucPlanillasRutinas.CargarDatos();
        }

        private void CargarVista(UserControl vista)
        {
            panelContenido.Controls.Clear();
            vista.Dock = DockStyle.Fill;
            panelContenido.Controls.Add(vista);
        }

        private void AgregarBotonNav(string texto, Action onClick, DockStyle dockPos)
        {
            var btn = new Button
            {
                Text = texto,
                Dock = dockPos,
                Height = 50,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                BackColor = texto == "Cerrar sesión" ? Color.DarkRed : Color.FromArgb(45, 62, 80),
                ForeColor = Color.White
            };

            btn.FlatAppearance.BorderSize = 0;
            btn.Click += (s, e) => onClick();
            panelNavbar.Controls.Add(btn);
        }

        public void LimpiarRutinaGeneradaEnPanel(string tipoRutina)
        {
            ucGenerarRutinas?.LimpiarRutinaGenerada(tipoRutina);
        }

        private void MostrarDashboard(Rol rol)
        {
            panelDashboard.BringToFront();
        }
    }
}
