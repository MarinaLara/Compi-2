using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace go
{
    public partial class go : Form
    {

        public go()
        {
            InitializeComponent();
        }

        public List<Lista_nodos> listaNodos;





        private void LexicoBtn_Click(object sender, EventArgs e)
        {
            /*string ubicacion = @"C:\Users\windows\Desktop\Lenguajes y automatas\lexico.go";
            string leer = File.ReadAllText(ubicacion);
            txtArchivo.Text = leer;*/




            string ubicacion = @"C:\Users\admin\Desktop\Compilador go\lexico.go";
            string leer = File.ReadAllText(ubicacion);
            txtArchivo.Text = leer;
            string text = txtArchivo.Text;
            dataGridView1.Rows.Clear();
            dataGridView2.Rows.Clear();

            Lexico clase_lexico = new Lexico();
            clase_lexico.cadena = text;
            clase_lexico.resultados = dataGridView1;
            clase_lexico.analizadorlexico();






        }

        private void Archivotb_TextChanged(object sender, EventArgs e)
        {

        }

        private void sintacticoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            listaNodos = Lexico.lista_nodos;
            dataGridView3.Rows.Clear();
            dataGridView4.Rows.Clear();
            Sintaxis clase_sintaxis = new Sintaxis(listaNodos);
            clase_sintaxis.resultados = dataGridView2;
            clase_sintaxis.posfijo = dataGridView3;
            clase_sintaxis.variables = dataGridView4;
            clase_sintaxis.analizador();

            //}


        }
    }
}