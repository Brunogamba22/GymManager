using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq; // Necesario para LINQ (FirstOrDefault)
using System.Windows.Forms;
using GymManager.Controllers; // Necesario para los controladores
using GymManager.Models;
using GymManager.Utils;      // Necesario para Sesion
using GymManager.Forms;      // Necesario para FrmMain

namespace GymManager.Views
{
    public partial class UcEditarRutina : UserControl
    {
        // Colores
        private Color primaryColor = Color.FromArgb(46, 134, 171);
        private Color backgroundColor = Color.FromArgb(248, 249, 250);
        private Color successColor = Color.FromArgb(40, 167, 69);
        private Color warningColor = Color.FromArgb(255, 193, 7);
        private Color dangerColor = Color.FromArgb(220, 53, 69);
        private Color textColor = Color.FromArgb(33, 37, 41); // Añadido para consistencia

        // Variables para guardar la rutina actual
        private List<DetalleRutina> _rutinaActualParaEditar; // Guarda la lista original recibida
        private string _tipoRutinaActual = ""; // Guarda el tipo (Hombres, Mujeres, etc.)

        // Controladores necesarios
        private readonly RutinaController _rutinaController = new RutinaController();
        private readonly GeneroController _generoController = new GeneroController();
        private readonly EjercicioController _ejercicioController = new EjercicioController(); // Para buscar IDs
        private List<Genero> _listaDeGeneros = new List<Genero>();

        // Menú contextual (si lo necesitas para agregar ejercicios)
        private ContextMenuStrip menuEjercicios;

        public UcEditarRutina()
        {
            InitializeComponent(); // Crea los controles del .Designer
            ApplyModernStyles();
            ConfigurarGrid();
            ConfigurarMenuContextual(); // Configura el menú para agregar (opcional)
            
            // Cargar géneros al inicio para usarlos al guardar
            try { _listaDeGeneros = _generoController.ObtenerTodos(); }
            catch (Exception ex) { MessageBox.Show("Error al cargar géneros: " + ex.Message); }
        }

        private void ApplyModernStyles()
        {
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI", 9);
        }

        private void ConfigurarGrid()
        {
            dgvRutinas.BackgroundColor = Color.White;
            dgvRutinas.BorderStyle = BorderStyle.None;
            dgvRutinas.EnableHeadersVisualStyles = false;
            dgvRutinas.AllowUserToAddRows = true; // Permitir agregar filas nuevas
            dgvRutinas.AllowUserToDeleteRows = true;
            dgvRutinas.ReadOnly = false; // La grilla general no es ReadOnly
            dgvRutinas.AllowUserToResizeColumns = false;
            dgvRutinas.AllowUserToResizeRows = false;
            dgvRutinas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dgvRutinas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRutinas.RowHeadersVisible = false;
            dgvRutinas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRutinas.RowTemplate.Height = 35;
            dgvRutinas.ColumnHeadersDefaultCellStyle.BackColor = primaryColor;
            dgvRutinas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRutinas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvRutinas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRutinas.ColumnHeadersHeight = 40;
            dgvRutinas.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvRutinas.DefaultCellStyle.BackColor = Color.White;
            dgvRutinas.DefaultCellStyle.ForeColor = textColor;
            dgvRutinas.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRutinas.DefaultCellStyle.Padding = new Padding(5);
            dgvRutinas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);

             // Hacer columnas específicas editables o no
             // Los nombres ("Ejercicio", "Series", etc.) deben coincidir con los del .Designer
             dgvRutinas.Columns["Ejercicio"].ReadOnly = false; // Permitir cambiar nombre o usar ComboBox
             dgvRutinas.Columns["Series"].ReadOnly = false;
             dgvRutinas.Columns["Repeticiones"].ReadOnly = false;
             dgvRutinas.Columns["Descanso"].ReadOnly = false;
        }

        private void ConfigurarMenuContextual()
        {
            menuEjercicios = new ContextMenuStrip { BackColor = Color.White, Font = new Font("Segoe UI", 9), ShowImageMargin = false };
            // ... (código para llenar el menú con ejercicios si lo necesitas) ...
            
             // Opción para ejercicio personalizado
             ToolStripMenuItem itemPersonalizado = new ToolStripMenuItem("🎯 EJERCICIO PERSONALIZADO")
             {
                 Font = new Font("Segoe UI", 9, FontStyle.Bold), ForeColor = successColor
             };
             itemPersonalizado.Click += (s, e) => AgregarEjercicioPersonalizado();
             menuEjercicios.Items.Add(itemPersonalizado);
        }

        // Métodos para el menú contextual (si lo usas para agregar)
        private void ItemEjercicio_Click(object sender, EventArgs e) {/*...*/}
        private void AgregarEjercicioConValores(string nombre, int series, int repeticiones, int descanso) {/*...*/}
        private void AgregarEjercicioPersonalizado()
        {
             // Agrega una fila nueva con valores por defecto
             dgvRutinas.Rows.Add("", "3", "10", "60"); // Nombre vacío para que el usuario complete

             // Opcional: enfocar la celda del nombre para edición inmediata
             if (dgvRutinas.Rows.Count > 0)
             {
                 int lastRow = dgvRutinas.Rows.Count - (dgvRutinas.AllowUserToAddRows ? 2 : 1); // Ajuste por NewRow
                 if(lastRow >= 0)
                 {
                     dgvRutinas.CurrentCell = dgvRutinas.Rows[lastRow].Cells["Ejercicio"];
                     dgvRutinas.BeginEdit(true);
                 }
             }
        }

        // Método de estilo para botones
        private void StyleButton(Button btn, Color bgColor)
        {
            if (btn == null) return;
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

        // 🔥 MÉTODO PÚBLICO PARA CARGAR LA RUTINA 🔥
        public void CargarRutinaParaEditar(List<DetalleRutina> rutina, string tipoRutina)
        {
            _rutinaActualParaEditar = rutina; // Guarda la lista original por si necesita los IDs
            _tipoRutinaActual = tipoRutina;   // Guarda el tipo ("Hombres", etc.)
            dgvRutinas.Rows.Clear();          // Limpia la grilla
            
            if (_rutinaActualParaEditar == null) return;

            // Llena la grilla con los datos recibidos
            foreach (var detalle in _rutinaActualParaEditar)
            {
                dgvRutinas.Rows.Add(
                    detalle.EjercicioNombre, 
                    detalle.Series, 
                    detalle.Repeticiones, 
                    detalle.Descanso 
                );
            }
            
            // Actualiza los textos de la interfaz
            lblTitulo.Text = $"✏️ EDITAR RUTINA - {tipoRutina}";
            lblDescripcion.Text = $"Modificá series, repeticiones o agregá/quitá ejercicios.";
        }

        // 🔥 BOTÓN GUARDAR CAMBIOS 🔥
        private void btnGuardar_Click(object sender, EventArgs e)
        {
             int rowCount = dgvRutinas.Rows.Count - (dgvRutinas.AllowUserToAddRows ? 1 : 0); // Filas reales
            if (rowCount <= 0)
            {
                MessageBox.Show("No hay ejercicios en la rutina para guardar.", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Validar datos antes de guardar
            for (int i = 0; i < rowCount; i++)
            {
                var row = dgvRutinas.Rows[i];
                if (string.IsNullOrWhiteSpace(row.Cells["Ejercicio"].Value?.ToString())) { /* Mensaje error nombre vacío */ return; }
                if (!int.TryParse(row.Cells["Series"].Value?.ToString(), out _) ||
                    !int.TryParse(row.Cells["Repeticiones"].Value?.ToString(), out _) ||
                    !int.TryParse(row.Cells["Descanso"].Value?.ToString(), out _)) { /* Mensaje error números */ return; }
            }

            // Si las validaciones pasan, guardamos
            GuardarRutinaEditadaEnBD();
        }
        
        // 🔥 MÉTODO PARA GUARDAR EN LA BASE DE DATOS 🔥
        private void GuardarRutinaEditadaEnBD()
        {
            try
            {
                 if (Sesion.Actual == null) throw new InvalidOperationException("No hay un usuario logueado.");

                var rutinaModificada = new List<DetalleRutina>();
                int rowCount = dgvRutinas.Rows.Count - (dgvRutinas.AllowUserToAddRows ? 1 : 0);

                for(int i = 0; i < rowCount; i++)
                {
                    var row = dgvRutinas.Rows[i];
                    string nombreEjercicio = row.Cells["Ejercicio"].Value?.ToString();
                    if (string.IsNullOrEmpty(nombreEjercicio)) continue; 

                    // Buscar el ID del ejercicio por nombre en la base de datos
                    var ejercicioDb = _ejercicioController.ObtenerPorNombre(nombreEjercicio);
                    int idEjercicioReal = ejercicioDb?.Id ?? 0;

                    if (idEjercicioReal == 0)
                    {
                         MessageBox.Show($"El ejercicio '{nombreEjercicio}' no existe en la base de datos. Por favor, verifica el nombre o agrégalo.", "Ejercicio no encontrado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                         return; // Detener guardado
                    }

                    rutinaModificada.Add(new DetalleRutina
                    {
                        IdEjercicio = idEjercicioReal, // Usamos el ID real de la BD
                        EjercicioNombre = nombreEjercicio, // Guardamos nombre por si acaso
                        Series = Convert.ToInt32(row.Cells["Series"].Value),
                        Repeticiones = Convert.ToInt32(row.Cells["Repeticiones"].Value),
                        Descanso = Convert.ToInt32(row.Cells["Descanso"].Value)
                    });
                }

                if (rutinaModificada.Count == 0) { /* Mensaje rutina vacía */ return; }

                 var generoEncontrado = _listaDeGeneros.FirstOrDefault(g => g.Nombre.Equals(_tipoRutinaActual, StringComparison.OrdinalIgnoreCase));
                 int idGeneroParaGuardar = generoEncontrado?.Id ?? 1; // ID 1 como fallback

                string nombreRutina = $"Rutina Editada {_tipoRutinaActual} - {DateTime.Now:dd/MM/yyyy}";
                int nuevoIdRutina = _rutinaController.CrearEncabezadoRutina(_tipoRutinaActual, Sesion.Actual.IdUsuario, nombreRutina, idGeneroParaGuardar);

                foreach (var detalle in rutinaModificada)
                {
                    detalle.IdRutina = nuevoIdRutina;
                    _rutinaController.AgregarDetalle(detalle);
                }

                MessageBox.Show("Rutina modificada guardada con éxito.", "Éxito", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                dgvRutinas.Rows.Clear(); // Limpiar la grilla
                _rutinaActualParaEditar = null; // Limpiar la variable interna
                var frmMain = this.ParentForm as FrmMain;
                frmMain?.MostrarPlanillas(); // Navegar a planillas después de guardar
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la rutina: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // Handlers de eventos para los botones del designer
        private void btnAgregarEjercicio_Click(object sender, EventArgs e) { menuEjercicios?.Show(btnAgregarEjercicio, new Point(0, btnAgregarEjercicio.Height)); }
        private void btnEliminarEjercicio_Click(object sender, EventArgs e) 
        {
            if (dgvRutinas.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvRutinas.SelectedRows)
                {
                    if (!row.IsNewRow) dgvRutinas.Rows.Remove(row);
                }
            } else { /* Mensaje: seleccionar fila */ }
        }
        private void btnLimpiarTodo_Click(object sender, EventArgs e) 
        {
             if(MessageBox.Show("...", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                dgvRutinas.Rows.Clear();
        }

        private void UcEditarRutina_Load(object sender, EventArgs e)
        {
            // Aplicar estilos a los botones
            StyleButton(btnGuardar, successColor);
            StyleButton(btnAgregarEjercicio, primaryColor);
            StyleButton(btnEliminarEjercicio, dangerColor);
            StyleButton(btnLimpiarTodo, warningColor);
        }
    }
}