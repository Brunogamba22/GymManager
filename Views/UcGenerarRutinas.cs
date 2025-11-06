using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GymManager.Controllers;
using GymManager.Models;
using GymManager.Utils;
using GymManager.Forms;

namespace GymManager.Views
{
    public partial class UcGenerarRutinas : UserControl
    {
        // Colores
        private Color primaryColor = Color.FromArgb(46, 134, 171);
        private Color secondaryColor = Color.FromArgb(162, 59, 114);
        private Color successColor = Color.FromArgb(28, 167, 69);
        private Color dangerColor = Color.FromArgb(220, 53, 69);
        private Color warningColor = Color.FromArgb(255, 193, 7);
        private Color textColor = Color.FromArgb(33, 37, 41);
        private Color borderColor = Color.FromArgb(222, 226, 230);
        private Color tabHoverColor = Color.FromArgb(240, 240, 240);
        private Color backgroundColor = Color.FromArgb(248, 249, 250);
        private Panel[] tabPanels;
        private Label[] tabLabels;

        // Listas de rutinas
        public List<DetalleRutina> rutinaHombres = new List<DetalleRutina>();
        public List<DetalleRutina> rutinaMujeres = new List<DetalleRutina>();
        public List<DetalleRutina> rutinaDeportistas = new List<DetalleRutina>();

        // Controladores
        private List<Genero> _listaDeGeneros = new List<Genero>();
        private readonly EjercicioController _ejercicioController = new EjercicioController();
        private readonly RutinaController _rutinaController = new RutinaController();
        private readonly GrupoMuscularController _grupoMuscularController = new GrupoMuscularController();
        private readonly GeneroController _generoController = new GeneroController();

        public UcGenerarRutinas()
        {
            InitializeComponent();
            this.Load += UcGenerarRutinas_Load;
        }

        private void UcGenerarRutinas_Load(object sender, EventArgs e)
        {
            tabPanels = new[] { panelHombres, panelMujeres, panelDeportistas };
            tabLabels = new[] { lblTabHombres, lblTabMujeres, lblTabDeportistas };

            ShowTab(0);
            ConfigurarObjetivos();

            try
            {
                var gruposMusculares = _grupoMuscularController.ObtenerTodos();
                var nombresGrupos = gruposMusculares.Select(g => g.Nombre).ToArray();
                chkListHombres.Items.AddRange(nombresGrupos);
                chkListMujeres.Items.AddRange(nombresGrupos);
                chkListDeportistas.Items.AddRange(nombresGrupos);

                _listaDeGeneros = _generoController.ObtenerTodos();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al cargar datos iniciales: " + ex.Message, "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ConfigurarObjetivos()
        {
            var objetivos = new[] { "Hipertrofia", "Fuerza", "Resistencia" };
            if (cmbObjetivoHombres != null) { cmbObjetivoHombres.Items.AddRange(objetivos); cmbObjetivoHombres.SelectedIndex = 0; }
            if (cmbObjetivoMujeres != null) { cmbObjetivoMujeres.Items.AddRange(objetivos); cmbObjetivoMujeres.SelectedIndex = 0; }
            if (cmbObjetivoDeportistas != null) { cmbObjetivoDeportistas.Items.AddRange(objetivos); cmbObjetivoDeportistas.SelectedIndex = 0; }
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
            if (tabPanels == null || tabLabels == null) return;
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

        private void lblTabHombres_Click(object sender, EventArgs e) => ShowTab(0);
        private void lblTabMujeres_Click(object sender, EventArgs e) => ShowTab(1);
        private void lblTabDeportistas_Click(object sender, EventArgs e) => ShowTab(2);
        private void TabLabel_MouseEnter(object sender, EventArgs e) { /* ... */ }
        private void TabLabel_MouseLeave(object sender, EventArgs e) { /* ... */ }

        #endregion

        // Eventos "Generar"
        private void btnGenerarHombres_Click(object sender, EventArgs e)
            => GenerarRutinaReal("Hombres", dgvHombres, rutinaHombres, chkListHombres, cmbObjetivoHombres);

        private void btnGenerarMujeres_Click(object sender, EventArgs e)
            => GenerarRutinaReal("Mujeres", dgvMujeres, rutinaMujeres, chkListMujeres, cmbObjetivoMujeres);

        private void btnGenerarDeportistas_Click(object sender, EventArgs e)
            => GenerarRutinaReal("Deportistas", dgvDeportistas, rutinaDeportistas, chkListDeportistas, cmbObjetivoDeportistas);

        private void OnGrupoMuscular_ItemCheck(CheckedListBox chkList, Button btnGenerar)
        {
            this.BeginInvoke((MethodInvoker)delegate {
                btnGenerar.Enabled = chkList.CheckedItems.Count > 0;
            });
        }

        //LÓGICA DE GENERACIÓN "INTELIGENTE" 
        private void GenerarRutinaReal(string tipo, DataGridView grilla, List<DetalleRutina> listaRutina, CheckedListBox chkList, ComboBox cmbObjetivo)
        {
            var gruposSeleccionados = chkList.CheckedItems.Cast<string>().ToList();
            if (gruposSeleccionados.Count == 0) { MessageBox.Show("Seleccione al menos un grupo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            string objetivo = cmbObjetivo?.SelectedItem?.ToString() ?? "Hipertrofia";

            int series; int repeticiones;
            switch (objetivo)
            {
                case "Fuerza": series = 5; repeticiones = 5; break;
                case "Resistencia": series = 3; repeticiones = 15; break;
                case "Hipertrofia": default: series = 4; repeticiones = 10; break;
            }

            try
            {
                listaRutina.Clear();
                var ejerciciosParaRutina = new List<Ejercicio>();

                foreach (string grupo in gruposSeleccionados)
                {
                    List<Ejercicio> disponibles = _ejercicioController.ObtenerPorGrupoMuscular(grupo)
                                                      .OrderBy(ej => ej.Nombre)
                                                      .ToList();
                    if (disponibles.Count == 0) { MessageBox.Show($"No se encontraron ejercicios para '{grupo}'.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                    ejerciciosParaRutina.AddRange(disponibles);
                }

                ejerciciosParaRutina = ejerciciosParaRutina.GroupBy(ej => ej.Id).Select(g => g.First()).ToList();
                if (ejerciciosParaRutina.Count == 0) { MessageBox.Show("No se encontraron ejercicios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

                foreach (var ejercicio in ejerciciosParaRutina)
                {
                    listaRutina.Add(new DetalleRutina
                    {
                        EjercicioNombre = ejercicio.Nombre,
                        IdEjercicio = ejercicio.Id,
                        Series = series,
                        Repeticiones = repeticiones,
                        Carga = null
                    });
                }

                MostrarRutinaEnGrid(grilla, listaRutina);

                if (tipo == "Hombres") HabilitarAccionesHombres(true);
                if (tipo == "Mujeres") HabilitarAccionesMujeres(true);
                if (tipo == "Deportistas") HabilitarAccionesDeportistas(true);
            }
            catch (Exception ex) { MessageBox.Show($"Error al generar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void MostrarRutinaEnGrid(DataGridView grilla, List<DetalleRutina> rutina)
        {
            grilla.Visible = rutina.Count > 0;
            grilla.Rows.Clear();
            foreach (var detalle in rutina) { grilla.Rows.Add(detalle.EjercicioNombre, detalle.Series, detalle.Repeticiones, detalle.Carga?.ToString() ?? ""); }
        }

        // Eventos "Guardar"
        private void btnGuardarHombres_Click(object sender, EventArgs e)
            => GuardarRutina("Hombres", rutinaHombres, chkListHombres, cmbObjetivoHombres);

        private void btnGuardarMujeres_Click(object sender, EventArgs e)
            => GuardarRutina("Mujeres", rutinaMujeres, chkListMujeres, cmbObjetivoMujeres);

        private void btnGuardarDeportistas_Click(object sender, EventArgs e)
            => GuardarRutina("Deportistas", rutinaDeportistas, chkListDeportistas, cmbObjetivoDeportistas);

        private void GuardarRutina(string tipoRutina, List<DetalleRutina> detalles, CheckedListBox chkList, ComboBox cmbObjetivo)
        {
            try
            {
                if (Sesion.Actual == null) throw new InvalidOperationException("No logueado.");
                if (detalles == null || detalles.Count == 0) throw new InvalidOperationException("Rutina vacía.");

                string nombreGenero = tipoRutina switch
                {
                    "Hombres" => "Masculino",
                    "Mujeres" => "Femenino",
                    "Deportistas" => "Deportistas",
                    _ => "Masculino"
                };
                var generoEncontrado = _listaDeGeneros.FirstOrDefault(g => g.Nombre.Equals(nombreGenero, StringComparison.OrdinalIgnoreCase));
                if (generoEncontrado == null) throw new Exception($"No se encontró el género '{nombreGenero}' en la BD.");
                int idGeneroParaGuardar = generoEncontrado.Id;

                string gruposSeleccionados = string.Join(" + ", chkList.CheckedItems.Cast<string>());
                string nombreRutina = $"Rutina {gruposSeleccionados} - {DateTime.Now:dd/MM/yyyy}";

                int nuevoIdRutina = _rutinaController.CrearEncabezadoRutina(tipoRutina, Sesion.Actual.IdUsuario, nombreRutina, idGeneroParaGuardar, false);

                foreach (var detalle in detalles)
                {
                    detalle.IdRutina = nuevoIdRutina;
                    _rutinaController.AgregarDetalle(detalle);
                }
                MessageBox.Show($"Rutina '{gruposSeleccionados}' guardada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (tipoRutina == "Hombres") LimpiarPanel(dgvHombres, rutinaHombres, chkListHombres, cmbObjetivoHombres, btnGenerarHombres);
                if (tipoRutina == "Mujeres") LimpiarPanel(dgvMujeres, rutinaMujeres, chkListMujeres, cmbObjetivoMujeres, btnGenerarMujeres);
                if (tipoRutina == "Deportistas") LimpiarPanel(dgvDeportistas, rutinaDeportistas, chkListDeportistas, cmbObjetivoDeportistas, btnGenerarDeportistas);
            }
            catch (Exception ex) { MessageBox.Show($"Error al guardar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        #region "Acciones (Limpiar, Editar, Habilitar, etc.)"

        public void LimpiarRutinaGenerada(string tipoRutina)
        {
            if (tipoRutina.Equals("Hombres", StringComparison.OrdinalIgnoreCase))
            {
                LimpiarPanel(dgvHombres, rutinaHombres, chkListHombres, cmbObjetivoHombres, btnGenerarHombres);
            }
            if (tipoRutina.Equals("Mujeres", StringComparison.OrdinalIgnoreCase))
            {
                LimpiarPanel(dgvMujeres, rutinaMujeres, chkListMujeres, cmbObjetivoMujeres, btnGenerarMujeres);
            }
            if (tipoRutina.Equals("Deportistas", StringComparison.OrdinalIgnoreCase))
            {
                LimpiarPanel(dgvDeportistas, rutinaDeportistas, chkListDeportistas, cmbObjetivoDeportistas, btnGenerarDeportistas);
            }
        }

        private void LimpiarPanel(DataGridView grilla, List<DetalleRutina> listaRutina, CheckedListBox chkList, ComboBox cmbObjetivo, Button btnGenerar)
        {
            grilla.Rows.Clear();
            grilla.Visible = false;
            listaRutina.Clear();

            for (int i = 0; i < chkList.Items.Count; i++)
            {
                chkList.SetItemChecked(i, false);
            }

            if (cmbObjetivo != null) cmbObjetivo.SelectedIndex = 0;

            if (btnGenerar != null) btnGenerar.Enabled = false;
            if (grilla == dgvHombres) HabilitarAccionesHombres(false);
            if (grilla == dgvMujeres) HabilitarAccionesMujeres(false);
            if (grilla == dgvDeportistas) HabilitarAccionesDeportistas(false);
        }

        private void btnLimpiarHombres_Click(object sender, EventArgs e) { if (ConfirmarLimpieza("HOMBRES")) { LimpiarPanel(dgvHombres, rutinaHombres, chkListHombres, cmbObjetivoHombres, btnGenerarHombres); } }
        private void btnLimpiarMujeres_Click(object sender, EventArgs e) { if (ConfirmarLimpieza("MUJERES")) { LimpiarPanel(dgvMujeres, rutinaMujeres, chkListMujeres, cmbObjetivoMujeres, btnGenerarMujeres); } }
        private void btnLimpiarDeportistas_Click(object sender, EventArgs e) { if (ConfirmarLimpieza("DEPORTISTAS")) { LimpiarPanel(dgvDeportistas, rutinaDeportistas, chkListDeportistas, cmbObjetivoDeportistas, btnGenerarDeportistas); } }

        private bool ConfirmarLimpieza(string tipo) { return MessageBox.Show($"¿Limpiar rutina de {tipo}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes; }

        private void HabilitarAccionesHombres(bool habilitar) { if (btnGuardarHombres != null) btnGuardarHombres.Enabled = habilitar; if (btnLimpiarHombres != null) btnLimpiarHombres.Enabled = habilitar; }
        private void HabilitarAccionesMujeres(bool habilitar) { if (btnGuardarMujeres != null) btnGuardarMujeres.Enabled = habilitar; if (btnLimpiarMujeres != null) btnLimpiarMujeres.Enabled = habilitar; }
        private void HabilitarAccionesDeportistas(bool habilitar) { if (btnGuardarDeportistas != null) btnGuardarDeportistas.Enabled = habilitar; if (btnLimpiarDeportistas != null) btnLimpiarDeportistas.Enabled = habilitar; }

        #endregion
    }
}