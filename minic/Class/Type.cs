using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minic.Class
{
    public class Type
    {
        //class type object
        public string cadena { get; set; } //Data
        public int linea { get; set; } //line in the file
        public int column_I { get; set; } //first column
        public int column_F { get; set; } //last column
        public string Error { get; set; } //if exist a error
        public string description{ get; set; }

        public override string ToString()
        {
            return $"{cadena}\t\tline {linea} cols {column_I}-{column_F} is {description} {Error}";
        }
    }
}
