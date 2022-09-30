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
                cboIngrediente.DisplayMember = "n_ingrediente";
                cboIngrediente.ValueMember = "id_ingrediente"; 
            }
        }

        private void FrmAltaReceta_Load(object sender, EventArgs e)
        {
            lblNro.Text = "Receta #:" + helper.ProximaReceta("SP_PROXIMO_ID", "@next").ToString();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (dgvDetalle.Rows.Count < 3)
            {
                MessageBox.Show("Debe ingresar al menos 3 ingredientes", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            receta.cheff = txtCheff.Text;
            receta.nombre = txtNombre.Text;

            receta.tipoReceta = cboTipo.SelectedIndex + 1;

            if(helper.AceptarPresupuesto(receta))
            {
                MessageBox.Show("Receta guardada con exito", "Registro", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
            }
        }

        private void LimpiarCampos()
        {
            txtNombre.Text = string.Empty;
            txtNombre.Text = string.Empty;
            cboTipo.SelectedIndex = -1;
            cboIngrediente.SelectedIndex = -1;
            nudCantidad.Value = 1;
            dgvDetalle.Rows.Clear();
            lblNro.Text = "Receta #:" + helper.ProximaReceta("SP_PROXIMO_ID", "@next").ToString();
            lblTotalIngredientes.Text = "Total de ingredientes;";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Debe ingresar el nombre de la receta", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtNombre.Focus();
                return;
            }
            else if (string.IsNullOrEmpty(txtCheff.Text))
            {
                MessageBox.Show("Debe ingresar el nombre del cheff", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                txtCheff.Focus();
                return;
            }
            else if (cboTipo.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar un tipo de receta valido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            
            if (cboIngrediente.SelectedIndex!=-1)
            {
                if(!existe(cboIngrediente.Text))
                {
                    DataRowView item = (DataRowView)cboIngrediente.SelectedItem;
                    int prod = Convert.ToInt32(item.Row.ItemArray[0]);
                    string nom = item.Row.ItemArray[1].ToString();
                    Ingrediente I = new Ingrediente(prod, nom);
                    int cantidad = Convert.ToInt32(nudCantidad.Text);
                    DetalleReceta det = new DetalleReceta(I,cantidad);
                    receta.AgregarDetalle(det);
                    dgvDetalle.Rows.Add(new object[] { item.Row.ItemArray[0], item.Row.ItemArray[1], det.cantidad,cantidad });
                    lblTotalIngredientes.Text = "Total de ingredientes: " + dgvDetalle.Rows.Count.ToString();
                }
            }
        }

        private bool existe(String selectedItem)
        {
            bool aux = false;
            foreach (DataGridViewRow item in dgvDetalle.Rows)
            {
                if(item.Cells["Ingrediente"].Value.ToString().Equals(selectedItem))
                {
                    aux = true;
                    break;
                }
            }
            return aux;
        }

    }
}
