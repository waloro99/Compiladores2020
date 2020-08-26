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
        public bool flag_comment = false; //if there is an open comment --> true

        //Patterns
        private string onlyLetters = @"^[a-zA-Z]+$";
        private string boolPattern = @"^(true|false)$";
        private string decimalConst = @"^[0-9]+$";
        private string doubleGeneral = @"^[0-9]+[.](([0-9]*)([E|e][+|-]?[0-9]+)?)$"; //double or Exp
        private string doubleFloat = @"^[0-9]+[.]([0-9]+)$"; //double with '.' and decimal numbers
        private string doubleFloat2 = @"^[0-9]+[.]$"; //double without decimal numbers
        private string expS = @"^[0-9]+[.](([0-9]*)([E|e][0-9]+)?)$"; //Exp without '+-' simbols
        private string hexaDecimal = @"^(0x|0X)[0-9]*[a-fA-F]*$";

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
                if (flag_comment == false)
                {
                    //check if it is not comment or string
                    string type_line = Is_Line(file[i]);
                    if (type_line != "")
                        Scanner_Line(file[i], row, type_line); //filter the commentary and string, string the line complete
                    else
                        Filter_First(word, row); //first filter for word by word, array the word          
                }
                else
                    Continue_Commented(file[i], row);
                column = 1; //restart             
            }

        }

        //method for first filter, parameters words and line
        private void Filter_First(string[] word, int line)
        {
            //call the tokens
            List<string> ope = t.Operators_Words();
            string operators1 = Operator_A(ope,1); //double oerator like '==', '!=', '||'
            string operators2 = Operator_A(ope, 2); //single operators like '=', '*', '+', ';'.
            //scroll the array word by word
            for (int i = 0; i < word.Length; i++)
            {
                //only letters
                if (Regex.IsMatch(word[i], onlyLetters))
                { 
                    //if not is reserved word
                    if (!Is_ReservedWord(word[i], line))
                    {
                        //if is a constant boolean
                        if (Regex.IsMatch(word[i], boolPattern))
                        {
                            //is a bool
                            Insert_Word(word[i], line, "T_Bool: " + word[i]);
                        }
                        else
                        {
                            //is a identifier
                            Insert_Word(word[i], line, "T_Identifier");
                        }
                    }
                }
                
                //only numbers --> decimal constant
                else if (Regex.IsMatch(word[i], decimalConst))
                {
                    Insert_Word(word[i], line, "T_IntConst (value = " + word[i] + ")");
                }
                else if (Regex.IsMatch(word[i], doubleGeneral )) //double consts or exp numbers
                {
                    if (Regex.IsMatch(word[i], doubleFloat)) //Only double, with decimal
                    {
                        Insert_Word(word[i], line, "T_DoubleConst (value = " + word[i] + ")");
                    }
                    else if(Regex.IsMatch(word[i], doubleFloat2)) //without decimals
                    {
                        Insert_Word(word[i], line, "T_DoubleConst (value = " + word[i] + "00)");
                    }
                    else if(Regex.IsMatch(word[i], expS)) //Exp without '+-'
                    {
                        Insert_Word(word[i], line, "T_DoubleExpConst (value = " + word[i].Split('E')[0] + "E+" + word[i].Split('E')[1] + ")");
                    }
                    else
                    {
                        Insert_Word(word[i], line, "T_DoubleExpConst (value = " + word[i] + ")");
                    }
                    
                }
                //only operator (1)
                else if (Regex.IsMatch(word[i], @"(" + operators1 +")$"))
                {
                    Insert_Word(word[i], line, "T_Operator");
                }
                //only operator (1)
                else if (Regex.IsMatch(word[i], @"(" + operators2 + ")$"))
                {
                    Insert_Word(word[i], line, "T_Operator");
                }
                //numbers and letters
                else if (Regex.IsMatch(word[i], hexaDecimal))
                {
                    Insert_Word(word[i], line, "T_Hexadecimal: (value = " + word[i] + ")");
                }
                //other case
                else
                {
                    Second_Filter(word[i], line);
                }
            }
        }

        //method for Second Filter, many tokens in the same line
        private void Second_Filter(string word, int line)
        {
            var copy = word;

            var reserved = Reserved_A(t.Reserved_Words());
            var listReserved = Regex.Matches(copy, @"(" + reserved + ")"); //find Reserved words
            RemoveRecurrence(ref copy, listReserved);

            var listHexaDecimals = Regex.Matches(copy, @"(0x|0X)[0-9]*[a-fA-F]*");
            RemoveRecurrence(ref copy, listHexaDecimals);

            var listExp = Regex.Matches(copy, @"([0-9]+[.](([0-9]*)([E|e][+|-]?[0-9]+)))");
            RemoveRecurrence(ref copy, listExp);

            var listDouble = Regex.Matches(copy, @"[0-9]+[.]([0-9]+)?");
            RemoveRecurrence(ref copy, listDouble);

            var operators1 = Operator_A(t.Operators_Words(), 1);
            var listOperators = Regex.Matches(copy, @""+ operators1 +""); //find dobules operators like == != () {} <= >=
            RemoveRecurrence(ref copy, listOperators);

            var operators2 = Operator_A(t.Operators_Words(), 2);
            var listOperators2 = Regex.Matches(copy, @"" + operators2 + ""); //find single operators like = , . ! < > { } ( )
            RemoveRecurrence(ref copy, listOperators2);

            var listDecimals = Regex.Matches(copy, @"[0-9]+");
            RemoveRecurrence(ref copy, listDecimals);

            var listBoolean = Regex.Matches(copy, @"(true|false)"); //find boolean values.
            RemoveRecurrence(ref copy, listBoolean);

            var listOnlyWords = Regex.Matches(copy, @"[a-zA-Z]+"); //Only identifiers
            RemoveRecurrence(ref copy, listOnlyWords);
        }

        //method for scanner line, because exist commentary or string or both
        private void Scanner_Line(string word, int line, string type) //type-->string or commentary/2
        {
            //depend the case 
            switch (type)
            {
                case "string":
                    //check that the string has the correct format
                    string flag_strig = Is_CorrectString(word, line); // --> bad method
                    break;
                case "commentary":
                    Case_Commentary(word,line);
                    break;
                case "commentary2":
                    Case_Commentary2(word,line);
                    break;
                default:
                    break;
            }
        }

        //method to know if it is a reserved word
        private bool Is_ReservedWord(string w, int line)
        {
            //call the tokens
            List<string> reser = t.Reserved_Words();
            string reserved = Reserved_A(reser);

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
                Insert_Word(w, line, "T_StringConst");
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
            newType.description = type;
            NewFile.Add(newType);

        }

        //method to know if it is necessary to analyze by line
        private string Is_Line(string line)
        {
            char[] array_line = line.ToArray(); //line character by character
            //scroll the array
            for (int i = 0; i < array_line.Length; i++)
            {
                //if is a line commentary
                if (array_line[i] == '/' && array_line[i + 1] == '*')
                    return "commentary2";
                //if is a begin commentary
                else if (array_line[i] == '/' && array_line[i + 1] == '/')
                    return "commentary";
                //if is a string
                else if (array_line[i] == '"')
                    return "string";
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

        //method for array the operator / array the reserved word
        private string Reserved_A(List<string> o)
        {
            string res = string.Empty;
            foreach (var item in o)
                res += "|" + item;

            return res.TrimStart('|');
        }

        private string Operator_A(List<string> o, int group)
        {
            string res = string.Empty;

            switch (group)
            {
                case 1:
                    foreach (var item1 in o)
                        if (item1 == "||")
                        {
                            res += "|" + "(\\|\\|)";
                        }
                        else if (item1 == "[]")
                        {
                            res += "|" + "\\[\\]";
                        }
                        else if (item1 == "()")
                        {
                            res += "|" + "\\(\\)";
                        }
                        else if (item1 == "{}")
                        {
                            res += "|" + "\\{\\}";
                        }
                        else if (item1 == "==" || item1 == ">=" || item1 == "<=" || item1 == "!=" || item1 == "&&" || item1 == "")
                        {
                            res += "|" + item1;
                        }
                    break;

                case 2:
                    foreach (var item in o)
                        if (item == "[")
                        {
                            res += "|" + "\\[";
                        }
                        else if (item == "]")
                        {
                            res += "|" + "\\]";
                        }
                        else if (item == "(")
                        {
                            res += "|" + "\\(";
                        }
                        else if (item == ")")
                        {
                            res += "|" + "\\)";
                        }
                        else if (item == "}")
                        {
                            res += "|" + "\\}";
                        }
                        else if (item == "{")
                        {
                            res += "|" + "\\{";
                        }
                        else if (item == "+")
                        {
                            res += "|" + "\\+";
                        }
                        else if (item == "*")
                        {
                            res += "|" + "\\*";
                        }
                        else if (item == ".")
                        {
                            res += "|" + "\\.";
                        }
                        else if (item == ";" | item == "," || item == "!" || item == "-" || item == "/" || item == "%" || item == "<" || item == ">" || item == "<" || item == "=" || item == "<")
                        {
                            res += "|" + item;
                        }
                    break;
            }
            return res.TrimStart('|');
        }

        private void RemoveRecurrence(ref string copy, MatchCollection recurrences)
        {
            if (recurrences.Count != 0)
            {
                foreach (Match match in recurrences)
                {
                    copy = copy.Replace(match.Value, "");
                }
            }
        }

        //method for case commentary in method scanner line
        private void Case_Commentary(string word, int line)
        {
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
            if (before_Comment != "")
            {
                string[] b_word = Regex.Split(before_Comment, " ");//parse string separately
                Filter_First(b_word, line);
            }         
        }

        //method for case commentary2 in method scanner line
        private void Case_Commentary2(string word, int line)
        {
            //check that the commentary2 has the correct format
            char[] Word_A2 = word.ToArray();
            string before_Comment2 = string.Empty;
            string after_Comment2 = string.Empty;
            bool flag_before = false;
            bool flag_after = false;
            //Scroll the line
            for (int i = 0; i < word.Length; i++)
            {
                if (i < word.Length-1 && Word_A2[i] != '/' && Word_A2[i + 1] != '*' && flag_before == false)
                    before_Comment2 = before_Comment2 + Word_A2[i];
                else if (flag_before == false && Word_A2[i] == '/' && Word_A2[i + 1] == '*' )
                    flag_before = true;
                else if (i < word.Length - 1 &&  Word_A2[i] == '*' && Word_A2[i + 1] == '/')
                {
                    flag_after = true;
                    i++; //quit character '/'
                }                   
                else if (flag_before == true && flag_after == true)
                    after_Comment2 = after_Comment2 + Word_A2[i];
            }
            //check commentary before
            if (before_Comment2 != "")
            {
                string[] b_word = Regex.Split(before_Comment2, " ");//parse string separately
                Filter_First(b_word, line);
            }
            //check flag
            if (flag_after == true)
            {
                if (after_Comment2 != "")
                {
                    string[] b_word = Regex.Split(after_Comment2, " ");//parse string separately
                    Filter_First(b_word, line);
                }
            }
            else
                flag_comment = true; //exist open commentary
        }

        //method to know if this line is the closing of the comment
        private void Continue_Commented(string word, int line)
        {
            char[] word_A = word.ToArray();
            string after_commentary = string.Empty;
            //scroll the string
            for (int i = 0; i < word_A.Length; i++)
            {
                if (i < word_A.Length - 1 && word_A[i] == '*' && word_A[i+1] == '/' )
                {
                    flag_comment = false;
                    i++;//next character analysis
                }               
                else if (flag_comment == false)
                    after_commentary = after_commentary + word_A[i];
            }
            if (after_commentary != "")
            {
                string[] b_word = Regex.Split(after_commentary, " ");//parse string separately
                Filter_First(b_word,line);
            }
        }

        #endregion


    }
}
