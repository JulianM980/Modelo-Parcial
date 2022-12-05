
using ParcialApp.Acceso_a_datos;
using ParcialApp.Dominio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ParcialApp.Presentacion
{
    public partial class Frm_Alta : Form
    {
        private Factura factura;
        
        public Frm_Alta()
        {
            InitializeComponent();
            factura = new Factura();

        }




        private void btnAceptar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtCliente.Text))
            {
                MessageBox.Show("Debe ingresar un cliente valido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (cboForma.SelectedIndex == -1)
            {
                MessageBox.Show("Debe seleccionar una forma de pago valida", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            else if (dgvDetalles.Rows.Count < 1)
            {
                MessageBox.Show("Debe ingresar al menos un producto", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            double total = 0; 
            foreach (DataGridViewRow valor in dgvDetalles.Rows)
            {
                if (cboForma.SelectedIndex == 1)
                {
                    total += Convert.ToDouble(valor.Cells["colSubTotal"].Value);
                }
                else
                {
                    total += Convert.ToDouble(valor.Cells["colSubTotal"].Value)*1.10;
                }
            }
            factura.Total=total;
            factura.FormaPago = cboForma.SelectedIndex;
            factura.Cliente = txtCliente.Text;
            if (HelperDB.ObtenerInstancia().AgregarDetalle(factura))
            {
                MessageBox.Show("Factura ingresada correctamente", "Informacion", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LimpiarCampos();
            }
          
        }

        private void LimpiarCampos()
        {
            txtCliente.Text = string.Empty;
            cboForma.SelectedIndex = -1;
            cboProducto.SelectedIndex = -1;
            nudCantidad.Value = 1;
            dgvDetalles.Rows.Clear();
            lblTotal.Text = "Total $ ";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            DialogResult result = MessageBox.Show("¿Está seguro que desea cancelar?", "Salir", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();

            }
            else
            {
                return;
            }
        }

        private void Frm_Alta_Presupuesto_Load(object sender, EventArgs e)
        {
            CargarCombo();
        }

        private void CargarCombo()
        {
            DataTable table= HelperDB.ObtenerInstancia().ConsultaSQL("SP_CONSULTAR_PRODUCTOS");
            cboProducto.DataSource = table;
            cboProducto.DisplayMember = "n_producto";
            cboProducto.ValueMember = "id_producto";
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            
            if (cboProducto.SelectedIndex != -1)
            {
                if (!exite(cboProducto.Text))
                {
                    DataRowView item =(DataRowView)cboProducto.SelectedItem;
                    int Prod = Convert.ToInt32(item.Row.ItemArray[0]);
                    string Nom = item.Row.ItemArray[1].ToString();
                    double Precio= Convert.ToDouble(item.Row.ItemArray[2]);
                    Producto p = new Producto(Prod, Nom, Precio);
                    int Cantidad = (int)nudCantidad.Value;
                    DetalleFactura det = new DetalleFactura(p, Cantidad);
                    double SubTotal = Precio * Cantidad;
                    double total = 0;
                    foreach(DataGridViewRow valor in dgvDetalles.Rows)
                    {
                        total += Convert.ToDouble(valor.Cells["colSubTotal"].Value);
                    }
                    factura.AgregarDetalle(det);
                    dgvDetalles.Rows.Add(new object[] { det.Producto.IdProducto, det.Producto.Nombre, det.Producto.Precio, det.Cantidad,SubTotal });
                    lblTotal.Text = "Total $ " + Convert.ToString(total+SubTotal);
                    
                }
            }
        }

        private bool exite(string selectedItem)
        {
            bool aux = false;
            foreach(DataGridViewRow item in dgvDetalles.Rows)
            {
                if (item.Cells["producto"].Value.ToString().Equals(selectedItem))
                {
                    aux = true;
                    break;
                }
            }
            return aux;
        }

        private bool ExisteProductoEnGrilla(string text)
        {
            foreach (DataGridViewRow fila in dgvDetalles.Rows)
            {
                if (fila.Cells["producto"].Value.Equals(text))
                    return true;
            }
            return false;
        }

       



        private void dgvDetalles_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (dgvDetalles.CurrentCell.ColumnIndex == 5)
            {
                factura.QuitarDetalle(dgvDetalles.CurrentRow.Index);
                dgvDetalles.Rows.Remove(dgvDetalles.CurrentRow);
                double total = 0;
                foreach (DataGridViewRow valor in dgvDetalles.Rows)
                {
                    total += Convert.ToDouble(valor.Cells["colSubTotal"].Value);
                }
                lblTotal.Text = "Total $ " + Convert.ToString(total);
            }
        }
    }
}
