using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using GymManager.Controllers;
using GymManager.Models;
using GymManager.Utils;
using GymManager.Forms; // Necesario para referenciar FrmMain

namespace GymManager.Views
{
    public partial class UcGenerarRutinas : UserControl
    {
        // Colores y arreglos para la UI
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

        // Listas y Controladores
        private readonly List<DetalleRutina> rutinaHombres = new List<DetalleRutina>();
        private readonly List<DetalleRutina> rutinaMujeres = new List<DetalleRutina>();
        private readonly List<DetalleRutina> rutinaDeportistas = new List<DetalleRutina>();
        private List<Genero> _listaDeGeneros = new List<Genero>();

        private readonly EjercicioController _ejercicioController = new EjercicioController();
        private readonly RutinaController _rutinaController = new RutinaController();
        private readonly GrupoMuscularController _grupoMuscularController = new GrupoMuscularController();
        private readonly GeneroController _generoController = new GeneroController();

        // Referencias a botones (el Designer las asigna)
        private Button btnEditarHombres, btnLimpiarHombres, btnGuardarHombres;
        private Button btnEditarMujeres, btnLimpiarMujeres, btnGuardarMujeres;
        private Button btnEditarDeportistas, btnLimpiarDeportistas, btnGuardarDeportistas;

        public UcGenerarRutinas()
        {
            InitializeComponent();
            this.Load += UcGenerarRutinas_Load;
        }

        private void UcGenerarRutinas_Load(object sender, EventArgs e)
        {
            // Es crucial inicializar estos arrays DESPUÉS de InitializeComponent
            tabPanels = new[] { panelHombres, panelMujeres, panelDeportistas };
            tabLabels = new[] { lblTabHombres, lblTabMujeres, lblTabDeportistas };
            ShowTab(0); // Ahora sí se puede llamar

            // Cargar datos en los CheckedListBox
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

        #region "Lógica de UI (Tabs, Estilos, etc.)"

        // Implementación del método StyleButton (debe estar aquí)
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

        // Implementación del método ShowTab (debe estar aquí)
        private void ShowTab(int indiceTab)
        {
            if (tabPanels == null || tabLabels == null) return; // Chequeo extra
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

        // Implementaciones de los handlers de eventos
        private void lblTabHombres_Click(object sender, EventArgs e) => ShowTab(0);
        private void lblTabMujeres_Click(object sender, EventArgs e) => ShowTab(1);
        private void lblTabDeportistas_Click(object sender, EventArgs e) => ShowTab(2);
        private void TabLabel_MouseEnter(object sender, EventArgs e) { /* Lógica de hover si la necesitas */ }
        private void TabLabel_MouseLeave(object sender, EventArgs e) { /* Lógica de hover si la necesitas */ }

        #endregion

        // Eventos "Generar"
        private void btnGenerarHombres_Click(object sender, EventArgs e)
            => GenerarRutinaReal("Hombres", dgvHombres, rutinaHombres, chkListHombres);

        private void btnGenerarMujeres_Click(object sender, EventArgs e)
            => GenerarRutinaReal("Mujeres", dgvMujeres, rutinaMujeres, chkListMujeres);

        private void btnGenerarDeportistas_Click(object sender, EventArgs e)
            => GenerarRutinaReal("Deportistas", dgvDeportistas, rutinaDeportistas, chkListDeportistas);

        // Implementación del handler ItemCheck (debe estar aquí)
        private void OnGrupoMuscular_ItemCheck(CheckedListBox chkList, Button btnGenerar)
        {
            // Usamos BeginInvoke para asegurar que la verificación se haga después de actualizar el estado
            this.BeginInvoke((MethodInvoker)delegate {
                btnGenerar.Enabled = chkList.CheckedItems.Count > 0;
            });
        }


        private void GenerarRutinaReal(string tipo, DataGridView grilla, List<DetalleRutina> listaRutina, CheckedListBox chkList)
        {
            var gruposSeleccionados = chkList.CheckedItems.Cast<string>().ToList();
            if (gruposSeleccionados.Count == 0) { MessageBox.Show("Seleccione al menos un grupo.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

            try
            {
                listaRutina.Clear();
                var ejerciciosParaRutina = new List<Ejercicio>();
                int ejerciciosPorGrupo = 3;

                foreach (string grupo in gruposSeleccionados)
                {
                    List<Ejercicio> disponibles = _ejercicioController.ObtenerPorGrupoMuscular(grupo);
                    if (disponibles.Count < ejerciciosPorGrupo) { MessageBox.Show($"No hay suficientes ejercicios de '{grupo}'. Se usarán {disponibles.Count}.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning); }
                    Random rnd = new Random();
                    ejerciciosParaRutina.AddRange(disponibles.OrderBy(x => rnd.Next()).Take(Math.Min(ejerciciosPorGrupo, disponibles.Count)));
                }
                ejerciciosParaRutina = ejerciciosParaRutina.GroupBy(ej => ej.Id).Select(g => g.First()).ToList(); // Evitar duplicados

                if (ejerciciosParaRutina.Count == 0) { MessageBox.Show("No se encontraron ejercicios.", "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

                foreach (var ejercicio in ejerciciosParaRutina)
                {
                    listaRutina.Add(new DetalleRutina
                    {
                        EjercicioNombre = ejercicio.Nombre,
                        IdEjercicio = ejercicio.Id, // Guardamos el ID
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
            catch (Exception ex) { MessageBox.Show($"Error al generar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void MostrarRutinaEnGrid(DataGridView grilla, List<DetalleRutina> rutina)
        {
            grilla.Visible = rutina.Count > 0;
            grilla.Rows.Clear();
            foreach (var detalle in rutina) { grilla.Rows.Add(detalle.EjercicioNombre, detalle.Series, detalle.Repeticiones, $"{detalle.Descanso} s"); }
        }

        // Eventos "Guardar"
        private void btnGuardarHombres_Click(object sender, EventArgs e) => GuardarRutina("Hombres", rutinaHombres, chkListHombres);
        private void btnGuardarMujeres_Click(object sender, EventArgs e) => GuardarRutina("Mujeres", rutinaMujeres, chkListMujeres);
        private void btnGuardarDeportistas_Click(object sender, EventArgs e) => GuardarRutina("Deportistas", rutinaDeportistas, chkListDeportistas);

        private void GuardarRutina(string tipoRutina, List<DetalleRutina> detalles, CheckedListBox chkList)
        {
            try
            {
                if (Sesion.Actual == null) throw new InvalidOperationException("No logueado.");
                if (detalles == null || detalles.Count == 0) throw new InvalidOperationException("Rutina vacía.");

                var generoEncontrado = _listaDeGeneros.FirstOrDefault(g => g.Nombre.Equals(tipoRutina, StringComparison.OrdinalIgnoreCase));
                int idGeneroParaGuardar = generoEncontrado?.Id ?? 1; // Fallback a ID 1 si no se encuentra

                string gruposSeleccionados = string.Join(" + ", chkList.CheckedItems.Cast<string>());
                string nombreRutina = $"Rutina {gruposSeleccionados} - {DateTime.Now:dd/MM/yyyy}";

                int nuevoIdRutina = _rutinaController.CrearEncabezadoRutina(tipoRutina, Sesion.Actual.IdUsuario, nombreRutina, idGeneroParaGuardar);

                foreach (var detalle in detalles)
                {
                    detalle.IdRutina = nuevoIdRutina;
                    if (detalle.IdEjercicio <= 0) { /* Manejar si falta ID */ }
                    _rutinaController.AgregarDetalle(detalle);
                }
                MessageBox.Show($"Rutina '{gruposSeleccionados}' guardada.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                if (tipoRutina == "Hombres") LimpiarPanel(dgvHombres, rutinaHombres, chkListHombres);
                if (tipoRutina == "Mujeres") LimpiarPanel(dgvMujeres, rutinaMujeres, chkListMujeres);
                if (tipoRutina == "Deportistas") LimpiarPanel(dgvDeportistas, rutinaDeportistas, chkListDeportistas);
            }
            catch (Exception ex) { MessageBox.Show($"Error al guardar: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        #region "Acciones (Limpiar, Editar, Habilitar, etc.)"

        // Implementación de LimpiarPanel (debe estar aquí)
        private void LimpiarPanel(DataGridView grilla, List<DetalleRutina> listaRutina, CheckedListBox chkList)
        {
            grilla.Rows.Clear();
            grilla.Visible = false;
            listaRutina.Clear();
            for (int i = 0; i < chkList.Items.Count; i++) { chkList.SetItemChecked(i, false); }

            // Deshabilitar botones de acción secundarios Y el de generar
            Button btnGenerar = null;
            if (grilla == dgvHombres) { HabilitarAccionesHombres(false); btnGenerar = btnGenerarHombres; }
            if (grilla == dgvMujeres) { HabilitarAccionesMujeres(false); btnGenerar = btnGenerarMujeres; }
            if (grilla == dgvDeportistas) { HabilitarAccionesDeportistas(false); btnGenerar = btnGenerarDeportistas; }
            if (btnGenerar != null) btnGenerar.Enabled = false;
        }

        // Implementaciones de handlers Limpiar
        private void btnLimpiarHombres_Click(object sender, EventArgs e) { if (ConfirmarLimpieza("HOMBRES")) { LimpiarPanel(dgvHombres, rutinaHombres, chkListHombres); } }
        private void btnLimpiarMujeres_Click(object sender, EventArgs e) { if (ConfirmarLimpieza("MUJERES")) { LimpiarPanel(dgvMujeres, rutinaMujeres, chkListMujeres); } }
        private void btnLimpiarDeportistas_Click(object sender, EventArgs e) { if (ConfirmarLimpieza("DEPORTISTAS")) { LimpiarPanel(dgvDeportistas, rutinaDeportistas, chkListDeportistas); } }

        private bool ConfirmarLimpieza(string tipo) { return MessageBox.Show($"¿Limpiar rutina de {tipo}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes; }
        private void HabilitarAccionesHombres(bool habilitar) { if (btnEditarHombres != null) btnEditarHombres.Enabled = habilitar; if (btnGuardarHombres != null) btnGuardarHombres.Enabled = habilitar; if (btnLimpiarHombres != null) btnLimpiarHombres.Enabled = habilitar; }
        private void HabilitarAccionesMujeres(bool habilitar) { if (btnEditarMujeres != null) btnEditarMujeres.Enabled = habilitar; if (btnGuardarMujeres != null) btnGuardarMujeres.Enabled = habilitar; if (btnLimpiarMujeres != null) btnLimpiarMujeres.Enabled = habilitar; }
        private void HabilitarAccionesDeportistas(bool habilitar) { if (btnEditarDeportistas != null) btnEditarDeportistas.Enabled = habilitar; if (btnGuardarDeportistas != null) btnGuardarDeportistas.Enabled = habilitar; if (btnLimpiarDeportistas != null) btnLimpiarDeportistas.Enabled = habilitar; }

        // 🔥 EVENTOS EDITAR MODIFICADOS para llamar a NavegarAEdicion 🔥
        private void btnEditarHombres_Click(object sender, EventArgs e) => NavegarAEdicion(rutinaHombres, "Hombres");
        private void btnEditarMujeres_Click(object sender, EventArgs e) => NavegarAEdicion(rutinaMujeres, "Mujeres");
        private void btnEditarDeportistas_Click(object sender, EventArgs e) => NavegarAEdicion(rutinaDeportistas, "Deportistas");

        // 🔥 MÉTODO PARA NAVEGAR A EDICIÓN (Implementación aquí) 🔥
        private void NavegarAEdicion(List<DetalleRutina> rutinaActual, string tipoRutina)
        {
            if (rutinaActual == null || rutinaActual.Count == 0) { /* Mensaje */ return; }

            // 🔥 AVISO AL USUARIO 🔥
            MessageBox.Show($"La rutina generada para '{tipoRutina}' se abrirá en el panel 'Editar Rutina'.",
                            "Navegando a Edición", MessageBoxButtons.OK, MessageBoxIcon.Information);

            var rutinaParaEditar = new List<DetalleRutina>(rutinaActual.Select(d => new DetalleRutina
            {
                IdEjercicio = d.IdEjercicio,
                EjercicioNombre = d.EjercicioNombre,
                Series = d.Series,
                Repeticiones = d.Repeticiones,
                Descanso = d.Descanso
            }));

            var frmMain = this.ParentForm as FrmMain;
            frmMain?.MostrarPanelEdicion(rutinaParaEditar, tipoRutina);
        }

        #endregion
    }
}