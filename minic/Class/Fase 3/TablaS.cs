using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minic.Class.Fase_3
{
    public class TablaS
    {
        public Type token { get; set; } //datos del token

        public string val { get; set; } //atributo valor

        public string type { get; set; } //atributo tipo

        public string error { get; set; } //Si hay error

        public TablaS(Type tk)
        {
            token = tk;
        }

        public override string ToString()
        {
            return $"{token.description}\t\tline {token.linea} cols {token.column_I}-{token.column_F} type {type} val {val}\n";
        }
    }
}
