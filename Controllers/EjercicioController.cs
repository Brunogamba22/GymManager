using GymManager.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymManager.Controllers
{
    public class EjercicioController
    {
        private List<Ejercicio> ejercicios = new List<Ejercicio>();
        private int proximoId = 1;

        public List<Ejercicio> ObtenerTodos()
        {
            return ejercicios.ToList(); // copia
        }

        public void Agregar(Ejercicio e)
        {
            e.Id = proximoId++;
            ejercicios.Add(e);
        }

        public void Editar(Ejercicio e)
        {
            var existente = ejercicios.FirstOrDefault(x => x.Id == e.Id);
            if (existente != null)
            {
                existente.Nombre = e.Nombre;
                existente.Musculo = e.Musculo;
                existente.Descripcion = e.Descripcion;
            }
        }

        public void Eliminar(int id)
        {
            var ejercicio = ejercicios.FirstOrDefault(e => e.Id == id);
            if (ejercicio != null)
                ejercicios.Remove(ejercicio);
        }
    }
}
