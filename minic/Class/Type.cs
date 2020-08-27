using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minic.Class
{
    public class Type : IComparable
    {
        //class type object
        public string cadena { get; set; } //Data
        public int linea { get; set; } //line in the file
        public int column_I { get; set; } //first column
        public int column_F { get; set; } //last column
        public string Error { get; set; } //if exist a error
        public string description{ get; set; }

        public int IndexMatch { get; set; }

        public override string ToString()
        {
            if (Error == "")
                return $"{cadena}\t\tline {linea} cols {column_I}-{column_F} is {description}\n";
            else if (Error != "" && description != "" && description != "Identifier length exceeds 31 characters")
                return $"*** {Error} line {linea}. *** EOF in unfinished '{cadena}'\n";
            else
                return $"*** {Error} line {linea}. {description} '{cadena}'\n";           
        }

        public int CompareTo(object obj)
        {
            var comparer = (Type) obj;
            return IndexMatch.CompareTo(comparer.IndexMatch);
        }

        public static Comparison<Type> OrderByIndex = delegate(Type ty1, Type ty2)
        {
            return ty1.CompareTo(ty2);
        };
    }
}
