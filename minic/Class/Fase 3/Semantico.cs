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

        private string Flujo(List<Type> tokens) 
        {
            return null;
        }

        #endregion
    }
}
