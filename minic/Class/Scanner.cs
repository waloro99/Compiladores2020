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
        Token t = new Token(); //call the tokens
        public int column = 1;
        public int row = 0;

        //method public for scanner the file
        public List<Type> Scanner_Lexic(string[] file)
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
                string[] word = Regex.Split(file[i], " ");
                row++; //the line increment 
                //check if it is not comment or string
                string type_line = Is_Line(file[i]);
                if (type_line != "")
                    Scanner_Line(file[i], row, type_line); //filter the commentary and string, string the line complete
                else
                    Filter_First(word, row); //first filter for word by word, array the word          
                column = 1; //restart
            }

        }

        //method for first filter, parameters words and line
        private void Filter_First(string[] word, int line)
        {
            //call the tokens
            List<string> ope = t.Operators_Words();
            string operators = Operator_A(ope);
            //scroll the array word by word
            for (int i = 0; i < word.Length; i++)
            {
                //only letters
                if (Regex.IsMatch(word[i], @"^[a-z A-Z]+$"))
                {
                    //if not is reserved word
                    if (!Is_ReservedWord(word[i], line))
                    {
                        //if is a constant boolean
                        if (Regex.IsMatch(word[i], @"^(true|false)$"))
                        {
                            //is a bool
                            Insert_Word(word[i], line, "Bool: " + word[i]);
                        }
                        else
                        {
                            //is a identifier
                            Insert_Word(word[i], line, "Identifier");
                        }
                    }
                }
                
                //only numbers --> decimal constant
                else if (Regex.IsMatch(word[i], @"^[0-9]+$"))
                {
                    Insert_Word(word[i], line, "Value Int: " + word[i]);
                }
                //only operator (1)
                else if (Regex.IsMatch(word[i], @"^[" + operators + "]$")) //----------------> posible esta mala verificar-
                {
                    Insert_Word(word[i], line, "Operator: " + word[i]);
                }
                //numbers and letters
                else if (Regex.IsMatch(word[i], @"^[0-9 a-z A-Z]+$"))
                {
                    //hexadecimal constant
                    if (Regex.IsMatch(word[i], @"^(0x|0X)[0-9]+[a-zA-Z]*$"))
                    {
                        Insert_Word(word[i], line, "Value Hexadecimal: " + word[i]);
                    }
                }
                //other case
                else
                {
                    Second_Filter(word[i], line);
                }
            }
        }

        //method for Second Filter, only words
        private void Second_Filter(string word, int line)
        {
            //empty
        }

        //method for scanner line, because exist commentary or string or both
        private void Scanner_Line(string word, int line, string type) //type-->string or commentary
        {
            //depend the case 
            switch (type)
            {
                case "string":
                    //check that the string has the correct format
                    string flag_strig = Is_CorrectString(word, line); // --> bad method
                    break;
                case "commentary":
                    //check that the commentary has the correct format
                    char[] Word_A = word.ToArray();
                    string before_Comment = "";
                    int i = 0;
                    while (Word_A[i] != '/' && Word_A[i + 1] != '/')
                    {
                        before_Comment = before_Comment + Word_A[i];
                        i++;
                    }
                    //call the method for analisys the string before
                    //insert 
                    Type newType = new Type();
                    newType.cadena = "//...";
                    newType.linea = line;
                    newType.Error = ""; //dont exist error
                    newType.column_I = i+1; //initial in zero
                    newType.column_F = Word_A.Length - i; //all the line after the '//'
                    NewFile.Add(newType);
                    break;
                case "commentary2":
                    break;
                default:
                    break;
            }
        }

        //method for array the operator / array the reserved word
        private string Operator_A(List<string> o)
        {
            string res = string.Empty;
            foreach (var item in o)
                res = res + "|" + item;

            return res.TrimStart('|');
        }

        //method to know if it is a reserved word
        private bool Is_ReservedWord(string w, int line)
        {
            //call the tokens
            List<string> reser = t.Reserved_Words();
            string reserved = Operator_A(reser);

            if (Regex.IsMatch(w, @"^(" + reserved + ")$"))
            {
                Insert_Word(w, line, "T_" + w);
                return true;
            }
            else
                return false;
        }

        //method to know if it is a string constant
        private bool Is_String(string w, int line)
        {
            if (Regex.IsMatch(w, @"^")) //-----------------------------> Falta hacer expresion regular
            {
                Insert_Word(w, line, "Constant String");
                return true;
            }
            else
                return false;
        }

        //method for insert in list type
        private void Insert_Word(string word, int line, string type)
        {
            //insert into TYPE list
            Type newType = new Type();
            newType.cadena = word;
            newType.linea = line;
            newType.Error = ""; //dont exist error
            newType.column_I = column;
            column = column + (word.Length - 1);
            newType.column_F = column;
            column = column + 2; //space + next character
            NewFile.Add(newType);

        }

        //method to know if it is necessary to analyze by line
        private string Is_Line(string line)
        {
            char[] array_line = line.ToArray(); //line character by character
            //scroll the array
            for (int i = 0; i < array_line.Length; i++)
            {
                //if is a string
                if (array_line[i] == '"')
                    return "string";
                //if is a line commentary
                else if (array_line[i] == '/' && array_line[i + 1] == '/')
                    return "commentary";
                //if is a begin commentary
                else if (array_line[i] == '/' && array_line[i + 1] == '*')
                    return "commentary2";
            }

            return "";
        }

        //method for check that the string has the correct format
        private string Is_CorrectString(string line, int linea) //characters
        {
            string I_string = ""; //before the string
            string F_string = ""; //after the string
            string N_string = "";
            bool flag_I = false;
            bool flag_F = false;
            //scroll the line
            char[] Line_A = line.ToArray();
            for (int i = 0; i < Line_A.Length; i++)
            {
                if (flag_I == false && Line_A[i] != '"')//before
                {
                    I_string = I_string + Line_A[i];
                }
                else if (flag_I == false && Line_A[i] == '"')//now
                {
                    flag_I = true;//delete flag
                    N_string = N_string + Line_A[i];
                }
                else if (flag_I == true && flag_F == false )
                {

                }
            }
            return null;

            //ESTE METODO ESTA MALO HAY QUE CAMBIARLO 
            // hay que tomar como ejemplo una cadena de este tipo --> string c = "hola" + "mundo";
        }


        #endregion


    }
}
