using System.Collections.Generic;
using System.Data.SqlClient;
using GymManager.Models;
using GymManager.Utils;

namespace GymManager.Controllers
{
    public class GeneroController
    {
        public List<Genero> ObtenerTodos()
        {
            var lista = new List<Genero>();
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();
                string query = "SELECT id_genero, nombre FROM Genero;";
                using (var cmd = new SqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        lista.Add(new Genero
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