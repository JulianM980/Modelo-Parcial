using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasSLN.datos
{
    internal class Parametro
    {
        public string nombre { get; set; }
        public int valor { get; set; }

        public Parametro(string Nombre, int Valor)
        {
            this.nombre = Nombre;
            this.valor = Valor;
        }
    }
}
