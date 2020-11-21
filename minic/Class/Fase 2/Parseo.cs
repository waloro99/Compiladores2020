﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace minic.Class.Fase_2
{
    public class Parseo
    {

        public Stack<int> pila = new Stack<int>(); // la pila guarda los estados de la tabla de analisis
        public List<Type> simbolo = new List<Type>(); // aqui guarda los datos que se van ingresando
        public Stack<string> accion = new Stack<string>(); //se va guardando las acciones que va realizando los parseos
        public string errores; //guarda los valores encontrados
        public Queue<Type> entrada =  new Queue<Type>();
        public List<string> listOpciones = new List<string>(); //opciones por si hay conflictos en los estados de la tabla de LR1
        public Queue<string> OpcionesCola = new Queue<string>();//para mover dentro de las estructuras
        //banderas para controlar el flujo
        bool f_inicio = true, f_desplazar = false, f_reducir = false, f_irA = false, f_aceptar = false, f_error = false,f_opc = false;

        //--------------------------------------------- PUBLIC FUNCTIONS -----------------------------------------------------

        #region public functions

        //metodo publico para hacer el parseo
        public string Tabla_parseo(List<Type> tokens)
        {
            string error = "";
            error = Flujo(tokens);
            return error;
        }
        #endregion

        //--------------------------------------------- PRIVATE FUNCTIONS -----------------------------------------------------

        #region private functions

        //metodo para llevar el flujo del parseo
        private string Flujo(List<Type> tokens)
        {
            string error = "";
            //Agrego el ultimo token para saber fin de la cadena que seria $
            tokens = AddLastString(tokens);
            //agrega a la pila el estado cero
            pila.Push(0);

            //llenado de cola con tokens
            entrada = LlenarCola(tokens);
            //vamos a utilizar la lista como la entrada
            while (entrada.Count > 0)
            {
                int p;  //obtenemos el top de la pila
                string acc = ""; //accion guarda el dato enviado por el metodo
                string[] datos = new string[2]; //movimiento, estado

                if (f_inicio == true && f_error == false)
                {
                    p = pila.Peek(); //obtenemos el top de la pila
                    acc = MetodoFalso(p, entrada.Peek());
                    datos = SplitAction(acc);//obtiene accion y estado
                    f_inicio = false;//se termino la primera lectura
                    accion.Push(datos[0]); //guardo la accion
                    FlagAccion(datos[0]); //activo la accion
                }
                else if (f_reducir == true && f_error == false)
                {
                    //se sabe que cuando hay una reduccion se continua un ir_A
                    f_reducir = false;
                    p = pila.Peek(); //obtenemos el top de la pila
                    Type a = Ir_A();
                    acc = MetodoFalso(p, a); //obtenemos el ultimo valor de Simbolo
                    datos = SplitAction(acc);//obtiene accion y estado
                    accion.Push(datos[0]); //guardo la accion
                    FlagAccion(datos[0]); //activo la accion en este caso deberia de enviar IR_A
                }
                else if (f_opc == true && f_error == false)
                {
                    datos = SplitAction(OpcionesCola.Dequeue());//obtiene accion y estado
                    accion.Push(datos[0]); //guardo la accion
                    FlagAccion(datos[0]); //activo la accion en este caso deberia de enviar IR_A
                }
                else if(f_error == false)
                {
                    //depende de lo que pase despues durante el programa 
                    p = pila.Peek(); //obtenemos el top de la pila
                    acc = MetodoFalso(p, entrada.Peek());
                    datos = SplitAction(acc);//obtiene accion y estado
                    accion.Push(datos[0]); //guardo la accion
                    FlagAccion(datos[0]); //activo la accion
                }

                //Se decide que hacer dependiendo de la bandera activada
                if (f_desplazar == true && f_error == false)
                {
                    pila.Push(Convert.ToInt16(datos[1])); //guardo el estado
                    simbolo.Add(entrada.Dequeue());//pasar el dato de entrada a simbolo
                    f_desplazar = false; //quito bandera
                }

                else if (f_reducir == true && f_error == false)
                {
                    int r = Reducir(Convert.ToInt16(datos[1])); // cambiamos la lista simbolo y nos devuelve cuantos pop hacer
                    for (int i = 0; i < r; i++)
                    {
                        pila.Pop(); //hace los pop necesarios para la reduccion 
                    }
                }

                else if (f_irA == true && f_error == false)
                {
                    pila.Push(Convert.ToInt16(datos[1])); //guardo el estado 
                    f_irA = false;
                }

                else if (f_aceptar == true && f_error == false)
                {
                    //Significa que termino todo bien solo hay que verificar despues del while que no exista otra cadena
                    entrada.Dequeue(); //salida de $
                }

                else if (f_error == true && f_opc == false)
                {
                    Type n_error = new Type();
                    n_error = entrada.Peek();
                    error = "Linea: "+ n_error.linea +" Columna: "+ n_error.column_I +" - "+ n_error.column_F + " Simbolo: "+ n_error.cadena +"    Error: No se esperaba este simbolo.";
                    entrada.Clear();
                }
                else if (f_opc == true)
                {
                    if (OpcionesCola.Count != 0)
                    {
                        //agregar metodo nuevo
                    }
                    else
                    {
                        f_opc = false;
                        f_error = true;
                    }
                }

            }

            return error;

        }

        //metodo para agregar un nuevo token a la lista que se utilizara despues
        private List<Type> AddLastString(List<Type> tokens)
        {
            List<Type> res = tokens;
            Type newType = new Type();
            newType.cadena = "$";
            newType.linea = 0;
            newType.column_I = 0;
            newType.column_F = 0;
            newType.Error = "";
            newType.description = "$";
            res.Add(newType);
            return res;

        }

        //Metodo para llenar la cola de las entradas
        private Queue<Type> LlenarCola(List<Type> tokens)
        {
            Queue<Type> res = new Queue<Type>();
            foreach (var item in tokens)
            {
                res.Enqueue(item);
            }
            return res;
        }

        //metodo que devuelve los datos que se realizaran para el parseo -- aqui debe de decirnos si hay error
        private string MetodoFalso(int pila, Type entrada) 
        {
            LR1 lr1 = new LR1();
            string res = lr1.GetAction(pila, entrada,ref listOpciones);
            if (res != "-1")
            {
                return res;
            }
            return "error";
        }

        //metodo para saber que bandera activar
        private void FlagAccion(string accion)
        {
            if (accion == "desplazar")
            {
                f_desplazar = true;
            }
            else if (accion == "reducir")
            {
                f_reducir = true;
            }
            else if (accion == "irA")
            {
                f_irA = true;
            }
            else if (accion == "aceptar")
            {
                f_aceptar = true;
            }
            else if (accion == "error")
            {
                f_error = true;
            }
        }

        //metodo que se utiliza para reducir la cadena (ingresa el estado de la produccion)
        private int Reducir(int p)
        {
            //Obtenemos la produccion del estado que se indica con el parametro
            Production produccion = new Production();
            LR1 lr1 = new LR1();
            produccion = lr1.GetState(p);
            //Se pregunta cuantos simbolos produce y se guarda en una variable
            int count = produccion.Lista_elementos.Count;
            bool f = false;
            foreach (var item in produccion.Lista_elementos)
            {
                if (item.Caracter == "''")
                {
                    f = true;
                }
            }
            if (count == 1 && f == true)
            {
                count = 0;
            }
            else
            {
                //se eliminan los ultimos valores de la lista simbolos dependiendo del numero de arriba
                simbolo.Reverse();//le doy vuelta a la lista simbolos
                int x = 0;

                while (x < count)
                {
                    simbolo.Remove(simbolo[0]);
                    x++;
                }

                simbolo.Reverse();//la regreso a la normalidad
            }
            
            //Se agrega el simbolo no terminal a la lista de simbolos
            Type t = new Type();
            t.cadena = produccion.Padre;
            t.linea = 0;
            t.column_I = 0;
            t.column_F = 0;
            t.Error = "";
            t.description = produccion.Padre;
            simbolo.Add(t);
            //y se retorna el valor de cuantos simbolos produce

            return count;
        }

        //metodo para obtener el ultimo de la lista simbolo
        private Type Ir_A()
        {
            Type last = new Type();
            foreach (var item in simbolo)
            {
                last = item;
            }

            return last;
        }

        //metodo para definir que accion se realizara
        private string[] SplitAction(string acc) 
        {
            string[] res = new string[2];
            char c = acc[0];
            if (acc == "acc")
            {
                res[0] = "aceptar";
                res[1] = 0.ToString();
            }
            else if (acc == "error")
            {
                res[0] = "error";
                res[1] = 0.ToString();
            }
            else if (c == 'd' || c == 's')
            {
                res[0] = "desplazar";
                acc = acc.Remove(0, 1);
                res[1] = acc;
            }
            else if (c == 'r')
            {
                res[0] = "reducir";
                acc = acc.Remove(0, 1);
                res[1] = acc;
            }
            else if (acc == "-2")
            {
                foreach (var item in listOpciones)
                {
                    OpcionesCola.Enqueue(item);
                }
                f_opc = true;//para activar el nuevo metodo
                res =  SplitAction(OpcionesCola.Dequeue());//recursividad
            }
            else 
            {
                res[0] = "irA";
                res[1] = acc;            
            }

            return res;
        }

        #endregion
    }
}
