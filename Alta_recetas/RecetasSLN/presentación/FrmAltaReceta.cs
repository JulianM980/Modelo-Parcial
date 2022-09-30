using RecetasSLN.datos;
using RecetasSLN.dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RecetasSLN.presentación
{
    public partial class FrmAltaReceta : Form
    {
        private AccesoDB helper;
        private Receta receta;
        public FrmAltaReceta()
        {
            InitializeComponent();
            helper = new AccesoDB();
            CargarIngredientes();
            receta = new Receta();
        }

        private void CargarIngredientes()
        {
            DataTable tabla = helper.ConsultaSQL("SP_CONSULTAR_INGREDIENTES");
            if(tabla!=null)
            {
                cboIngrediente.DataSource = tabla;
                cboIngrediente.DisplayMember = "n_ingrdiente";
                cboIngrediente.ValueMember = "id_ingrediente";
            }
        }

        private void FrmAltaReceta_Load(object sender, EventArgs e)
        {

        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {

        }
    }
}
