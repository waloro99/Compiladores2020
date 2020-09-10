using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minic.Class
{
    public class ASDRecursivo
    {
        //---------------------------------------FUNCTIONS PUBLIC---------------------------------------
        public List<Type> t = new List<Type>(); //list witch tokens
        public string lookahad = string.Empty; //var global
        public string Msg_Error = string.Empty;

        #region FUNCTIONS PUBLICS

        //method public 
        public List<Type> ASDRecursivo_F(List<Type> tokens)
        {
            t = tokens;
            ASDR_Flow();
            return t;
        }

        #endregion

        //---------------------------------------FUNCTIONS PRIVATE--------------------------------------

        #region FUNCTIONS PRIVATE

        //Method to go through the program by asd ASDR.
        private void ASDR_Flow()
        {
            //lookahad
            F_lookahad();
            //begin gramatic
            Parse_Program();
        }

        //Method to get the lookahad
        private void F_lookahad()
        {

        }

        //Method for know next token
        private string NextToken()
        {
            return null;
        }

        //Method for know match the token
        private void MatchToken(string token)
        {
            if (lookahad ==  token)
            {
                lookahad = NextToken();
            }
            else
            {
                //ERROR
            }
        }

        //Program → Decl
        private void Parse_Program()
        {
            Parse_Decl();
        }

        //Decl → VariableDecl Decl’ | FunctionDecl Decl’
        private void Parse_Decl()
        {
            Parse_VariableDecl();
            Parse_DeclP();
            //else
            Parse_FunctionDecl();
            Parse_DeclP();
        }

        //Decl’ → VariableDecl Decl’ | FunctionDecl Decl’ | eps
        private void Parse_DeclP()
        {
            Parse_VariableDecl();
            Parse_DeclP();
            //else
            Parse_FunctionDecl();
            Parse_DeclP();
            //else eps
        }

        //VariableDecl  → Variable ;
        private void Parse_VariableDecl()
        {
            Parse_Variable();
            MatchToken(";");
        }

        //Variable → Type ident
        private void Parse_Variable()
        {
            Parse_Type();
            MatchToken("identificador"); //change name
        }

        //Type → Type’ Type2
        private void Parse_Type()
        {
            Parse_TypeP();
            Parse_Type2();
        }

        //Type’ → int | double | bool | string | ident
        private void Parse_TypeP()
        {
            if (lookahad == "int")
                MatchToken("int");
            else if (lookahad == "double")
                MatchToken("double");
            else if (lookahad == "bool")
                MatchToken("bool");
            else if (lookahad == "string")
                MatchToken("string");
            else if (lookahad == "identificador")
                MatchToken("identificador");
            //else error
        }

        //Type2 → [] Type2 | eps
        private void Parse_Type2()
        {
            if (lookahad == "[]")
            {
                MatchToken("[]");
                Parse_Type2();
            }
            else
            {
                //eps
            }
         
        }

        //FunctionDecl → Type ident ( Formals ) Stmt’ | void ident ( Formals ) Stmt’
        private void Parse_FunctionDecl()
        {
            if (lookahad != "void")
            {
                Parse_Type();
                MatchToken("identificador");
                MatchToken("(");
                Parse_Formals();
                MatchToken(")");
                Parse_StmtP();
            }
            else if (lookahad == "void")
            {
                MatchToken("void");
                MatchToken("identificador");
                MatchToken("(");
                Parse_Formals();
                MatchToken(")");
                Parse_StmtP();
            }
            else
            {
                //error
            }

        }

        //Stmt’ → Stmt Stmt’ | eps
        private void Parse_StmtP()
        {
            Parse_Stmt();
            Parse_StmtP();
            //else eps
        }

        //Formals → Variable Variable’ , | eps
        private void Parse_Formals()
        {
            Parse_Variable();
            Parse_VariableP();
            MatchToken(",");
            //else eps
        }

        //Variable’ →  Variable Variable’ | eps
        private void Parse_VariableP()
        {
            Parse_Variable();
            Parse_VariableP();
            //else eps
        }

        //Stmt → IfStmt | ForStmt | Expr ;
        private void Parse_Stmt()
        {
            //if ifstmt
            Parse_IfStmt();
            //else if forstms
            Parse_ForStmt();
            //else expr
            Parse_Expr();
            MatchToken(";");
            //else error
        }

        //IfStmt → if ( Expr ) Stmt IfStmt’
        private void Parse_IfStmt()
        {
            MatchToken("if");
            MatchToken("(");
            Parse_Expr();
            MatchToken(")");
            Parse_Stmt();
            Parse_IfStmtP();
        }

        //IfStmt’ → else Stmt | eps
        private void Parse_IfStmtP()
        {
            if (lookahad == "else")
            {
                MatchToken("else");
                Parse_Stmt();
            }
            //eps
        }

        //ForStmt → for ( ForStmt’  ; Expr ; ForStmt’  ) Stmt
        private void Parse_ForStmt()
        {
            MatchToken("for");
            MatchToken("(");
            Parse_ForStmtP();
            MatchToken(";");
            Parse_Expr();
            MatchToken(";");
            Parse_ForStmtP();
            MatchToken(")");
            Parse_Stmt();
        }

        //ForStmt’ → Expr | eps
        private void Parse_ForStmtP()
        {
            // if Expr
            Parse_Expr();
            // else eps
        }

        //Expr →  Expr’ Expr1
        private void Parse_Expr()
        {
            Parse_ExprP();
            Parse_Expr1();
        }

        //Expr1 → || Expr’ Expr1 | eps
        private void Parse_Expr1()
        {
            MatchToken("||");
            Parse_ExprP();
            Parse_Expr1();
            //else eps
        }

        //Expr’ → Expr2’ Expr2
        private void Parse_ExprP()
        {
            Parse_Expr2P();
            Parse_Expr2();
        }

        //Expr2 → && Expr2’ Expr2 | eps
        private void Parse_Expr2()
        {
            MatchToken("&&");
            Parse_Expr2P();
            Parse_Expr2();
            //else eps
        }

        //Expr2’ → Expr3’ Expr3
        private void Parse_Expr2P()
        {
            Parse_Expr3P();
            Parse_Expr3();
        }

        //Expr3 → == Expr3’ Expr3 | != Expr3’ Expr3 | eps
        private void Parse_Expr3()
        {
            MatchToken("==");
            Parse_Expr3P();
            Parse_Expr3();
            //else 
            MatchToken("!=");
            Parse_Expr3P();
            Parse_Expr3();
            //else eps
        }

        //Expr3’ → Expr4’ Expr4
        private void Parse_Expr3P()
        {
            Parse_Expr4P();
            Parse_Expr4();
        }

        //Expr4 → < Expr4’ Expr4 | > Expr4’ Expr4 | <= Expr4’ Expr4 | >= Expr4’ Expr4 | eps
        private void Parse_Expr4()
        {
            MatchToken("<");
            Parse_Expr4P();
            Parse_Expr4();
            //else 
            MatchToken(">");
            Parse_Expr4P();
            Parse_Expr4();
            //else 
            MatchToken("<=");
            Parse_Expr4P();
            Parse_Expr4();
            //else 
            MatchToken(">=");
            Parse_Expr4P();
            Parse_Expr4();
            //else eps
        }

        //Expr4’ →  Expr5’ Expr5
        private void Parse_Expr4P()
        {
            Parse_Expr5P();
            Parse_Expr5();
        }

        //Expr5 → + Expr5’ Expr5 | - Expr5’ Expr5 | eps
        private void Parse_Expr5()
        {
            MatchToken("+");
            Parse_Expr5P();
            Parse_Expr5();
            //else 
            MatchToken("-");
            Parse_Expr5P();
            Parse_Expr5();
            //else eps
        }

        //Expr5’ → Expr6’ Expr6
        private void Parse_Expr5P()
        {
            Parse_Expr6P();
            Parse_Expr6();
        }

        //Expr6 → * Expr6’ Expr6 | / Expr6’ Expr6 | % Expr6’ Expr6 | eps
        private void Parse_Expr6()
        {
            MatchToken("*");
            Parse_Expr6P();
            Parse_Expr6();
            //else 
            MatchToken("/");
            Parse_Expr6P();
            Parse_Expr6();
            //else 
            MatchToken("%");
            Parse_Expr6P();
            Parse_Expr6();
            //else eps
        }

        //Expr6’ → LValue = Expr |  Constant | LValue | this | - Expr | ! Expr | ( Expr ) |  New (ident) | eps
        private void Parse_Expr6P()
        {
            Parse_LValue();
            MatchToken("=");
            Parse_Expr();
            //else
            Parse_Constant();
            //else
            Parse_LValue();
            //else
            MatchToken("this");
            //else
            MatchToken("-");
            Parse_Expr();
            //else
            MatchToken("!");
            Parse_Expr();
            //else
            MatchToken("(");
            Parse_Expr();
            MatchToken(")");
            //else
            MatchToken("New");
            MatchToken("(");
            MatchToken("identificador");
            MatchToken(")");
            //else eps

        }

        //LValue → ident | Expr LValue’
        private void Parse_LValue()
        {
            MatchToken("identificador");
            //else
            Parse_Expr();
            Parse_LValueP();
        }

        //LValue’ → . ident | [ Expr ]
        private void Parse_LValueP()
        {
            MatchToken(".");
            MatchToken("identificador");
            //else
            MatchToken("[");
            Parse_Expr();
            MatchToken("]");
        }

        //Constant → intConstant | doubleConstant | boolConstant | stringConstant | null
        private void Parse_Constant()
        {
            MatchToken("intConstant");
            //else
            MatchToken("doubleConstant");
            //else
            MatchToken("boolConstant");
            //else
            MatchToken("stringConstant");
            //else eps
        }

        #endregion
    }
}
