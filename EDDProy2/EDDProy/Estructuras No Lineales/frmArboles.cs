using EDDemo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml.Linq;


namespace EDDemo.Estructuras_No_Lineales
{
    public partial class frmArbo : Form
    {
        ArbolBusqueda Arbol;
        NodoBinario Raiz;

        public frmArbo()
        {
            InitializeComponent();
            Arbol = new ArbolBusqueda();
            Raiz = null;
        }

        private void btnCrearArbol_Click(object sender, EventArgs e)
        {
            Arbol = null;
            Raiz = null;
            Arbol = new ArbolBusqueda();
            txtArbol.Text = "";
            txtDato.Text = "";
            Arbol.strArbol = "";

            Random rnd = new Random();
            for (int nNodos = 1; nNodos <= txtNodos.Value; nNodos++)
            {
                int Dato = rnd.Next(1, 100);
                Raiz = Arbol.RegresaRaiz();
                Arbol.InsertaNodo(Dato, ref Raiz);
            }

            Arbol.MuestraArbolAcostado(1, Raiz);
            txtArbol.Text = Arbol.strArbol;
            txtDato.Text = "";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            Raiz = Arbol.RegresaRaiz();
            Arbol.strArbol = "";
            Arbol.InsertaNodo(int.Parse(txtDato.Text), ref Raiz);
            Arbol.MuestraArbolAcostado(1, Raiz);
            txtArbol.Text = Arbol.strArbol;
            txtDato.Text = "";
        }

        private void btnBuscar_Click(object sender, EventArgs e)
        {
            int valorBuscado;
            bool Numero = int.TryParse(txtDato.Text, out valorBuscado);
            if (!Numero)
            {
                MessageBox.Show("Ingresa un valor numerico");
                return;
            }

            bool encontrado = Arbol.Busqueda(valorBuscado, Arbol.RegresaRaiz());
            if (encontrado)
                MessageBox.Show("Valor encontrado en el arbol.");
            else
                MessageBox.Show("Valor no encontrado en el arbol.");
        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtDato.Text))
                {
                    MessageBox.Show("Por favor ingrese un valor para eliminar");
                    return;
                }

                int datoEliminar = int.Parse(txtDato.Text);
                if (Arbol.EliminarNodo(datoEliminar))
                {
                    Raiz = Arbol.RegresaRaiz();
                    Arbol.strArbol = "";
                    Arbol.MuestraArbolAcostado(1, Raiz);
                    txtArbol.Text = Arbol.strArbol;
                    MessageBox.Show($"El nodo {datoEliminar} ha sido eliminado");
                }
                else
                {
                    MessageBox.Show($"El nodo {datoEliminar} no se encontro en el arbol");
                }
                txtDato.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error al eliminar el nodo: {ex.Message}");
            }
        }

        private void btnRecorrer_Click(object sender, EventArgs e)
        {
            Raiz = Arbol.RegresaRaiz();
            Arbol.strRecorrido = "";

            if (Raiz == null)
            {
                lblRecorridoPreOrden.Text = "El arbol esta vacio";
                return;
            }
            lblRecorridoPreOrden.Text = "";
            Arbol.PreOrden(Raiz);
            lblRecorridoPreOrden.Text = Arbol.strRecorrido;

            Raiz = Arbol.RegresaRaiz();
            Arbol.strRecorrido = "";

            if (Raiz == null)
            {
                lblRecorridoPostOrden.Text = "El arbol esta vacio";
                return;
            }
            lblRecorridoInOrden.Text = "";
            Arbol.InOrden(Raiz);
            lblRecorridoInOrden.Text = Arbol.strRecorrido;

            Raiz = Arbol.RegresaRaiz();
            Arbol.strRecorrido = "";

            if (Raiz == null)
            {
                lblRecorridoPostOrden.Text = "El arbol esta vacio";
                return;
            }
            lblRecorridoPostOrden.Text = "";
            Arbol.PostOrden(Raiz);
            lblRecorridoPostOrden.Text = Arbol.strRecorrido;

            Raiz = Arbol.RegresaRaiz();
            Arbol.strRecorrido = "";

            if (Raiz == null)
            {
                lblNiveles.Text = "El arbol esta vacio";
                return;
            }
            lblNiveles.Text = "";
            Arbol.RecorridoPorNiveles(Raiz);
            lblNiveles.Text = Arbol.strRecorrido;
        }

        private void btnGrafica_Click(object sender, EventArgs e)
        {
            String graphVizString;
            String strOrientacion = "";

            Raiz = Arbol.RegresaRaiz();
            if (Raiz == null)
            {
                MessageBox.Show("El arbol esta vacio");
                return;
            }
            if (rdbtnHorizontal.Checked)
            {
                strOrientacion = "rankdir=\"LR\";";
            }

            StringBuilder b = new StringBuilder();
            b.Append("digraph G { " + strOrientacion + "node [shape=\"circle\"]; " + Environment.NewLine);
            b.Append(Arbol.ToDot(Raiz));
            b.Append("}");
            graphVizString = b.ToString();

            Bitmap bm = FileDotEngine.Run(graphVizString);

            frmGrafica graf = new frmGrafica();
            graf.ActualizaGrafica(bm);
            graf.MdiParent = this.MdiParent;
            graf.Show();
        }

        private void btnLimpiar_Click(object sender, EventArgs e)
        {
            Arbol.PodarArbol();
            Raiz = null;
            txtArbol.Text = "";
            txtDato.Text = "";
            lblRecorridoPreOrden.Text = "";
            lblRecorridoInOrden.Text = "";
            lblRecorridoPostOrden.Text = "";
        }

        private void btnInfo_Click(object sender, EventArgs e)
        {
            if (Arbol.EstaVacio())
            {
                MessageBox.Show("No existe arbol");
                return;
            }

            string info = $"Altura del arbol: {Arbol.ObtenerAltura()}\n" +
                          $"Cantidad de hojas: {Arbol.ContarHojas()}\n" +
                          $"Total de nodos: {Arbol.ContarNodos()}\n " +
                          $"El arbol esta completo: {Arbol.EsArbolCompleto()}\n" +
                          $"Es arbol esta lleno: {Arbol.EsArbolLleno()}";

            MessageBox.Show(info, "Informacion del Arbol", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void frmArbo_Load(object sender, EventArgs e)
        {

        }
    }
    
}
