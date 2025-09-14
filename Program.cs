// Importación de bibliotecas del sistema y del proyecto
using System;                             // Funcionalidades básicas (ej: Console, Exception)
using System.Collections.Generic;         // Estructuras de datos como List<>
using System.Linq;                        // Consultas LINQ (ej: .Where, .Select)
using System.Threading.Tasks;             // Soporte para programación asíncrona (async/await)
using System.Windows.Forms;               // Componentes de Windows Forms (botones, formularios, etc.)
using GymManager.Models;                  // Referencia a las clases del modelo (Usuario, etc.)
using GymManager.Utils;                   // Referencia a clases utilitarias como Sesion

namespace GymManager
{
    // Clase principal del programa
    internal static class Program
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>

        [STAThread] // Indica que se usará un modelo de subprocesamiento compatible con Windows Forms
        static void Main()
        {
            // Aplica estilos visuales modernos (bordes redondeados, etc.)
            Application.EnableVisualStyles();

            // Usa el motor moderno para mostrar texto en los formularios
            Application.SetCompatibleTextRenderingDefault(false);

            // -----------------------
            // ⚠️ SIMULACIÓN DE LOGIN
            // -----------------------
            // Esto sirve para que, durante el desarrollo, se saltee la pantalla de login
            // y se cargue directamente un usuario de prueba (en este caso, un Administrador).
            // Podés comentar esto si querés probar el Login real.

            Sesion.Actual = new Usuario
            {
                Id = 1,                                 // ID del usuario simulado
                Nombre = "MC Bruninho",                 // Nombre ficticio del usuario
                Email = "mc@gmail.com",                 // Email ficticio del usuario
                Rol = Rol.Profesor              // Rol del usuario (Administrador, Profesor, etc.)
            };

            // ----------------------------------------
            // Abre el formulario principal del sistema
            // ----------------------------------------
            // Si querés probar el Login, reemplazá FrmMain por FrmLogin:

             Application.Run(new Forms.FrmLogin());
           // Application.Run(new Forms.FrmMain()); 
        }
    }
}
