using System;

namespace minic.Class.Fase_3
{
    public class Arbol
    {
        public Nodo Raiz { get; set; }

        public Arbol()
        {
            Raiz = null;
        }

        public void Insert(Token token)
        {
            if (Raiz == null)
            {
                var tablaS = new TablaS(token);

                Raiz = new Nodo();
                Raiz.Lista_Nodos.Add(tablaS);
            }
            else
            {
                var tablaS = new TablaS(token);

                Nodo nuevoNodo = new Nodo();
                nuevoNodo.Lista_Nodos.Add(tablaS);
            }
        }


        public void Recorrido(Nodo n, Token x)
        {
            if (n != null)
            {
                foreach (var item in n.Lista_Nodos)
                {
                    if (item.token.description == x.description)
                    {
                        item.val = x.description;
                    }
                }
            }
        }
    }
}
