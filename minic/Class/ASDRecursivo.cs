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
            Parse_Program();
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

        }

        //Decl’ → VariableDecl Decl’ | FunctionDecl Decl’ | eps
        private void Parse_DeclP()
        {

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
            //else error
        }

        //IfStmt → if ( Expr ) Stmt IfStmt’
        private void Parse_IfStmt()
        {

        }

        //IfStmt’ → else Stmt | eps
        private void Parse_IfStmtP()
        {

        }

        //ForStmt → for ( ForStmt’  ; Expr ; ForStmt’  ) Stmt
        private void Parse_ForStmt()
        {

        }

        #endregion
    }
}
