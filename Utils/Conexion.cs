using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using System.Configuration;

namespace GymManager.Utils
{
    // Clase estática para acceder a la cadena de conexión desde cualquier parte del proyecto
    public static class Conexion
    {
        // Propiedad de solo lectura que obtiene la cadena de conexión desde App.config
        public static string CadenaConexion =>
            ConfigurationManager.ConnectionStrings["GymDB"].ConnectionString;
    }
}
