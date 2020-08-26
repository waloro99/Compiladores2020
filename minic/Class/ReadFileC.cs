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
        public void WriteFile(List<Type> list, string PathF)
        {
            string NewPath = NewPath_File(PathF);
            NewPath = NewPath + "out";
            TextWriter write = new StreamWriter(NewPath);
            foreach (var item in list)
            {
                write.WriteLine(item.ToString());//override
            }
            write.Close(); //close the file
        }
        #endregion

        //---------------------------------------FUNCTIONS PRIVATE---------------------------------------

        #region Functions Private

        private string NewPath_File(string path)
        {
            char[] path_A = path.ToArray();
            string new_Path = string.Empty;
            bool flag_point = false;
            for (int i = path_A.Length-1; i > -1 ; i--)
            {
                if (path_A[i] == '.' && flag_point == false)
                    flag_point = true;
                if (flag_point == true)
                    new_Path = new_Path + path_A[i];
            }
            char[] revers = new_Path.ToCharArray(); //hola --> aloh
            Array.Reverse(revers);
            return new string(revers);
        }

        #endregion

    }
}
