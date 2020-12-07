using minic.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using minic.Class.Fase_2;
using minic.Class.Fase_3;

namespace minic
{
    public partial class Form1 : Form
    {
        // ----------------------------------------VAR GLOBAL-------------------------------------------

        public string PathFile = string.Empty; // var path the file
        public List<Class.Token> FileScanner = new List<Class.Token>(); //list token

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
            var openFileDialog = new OpenFileDialog();

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
            //-------------------------------- PHASE 01
            Lexical_Analysis(); //begin
            //-------------------------------- LAB 01
            //ASD_Recursive();
            //-------------------------------- PHASE 2
            AS_LR1();
            //-------------------------------- PHASE 3
            Semantic_Analysis();
        }

        //---------------------------------------FUNCTIONS PUBLIC---------------------------------------

        #region Functions Public

        //method to do Lexical Analysis --> era privada
        public void Lexical_Analysis()
        {
            //instance class
            var rf = new ReadFileC();
            var s = new Scanner();

            //return config start
            button2.Enabled = false;
            textBox1.Enabled = false;

            //get data from the all file
            var res = rf.ReadFile(PathFile);

            //check if it is empty
            if (res.Length == 0)
                MessageBox.Show("El archivo se encontro vacio."); //end program
            else
            {
                //method for analysis
                FileScanner = s.Scanner_Lexic(res);

                var flag_error = false;
                var errors = string.Empty;
                //Show errors
                foreach (var item in FileScanner)
                {
                    if (item.Error != "")
                    {
                        errors += item.ToString();
                        flag_error = true;
                    }
                }

                //if (flag_error == false)
                //    MessageBox.Show("Se terminó de analizar el archivo", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //else
                //{
                //    MessageBox.Show(errors, "Error:", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //    MessageBox.Show("El archivo se termino de analizar.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //}
                //write the file
                rf.WriteFile(FileScanner,PathFile);

                MessageBox.Show("Se terminó de analizar el archivo", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion

        //---------------------------------------FUNCTIONS PRIVATE--------------------------------------

        #region Functions Private
        //Method to do Syntactic Analyis Decendant Recursive -->LAB A
        private void ASD_Recursive()
        {
            //Instance class
            ASDRecursivo asd = new ASDRecursivo();

            //Find Error syntactic analysis
            string Msg_Error = asd.ASDRecursivo_F(FileScanner);

            //Show Error Syntactic

            if (Msg_Error != "")
            {
                MessageBox.Show(Msg_Error, "Error ASDR:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Correct Sintaxis", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Method to do Syntactic Analyis  --> PHASE 2
        private void AS_LR1() 
        {
            var parseo = new Parseo();
            //Find Error syntactic analysis
            var Msg_Error = parseo.Tabla_parseo(FileScanner);

            //Show Error Syntactic

            if (Msg_Error != "")
            {
                MessageBox.Show(Msg_Error, "Error LR1:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Sintáxis correcta", "Información", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        //Method to do Semantic Analysis --> PHASE 3
        private void Semantic_Analysis()
        {
            Semantico s = new Semantico();
            string Msg_Error = s.Tabla_Simbolos(FileScanner, PathFile);

            //Show Error Syntactic

            if (Msg_Error != "")
            {
                MessageBox.Show(Msg_Error, "Error Semantico:", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                MessageBox.Show("Semántica correcta", "", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
        #endregion
    }
}
