using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasSLN.dominio
{
    internal class Ingrediente
    {
        public int ingredienteId { get; set; }
        public string nombre { get; set; }
        public string unidad { get; set; }

        public Ingrediente()
        {
            ingredienteId = 0;
            nombre = "";
            unidad = "";
        }

        public Ingrediente(int IngredienteId,string Nombre,string Unidad)
        {
            this.ingredienteId = IngredienteId;
            this.nombre = Nombre;
            this.unidad = Unidad;
        }
    }
}
