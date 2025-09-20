using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

using GymManager.Controllers;
using GymManager.Models;

namespace GymManager.Views
{
    public partial class UcReportes : UserControl
    {
        //  Controladores para acceder a los datos
        private UsuarioController usuarioController = new UsuarioController();
        private EjercicioController ejercicioController = new EjercicioController();

        // Constructor principal del UserControl
        public UcReportes()
        {
            InitializeComponent(); // Carga los componentes visuales
        }

        //  Evento que se ejecuta cuando el control se carga
        private void UcReportes_Load(object sender, EventArgs e)
        {
            // Total de ejercicios cargados en memoria
            int totalEjercicios = ejercicioController.ObtenerTodos().Count;
            lblTotalEjercicios.Text = totalEjercicios.ToString(); // Mostramos el número

            // Lista de usuarios cargada desde el controlador
            var usuarios = usuarioController.ObtenerTodos();

            // Filtramos usuarios según su rol
            int admins = usuarios.Count(u => u.Rol == Rol.Administrador);
            int profes = usuarios.Count(u => u.Rol == Rol.Profesor);
            int receps = usuarios.Count(u => u.Rol == Rol.Recepcionista);

            //  Mostramos los resultados en cada label
            lblAdmins.Text = $"Administradores: {admins}";
            lblProfes.Text = $"Profesores: {profes}";
            lblRecepcionistas.Text = $"Recepcionistas: {receps}";
        }
    }
}
