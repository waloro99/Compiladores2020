using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace minic.Class.Fase_3
{
    public class Semantico
    {
        //--------------------------------------------- PUBLIC FUNCTIONS -----------------------------------------------------

        #region public functions
        Arbol x = new Arbol();

        //metodo publico para hacer el parseo
        public string Tabla_Simbolos(List<Token> tokens, string path)
        {
            var error = Flujo(tokens, path);
            return error;
        }
        #endregion

        //--------------------------------------------- PRIVATE FUNCTIONS -----------------------------------------------------

        #region private functions

        //Flujo que llevara para la lectura de datos
        private string Flujo(List<Token> tokens, string path) 
        {
            var error = string.Empty;

            if (x.Raiz != null)
            {
                foreach (var item in x.Raiz.Lista_Nodos)
                {
                    MatchRule(item);
                }

                foreach (var item in tokens)
                {
                    x.Recorrido(x.Raiz, item);
                }
            }

            EscribirTabla(tokens, path);

            return error;
        }

        private void MatchRule(TablaS dato) 
        {
            switch (dato.token.description)
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
            }
        }

        #region Reglas Semanticas

        private void Match_Constant() 
        {
            string opc = "";

            switch (opc)
            {
                case "intConstant":
                    break;
                case "doubleConstant":
                    break;
                case "boolConstant":
                    break;
                case "stringConstant":
                    break;
                case "null":
                    break;
            }
        }

        private void Match_LValue()
        {
            string opc = "";
            switch (opc)
            {
                case "ident":
                    break;
                case "Expr":
                    break;
            }
        }
        private void Match_Expr()
        {
            string opc = "";

            switch (opc)
            {
                case "Constant":
                    break;
                case "LValue":
                    break;
                case "this":
                    break;
                case "(":
                    break;
                case "+":
                    break;
                case "*":
                    break;
                case "%":
                    break;
                case "-":
                    break;
                case "<":
                    break;
                case ">":
                    break;
                case "<=":
                    break;
                case ">=":
                    break;
                case "==":
                    break;
                case "&&":
                    break;
                case "!":
                    break;
                case "New":
                    break;
            }
        }

        private void Match_Expr2()
        {
            string opc = "";
            switch (opc)
            {
                case ",":
                    break;
            }

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
            string opc = "";
            switch (opc)
            {
                case ",":
                    break;
                default:
                    break;
            }
        }
        private void Match_IfStmt()
        {

        }
        private void Match_Expr1()
        {
            string opc = "";
            switch (opc)
            {
                case "Expr":
                    break;
                default:
                    break;
            }
        }
        private void Match_Actuals()
        {
            string opc = "";
            switch (opc)
            {
                case ",":
                    break;
                default:
                    break;
            }
        }
        private void Match_CallStmt()
        {
            string opc = "";
            switch (opc)
            {
                case "ident":
                    break;
                default:
                    break;
            }
        }
        private void Match_Stmt()
        {
            string opc = "";
            switch (opc)
            {
                case "CallStmt":
                    break;
                case "StmtBlock":
                    break;
                case "PrintStmt":
                    break;
                case "ReturnStmt":
                    break;
                case "BreakStmt":
                    break;
                case "ForStmt":
                    break;
                case "WhileStmt":
                    break;
                case "IfStmt":
                    break;
                case "Expr1":
                    break;
                default:
                    break;
            }
        }
        private void Match_Stmt2()
        {
            string opc = "";
            switch (opc)
            {
                case "Stmt":
                    break;
                default:
                    break;
            }
        }
        private void Match_ConstDecl2()
        {
            string opc = "";
            switch (opc)
            {
                case "ConstDecl":
                    break;
                default:
                    break;
            }
        }
        private void Match_VariableDecl2()
        {
            string opc = "";
            switch (opc)
            {
                case "VariableDecl":
                    break;
                default:
                    break;
            }
        }
        private void Match_StmtBlock()
        {

        }
        private void Match_Prototype()
        {
            string opc = "";
            switch (opc)
            {
                case "void":
                    break;
                default:
                    break;
            }
        }
        private void Match_Prototype2()
        {
            string opc = "";
            switch (opc)
            {
                case "Prototype":
                    break;
                default:
                    break;
            }
        }
        private void Match_InterfaceDecl()
        {

        }
        private void Match_Field()
        {
            string opc = "";
            switch (opc)
            {
                case "ConstDecl":
                    break;
                case "FuncionDecl":
                    break;
                case "VariableDecl":
                    break;
                    break;
                default:
                    break;
            }
        }
        private void Match_Field2()
        {
            string opc = "";
            switch (opc)
            {
                case "Field":
                    break;
                default:
                    break;
            }
        }
        private void Match_ClassDecl3()
        {
            string opc = "";
            switch (opc)
            {
                case "ident":
                    break;
                case ",":
                    break;
                default:
                    break;
            }
        }
        private void Match_ClassDecl2()
        {
            string opc = "";
            switch (opc)
            {
                case ":":
                    break;
                default:
                    break;
            }
        }
        private void Match_ClassDecl()
        {

        }
        private void Match_Formals()
        {
            string opc = "";
            switch (opc)
            {
                case ",":
                    break;
                default:
                    break;
            }
        }
        private void Match_FunctionDecl()
        {
            string opc = "";
            switch (opc)
            {
                case "void":
                    break;
                default:
                    break;
            }
        }
        private void Match_Type()
        {
            string opc = "";
            switch (opc)
            {
                case "int":
                    break;
                case "double":
                    break;
                case "bool":
                    break;
                case "string":
                    break;
                case "ident":
                    break;
                case "Type":
                    break;
                default:
                    break;
            }
        }
        private void Match_ConstType()
        {
            string opc = "";
            switch (opc)
            {
                case "int":
                    break;
                case "double":
                    break;
                case "bool":
                    break;
                case "string":
                    break;
                default:
                    break;
            }
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
            string opc = "";
            switch (opc)
            {
                case "InterfaceDecl":
                    break;
                case "ClassDecl":
                    break;
                case "ConstDecl":
                    break;
                case "FunctionDecl":
                    break;
                case "VariableDecl":
                    break;
                default:
                    break;
            }

        }
        private void Match_Decl2()
        {
            string opc = "";
            switch (opc)
            {
                case "Decl":
                    break;
                default:
                    break;
            }
        }
        private void Match_Program()
        {

        }
        private void Match_ProgramP()
        {

        }

        #endregion


        private void EscribirTabla(List<Token> tokens, string path)
        { 
            var tabla = new List<TablaS>();

            foreach (var item in tokens)
            {
                var n = new TablaS(item);
                n.type = "int";

                if (n.token.description == "T_Identifier")
                {
                    tabla.Add(n);
                }
            }

            var NewPath = NewPath_File(path) + "tabla";

            var write = new StreamWriter(NewPath);

            foreach (var item in tabla)
            {
                write.WriteLine(item.ToString());
            }
            write.Close();
        }

        private string NewPath_File(string path)
        {
            var path_A = path.ToArray();
            var new_Path = string.Empty;
            var flag_point = false;

            for (int i = path_A.Length - 1; i > -1; i--)
            {
                if (path_A[i] == '.' && flag_point == false)
                    flag_point = true;
                if (flag_point)
                    new_Path = new_Path + path_A[i];
            }
            
            var revers = new_Path.ToCharArray(); //hola --> aloh
            Array.Reverse(revers);
            return new string(revers);
        }
        #endregion
    }
}