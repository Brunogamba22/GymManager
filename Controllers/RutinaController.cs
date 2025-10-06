using System.Data.SqlClient;
using GymManager.Utils;
using GymManager.Models;

namespace GymManager.Controllers
{
    public class RutinaController
    {
        public int CrearNuevaRutina(string tipoRutina, int idProfesor, string nombre)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                string query = @"
                    INSERT INTO Rutina (nombre, tipo, fecha_creacion, id_profesor)
                    OUTPUT INSERTED.id_rutina
                    VALUES (@nombre, @tipo, GETDATE(), @idProfesor);";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@tipo", tipoRutina);
                    cmd.Parameters.AddWithValue("@idProfesor", idProfesor);

                    return (int)cmd.ExecuteScalar(); // Devuelve el ID recién creado
                }
            }
        }
    }
}
