using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GymManager.Controllers;
using GymManager.Models;
using GymManager.Utils;

namespace GymManager.Views
{
    public partial class UcGenerarRutinas : UserControl
    {
        // ------------------------------------------------------------
        // 🎨 COLORES Y ARREGLOS PARA LA UI (VERSIÓN FINAL COMPATIBLE)
        // ------------------------------------------------------------
        private Color primaryColor = Color.FromArgb(46, 134, 171);
        private Color secondaryColor = Color.FromArgb(162, 59, 114);
        private Color successColor = Color.FromArgb(28, 167, 69);
        private Color dangerColor = Color.FromArgb(220, 53, 69);
        private Color warningColor = Color.FromArgb(255, 193, 7);
        private Color textColor = Color.FromArgb(33, 37, 41);
        private Color borderColor = Color.FromArgb(222, 226, 230);
        private Color tabHoverColor = Color.FromArgb(240, 240, 240);
        // VARIABLE QUE FALTABA Y CAUSABA EL ERROR
        private Color backgroundColor = Color.FromArgb(248, 249, 250);
        private Panel[] tabPanels;
        private Label[] tabLabels;

        // ------------------------------------------------------------
        // ❗ LISTAS Y CONTROLADORES REALES
        // ------------------------------------------------------------
        private readonly List<DetalleRutina> rutinaHombres = new List<DetalleRutina>();
        private readonly List<DetalleRutina> rutinaMujeres = new List<DetalleRutina>();
        private readonly List<DetalleRutina> rutinaDeportistas = new List<DetalleRutina>();

        private readonly EjercicioController _ejercicioController = new EjercicioController();
        private readonly RutinaController _rutinaController = new RutinaController();

        // ------------------------------------------------------------
        // 🔘 REFERENCIAS A BOTONES CREADOS POR EL DISEÑADOR
        // ------------------------------------------------------------
        private Button btnEditarHombres, btnLimpiarHombres, btnGuardarHombres;
        private Button btnEditarMujeres, btnLimpiarMujeres, btnGuardarMujeres;
        private Button btnEditarDeportistas, btnLimpiarDeportistas, btnGuardarDeportistas;

        // ------------------------------------------------------------
        // 🏁 CONSTRUCTOR
        // ------------------------------------------------------------
        public UcGenerarRutinas()
        {
            InitializeComponent();
            this.Load += UcGenerarRutinas_Load;
        }

        // ------------------------------------------------------------
        // 🔧 CONFIGURACIÓN INICIAL AL CARGAR EL CONTROL
        // ------------------------------------------------------------
        private void UcGenerarRutinas_Load(object sender, EventArgs e)
        {
            tabPanels = new[] { panelHombres, panelMujeres, panelDeportistas };
            tabLabels = new[] { lblTabHombres, lblTabMujeres, lblTabDeportistas };
            ShowTab(0);

            StyleButton(btnGenerarHombres, primaryColor);
            StyleButton(btnGenerarMujeres, secondaryColor);
            StyleButton(btnGenerarDeportistas, successColor);
        }

        #region "Lógica de UI (Tabs, Estilos, etc.)"

        private void StyleButton(Button boton, Color colorFondo)
        {
            if (boton == null) return;
            boton.BackColor = colorFondo;
            boton.ForeColor = Color.White;
            boton.FlatStyle = FlatStyle.Flat;
            boton.FlatAppearance.BorderSize = 0;
            boton.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            boton.Cursor = Cursors.Hand;
            boton.Padding = new Padding(12, 6, 12, 6);
            boton.FlatAppearance.MouseOverBackColor = ControlPaint.Dark(colorFondo, 0.1f);
            boton.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(colorFondo, 0.2f);
        }

        private void ShowTab(int indiceTab)
        {
            foreach (Panel panel in tabPanels) panel.Visible = false;
            foreach (Label etiqueta in tabLabels)
            {
                etiqueta.BackColor = Color.White;
                etiqueta.ForeColor = textColor;
                etiqueta.Font = new Font("Segoe UI", 10, FontStyle.Regular);
            }
            tabPanels[indiceTab].Visible = true;
            tabLabels[indiceTab].BackColor = GetTabColor(indiceTab);
            tabLabels[indiceTab].ForeColor = Color.White;
            tabLabels[indiceTab].Font = new Font("Segoe UI", 10, FontStyle.Bold);
        }

        private Color GetTabColor(int indiceTab) => indiceTab switch
        {
            0 => primaryColor,
            1 => secondaryColor,
            2 => successColor,
            _ => primaryColor
        };

        // Nombres de evento en minúscula para coincidir con el diseñador
        private void lblTabHombres_Click(object sender, EventArgs e) => ShowTab(0);
        private void lblTabMujeres_Click(object sender, EventArgs e) => ShowTab(1);
        private void lblTabDeportistas_Click(object sender, EventArgs e) => ShowTab(2);

        private void TabLabel_MouseEnter(object sender, EventArgs e)
        {
            if (sender is Label etiqueta)
            {
                // Lógica de hover...
            }
        }
        private void TabLabel_MouseLeave(object sender, EventArgs e)
        {
            if (sender is Label etiqueta)
            {
                // Lógica de hover...
            }
        }

        #endregion

        // ------------------------------------------------------------
        // ⚙️ EVENTOS "GENERAR" (CONECTADOS A LA BASE DE DATOS)
        // ------------------------------------------------------------
        private void btnGenerarHombres_Click(object sender, EventArgs e)
            => GenerarRutinaReal("Hombres", dgvHombres, rutinaHombres);

        private void btnGenerarMujeres_Click(object sender, EventArgs e)
            => GenerarRutinaReal("Mujeres", dgvMujeres, rutinaMujeres);

        private void btnGenerarDeportistas_Click(object sender, EventArgs e)
            => GenerarRutinaReal("Deportistas", dgvDeportistas, rutinaDeportistas);

        private void GenerarRutinaReal(string tipo, DataGridView grilla, List<DetalleRutina> listaRutina)
        {
            try
            {
                listaRutina.Clear();
                Dictionary<string, int> ejerciciosPorGrupo = ObtenerGruposParaTipo(tipo);
                var ejerciciosParaRutina = new List<Ejercicio>();

                foreach (var par in ejerciciosPorGrupo)
                {
                    string grupoMuscular = par.Key;
                    int cantidad = par.Value;
                    List<Ejercicio> disponibles = _ejercicioController.ObtenerPorGrupoMuscular(grupoMuscular);

                    if (disponibles.Count < cantidad)
                    {
                        MessageBox.Show($"No hay suficientes ejercicios de '{grupoMuscular}'. Se necesitan {cantidad} y solo hay {disponibles.Count}.", "Datos insuficientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    Random rnd = new Random();
                    ejerciciosParaRutina.AddRange(disponibles.OrderBy(x => rnd.Next()).Take(cantidad));
                }

                foreach (var ejercicio in ejerciciosParaRutina)
                {
                    listaRutina.Add(new DetalleRutina
                    {
                        EjercicioNombre = ejercicio.Nombre,
                        IdEjercicio = ejercicio.Id,
                        Series = 4,
                        Repeticiones = 12,
                        Descanso = 60
                    });
                }

                MostrarRutinaEnGrid(grilla, listaRutina);

                if (tipo == "Hombres") HabilitarAccionesHombres(true);
                if (tipo == "Mujeres") HabilitarAccionesMujeres(true);
                if (tipo == "Deportistas") HabilitarAccionesDeportistas(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al generar la rutina: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private Dictionary<string, int> ObtenerGruposParaTipo(string tipo)
        {
            switch (tipo)
            {
                case "Hombres":
                    return new Dictionary<string, int> { { "Pecho", 2 }, { "Espalda", 2 }, { "Hombro", 1 } };
                case "Mujeres":
                    return new Dictionary<string, int> { { "Piernas", 3 }, { "Glúteos", 2 } };
                case "Deportistas":
                    return new Dictionary<string, int> { { "Pecho", 1 }, { "Espalda", 1 }, { "Piernas", 1 }, { "Hombro", 1 }, { "Brazos", 1 } };
                default:
                    return new Dictionary<string, int>();
            }
        }

        private void MostrarRutinaEnGrid(DataGridView grilla, List<DetalleRutina> rutina)
        {
            grilla.Rows.Clear();
            foreach (var detalle in rutina)
            {
                grilla.Rows.Add(
                    detalle.EjercicioNombre,
                    detalle.Series,
                    detalle.Repeticiones,
                    $"{detalle.Descanso} s"
                );
            }
        }

        // ------------------------------------------------------------
        // 💾 EVENTOS "GUARDAR" (PERSISTEN LA RUTINA GENERADA)
        // ------------------------------------------------------------
        private void btnGuardarHombres_Click(object sender, EventArgs e)
            => GuardarRutina("Hombres", rutinaHombres);

        private void btnGuardarMujeres_Click(object sender, EventArgs e)
            => GuardarRutina("Mujeres", rutinaMujeres);

        private void btnGuardarDeportistas_Click(object sender, EventArgs e)
            => GuardarRutina("Deportistas", rutinaDeportistas);

        private void GuardarRutina(string tipoRutina, List<DetalleRutina> detalles)
        {
            try
            {
                if (Sesion.Actual == null)
                    throw new InvalidOperationException("No hay un usuario logueado.");

                if (detalles == null || detalles.Count == 0)
                    throw new InvalidOperationException("No hay ejercicios en la rutina para guardar.");

                string nombreRutina = $"Rutina {tipoRutina} - {DateTime.Now:dd/MM/yyyy}";
                int nuevoIdRutina = _rutinaController.CrearEncabezadoRutina(tipoRutina, Sesion.Actual.IdUsuario, nombreRutina);

                foreach (var detalle in detalles)
                {
                    detalle.IdRutina = nuevoIdRutina;
                    _rutinaController.AgregarDetalle(detalle);
                }

                MessageBox.Show($"Rutina '{tipoRutina}' guardada correctamente con {detalles.Count} ejercicios.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la rutina: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region "Acciones (Limpiar, Editar, Habilitar, etc.)"

        private void btnLimpiarHombres_Click(object sender, EventArgs e)
        {
            if (ConfirmarLimpieza("HOMBRES"))
            {
                dgvHombres.Rows.Clear();
                rutinaHombres.Clear();
                HabilitarAccionesHombres(false);
            }
        }

        private void btnLimpiarMujeres_Click(object sender, EventArgs e)
        {
            if (ConfirmarLimpieza("MUJERES"))
            {
                dgvMujeres.Rows.Clear();
                rutinaMujeres.Clear();
                HabilitarAccionesMujeres(false);
            }
        }

        private void btnLimpiarDeportistas_Click(object sender, EventArgs e)
        {
            if (ConfirmarLimpieza("DEPORTISTAS"))
            {
                dgvDeportistas.Rows.Clear();
                rutinaDeportistas.Clear();
                HabilitarAccionesDeportistas(false);
            }
        }

        private bool ConfirmarLimpieza(string tipo)
        {
            return MessageBox.Show(
                $"¿Seguro querés limpiar la rutina de {tipo}?",
                "Confirmar limpieza",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question) == DialogResult.Yes;
        }

        private void HabilitarAccionesHombres(bool habilitar)
        {
            if (btnEditarHombres != null) btnEditarHombres.Enabled = habilitar;
            if (btnGuardarHombres != null) btnGuardarHombres.Enabled = habilitar;
            if (btnLimpiarHombres != null) btnLimpiarHombres.Enabled = habilitar;
        }

        private void HabilitarAccionesMujeres(bool habilitar)
        {
            if (btnEditarMujeres != null) btnEditarMujeres.Enabled = habilitar;
            if (btnGuardarMujeres != null) btnGuardarMujeres.Enabled = habilitar;
            if (btnLimpiarMujeres != null) btnLimpiarMujeres.Enabled = habilitar;
        }

        private void HabilitarAccionesDeportistas(bool habilitar)
        {
            if (btnEditarDeportistas != null) btnEditarDeportistas.Enabled = habilitar;
            if (btnGuardarDeportistas != null) btnGuardarDeportistas.Enabled = habilitar;
            if (btnLimpiarDeportistas != null) btnLimpiarDeportistas.Enabled = habilitar;
        }

        // Los botones de Editar siguen como placeholder, su lógica es para otra vista
        private void btnEditarHombres_Click(object sender, EventArgs e) => MessageBox.Show("La edición se realiza en el panel de Planillas.");
        private void btnEditarMujeres_Click(object sender, EventArgs e) => MessageBox.Show("La edición se realiza en el panel de Planillas.");
        private void btnEditarDeportistas_Click(object sender, EventArgs e) => MessageBox.Show("La edición se realiza en el panel de Planillas.");

        #endregion
    }
}