﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gocompi
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }


        #region 'variables'
        //VARIABLES PARA PESTAÑAS
        ArrayList ListaPestaña = new ArrayList();
        int ContarPestaña = 1;

        //Lexico
        int token = 0;
        int estado = 0;
        int colum = 0;
        int contlinea = 1;
        char[] longitudtexto;
        int longitud;
        int i = 0;
        string cadenacaracter = "";

        //Lexico Errores
        string error, tipo;
        char caracter;

        //Final de la Línea 
        public Boolean finaldeLinea = false;

        public struct Tokenl
        {
            public string palabracadena { get; set; }
            public string tipo { get; set; }
            public int Tkn { get; set; }
            public int acomuladorlineaL { get; set; }

        }

        public List<Tokenl> ListaTokens = new List<Tokenl>();

        //sintactico
        int puntero = 0;

        string aux;
        string tipo;
        string unitario;



        //Cola auxiliar de variable
        Queue cola_var = new Queue();
        Queue cola_var2 = new Queue();


        //Lista de variables
        Dictionary<string, string> list_var = new Dictionary<string, string>();

                //----------------------------------------------------PREPARAR POSFIJO------------------------------------------------------------------

        //Lista para ensamblador
     
        List<string> lista_posfijo = new List<string>();

        //Pila de etiquetas para el posfijo
        Stack<string> pila_etiquetas = new Stack<string>();
        Stack<string> pila_BRF_A = new Stack<string>();
        Stack<string> pila_BRI_B = new Stack<string>();
        Stack<string> pila_BRF_C = new Stack<string>();
        Stack<string> pila_BRI_D = new Stack<string>();
        Stack<string> pila_BRF_E = new Stack<string>();
        Stack<string> pila_BRI_F = new Stack<string>();
        Stack<string> pila_Sentencia = new Stack<string>();
        int cont_a = 0;
        int cont_b = 0;
        int cont_c = 0;
        int cont_d = 0;
        int cont_e = 0;
        int cont_f = 0;
        int cont_s = 0;
        string vartip;

        Stack <string> auxIncremento = new Stack<string>();
        Stack <string> auxIncremento2= new Stack<string>();

        #endregion

        #region 'Otros'
        //CREAR NUEVA PESTAÑA
        private void nuevoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrearPestaña();
        }

        //CREA UN NUEVO CAMPO DE TEXTO
        private RichTextBox GetRichTextBox()
        {
            RichTextBox rtb = null;

            TabPage NuevaPestaña = tabControl1.SelectedTab;
            if (NuevaPestaña != null)
            {
                rtb = NuevaPestaña.Controls[0] as RichTextBox;

            }

            return rtb;
        }

        //CREAR PESTAÑA
        public void CrearPestaña()
        {
            // Creamos una nueva Pestaña
            TabPage NuevaPestaña = new TabPage("Nueva Pestaña " + ContarPestaña); // Creamos una nueva pestaña
            RichTextBox rtb = new RichTextBox();
            rtb.SelectionFont = new Font("Arial", 20, FontStyle.Regular);
            PictureBox pct = new PictureBox();
            pct.Dock = DockStyle.Left;
            pct.Height = 587;
            pct.Width = 50;
            pct.BackColor = Color.FromArgb(112, 112, 112);
            //            rtb.Lines
            rtb.Dock = DockStyle.Fill;
            ContarPestaña++; //variable que lleva el control de la cantidad de pestaña creada
            NuevaPestaña.Controls.Add(rtb);
            NuevaPestaña.Controls.Add(pct);
            tabControl1.TabPages.Add(NuevaPestaña);

            rtb.AcceptsTab = true;
        }

        //ELIMINAR PESTAÑA ACTUAL
        private void EliminarPestaña()
        {
            TabPage current_tab = tabControl1.SelectedTab;
            ListaPestaña.Remove(current_tab);
            tabControl1.TabPages.Remove(current_tab);
            ContarPestaña--;
        }

        //CERRAR LA PESTAÑA ACTUAL
        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EliminarPestaña();
        }

        //PARA ABRIR
        private void abrirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CrearPestaña();
            Stream myStream;
            OpenFileDialog OpenFileDialog1 = new OpenFileDialog();
            OpenFileDialog1.Filter = "Text [*.txt*]|*.txt|All Files [*,*]|*,*";
            OpenFileDialog1.CheckFileExists = true;
            OpenFileDialog1.Title = "Abrir Archivo";

            if (OpenFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {

                if ((myStream = OpenFileDialog1.OpenFile()) != null)
                {
                    string strfilename = OpenFileDialog1.FileName;
                    string filetext = File.ReadAllText(strfilename);
                    GetRichTextBox().Text = filetext;
                }
            } 
        }

        //PARA GUARDAR
        private void guardarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog SaveFileDialog1 = new SaveFileDialog();
            SaveFileDialog1.Filter = "Text (*.txt)|*.txt|HTML(*.html*)|*.html|All files(*.*)|*.*";
            SaveFileDialog1.CheckPathExists = true;

            if (SaveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (Stream s = File.Open(SaveFileDialog1.FileName, FileMode.CreateNew))
                using (StreamWriter sw = new StreamWriter(s))
                {
                    sw.Write(GetRichTextBox().Text);
                }
            }
        }

        //PARA CORTAR
        private void cortarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetRichTextBox().Cut();
        }

        //PARA COPIAR
        private void copiarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetRichTextBox().Copy();
        }

        //PARA PEGAR
        private void pegarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetRichTextBox().Paste();
        }

        #endregion


        #region Botones
        //BOTON LEXICO
        private void lexicoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == null)
            {
                MessageBox.Show("No hay código a analizar");
            }
            else
            {
                AnalizadorLexico();
                MessageBox.Show("Analisis Lexico Completado");
            }
        }

        //BOTON SINTACTICO

        //BOTON SEMANTICO

        #endregion

        #region 'Lexico'

        public void AnalizadorLexico()
        {
            finaldeLinea = false;
            contlinea = 1;
            cadenacaracter = "";
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();

            string tmpText = tabControl1.SelectedTab.Controls[0].Text + "\n";
            longitudtexto = tmpText.ToCharArray();


            longitud = longitudtexto.Length;

            estado = 0;

            for (i = 0; i < longitud; i++)
            {
                caracter = longitudtexto[i];
                colum = insColumn(caracter);
                estado = Matriz[estado, colum];

                if (!((caracter == ' ' || caracter == '\n' || caracter == '\t') && estado == 0))
                {
                    cadenacaracter = cadenacaracter + caracter;
                }



                if (longitud == i + 1)
                {
                    if (estado == 0)
                    {
                        estado = Matriz[estado, 25];
                    }
                    token = estado;
                    finaldeLinea = true;
                }

                if (estado >= 101 || estado == 150)
                {
                    if (estado == 129 ||estado == 127 || estado == 150 || estado == 130 || estado == 133 || estado == 134 || estado == 135 || estado == 136 || estado == 137 || 
                        estado == 500 || estado == 501 || estado == 502 || estado == 503 || estado == 504 || estado == 505 || estado == 506 || estado == 507)
                    {
                        
                    }
                    else
                        if (cadenacaracter.Length > 1)
                        {
                            cadenacaracter = cadenacaracter.Substring(0, cadenacaracter.Length - 1);
                            i = i - 1;
                        }
                    token = estado;


                    if (token == 101) //identificadores
                    {

                        palabrasReservadas(cadenacaracter);
                        int token2 = token;
                        tipoPalabra(token2);
                        dataGridView1.Rows.Add(cadenacaracter, token, contlinea, tipo);

                        Tokenl nuevotoken = new Tokenl { palabracadena = cadenacaracter, Tkn = token, acomuladorlineaL = contlinea };
                        ListaTokens.Add(nuevotoken);

                        cadenacaracter = "";

                    }
                    else if (token == 150)
                    {
                        int token2 = token;
                        tipoPalabra(token2);
                        cadenacaracter = "";
                    }
                    else
                        if (token >= 500 && token <= 507) //errores
                        {
                            if ((cadenacaracter.Length - 1) != caracter)
                            {
                                cadenacaracter = cadenacaracter.Substring(0, cadenacaracter.Length - 1);
                                
                                cadenacaracter += caracter;
                            }


                            while (!((caracter == ' ' || caracter == '\n' || caracter == '\t')))
                            {
                                i = i + 1;
                                caracter = longitudtexto[i];
                                //colum = insColumn(caracter);
                                //estado = Matriz[estado, colum];

                                if (!((caracter == ' ' || caracter == '\n' || caracter == '\t')))
                                {
                                    cadenacaracter = cadenacaracter + caracter;
                                }
                               
                            }

                            errores(token);
                            dataGridView2.Rows.Add(token, cadenacaracter, contlinea, error);
                            cadenacaracter = "";

                            
                        }
                        else
                        {
                            int token2 = token;
                            tipoPalabra(token2);
                            dataGridView1.Rows.Add(cadenacaracter, token, contlinea, tipo);
                            //cali
                            Tokenl nuevotoken = new Tokenl { palabracadena = cadenacaracter, Tkn = token, acomuladorlineaL = contlinea };
                            ListaTokens.Add(nuevotoken);



                            cadenacaracter = "";
                        }

                    estado = 0;
                    caracter = ' ';
                    error = "";
                }

                if (caracter == '\n')
                {
                    contlinea++;
                }

            }
        }

        public void insertarResultados()
        {
            dataGridView1.Rows.Add(estado);
        }

        public void inicializarValores()
        {
            estado = 0;
            token = 0;

        }

        //INSERTAR COLUMNA
        public int insColumn(char caracter)
        {
            if (char.IsLetter(caracter))
            {
                colum = 0; //letra

            }
            else if (char.IsDigit(caracter))
            {
                colum = 1; //numero
            }
            else
            {
                #region 'switch'
                switch (caracter)
                {
                    case '.':
                        colum = 2;
                        break;
                    case ';':
                        colum = 3;
                        break;
                    case ',':
                        colum = 4;
                        break;
                    case '+':
                        colum = 5;
                        break;
                    case '-':
                        colum = 6;
                        break;
                    case '*':
                        colum = 7;
                        break;
                    case '/':
                        colum = 8;
                        break;
                    case '%':
                        colum = 9;
                        break;
                    case '(':
                        colum = 10;
                        break;
                    case ')':
                        colum = 11;
                        break;
                    case '{':
                        colum = 12;
                        break;
                    case '}':
                        colum = 13;
                        break;
                    case '[':
                        colum = 14;
                        break;
                    case ']':
                        colum = 15;
                        break;
                    case '=':
                        colum = 16;
                        break;
                    case ':':
                        colum = 17;
                        break;
                    case '!':
                        colum = 18;
                        break;
                    case '<':
                        colum = 19;
                        break;
                    case '>':
                        colum = 20;
                        break;
                    case '&':
                        colum = 21;
                        break;
                    case '|':
                        colum = 22;
                        break;
                    case '"':
                        colum = 23;
                        break;
                    case '\n':
                        colum = 24; //ln
                        break;
                    case ' ': //espacio
                        colum = 27;
                        break;
                    case '\t':
                        colum = 26; //tab
                        break;
                    case '#': //oc
                        colum = 28;
                        break;
                    default:
                        colum = 28; //oc    el 25 es eof
                        break;
                }
                #endregion
            }

            return colum;
        }

        #region Matriz
        int[,] Matriz = { 
                            //	A-Z	,	0-9	,	.	,	;	,	,	,	+	,	-	,	*	,	/	,	%	,	(	,	)	,	{	,	}	,	[	,	]	,	=	,	:	,	!	,	<	,	>	,	&	,	|	,	"	,	LN	,	EOF	,	TB	,	EB	,	OC	,
                        //	     0	,	1	,	2	,	3	,	4	,	5	,	6	,	7	,	8	,	9	,	10	,	11	,	12	,	13	,	14	,	15	,	16	,	17	,	18	,	19	,	20	,	21	,	22	,	23	,	24	,	25	,	26	,	27	,	28	,
                        /*0*/{	1	,	2	,	17	,	105	,	106	,	111	,	112	,	113	,	5	,	115	,	121	,	122	,	123	,	124	,	125	,	126	,	16	,	9	,	12	,	11	,	10	,	14	,	15	,	13	,	0	,	0	,	0	,	0	,	506	},
                        /*1*/{	1	,	1	,	101	,	101	,	101	,	101	,	101	,	101	,	101	,	101	,	101	,	101	,	101	,	101	,	101	,	101	,	101	,	101	,	101	,	101	,	101	,	101	,	101	,	101	,	101	,	101	,	101	,	101	,	101	},
                        /*2*/{	507	,	2	,	3	,	102	,	102	,	102	,	102	,	102	,	102	,	102	,	102	,	102	,	102	,	102	,	102	,	102	,	102	,	102	,	102	,	102	,	102	,	102	,	102	,	102	,	102	,	102	,	102	,	102	,	102	},
                        /*3*/{	500	,	4	,	500	,	500	,	500	,	500	,	500	,	500	,	500	,	500	,	500	,	500	,	500	,	500	,	500	,	500	,	500	,	500	,	500	,	500	,	500	,	500	,	500	,	500	,	500	,	500	,	500	,	500	,	506	},
                        /*4*/{	500	,	4	,	500	,	103	,	103	,	103	,	103	,	103	,	103	,	103	,	103	,	103	,	103	,	103	,	103	,	103	,	103	,	103	,	103	,	103	,	103	,	103	,	103	,	103	,	103	,	103	,	103	,	103	,	103	},
                        /*5*/{	114	,	114	,	114	,	114	,	114	,	114	,	114	,	7	,	6	,	114	,	114	,	114	,	114	,	114	,	114	,	114	,	114	,	114	,	114	,	114	,	114	,	114	,	114	,	114	,	114	,	114	,	114	,	114	,	114	},
                        /*6*/{	6	,	6	,	6	,	6	,	6	,	6	,	6	,	6	,	6	,	6	,	6	,	6	,	6	,	6	,	6	,	6	,	6	,	6	,	6	,	6	,	6	,	6	,	6	,	6	,	150	,	150	,	6	,	6	,	6	},
                        /*7*/{	7	,	7	,	7	,	7	,	7	,	7	,	7	,	8	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	},
                        /*8*/{	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	150	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	7	,	501	,	7	,	7	,	7	},
                        /*9*/{	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	127	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	506	},
                        /*10*/{	132	,	132	,	132	,	132	,	132	,	132	,	132	,	132	,	132	,	132	,	132	,	132	,	132	,	132	,	132	,	132	,	134	,	132	,	132	,	132	,	132	,	132	,	132	,	132	,	132	,	132	,	132	,	132	,	132	},
                        /*11*/{	131	,	131	,	131	,	131	,	131	,	131	,	131	,	131	,	131	,	131	,	131	,	131	,	131	,	131	,	131	,	131	,	133	,	131	,	131	,	131	,	131	,	131	,	131	,	131	,	131	,	131	,	131	,	131	,	131	},
                        /*12*/{	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	130	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	},
                        /*13*/{	13	,	13	,	13	,	13	,	13	,	13	,	13	,	13	,	13	,	13	,	13	,	13	,	13	,	13	,	13	,	13	,	13	,	13	,	13	,	13	,	13	,	13	,	13	,	137	,	503	,	503	,	13	,	13	,	13	},
                        /*14*/{	504	,	504	,	504	,	504	,	504	,	504	,	504	,	504	,	504	,	504	,	504	,	504	,	504	,	504	,	504	,	504	,	504	,	504	,	504	,	504	,	504	,	135	,	504	,	504	,	504	,	504	,	504	,	504	,	506	},
                        /*15*/{	505	,	505	,	505	,	505	,	505	,	505	,	505	,	505	,	505	,	505	,	505	,	505	,	505	,	505	,	505	,	505	,	505	,	505	,	505	,	505	,	505	,	505	,	136	,	505	,	505	,	505	,	505	,	505	,	506	},
                        /*16*/{	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	129	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	502	,	506	},
                        /*17*/{	104	,	4	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	,	506	},


                        };
        #endregion       

        //PALABRAS RESERVADAS
        public int palabrasReservadas(string PalRes)
        {
            switch (PalRes)
            {
                case "package":
                    token = 200;
                    break;
                case "main":
                    token = 201;
                    break;
                case "func":
                    token = 202;
                    break;
                case "print":
                    token = 203;
                    break;
                case "scan":
                    token = 204;
                    break;
                case "var":
                    token = 205;
                    break;
                case "int":
                    token = 206;
                    break;
                case "string":
                    token = 207;
                    break;
                case "bool":
                    token = 208;
                    break;
                case "if":
                    token = 209;
                    break;
                case "else":
                    token = 210;
                    break;
                case "for":
                    token = 211;
                    break;
                case "true":
                    token = 212;
                    break;
                case "false":
                    token = 213;
                    break;
                case "break":
                    token = 214;
                    break;
                case "continue":
                    token = 215;
                    break;
                case "const":
                    token = 216;
                    break;
                case "type":
                    token = 217;
                    break;
                case "float":
                    token = 218;
                    break;
                default:
                    token = 101;
                    break;
            }

            return token;
        }

        //TIPO DE PALABRA
        public string tipoPalabra(int token2)
        {
            if (token2 == 101)
            {
                tipo = "identificador";
            }
            else if (token2 == 102)
            {
                tipo = "Numeros enteros";
            }
            else if (token2 == 103)
            {
                tipo = "Numeros decimales";
            }
            else if (token2 == 137)
            {
                tipo = "cadena";
            }
            else if (token2 == 127 || (token2 >= 129 && token2 <= 136))
            {
                tipo = "simbolo doble";
            }
            else if ((token2 >= 104 && token2 <= 106) || (token2 >= 111 && token2 <= 115) || (token2 >= 121 && token2 <= 126) || token == 131 || token == 132)
            {
                tipo = "simbolo sencillo";
            }
            else if (token2 >= 200 && token2 <= 218)
            {
                tipo = "palabras reservadas";
            }
            else if (token2 == 150)
            {
                tipo = "comentario";
            }

            return tipo;
        }

        //ERRORES
        public string errores(int token)
        {
            switch (token)
            {
                case 500:
                    error = "Se esperaba decimal";
                    break;
                case 501:
                    error = "Se esperaba cierre del comentario";
                    break;
                case 502:
                    error = "Se esperaba =";
                    break;
                case 503:
                    error = "Se esperaba cierre de comilla";
                    break;
                case 504:
                    error = "Se esperaba &";
                    break;
                case 505:
                    error = "Se esperaba |";
                    break;
                case 506:
                    error = "simbolo desconocido";
                    break;
                case 507:
                    error = "Se esperaba entero";
                    break;
            }

            return error;
        }
        
        #endregion


        #region 'Sintactico'

        public void ana() 
        {
            if (ListaTokens[puntero].Tkn == 200)
            {
                puntero++;

                //main
                if (ListaTokens[puntero].Tkn == 201)
                {
                    puntero++;

                    //func
                    if (ListaTokens[puntero].Tkn == 202)
                    {
                        puntero++;

                        //main
                        if (ListaTokens[puntero].Tkn == 201)
                        {
                            puntero++;

                            //(
                            if (ListaTokens[puntero].Tkn == 106)
                            {
                                puntero++;

                                //)
                                if (ListaTokens[puntero].Tkn == 107)
                                {
                                    puntero++;
                                    puntero = Bloque();
                                }
                                else
                                {
                                    llenafallasintaxis2();
                                    break;
                                }
                            }
                            else
                            {
                                llenafallasintaxis2();
                                break;
                            }
                        }
                        else
                        {
                            llenafallasintaxis2();
                            break;
                        }
                    }
                    else
                    {
                        llenafallasintaxis2();
                        break;
                    }
                }
                else
                {
                    llenafallasintaxis2();
                    break;
                }
            }
        }

        public void llenafallasintaxis()
        {

            MessageBox.Show("Termino La Lista En " + lista_nodos[puntero].l + ", No Se Completo El Programa");
        }

        public int Statement_list()
        {
            try
            {
                /*             identificador                           if                              print                        scan           */
                if (ListaTokens[puntero].Tkn == 100 || ListaTokens[puntero].Tkn == 209 || ListaTokens[puntero].Tkn == 203 || ListaTokens[puntero].Tkn == 204 || ListaTokens[puntero].Tkn == 211 || ListaTokens[puntero].Tkn == 210 || ListaTokens[puntero].Tkn == 101 || ListaTokens[puntero].Tkn == 102 || ListaTokens[puntero].Tkn == 205)
                {
                    puntero = Statement();
                }
            }
            catch
            {
                llenafallasintaxis();
            }
            return puntero;
        }

        public int Statement()
        {
            try
            {
                /*             identificador                           if                              print                        scan                            else */
                while (ListaTokens[puntero].Tkn == 100 || ListaTokens[puntero].Tkn == 209 || ListaTokens[puntero].Tkn == 203 || ListaTokens[puntero].Tkn == 204 || ListaTokens[puntero].Tkn == 210 || ListaTokens[puntero].Tkn == 101 || ListaTokens[puntero].Tkn == 102 || ListaTokens[puntero].Tkn == 211 || ListaTokens[puntero].Tkn == 205)
                {

                    //Declaracion de tipos 
                    if (ListaTokens[puntero].Tkn == 205)
                    {
                        puntero = vardec1();
                    }

                    //Declaracion de la variable
                    if (ListaTokens[puntero].Tkn == 100)
                    {
                        puntero = vardec();
                    }
                    //if
                    else if (ListaTokens[puntero].Tkn == 209)
                    {
                        puntero = EsIf();

                    }
                    //else
                    /*else if (ListaTokens[puntero].Tkn == 210)
                    {
                        puntero = EsElse();
                    }*/
                    //print
                    else if (ListaTokens[puntero].Tkn == 203)
                    {
                        puntero = EsPrint();
                    }
                    //scan
                    else if (ListaTokens[puntero].Tkn == 204)
                    {
                        puntero = EsScan();
                    }
                    //for
                    else if (ListaTokens[puntero].Tkn == 211)
                    {
                        puntero = EsFor();

                    }
                    //Expresion numerica
                    else if (ListaTokens[puntero].Tkn == 101 || ListaTokens[puntero].Tkn == 102)
                    {
                        puntero = exp_num();
                    }

                }
            }
            catch
            {
                llenafallasintaxis2();
            }
            return puntero;
        }

        public int vardec1()
        {
            try
            {   //var
                if (ListaTokens[puntero].Tkn == 205)
                {
                    puntero++;

                    //id
                    if (ListaTokens[puntero].Tkn == 100)
                    {


                        aux = ListaTokens[puntero].Tkn;
                        cola_var.Enqueue(aux);
                        puntero++;

                        if (ListaTokens[puntero].Tkn == 206 || ListaTokens[puntero].Tkn == 207 || ListaTokens[puntero].Tkn == 208 || ListaTokens[puntero].Tkn == 218)
                        {

                            tipo = ListaTokens[puntero].Tkn;
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
                        else if (ListaTokens[puntero].Tkn == 105)
                        {
                            vardec1aux();
                        }
                        else
                        {
                            llenafallasintaxis2();
                        }

                    }
                    else
                    {
                        llenafallasintaxis2();
                    }
                }
                else
                {
                    llenafallasintaxis2();
                }
            }
            catch
            {
                llenafallasintaxis();
            }

            return puntero;
        }

        public int vardec1aux()
        {
            //id
            if (ListaTokens[puntero].Tkn == 105)
            {
                puntero++;
                if (ListaTokens[puntero].Tkn == 100)
                {

                    aux = ListaTokens[puntero].Tkn;
                    cola_var.Enqueue(aux);
                    puntero++;

                    if (ListaTokens[puntero].Tkn == 105)
                    {

                        vardec1aux();
                    }
                    else if (ListaTokens[puntero].Tkn == 206 || ListaTokens[puntero].Tkn == 207 || ListaTokens[puntero].Tkn == 208 || ListaTokens[puntero].Tkn == 218)
                    {
                        tipo = aux = ListaTokens[puntero].Tkn;
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
                    else
                    {
                        llenafallasintaxis2();
                    }
                }
                else
                {
                    llenafallasintaxis2();
                }
            }
            else
            {
                llenafallasintaxis2();
            }
            return puntero;
        }

        public int EsIf()
        {
            var lp = new listPost();//******
            try
            {
                // If
                if (ListaTokens[puntero].Tkn == 209)
                {
                    //Etiquetas
                    cont_a++;
                    cont_b++;
                    cont_s++;
                    pila_BRF_A.Push("BRF-A" + cont_a);
                    pila_BRI_B.Push("BRI-B" + cont_b);
                    pila_etiquetas.Push("B" + cont_b);
                    pila_etiquetas.Push("A" + cont_a);
                    //pila_Sentencia se utiliza para agregar la etiqueta S"n" en la lista de postfijo:
                    pila_Sentencia.Push("S" + cont_s);
                    //pila_SentenciaAsm se utiliza para tomar el contador que se encuentra en la etiqueta S"n" en la lista de postfjio y utilizarla en las etiquetas del ensamblador:
                    pila_SentenciaAsm.Enqueue(Convert.TknoString(cont_s));




                    puntero++;
                    //(
                    if (ListaTokens[puntero].Tkn == 106)
                    {
                        puntero++;
                        puntero = exp_log();


                        //)
                        if (ListaTokens[puntero].Tkn == 107)
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
                            if (ListaTokens[puntero].Tkn == 108)
                            {


                                lp.p = pila_Sentencia.Peek();//************
                                lp.f = "sentencia";//************
                                lt.Add(lp);//*******
                                pila_Sentencia.Pop();

                                puntero++;
                                Statement();

                                //}
                                if (ListaTokens[puntero].Tkn == 109)
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

                                    if (ListaTokens[puntero].Tkn == 210)
                                    {
                                        cont_s++;
                                        //pila_Sentencia se utiliza para agregar la etiqueta S"n" en la lista de postfijo:
                                        pila_Sentencia.Push("S" + cont_s);
                                        //pila_SentenciaAsm se utiliza para tomar el contador que se encuentra en la etiqueta S"n" en la lista de postfjio y utilizarla en las etiquetas del ensamblador:
                                        pila_SentenciaAsm.Enqueue(Convert.TknoString(cont_s));

                                        lp.p = pila_Sentencia.Peek();//************
                                        lp.f = "sentencia";//************
                                        lt.Add(lp);//*******
                                        pila_Sentencia.Pop();

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
                                    llenafallasintaxis2();
                                }
                            }
                            else
                            {
                                llenafallasintaxis2();
                            }
                        }
                        else
                        {
                            llenafallasintaxis2();
                        }

                    }
                    else
                    {
                        llenafallasintaxis2();
                    }
                }
                else
                {
                    llenafallasintaxis2();
                }
            }
            catch
            {
                llenafallasintaxis();
            }
            return puntero;
        }
        #endregion


    }
}
