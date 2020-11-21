using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minic.Class.Fase_3
{
    public class Arbol
    {
        public Nodo Raiz { get; set; }

        public Arbol()
        {
            Raiz = null;
        }

        public void Insert(Type token)
        {
            if (Raiz == null)
            {
                var tablaS = new TablaS(token);

                Raiz = new Nodo();
                Raiz.Lista_Nodos.Add(tablaS);
            }
            else
            {

            }
        }
    }
}
