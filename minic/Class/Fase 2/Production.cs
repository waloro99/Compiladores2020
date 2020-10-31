using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace minic.Class.Fase_2
{
    public class Production
    {
        //Para gramática base
        public int Index { get; set; }
        public string Padre { get; set; } //Estado o padre de una producción
        public List<Element> Lista_elementos { get; set; }

        //Para las producciones canónicas
        public int Num_estado { get; set; }
        public int Emisor { get; set; } //Desde que estado se evalúa
        public string Caracter_analizar { get; set; }
        public int Num_estado_nuevo { get; set; }
        public string Lookahead { get; set; }
        public bool Aceptacion { get; set; }

        //constructor de la gramática base
        public Production(int index, string padre)
        {
            Index = index;
            Padre = padre;
            Lista_elementos = new List<Element>();
        }

        //Constructor para las producciones canónicas
        public Production()
        {
            Index = 0;
            Padre = string.Empty;
            Lista_elementos = new List<Element>();
            Num_estado = 0;
            Emisor = 0;
            Caracter_analizar = string.Empty;
            Num_estado_nuevo = 0;
            Lookahead = string.Empty;
            Aceptacion = false;
        }
    }
}
