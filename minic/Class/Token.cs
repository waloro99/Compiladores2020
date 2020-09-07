using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minic.Class
{
    public class Token
    {

        //---------------------------------------FUNCTIONS PUBLIC---------------------------------------

        #region FUNCTIONS PUBLIC

        //var that operators and punctuation characters
        public string operators = "+ - * / % < <= > >= = == != && || ! ; , . [ ] ( ) { } [] () {}";

        //var that contains reserved words
        public string sentence = "void int double bool string class const interface null this for while" +
                                 " foreach if else return break New NewArray Console WriteLine Print"; //Add "Print" --> Lab A

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
            List<string> words = new List<string>(); //create list, save the data
            char delimiter = ' '; //delimiter for split words
            string[] values = sentence.Split(delimiter);
            //scroll in the array 
            for (int i = 0; i < values.Length; i++)
                words.Add(values[i]);

            return words; //return the list completed
        }

        //method private that divide the words of the operators
        private List<string> Operators_Method()
        {
            List<string> words = new List<string>(); //create list, save the data
            char delimiter = ' '; //delimiter for split words
            string[] values = operators.Split(delimiter);
            //scroll in the array 
            for (int i = 0; i < values.Length; i++)
                words.Add(values[i]);

            return words; //return the list completed
        }

        #endregion

    }
}
