using RecetasSLN.dominio;
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

        public int EjecurtarSQL(string SPNombre,List<Parametro> values)
        {
            int afectadas = 0;
            SqlTransaction t = null;

            try
            {
                SqlCommand cmd = new SqlCommand();
                cnn.Open();
                t = cnn.BeginTransaction();
                cmd.Connection = cnn;
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = SPNombre;
                cmd.Transaction = t;

                if(values!=null)
                {
                    foreach(Parametro param in values)
                    {
                        cmd.Parameters.AddWithValue(param.nombre, param.valor);
                    }
                }

                afectadas = cmd.ExecuteNonQuery();
                t.Commit();
            }
            catch (SqlException)
            {
                if (t != null) { t.Rollback(); }
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open) cnn.Close();
            }

            return afectadas;
        }

        public bool AceptarPresupuesto(Receta oreceta)
        {
            bool ok = true;
            SqlTransaction t = null;
            SqlCommand cmd = new SqlCommand();
            try
            {
                cnn.Open();
                t = cnn.BeginTransaction();
                cmd.Connection = cnn;
                cmd.Transaction = t;
                cmd.CommandText = "SP_INSERTAR_RECETA";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@nombre", oreceta.nombre);
                cmd.Parameters.AddWithValue("@cheff", oreceta.cheff);
                cmd.Parameters.AddWithValue("@tipo_receta", oreceta.tipoReceta);

                //Parametro de salida
                SqlParameter pOut = new SqlParameter();
                pOut.ParameterName = "@id"; //Nombre del parametro
                pOut.DbType = DbType.Int32; //Tipo de dato que devuelve el parametro
                pOut.Direction = ParameterDirection.Output; //Direccion del parametro
                cmd.Parameters.Add(pOut); //Agregamos el parametro
                cmd.ExecuteNonQuery(); //Cantidad de filas

                int recetaNro = (int)pOut.Value; //Cargamos que numero de receta es

                SqlCommand cmdDetalle =null;
                
                foreach(DetalleReceta item in oreceta.detalleReceta)
                {
                    cmdDetalle = new SqlCommand("SP_INSERTAR_DETALLES", cnn, t);
                    cmdDetalle.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@id_receta", recetaNro);
                    cmd.Parameters.AddWithValue("@id_ingrediente",item.ingrediente.ingredienteId);
                    cmd.Parameters.AddWithValue("@cantidad", item.cantidad);
                    cmd.ExecuteNonQuery();

                }

                t.Commit();
                ok = true;
            }
            catch (Exception ex)
            {
                if (t != null)
                    t.Rollback();
            }
            finally
            {
                if (cnn != null && cnn.State == ConnectionState.Open) cnn.Close();
            }

            return ok;
        }

        internal int ProximaReceta(string SPNombre,string PNombre)
        {
            int aux;
            try
            {
                SqlCommand CMD = new SqlCommand();
                cnn.Open();
                CMD.Connection = cnn;
                CMD.CommandText = SPNombre;
                CMD.CommandType = CommandType.StoredProcedure;
                SqlParameter pOut = new SqlParameter();
                pOut.ParameterName = PNombre;
                pOut.DbType = DbType.Int32;
                pOut.Direction = ParameterDirection.Output;
                CMD.Parameters.Add(pOut);
                CMD.ExecuteNonQuery();

                cnn.Close();
                aux= (int)pOut.Value;
            }
            catch (Exception ex)
            {
                aux = 1;
            }
            return aux;
        }
    }
}
