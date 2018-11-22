using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace go
{
    class Sintaxis
    {
        public static List<Lista_nodos> lista_nodos;
        public static List<listPost> lt = new List<listPost>();
        

        int puntero = 0;
        public DataGridView resultados;
        public DataGridView posfijo;
        public DataGridView variables;

        //-------------------------------------------------VERIFICAR ERRORES DE VARIABLES---------------------------------------------------------

        string aux;
        string tipo;
        string unitario;



        //Cola auxiliar de variable
        Queue cola_var = new Queue();
        Queue cola_var2 = new Queue();


        //Lista de variables
        Dictionary<string, string> list_var = new Dictionary<string, string>();

        //--------------------------------------------------------SISTEMA DE TIPOS-----------------------------------------------------------------

        string pe;


        //Pila auxiliar para prioridad de operadores
        //Stack pila_entrada = new Stack();
        //Pila auxiliar para prioridad de operadores
        Stack<string> pila_entrada = new Stack<string>();

        //Pila auxiliar para el sistema de tipos
        //Stack <string>pila_salida = new Stack <string>();

        //Lista de sistema de tipos
        List<string> lista_sistema_tipos = new List<string>();



        //----------------------------------------------------PREPARAR POSFIJO------------------------------------------------------------------

        //Lista para ensamblador
     
        List<string> lista_posfijo = new List<string>();

        //Pila de etiquetas para el posfijo
        Stack<string> pila_etiquetas = new Stack<string>();
        Stack<string> pila_BRF_A = new Stack<string>();
        Stack<string> pila_BRI_B = new Stack<string>();
        Stack<string> pila_BRF_C = new Stack<string>();
        Stack<string> pila_BRI_D = new Stack<string>();
        int cont_a = 0;
        int cont_b = 0;
        int cont_c = 0;
        int cont_d = 0;

        

        //----------------------------------------------------ASM------------------------------------------------------------------
        string asm;
        TextWriter archivo;
        Stack<string> pila_asm = new Stack<string>();
        string op1;
        string op2;
        string auxVarAsm;
        string auxVarAsm2;
        string auxSentAsm;
        string auxSentAsm2;
        string instruccion;
        string resultado;
        string vartemp;
        int contAsm = 0;
        List<string> posfijoP = new List<string>();
        List<int> posfijoPTokens = new List<int>();

        Stack<int> InfijoTokens = new Stack<int>();
        Stack<string> Infijo = new Stack<string>();



        public Sintaxis(List<Lista_nodos> lista)
        {
            lista_nodos = lista;
        }

        public void analizador()
        {
            try
            {
                while (lista_nodos.Count > puntero)
                {
                    lt.Clear();
                    //package
                    if (lista_nodos[puntero].t == 200)
                    {
                        puntero++;

                        //main
                        if (lista_nodos[puntero].t == 201)
                        {
                            puntero++;

                            //func
                            if (lista_nodos[puntero].t == 202)
                            {
                                puntero++;

                                //main
                                if (lista_nodos[puntero].t == 201)
                                {
                                    puntero++;

                                    //(
                                    if (lista_nodos[puntero].t == 106)
                                    {
                                        puntero++;

                                        //)
                                        if (lista_nodos[puntero].t == 107)
                                        {
                                            puntero++;
                                            puntero = Bloque();
                                        }
                                        else
                                        {
                                            Llenafallasintaxis2();
                                            break;
                                        }
                                    }
                                    else
                                    {
                                        Llenafallasintaxis2();
                                        break;
                                    }
                                }
                                else
                                {
                                    Llenafallasintaxis2();
                                    break;
                                }
                            }
                            else
                            {
                                Llenafallasintaxis2();
                                break;
                            }
                        }
                        else
                        {
                            Llenafallasintaxis2();
                            break;
                        }
                    }
                    else
                    {
                        Llenafallasintaxis2();
                        break;
                    }
                }
            }
            catch
            {
                Llenafallasintaxis();

            }
        }

        public int Bloque()
        {

            try
            {
                //{
                if (lista_nodos[puntero].t == 108)
                {
                    puntero++;
                    puntero = Statement_list();

                    //}
                    if (lista_nodos[puntero].t == 109)
                    {
                        puntero++;

                        //imprime que variables no estan en uso
                        foreach (KeyValuePair<string, string> nodo in list_var)
                        {
                            bool nodo2 = cola_var2.Contains(nodo.Key);
                            if (nodo2 == false)
                                MessageBox.Show("VARIABLE: " + nodo.Key + " SIN USO");
                        }

                        LlenarPosfijoDgv();
                        LlenarVariablesDgv();
                        MessageBox.Show("Analisis terminado");
                        PrepararAsm();




                    }
                    else

                    {
                        Llenafallasintaxis2();
                    }

                }
                else
                {
                    Llenafallasintaxis2();
                }
            }
            catch
            {
                Llenafallasintaxis();
            }
            return puntero;
        }

        public int Statement_list()
        {
            try
            {
                /*             identificador                           if                              print                        scan           */
                if (lista_nodos[puntero].t == 100 || lista_nodos[puntero].t == 209 || lista_nodos[puntero].t == 203 || lista_nodos[puntero].t == 204 || lista_nodos[puntero].t == 211 || lista_nodos[puntero].t == 210 || lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102 || lista_nodos[puntero].t == 205)
                {
                    puntero = Statement();
                }
            }
            catch
            {
                Llenafallasintaxis();
            }
            return puntero;
        }

        public int Statement()
        {
            try
            {
                /*             identificador                           if                              print                        scan                            else */
                while (lista_nodos[puntero].t == 100 || lista_nodos[puntero].t == 209 || lista_nodos[puntero].t == 203 || lista_nodos[puntero].t == 204 || lista_nodos[puntero].t == 210 || lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102 || lista_nodos[puntero].t == 211 || lista_nodos[puntero].t == 205)
                {

                    //Declaracion de tipos 
                    if (lista_nodos[puntero].t == 205)
                    {
                        puntero = Vardec1();
                    }

                    //Declaracion de la variable
                    if (lista_nodos[puntero].t == 100)
                    {
                        puntero = Vardec();
                    }
                    //if
                    else if (lista_nodos[puntero].t == 209)
                    {
                        puntero = EsIf();

                    }
                    //else
                    /*else if (lista_nodos[puntero].t == 210)
                    {
                        puntero = EsElse();
                    }*/
                    //print
                    else if (lista_nodos[puntero].t == 203)
                    {
                        puntero = EsPrint();
                    }
                    //scan
                    else if (lista_nodos[puntero].t == 204)
                    {
                        puntero = EsScan();
                    }
                    //for
                    else if (lista_nodos[puntero].t == 211)
                    {
                        puntero = EsWhile();

                    }
                    //Expresion numerica
                    else if (lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102)
                    {
                        puntero = Exp_num();
                    }

                }
            }
            catch
            {
                Llenafallasintaxis2();
            }
            return puntero;
        }

        public int Vardec()
        {
            var lp = new listPost();//******

            try
            { //id
                if (lista_nodos[puntero].t == 100)
                {

                    //Verifica en el direccionario la existencia del id...
                    bool lex = list_var.ContainsKey(lista_nodos[puntero].l);
                    //Si no existe, imprime que la variable no esta declarada...
                    if (lex == false)
                    {
                        MessageBox.Show("VARIABLE: " + lista_nodos[puntero].l + " NO DECLARADA");

                    }
                    else
                    {
                        //Se agregan las variables que estan en asignacion
                        aux = lista_nodos[puntero].l;
                        cola_var2.Enqueue(aux);

                        //Recorre el diccionario de variables y su tipo correspondiente...
                        foreach (KeyValuePair<string, string> nodo in list_var)
                        {
                            //Si el lexema que se encuentra en aux coincide con una del diccionario...
                            if (aux == nodo.Key)
                            {

                                //Agrega el tipo de la variable a la lista de tipos, para el sistema de tipos...
                                lista_sistema_tipos.Add(nodo.Value);
                                lista_posfijo.Add(lista_nodos[puntero].l);

                                lp.p = lista_nodos[puntero].l;//************
                                lp.f = "operando";//************
                                lt.Add(lp);//*******

                            }
                        }


                    }
                    puntero++;
                    //:=
                    if (lista_nodos[puntero].t == 123)
                    {

                        Insertar();
                        puntero++;

                        if (lista_nodos[puntero].t == 124 || lista_nodos[puntero].t == 212 || lista_nodos[puntero].t == 213)
                        {

                            Insertar();
                            puntero++;
                        }
                        else
                        {

                            puntero = Exp_num();
                        }


                        //Para veciar la pila en la lista
                        foreach (string a in pila_entrada)
                        {
                            lista_sistema_tipos.Add(a);
                            lista_posfijo.Add(a);
                            lp.p = a;//************
                            lp.f = "operador";//************
                            lt.Add(lp);//*******


                        }


                        //Es para borrar la pila y volver a ser la evaluacion
                        pila_entrada.Clear();

                        //--------------------------------¡¡¡¡¡¡¡¡¡¡¡METER SISTEMA DE TIPO AQUI!!!!!!!!!!!!---------------------------------
                        SistemaDeTipos();
                        //------------------------------------------------------------------------------------------------------------------


                        //Es para borrar la lista y volver a ser la evaluacion
                        pila_entrada.Clear();
                        lista_sistema_tipos.Clear();
                    }
                    else
                    {
                        Llenafallasintaxis2();
                    }
                }
                else
                {
                    Llenafallasintaxis2();
                }
            }
            catch
            {
                Llenafallasintaxis();
            }
            return puntero;
        }

        public int Vardec1()
        {
            try
            {   //var
                if (lista_nodos[puntero].t == 205)
                {
                    puntero++;

                    //id
                    if (lista_nodos[puntero].t == 100)
                    {


                        aux = lista_nodos[puntero].l;
                        cola_var.Enqueue(aux);
                        puntero++;

                        if (lista_nodos[puntero].t == 206 || lista_nodos[puntero].t == 207 || lista_nodos[puntero].t == 208 || lista_nodos[puntero].t == 218)
                        {

                            tipo = lista_nodos[puntero].l;
                            foreach (string variable in cola_var)
                            {

                                bool d = list_var.ContainsKey(variable);
                                if (d == false)
                                {
                                    list_var.Add(variable, tipo);
                                }
                                else
                                {
                                    MessageBox.Show("Variable ya declarada: " + variable);
                                }
                            }
                            cola_var.Clear();
                            puntero++;
                        }
                        else if (lista_nodos[puntero].t == 105)
                        {
                            Vardec1aux();
                        } else
                        {
                            Llenafallasintaxis2();
                        }

                    }
                    else
                    {
                        Llenafallasintaxis2();
                    }
                }
                else
                {
                    Llenafallasintaxis2();
                }
            }
            catch
            {
                Llenafallasintaxis();
            }

            return puntero;
        }

        public int Vardec1aux()
        {
            //id
            if (lista_nodos[puntero].t == 105)
            {
                puntero++;
                if (lista_nodos[puntero].t == 100)
                {

                    aux = lista_nodos[puntero].l;
                    cola_var.Enqueue(aux);
                    puntero++;

                    if (lista_nodos[puntero].t == 105)
                    {

                        Vardec1aux();
                    }
                    else if (lista_nodos[puntero].t == 206 || lista_nodos[puntero].t == 207 || lista_nodos[puntero].t == 208 || lista_nodos[puntero].t == 218)
                    {
                        tipo = aux = lista_nodos[puntero].l;
                        foreach (string variable in cola_var)
                        {

                            bool d = list_var.ContainsKey(variable);
                            if (d == false)
                            {
                                list_var.Add(variable, tipo);
                            }
                            else
                            {
                                MessageBox.Show("Variable ya declarada: " + variable);
                            }
                        }
                        cola_var.Clear();

                        puntero++;
                    } else
                    {
                        Llenafallasintaxis2();
                    }
                }
                else
                {
                    Llenafallasintaxis2();
                }
            }
            else
            {
                Llenafallasintaxis2();
            }
            return puntero;
        }

        public int EsIf()
        {
            var lp = new listPost();//******
            try
            {
                // If
                if (lista_nodos[puntero].t == 209)
                {
                    //Etiquetas
                    cont_a++;
                    cont_b++;
                    pila_BRF_A.Push("BRF-A" + cont_a);
                    pila_BRI_B.Push("BRI-B" + cont_b);
                    pila_etiquetas.Push("B" + cont_b);
                    pila_etiquetas.Push("A" + cont_a);




                    puntero++;
                    //(
                    if (lista_nodos[puntero].t == 106)
                    {
                        puntero++;
                        puntero = Exp_log();


                        //)
                        if (lista_nodos[puntero].t == 107)
                        {
                            //Para veciar la pila en la lista
                            foreach (string a in pila_entrada)
                            {
                                lista_sistema_tipos.Add(a);
                                lista_posfijo.Add(a);
                                lp.p = a;//************
                                lp.f = "operador";//************
                                lt.Add(lp);//*******

                            }

                            //Es para borrar la pila y volver a ser la evaluacion
                            pila_entrada.Clear();

                            //--------------------------------¡¡¡¡¡¡¡¡¡¡¡METER SISTEMA DE TIPO AQUI!!!!!!!!!!!!---------------------------------
                            SistemaDeTipos();
                            //------------------------------------------------------------------------------------------------------------------


                            //Es para borrar la lista y volver a ser la evaluacion
                            pila_entrada.Clear();
                            lista_sistema_tipos.Clear();



                            lista_posfijo.Add(pila_BRF_A.Peek());
                            lp.p = pila_BRF_A.Peek();//************
                            lp.f = "etiqueta";//************
                            lt.Add(lp);//*******
                            pila_BRF_A.Pop();

                            puntero++;

                            //{
                            if (lista_nodos[puntero].t == 108)
                            {
                                puntero++;
                                Statement();

                                //}
                                if (lista_nodos[puntero].t == 109)
                                {
                                    lista_posfijo.Add(pila_BRI_B.Peek());
                                    lp.p = pila_BRI_B.Peek();//************
                                    lp.f = "etiqueta";//************
                                    lt.Add(lp);//*******
                                    pila_BRI_B.Pop();
                                    lista_posfijo.Add(pila_etiquetas.Peek());
                                    lp.p = pila_etiquetas.Peek();//************
                                    lp.f = "apuntador";//************
                                    lt.Add(lp);//*******
                                    pila_etiquetas.Pop();

                                    puntero++;

                                    if (lista_nodos[puntero].t == 210)
                                    {
                                        puntero = EsElse();
                                    }

                                    lista_posfijo.Add(pila_etiquetas.Peek());
                                    lp.p = pila_etiquetas.Peek();//************
                                    lp.f = "apuntador";//************
                                    lt.Add(lp);//*******
                                    pila_etiquetas.Pop();


                                }
                                else
                                {
                                    Llenafallasintaxis2();
                                }
                            }
                            else
                            {
                                Llenafallasintaxis2();
                            }
                        }
                        else
                        {
                            Llenafallasintaxis2();
                        }

                    }
                    else
                    {
                        Llenafallasintaxis2();
                    }
                }
                else
                {
                    Llenafallasintaxis2();
                }
            }
            catch
            {
                Llenafallasintaxis();
            }
            return puntero;
        }

        public int EsWhile()
        {
            var lp = new listPost();//******

            try
            {
                // While
                if (lista_nodos[puntero].t == 211)
                {
                    //Etiquetas
                    cont_c++;
                    cont_d++;
                    pila_BRI_D.Push("BRI-D" + cont_d);
                    pila_BRF_C.Push("BRF-C" + cont_c);
                    pila_etiquetas.Push("C" + cont_c);
                    pila_etiquetas.Push("D" + cont_d);

                    lista_posfijo.Add(pila_etiquetas.Peek());
                    lp.p = pila_etiquetas.Peek();//************
                    lp.f = "apuntador";//************
                    lt.Add(lp);//*******



                    pila_etiquetas.Pop();

                    puntero++;

                    //(
                    if (lista_nodos[puntero].t == 106)
                    {
                        puntero++;
                        puntero = Exp_log();


                        //)
                        if (lista_nodos[puntero].t == 107)
                        {
                            //Para veciar la pila en la lista
                            foreach (string a in pila_entrada)
                            {
                                lista_sistema_tipos.Add(a);
                                lista_posfijo.Add(a);
                                lp.p = a;//************
                                lp.f = "operador";//************
                                lt.Add(lp);//*******


                            }

                            //Es para borrar la pila y volver a ser la evaluacion
                            pila_entrada.Clear();

                            //--------------------------------¡¡¡¡¡¡¡¡¡¡¡METER SISTEMA DE TIPO AQUI!!!!!!!!!!!!---------------------------------
                            SistemaDeTipos();
                            //------------------------------------------------------------------------------------------------------------------


                            //Es para borrar la lista y volver a ser la evaluacion
                            pila_entrada.Clear();
                            lista_sistema_tipos.Clear();

                            lista_posfijo.Add(pila_BRF_C.Peek());
                            lp.p = pila_BRF_C.Peek();//************
                            lp.f = "etiqueta";//************
                            lt.Add(lp);//*******
                            pila_BRF_C.Pop();

                            puntero++;

                            //{
                            if (lista_nodos[puntero].t == 108)
                            {
                                puntero++;
                                Statement();

                                //}
                                if (lista_nodos[puntero].t == 109)
                                {
                                    lista_posfijo.Add(pila_BRI_D.Peek());
                                    lp.p = pila_BRI_D.Peek();//************
                                    lp.f = "etiqueta";//************
                                    lt.Add(lp);//*******
                                    pila_BRI_D.Pop();

                                    lista_posfijo.Add(pila_etiquetas.Peek());
                                    lp.p = pila_etiquetas.Peek();//************
                                    lp.f = "apuntador";//************
                                    lt.Add(lp);//*******
                                    pila_etiquetas.Pop();
                                    puntero++;

                                }
                                else
                                {
                                    Llenafallasintaxis2();
                                }
                            }
                            else
                            {
                                Llenafallasintaxis2();
                            }
                        }
                        else
                        {
                            Llenafallasintaxis2();
                        }

                    }
                    else
                    {
                        Llenafallasintaxis2();
                    }
                }
                else
                {
                    Llenafallasintaxis2();
                }
            }
            catch
            {
                Llenafallasintaxis();
            }
            return puntero;
        }

        public int EsScan()
        {
            var lp = new listPost();//******
            try
            {
                if (lista_nodos[puntero].t == 204) //print
                {
                    Insertar();
                    puntero++;
                    if (lista_nodos[puntero].t == 106) //(
                    {
                        puntero++;
                        /*if (lista_nodos[puntero].t == 124) //""
                        {
                            puntero++;
                            if (lista_nodos[puntero].t == 107)//)
                            {
                                puntero++;
                            }
                            else if (lista_nodos[puntero].t == 105)
                            {
                                puntero++;
                                if (lista_nodos[puntero].t == 100)
                                {
                                    puntero++;
                                    if (lista_nodos[puntero].t == 107) //)
                                    {
                                        puntero++;
                                    }
                                    else
                                    {
                                        Llenafallasintaxis2();
                                    }
                                }

                            }
                        }
                        else*/
                        if (lista_nodos[puntero].t == 100)
                        {

                            //Se agregan las variables que estan en asignacion
                            aux = lista_nodos[puntero].l;
                            cola_var2.Enqueue(aux);

                            //Verifica en el direccionario la existencia del id...
                            bool lex = list_var.ContainsKey(lista_nodos[puntero].l);
                            //Si no existe, imprime que la variable no esta declarada...
                            if (lex == false)
                            {
                                MessageBox.Show("VARIABLE: " + lista_nodos[puntero].l + " NO DECLARADA");
                                lista_posfijo.Add(lista_nodos[puntero].l);
                                lp.p = lista_nodos[puntero].l;//************
                                lp.f = "operando";//************
                                lt.Add(lp);//*******

                            }
                            else
                            {
                                lista_posfijo.Add(lista_nodos[puntero].l);
                                lp.p = lista_nodos[puntero].l;//************
                                lp.f = "operando";//************
                                lt.Add(lp);//*******
                            }
                            puntero++;
                            if (lista_nodos[puntero].t == 107) //)
                            {
                                pe = Convert.ToString(pila_entrada.Peek());
                                if (pe == "scan")
                                {
                                    lista_posfijo.Add(pe);
                                    lp.p = pe;//************
                                    lp.f = "operador";//************
                                    lt.Add(lp);//*******

                                }
                                pila_entrada.Clear();
                                puntero++;
                            }
                            else
                            {
                                Llenafallasintaxis2();
                            }
                        }
                        else
                        {
                            Llenafallasintaxis2();
                        }
                    }
                    else
                    {
                        Llenafallasintaxis2();
                    }
                }

            }
            catch
            {
                Llenafallasintaxis2();
            }
            return puntero;
        }

        public int EsElse()
        {
            try
            {
                //else
                if (lista_nodos[puntero].t == 210)
                {
                    puntero++;
                    //{
                    if (lista_nodos[puntero].t == 108)
                    {
                        puntero++;
                        puntero = Statement_list();
                        //}
                        if (lista_nodos[puntero].t == 109)
                        {
                            puntero++;
                        }
                    }
                }
            }
            catch
            {
                Llenafallasintaxis2();
            }
            return puntero;
        }

        public int EsPrint()
        {
            var lp = new listPost();//******
            try
            {
                if (lista_nodos[puntero].t == 203) //print
                {
                    Insertar();
                    puntero++;
                    if (lista_nodos[puntero].t == 106) //(
                    {
                        puntero++;
                        if (lista_nodos[puntero].t == 124) //""
                        {
                            lista_posfijo.Add(lista_nodos[puntero].l);
                            lp.p = lista_nodos[puntero].l;//************
                            lp.f = "operando";//************
                            lt.Add(lp);//*******
                            puntero++;
                            if (lista_nodos[puntero].t == 107)//)
                            {
                                pe = Convert.ToString(pila_entrada.Peek());
                                if (pe == "print")
                                {
                                    lista_posfijo.Add(pe);
                                    lp.p = pe;//************
                                    lp.f = "operador";//************
                                    lt.Add(lp);//*******
                                }
                                pila_entrada.Clear();
                                puntero++;
                            }/*
                            else if (lista_nodos[puntero].t == 105)
                            {
                                    puntero++;
                                if(lista_nodos[puntero].t == 100){
                                    puntero++;
                                    if (lista_nodos[puntero].t == 107) //)
                                    {
                                        puntero++;
                                    }
                                    else
                                    {
                                        Llenafallasintaxis2();
                                    }
                                }
                               
                            }*/
                        }
                        else if (lista_nodos[puntero].t == 100)
                        {
                            //Se agregan las variables que estan en asignacion
                            aux = lista_nodos[puntero].l;
                            cola_var2.Enqueue(aux);

                            //Verifica en el direccionario la existencia del id...
                            bool lex = list_var.ContainsKey(lista_nodos[puntero].l);
                            //Si no existe, imprime que la variable no esta declarada...
                            if (lex == false)
                            {
                                MessageBox.Show("VARIABLE: " + lista_nodos[puntero].l + " NO DECLARADA");
                                lista_posfijo.Add(lista_nodos[puntero].l);
                                lp.p = lista_nodos[puntero].l;//************
                                lp.f = "operando";//************
                                lt.Add(lp);//*******

                            }
                            else
                            {
                                lista_posfijo.Add(lista_nodos[puntero].l);
                                lp.p = lista_nodos[puntero].l;//************
                                lp.f = "operando";//************
                                lt.Add(lp);//*******
                            }
                            puntero++;
                            if (lista_nodos[puntero].t == 107) //)
                            {
                                pe = Convert.ToString(pila_entrada.Peek());
                                if (pe == "print")
                                {
                                    lista_posfijo.Add(pe);
                                    lp.p = pe;//************
                                    lp.f = "operador";//************
                                    lt.Add(lp);//*******
                                }
                                pila_entrada.Clear();
                                puntero++;
                            }
                            else
                            {
                                Llenafallasintaxis2();
                            }
                        }
                        else
                        {
                            Llenafallasintaxis2();
                        }
                    }
                    else
                    {
                        Llenafallasintaxis2();
                    }
                }
            }
            catch
            {
                Llenafallasintaxis();
            }
            return puntero;
        }

        //cadena
        public int valor_cadena()
        {
            try
            {
                // cadena
                if (lista_nodos[puntero].t == 124)
                {
                    puntero++;
                }
            }
            catch
            {
                Llenafallasintaxis2();
            }
            return puntero;
        }

        //valor numerico
        public int valor_num()
        {
            try
            {
                if (lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102)
                {
                    puntero++;
                }
            }
            catch
            {
                Llenafallasintaxis2();
            }
            return puntero;
        }

        //valor logico
        public int valor_log()
        {
            try
            {  //               true                             false
                if (lista_nodos[puntero].t == 212 || lista_nodos[puntero].t == 213)
                {
                    Insertar();
                    puntero++;
                }
            }
            catch
            {
                Llenafallasintaxis2();
            }
            return puntero;
        }

        //operadores logicos not
        public int Op_log1()
        {
            try
            {
                // !=
                if (lista_nodos[puntero].t == 118)
                {
                    Insertar();
                    puntero++;
                }
            }
            catch
            {
                Llenafallasintaxis2();
            }
            return puntero;
        }

        //operadores logicos and y or
        public int Op_log2()
        {
            try
            {
                //        &&                                ||
                if (lista_nodos[puntero].t == 125 || lista_nodos[puntero].t == 126)
                {
                    Insertar();
                    puntero++;
                }
            }
            catch
            {
                Llenafallasintaxis2();
            }
            return puntero;
        }

        //operadores numericos
        public int Op_num()
        {
            try
            {
                //        +                                   -                                           *                      /   
                if (lista_nodos[puntero].t == 112 || lista_nodos[puntero].t == 113 || lista_nodos[puntero].t == 117 || lista_nodos[puntero].t == 115)
                {
                    puntero++;
                }
            }
            catch
            {
                Llenafallasintaxis2();
            }
            return puntero;
        }

        //operadores relacionales
        public int Op_rel()
        {
            try
            {
                //        <                                   >                                           <=                      >=                                !=                               ==
                if (lista_nodos[puntero].t == 119 || lista_nodos[puntero].t == 120 || lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 122 || lista_nodos[puntero].t == 118 || lista_nodos[puntero].t == 129)
                {
                    Insertar();
                    puntero++;
                }
            }
            catch
            {
                Llenafallasintaxis2();
            }
            return puntero;
        }

        //Expresion relacional ...
        public int Exp_rel()
        {

            if (lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102 || lista_nodos[puntero].t == 106 || lista_nodos[puntero].t == 112 || lista_nodos[puntero].t == 113 || lista_nodos[puntero].t == 100) //tokens con los que empieza la Expresion numerica
            {
                puntero = Exp_num();
                puntero = Op_rel();
                puntero = Exp_num();
            }
            else
            {
                Llenafallasintaxis2();
            }


            return puntero;
        }

        public int Exp_log()
        {
            try
            {
                if (lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102 || lista_nodos[puntero].t == 112 || lista_nodos[puntero].t == 113 || lista_nodos[puntero].t == 100) //tokens con los que empieza la Expresion numerica que lo contiene la Expresion relacional
                {
                    puntero = Exp_rel();
                    puntero = Exp_log1();

                }
                else if (lista_nodos[puntero].t == 118) //!=
                {
                    puntero = Op_log1();
                    puntero = Exp_log();
                    puntero = Exp_log1();

                } else if (lista_nodos[puntero].t == 106) //(
                {
                    puntero++;
                    puntero = Exp_log();

                    if (lista_nodos[puntero].t == 107)//)
                    {
                        puntero++;
                        puntero = Exp_log1();
                    }
                    else
                    {
                        Llenafallasintaxis2();
                    }
                } else if (lista_nodos[puntero].t == 100)
                {
                    //Verifica en el direccionario la existencia del id...
                    bool lex = list_var.ContainsKey(lista_nodos[puntero].l);
                    //Si no existe, imprime que la variable no esta declarada...
                    if (lex == false)
                    {
                        MessageBox.Show("VARIABLE: " + lista_nodos[puntero].l + " NO DECLARADA");

                    }
                    else
                    {
                        //Se agregan las variables que estan en asignacion
                        aux = lista_nodos[puntero].l;
                        cola_var2.Enqueue(aux);

                        //Recorre el diccionario de variables y su tipo correspondiente...
                        foreach (KeyValuePair<string, string> nodo in list_var)
                        {
                            //Si el lexema que se encuentra en aux coincide con una del diccionario...
                            if (aux == nodo.Key)
                            {
                                //Agrega el tipo de la variable a la lista de tipos, para el sistema de tipos...
                                lista_sistema_tipos.Add(nodo.Value);
                            }
                        }
                    }
                    puntero++;
                    puntero = Exp_log1();

                } else if (lista_nodos[puntero].t == 212 || lista_nodos[puntero].t == 213) //valor logico
                {
                    puntero = valor_log();
                    puntero = Exp_log1();
                }
            }
            catch
            {
                Llenafallasintaxis2();
            }
            return puntero;
        }

        //Expresion logica 1 ..
        public int Exp_log1()
        {
            if (lista_nodos[puntero].t == 125 || lista_nodos[puntero].t == 126)
            {
                puntero = Op_log2();
                puntero = Exp_log();
                puntero = Exp_log1();
            }
            else
            {
                puntero = puntero;
            }
            return puntero;
        }

        //Expresion numerica
        public int Exp_num()
        {
            var lp = new listPost();//******

            try
            {
                if (lista_nodos[puntero].t == 106) // parentesis
                {
                    Insertar();
                    puntero++;
                    puntero = Exp_num();

                    if (lista_nodos[puntero].t == 107)//parentesis
                    {
                        Insertar();
                        puntero++;
                        puntero = Exp_num1();
                    }
                    else
                    {
                        Llenafallasintaxis2();
                    }
                } else if (lista_nodos[puntero].t == 112)//+
                {
                    unitario = "@";
                    Insertar();
                    unitario = "";
                    puntero++;
                    puntero = Exp_num();
                    puntero = Exp_num1();
                }
                else if (lista_nodos[puntero].t == 113) //-
                {

                    unitario = "$";
                    Insertar();
                    unitario = "";
                    puntero++;
                    puntero = Exp_num();
                    puntero = Exp_num1();
                }
                else if (lista_nodos[puntero].t == 100) //id
                {
                    //Verifica en el direccionario la existencia del id...
                    bool lex = list_var.ContainsKey(lista_nodos[puntero].l);
                    //Si no existe, imprime que la variable no esta declarada...
                    if (lex == false)
                    {
                        MessageBox.Show("VARIABLE: " + lista_nodos[puntero].l + " NO DECLARADA");

                    }
                    else
                    {
                        //Se agregan las variables que estan en asignacion
                        aux = lista_nodos[puntero].l;
                        cola_var2.Enqueue(aux);

                        //Recorre el diccionario de variables y su tipo correspondiente...
                        foreach (KeyValuePair<string, string> nodo in list_var)
                        {
                            //Si el lexema que se encuentra en aux coincide con una del diccionario...
                            if (aux == nodo.Key)
                            {
                                //Agrega el tipo de la variable a la lista de tipos, para el sistema de tipos...
                                lista_sistema_tipos.Add(nodo.Value);
                                lista_posfijo.Add(lista_nodos[puntero].l);
                                lp.p = lista_nodos[puntero].l;//************
                                lp.f = "operando";//************
                                lt.Add(lp);//*******
                            }
                        }
                    }

                    puntero++;
                    puntero = Exp_num1();
                }
                else if (lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102) //valor numerico
                {
                    Insertar();
                    puntero++;
                    puntero = Exp_num1();
                }
                else
                {
                    Llenafallasintaxis2();
                }

            }
            catch
            {
                Llenafallasintaxis2();
            }

            return puntero;
        }

        //Expresion numerica 1
        public int Exp_num1()
        {
            if (lista_nodos[puntero].t == 112 || lista_nodos[puntero].t == 113 || lista_nodos[puntero].t == 117 || lista_nodos[puntero].t == 115) //operador numerico
            {
                Insertar();
                puntero++;
                puntero = Exp_num();
                puntero = Exp_num1();
            }
            else
            {
                puntero = puntero;
            }
            return puntero;
        }

        public void Llenafallasintaxis()
        {

            MessageBox.Show("Termino La Lista En " + lista_nodos[puntero].l + ", No Se Completo El Programa");
        }

        public void Llenafallasintaxis2()
        {
            resultados.Rows.Add(lista_nodos[puntero].t, "Error Con " + lista_nodos[puntero].l, lista_nodos[puntero - 1].r);
        }

        public void LlenarPosfijoDgv()
        {

             for (int i = 0; i < lt.Count; i++)
             {
                 posfijo.Rows.Add(lt[i].p,lt[i].f);

             }

        }

        public void LlenarVariablesDgv()
        {
            foreach (KeyValuePair<string, string> nodo in list_var)
            {
                //textBox3.Text += ("Key = {0}, Value = {1}", kvp.Key, kvp.Value);
                variables.Rows.Add(nodo.Key, nodo.Value);
            }
        }

        public void Insertar()
        {
            var lp = new listPost();//******

            try
            {
                if (lista_nodos[puntero].t == 203)
                {
                    if (pila_entrada.Count == 0)
                    {
                        pila_entrada.Push("print");
                    }
                    else
                    {
                        pila_entrada.Push("print");
                    }
                }
                else if (lista_nodos[puntero].t == 204)
                {
                    if (pila_entrada.Count == 0)
                    {
                        pila_entrada.Push("scan");
                    }
                    else
                    {
                        pila_entrada.Push("scan");
                    }
                }
                else if (lista_nodos[puntero].t == 212)
                {
                    lista_sistema_tipos.Add("bool");
                    lista_posfijo.Add(lista_nodos[puntero].l);
                    lp.p = lista_nodos[puntero].l;//************
                    lp.f = "operando";//************
                    lt.Add(lp);//*******
                }//Si el lexema es 212 es false token de un string, agrega a la lista de tipos un string...
                else if (lista_nodos[puntero].t == 213)
                {
                    lista_sistema_tipos.Add("bool");
                    lista_posfijo.Add(lista_nodos[puntero].l);
                    lp.p = lista_nodos[puntero].l;//************
                    lp.f = "operando";//************
                    lt.Add(lp);//*******
                }
                else if (lista_nodos[puntero].t == 124)
                {
                    lista_sistema_tipos.Add("string");
                    lista_posfijo.Add(lista_nodos[puntero].l);
                    lp.p = lista_nodos[puntero].l;//************
                    lp.f = "operando";//************
                    lt.Add(lp);//*******
                }
                else if (lista_nodos[puntero].t == 101)
                {
                    lista_sistema_tipos.Add("int");
                    lista_posfijo.Add(lista_nodos[puntero].l);
                    lp.p = lista_nodos[puntero].l;//************
                    lp.f = "operando";//************
                    lt.Add(lp);//*******

                }
                else if (lista_nodos[puntero].t == 102)
                {
                    lista_sistema_tipos.Add("float");
                    lista_posfijo.Add(lista_nodos[puntero].l);
                    lp.p = lista_nodos[puntero].l;//************
                    lp.f = "operando";//************
                    lt.Add(lp);//*******

                }
                else if (lista_nodos[puntero].t == 123)//:=
                {
                    if (pila_entrada.Count == 0)
                    {
                        pila_entrada.Push(":=");

                    }
                    else
                    {
                        int n;
                        for (n = 0; n <= pila_entrada.Count; n++)
                        {
                            pe = Convert.ToString(pila_entrada.Peek());


                            if (pe == ":=" || pe == "+" || pe == "-" || pe == "*" || pe == "/" || pe == "^" || pe == "@" || pe == "$")
                            {
                                lista_sistema_tipos.Add(pe);
                             
                                lista_posfijo.Add(lista_nodos[puntero].l);

                                lp.p = lista_nodos[puntero].l;//************
                                lp.f = "operador";//************
                                lt.Add(lp);//*******

                                pila_entrada.Pop();
                                if (n == pila_entrada.Count)
                                {
                                    n = 0;
                                }
                            }
                            else
                            {
                                pila_entrada.Push(":=");
                                break;
                            }
                        }

                    }

                }
                else if (lista_nodos[puntero].t == 112) //+(unitario)
                {
                    if (unitario == "@")
                    {
                        if (pila_entrada.Count == 0)
                        {
                            pila_entrada.Push("@");

                        }
                        else
                        {
                            int n;
                            for (n = 0; n <= pila_entrada.Count; n++)
                            {
                                pe = Convert.ToString(pila_entrada.Peek());


                                if (pe == "@")
                                {
                                    lista_sistema_tipos.Add(pe);
                                    lista_posfijo.Add(lista_nodos[puntero].l);
                                    lp.p = lista_nodos[puntero].l;//************
                                    lp.f = "operador";//************
                                    lt.Add(lp);//*******

                                    pila_entrada.Pop();
                                    if (n == pila_entrada.Count)
                                    {
                                        n = 0;
                                    }
                                }
                                else
                                {
                                    pila_entrada.Push("@");
                                    break;
                                }
                            }

                        }
                    }
                    else
                    {
                        if (pila_entrada.Count == 0)
                        {
                            pila_entrada.Push("+");
                        }
                        else
                        {

                            int n = 0;


                            for (n = 0; n <= pila_entrada.Count; n++)
                            {
                                pe = Convert.ToString(pila_entrada.Peek());

                                if (pe == "+" || pe == "-" || pe == "*" || pe == "/" || pe == "^" || pe == "@" || pe == "$")
                                {
                                    lista_sistema_tipos.Add(pe);
                                    lista_posfijo.Add(lista_nodos[puntero].l);
                                    lp.p = lista_nodos[puntero].l;//************
                                    lp.f = "operador";//************
                                    lt.Add(lp);//*******
                                    pila_entrada.Pop();
                                    if (n == pila_entrada.Count)
                                    {
                                        n = 0;
                                    }

                                }
                                else
                                {
                                    pila_entrada.Push("+");
                                    break;
                                }
                            }
                        }
                    }


                }
                else if (lista_nodos[puntero].t == 113) // - (Unitario)
                {
                    if (unitario == "$")
                    {
                        if (pila_entrada.Count == 0)
                        {
                            pila_entrada.Push("$");
                        }
                        else
                        {
                            int n;
                            for (n = 0; n <= pila_entrada.Count; n++)
                            {
                                pe = Convert.ToString(pila_entrada.Peek());


                                if (pe == "$")
                                {
                                    lista_sistema_tipos.Add(pe);
                                    lista_posfijo.Add(lista_nodos[puntero].l);
                                    lp.p = lista_nodos[puntero].l;//************
                                    lp.f = "operador";//************
                                    lt.Add(lp);//*******
                                    pila_entrada.Pop();
                                    if (n == pila_entrada.Count)
                                    {
                                        n = 0;
                                    }
                                }
                                else
                                {
                                    pila_entrada.Push("$");
                                    break;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (pila_entrada.Count == 0)
                        {
                            pila_entrada.Push("-");
                        }
                        else
                        {
                            int n;
                            for (n = 0; n <= pila_entrada.Count; n++)
                            {
                                pe = Convert.ToString(pila_entrada.Peek());


                                if (pe == "+" || pe == "-" || pe == "*" || pe == "/" || pe == "^" || pe == "@" || pe == "$")
                                {
                                    lista_sistema_tipos.Add(pe);
                                    lista_posfijo.Add(lista_nodos[puntero].l);
                                    lp.p = lista_nodos[puntero].l;//************
                                    lp.f = "operador";//************
                                    lt.Add(lp);//*******
                                    pila_entrada.Pop();

                                    if (n == pila_entrada.Count)
                                    {
                                        n = 0;
                                    }
                                }
                                else
                                {
                                    pila_entrada.Push("-");
                                    break;
                                }
                            }
                        }
                    }


                }
                else if (lista_nodos[puntero].t == 117) //*
                {
                    if (pila_entrada.Count == 0)
                    {
                        pila_entrada.Push("*");
                    }
                    else
                    {
                        int n;
                        for (n = 0; n <= pila_entrada.Count; n++)
                        {
                            pe = Convert.ToString(pila_entrada.Peek());

                            if (pe == "*" || pe == "/" || pe == "^" || pe == "@" || pe == "$")
                            {
                                lista_sistema_tipos.Add(pe);
                                lista_posfijo.Add(lista_nodos[puntero].l);
                                lp.p = lista_nodos[puntero].l;//************
                                lp.f = "operador";//************
                                lt.Add(lp);//*******
                                pila_entrada.Pop();
                                if (n == pila_entrada.Count)
                                {
                                    n = 0;
                                }
                            }
                            else
                            {
                                pila_entrada.Push("*");
                                break;
                            }

                        }
                    }
                }
                else if (lista_nodos[puntero].t == 115)// /
                {
                    if (pila_entrada.Count == 0)
                    {
                        pila_entrada.Push("/");
                    }
                    else
                    {
                        int n;
                        for (n = 0; n <= pila_entrada.Count; n++)
                        {
                            pe = Convert.ToString(pila_entrada.Peek());

                            if (pe == "*" || pe == "/" || pe == "^" || pe == "@" || pe == "$")
                            {
                                lista_sistema_tipos.Add(pe);
                                lista_posfijo.Add(lista_nodos[puntero].l);
                                lp.p = lista_nodos[puntero].l;//************
                                lp.f = "operador";//************
                                lt.Add(lp);//*******
                                pila_entrada.Pop();
                                if (n == pila_entrada.Count)
                                {
                                    n = 0;
                                }
                            }
                            else
                            {
                                pila_entrada.Push("/");
                                break;
                            }

                        }
                    }
                }
                else if (lista_nodos[puntero].t == 106) //(
                {
                    if (pila_entrada.Count == 0)
                    {
                        pila_entrada.Push("(");
                    }
                    else
                    {
                        pila_entrada.Push("(");

                    }
                }
                else if (lista_nodos[puntero].t == 107) //)
                {
                    int n;
                    for (n = 0; n <= pila_entrada.Count; n++)
                    {
                        pe = Convert.ToString(pila_entrada.Peek());

                        if (pe != "(")
                        {
                            lista_sistema_tipos.Add(pe);
                            lista_posfijo.Add(lista_nodos[puntero].l);
                            lp.p = lista_nodos[puntero].l;//************
                            lp.f = "operador";//************
                            lt.Add(lp);//*******
                            pila_entrada.Pop();
                            if (n == pila_entrada.Count)
                            {
                                n = 0;
                            }
                        }
                        else if (pe == "(")
                        {

                            pila_entrada.Pop();
                            break;
                        }
                    }
                }
                else if (lista_nodos[puntero].t == 119) // <
                {
                    if (pila_entrada.Count == 0)
                    {
                        pila_entrada.Push("<");
                    }
                    else
                    {
                        int n;
                        for (n = 0; n <= pila_entrada.Count; n++)
                        {
                            pe = Convert.ToString(pila_entrada.Peek());

                            if (pe == "<" || pe == ">" || pe == "<=" || pe == ">=" || pe == "==" || pe == "!=" || pe == "!" || pe == "+" || pe == "-" || pe == "*" || pe == "/" || pe == "^" || pe == "@" || pe == "$")
                            {
                                lista_sistema_tipos.Add(pe);
                                lista_posfijo.Add(lista_nodos[puntero].l);
                                lp.p = lista_nodos[puntero].l;//************
                                lp.f = "operador";//************
                                lt.Add(lp);//*******
                                pila_entrada.Pop();
                                if (n == pila_entrada.Count)
                                {
                                    n = 0;
                                }
                            }
                            else
                            {
                                pila_entrada.Push("<");
                                break;
                            }

                        }
                    }
                }
                else if (lista_nodos[puntero].t == 120) // >
                {
                    if (pila_entrada.Count == 0)
                    {
                        pila_entrada.Push(">");
                    }
                    else
                    {
                        int n;
                        for (n = 0; n <= pila_entrada.Count; n++)
                        {
                            pe = Convert.ToString(pila_entrada.Peek());

                            if (pe == "<" || pe == ">" || pe == "<=" || pe == ">=" || pe == "==" || pe == "!=" || pe == "!" || pe == "+" || pe == "-" || pe == "*" || pe == "/" || pe == "^" || pe == "@" || pe == "$")
                            {
                                lista_sistema_tipos.Add(pe);
                                lista_posfijo.Add(lista_nodos[puntero].l);
                                lp.p = lista_nodos[puntero].l;//************
                                lp.f = "operador";//************
                                lt.Add(lp);//*******
                                pila_entrada.Pop();
                                if (n == pila_entrada.Count)
                                {
                                    n = 0;
                                }
                            }
                            else
                            {
                                pila_entrada.Push(">");
                                break;
                            }

                        }
                    }
                }
                else if (lista_nodos[puntero].t == 101) //<=
                {
                    if (pila_entrada.Count == 0)
                    {
                        pila_entrada.Push("<=");
                    }
                    else
                    {
                        int n;
                        for (n = 0; n <= pila_entrada.Count; n++)
                        {
                            pe = Convert.ToString(pila_entrada.Peek());

                            if (pe == "<" || pe == ">" || pe == "<=" || pe == ">=" || pe == "==" || pe == "!=" || pe == "!" || pe == "+" || pe == "-" || pe == "*" || pe == "/" || pe == "^" || pe == "@" || pe == "$")
                            {
                                lista_sistema_tipos.Add(pe);
                                lista_posfijo.Add(lista_nodos[puntero].l);
                                lp.p = lista_nodos[puntero].l;//************
                                lp.f = "operador";//************
                                lt.Add(lp);//*******
                                pila_entrada.Pop();
                                if (n == pila_entrada.Count)
                                {
                                    n = 0;
                                }
                            }
                            else
                            {
                                pila_entrada.Push("<=");
                                break;
                            }

                        }
                    }
                }
                else if (lista_nodos[puntero].t == 122) //>=
                {
                    if (pila_entrada.Count == 0)
                    {
                        pila_entrada.Push(">=");
                    }
                    else
                    {
                        int n;
                        for (n = 0; n <= pila_entrada.Count; n++)
                        {
                            pe = Convert.ToString(pila_entrada.Peek());

                            if (pe == "<" || pe == ">" || pe == "<=" || pe == ">=" || pe == "==" || pe == "!=" || pe == "!" || pe == "+" || pe == "-" || pe == "*" || pe == "/" || pe == "^" || pe == "@" || pe == "$")
                            {
                                lista_sistema_tipos.Add(pe);
                                lista_posfijo.Add(lista_nodos[puntero].l);
                                lp.p = lista_nodos[puntero].l;//************
                                lp.f = "operador";//************
                                lt.Add(lp);//*******
                                pila_entrada.Pop();
                                if (n == pila_entrada.Count)
                                {
                                    n = 0;
                                }
                            }
                            else
                            {
                                pila_entrada.Push(">=");
                                break;
                            }

                        }
                    }
                }
                else if (lista_nodos[puntero].t == 129) //==
                {
                    if (pila_entrada.Count == 0)
                    {
                        pila_entrada.Push("==");
                    }
                    else
                    {
                        int n;
                        for (n = 0; n <= pila_entrada.Count; n++)
                        {
                            pe = Convert.ToString(pila_entrada.Peek());

                            if (pe == "<" || pe == ">" || pe == "<=" || pe == ">=" || pe == "==" || pe == "!=" || pe == "!" || pe == "+" || pe == "-" || pe == "*" || pe == "/" || pe == "^" || pe == "@" || pe == "$")
                            {
                                lista_sistema_tipos.Add(pe);
                                lista_posfijo.Add(lista_nodos[puntero].l);
                                lp.p = lista_nodos[puntero].l;//************
                                lp.f = "operador";//************
                                lt.Add(lp);//*******
                                pila_entrada.Pop();
                                if (n == pila_entrada.Count)
                                {
                                    n = 0;
                                }
                            }
                            else
                            {
                                pila_entrada.Push("==");
                                break;
                            }

                        }
                    }
                }
                else if (lista_nodos[puntero].t == 118) //!=
                {
                    if (pila_entrada.Count == 0)
                    {
                        pila_entrada.Push("!=");
                    }
                    else
                    {
                        int n;
                        for (n = 0; n <= pila_entrada.Count; n++)
                        {
                            pe = Convert.ToString(pila_entrada.Peek());

                            if (pe == "<" || pe == ">" || pe == "<=" || pe == ">=" || pe == "==" || pe == "!=" || pe == "!" || pe == "+" || pe == "-" || pe == "*" || pe == "/" || pe == "^" || pe == "@" || pe == "$")
                            {
                                lista_sistema_tipos.Add(pe);
                                lista_posfijo.Add(lista_nodos[puntero].l);
                                lp.p = lista_nodos[puntero].l;//************
                                lp.f = "operador";//************
                                lt.Add(lp);//*******
                                pila_entrada.Pop();
                                if (n == pila_entrada.Count)
                                {
                                    n = 0;
                                }
                            }
                            else
                            {
                                pila_entrada.Push("!=");
                                break;
                            }

                        }
                    }
                }
                else if (lista_nodos[puntero].t == 125)//&&
                {
                    if (pila_entrada.Count == 0)
                    {
                        pila_entrada.Push("&&");
                    }
                    else
                    {
                        int n;
                        for (n = 0; n <= pila_entrada.Count; n++)
                        {
                            pe = Convert.ToString(pila_entrada.Peek());

                            if (pe == "<" || pe == ">" || pe == "<=" || pe == ">=" || pe == "==" || pe == "!=" || pe == "!" || pe == "+" || pe == "-" || pe == "*" || pe == "/" || pe == "^" || pe == "@" || pe == "$")
                            {
                                lista_sistema_tipos.Add(pe);
                                lista_posfijo.Add(lista_nodos[puntero].l);
                                lp.p = lista_nodos[puntero].l;//************
                                lp.f = "operador";//************
                                lt.Add(lp);//*******
                                pila_entrada.Pop();
                                if (n == pila_entrada.Count)
                                {
                                    n = 0;
                                }
                            }
                            else
                            {
                                pila_entrada.Push("&&");
                                break;
                            }

                        }
                    }
                }
                else if (lista_nodos[puntero].t == 126) //||
                {
                    if (pila_entrada.Count == 0)
                    {
                        pila_entrada.Push("||");
                    }
                    else
                    {
                        int n;
                        for (n = 0; n <= pila_entrada.Count; n++)
                        {
                            pe = Convert.ToString(pila_entrada.Peek());

                            if (pe == "<" || pe == ">" || pe == "<=" || pe == ">=" || pe == "==" || pe == "!=" || pe == "!" || pe == "+" || pe == "-" || pe == "*" || pe == "/" || pe == "^" || pe == "@" || pe == "$")
                            {
                                lista_sistema_tipos.Add(pe);
                                lista_posfijo.Add(lista_nodos[puntero].l);
                                lp.p = lista_nodos[puntero].l;//************
                                lp.f = "operador";//************
                                lt.Add(lp);//*******
                                pila_entrada.Pop();
                                if (n == pila_entrada.Count)
                                {
                                    n = 0;
                                }
                            }
                            else
                            {
                                pila_entrada.Push("||");
                                break;
                            }

                        }
                    }
                }
                else if (lista_nodos[puntero].t == 123) //!
                {
                    if (pila_entrada.Count == 0)
                    {
                        pila_entrada.Push("!");
                    }
                    else
                    {
                        int n;
                        for (n = 0; n <= pila_entrada.Count; n++)
                        {
                            pe = Convert.ToString(pila_entrada.Peek());

                            if (pe == "<" || pe == ">" || pe == "<=" || pe == ">=" || pe == "==" || pe == "!=" || pe == "!" || pe == "+" || pe == "-" || pe == "*" || pe == "/" || pe == "^" || pe == "@" || pe == "$")
                            {
                                lista_sistema_tipos.Add(pe);
                                lista_posfijo.Add(lista_nodos[puntero].l);
                                lp.p = lista_nodos[puntero].l;//************
                                lp.f = "operador";//************
                                lt.Add(lp);//*******
                                pila_entrada.Pop();
                                if (n == pila_entrada.Count)
                                {
                                    n = 0;
                                }
                            }
                            else
                            {
                                pila_entrada.Push("!");
                                break;
                            }

                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error de semantica you");
            }




        }

        private int SistemaDeTipos()
        {
            for (int i = 0; i < lista_sistema_tipos.Count; i++)
            {
                try
                {
                    //              entero                          didgito                        cadena                       bool
                    if (lista_sistema_tipos.ElementAt(i) == "int" || lista_sistema_tipos.ElementAt(i) == "float" || lista_sistema_tipos.ElementAt(i) == "string" || lista_sistema_tipos.ElementAt(i) == "bool")
                    {
                        pila_entrada.Push(lista_sistema_tipos.ElementAt(i));
                    }
                    else
                    {
                        #region asignacion
                        /*asignacion*/
                        if (lista_sistema_tipos.ElementAt(i) == ":=")
                        {

                            /*int*/
                            if (pila_entrada.Peek() == "int")
                            {
                                pila_entrada.Pop();
                                if (pila_entrada.Peek() == "int") //int
                                {
                                    pila_entrada.Pop();
                                }
                                else
                                {
                                    puntero--;
                                    MessageBox.Show("Error de tipo en la asignacion en el renglon" + lista_nodos[puntero].r);
                                    puntero++;
                                }
                            }

                            /*real*/
                            else if (pila_entrada.Peek() == "float")
                            {
                                pila_entrada.Pop();
                                if (pila_entrada.Peek() == "float") //real
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("float");
                                }
                                else if (pila_entrada.Peek() == "int") //int
                                {
                                    pila_entrada.Pop();
                                }
                                else
                                {
                                    puntero--;
                                    MessageBox.Show("Error de tipo en la asignacion en el renglon" + lista_nodos[puntero].r);
                                    puntero++;
                                }
                            }

                            /*string*/
                            else if (pila_entrada.Peek() == "string")
                            {
                                pila_entrada.Pop();
                                if (pila_entrada.Peek() == "string") //cadena
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("string");
                                }
                                else
                                {
                                    puntero--;
                                    MessageBox.Show("Error de tipo en la asignacion en el renglon" + lista_nodos[puntero].r);
                                    puntero++;
                                }
                            }
                        }
                        #endregion

                        #region suma 
                        /*suma*/
                        else if (lista_sistema_tipos.ElementAt(i) == "+")
                        {

                            /*int*/
                            if (pila_entrada.Peek() == "int")
                            {
                                pila_entrada.Pop();
                                if (pila_entrada.Peek() == "int") //int
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("int");
                                }
                                else if (pila_entrada.Peek() == "float") //real
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("float");
                                }
                                else
                                {
                                    MessageBox.Show("Error de Tipo");
                                }
                            }

                            /*real*/
                            else if (pila_entrada.Peek() == "float")
                            {
                                pila_entrada.Pop();
                                if (pila_entrada.Peek() == "int") //int
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("float");
                                }
                                else if (pila_entrada.Peek() == "float") //real
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("float");
                                }
                                else
                                {
                                    MessageBox.Show("Error de Tipo");
                                }
                            }

                            /*string*/
                            else if (pila_entrada.Peek() == "string")
                            {
                                pila_entrada.Pop();
                                if (pila_entrada.Peek() == "string") //cadena
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("string");
                                }
                                else
                                {
                                    MessageBox.Show("Error de Tipo");
                                }
                            }
                        }
                        #endregion

                        #region resta
                        /*resta*/
                        else if (lista_sistema_tipos.ElementAt(i) == "-")
                        {
                            /*int*/
                            if (pila_entrada.Peek() == "int")
                            {
                                pila_entrada.Pop();
                                if (pila_entrada.Peek() == "int") //int
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("int");
                                }
                                else if (pila_entrada.Peek() == "float") //real
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("float");
                                }
                                else
                                {
                                    MessageBox.Show("Error de Tipo");
                                }
                            }

                            /*real*/
                            else if (pila_entrada.Peek() == "float")
                            {
                                pila_entrada.Pop();
                                if (pila_entrada.Peek() == "int") //int
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("float");
                                }
                                else if (pila_entrada.Peek() == "float") //real
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("float");
                                }
                                else
                                {
                                    MessageBox.Show("Error de Tipo");
                                }
                            }
                        }
                        #endregion

                        #region multi
                        else if (lista_sistema_tipos.ElementAt(i) == "*")
                        {
                            /*int*/
                            if (pila_entrada.Peek() == "int")
                            {
                                pila_entrada.Pop();
                                if (pila_entrada.Peek() == "int") //int
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("int");
                                }
                                else if (pila_entrada.Peek() == "float") //real
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("float");
                                }
                                else
                                {
                                    MessageBox.Show("Error de Tipo");
                                }
                            }

                            /*real*/
                            else if (pila_entrada.Peek() == "float")
                            {
                                pila_entrada.Pop();
                                if (pila_entrada.Peek() == "int") //int
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("float");
                                }
                                else if (pila_entrada.Peek() == "float") //real
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("float");
                                }
                                else
                                {
                                    MessageBox.Show("Error de Tipo");
                                }
                            }
                        }
                        #endregion

                        #region division
                        /*div*/
                        else if (lista_sistema_tipos.ElementAt(i) == "/")
                        {
                            /*int*/
                            if (pila_entrada.Peek() == "int")
                            {
                                pila_entrada.Pop();
                                if (pila_entrada.Peek() == "int") //int
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("float");
                                }
                                else if (pila_entrada.Peek() == "float") //real
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("float");
                                }
                                else
                                {
                                    MessageBox.Show("Error de Tipo");
                                }
                            }

                            /*real*/
                            else if (pila_entrada.Peek() == "float")
                            {
                                pila_entrada.Pop();
                                if (pila_entrada.Peek() == "int") //int
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("float");
                                }
                                else if (pila_entrada.Peek() == "float") //real
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("float");
                                }
                                else
                                {
                                    MessageBox.Show("Error de Tipo");
                                }
                            }
                        }
                        #endregion

                        #region Op_rel
                        /*!=, <, >, <=, >=, ==*/
                        else if (lista_sistema_tipos.ElementAt(i) == "!=" || lista_sistema_tipos.ElementAt(i) == "<" || lista_sistema_tipos.ElementAt(i) == ">" || lista_sistema_tipos.ElementAt(i) == "<=" || lista_sistema_tipos.ElementAt(i) == ">=" || lista_sistema_tipos.ElementAt(i) == "==")
                        {
                            /*int*/
                            if (pila_entrada.Peek() == "int")
                            {
                                pila_entrada.Pop();
                                if (pila_entrada.Peek() == "int") //int
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("bool");
                                }
                                else if (pila_entrada.Peek() == "float") //real
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("bool");
                                }
                                else
                                {
                                    MessageBox.Show("Error de Tipo");
                                }
                            }

                            /*real*/
                            else if (pila_entrada.Peek() == "float")
                            {
                                pila_entrada.Pop();
                                if (pila_entrada.Peek() == "int") //int
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("bool");
                                }
                                else if (pila_entrada.Peek() == "float") //real
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("bool");
                                }
                                else
                                {
                                    MessageBox.Show("Error de Tipo");
                                }
                            }
                        }
                        #endregion

                        #region logicos
                        /*and = &&, or = ||*/
                        else if (lista_sistema_tipos.ElementAt(i) == "&&" || lista_sistema_tipos.ElementAt(i) == "||")
                        {
                            /*bool*/
                            if (pila_entrada.Peek() == "bool")
                            {
                                pila_entrada.Pop();
                                if (pila_entrada.Peek() == "bool") //bool
                                {
                                    pila_entrada.Pop();
                                    pila_entrada.Push("bool");
                                }
                                else
                                {
                                    MessageBox.Show("Error de Tipo");
                                }
                            }
                        }
                        #endregion

                        #region logicos2
                        /*not = !*/
                        else if (lista_sistema_tipos.ElementAt(i) == "!")
                        {
                            /*bool*/
                            if (pila_entrada.Peek() == "bool")
                            {
                                pila_entrada.Pop();
                                pila_entrada.Push("bool");
                            }
                        }
                        #endregion
                    }

                }
                catch
                {
                    MessageBox.Show("Error de sintaxis you");
                }
            }
            return puntero;
        }

        private void Limpiarpila_entrada()
        {
            while (pila_entrada.Count != 0)
            {
                lista_sistema_tipos.Add(pila_entrada.Pop());

            }
        }

        private void AnalizarPostAsm()
        {
            for (int i = 0; i < lt.Count; i++)
            {
               
                contAsm++;

                try
                {
                    if (lt[i].f == "operando")
                    {
                        pila_asm.Push(lt[i].p);

                    }else if (lt[i].f == "operador")
                    {
                        if (lt[i].p == "print")
                        {
                            op1 = pila_asm.Peek();
                            pila_asm.Pop();

                            //Verifica en el direccionario la existencia del id...
                            bool variableOCadena = list_var.ContainsKey(op1);
                            //Si no existe, imprime que la variable no esta declarada...
                            if (variableOCadena == true)
                            {
                                auxSentAsm = "mov ah,09" + "\n" + "lea dx," + op1 + "\n" + "int 21h" + "\n" + "\n";
                                auxSentAsm2 = auxSentAsm2 + auxSentAsm;

                                auxSentAsm = "mov ah,09" + "\n" + "lea dx," + "salto " + "\n" + "int 21h" + "\n" + "\n";
                                auxSentAsm2 = auxSentAsm2 + auxSentAsm;
                            }
                            else
                            {
                                auxVarAsm = "msg"+ contAsm + " " + "db" + " " + op1 + "," + "'$'" + "\n" + "\n";
                                auxVarAsm2 = auxVarAsm2 + auxVarAsm;

                                auxSentAsm = "mov ah,09" + "\n" + "lea dx," + "msg" + contAsm + "\n" + "int 21h" + "\n" + "\n";
                                auxSentAsm2 = auxSentAsm2 + auxSentAsm;

                                auxSentAsm = "mov ah,09" + "\n" + "lea dx," + "salto " + "\n" + "int 21h" + "\n" + "\n";
                                auxSentAsm2 = auxSentAsm2 + auxSentAsm;
                            }
                            

                        } else if (lt[i].p == "scan")
                        {

                        } else if (lt[i].p == ":=")
                        {
                            op2 = pila_asm.Peek();
                            pila_asm.Pop();
                            op1 = pila_asm.Peek();
                            pila_asm.Pop();

                            foreach (KeyValuePair<string, string> nodo in list_var)
                            {
                                //Si el lexema que se encuentra en aux coincide con una del diccionario...
                                if (op1 == nodo.Key)
                                {
                                    if (nodo.Value == "int")
                                    {

                                    }
                                    else if (nodo.Value == "float")
                                    {

                                    }else if (nodo.Value == "string")
                                    {
                                        auxVarAsm = op1+" "+"db"+" "+op2+","+"'$'"+"\n"+"\n";
                                        auxVarAsm2 = auxVarAsm2 + auxVarAsm;
                                        
                                    }
                                    else if (nodo.Value == "bool")
                                    {

                                    }
                                }             
                            }
                        }

                    }
                    else if (lt[i].f == "etiqueta")
                    {

                    }else if (lt[i].f == "apuntador")
                    {

                    }
                } catch {
                    MessageBox.Show("Checar la lista de postfijo y el metodo analizarPostAsm");
                }
            }    
        }

        public void PrepararAsm()
        {
            AnalizarPostAsm();
            asm = ".model small" + "\n" + ".stack"+ "\n";
            asm = asm + "\n";
            asm = asm + ".data" + "\n" + "\n";
            asm = asm + "salto db ' ',10,13,'$'" + "\n" + "\n";
            asm = asm + auxVarAsm2;
            asm = asm + "\n";
            asm = asm + ".code" + "\n" + "\n" + "mov ax,seg @data" + "\n" + "mov ds,ax" + "\n" + "\n";
            asm = asm + auxSentAsm2;
            asm = asm + "\n";
            asm = asm + ".exit" + "\n" + "end";
            asm = asm + "result" + "\n" + "dw0";
            StreamWriter archivo = new StreamWriter(@"C:\masm611\MASM611\BIN\archivo.asm");
            archivo.WriteLine(asm);
            archivo.Close();
        }

        /*--------------------------------------------------------SISTEMA DE TIPOS ASM-----------------------------------------------------------------
        te pongo aqui el sistema de tipos que mas o menos logro entender yo con el asm pero no se como integrarlo con todas las variables que pusiste porque por mas que trate no le entendi, sorry */
        public void SistemaTiposEnsamblador(List<string> posfijoP, List<int> posfijoPTokens)
        {
            this.posfijoP = posfijoP;
            this.posfijoPTokens = posfijoPTokens;

            asm = ""; instruccion = ""; auxVarAsm = ""; auxVarAsm2 = ""; resultado = ""; vartemp = "";
            for (int i = 0; i < posfijoP.Count; i++)
            {
                if (posfijoPTokens.ElementAt(i) == 100 || posfijoPTokens.ElementAt(i) == 101 || posfijoPTokens.ElementAt(i) == 102 ||
                    posfijoPTokens.ElementAt(i) == 124 || posfijoPTokens.ElementAt(i) == 212 || posfijoPTokens.ElementAt(i) == 113) // ID, Ent, Dec, Cadena, True, False
                {
                    Infijo.Push(posfijoP.ElementAt(i));
                    InfijoTokens.Push(posfijoPTokens.ElementAt(i));
                }
                else
                {

                    if (posfijoP.ElementAt(i) == "+")
                    {

                        auxVarAsm2 = Infijo.Pop();
                        InfijoTokens.Pop();

                        auxVarAsm = Infijo.Pop();
                        InfijoTokens.Pop();

                        instruccion = instruccion + "\n SUMAR " + auxVarAsm + "," + auxVarAsm2 + ", resul";

                        Infijo.Push("resul");
                        InfijoTokens.Push(101);
                    }

                    else if (posfijoP.ElementAt(i) == "-")
                    {
                        auxVarAsm2 = Infijo.Pop();
                        InfijoTokens.Pop();

                        auxVarAsm = Infijo.Pop();
                        InfijoTokens.Pop();

                        instruccion = instruccion + "\n RESTA " + auxVarAsm + "," + auxVarAsm2 + ", resul";

                        Infijo.Push("resul");
                        InfijoTokens.Push(101);
                    }

                    else if (posfijoP.ElementAt(i) == "*")
                    {
                        auxVarAsm2 = Infijo.Pop();
                        InfijoTokens.Pop();

                        auxVarAsm = Infijo.Pop();
                        InfijoTokens.Pop();

                        instruccion = instruccion + "\n MULTI " + auxVarAsm + "," + auxVarAsm2 + ", resul";

                        Infijo.Push("resul");
                        InfijoTokens.Push(101);
                    }

                    else if (posfijoP.ElementAt(i) == "/")
                    {
                        auxVarAsm2 = Infijo.Pop();
                        InfijoTokens.Pop();

                        auxVarAsm = Infijo.Pop();
                        InfijoTokens.Pop();

                        instruccion = instruccion + "\n DIVIDE " + auxVarAsm + "," + auxVarAsm2 + ", resul";

                        Infijo.Push("resul");
                        InfijoTokens.Push(101);

                    }

                    else if (posfijoP.ElementAt(i) == "") //aqui va el signo que usaste para lo del menos unitario
                    {
                        auxVarAsm2 = Infijo.Pop();
                        InfijoTokens.Pop();

                        auxVarAsm = Infijo.Pop();
                        InfijoTokens.Pop();

                        instruccion = instruccion + "\n RESTA " + 0 + "," + auxVarAsm2 + ", resul";

                        Infijo.Push("resul");
                        InfijoTokens.Push(101);
                    }

                    else if (posfijoP.ElementAt(i) == ":=")
                    {
                        /*ID*/
                        if (InfijoTokens.Peek() == 100)
                        {
                            auxVarAsm2 = Infijo.Pop();
                            InfijoTokens.Pop();
                            auxVarAsm = Infijo.Pop();
                            InfijoTokens.Pop();

                            instruccion = instruccion + "\n S_ASIGNATION " + auxVarAsm + "," + auxVarAsm2;
                        }

                        /*INT*/
                        else if (InfijoTokens.Peek() == 101)
                        {
                            auxVarAsm2 = Infijo.Pop();
                            InfijoTokens.Pop();
                            auxVarAsm = Infijo.Pop();
                            InfijoTokens.Pop();

                            instruccion = instruccion + "\n I_ASIGNATION " + auxVarAsm + "," + auxVarAsm2;
                        }

                        /*TRUE*/
                        else if (InfijoTokens.Peek() == 212)
                        {
                            auxVarAsm2 = Infijo.Pop();
                            InfijoTokens.Pop();
                            auxVarAsm = Infijo.Pop();
                            InfijoTokens.Pop();

                            instruccion = instruccion + "\n I_ASIGNATION " + auxVarAsm + "," + auxVarAsm2;
                        }

                        /*FALSE*/
                        else if (InfijoTokens.Peek() == 213)
                        {
                            auxVarAsm2 = Infijo.Pop();
                            InfijoTokens.Pop();
                            auxVarAsm = Infijo.Pop();
                            InfijoTokens.Pop();

                            instruccion = instruccion + "\n I_ASIGNATION " + auxVarAsm + "," + auxVarAsm2;
                        }

                        /*CADENA*/
                        else if (InfijoTokens.Peek() == 120)
                        {
                            auxVarAsm2 = Infijo.Pop();
                            InfijoTokens.Pop();
                            auxVarAsm = Infijo.Pop();
                            InfijoTokens.Pop();

                            asm = asm + "\n\t Cadena" + contAsm + "  DB '" + auxVarAsm2.Substring(1, auxVarAsm2.Length - 2) + "','$'";

                            instruccion = instruccion + "\n S_ASIGNATION " + auxVarAsm + ", Cadena" + contAsm;
                            contAsm++;
                        }
                    }

                    else if (posfijoP.ElementAt(i) == "<")
                    {
                        auxVarAsm2 = Infijo.Pop();
                        InfijoTokens.Pop();

                        auxVarAsm = Infijo.Pop();
                        InfijoTokens.Pop();

                        instruccion = instruccion + "\n I_MENOR " + auxVarAsm + "," + auxVarAsm2 + ", resul";

                        Infijo.Push("resul");
                        InfijoTokens.Push(101);
                    }

                    else if (posfijoP.ElementAt(i) == ">")
                    {
                        auxVarAsm2 = Infijo.Pop();
                        InfijoTokens.Pop();

                        auxVarAsm = Infijo.Pop();
                        InfijoTokens.Pop();

                        instruccion = instruccion + "\n I_MAYOR " + auxVarAsm + "," + auxVarAsm2 + ", resul";

                        Infijo.Push("resul");
                        InfijoTokens.Push(101);
                    }

                    else if (posfijoP.ElementAt(i) == "<=")
                    {
                        auxVarAsm2 = Infijo.Pop();
                        InfijoTokens.Pop();

                        auxVarAsm = Infijo.Pop();
                        InfijoTokens.Pop();

                        instruccion = instruccion + "\n I_MENORIGUAL " + auxVarAsm + "," + auxVarAsm2 + ", resul";

                        Infijo.Push("resul");
                        InfijoTokens.Push(101);
                    }

                    else if (posfijoP.ElementAt(i) == ">=")
                    {
                        auxVarAsm2 = Infijo.Pop();
                        InfijoTokens.Pop();

                        auxVarAsm = Infijo.Pop();
                        InfijoTokens.Pop();

                        instruccion = instruccion + "\n I_MAYORIGUAL " + auxVarAsm + "," + auxVarAsm2 + ", resul";

                        Infijo.Push("resul");
                        InfijoTokens.Push(101);
                    }

                    else if (posfijoP.ElementAt(i) == "==")
                    {
                        auxVarAsm2 = Infijo.Pop();
                        InfijoTokens.Pop();

                        auxVarAsm = Infijo.Pop();
                        InfijoTokens.Pop();

                        instruccion = instruccion + "\n I_IGUAL " + auxVarAsm + "," + auxVarAsm2 + ", resul";

                        Infijo.Push("resul");
                        InfijoTokens.Push(101);
                    }

                    else if (posfijoP.ElementAt(i) == "!=")
                    {
                        auxVarAsm2 = Infijo.Pop();
                        InfijoTokens.Pop();

                        auxVarAsm = Infijo.Pop();
                        InfijoTokens.Pop();

                        instruccion = instruccion + "\n I_DIFERENTES " + auxVarAsm + "," + auxVarAsm2 + ", resul";

                        Infijo.Push("resul");
                        InfijoTokens.Push(101);
                    }
                    /*SCAN*/
                    else if (posfijoPTokens.ElementAt(i) == 204)
                    {
                        InfijoTokens.Pop();

                        instruccion = instruccion + "\nreadstring@ " + Infijo.Pop();
                        instruccion = instruccion + "\n WRITELN";
                    }
                    /*PRINT*/
                    else if (posfijoPTokens.ElementAt(i) == 203)
                    {
                        /*INT*/
                        if (InfijoTokens.Peek() == 101)
                        {
                            auxVarAsm = Infijo.Pop();
                            InfijoTokens.Pop();

                            instruccion = instruccion + "\n ITOA  resul, " + auxVarAsm;

                            Infijo.Push("resul");

                            instruccion = instruccion + "\n WRITEN " + Infijo.Pop();
                            instruccion = instruccion + "\n WRITE salta";
                        }
                        /*CADENA*/
                        else if (InfijoTokens.Peek() == 124)
                        {
                            auxVarAsm = Infijo.Pop();
                            InfijoTokens.Pop();

                            instruccion = instruccion + "\n WRITE " + auxVarAsm;
                            instruccion = instruccion + "\n WRITELN ";
                        }
                        /*ID*/
                        else if (InfijoTokens.Peek() == 100)
                        {
                            auxVarAsm = Infijo.Pop();
                            InfijoTokens.Pop();

                            instruccion = instruccion + "\n ITOA  resul, " + auxVarAsm;
                            Infijo.Push("resul");

                            instruccion = instruccion + "\n WRITE " + Infijo.Pop();
                            
                            instruccion = instruccion + "\n WRITELN ";
                        }

                    }

                    else if (posfijoP.ElementAt(i) == "BRF-A")
                    {
                        auxVarAsm = Infijo.Pop();
                        InfijoTokens.Pop();

                        instruccion = instruccion + "\n Jf " + auxVarAsm + "," + "A";
                    }

                    else if (posfijoP.ElementAt(i) == "BRI-B")
                    {
                        instruccion = instruccion + "\n JMAY " + 1 + "," + "B";
                    }

                    else if (posfijoP.ElementAt(i) == "BRF-C")
                    {
                        auxVarAsm = Infijo.Pop();
                        InfijoTokens.Pop();

                        instruccion = instruccion + "\n Jf " + auxVarAsm + "," + "C";
                    }

                    else if (posfijoP.ElementAt(i) == "BRI-D")
                    {
                        instruccion = instruccion + "\n JMAY " + 1 + "," + "D";
                    }

                    else if (posfijoP.ElementAt(i) == "A:")
                    {
                        instruccion = instruccion + "\n\n " + "A:";
                    }

                    else if (posfijoP.ElementAt(i) == "B:")
                    {
                        instruccion = instruccion + "\n\n " + "B:";
                    }

                    else if (posfijoP.ElementAt(i) == "C:")
                    {
                        instruccion = instruccion + "\n\n " + "C:";
                    }

                    else if (posfijoP.ElementAt(i) == "D:")
                    {
                        instruccion = instruccion + "\n\n " + "D:";
                    }
                }
            }

            instruccion = instruccion + "READ";
        }

    }
}
