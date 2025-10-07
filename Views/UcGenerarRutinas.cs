using System;                                      // Provee tipos base (EventArgs, DateTime, etc.)
using System.Collections.Generic;                  // Para usar List<T>
using System.Drawing;                              // Para tipos de color y fuentes
using System.Linq;                                 // Para LINQ (GroupBy, FirstOrDefault, etc.)
using System.Windows.Forms;                        // Controles y eventos de Windows Forms
using GymManager.Controllers;                      // Controladores de la capa de datos (Ejercicio, Rutina, Detalle)
using GymManager.Models;                           // Modelos (Usuario, DetalleRutina, etc.)
using GymManager.Utils;                            // Utilidades (Sesion, RutinaSimulador, etc.)

namespace GymManager.Views
{
    public partial class UcGenerarRutinas : UserControl // Partial: se complementa con el .Designer.cs
    {
        // ------------------------------------------------------------
        // 🎨 COLORES (mismos nombres que usa el .Designer.cs)
        // ------------------------------------------------------------
        private Color primaryColor = Color.FromArgb(46, 134, 171);     // Azul principal para botones de Hombres
        private Color secondaryColor = Color.FromArgb(162, 59, 114);   // Rosa/violáceo para botones de Mujeres
        private Color successColor = Color.FromArgb(28, 167, 69);      // Verde para acciones de guardar/éxito
        private Color backgroundColor = Color.FromArgb(248, 249, 250); // Gris muy claro para fondos
        private Color textColor = Color.FromArgb(33, 37, 41);          // Gris oscuro para textos
        private Color borderColor = Color.FromArgb(222, 226, 230);     // Gris claro para bordes de grillas
        private Color tabHoverColor = Color.FromArgb(240, 240, 240);   // Color de hover en tabs
        private Color dangerColor = Color.FromArgb(220, 53, 69);       // Rojo para limpiar/eliminar
        private Color warningColor = Color.FromArgb(255, 193, 7);      // Amarillo para “Editar”

        // ------------------------------------------------------------
        // 📋 ARREGLOS PARA MANEJAR TABS (también los usa el diseñador)
        // ------------------------------------------------------------
        private Panel[] tabPanels;                                     // Array con los paneles de contenido
        private Label[] tabLabels;                                     // Array con las etiquetas (tabs)

        // ------------------------------------------------------------
        // 💪 LISTAS DE RUTINA GENERADAS (simulación)
        // ------------------------------------------------------------
        private List<RutinaSimulador.EjercicioRutina> rutinaHombres = new List<RutinaSimulador.EjercicioRutina>();      // Rutina generada para Hombres
        private List<RutinaSimulador.EjercicioRutina> rutinaMujeres = new List<RutinaSimulador.EjercicioRutina>();      // Rutina generada para Mujeres
        private List<RutinaSimulador.EjercicioRutina> rutinaDeportistas = new List<RutinaSimulador.EjercicioRutina>();  // Rutina generada para Deportistas

        // ------------------------------------------------------------
        // 🔘 REFERENCIAS A BOTONES (que asigna el .Designer dinámicamente)
        //    OJO: el .Designer crea los botones localmente y luego los asigna a estas variables.
        // ------------------------------------------------------------
        private Button btnEditarHombres;             // Botón EDITAR del panel Hombres
        private Button btnLimpiarHombres;            // Botón LIMPIAR del panel Hombres
        private Button btnGuardarHombres;            // Botón GUARDAR del panel Hombres

        private Button btnEditarMujeres;             // Botón EDITAR del panel Mujeres
        private Button btnLimpiarMujeres;            // Botón LIMPIAR del panel Mujeres
        private Button btnGuardarMujeres;            // Botón GUARDAR del panel Mujeres

        private Button btnEditarDeportistas;         // Botón EDITAR del panel Deportistas
        private Button btnLimpiarDeportistas;        // Botón LIMPIAR del panel Deportistas
        private Button btnGuardarDeportistas;        // Botón GUARDAR del panel Deportistas

        // ------------------------------------------------------------
        // 🏁 CONSTRUCTOR
        // ------------------------------------------------------------
        public UcGenerarRutinas()
        {
            InitializeComponent();                   // Inicializa todos los controles definidos en el .Designer.cs

            this.Load += UcGenerarRutinas_Load;      // Suscribo un handler al Load para terminar de configurar todo
        }

        // ------------------------------------------------------------
        // 🔧 AL CARGAR EL CONTROL: completa arrays de tabs y aplica estilos
        // ------------------------------------------------------------
        private void UcGenerarRutinas_Load(object sender, EventArgs e)
        {
            // Aseguramos que los arreglos de tabs conozcan los paneles/labels creados por el diseñador
            tabPanels = new[] { panelHombres, panelMujeres, panelDeportistas }; // Paneles visibles por pestaña
            tabLabels = new[] { lblTabHombres, lblTabMujeres, lblTabDeportistas }; // Etiquetas que actúan como tabs

            ShowTab(0);                               // Por defecto, mostramos la pestaña 0 (Hombres)

            // Aplico estilos a los botones principales “Generar” que el diseñador creó
            if (btnGenerarHombres != null) StyleButton(btnGenerarHombres, primaryColor);     // Botón Generar Hombres
            if (btnGenerarMujeres != null) StyleButton(btnGenerarMujeres, secondaryColor);   // Botón Generar Mujeres
            if (btnGenerarDeportistas != null) StyleButton(btnGenerarDeportistas, successColor);// Botón Generar Deportistas

            // Aplico estilos a los botones de acción (si el diseñador ya los asignó)
            if (btnEditarHombres != null) StyleButton(btnEditarHombres, warningColor);       // Botón Editar Hombres
            if (btnLimpiarHombres != null) StyleButton(btnLimpiarHombres, dangerColor);      // Botón Limpiar Hombres
            if (btnGuardarHombres != null) StyleButton(btnGuardarHombres, successColor);     // Botón Guardar Hombres

            if (btnEditarMujeres != null) StyleButton(btnEditarMujeres, warningColor);       // Botón Editar Mujeres
            if (btnLimpiarMujeres != null) StyleButton(btnLimpiarMujeres, dangerColor);      // Botón Limpiar Mujeres
            if (btnGuardarMujeres != null) StyleButton(btnGuardarMujeres, successColor);     // Botón Guardar Mujeres

            if (btnEditarDeportistas != null) StyleButton(btnEditarDeportistas, warningColor);// Botón Editar Deportistas
            if (btnLimpiarDeportistas != null) StyleButton(btnLimpiarDeportistas, dangerColor);// Botón Limpiar Deportistas
            if (btnGuardarDeportistas != null) StyleButton(btnGuardarDeportistas, successColor);// Botón Guardar Deportistas
        }

        // ------------------------------------------------------------
        // 🎨 APLICA ESTILO A UN BOTÓN (usado por el .Designer.cs)
        // ------------------------------------------------------------
        private void StyleButton(Button boton, Color colorFondo)
        {
            boton.BackColor = colorFondo;                      // Color de fondo
            boton.ForeColor = Color.White;                     // Texto en blanco
            boton.FlatStyle = FlatStyle.Flat;                  // Estilo plano (moderno)
            boton.FlatAppearance.BorderSize = 0;               // Sin borde
            boton.Font = new Font("Segoe UI", 9, FontStyle.Bold); // Tipografía consistente
            boton.Cursor = Cursors.Hand;                       // Cursor de mano al pasar
            boton.Padding = new Padding(12, 6, 12, 6);         // Padding interno para mejor click area
            boton.FlatAppearance.MouseOverBackColor =          // Hover más oscuro
                ControlPaint.Dark(colorFondo, 0.1f);
            boton.FlatAppearance.MouseDownBackColor =          // Click más oscuro
                ControlPaint.Dark(colorFondo, 0.2f);
        }

        // ------------------------------------------------------------
        // 🔁 MUESTRA UNA PESTAÑA Y OCULTA EL RESTO
        // ------------------------------------------------------------
        private void ShowTab(int indiceTab)
        {
            // Oculta todos los paneles primero
            foreach (Panel panel in tabPanels)
                panel.Visible = false;

            // Resetea estilo de todas las etiquetas (tabs)
            foreach (Label etiqueta in tabLabels)
            {
                etiqueta.BackColor = Color.White;              // Fondo blanco por defecto
                etiqueta.ForeColor = textColor;                // Texto oscuro
                etiqueta.Font = new Font("Segoe UI", 10, FontStyle.Regular); // Fuente normal
            }

            // Muestra el panel seleccionado
            tabPanels[indiceTab].Visible = true;               // Hace visible el panel de la pestaña

            // Estiliza la etiqueta activa (tab activo)
            tabLabels[indiceTab].BackColor = GetTabColor(indiceTab); // Fondo con el color del tab
            tabLabels[indiceTab].ForeColor = Color.White;             // Texto blanco
            tabLabels[indiceTab].Font = new Font("Segoe UI", 10, FontStyle.Bold); // Fuente bold
        }

        // ------------------------------------------------------------
        // 🎨 OBTIENE EL COLOR DE FONDO PARA CADA TAB (coincide con el diseñador)
        // ------------------------------------------------------------
        private Color GetTabColor(int indiceTab)
        {
            return indiceTab switch
            {
                0 => primaryColor,         // Hombres
                1 => secondaryColor,       // Mujeres
                2 => successColor,         // Deportistas
                _ => primaryColor          // Default por seguridad
            };
        }

        // ------------------------------------------------------------
        // 🧷 HANDLERS DE CLIC EN LAS TABS (mismos nombres que usa el diseñador)
        // ------------------------------------------------------------
        private void lblTabHombres_Click(object sender, EventArgs e) => ShowTab(0);   // Al hacer clic en “HOMBRES”
        private void lblTabMujeres_Click(object sender, EventArgs e) => ShowTab(1);   // Al hacer clic en “MUJERES”
        private void lblTabDeportistas_Click(object sender, EventArgs e) => ShowTab(2);// Al hacer clic en “DEPORTISTAS”

        // ------------------------------------------------------------
        // 🖱️ EFECTO HOVER PARA TABS (mismos nombres que usa el diseñador)
        // ------------------------------------------------------------
        private void TabLabel_MouseEnter(object sender, EventArgs e)
        {
            Label etiqueta = (Label)sender;                    // Castea el sender a Label
            if (!etiqueta.BackColor.Equals(Color.White))       // Si no es la activa (activa no es blanca)
                return;                                        // No aplicamos hover a la activa
            etiqueta.BackColor = tabHoverColor;                // Fondo de hover
            etiqueta.Cursor = Cursors.Hand;                    // Cursor de mano
        }

        private void TabLabel_MouseLeave(object sender, EventArgs e)
        {
            Label etiqueta = (Label)sender;                    // Castea el sender a Label
            if (!etiqueta.BackColor.Equals(tabHoverColor))     // Si no está en hover, no cambiamos
                return;
            etiqueta.BackColor = Color.White;                  // Vuelve al fondo blanco normal
        }

        // ------------------------------------------------------------
        // ⚙️ GENERAR RUTINAS (usa el simulador)
        // ------------------------------------------------------------
        private void btnGenerarHombres_Click(object sender, EventArgs e)
        {
            rutinaHombres = RutinaSimulador.GenerarRutina("Hombres");   // Genera lista simulada para Hombres
            MostrarRutinaEnGrid(dgvHombres, rutinaHombres);             // Muestra en la grilla del panel
            HabilitarAccionesHombres(true);                             // Habilita Editar/Guardar/Limpiar
            MessageBox.Show($"Rutina HOMBRES generada: {rutinaHombres.Count} ejercicios.", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);      // Mensaje informativo
        }

        private void btnGenerarMujeres_Click(object sender, EventArgs e)
        {
            rutinaMujeres = RutinaSimulador.GenerarRutina("Mujeres");    // Genera lista simulada para Mujeres
            MostrarRutinaEnGrid(dgvMujeres, rutinaMujeres);              // Muestra en grilla
            HabilitarAccionesMujeres(true);                              // Habilita acciones
            MessageBox.Show($"Rutina MUJERES generada: {rutinaMujeres.Count} ejercicios.", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);       // Mensaje informativo
        }

        private void btnGenerarDeportistas_Click(object sender, EventArgs e)
        {
            rutinaDeportistas = RutinaSimulador.GenerarRutina("Deportistas"); // Genera lista simulada para Deportistas
            MostrarRutinaEnGrid(dgvDeportistas, rutinaDeportistas);           // Muestra en grilla
            HabilitarAccionesDeportistas(true);                               // Habilita acciones
            MessageBox.Show($"Rutina DEPORTISTAS generada: {rutinaDeportistas.Count} ejercicios.", "Info",
                MessageBoxButtons.OK, MessageBoxIcon.Information);            // Mensaje informativo
        }

        // ------------------------------------------------------------
        // 🖼️ PINTA LA RUTINA EN LA GRILLA (usa DescansoSegundos)
        // ------------------------------------------------------------
        private void MostrarRutinaEnGrid(DataGridView grilla, List<RutinaSimulador.EjercicioRutina> rutina)
        {
            grilla.Rows.Clear();                                           // Limpia filas existentes
            foreach (var ejercicio in rutina)                              // Recorre cada ejercicio de la lista
            {
                grilla.Rows.Add(                                          // Agrega una fila con columnas en orden
                    ejercicio.Nombre,                                     // Columna: Ejercicio
                    ejercicio.Series,                                     // Columna: Series
                    ejercicio.Repeticiones,                                // Columna: Repeticiones
                    $"{ejercicio.DescansoSegundos} s"                     // Columna: Descanso (segundos) ✅ (no 'Descanso')
                );
            }
        }

        // ------------------------------------------------------------
        // ✏️ BOTONES EDITAR (placeholder: solo mensaje; tu edición real va en panel del Profesor)
        // ------------------------------------------------------------
        private void btnEditarHombres_Click(object sender, EventArgs e)
        {
            if (rutinaHombres.Count == 0)                                 // Valida que haya una rutina generada
            {
                MessageBox.Show("No hay rutina de HOMBRES para editar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);    // Mensaje si no hay datos
                return;                                                   // Sale sin hacer nada
            }
            MessageBox.Show("Rutina de HOMBRES lista para editar (placeholder).", "Edición",
                MessageBoxButtons.OK, MessageBoxIcon.Information);        // Placeholder de edición
        }

        private void btnEditarMujeres_Click(object sender, EventArgs e)
        {
            if (rutinaMujeres.Count == 0)                                 // Verifica lista
            {
                MessageBox.Show("No hay rutina de MUJERES para editar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);    // Mensaje
                return;                                                   // Sale
            }
            MessageBox.Show("Rutina de MUJERES lista para editar (placeholder).", "Edición",
                MessageBoxButtons.OK, MessageBoxIcon.Information);        // Placeholder
        }

        private void btnEditarDeportistas_Click(object sender, EventArgs e)
        {
            if (rutinaDeportistas.Count == 0)                             // Verifica lista
            {
                MessageBox.Show("No hay rutina de DEPORTISTAS para editar.", "Aviso",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);    // Mensaje
                return;                                                   // Sale
            }
            MessageBox.Show("Rutina de DEPORTISTAS lista para editar (placeholder).", "Edición",
                MessageBoxButtons.OK, MessageBoxIcon.Information);        // Placeholder
        }

        // ------------------------------------------------------------
        // 💾 BOTONES GUARDAR → persiste una rutina simulada en la BD
        //    Intentamos mapear por NOMBRE cada ejercicio simulado a uno real
        // ------------------------------------------------------------
        private void btnGuardarHombres_Click(object sender, EventArgs e)   // Handler del botón Guardar HOMBRES
            => GuardarRutina("HOMBRES", rutinaHombres);                    // Llama al método general

        private void btnGuardarMujeres_Click(object sender, EventArgs e)   // Handler del botón Guardar MUJERES
            => GuardarRutina("MUJERES", rutinaMujeres);                    // Llama al método general

        private void btnGuardarDeportistas_Click(object sender, EventArgs e)// Handler del botón Guardar DEPORTISTAS
            => GuardarRutina("DEPORTISTAS", rutinaDeportistas);            // Llama al método general

        // ------------------------------------------------------------
        // 🧹 BOTONES LIMPIAR → vacía grilla y deshabilita acciones
        // ------------------------------------------------------------
        private void btnLimpiarHombres_Click(object sender, EventArgs e)
        {
            if (ConfirmarLimpieza("HOMBRES"))                              // Pide confirmación al usuario
            {
                dgvHombres.Rows.Clear();                                   // Limpia grilla
                rutinaHombres.Clear();                                     // Limpia lista en memoria
                HabilitarAccionesHombres(false);                           // Deshabilita acciones
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

        // ------------------------------------------------------------
        // 🧰 HABILITADORES POR PANEL (evita null si aún no están asignados)
        // ------------------------------------------------------------
        private void HabilitarAccionesHombres(bool habilitar)
        {
            if (btnEditarHombres != null) btnEditarHombres.Enabled = habilitar;   // Habilita/Deshabilita Editar
            if (btnGuardarHombres != null) btnGuardarHombres.Enabled = habilitar; // Habilita/Deshabilita Guardar
            if (btnLimpiarHombres != null) btnLimpiarHombres.Enabled = habilitar; // Habilita/Deshabilita Limpiar
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

        // ------------------------------------------------------------
        // ❓ CONFIRMACIÓN DE LIMPIEZA
        // ------------------------------------------------------------
        private bool ConfirmarLimpieza(string tipo)
        {
            DialogResult respuesta = MessageBox.Show(
                $"¿Seguro querés limpiar la rutina de {tipo}?",           // Mensaje de confirmación
                "Confirmar limpieza",                                     // Título
                MessageBoxButtons.YesNo,                                  // Botones Sí/No
                MessageBoxIcon.Question                                   // Icono de pregunta
            );
            return respuesta == DialogResult.Yes;                         // Devuelve true si confirma
        }

        // ------------------------------------------------------------
        // 🧱 PERSISTENCIA: GUARDA EN BD LO GENERADO EN MEMORIA
        //    Mapea por NOMBRE al Id de la tabla Ejercicios. Si no encuentra, saltea.
        // ------------------------------------------------------------
        private void GuardarRutina(string tipoRutina, List<RutinaSimulador.EjercicioRutina> ejerciciosSimulados)
        {
            try
            {
                // Valida que haya una sesión de usuario para asignar “profesor”
                if (Sesion.Actual == null)                                                 // Si no hay sesión iniciada…
                    throw new InvalidOperationException("No hay usuario logueado en la sesión actual."); // Error claro

                // Valida que haya ejercicios a guardar
                if (ejerciciosSimulados == null || ejerciciosSimulados.Count == 0)         // Si la lista está vacía…
                    throw new InvalidOperationException("No hay ejercicios para guardar."); // Error claro

                // Instancia de controladores de datos (ADO.NET)
                EjercicioController ejercicioController = new EjercicioController();       // Acceso a Ejercicios
                DetalleRutinaController detalleController = new DetalleRutinaController(); // Acceso a DetalleRutina
                RutinaController rutinaController = new RutinaController();                 // Acceso a Rutina

                // Trae todos los ejercicios reales desde BD para mapear por nombre
                List<Ejercicio> ejerciciosBD = ejercicioController.ObtenerTodos();         // Consulta a BD

                // Crea una rutina cabecera y obtiene su Id generado (IDENTITY)
                int idRutina = rutinaController.CrearNuevaRutina(                          // Inserta cabecera Rutina
                    tipoRutina,                                                            // Tipo (HOMBRES/MUJERES/DEPORTISTAS)
                    Sesion.Actual.IdUsuario,                                               // Profesor actual (desde Sesion)
                    $"Rutina {tipoRutina} - {DateTime.Now:dd/MM/yyyy}"                     // Descripción o nombre visible
                );

                int agregados = 0;                                                         // Contador de detalles agregados
                foreach (var ejercicioSim in ejerciciosSimulados)                          // Recorre lista simulada
                {
                    // Busca por nombre del ejercicio, sin importar mayúsculas/minúsculas
                    Ejercicio ejercicioReal = ejerciciosBD                                  // LINQ sobre ejercicios reales
                        .FirstOrDefault(x => string.Equals(x.Nombre,                        // Compara el nombre
                                             ejercicioSim.Nombre,                           // Con el del simulador
                                             StringComparison.OrdinalIgnoreCase));          // Case-insensitive

                    if (ejercicioReal == null)                                             // Si no lo encuentra, lo saltea
                        continue;                                                           // Pasa al siguiente

                    // Construye el detalle a insertar
                    DetalleRutina detalle = new DetalleRutina
                    {
                        IdRutina = idRutina,                                               // FK a Rutina
                        IdEjercicio = ejercicioReal.Id,                                    // Id real del ejercicio en BD
                        Series = ejercicioSim.Series,                                      // Series desde simulador
                        Repeticiones = ParseEnteroSeguro(ejercicioSim.Repeticiones, 12),   // Reps: intenta parsear, default 12
                        Descanso = ejercicioSim.DescansoSegundos,                          // Descanso: segundos
                        Carga = null                                                       // Carga: null por ahora (opcional)
                    };

                    detalleController.Agregar(detalle);                                    // Inserta el detalle en BD
                    agregados++;                                                           // Incrementa contador
                }

                // Muestra resultado al usuario
                MessageBox.Show(
                    $"Rutina '{tipoRutina}' guardada.\nDetalles insertados: {agregados}.", // Mensaje final
                    "Éxito",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information
                );
            }
            catch (Exception ex)                                                           // Manejo de excepciones
            {
                MessageBox.Show($"Error al guardar rutina: {ex.Message}",                  // Muestra error
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        // ------------------------------------------------------------
        // 🔢 PARSEA ENTERO CON DEFAULT SI FALLA
        // ------------------------------------------------------------
        private int ParseEnteroSeguro(string texto, int valorPorDefecto)
        {
            if (int.TryParse(texto, out int numero))                                       // Intenta convertir a entero
                return numero;                                                             // Si sale bien, devuelve el valor
            return valorPorDefecto;                                                        // Si falla, devuelve el default
        }

        // ------------------------------------------------------------
        // ♻️ RESTAURAR RUTINAS GENERADAS
        // ------------------------------------------------------------
        // Este método se invoca desde FrmMain al volver a abrir la vista
        // "Generar Rutinas". Si ya había rutinas generadas en memoria
        // (por ejemplo, Hombres, Mujeres o Deportistas), las vuelve a
        // mostrar en sus DataGridViews correspondientes y re-habilita
        // los botones de acción.
        //
        // Así se evita que el usuario pierda lo generado si navega entre
        // pantallas.
        // ------------------------------------------------------------
        public void RestaurarRutinas()
        {
            // 🔹 RUTINA HOMBRES
            if (rutinaHombres != null && rutinaHombres.Count > 0)
            {
                MostrarRutinaEnGrid(dgvHombres, rutinaHombres); // vuelve a dibujar la tabla
                HabilitarAccionesHombres(true);                 // habilita los botones asociados
            }

            // 🔹 RUTINA MUJERES
            if (rutinaMujeres != null && rutinaMujeres.Count > 0)
            {
                MostrarRutinaEnGrid(dgvMujeres, rutinaMujeres);
                HabilitarAccionesMujeres(true);
            }

            // 🔹 RUTINA DEPORTISTAS
            if (rutinaDeportistas != null && rutinaDeportistas.Count > 0)
            {
                MostrarRutinaEnGrid(dgvDeportistas, rutinaDeportistas);
                HabilitarAccionesDeportistas(true);
            }
        }

    }
}
