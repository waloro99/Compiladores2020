using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minic.Class
{
    public class ReadFileC
    {
        //---------------------------------------FUNCTIONS PUBLIC---------------------------------------

        #region ReadFile

        //Method read file and save
        public string[] ReadFile(string PathF)
        {
            string NameFile = @"" + PathF; //path the file
            StreamReader read = new StreamReader(NameFile); //reader
            var txtAll = read.ReadToEnd(); //have all text
            string[] T_Lineas = txtAll.Split(new[] { "\n\r", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries); //split line
            return T_Lineas; //test function
        }

        #endregion

        //---------------------------------------FUNCTIONS PRIVATE---------------------------------------

    }
}
