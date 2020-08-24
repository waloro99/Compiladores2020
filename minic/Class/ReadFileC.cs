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
            StreamReader read = new StreamReader(PathF); //reader
            var txtAll = read.ReadToEnd(); //have all text
            return txtAll.Split(new[] { "\n\r", "\n", "\r" }, StringSplitOptions.RemoveEmptyEntries); //split line
        }

        #endregion


        #region WriteFile

        #endregion

        //---------------------------------------FUNCTIONS PRIVATE---------------------------------------

    }
}
