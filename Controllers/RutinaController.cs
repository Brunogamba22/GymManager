using GymManager.Models;
using GymManager.Utils;
using System;
using System.Data.SqlClient;

namespace GymManager.Controllers
{
    public class RutinaController
    {
        public int CrearEncabezadoRutina(string tipoRutina, int idProfesor, string nombre, int idGenero)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();

                // Tu base de datos no tiene fecha_creacion ni tipo, adapté la consulta a tu esquema
                string query = @"
                    INSERT INTO Rutina (nombre, fecha, creadaPor, id_genero)
                    OUTPUT INSERTED.id_rutina
                    VALUES (@nombre, GETDATE(), @creadaPor, @idGenero);";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@nombre", nombre);
                    cmd.Parameters.AddWithValue("@creadaPor", idProfesor);
                    cmd.Parameters.AddWithValue("@idGenero", idGenero);

                    return (int)cmd.ExecuteScalar(); // Devuelve el ID recién creado
                }
            }
        }

        public void AgregarDetalle(DetalleRutina detalle)
        {
            using (var conn = new SqlConnection(Conexion.Cadena))
            {
                conn.Open();
                string query = @"
                    INSERT INTO DetalleRutina (id_rutina, id_ejercicio, series, repeticiones, carga, descanso)
                    VALUES (@idRutina, @idEjercicio, @series, @repeticiones, @carga, @descanso);";

                using (var cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@idRutina", detalle.IdRutina);
                    cmd.Parameters.AddWithValue("@idEjercicio", detalle.IdEjercicio);
                    cmd.Parameters.AddWithValue("@series", detalle.Series);
                    cmd.Parameters.AddWithValue("@repeticiones", detalle.Repeticiones);
                    cmd.Parameters.AddWithValue("@carga", (object)detalle.Carga ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@descanso", (object)detalle.Descanso ?? DBNull.Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}