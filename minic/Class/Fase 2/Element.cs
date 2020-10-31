using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace minic.Class.Fase_2
{
    public class Element
    {
        public string Caracter { get; set; }
        public bool Terminal { get; set; }
        public bool Actual { get; set; }

        public Element(string caracter)
        {
            Caracter = caracter;
            Terminal = false;
            Actual = false;
        }
    }
}
