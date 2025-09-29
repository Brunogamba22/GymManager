using GymManager.Models.Events;
using GymManager.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GymManager.Views
{
    public partial class UcEditarRutina : UserControl
    {
        private Color primaryColor = Color.FromArgb(46, 134, 171);
        private Color backgroundColor = Color.FromArgb(248, 249, 250);
        private Color successColor = Color.FromArgb(40, 167, 69);
        private Color warningColor = Color.FromArgb(255, 193, 7);
        private Color dangerColor = Color.FromArgb(220, 53, 69);

        // 🔥 VARIABLES PARA GUARDAR INFORMACIÓN DE LA RUTINA
        private string _tipoRutinaActual = "";
        private string _nombreRutinaOriginal = "";

        // 🔥 MENÚ CONTEXTUAL PARA EJERCICIOS
        private ContextMenuStrip menuEjercicios;

        public UcEditarRutina()
        {
            InitializeComponent();
            ApplyModernStyles();
            ConfigurarGrid();
            ConfigurarMenuContextual(); // 🔥 CONFIGURAR MENÚ
            // Suscribirse al evento de rutina generada
            EventosRutina.RutinaGeneradaParaEdicion += OnRutinaGeneradaParaEdicion;
        }

        private void ApplyModernStyles()
        {
            this.BackColor = backgroundColor;
            this.Font = new Font("Segoe UI", 9);
        }

        private void ConfigurarGrid()
        {
            // Configuración básica del DataGridView
            dgvRutinas.BackgroundColor = Color.White;
            dgvRutinas.BorderStyle = BorderStyle.None;
            dgvRutinas.EnableHeadersVisualStyles = false;

            // Permitir edición
            dgvRutinas.AllowUserToAddRows = true;
            dgvRutinas.AllowUserToDeleteRows = true;
            dgvRutinas.ReadOnly = false;
            dgvRutinas.AllowUserToResizeColumns = false;
            dgvRutinas.AllowUserToResizeRows = false;
            dgvRutinas.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing;

            // Configuración visual
            dgvRutinas.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dgvRutinas.RowHeadersVisible = false;
            dgvRutinas.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dgvRutinas.RowTemplate.Height = 35;

            // Estilo de encabezados
            dgvRutinas.ColumnHeadersDefaultCellStyle.BackColor = primaryColor;
            dgvRutinas.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dgvRutinas.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            dgvRutinas.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRutinas.ColumnHeadersHeight = 40;

            // Estilo de celdas
            dgvRutinas.DefaultCellStyle.Font = new Font("Segoe UI", 9);
            dgvRutinas.DefaultCellStyle.BackColor = Color.White;
            dgvRutinas.DefaultCellStyle.ForeColor = Color.FromArgb(33, 37, 41);
            dgvRutinas.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            dgvRutinas.DefaultCellStyle.Padding = new Padding(5);

            // Filas alternadas
            dgvRutinas.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(250, 250, 250);
        }

        // 🔥 CONFIGURAR MENÚ CONTEXTUAL PARA EJERCICIOS
        private void ConfigurarMenuContextual()
        {
            menuEjercicios = new ContextMenuStrip();
            menuEjercicios.BackColor = Color.White;
            menuEjercicios.Font = new Font("Segoe UI", 9);
            menuEjercicios.ShowImageMargin = false;

            // Categorías de ejercicios con valores predefinidos
            var categorias = new Dictionary<string, List<(string Nombre, int Series, int Reps, int Descanso)>>
            {
                {
                    "PECHO", new List<(string, int, int, int)>
                    {
                        ("Press banca plano", 4, 8, 90),
                        ("Press banca inclinado", 3, 10, 75),
                        ("Aperturas con mancuernas", 3, 12, 60),
                        ("Fondos en paralelas", 3, 10, 75),
                        ("Cruce de poleas", 3, 12, 60)
                    }
                },
                {
                    "PIERNAS", new List<(string, int, int, int)>
                    {
                        ("Sentadillas", 4, 8, 90),
                        ("Peso muerto", 4, 6, 120),
                        ("Prensa de piernas", 4, 10, 75),
                        ("Zancadas", 3, 10, 75),
                        ("Extensiones de cuadriceps", 3, 12, 60),
                        ("Curl de femoral", 3, 12, 60)
                    }
                },
                {
                    "ESPALDA", new List<(string, int, int, int)>
                    {
                        ("Dominadas", 3, 8, 75),
                        ("Remo con barra", 4, 8, 90),
                        ("Jalón al pecho", 4, 10, 75),
                        ("Remo con mancuerna", 3, 10, 75),
                        ("Peso muerto rumano", 3, 8, 90)
                    }
                },
                {
                    "HOMBROS", new List<(string, int, int, int)>
                    {
                        ("Press militar", 4, 8, 90),
                        ("Elevaciones laterales", 3, 12, 60),
                        ("Elevaciones frontales", 3, 12, 60),
                        ("Pájaros", 3, 12, 60),
                        ("Encogimientos", 4, 10, 60)
                    }
                },
                {
                    "BÍCEPS", new List<(string, int, int, int)>
                    {
                        ("Curl de bíceps con barra", 3, 10, 60),
                        ("Curl de bíceps con mancuernas", 3, 10, 60),
                        ("Curl martillo", 3, 12, 60),
                        ("Curl concentrado", 3, 12, 60)
                    }
                },
                {
                    "TRÍCEPS", new List<(string, int, int, int)>
                    {
                        ("Fondos en banco", 3, 12, 60),
                        ("Extensiones de tríceps", 3, 12, 60),
                        ("Press francés", 3, 10, 75),
                        ("Jalón de tríceps", 3, 12, 60)
                    }
                },
                {
                    "ABDOMINALES", new List<(string, int, int, int)>
                    {
                        ("Plancha abdominal", 3, 30, 30),
                        ("Crunch abdominal", 3, 15, 45),
                        ("Elevaciones de piernas", 3, 12, 45),
                        ("Russian twist", 3, 15, 45),
                        ("Mountain climbers", 3, 20, 45)
                    }
                },
                {
                    "CARDIO", new List<(string, int, int, int)>
                    {
                        ("Burpees", 3, 15, 45),
                        ("Saltos de cuerda", 3, 60, 60),
                        ("Correr en cinta", 1, 20, 0),
                        ("Bicicleta estática", 1, 20, 0)
                    }
                }
            };

            // Crear menú para cada categoría
            foreach (var categoria in categorias)
            {
                ToolStripMenuItem itemCategoria = new ToolStripMenuItem(categoria.Key);
                itemCategoria.Font = new Font("Segoe UI", 9, FontStyle.Bold);
                itemCategoria.ForeColor = primaryColor;

                foreach (var ejercicio in categoria.Value)
                {
                    ToolStripMenuItem itemEjercicio = new ToolStripMenuItem(ejercicio.Nombre);
                    itemEjercicio.Tag = ejercicio; // Guardar datos del ejercicio
                    itemEjercicio.Click += ItemEjercicio_Click;
                    itemCategoria.DropDownItems.Add(itemEjercicio);
                }

                menuEjercicios.Items.Add(itemCategoria);
            }

            // Separador
            menuEjercicios.Items.Add(new ToolStripSeparator());

            // Opción para ejercicio personalizado
            ToolStripMenuItem itemPersonalizado = new ToolStripMenuItem("🎯 EJERCICIO PERSONALIZADO");
            itemPersonalizado.Font = new Font("Segoe UI", 9, FontStyle.Bold);
            itemPersonalizado.ForeColor = successColor;
            itemPersonalizado.Click += (s, e) => AgregarEjercicioPersonalizado();
            menuEjercicios.Items.Add(itemPersonalizado);
        }

        // MANEJADOR DE CLIC EN EJERCICIO DEL MENÚ
        private void ItemEjercicio_Click(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Tag is ValueTuple<string, int, int, int> ejercicioData)
            {
                AgregarEjercicioConValores(ejercicioData.Item1, ejercicioData.Item2, ejercicioData.Item3, ejercicioData.Item4);
            }
        }

        // AGREGAR EJERCICIO CON VALORES PREDEFINIDOS
        private void AgregarEjercicioConValores(string nombre, int series, int repeticiones, int descanso)
        {
            dgvRutinas.Rows.Add(nombre, series.ToString(), repeticiones.ToString(), descanso.ToString());
        }

        //  AGREGAR EJERCICIO PERSONALIZADO
        private void AgregarEjercicioPersonalizado()
        {
            dgvRutinas.Rows.Add("Ejercicio personalizado", "3", "10", "60");

            // Seleccionar la celda del nombre para edición inmediata
            if (dgvRutinas.Rows.Count > 0)
            {
                int lastRow = dgvRutinas.Rows.Count - 1;
                dgvRutinas.CurrentCell = dgvRutinas.Rows[lastRow].Cells[0];
                dgvRutinas.BeginEdit(true);
            }
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

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            if (dgvRutinas.Rows.Count == 0 || (dgvRutinas.Rows.Count == 1 && dgvRutinas.Rows[0].IsNewRow))
            {
                MessageBox.Show("No hay rutinas para guardar.", "Información",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            // Validar datos
            for (int i = 0; i < dgvRutinas.Rows.Count - 1; i++)
            {
                var row = dgvRutinas.Rows[i];

                if (string.IsNullOrWhiteSpace(row.Cells[0].Value?.ToString()))
                {
                    MessageBox.Show($"El ejercicio en la fila {i + 1} está vacío.", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // Validar que series, repeticiones y descanso sean números válidos
                if (!int.TryParse(row.Cells[1].Value?.ToString(), out _) ||
                    !int.TryParse(row.Cells[2].Value?.ToString(), out _) ||
                    !int.TryParse(row.Cells[3].Value?.ToString(), out _))
                {
                    MessageBox.Show($"Los valores en la fila {i + 1} deben ser números válidos.", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            //  GUARDAR LA RUTINA EDITADA Y DISPARAR EVENTO
            GuardarRutinaEditada();

            MessageBox.Show("✅ Rutina guardada con éxito", "Éxito",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //  NUEVO MÉTODO: Guardar rutina editada y disparar evento
        private void GuardarRutinaEditada()
        {
            try
            {
                // Obtener ejercicios del DataGridView
                var ejercicios = new List<RutinaSimulador.EjercicioRutina>();

                for (int i = 0; i < dgvRutinas.Rows.Count - 1; i++) // Excluir la fila nueva
                {
                    var row = dgvRutinas.Rows[i];

                    var ejercicio = new RutinaSimulador.EjercicioRutina
                    {
                        Nombre = row.Cells[0].Value?.ToString() ?? "",
                        Series = int.Parse(row.Cells[1].Value?.ToString() ?? "0"),
                        Repeticiones = int.Parse(row.Cells[2].Value?.ToString() ?? "0"),
                        Descanso = int.Parse(row.Cells[3].Value?.ToString() ?? "0")
                    };

                    ejercicios.Add(ejercicio);
                }

                // Crear nombre único para la rutina editada
                string nombreRutinaEditada = $"{_tipoRutinaActual}.{DateTime.Now:yyyyMMdd_HHmmss}_EDITADA";

                //  DISPARAR EVENTO PARA QUE SE GUARDE EN PLANILLAS
                EventosRutina.DispararRutinaGuardada(
                    nombreRutinaEditada,
                    _tipoRutinaActual,
                    DateTime.Now,
                    ejercicios
                );

                // Actualizar interfaz
                lblTitulo.Text = $"✏️ RUTINA EDITADA - {_tipoRutinaActual}";
                lblDescripcion.Text = $"Rutina editada guardada exitosamente: {nombreRutinaEditada}";
                lblDescripcion.ForeColor = successColor;

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al guardar la rutina: {ex.Message}", "Error",
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // MODIFICADO: Ahora abre el menú contextual
        private void btnAgregarEjercicio_Click(object sender, EventArgs e)
        {
            // Mostrar el menú contextual debajo del botón
            menuEjercicios.Show(btnAgregarEjercicio, new Point(0, btnAgregarEjercicio.Height));
        }

        private void btnEliminarEjercicio_Click(object sender, EventArgs e)
        {
            if (dgvRutinas.SelectedRows.Count > 0)
            {
                foreach (DataGridViewRow row in dgvRutinas.SelectedRows)
                {
                    if (!row.IsNewRow)
                        dgvRutinas.Rows.Remove(row);
                }
            }
            else
            {
                MessageBox.Show("Selecciona un ejercicio para eliminar.", "Información",
                              MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLimpiarTodo_Click(object sender, EventArgs e)
        {
            var result = MessageBox.Show("¿Eliminar todos los ejercicios?", "Confirmar",
                                       MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
                dgvRutinas.Rows.Clear();
        }

        private void OnRutinaGeneradaParaEdicion(object sender, RutinaGeneradaEventArgs e)
        {
            //  GUARDAR INFORMACIÓN DE LA RUTINA ACTUAL
            _tipoRutinaActual = e.TipoRutina;
            _nombreRutinaOriginal = e.NombreRutina;

            // Limpiar grid actual
            dgvRutinas.Rows.Clear();

            // Cargar ejercicios de la rutina generada
            foreach (var ejercicio in e.Ejercicios)
            {
                dgvRutinas.Rows.Add(ejercicio.Nombre, ejercicio.Series, ejercicio.Repeticiones, ejercicio.Descanso);
            }

            // Actualizar interfaz
            lblTitulo.Text = $"✏️ EDITAR RUTINA - {e.TipoRutina}";
            lblDescripcion.Text = $"Editando: {e.NombreRutina} - {e.Ejercicios.Count} ejercicios";
            lblDescripcion.ForeColor = primaryColor;

            MessageBox.Show($"Rutina de {e.TipoRutina} cargada para edición", "Edición",
                          MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        //  NUEVO: Aplicar estilos a los botones después de la inicialización
        private void UcEditarRutina_Load(object sender, EventArgs e)
        {
            StyleButton(btnGuardar, successColor);
            StyleButton(btnAgregarEjercicio, primaryColor);
            StyleButton(btnEliminarEjercicio, dangerColor);
            StyleButton(btnLimpiarTodo, warningColor);
        }
    }
}