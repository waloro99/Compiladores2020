using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minic.Class.Fase_3
{
    public class Semantico
    {
        //--------------------------------------------- PUBLIC FUNCTIONS -----------------------------------------------------

        #region public functions

        //metodo publico para hacer el parseo
        public string Tabla_Simbolos(List<Type> tokens)
        {
            string error = "";
            error = Flujo(tokens);
            return error;
        }
        #endregion

        //--------------------------------------------- PRIVATE FUNCTIONS -----------------------------------------------------

        #region private functions

        //Flujo que llevara para la lectura de datos
        private string Flujo(List<Type> tokens) 
        {
            return "";
        }

        //Metodo
        private void MatchRule(Type dato) 
        {
            switch (dato.description)
            {
                case "Constant":
                    Match_Constant();
                    break;
                case "LValue":
                    Match_LValue();
                    break;
                case "Expr":
                    Match_Expr();
                    break;
                case "Expr2":
                    Match_Expr2();
                    break;
                case "PrintStmt":
                    Match_PrintStmt();
                    break;
                case "BreakStmt":
                    Match_BreakStmt();
                    break;
                case "ReturnStmt":
                    Match_ReturnStmt();
                    break;
                case "IfStmt2":
                    Match_IfStmt2();
                    break;
                case "IfStmt":
                    Match_IfStmt();
                    break;
                case "Expr1":
                    Match_Expr1();
                    break;
                case "Actuals":
                    Match_Actuals();
                    break;
                case "CallStmt":
                    Match_CallStmt();
                    break;
                case "Stmt":
                    Match_Stmt();
                    break;
                case "Stmt2":
                    Match_Stmt2();
                    break;
                case "ConstDecl2":
                    Match_ConstDecl2();
                    break;
                case "VariableDecl2":
                    Match_VariableDecl2();
                    break;
                case "StmtBlock":
                    Match_StmtBlock();
                    break;
                case "Prototype":
                    Match_Prototype();
                    break;
                case "Prototype2":
                    Match_Prototype2();
                    break;
                case "InterfaceDecl":
                    Match_InterfaceDecl();
                    break;
                case "Field":
                    Match_Field();
                    break;
                case "Field2":
                    Match_Field2();
                    break;
                case "ClassDecl3":
                    Match_ClassDecl3();
                    break;
                case "ClassDecl2":
                    Match_ClassDecl2();
                    break;
                case "ClassDecl1":
                    Match_ClassDecl();
                    break;
                case "Formals":
                    Match_Formals();
                    break;
                case "FunctionDecl":
                    Match_FunctionDecl();
                    break;
                case "Type":
                    Match_Type();
                    break;
                case "ConstType":
                    Match_ConstType();
                    break;
                case "ConstDecl":
                    Match_ConstDecl();
                    break;
                case "Variable":
                    Match_Variable();
                    break;
                case "VariableDecl":
                    Match_VariableDecl();
                    break;
                case "Decl":
                    Match_Decl();
                    break;
                case "Decl2":
                    Match_Decl2();
                    break;
                case "Program":
                    Match_Program();
                    break;
                case "Program'":
                    Match_ProgramP();
                    break;
                default:
                    break;
            }
        }

        //Reglas Semanticas
        #region Reglas Semanticas

        private void Match_Constant() 
        {
        
        }

        private void Match_LValue()
        {

        }
        private void Match_Expr()
        {

        }

        private void Match_Expr2()
        {

        }
        private void Match_PrintStmt()
        {

        }

        private void Match_BreakStmt()
        {

        }

        private void Match_ReturnStmt()
        {

        }
        private void Match_IfStmt2()
        {

        }
        private void Match_IfStmt()
        {

        }
        private void Match_Expr1()
        {

        }
        private void Match_Actuals()
        {

        }
        private void Match_CallStmt()
        {

        }
        private void Match_Stmt()
        {

        }
        private void Match_Stmt2()
        {

        }
        private void Match_ConstDecl2()
        {

        }
        private void Match_VariableDecl2()
        {

        }
        private void Match_StmtBlock()
        {

        }
        private void Match_Prototype()
        {

        }
        private void Match_Prototype2()
        {

        }
        private void Match_InterfaceDecl()
        {

        }
        private void Match_Field()
        {

        }
        private void Match_Field2()
        {

        }
        private void Match_ClassDecl3()
        {

        }
        private void Match_ClassDecl2()
        {

        }
        private void Match_ClassDecl()
        {

        }
        private void Match_Formals()
        {

        }
        private void Match_FunctionDecl()
        {

        }
        private void Match_Type()
        {

        }
        private void Match_ConstType()
        {

        }
        private void Match_ConstDecl()
        {

        }
        private void Match_Variable()
        {

        }
        private void Match_VariableDecl()
        {

        }
        private void Match_Decl()
        {

        }
        private void Match_Decl2()
        {

        }
        private void Match_Program()
        {

        }
        private void Match_ProgramP()
        {

        }

        #endregion

        #endregion
    }
}
