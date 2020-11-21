using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minic.Class.Fase_3
{
    public class Nodo
    {
        public Nodo Siguiente { get; set; }
        public List<TablaS> Lista_Nodos { get; set; }

        public Nodo()
        {
            Lista_Nodos = new List<TablaS>();
        }

    }
}
