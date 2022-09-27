using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecetasSLN.datos
{
    internal class AccesoDB
    {
        private SqlConnection cnn;

        public AccesoDB()
        {
            cnn = new SqlConnection(Properties.Resources.cnnString);
        }

        public DataTable ConsultaSQL(string SPNombre)
        {
            SqlCommand cmd = new SqlCommand();
            DataTable tabla = new DataTable();

            cnn.Open();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandText = SPNombre;
            tabla.Load(cmd.ExecuteReader());
            cnn.Close();
            return tabla;
        }

        public DataTable ConsultaSQL(string SPNombre, List<Parametro> value)
        {
            SqlCommand cmd = new SqlCommand();
            DataTable table = new DataTable();
            cnn.Open();
            cmd.Connection = cnn;
            cmd.CommandType = CommandType.StoredProcedure;
            foreach  (Parametro parametro in value)
            {
                cmd.Parameters.AddWithValue(parametro.nombre, parametro.valor);
            }
            table.Load(cmd.ExecuteReader());
            cnn.Close();
            return table;
        }
    }
}
