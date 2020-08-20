using minic.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace minic
{
    public partial class Form1 : Form
    {
        // ----------------------------------------VAR GLOBAL-------------------------------------------

        public string PathFile = string.Empty; // var path the file

        //----------------------------------------- INTERFACE -----------------------------------------

        public Form1()
        {
            InitializeComponent();

            //put button
            button1.MouseClick += this.Press_button;

            //mouse move
            button1.MouseLeave += this.Quit_button;

            //instance
            Bitmap imagen = new Bitmap(Application.StartupPath + @"\img\ima_archivo1.png");
            button1.Image = imagen;

        }

        #region Button_Search

        //method for back imagen original 
        private void Quit_button(object obj, EventArgs evt)
        {
            Bitmap imagen = new Bitmap(Application.StartupPath + @"\img\ima_archivo1.png");
            button1.Image = imagen;
        }

        //method for change imagen dowload
        private void Press_button(object obj, EventArgs evt)
        {
            Bitmap imagen = new Bitmap(Application.StartupPath + @"\img\ima_archivo2.png");
            button1.Image = imagen;
        }

        #endregion

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text documents (.txt)|*.txt"; //only file .txt
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                PathFile = openFileDialog.FileName;
                button2.Enabled = true;
                textBox1.Enabled = true;
            }

            textBox1.Text = PathFile;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Lexical_Analysis(); //begin
        }

        //---------------------------------------FUNCTIONS PUBLIC---------------------------------------

        #region Functions Public

        //method to do Lexical Analysis
        public void Lexical_Analysis()
        {
            //instance class
            ReadFileC rf = new ReadFileC();
            Token t = new Token();

            //return config start
            button2.Enabled = false;
            textBox1.Enabled = false;

            //get data from the all file
            string[] res = rf.ReadFile(PathFile);

            if (res.Length == 0)
            {
                MessageBox.Show("El archivo se encontro vacio."); //end program
            }
            else
            {
                //method for analysis

                //codigo basura
               /* List<string> reserved = t.Reserved_Words();
                foreach (var item in reserved)
                {
                    MessageBox.Show(item);
                }*/ 


            }

        }


        #endregion

        //---------------------------------------FUNCTIONS PRIVATE--------------------------------------

        #region Functions Private

        #endregion


    }
}
