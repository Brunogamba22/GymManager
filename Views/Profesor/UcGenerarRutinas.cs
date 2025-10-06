using GymManager.Controllers;
using GymManager.Models;
using GymManager.Models.Events;
using GymManager.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Views
{
    public partial class UcGenerarRutinas : UserControl
    {
        // Colores personalizados
        private Color primaryColor = Color.FromArgb(46, 134, 171);
        private Color secondaryColor = Color.FromArgb(162, 59, 114);
        private Color successColor = Color.FromArgb(28, 167, 69);
        private Color backgroundColor = Color.FromArgb(248, 249, 250);
        private Color textColor = Color.FromArgb(33, 37, 41);
        private Color borderColor = Color.FromArgb(222, 226, 230);
        private Color tabHoverColor = Color.FromArgb(240, 240, 240);
        private Color warningColor = Color.FromArgb(255, 193, 7);
        private Color dangerColor = Color.FromArgb(220, 53, 69);

        private Panel[] tabPanels;
        private Label[] tabLabels;

        // Variables para almacenar las rutinas generadas
        private List<RutinaSimulador.EjercicioRutina> rutinaHombres = new List<RutinaSimulador.EjercicioRutina>();
        private List<RutinaSimulador.EjercicioRutina> rutinaMujeres = new List<RutinaSimulador.EjercicioRutina>();
        private List<RutinaSimulador.EjercicioRutina> rutinaDeportistas = new List<RutinaSimulador.EjercicioRutina>();

        //  VARIABLES PARA LOS BOTONES DE ACCIÓN
        private Button btnEditarHombres;
        private Button btnLimpiarHombres;
        private Button btnEditarMujeres;
        private Button btnLimpiarMujeres;
        private Button btnEditarDeportistas;
        private Button btnLimpiarDeportistas;

        // NUEVAS VARIABLES PARA BOTONES GUARDAR
        private Button btnGuardarHombres;
        private Button btnGuardarMujeres;
        private Button btnGuardarDeportistas;

        public UcGenerarRutinas()
        {
            InitializeComponent();
            ApplyModernStyles();
            ConfigurarGrids();
            SetupTabSystem();
        }

        private void ApplyModernStyles()
        {
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI", 9, FontStyle.Regular);
        }

        private void SetupTabSystem()
        {
            tabPanels = new Panel[] { panelHombres, panelMujeres, panelDeportistas };
            tabLabels = new Label[] { lblTabHombres, lblTabMujeres, lblTabDeportistas };
            ShowTab(0);
        }

        private void ShowTab(int tabIndex)
        {
            foreach (var panel in tabPanels)
            {
                panel.Visible = false;
            }

            foreach (var label in tabLabels)
            {
                label.BackColor = Color.White;
                label.ForeColor = textColor;
                label.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            }

            tabPanels[tabIndex].Visible = true;
            tabLabels[tabIndex].BackColor = GetTabColor(tabIndex);
            tabLabels[tabIndex].ForeColor = Color.White;
            tabLabels[tabIndex].Font = new Font("Segoe UI", 10, FontStyle.Bold);
        }

        private Color GetTabColor(int tabIndex)
        {
            return tabIndex switch
            {
                0 => primaryColor,
                1 => secondaryColor,
                2 => successColor,
                _ => primaryColor
            };
        }

        private void ConfigurarGrids()
        {
            ConfigurarGrid(dgvHombres);
            ConfigurarGrid(dgvMujeres);
            ConfigurarGrid(dgvDeportistas);
        }

        private void ConfigurarGrid(DataGridView dgv)
        {
            dgv.BackgroundColor = Color.White;
            dgv.BorderStyle = BorderStyle.None;
            dgv.EnableHeadersVisualStyles = false;
            dgv.AllowUserToAddRows = false;
            dgv.AllowUserToDeleteRows = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ReadOnly = true;
            dgv.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgv.RowHeadersVisible = false;
            dgv.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dgv.RowTemplate.Height = 35;
            dgv.ColumnHeadersHeight = 38;

            dgv.AllowUserToResizeColumns = false;
            dgv.AllowUserToResizeRows = false;
            dgv.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            dgv.DefaultCellStyle.SelectionBackColor = Color.FromArgb(240, 240, 240);
            dgv.DefaultCellStyle.SelectionForeColor = textColor;

            // Estilo de encabezados
            dgv.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(240, 240, 240);
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = textColor;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgv.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;

            // Estilo de celdas
            dgv.DefaultCellStyle.BackColor = Color.White;
            dgv.DefaultCellStyle.ForeColor = textColor;
            dgv.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgv.DefaultCellStyle.Padding = new Padding(3, 4, 3, 4);

            // Estilo de filas alternadas
            dgv.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dgv.GridColor = borderColor;
        }

        private void StyleButton(Button btn, Color bgColor)
        {
            btn.BackColor = bgColor;
            btn.ForeColor = Color.White;
            btn.FlatStyle = FlatStyle.Flat;
            btn.FlatAppearance.BorderSize = 0;
            btn.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            btn.Cursor = Cursors.Hand;
            btn.Padding = new Padding(12, 6, 12, 6);

            btn.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(bgColor, 0.1f);
            btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(bgColor, 0.2f);
        }

        // Eventos para las tabs
        private void lblTabHombres_Click(object sender, EventArgs e) => ShowTab(0);
        private void lblTabMujeres_Click(object sender, EventArgs e) => ShowTab(1);
        private void lblTabDeportistas_Click(object sender, EventArgs e) => ShowTab(2);

        // Eventos hover para tabs
        private void TabLabel_MouseEnter(object sender, EventArgs e)
        {
            var label = (Label)sender;
            if (!label.BackColor.Equals(Color.White)) return;
            label.BackColor = tabHoverColor;
            label.Cursor = Cursors.Hand;
        }

        private void TabLabel_MouseLeave(object sender, EventArgs e)
        {
            var label = (Label)sender;
            if (!label.BackColor.Equals(tabHoverColor)) return;
            label.BackColor = Color.White;
        }

        // MÉTODOS DE GENERACIÓN MEJORADOS
        private void btnGenerarHombres_Click(object sender, EventArgs e)
        {
            try
            {
                rutinaHombres = RutinaSimulador.GenerarRutina("Hombres");
                MostrarRutinaEnGrid(dgvHombres, rutinaHombres);

                // Habilitar botones de acción
                btnEditarHombres.Enabled = true;
                btnLimpiarHombres.Enabled = true;
                btnGuardarHombres.Enabled = true;

                MessageBox.Show($"✅ Rutina para HOMBRES generada exitosamente\n📊 Ejercicios: {rutinaHombres.Count}",
                              "Generación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar rutina: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGenerarMujeres_Click(object sender, EventArgs e)
        {
            try
            {
                rutinaMujeres = RutinaSimulador.GenerarRutina("Mujeres");
                MostrarRutinaEnGrid(dgvMujeres, rutinaMujeres);

                btnEditarMujeres.Enabled = true;
                btnLimpiarMujeres.Enabled = true;
                btnGuardarMujeres.Enabled = true;

                MessageBox.Show($"✅ Rutina para MUJERES generada exitosamente\n📊 Ejercicios: {rutinaMujeres.Count}",
                              "Generación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar rutina: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnGenerarDeportistas_Click(object sender, EventArgs e)
        {
            try
            {
                rutinaDeportistas = RutinaSimulador.GenerarRutina("Deportistas");
                MostrarRutinaEnGrid(dgvDeportistas, rutinaDeportistas);

                btnEditarDeportistas.Enabled = true;
                btnLimpiarDeportistas.Enabled = true;
                btnGuardarDeportistas.Enabled = true;

                MessageBox.Show($"✅ Rutina para DEPORTISTAS generada exitosamente\n📊 Ejercicios: {rutinaDeportistas.Count}",
                              "Generación Exitosa", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al generar rutina: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarRutinaEnGrid(DataGridView dgv, List<RutinaSimulador.EjercicioRutina> rutina)
        {
            dgv.Rows.Clear();
            foreach (var ejercicio in rutina)
            {
                dgv.Rows.Add(ejercicio.Nombre, ejercicio.Series, ejercicio.Repeticiones, $"{ejercicio.Descanso} s");
            }
        }

        // BOTONES EDITAR
        private void btnEditarHombres_Click(object sender, EventArgs e)
        {
            if (rutinaHombres.Count == 0)
            {
                MessageBox.Show("No hay rutina generada para editar", "Información",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 🔥 CORREGIDO: Pasar el nombre de la rutina
            string nombreRutina = $"HOMBRES.{DateTime.Now:yyyyMMdd_HHmmss}";
            EventosRutina.DispararRutinaGenerada("HOMBRES", nombreRutina, rutinaHombres);
            MessageBox.Show("Rutina de HOMBRES lista para editar", "Edición",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEditarMujeres_Click(object sender, EventArgs e)
        {
            if (rutinaMujeres.Count == 0)
            {
                MessageBox.Show("No hay rutina generada para editar", "Información",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 🔥 CORREGIDO: Pasar el nombre de la rutina
            string nombreRutina = $"MUJERES.{DateTime.Now:yyyyMMdd_HHmmss}";
            EventosRutina.DispararRutinaGenerada("MUJERES", nombreRutina, rutinaMujeres);
            MessageBox.Show("Rutina de MUJERES lista para editar", "Edición",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnEditarDeportistas_Click(object sender, EventArgs e)
        {
            if (rutinaDeportistas.Count == 0)
            {
                MessageBox.Show("No hay rutina generada para editar", "Información",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // 🔥 CORREGIDO: Pasar el nombre de la rutina
            string nombreRutina = $"DEPORTISTAS.{DateTime.Now:yyyyMMdd_HHmmss}";
            EventosRutina.DispararRutinaGenerada("DEPORTISTAS", nombreRutina, rutinaDeportistas);
            MessageBox.Show("Rutina de DEPORTISTAS lista para editar", "Edición",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // 🔥 NUEVOS MÉTODOS PARA GUARDAR RUTINAS
        private void btnGuardarHombres_Click(object sender, EventArgs e)
        {
            GuardarRutina("HOMBRES", rutinaHombres);
        }

        private void btnGuardarMujeres_Click(object sender, EventArgs e)
        {
            GuardarRutina("MUJERES", rutinaMujeres);
        }

        private void btnGuardarDeportistas_Click(object sender, EventArgs e)
        {
            GuardarRutina("DEPORTISTAS", rutinaDeportistas);
        }

        private void GuardarRutina(string tipoRutina)
        {
            try
            {
                var ejercicioController = new EjercicioController();
                var detalleController = new DetalleRutinaController();
                var rutinaController = new RutinaController();

                // 1️⃣ Obtener ejercicios reales desde la BD
                var ejercicios = ejercicioController.ObtenerTodos();
                if (ejercicios.Count == 0)
                {
                    MessageBox.Show("No hay ejercicios disponibles en la base de datos.",
                                    "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                // 2️⃣ Crear nueva rutina
                int idRutina = rutinaController.CrearNuevaRutina(
                    tipoRutina,
                    Sesion.Actual.IdUsuario,  // Profesor actual
                    $"Rutina {tipoRutina} - {DateTime.Now:dd/MM/yyyy}"
                );

                // 3️⃣ Insertar los detalles de cada ejercicio
                foreach (var ej in ejercicios)
                {
                    var detalle = new DetalleRutina
                    {
                        IdRutina = idRutina,
                        IdEjercicio = ej.Id,       // ✅ Id real del ejercicio desde BD
                        Series = 4,                // Default o configurable
                        Repeticiones = 12,
                        Descanso = 60,
                        Carga = null
                    };

                    detalleController.Agregar(detalle);
                }

                // 4️⃣ Disparar evento para refrescar planillas o vistas
                EventosRutina.DispararRutinaGuardada(
                    $"Rutina {tipoRutina} - {DateTime.Now:yyyyMMdd_HHmmss}",
                    tipoRutina,
                    DateTime.Now,
                    null // No pasamos lista simulada, ya viene desde BD
                );

                MessageBox.Show($"✅ Rutina de {tipoRutina} guardada correctamente con {ejercicios.Count} ejercicios.",
                                "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la rutina: {ex.Message}",
                                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        // BOTONES LIMPIAR
        private void btnLimpiarHombres_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Estás seguro de que quieres limpiar la rutina de HOMBRES?",
                                       "Confirmar Limpieza", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                dgvHombres.Rows.Clear();
                rutinaHombres.Clear();
                btnEditarHombres.Enabled = false;
                btnLimpiarHombres.Enabled = false;
                btnGuardarHombres.Enabled = false;
            }
        }

        private void btnLimpiarMujeres_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Estás seguro de que quieres limpiar la rutina de MUJERES?",
                                       "Confirmar Limpieza", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                dgvMujeres.Rows.Clear();
                rutinaMujeres.Clear();
                btnEditarMujeres.Enabled = false;
                btnLimpiarMujeres.Enabled = false;
                btnGuardarMujeres.Enabled = false;
            }
        }

        private void btnLimpiarDeportistas_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Estás seguro de que quieres limpiar la rutina de DEPORTISTAS?",
                                       "Confirmar Limpieza", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                dgvDeportistas.Rows.Clear();
                rutinaDeportistas.Clear();
                btnEditarDeportistas.Enabled = false;
                btnLimpiarDeportistas.Enabled = false;
                btnGuardarDeportistas.Enabled = false;
            }
        }

        // MÉTODO PARA RESTAURAR RUTINAS AL VOLVER A LA PESTAÑA
        public void RestaurarRutinas()
        {
            if (rutinaHombres.Count > 0)
            {
                MostrarRutinaEnGrid(dgvHombres, rutinaHombres);
                if (btnEditarHombres != null) btnEditarHombres.Enabled = true;
                if (btnLimpiarHombres != null) btnLimpiarHombres.Enabled = true;
                if (btnGuardarHombres != null) btnGuardarHombres.Enabled = true;
            }

            if (rutinaMujeres.Count > 0)
            {
                MostrarRutinaEnGrid(dgvMujeres, rutinaMujeres);
                if (btnEditarMujeres != null) btnEditarMujeres.Enabled = true;
                if (btnLimpiarMujeres != null) btnLimpiarMujeres.Enabled = true;
                if (btnGuardarMujeres != null) btnGuardarMujeres.Enabled = true;
            }

            if (rutinaDeportistas.Count > 0)
            {
                MostrarRutinaEnGrid(dgvDeportistas, rutinaDeportistas);
                if (btnEditarDeportistas != null) btnEditarDeportistas.Enabled = true;
                if (btnLimpiarDeportistas != null) btnLimpiarDeportistas.Enabled = true;
                if (btnGuardarDeportistas != null) btnGuardarDeportistas.Enabled = true;
            }
        }
    }
}