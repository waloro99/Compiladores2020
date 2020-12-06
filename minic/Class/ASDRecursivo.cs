using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace minic.Class
{
    public class ASDRecursivo
    {
        //---------------------------------------FUNCTIONS PUBLIC---------------------------------------
        public List<Token> t = new List<Token>(); //list with tokens
        public Token token = new Token(); //value the lookahad
        public string lookahad = string.Empty; //var global
        public string Msg_Error = string.Empty;
        public int x = -1; //scroll var

        public bool flagError = false;

        #region FUNCTIONS PUBLICS

        //method public 
        public string ASDRecursivo_F(List<Token> tokens)
        {
            t = tokens;
            ASDR_Flow();
            return Msg_Error;
        }

        #endregion

        //---------------------------------------FUNCTIONS PRIVATE--------------------------------------

        #region FUNCTIONS PRIVATE

        //Method to go through the program by asd ASDR.
        private void ASDR_Flow()
        {
            //lookahad
            lookahad = NextToken();
            //begin gramatic
            Parse_Program();
        }

        //Method for know next token
        private string NextToken()
        {
            x++; //increment var
            if (x < t.Count())
            {
                token = t[x];
            }
            else
            {
                token.description = "";
            }
            //save type 
            string l = F_Lookahad(token);
            return l;
        }

        private string F_Lookahad(Token T_token)
        {
            if (T_token.description.Contains("T_Bool"))
            {
                return "boolConstant";
            }
            else if (T_token.description.Contains("T_IntConst"))
            {
                return "intConstant";
            }
            else if (T_token.description.Contains("T_DoubleConst"))
            {
                return "doubleConstant";
            }
            else if (T_token.description.Contains("T_DoubleExpConst")) //--> idk
            {
                return "doubleConstant";
            }
            else if (T_token.description.Contains("T_Operator"))
            {
                return T_token.cadena;
            }
            else if (T_token.description.Contains("T_Hexadecimal"))
            {
                return "hexaConstant";
            }
            else if (T_token.description.Contains("T_Identifier"))
            {
                return "identificador";
            }
            else if (T_token.description.Contains("T_StringConstant"))
            {
                return "stringConstant";
            }
            else if (T_token.description.Contains("T_"))
            {
                return T_token.cadena;
            }
            else
            {
                return "";
            }
        }

        //Method for know match the token
        private string MatchToken(string token, bool sTerminal)
        {
            if (lookahad != "")
            {
                if (lookahad == token)
                {
                    lookahad = NextToken();
                    return "";
                }

                //ERROR
                if (sTerminal)
                {
                    Msg_Error += "\n- Unexpected Token: " + lookahad + ", expected " + token;
                    lookahad = NextToken();
                    return "";
                }
            }
            else
            {
                return "exit";
            }

            return "Undefined";
        }

        //Program → Decl
        private void Parse_Program()
        {
            while (lookahad != "")
            {
                Parse_Decl();
            }

        }

        //Decl → VariableDecl Decl’ | FunctionDecl Decl’
        private void Parse_Decl()
        {
            if (Parse_VariableDecl() == "Undefined")
            {
                if (Parse_FunctionDecl() == "Undefined")
                {
                    if (Parse_DeclP() == "Undefined")
                    {
                        Msg_Error += lookahad;
                        lookahad = NextToken();
                    }
                }
            }
        }

        //Decl’ → VariableDecl Decl’ | FunctionDecl Decl’ | eps
        private string Parse_DeclP()
        {
            if (lookahad != "")
            {
                var p = Parse_VariableDecl();
                
                if (p != "Undefined" && p != "exit")
                {
                    return Parse_DeclP();
                }

                p = Parse_FunctionDecl();
                if (p != "Undefined" && p != "exit")
                {
                    return Parse_DeclP();
                }

                if (p == "exit")
                {
                    return p;
                }
                return "";
            }

            return "exit";
        }

        //VariableDecl  → Variable ;
        private string Parse_VariableDecl()
        {
            if (lookahad != "")
            {
                var p = Parse_Variable();

                if (p != "Undefined" && p != "exit")
                {
                    if (lookahad != "(" && lookahad != ")" && lookahad != "")
                    {
                        return MatchToken(";", true);
                    }

                    return MatchToken(";", false);
                }
                if (p == "exit")
                {
                    return p;
                }
                return "Undefined";
            }
            return "exit";
        }

        //Variable → Type ident
        private string Parse_Variable()
        {
            if (lookahad != "")
            {
                var p = Parse_Type();

                if (p != "Undefined" && p != "exit")
                {
                    return MatchToken("identificador", true); //change name
                }
                if (p == "exit")
                {
                    return "exit";
                }
                return "Undefined";
            }

            return "exit";
        }

        //Type → Type’ Type2
        private string Parse_Type()
        {
            if (lookahad != "")
            {
                var p = Parse_TypeP();
                if (p == "Undefined" && p != "exit")
                {
                    return "Undefined";
                }

                p = Parse_Type2();
                if (p == "Undefined" && p != "exit")
                {
                    return "";
                }

                if (p == "exit")
                {
                    return p;
                }

                return "";
            }

            return "exit";
        }

        //Type’ → int | double | bool | string | ident
        private string Parse_TypeP()
        {
            if (lookahad != "")
            {
                if (lookahad == "int")
                {
                    lookahad = NextToken();
                    return "";
                }
                else if (lookahad == "double")
                {
                    lookahad = NextToken();
                    return "";
                }
                else if (lookahad == "bool")
                {
                    lookahad = NextToken();
                    return "";
                }
                else if (lookahad == "string")
                {
                    lookahad = NextToken();
                    return "";
                }
                else if (lookahad == "identificador")
                {
                    lookahad = NextToken();
                    return "";
                }

                //else error -- DONE
                return "Undefined";
            }

            return "exit";
        }

        //Type2 → [] Type2 | eps
        private string Parse_Type2()
        {
            if (lookahad != "")
            {
                return MatchToken("[]", false);
            }

            return "exit";
        }

        //FunctionDecl → Type ident ( Formals ) Stmt’ | void ident ( Formals ) Stmt’
        private string Parse_FunctionDecl()
        {
            if (lookahad != "")
            {
                if (lookahad != "void")
                {
                    if (lookahad != "(")
                    {
                        Parse_Type();
                        MatchToken("identificador", true);
                    }

                    MatchToken("(", true);
                    Parse_Formals();
                    MatchToken(")", true);
                    Parse_StmtP();
                }
                else if (lookahad == "void")
                {
                    MatchToken("void", true);
                    MatchToken("identificador", true);
                    MatchToken("(", true);
                    Parse_Formals();
                    MatchToken(")", true);
                }

                return "Undefined";
            }

            return "exit";
        }

        //Stmt’ → Stmt Stmt’ | eps
        private string Parse_StmtP()
        {
            if (lookahad != "")
            {
                if (Parse_Stmt() == "Undefined")
                {
                    return "";
                }
                else if (Parse_StmtP() == "Undefined")
                {
                    return "";
                }
                else
                {
                    return "Undefined";
                }
            }

            return "exit";
        }

        //Formals → Variable Variable’ , | eps
        private string Parse_Formals()
        {
            if (lookahad != "")
            {
                if (Parse_Variable() != "Undefined")
                {
                    if (Parse_VariableP() == "Undefined")
                    {
                        return "";
                    }
                    else if (Parse_VariableP() != "Undefined")
                    {
                        return MatchToken(",", true);
                    }
                }
                //else eps -- Done
                return "";
            }

            return "exit";
        }

        ////Variable’ →  Variable Variable’ | eps
        private string Parse_VariableP()
        {
            if (lookahad != "")
            {
                if (Parse_Variable() == "Undefined")
                {
                    return "Undefined";
                }

                return "";
            }

            return "exit";
        }

        //Stmt → IfStmt | ForStmt | Expr ;
        private string Parse_Stmt()
        {
            if (lookahad != "")
            {
                //if ifstmt
                if (Parse_IfStmt() == "Undefined")
                {
                    if (Parse_ForStmt() != "Undefined")
                    {
                        if (Parse_Expr() != "Undefined")
                        {
                            return MatchToken(";", true);
                        }
                    }
                }

                //else error--DONE
                return "Undefined";
            }

            return "exit";
        }

        //IfStmt → if ( Expr ) Stmt IfStmt’
        private string Parse_IfStmt()
        {
            if (lookahad != "")
            {
                if (MatchToken("if", false) != "Undefined")
                {
                    MatchToken("(", true);
                    Parse_Expr();
                    MatchToken(")", true);
                    Parse_Stmt();
                    Parse_IfStmtP();
                    return "";
                }

                return "Undefined";
            }

            return "exit";
        }

        //IfStmt’ → else Stmt | eps
        private string Parse_IfStmtP()
        {
            if (lookahad != "")
            {
                if (MatchToken("else", false) != "Undefined")
                {
                    return Parse_Stmt();
                }

                //eps
                return "";
            }

            return "exit";
        }

        //ForStmt → for ( ForStmt’  ; Expr ; ForStmt’  ) Stmt
        private string Parse_ForStmt()
        {
            if (lookahad != "exit")
            {
                if (MatchToken("for", false) != "Undefined")
                {
                    MatchToken("(", true);
                    Parse_ForStmtP();
                    Parse_Expr();
                    MatchToken(";", true);
                    Parse_ForStmtP();
                    MatchToken(")", true);
                    Parse_Stmt();
                }

                return "";
            }

            return "exit";
        }

        //ForStmt’ → Expr | eps
        private void Parse_ForStmtP()
        {
            // if Expr
            if (lookahad != "")
            {
                Parse_Expr(); // else eps
            }
        }

        //Expr →  Expr’ Expr1
        private string Parse_Expr()
        {
            if (lookahad != "")
            {
                Parse_ExprP();
                return Parse_Expr1();
            }

            return "exit";
        }

        //Expr1 → || Expr’ Expr1 | eps
        private string Parse_Expr1()
        {
            if (lookahad != "")
            {
                if (MatchToken("||", false) != "Undefined")
                {
                    Parse_ExprP();
                    return Parse_Expr1();
                }

                //else eps
                return "";
            }

            return "exit";
        }

        //Expr’ → Expr2’ Expr2
        private string Parse_ExprP()
        {
            if (lookahad != "")
            {
                Parse_Expr2P();
                return Parse_Expr2();
            }

            return "exit";
        }

        //Expr2 → && Expr2’ Expr2 | eps
        private string Parse_Expr2()
        {
            if (lookahad != "")
            {
                if (MatchToken("&&", false) != "Undefined")
                {
                    Parse_Expr2P();
                    return Parse_Expr2();
                }

                return "";
            }

            return "exit";

        }

        //Expr2’ → Expr3’ Expr3
        private string Parse_Expr2P()
        {
            if (lookahad != "")
            {
                Parse_Expr3P();
                return Parse_Expr3();
            }

            return "exit";
        }


        //Expr3 → == Expr3’ Expr3 | != Expr3’ Expr3 | eps
        private string Parse_Expr3()
        {
            if (lookahad != "")
            {
                if (MatchToken("==", false) != "Undefined")
                {
                    Parse_Expr3P();
                    return Parse_Expr3();
                }
                else if (MatchToken("!=", false) != "Undefined")
                {
                    Parse_Expr3P();
                    return Parse_Expr3();
                }
                return "";
            }

            return "exit";
        }

        //Expr3’ → Expr4’ Expr4
        private string Parse_Expr3P()
        {
            if (lookahad != "")
            {
                Parse_Expr4P();
                return Parse_Expr4();
            }

            return "exit";
        }

        //Expr4 → < Expr4’ Expr4 | > Expr4’ Expr4 | <= Expr4’ Expr4 | >= Expr4’ Expr4 | eps
        private string Parse_Expr4()
        {
            if (lookahad != "")
            {
                if (MatchToken("<", false) != "Undefined")
                {
                    Parse_Expr4P();
                    return Parse_Expr4();
                }

                if (MatchToken(">", false) != "Undefined")
                {
                    Parse_Expr4P();
                    return Parse_Expr4();
                }

                if (MatchToken("<=", false) != "Undefined")
                {
                    Parse_Expr4P();
                    return Parse_Expr4();
                }

                if (MatchToken(">=", false) != "Undefined")
                {
                    Parse_Expr4P();
                    return Parse_Expr4();
                }

                //else 
                return "";
            }

            return "exit";
        }

        //Expr4’ →  Expr5’ Expr5
        private string Parse_Expr4P()
        {
            if (lookahad != "")
            {
                Parse_Expr5P();
                return Parse_Expr5();
            }

            return "exit";
        }

        //Expr5 → + Expr5’ Expr5 | - Expr5’ Expr5 | eps
        private string Parse_Expr5()
        {
            if (lookahad != "")
            {
                if (MatchToken("+", false) != "Undefined")
                {
                    Parse_Expr5P();
                    return Parse_Expr5();
                }
                else if (MatchToken("-", false) != "Undefined")
                {
                    Parse_Expr5P();
                    return Parse_Expr5();
                }

                return "";
            }

            return "exit";
        }

        //Expr5’ → Expr6’ Expr6
        private string Parse_Expr5P()
        {
            if (lookahad != "")
            {
                Parse_Expr6P();
                return Parse_Expr6();
            }

            return "exit";
        }

        //Expr6 → * Expr6’ Expr6 | / Expr6’ Expr6 | % Expr6’ Expr6 | eps
        private string Parse_Expr6()
        {
            if (lookahad != "")
            {
                if (MatchToken("*", false) != "Undefined")
                {
                    Parse_Expr6P();
                    return Parse_Expr6();
                }
                else if (MatchToken("/", false) != "Undefined")
                {
                    Parse_Expr6P();
                    return Parse_Expr6();
                }
                else if (MatchToken("%", false) != "Undefined")
                {
                    Parse_Expr6P();
                    return Parse_Expr6();
                }

                return "";
            }

            return "exit";
        }

        //Expr6’ → LValue = Expr |  Constant | LValue | this | - Expr | ! Expr | ( Expr ) |  New (ident) | eps
        private string Parse_Expr6P()
        {
            if (lookahad != "")
            {
                if (Parse_LValue() != "Undefined" && MatchToken("=", false) == "")
                {
                    return Parse_Expr();
                }
                else if (Parse_Constant() == "Undefined")
                {
                    if (Parse_LValue() == "Undefined")
                    {
                        if (MatchToken("this", false) == "Undefined")
                        {
                            if (MatchToken("-", false) != "Undefined")
                            {
                                Parse_Expr();
                            }
                            else if (MatchToken("!", false) != "Undefined")
                            {
                                Parse_Expr();
                            }
                            else if (MatchToken("(", false) != "Undefined")
                            {
                                Parse_Expr();
                                MatchToken(")", false);
                            }
                            else if (MatchToken("New", false) != "Undefined")
                            {
                                MatchToken("(", true);
                                MatchToken("identificador", true);
                                MatchToken(")", true);
                            }
                        }
                    }
                }

                return "";
            }

            return "exit";

        }

        //LValue → ident | Expr LValue’
        private string Parse_LValue()
        {
            if (lookahad != "")
            {
                if (MatchToken("identificador", false) == "Undefined" && lookahad != "intConstant" &&
                    lookahad != "doubleConstant" && lookahad != "boolConstant" && lookahad != "stringConstant")
                {
                    if (Parse_Expr() != "Undefined")
                    {
                        return Parse_LValueP();
                    }
                    else
                    {
                        return "Undefined";
                    }
                }

                return "";
            }

            return "exit";

        }

        //LValue’ → . ident | [ Expr ]
        private string Parse_LValueP()
        {
            if (lookahad != "")
            {
                if (MatchToken(".", false) != "Undefined")
                {
                    MatchToken("identificador", true);
                    return "";
                }
                else if (MatchToken("[", true) != "Undefined")
                {
                    Parse_Expr();
                    MatchToken("]", true);
                    return "";
                }

                return "Undefined";
            }

            return "exit";
        }

        //Constant → intConstant | doubleConstant | boolConstant | stringConstant | null
        private string Parse_Constant()
        {
            if (lookahad != "")
            {
                if (MatchToken("intConstant", false) == "Undefined")
                {
                    if (MatchToken("doubleConstant", false) == "Undefined")
                    {
                        if (MatchToken("boolConstant", false) == "Undefined")
                        {
                            if (MatchToken("stringConstant", false) == "Undefined")
                            {
                                return "";
                            }
                            else if (MatchToken("null", false) != "Undefined")
                            {
                                return "";
                            }
                        }
                        else
                        {
                            return "";
                        }
                    }
                    else
                    {
                        return "";
                    }
                }
                else
                {
                    return "";
                }

                return "Undefined";
            }

            return "exit";
        }
        #endregion
    }
}
