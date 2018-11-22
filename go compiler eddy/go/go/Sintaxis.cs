using System;
using System.Collections.Generic;
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
        int puntero = 0;
        public DataGridView resultados;


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
                    else
                    {
                        llenafallasintaxis2();
                        break;
                    }
                }
            }
            catch
            {
                llenafallasintaxis();

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
                        MessageBox.Show("exito");
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

        public int Statement_list()
        {
            try
            {
                /*             identificador                           if                              print                        scan           */
                if (lista_nodos[puntero].t == 100 || lista_nodos[puntero].t == 209 || lista_nodos[puntero].t == 203 || lista_nodos[puntero].t == 204 || lista_nodos[puntero].t == 211 || lista_nodos[puntero].t == 210 || lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102)
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
                while (lista_nodos[puntero].t == 100 || lista_nodos[puntero].t == 209 || lista_nodos[puntero].t == 203 || lista_nodos[puntero].t == 204 || lista_nodos[puntero].t == 210 || lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102 || lista_nodos[puntero].t == 211)
                {
                    //Declaracion de la variable
                    if (lista_nodos[puntero].t == 100)
                    {
                        puntero = vardec();
                    }
                    //if
                    else if (lista_nodos[puntero].t == 209)
                    {
                        puntero = EsIf();                     
                   
                    }
                    //else
                    else if (lista_nodos[puntero].t == 210)
                    {
                        puntero = EsElse();
                    }
                    //print
                    else  if (lista_nodos[puntero].t == 203 )
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
                        puntero = EsFor();
                    }else if (lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102) ;
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

        public int vardec()
        {
            try
            { //id
                if (lista_nodos[puntero].t == 100)
                {
                    puntero++;
                    //:=
                    if (lista_nodos[puntero].t == 123)
                    {
                        puntero++;
                        if (lista_nodos[puntero].t == 100 || lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102 || lista_nodos[puntero].t == 124 || lista_nodos[puntero].t == 212 || lista_nodos[puntero].t == 213)
                        {
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
            }
            catch
            {
                llenafallasintaxis();
            }
            return puntero;
        }

        public int EsIf()
        {
            try
            {
                // If
                if (lista_nodos[puntero].t == 209)
                {
                    puntero++;
                    //(
                    if (lista_nodos[puntero].t == 106)
                    {
                        puntero++;
                        puntero = exp_rel();
                    

                        //)
                        if (lista_nodos[puntero].t == 107)
                        {
                            puntero++;

                            //{
                            if (lista_nodos[puntero].t == 108)
                            {
                                puntero++;
                                Statement();
                                
                                //}
                                if (lista_nodos[puntero].t == 109)
                                {
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

        public int EsFor()
        {
            try
            {
                if (lista_nodos[puntero].t == 211) //for
                {
                    puntero++;
                    if (lista_nodos[puntero].t == 106) //(
                    {
                        puntero++;
                        if (lista_nodos[puntero].t == 100) //ID
                        {
                            puntero++;
                            if (lista_nodos[puntero].t == 123) //:=
                            {
                                puntero++;
                                puntero = exp_num();
                                if (lista_nodos[puntero].t == 104) //;
                                {
                                    puntero++;
                                    if (lista_nodos[puntero].t == 100) //ID
                                    {
                                        puntero++;
                                        if (lista_nodos[puntero].t == 119 || lista_nodos[puntero].t == 120 || lista_nodos[puntero].t == 121 || lista_nodos[puntero].t == 122) // op_rel
                                        {
                                            puntero++;
                                            puntero = op_rel();

                                            if (lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102) //valor_num
                                            {
                                                puntero++;
                                                puntero = valor_num();

                                                if (lista_nodos[puntero].t == 104) //;
                                                {
                                                    puntero++;
                                                    if (lista_nodos[puntero].t == 100) //ID
                                                    {
                                                        puntero++;
                                                        if (lista_nodos[puntero].t == 112) //+
                                                        {
                                                            puntero++;
                                                            if (lista_nodos[puntero].t == 112) //+
                                                            {
                                                                puntero++;
                                                                if (lista_nodos[puntero].t == 107) //)
                                                                {
                                                                    puntero++;
                                                                    if (lista_nodos[puntero].t == 108) //{
                                                                    {
                                                                        puntero++;
                                                                        Statement();

                                                                        if (lista_nodos[puntero].t == 214) //break
                                                                        {
                                                                            puntero++;
                                                                            if (lista_nodos[puntero].t == 109) //}
                                                                            {
                                                                                puntero++;
                                                                            }
                                                                            else
                                                                            {
                                                                                llenafallasintaxis2();
                                                                            }
                                                                        }
                                                                        else if (lista_nodos[puntero].t == 215) //continue
                                                                        {
                                                                            puntero++;
                                                                            if (lista_nodos[puntero].t == 109) //}
                                                                            {
                                                                                puntero++;
                                                                            }
                                                                            else
                                                                            {
                                                                                llenafallasintaxis2();
                                                                            }
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
                                                        else if (lista_nodos[puntero].t == 113) //-
                                                        {
                                                            puntero++;
                                                            if (lista_nodos[puntero].t == 113) //-
                                                            {
                                                                puntero++;
                                                                if (lista_nodos[puntero].t == 107) //)
                                                                {
                                                                    puntero++;
                                                                    if (lista_nodos[puntero].t == 108) //{
                                                                    {
                                                                        puntero++;
                                                                        Statement();

                                                                        if (lista_nodos[puntero].t == 214) //break
                                                                        {
                                                                            puntero++;
                                                                            if (lista_nodos[puntero].t == 109) //}
                                                                            {
                                                                                puntero++;
                                                                            }
                                                                            else
                                                                            {
                                                                                llenafallasintaxis2();
                                                                            }
                                                                        }
                                                                        else if (lista_nodos[puntero].t == 215) //continue
                                                                        {
                                                                            puntero++;
                                                                            if (lista_nodos[puntero].t == 109) //}
                                                                            {
                                                                                puntero++;
                                                                            }
                                                                            else
                                                                            {
                                                                                llenafallasintaxis2();
                                                                            }
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


        public int EsScan()
        {
            try
            {
                if (lista_nodos[puntero].t == 204) //print
                {
                    puntero++;
                    if (lista_nodos[puntero].t == 106) //(
                    {
                        puntero++;
                        if (lista_nodos[puntero].t == 124) //""
                        {
                            puntero++;
                            if (lista_nodos[puntero].t == 107)
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
                                        llenafallasintaxis2();
                                    }
                                }

                            }
                        }
                        else if (lista_nodos[puntero].t == 100)
                        {
                            puntero++;
                            if (lista_nodos[puntero].t == 107) //)
                            {
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
                }

            }
            catch
            {

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

            }
            return puntero;
        }

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
                MessageBox.Show("Error");
            }
            return puntero;
        }

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
                MessageBox.Show("Error");
            }
            return puntero;
        }

        public int valor_log()
        {
            try
            {  //               true                             false
                if (lista_nodos[puntero].t == 212 || lista_nodos[puntero].t == 213)
                {
                    puntero++;
                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
            return puntero;
        }

        public int op_log1()
        {
            try
            {
                // !=
                if (lista_nodos[puntero].t == 118)
                {
                    puntero++;
                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
            return puntero;
        }

        public int op_log2()
        {
            try
            {
                //        &&                                ||
                if (lista_nodos[puntero].t == 125 || lista_nodos[puntero].t == 126)
                {
                    puntero++;
                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
            return puntero;
        }

        public int op_num()
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
                MessageBox.Show("Error");
            }
            return puntero;
        }

        public int op_rel()
        {
            try
            {
                //        <                                   >                                           <=                      >=                                !=
                if (lista_nodos[puntero].t == 119 || lista_nodos[puntero].t == 120 || lista_nodos[puntero].t == 121 || lista_nodos[puntero].t == 122 || lista_nodos[puntero].t == 118 || lista_nodos[puntero].t == 129)
                {
                    puntero++;
                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
            return puntero;
        }
        
        public int exp_rel()
        {
            try
            {
                //         valor numerico
                if (lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102)
                {
                    puntero++;
                    puntero = valor_num();

                    //op_rel
                    if (lista_nodos[puntero].t == 119 || lista_nodos[puntero].t == 120 || lista_nodos[puntero].t == 121 || lista_nodos[puntero].t == 122 || lista_nodos[puntero].t == 118 || lista_nodos[puntero].t == 129)
                    {
                        puntero++;
                        puntero = op_rel();

                        //valor numerico
                        if (lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102)
                        {
                            puntero++;
                            puntero = valor_num();
                        }
                        else
                        {
                            if (lista_nodos[puntero].t == 100)
                            {
                                puntero++;
                            }
                        }
                    }
                    else
                    {
                        llenafallasintaxis2();
                    }
                } //id
                else if (lista_nodos[puntero].t == 100)
                {
                    puntero++;

                    //op_rel
                    if (lista_nodos[puntero].t == 119 || lista_nodos[puntero].t == 120 || lista_nodos[puntero].t == 121 || lista_nodos[puntero].t == 122 || lista_nodos[puntero].t == 118 || lista_nodos[puntero].t == 129)
                    {
                        puntero++;
                        puntero = op_rel();

                        //id
                        if (lista_nodos[puntero].t == 100)
                        {
                            puntero++;

                        }
                        else
                        {
                            //valor numerico
                            if (lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102)
                            {
                                puntero++;
                                puntero = valor_num();
                            }
                        }
                    }
                    else
                    {
                        llenafallasintaxis2();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Error");
            }
            return puntero;
        }

        public int EsPrint()
        {
            try
            {
                if (lista_nodos[puntero].t == 203) //print
                {
                    puntero++;
                    if (lista_nodos[puntero].t == 106) //(
                    {
                        puntero++;
                        if (lista_nodos[puntero].t == 124) //""
                        {
                            puntero++;
                            if (lista_nodos[puntero].t == 107)
                            {
                                puntero++;
                            }
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
                                        llenafallasintaxis2();
                                    }
                                }
                               
                            }
                        }
                        else if (lista_nodos[puntero].t == 100)
                        {
                            puntero++;
                            if (lista_nodos[puntero].t == 107) //)
                            {
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
                }
            }
            catch
            {
                llenafallasintaxis();
            }
            return puntero;
        }

        public int exp_log()
        {
            try
            {
                if (lista_nodos[puntero].t == 100 || lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102)
                {
                    puntero++;
                    puntero = exp_rel();
                    if (lista_nodos[puntero].t == 118 || lista_nodos[puntero].t == 125 || lista_nodos[puntero].t == 126)
                    {
                        puntero++;
                        puntero = op_log1();
                        puntero = op_log2();
                        if (lista_nodos[puntero].t == 100 || lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102)
                        {
                            puntero++;
                            puntero = exp_rel();
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
            }
            catch
            {
                MessageBox.Show("Error");
            }
            return puntero;
        }

        public int exp_num()
        {
            try
            {
              /*  if (lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102) // valor numerico
                {
                    puntero++;
                    puntero = valor_num();
                }
                else*/ if (lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102) //valor numerico
                {
                    puntero++;
                    puntero = valor_num();

                    if (lista_nodos[puntero].t == 112 || lista_nodos[puntero].t == 113 || lista_nodos[puntero].t == 117 || lista_nodos[puntero].t == 115) //op_num
                    {
                        puntero++;
                        puntero = op_num();

                        if (lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102) //valor numerico
                        {
                            puntero++;
                            puntero = valor_num();
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
                else if (lista_nodos[puntero].t == 112 || lista_nodos[puntero].t == 113 || lista_nodos[puntero].t == 117 || lista_nodos[puntero].t == 115) // op_num
                {
                    puntero++;
                    puntero = op_num();

                    if (lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102) //valor numerico
                    {
                        puntero++;
                        puntero = valor_num();
                    }
                    else
                    {
                        llenafallasintaxis2();
                    }
                }
                else if (lista_nodos[puntero].t == 112 || lista_nodos[puntero].t == 113 || lista_nodos[puntero].t == 117 || lista_nodos[puntero].t == 115) // op_num
                {
                    puntero++;
                    puntero = op_num();

                    if (lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102) //valor numerico
                    {
                        puntero++;
                        puntero = valor_num();

                        if (lista_nodos[puntero].t == 112 || lista_nodos[puntero].t == 113 || lista_nodos[puntero].t == 117 || lista_nodos[puntero].t == 115) // op_num
                        {
                            puntero++;
                            puntero = op_num();

                            if (lista_nodos[puntero].t == 101 || lista_nodos[puntero].t == 102) //valor numerico
                            {
                                puntero++;
                                puntero = valor_num();
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
            }
            catch
            {
                MessageBox.Show("Error");
            }
            return puntero++;
        }

        public void llenafallasintaxis()
        {

            MessageBox.Show("Termino La Lista En " + lista_nodos[puntero].l + ", No Se Completo El Programa");
        }

        public void llenafallasintaxis2()
        {
            resultados.Rows.Add(lista_nodos[puntero].t, "Error Con " + lista_nodos[puntero].l, lista_nodos[puntero-1].r);

        }

    }

   
}
