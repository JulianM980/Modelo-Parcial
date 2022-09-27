using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasSLN.dominio
{
    internal class Receta
    {
        public int recetaNro { get; set; }
        public string nombre { get; set; }
        public string tipoReceta { get; set; }
        public string chef { get; set; }
        public List<DetalleReceta> detalleReceta = new List<DetalleReceta>();

        public Receta()
        {
            recetaNro = 0;
            nombre = "";
            tipoReceta = "";
            chef = "";
            detalleReceta = new List<DetalleReceta>();
        }
        public Receta(int RecetaNro, string Nombre, string TipoReceta,string Chef, List<DetalleReceta> DetalleReceta)
        {
            this.recetaNro = RecetaNro;
            this.nombre = Nombre;
            this.tipoReceta = TipoReceta;
            this.chef = Chef;
            this.detalleReceta = DetalleReceta;
        }

        public void AgregarDetalle(DetalleReceta detalle)
        {
            detalleReceta.Add(detalle);
        }
    }
}
