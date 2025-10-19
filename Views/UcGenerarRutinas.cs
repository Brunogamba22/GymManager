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

        // Referencias a botones
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
            tabPanels = new[] { panelHombres, panelMujeres, panelDeportistas };
            tabLabels = new[] { lblTabHombres, lblTabMujeres, lblTabDeportistas };
            ShowTab(0);

            // Cargar los grupos musculares en las listas con casillas
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
                MessageBox.Show("Error al cargar los grupos musculares: " + ex.Message, "Error de conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

        private void lblTabHombres_Click(object sender, EventArgs e) => ShowTab(0);
        private void lblTabMujeres_Click(object sender, EventArgs e) => ShowTab(1);
        private void lblTabDeportistas_Click(object sender, EventArgs e) => ShowTab(2);
        private void TabLabel_MouseEnter(object sender, EventArgs e) { /* Lógica de hover */ }
        private void TabLabel_MouseLeave(object sender, EventArgs e) { /* Lógica de hover */ }

        #endregion

        // Eventos "Generar" que ahora pasan el CheckedListBox
        private void btnGenerarHombres_Click(object sender, EventArgs e)
            => GenerarRutinaReal("Hombres", dgvHombres, rutinaHombres, chkListHombres);

        private void btnGenerarMujeres_Click(object sender, EventArgs e)
            => GenerarRutinaReal("Mujeres", dgvMujeres, rutinaMujeres, chkListMujeres);

        private void btnGenerarDeportistas_Click(object sender, EventArgs e)
            => GenerarRutinaReal("Deportistas", dgvDeportistas, rutinaDeportistas, chkListDeportistas);

        // Evento que se dispara al marcar/desmarcar una casilla para habilitar/deshabilitar el botón
        private void OnGrupoMuscular_ItemCheck(CheckedListBox chkList, Button btnGenerar)
        {
            this.BeginInvoke((MethodInvoker)delegate {
                btnGenerar.Enabled = chkList.CheckedItems.Count > 0;
            });
        }

        // Lógica de generación MODIFICADA para aceptar múltiples grupos
        private void GenerarRutinaReal(string tipo, DataGridView grilla, List<DetalleRutina> listaRutina, CheckedListBox chkList)
        {
            var gruposSeleccionados = chkList.CheckedItems.Cast<string>().ToList();

            if (gruposSeleccionados.Count == 0)
            {
                MessageBox.Show("Por favor, seleccione al menos un grupo muscular.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                listaRutina.Clear();
                var ejerciciosParaRutina = new List<Ejercicio>();

                int ejerciciosPorGrupo = 3; // Define cuántos ejercicios buscar por cada grupo muscular

                foreach (string grupo in gruposSeleccionados)
                {
                    List<Ejercicio> disponibles = _ejercicioController.ObtenerPorGrupoMuscular(grupo);

                    if (disponibles.Count < ejerciciosPorGrupo)
                    {
                        MessageBox.Show($"No hay suficientes ejercicios de '{grupo}'. La rutina se generará con los {disponibles.Count} disponibles.", "Datos insuficientes", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    Random rnd = new Random();
                    ejerciciosParaRutina.AddRange(disponibles.OrderBy(x => rnd.Next()).Take(ejerciciosPorGrupo));
                }

                if (ejerciciosParaRutina.Count == 0)
                {
                    MessageBox.Show("No se encontraron ejercicios para los grupos musculares seleccionados.", "Sin resultados", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
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

                // Habilitar botones de acción
                if (tipo == "Hombres") HabilitarAccionesHombres(true);
                if (tipo == "Mujeres") HabilitarAccionesMujeres(true);
                if (tipo == "Deportistas") HabilitarAccionesDeportistas(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocurrió un error al generar la rutina: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MostrarRutinaEnGrid(DataGridView grilla, List<DetalleRutina> rutina)
        {
            grilla.Visible = rutina.Count > 0; // La grilla solo se ve si tiene datos
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

        // Eventos "Guardar"
        private void btnGuardarHombres_Click(object sender, EventArgs e) => GuardarRutina("Hombres", rutinaHombres, chkListHombres);
        private void btnGuardarMujeres_Click(object sender, EventArgs e) => GuardarRutina("Mujeres", rutinaMujeres, chkListMujeres);
        private void btnGuardarDeportistas_Click(object sender, EventArgs e) => GuardarRutina("Deportistas", rutinaDeportistas, chkListDeportistas);

        private void GuardarRutina(string tipoRutina, List<DetalleRutina> detalles, CheckedListBox chkList)
        {
            try
            {
                if (Sesion.Actual == null)
                    throw new InvalidOperationException("No hay un usuario logueado.");

                if (detalles == null || detalles.Count == 0)
                    throw new InvalidOperationException("No hay ejercicios en la rutina para guardar.");

                // Buscamos en la lista de géneros el que coincida con el nombre de la pestaña (ej: "Hombres")
                var generoEncontrado = _listaDeGeneros.FirstOrDefault(g => g.Nombre.Equals(tipoRutina, StringComparison.OrdinalIgnoreCase));

                // Si no lo encontramos, usamos 1 como valor por defecto para evitar errores.
                int idGeneroParaGuardar = generoEncontrado?.Id ?? 1;

                string gruposSeleccionados = string.Join(" + ", chkList.CheckedItems.Cast<string>());
                string nombreRutina = $"Rutina {gruposSeleccionados} - {DateTime.Now:dd/MM/yyyy}";

                // Llamamos al controlador con el ID de género correcto
                int nuevoIdRutina = _rutinaController.CrearEncabezadoRutina(tipoRutina, Sesion.Actual.IdUsuario, nombreRutina, idGeneroParaGuardar);

                foreach (var detalle in detalles)
                {
                    detalle.IdRutina = nuevoIdRutina;
                    _rutinaController.AgregarDetalle(detalle);
                }

                MessageBox.Show($"Rutina de '{gruposSeleccionados}' guardada correctamente.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Limpieza automática post-guardado
                if (tipoRutina == "Hombres") LimpiarPanel(dgvHombres, rutinaHombres, chkListHombres);
                if (tipoRutina == "Mujeres") LimpiarPanel(dgvMujeres, rutinaMujeres, chkListMujeres);
                if (tipoRutina == "Deportistas") LimpiarPanel(dgvDeportistas, rutinaDeportistas, chkListDeportistas);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la rutina: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #region "Acciones (Limpiar, Editar, Habilitar, etc.)"

        private void LimpiarPanel(DataGridView grilla, List<DetalleRutina> listaRutina, CheckedListBox chkList)
        {
            grilla.Rows.Clear();
            grilla.Visible = false;
            listaRutina.Clear();

            for (int i = 0; i < chkList.Items.Count; i++)
            {
                chkList.SetItemChecked(i, false);
            }

            if (grilla == dgvHombres) HabilitarAccionesHombres(false);
            if (grilla == dgvMujeres) HabilitarAccionesMujeres(false);
            if (grilla == dgvDeportistas) HabilitarAccionesDeportistas(false);
        }

        private void btnLimpiarHombres_Click(object sender, EventArgs e)
        {
            if (ConfirmarLimpieza("HOMBRES"))
            {
                LimpiarPanel(dgvHombres, rutinaHombres, chkListHombres);
            }
        }
        private void btnLimpiarMujeres_Click(object sender, EventArgs e)
        {
            if (ConfirmarLimpieza("MUJERES"))
            {
                LimpiarPanel(dgvMujeres, rutinaMujeres, chkListMujeres);
            }
        }
        private void btnLimpiarDeportistas_Click(object sender, EventArgs e)
        {
            if (ConfirmarLimpieza("DEPORTISTAS"))
            {
                LimpiarPanel(dgvDeportistas, rutinaDeportistas, chkListDeportistas);
            }
        }

        private bool ConfirmarLimpieza(string tipo) { return MessageBox.Show($"¿Seguro querés limpiar la rutina de {tipo}?", "Confirmar limpieza", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes; }
        private void HabilitarAccionesHombres(bool habilitar) { if (btnEditarHombres != null) btnEditarHombres.Enabled = habilitar; if (btnGuardarHombres != null) btnGuardarHombres.Enabled = habilitar; if (btnLimpiarHombres != null) btnLimpiarHombres.Enabled = habilitar; }
        private void HabilitarAccionesMujeres(bool habilitar) { if (btnEditarMujeres != null) btnEditarMujeres.Enabled = habilitar; if (btnGuardarMujeres != null) btnGuardarMujeres.Enabled = habilitar; if (btnLimpiarMujeres != null) btnLimpiarMujeres.Enabled = habilitar; }
        private void HabilitarAccionesDeportistas(bool habilitar) { if (btnEditarDeportistas != null) btnEditarDeportistas.Enabled = habilitar; if (btnGuardarDeportistas != null) btnGuardarDeportistas.Enabled = habilitar; if (btnLimpiarDeportistas != null) btnLimpiarDeportistas.Enabled = habilitar; }

        private void btnEditarHombres_Click(object sender, EventArgs e) => MessageBox.Show("La edición se realiza en el panel de Planillas.");
        private void btnEditarMujeres_Click(object sender, EventArgs e) => MessageBox.Show("La edición se realiza en el panel de Planillas.");
        private void btnEditarDeportistas_Click(object sender, EventArgs e) => MessageBox.Show("La edición se realiza en el panel de Planillas.");
        #endregion
    }
}