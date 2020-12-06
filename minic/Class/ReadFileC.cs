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

        //Method write the file and save
        public void WriteFile(List<Token> list, string PathF)
        {
            var NewPath = NewPath_File(PathF) + "out";

            var write = new StreamWriter(NewPath);

            foreach (var item in list)
            {
                write.WriteLine(item.ToString());
            }

            write.Close();
        }
        #endregion

        //---------------------------------------FUNCTIONS PRIVATE---------------------------------------

        #region Functions Private

        private string NewPath_File(string path)
        {
            var path_A = path.ToArray();
            var new_Path = string.Empty;
            var flag_point = false;

            for (int i = path_A.Length-1; i > -1 ; i--)
            {
                if (path_A[i] == '.' && flag_point == false)
                    flag_point = true;
                if (flag_point == true)
                    new_Path = new_Path + path_A[i];
            }

            var revers = new_Path.ToCharArray(); //hola --> aloh
            Array.Reverse(revers);
            return new string(revers);
        }
        #endregion
    }
}
