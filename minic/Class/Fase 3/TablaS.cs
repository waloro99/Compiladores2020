using System;

namespace minic.Class.Fase_3
{
    public class TablaS
    {
        public Token token { get; set; } //datos del token

        public string val { get; set; } //atributo valor

        public string type { get; set; } //atributo tipo

        public TablaS(Token tk)
        {
            token = tk;
        }

        public override string ToString()
        {
            return $"token: {token.cadena}\t\t{token.description}\t\tline: {token.linea} type: {type} val: {val}\n";
        }
    }
}
