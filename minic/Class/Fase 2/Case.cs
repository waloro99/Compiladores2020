using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minic.Class.Fase_2
{
    public class Case
    {
        public string Match { get; set; }
        public List<string> Actions { get; set; }

        public Case(string match)
        {
            Match = match;
            Actions = new List<string>();
        }
    }
}
