using System.Collections.Generic;
using System.Data.SqlClient;
using GymManager.Utils;
using GymManager.Models;

namespace GymManager.Controllers
{
    public class GrupoMuscularController
    {
        public List<GrupoMuscular> ObtenerTodos()
        {
            var lista = new List<GrupoMuscular>();
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();
                string query = "SELECT id_grupo_muscular, nombre FROM Grupo_Muscular ORDER BY nombre;";
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new GrupoMuscular
                        {
                            Id = reader.GetInt32(0),
                            Nombre = reader.GetString(1)
                        });
                    }
                }
            }
            return lista;
        }
    }
}