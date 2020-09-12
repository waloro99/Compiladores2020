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
        public List<Type> t = new List<Type>(); //list with tokens
        public Type token = new Type(); //value the lookahad
        public string lookahad = string.Empty; //var global
        public string Msg_Error = string.Empty;
        public int x = -1; //scroll var

        public bool flagError = false;

        #region FUNCTIONS PUBLICS

        //method public 
        public string ASDRecursivo_F(List<Type> tokens)
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
            //save type 
            string l = F_Lookahad(token);
            return l;
        }

        private string F_Lookahad(Type T_token)
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
                return null;
            }
        }

        //Method for know match the token
        private string MatchToken(string token, bool sTerminal)
        {
            if (lookahad == token)
            {
                lookahad = NextToken();
                return "";
            }
            //ERROR
            if (sTerminal)
            {
                Msg_Error += "\n" + lookahad;
                lookahad = NextToken();
                return "";
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
                    Parse_DeclP();
                    Msg_Error += lookahad;
                    lookahad = NextToken();
                    
                }
            }
            
            //else

            //Parse_DeclP();
        }

        //Decl’ → VariableDecl Decl’ | FunctionDecl Decl’ | eps
        private string Parse_DeclP()
        {
            if (Parse_VariableDecl() != "Undefined")
            {
                return Parse_DeclP();
            }

            if (Parse_FunctionDecl() != "Undefined")
            {
                return Parse_DeclP();
            }

            return "";
        }

        //VariableDecl  → Variable ;
        private string Parse_VariableDecl()
        {
            if (Parse_Variable() != "Undefined")
            {
                if (lookahad != "(" && lookahad != ")")
                {
                    return MatchToken(";", true);
                }
                return MatchToken(";", false);
            }

            return "Undefined";

        }

        //Variable → Type ident
        private string Parse_Variable()
        {
            if (Parse_Type() != "Undefined")
            {
                return MatchToken("identificador", true); //change name
            }
            return "Undefined";
        }

        //Type → Type’ Type2
        private string Parse_Type()
        {
            if (Parse_TypeP() == "Undefined")
            {
                return "Undefined";
            }
            if (Parse_Type2() == "Undefined")
            {
                return "";
            }
            return "";
        }

        //Type’ → int | double | bool | string | ident
        private string Parse_TypeP()
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

        //Type2 → [] Type2 | eps
        private string Parse_Type2()
        {
            return MatchToken("[]", false);
        }

        //FunctionDecl → Type ident ( Formals ) Stmt’ | void ident ( Formals ) Stmt’
        private string Parse_FunctionDecl()
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

        //Stmt’ → Stmt Stmt’ | eps
        private string Parse_StmtP()
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

            //else eps
        }

        //Formals → Variable Variable’ , | eps
        private string Parse_Formals()
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

        ////Variable’ →  Variable Variable’ | eps
        private string Parse_VariableP()
        {
            if (Parse_Variable() == "Undefined")
            {
                return "Undefined";
            }

            return "";
            //else eps
        }

        //Stmt → IfStmt | ForStmt | Expr ;
        private string Parse_Stmt()
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

        //IfStmt → if ( Expr ) Stmt IfStmt’
        private string Parse_IfStmt()
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

        //IfStmt’ → else Stmt | eps
        private string Parse_IfStmtP()
        {
            if (MatchToken("else", false) != "Undefined")
            {
                return Parse_Stmt();
            }
            //eps
            return "";
        }

        //ForStmt → for ( ForStmt’  ; Expr ; ForStmt’  ) Stmt
        private string Parse_ForStmt()
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

        //ForStmt’ → Expr | eps
        private void Parse_ForStmtP()
        {
            // if Expr
            Parse_Expr();
            // else eps
        }

        //Expr →  Expr’ Expr1
        private string Parse_Expr()
        {
            Parse_ExprP();
            return Parse_Expr1();

        }

        //Expr1 → || Expr’ Expr1 | eps
        private string Parse_Expr1()
        {
            if (MatchToken("||", false) != "Undefined")
            {
                Parse_ExprP();
                return Parse_Expr1();
            }
            //else eps
            return "";
        }

        //Expr’ → Expr2’ Expr2
        private string Parse_ExprP()
        {
            Parse_Expr2P();
            return Parse_Expr2();
        }

        //Expr2 → && Expr2’ Expr2 | eps
        private string Parse_Expr2()
        {
            if (MatchToken("&&", false) != "Undefined")
            {
                Parse_Expr2P();
                return Parse_Expr2();
            }

            return "";
            //else eps
        }

        //Expr2’ → Expr3’ Expr3
        private string Parse_Expr2P()
        {
            Parse_Expr3P();
            return Parse_Expr3();
        }

        //Expr3 → == Expr3’ Expr3 | != Expr3’ Expr3 | eps
        private string Parse_Expr3()
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
            //else 
            return "";
            //else eps
        }

        //Expr3’ → Expr4’ Expr4
        private string Parse_Expr3P()
        {
            Parse_Expr4P();
            return Parse_Expr4();
        }

        //Expr4 → < Expr4’ Expr4 | > Expr4’ Expr4 | <= Expr4’ Expr4 | >= Expr4’ Expr4 | eps
        private string Parse_Expr4()
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

        //Expr4’ →  Expr5’ Expr5
        private string Parse_Expr4P()
        {
            Parse_Expr5P();
            return Parse_Expr5();
        }

        //Expr5 → + Expr5’ Expr5 | - Expr5’ Expr5 | eps
        private string Parse_Expr5()
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

        //Expr5’ → Expr6’ Expr6
        private string Parse_Expr5P()
        {
            Parse_Expr6P();
            return Parse_Expr6();
        }

        //Expr6 → * Expr6’ Expr6 | / Expr6’ Expr6 | % Expr6’ Expr6 | eps
        private string Parse_Expr6()
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

        //Expr6’ → LValue = Expr |  Constant | LValue | this | - Expr | ! Expr | ( Expr ) |  New (ident) | eps
        private string Parse_Expr6P()
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
            //else eps

        }

        //LValue → ident | Expr LValue’
        private string Parse_LValue()
        {
            if (MatchToken("identificador", false) == "Undefined" && lookahad != "intConstant" && lookahad != "doubleConstant" && lookahad != "boolConstant" && lookahad != "stringConstant")
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

        //LValue’ → . ident | [ Expr ]
        private string Parse_LValueP()
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

        //Constant → intConstant | doubleConstant | boolConstant | stringConstant | null
        private string Parse_Constant()
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
        #endregion
    }
}
