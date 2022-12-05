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

//CURSO 1w3 – LEGAJO 112866 – APELLIDO Martinez– NOMBRE Julian

//Cadena de Conexión: "Data Source=sqlgabineteinformatico.frc.utn.edu.ar;Initial Catalog=Qatar2022;User ID=alumnolab22;Password=SQL-Alu22"

namespace TUP_PI_EF_Qatar2022
{
    public partial class FrmJugadores : Form
    {
        private SqlConnection cnn;
        private SqlCommand cmd;
        public FrmJugadores()
        {
            InitializeComponent();
            cnn = new SqlConnection(@"Data Source=sqlgabineteinformatico.frc.utn.edu.ar;Initial Catalog=Qatar2022;User ID=alumnolab22;Password=SQL-Alu22");
        }

        private void FrmJugadores_Load(object sender, EventArgs e)
        {
            CargarLista();
            CargarCombo();
        }

        private void CargarLista()
        {
            DataTable table = new DataTable();
            cnn.Open();
            cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandText = "Select CONCAT(nombre,'|',camiseta,'|',posicion) as Datos, id_jugador,nombre,id_equipo,camiseta,posicion,fecha_nacimiento From Jugadores";
            table.Load(cmd.ExecuteReader());
            cnn.Close();
            lstJugadores.DataSource = table;
            lstJugadores.DisplayMember = "Datos";
            lstJugadores.ValueMember = "id_jugador";
        }

        private void CargarCombo()
        {
            DataTable table = new DataTable();
            cnn.Open();
            cmd = new SqlCommand();
            cmd.Connection = cnn;
            cmd.CommandText = "Select * from Equipos";
            table.Load(cmd.ExecuteReader());
            cnn.Close();

            cboEquipo.DataSource = table;
            cboEquipo.DisplayMember = "pais";
            cboEquipo.ValueMember = "id_equipo";
        }

        private void lstJugadores_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataRowView item = (DataRowView)lstJugadores.SelectedItem;
            txtIdJugador.Text = lstJugadores.SelectedIndex.ToString();
            txtNombre.Text = item.Row.ItemArray[2].ToString();
            cboEquipo.SelectedItem = Convert.ToInt32(item.Row.ItemArray[3]);
            nudCamiseta.Value = Convert.ToInt32(item.Row.ItemArray[4]);
            int Equipo= Convert.ToInt32(item.Row.ItemArray[5]);
            if (Equipo == 1)
            {
                rbtArquero.Checked = true;
            }
            else if (Equipo == 2)
            {
                rbtDefensor.Checked = true;
            }
            else if (Equipo == 3)
            {
                rbtMedioCampista.Checked = true;
            }
            else if (Equipo == 4)
            {
                rbtDelantero.Checked = true;
            }
            else if (Equipo >1||Equipo>4)
            {
                rbtArquero.Checked = true;
            }
            dtpFechaNacimiento.Value = Convert.ToDateTime(item.Row.ItemArray[6]);

        }

        private void btnGrabar_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtNombre.Text))
            {
                MessageBox.Show("Debe ingresar un nombre valido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (cboEquipo.SelectedIndex == -1)
            {
                MessageBox.Show("Debe ingresar un Equipo valido", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            else if (nudCamiseta.Value < 1 || nudCamiseta.Value > 26)
            {
                MessageBox.Show("El numero de camiseta debe estar entre 1 y 16", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                int posicion=0;
                if (rbtArquero.Checked)
                {
                    posicion = 1;

                }
                else if (rbtDefensor.Checked)
                {
                    posicion = 2;
                }
                else if (rbtMedioCampista.Checked)
                {
                    posicion = 3;
                }
                else if (rbtDelantero.Checked)
                {
                    posicion = 4;
                }
                cnn.Open();
                cmd = new SqlCommand();
                cmd.CommandText = "Update Jugadores Set id_equipo=@equipo, nombre=@nombre , camiseta=@camiseta, posicion=@posicion,fecha_nacimiento=@fecha Where id_jugador=@id";
                cmd.Connection = cnn;
                cmd.Parameters.AddWithValue("@equipo", Convert.ToInt32(cboEquipo.SelectedIndex));
                cmd.Parameters.AddWithValue("@nombre", txtNombre.Text);
                cmd.Parameters.AddWithValue("@camiseta", nudCamiseta.Value);
                cmd.Parameters.AddWithValue("@posicion", posicion);
                cmd.Parameters.AddWithValue("@fecha", dtpFechaNacimiento.Value);
                cmd.Parameters.AddWithValue("@id", Convert.ToInt32(txtIdJugador.Text));
                cmd.ExecuteNonQuery();
                cnn.Close();
                MessageBox.Show("Jugador actualizado", "Informacion", MessageBoxButtons.OK,MessageBoxIcon.Information);
                grpDatos.Enabled = false;
                btnGrabar.Enabled = true;
                txtNombre.Enabled = false;
                cboEquipo.Enabled = false;
                nudCamiseta.Enabled = false;
                rbtArquero.Checked = false;
                rbtArquero.Enabled = false;
                rbtDefensor.Enabled = false;
                rbtMedioCampista.Enabled = false;
                rbtDelantero.Enabled = false;
                dtpFechaNacimiento.Enabled = false;
                btnGrabar.Enabled = false;
                lstJugadores.SelectedIndex = 1;
                return;
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo actulizar" + ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            finally
            {
                cnn.Close();
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            grpDatos.Enabled = true;
            btnActualizar.Enabled = false;
            txtNombre.Enabled = true;
            cboEquipo.Enabled = true;
            nudCamiseta.Enabled = true;
            rbtArquero.Checked = true;
            rbtArquero.Enabled = true;
            rbtDefensor.Enabled = true;
            rbtMedioCampista.Enabled = true;
            rbtDelantero.Enabled = true;
            dtpFechaNacimiento.Enabled = true;
            btnGrabar.Enabled = true;
        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            DialogResult result= MessageBox.Show("Desea Salir?", "Informacion", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result == DialogResult.Yes)
            {
                this.Dispose();
            }
            else
            {
                return;
            }
        }
    }
    
}
