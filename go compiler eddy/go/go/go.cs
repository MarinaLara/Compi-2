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
            string ubicacion = @"D:\Escritorio\9veno Semestre\go compiler\lexico.go";
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
            Sintaxis clase_sintaxis = new Sintaxis(listaNodos);
            clase_sintaxis.resultados = dataGridView2;
            clase_sintaxis.analizador();
        }

      
    }
}
