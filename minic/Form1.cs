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

        //button buscar
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            //openFileDialog.Filter = "Text documents (.txt)|*.txt"; //only file .txt
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                PathFile = openFileDialog.FileName;
                button2.Enabled = true;
                textBox1.Enabled = true;
            }

            textBox1.Text = PathFile;
        }

        //buton Aceptar
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
            Scanner s = new Scanner();

            //return config start
            button2.Enabled = false;
            textBox1.Enabled = false;

            //get data from the all file
            string[] res = rf.ReadFile(PathFile);

            //check if it is empty
            if (res.Length == 0)
                MessageBox.Show("El archivo se encontro vacio."); //end program
            else
            {
                //method for analysis
                List<Class.Type> FileScanner = s.Scanner_Lexic(res);
                bool flag_error = false;
                //Show errors
                foreach (var item in FileScanner)
                {
                    if (item.Error != "")
                    {
                        MessageBox.Show(item.ToString());
                        flag_error = true;
                    }
                       
                }

                if (flag_error == false)
                    MessageBox.Show("El archivo se analizó correctamente");
                //write the file
                rf.WriteFile(FileScanner,PathFile);
            }

        }


        #endregion

        //---------------------------------------FUNCTIONS PRIVATE--------------------------------------

        #region Functions Private
        
        #endregion


    }
}
