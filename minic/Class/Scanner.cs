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
        //---------------------------------------PUBLIC FUNCTIONS---------------------------------------
        #region FUNCTIONS PUBLIC
        //List for save data the lexic analysis
        private List<Token> NewFile = new List<Token>();
        private Operadores_fase1 t = new Operadores_fase1(); //call the tokens
        private int column = 1;
        private int row = 0;
        private bool flag_comment = false; //if there is an open comment --> true

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
        public List<Token> Scanner_Lexic(string[] file)
        {
            //scrolll the array
            Scanner_Private(file);
            return NewFile; // return the new file for write 
        }
        #endregion
        //---------------------------------------PRIVATE FUNCTIONS---------------------------------------
        #region PRIVATE FUNCTIONS

        //method for analysis the file
        private void Scanner_Private(string[] file)
        {
            //scroll the file
            for (int i = 0; i < file.Length; i++) // i = fila
            {
                //separate the lines, separate the words
                var word = Regex.Split(file[i], " ");
                row++;

                if (flag_comment == false)
                {
                    //check if it is not comment or string
                    var type_line = Is_Line(file[i]);

                    if (type_line != "")
                        Scanner_Line(file[i], row, type_line); //filter the commentary and string, string the line complete
                    else
                        Filter_First(word, row); //first filter for word by word, array the word          
                }
                else
                    Continue_Commented(file[i], row);
                column = 1; //restart             
            }
            if (flag_comment == true) //check the error EOF
                Insert_Error("comment", row, "Error");
        }

        //method for first filter, parameters words and line
        private void Filter_First(string[] word, int line)
        {
            //call the tokens
            var ope = t.Operators_Words();
            var operators1 = Operator_A(ope, 1); //double oerator like '==', '!=', '||'
            var operators2 = Operator_A(ope, 2); //single operators like '=', '*', '+', ';'.

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
                            Second_Filter(word[i], line);
                        }
                    }
                }

                //only numbers --> decimal constant
                else if (Regex.IsMatch(word[i], decimalConst))
                {
                    Insert_Word(word[i], line, "T_IntConst (value = " + word[i] + ")");
                }
                else if (Regex.IsMatch(word[i], doubleGeneral)) //double consts or exp numbers
                {
                    if (Regex.IsMatch(word[i], doubleFloat)) //Only double, with decimal
                    {
                        Insert_Word(word[i], line, "T_DoubleConst (value = " + word[i] + ")");
                    }
                    else if (Regex.IsMatch(word[i], doubleFloat2)) //without decimals
                    {
                        Insert_Word(word[i], line, "T_DoubleConst (value = " + word[i] + "00)");
                    }
                    else if (Regex.IsMatch(word[i], expS)) //Exp without '+-'
                    {
                        if (word[i].Contains("E"))
                        {
                            Insert_Word(word[i], line, "T_DoubleExpConst (value = " + word[i].Split('E')[0] + "E+" + word[i].Split('E')[1] + ")");
                        }
                        else
                        {
                            Insert_Word(word[i], line, "T_DoubleExpConst (value = " + word[i].Split('e')[0] + "E+" + word[i].Split('e')[1] + ")");
                        }
                    }
                    else
                    {
                        Insert_Word(word[i], line, "T_DoubleExpConst (value = " + word[i] + ")");
                    }

                }
                //only operator (1)
                else if (Regex.IsMatch(word[i], @"^(" + operators1 + ")$"))
                {
                    Insert_Word(word[i], line, "T_Operator");
                }
                //only operator (1)
                else if (Regex.IsMatch(word[i], @"^(" + operators2 + ")$"))
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
                    column++;
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

            var listOnlyWords = Regex.Matches(copy, @"([a-zA-Z]+)((\d|_)*([a-zA-Z]*))*"); //Only identifiers
            RemoveRecurrence(ref copy, listOnlyWords);

            var listHexaDecimals = Regex.Matches(copy, @"(0x|0X)[0-9]*[a-fA-F]*");
            RemoveRecurrence(ref copy, listHexaDecimals);

            var listExp = Regex.Matches(copy, @"([0-9]+[.](([0-9]*)([E|e][+|-]?[0-9]+)))");
            RemoveRecurrence(ref copy, listExp);

            var listDouble = Regex.Matches(copy, @"[0-9]+[.]([0-9]+)?");
            RemoveRecurrence(ref copy, listDouble);

            var operators1 = Operator_A(t.Operators_Words(), 1);
            var listOperators = Regex.Matches(copy, @"" + operators1 + ""); //find doubles operators like == != () {} <= >=
            RemoveRecurrence(ref copy, listOperators);

            var listBoolean = Regex.Matches(copy, @"(true|false)"); //find boolean values.
            RemoveRecurrence(ref copy, listBoolean);

            var listDecimals = Regex.Matches(copy, @"[0-9]+");
            RemoveRecurrence(ref copy, listDecimals);

            var operators2 = Operator_A(t.Operators_Words(), 2);
            var listOperators2 = Regex.Matches(copy, @"" + operators2 + ""); //find single operators like = , . ! < > { } ( )
            RemoveRecurrence(ref copy, listOperators2);

            var listTokens = FindIndex(word, line, listReserved, listHexaDecimals, listExp, listDouble, listOperators,
                listOperators2, listDecimals, listBoolean, listOnlyWords, copy);

            foreach (var tokenType in listTokens)
            {
                tokenType.column_I = column;
                tokenType.column_F = tokenType.column_I + (tokenType.cadena.Length - 1);
                column = tokenType.column_F + 1;

                if (tokenType.description == "Identifier length exceeds 31 characters")
                {
                    tokenType.cadena = "truncated string";
                }

                NewFile.Add(tokenType);
            }
        }

        private List<Token> FindIndex(string word, int line, MatchCollection listReserved, MatchCollection listHexaDecimals, MatchCollection listExp, MatchCollection listDouble, MatchCollection listOperators, MatchCollection listOperators2, MatchCollection listDecimals, MatchCollection listBoolean, MatchCollection listOnlyWords, string copy)
        {
            var listTokens = new List<Token>();

            if (listReserved.Count != 0)
            {
                foreach (Match match1 in listReserved)
                {
                    Token newType = new Token();
                    newType.cadena = match1.Value;
                    newType.linea = line;
                    newType.Error = ""; //dont exist error
                    newType.IndexMatch = match1.Index;
                    newType.description = "T_" + match1.Value;
                    listTokens.Add(newType);
                }
            }

            if (listHexaDecimals.Count != 0)
            {
                foreach (Match match2 in listHexaDecimals)
                {
                    Token newType = new Token();
                    newType.cadena = match2.Value;
                    newType.linea = line;
                    newType.Error = ""; //dont exist error
                    newType.IndexMatch = match2.Index;
                    newType.description = "T_Hexadecimal: (value = " + match2.Value + ")";
                    listTokens.Add(newType);
                }
            }

            if (listExp.Count != 0)
            {
                foreach (Match match3 in listExp)
                {
                    Token newType = new Token();
                    newType.cadena = match3.Value;
                    newType.linea = line;
                    newType.Error = ""; //dont exist error
                    newType.IndexMatch = match3.Index;

                    if (Regex.IsMatch(match3.Value, expS)) //Exp without '+-'
                    {
                        if (match3.Value.Contains("E"))
                        {
                            newType.description = "T_DoubleExpConst (value = " + match3.Value.Split('E')[0] + "E+" + match3.Value.Split('E')[1] + ")";
                        }
                        else
                        {
                            newType.description = "T_DoubleExpConst (value = " + match3.Value.Split('e')[0] + "E+" + match3.Value.Split('e')[1] + ")";
                        }
                    }
                    else
                    {
                        newType.description = "T_DoubleExpConst (value = " + match3.Value + ")";
                    }

                    listTokens.Add(newType);
                }
            }

            if (listDouble.Count != 0)
            {
                foreach (Match match4 in listDouble)
                {
                    Token newType = new Token();
                    newType.cadena = match4.Value;
                    newType.linea = line;
                    newType.Error = ""; //dont exist error
                    newType.IndexMatch = match4.Index;

                    if (Regex.IsMatch(match4.Value, doubleFloat))
                    {
                        newType.description = "T_DoubleConst (value = " + match4 + ")";
                    }
                    if (Regex.IsMatch(match4.Value, doubleFloat2))
                    {
                        newType.description = "T_DoubleConst (value = " + match4 + "00)";
                    }
                    listTokens.Add(newType);
                }
            }

            if (listOperators.Count != 0)
            {
                foreach (Match match5 in listOperators)
                {
                    Token newType = new Token();
                    newType.cadena = match5.Value;
                    newType.linea = line;
                    newType.Error = ""; //dont exist error
                    newType.IndexMatch = match5.Index;
                    newType.description = "T_Operator";
                    listTokens.Add(newType);
                }
            }

            if (listOperators2.Count != 0)
            {
                foreach (Match match6 in listOperators2)
                {
                    Token newType = new Token();
                    newType.cadena = match6.Value;
                    newType.linea = line;
                    newType.Error = ""; //dont exist error
                    newType.IndexMatch = match6.Index;
                    newType.description = "T_Operator";
                    listTokens.Add(newType);
                }
            }

            if (listDecimals.Count != 0)
            {
                foreach (Match match7 in listDecimals)
                {
                    Token newType = new Token();
                    newType.cadena = match7.Value;
                    newType.linea = line;
                    newType.Error = ""; //dont exist error
                    newType.IndexMatch = match7.Index;
                    newType.description = "T_IntConst (value = " + match7 + ")";
                    listTokens.Add(newType);
                }
            }

            if (listBoolean.Count != 0)
            {
                foreach (Match match8 in listBoolean)
                {
                    Token newType = new Token();
                    newType.cadena = match8.Value;
                    newType.linea = line;
                    newType.Error = ""; //dont exist error
                    newType.IndexMatch = match8.Index;
                    newType.description = "T_Bool: " + match8.Value;
                    listTokens.Add(newType);
                }
            }

            if (listOnlyWords.Count != 0)
            {
                foreach (Match match9 in listOnlyWords)
                {
                    Token newType = new Token();

                    if (match9.Value.Length > 30)
                    {
                        newType.cadena = match9.Value.Substring(0, 31);
                        newType.linea = line;
                        newType.Error = ""; //dont exist error
                        newType.IndexMatch = match9.Index;
                        newType.description = "T_Identifier";


                        var errorToken = new Token();
                        errorToken.cadena = match9.Value.Substring(31, (match9.Value.Length - 31));
                        errorToken.linea = line;
                        errorToken.Error = "Error";
                        errorToken.description = "Identifier length exceeds 31 characters";
                        errorToken.IndexMatch = match9.Index + 30;
                        listTokens.Add(errorToken);
                    }
                    else
                    {
                        newType.cadena = match9.Value;
                        newType.linea = line;
                        newType.Error = ""; //dont exist error
                        newType.IndexMatch = match9.Index;
                        newType.description = "T_Identifier";

                    }

                    listTokens.Add(newType);
                }
            }

            if (copy.Length >= 1)
            {
                foreach (var item in copy.Split(' '))
                {
                    if (item != "" && !(item.Contains("\t")) && (item != "numbe"))
                    {
                        Token newType = new Token();
                        newType.cadena = item;
                        newType.linea = line;
                        newType.Error = "Error"; //dont exist error
                        newType.description = "*** Unrecognized char:";
                        newType.IndexMatch = word.IndexOf(item);
                        listTokens.Add(newType);
                    }
                }
            }

            listTokens.Sort(Token.OrderByIndex);
            return listTokens;
        }

        //method for scanner line, because exist commentary or string or both
        private void Scanner_Line(string word, int line, string type) //type-->string or commentary/2
        {
            //depend the case 
            switch (type)
            {
                case "string":
                    Case_String(word, line);
                    break;
                case "commentary":
                    Case_Commentary(word, line);
                    break;
                case "commentary2":
                    Case_Commentary2(word, line);
                    break;
                case "Error commentary":
                    Case_ErrorComentary(word, line);
                    break;
            }
        }

        //method to know if it is a reserved word
        private bool Is_ReservedWord(string w, int line)
        {
            var reserved = Reserved_A(t.Reserved_Words());

            if (Regex.IsMatch(w, @"^(" + reserved + ")$"))
            {
                Insert_Word(w, line, "T_" + w);
                return true;
            }
            return false;
        }

        //method to know if it is a string constant
        private void Is_String(string word, int line)
        {
            if (!word.Contains(@"\n"))
                Insert_Word(word, line, "T_StringConstant (value = " + word + ")");
            else
                Insert_Error(word, line, "Unrecognized char: 'new line'");
        }

        //method for insert in list type
        private void Insert_Word(string word, int line, string type)
        {
            //insert into TYPE list
            Token newType = new Token();
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
            var array_line = line.ToArray();

            for (int i = 0; i < array_line.Length; i++)
            {
                //if is a line commentary
                if (i < array_line.Length - 1 && array_line[i] == '/' && array_line[i + 1] == '*')
                    return "commentary2";
                //if is a begin commentary
                if (i < array_line.Length - 1 && array_line[i] == '/' && array_line[i + 1] == '/')
                    return "commentary";
                //if is a string
                if (array_line[i] == '"')
                    return "string";
                if (i < array_line.Length - 1 && array_line[i] == '*' && array_line[i + 1] == '/')
                    return "Error commentary";
            }

            return "";
        }

        //method for check that the string has the correct format
        private void Case_String(string word, int line) //characters
        {
            var before_String = string.Empty;
            var now_String = string.Empty;
            var after_String = string.Empty;
            var flag_bs = false; //before string
            var flag_as = false; //after string
            var word_A = word.ToArray();

            //scroll the line
            for (int i = 0; i < word_A.Length; i++)
            {
                if (word_A[i] != '"' && flag_bs == false && flag_as == false)
                    before_String = before_String + word_A[i];
                else if (word_A[i] == '"' && flag_bs == false && flag_as == false)
                {
                    flag_bs = true;
                    now_String = now_String + word_A[i];
                }
                else if (word_A[i] != '"' && flag_bs && flag_as == false)
                    now_String = now_String + word_A[i];
                else if (word_A[i] == '"' && flag_bs && flag_as == false)
                {
                    now_String = now_String + word_A[i];
                    flag_as = true;
                }
                else if (flag_as)
                    after_String = after_String + word_A[i];
            }

            if (before_String != "")
            {
                // analisys before string
                var word2 = Regex.Split(before_String, " ");
                Filter_First(word2, row); //first filter for word by word, array the word  
            }
            //if is string close
            if (flag_as == false)
            {
                //ERROR dont close string
                Insert_Error("Error String", line, "Error dont close string"); //wait 
            }
            else
            {
                Is_String(now_String, line);

                //analysis after string
                //check if it is not comment or string
                string type_line = Is_Line(after_String);
                if (type_line != "")
                    Scanner_Line(after_String, row, type_line); //filter the commentary and string, string the line complete
                else
                {
                    string[] b_word = Regex.Split(after_String, " ");//parse string separately
                    Filter_First(b_word, line);
                }
            }
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
                    if (match.Value != "")
                    {
                        var spaces = string.Empty;

                        for (int i = 0; i < match.Value.Length; i++)
                        {
                            spaces += " ";
                        }

                        copy = copy.Replace(match.Value, spaces);
                    }
                }
            }
        }

        //method for case commentary in method scanner line
        private void Case_Commentary(string word, int line)
        {
            //check that the commentary has the correct format
            var Word_A = word.ToArray();
            var before_Comment = "";
            var i = 0;

            while (Word_A[i] != '/' || Word_A[i + 1] != '/')
            {
                before_Comment = before_Comment + Word_A[i];
                i++;
            }
            
            //call the method for analisys the string before
            if (before_Comment != "")
            {
                var b_word = Regex.Split(before_Comment, " ");//parse string separately
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
                if (i < word.Length - 1 && Word_A2[i] != '/' && Word_A2[i + 1] != '*' && flag_before == false && flag_after == false)
                    before_Comment2 = before_Comment2 + Word_A2[i];
                else if (flag_before == false && Word_A2[i] == '/' && Word_A2[i + 1] == '*' && flag_after == false)
                    flag_before = true;
                else if (i < word.Length - 1 && Word_A2[i] == '*' && Word_A2[i + 1] == '/' && flag_after == false)
                {
                    flag_after = true;
                    i++; //quit character '/'
                }
                else if (flag_before && flag_after)
                    after_Comment2 = after_Comment2 + Word_A2[i];
            }
            //check commentary before
            if (before_Comment2 != "")
            {
                string[] b_word = Regex.Split(before_Comment2, " ");//parse string separately
                Filter_First(b_word, line);
            }
            //check flag
            if (flag_after)
            {
                if (after_Comment2 != "")
                {
                    //check if it is not comment or string
                    string type_line = Is_Line(after_Comment2);
                    if (type_line != "")
                        Scanner_Line(after_Comment2, row, type_line); //filter the commentary and string, string the line complete
                    else
                    {
                        string[] b_word = Regex.Split(after_Comment2, " ");//parse string separately
                        Filter_First(b_word, line);
                    }
                }
            }
            else
                flag_comment = true; //exist open commentary
        }

        //method for case commentary2 in method scanner line
        private void Case_ErrorComentary(string word, int line)
        {
            var word_A = word.ToArray(); 
            var before = string.Empty;
            var now = "*/";
            var after = "";
            var flag = false;

            for (int i = 0; i < word_A.Length; i++)
            {
                if (word_A[i] == '*' && word_A[i + 1] == '/')
                {
                    flag = true;
                    i++;
                }
                else if (flag == false)
                    before = before + word_A[i];
                else if (flag == true)
                    after = after + word_A[i];
            }
            
            if (before != "")
            {
                //check if it is not comment or string
                var type_line = Is_Line(before);

                if (type_line != "")
                    Scanner_Line(before, row, type_line); //filter the commentary and string, string the line complete
                else
                {
                    var b_word = Regex.Split(before, " ");//parse string separately
                    Filter_First(b_word, line);
                }
            }
            //insert error
            Insert_Error(now, line, "Error: End of comment unpaired");
            column += 2;
            
            if (after != "")
            {
                //check if it is not comment or string
                string type_line = Is_Line(after);
                if (type_line != "")
                    Scanner_Line(after, row, type_line); //filter the commentary and string, string the line complete
                else
                {
                    string[] b_word = Regex.Split(after, " ");//parse string separately
                    Filter_First(b_word, line);
                }
            }
        }

        //method to know if this line is the closing of the comment
        private void Continue_Commented(string word, int line)
        {
            var word_A = word.ToArray();
            var after_commentary = string.Empty;

            //scroll the string
            for (int i = 0; i < word_A.Length; i++)
            {
                if (i < word_A.Length - 1 && word_A[i] == '*' && word_A[i + 1] == '/')
                {
                    flag_comment = false;
                    i++;//next character analysis
                }
                else if (flag_comment == false)
                    after_commentary = after_commentary + word_A[i];
            }
            if (after_commentary != "")
            {
                //check if it is not comment or string
                var type_line = Is_Line(after_commentary);

                if (type_line != "")
                    Scanner_Line(after_commentary, row, type_line); //filter the commentary and string, string the line complete
                else
                {
                    string[] b_word = Regex.Split(after_commentary, " ");//parse string separately
                    Filter_First(b_word, line);
                }
            }
        }

        //method for insert error EOF
        private void Insert_Error(string word, int line, string type)
        {
            //insert into TYPE list
            Token newType = new Token();
            newType.cadena = word;
            newType.linea = line;
            newType.Error = "Error"; //dont exist error
            newType.column_I = 0;
            newType.column_F = 0;
            newType.description = type; //In this case for diferent other case use description
            NewFile.Add(newType);
        }
        #endregion
    }
}