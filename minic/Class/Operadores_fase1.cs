using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minic.Class
{
    public class Operadores_fase1
    {
        //---------------------------------------FUNCTIONS PUBLIC---------------------------------------
        #region FUNCTIONS PUBLIC

        //var that operators and punctuation characters
        private string operators = "+ - * / % < <= > >= = == != && || ! ; , . [ ] ( ) { } [] () {}";

        //var that contains reserved words
        private string sentence = "void int double bool string class const interface null this for while foreach if else return break New NewArray Console WriteLine Print";

        //method public that return the token reserved words
        public List<string> Reserved_Words()
        {
            return Reserved_Method();
        }

        //method public that return the token reserved words
        public List<string> Operators_Words()
        {
            return Operators_Method();
        }

        #endregion
        //---------------------------------------FUNCTIONS PRIVATE---------------------------------------

        #region FUNCTIONS PRIVATE

        //method private that divide the words of the sentence
        private List<string> Reserved_Method()
        {
            return sentence.Split(' ').ToList();
        }

        //method private that divide the words of the operators
        private List<string> Operators_Method()
        {
            return operators.Split(' ').ToList();
        }
        #endregion
    }
}
