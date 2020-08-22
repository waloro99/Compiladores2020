using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace minic.Class
{
    public class Scanner
    {

        //---------------------------------------FUNCTIONS PUBLIC---------------------------------------

        #region FUNCTIONS PUBLIC

        //List for save data the lexic analysis
        public List<Type> NewFile = new List<Type>();
        public int column = 0;
        public int row = 0;

        //method public for scanner the file
        public List<Type> Scanner_Lexic(string[] file )
        {
            //scrolll the array
            Scanner_Private(file);
            return NewFile; // return the new file for write 
        }


        #endregion
        //---------------------------------------FUNCTIONS PRIVATE---------------------------------------

        #region FUNCTIONS PRIVATE

        //method for analysis the file
        private void Scanner_Private(string[] file)
        {
            //scroll the file
            for (int i = 0; i < file.Length; i++) // i = fila
            {
                //separate the lines
                //separate the words
                string[] word = Regex.Split(file[i]," ");
                row ++; //the line increment 
                Filter_First(word,row);
            }

        }

        //method for first filter, parameters words and line
        private void Filter_First(string[] word, int line)
        {
            //call the tokens
            Token t = new Token();
            List<string> ope = t.Operators_Words();
            string operators = Operator_A(ope);
            //scroll the array word by word
            for (int i = 0; i < word.Length; i++)
            {
                //only letters
                if (Regex.IsMatch(word[i], @"^[a-z A-Z]+$"))
                {
                    //if not is reserved word
                    if (!Is_ReservedWord(word[i],line))
                    {
                        //es un identificador
                    }
                }
                //only numbers --> decimal constant
                else if (Regex.IsMatch(word[i], @"^[0-9]+$"))
                {

                }
                //only operator (1)
                else if (Regex.IsMatch(word[i], @"^["+operators+"]$"))
                {

                }
                //numbers and letters
                else if (Regex.IsMatch(word[i], @"^[0-9 a-z A-Z]+$"))
                {

                }
                //other case
                else
                {

                }
            }
        }


        //method for array the operator / array the reserved word
        private string Operator_A(List<string> o)
        {
            string res = "";
            foreach (var item in o)
                res = res + "|" + item;

            res = res.TrimStart('|');
            return res;
        }

        //method to know if it is a reserved word
        private bool Is_ReservedWord(string w, int line)
        {
            //call the tokens
            Token t = new Token();
            List<string> reser = t.Reserved_Words();
            string reserved = Operator_A(reser);

            if (Regex.IsMatch(w, @"^(" + reserved + ")$"))
            {
                Insert_Word(w,line,"Palabra Reservada");
                return true;
            }
            else
                return false;
        }


        //method for insert in list type
        private void Insert_Word(string word, int line, string type)
        {
            //insertar en la lista de tipo TYPE
        }

        #endregion


    }
}
