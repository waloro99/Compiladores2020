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
        public List<string> NewFile = new List<string>();
        public int column = 0;
        public int row = 0;

        //method public for scanner the file
        public List<string> Scanner_Lexic(string[] file )
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
            //scroll the array character by character
            for (int i = 0; i < word.Length; i++)
            {
                //type
                if (Regex.IsMatch(word[i], @""))
                {

                }
            }

        }

        #endregion


    }
}
