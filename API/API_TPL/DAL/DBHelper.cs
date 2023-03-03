using Oracle.ManagedDataAccess.Client;
using System;
using System.Data;

namespace API_TPL.DAL
{
    /// <summary>
    /// 
    /// </summary>
    public class DBHelper
    {
        static String connString_default = System.Configuration.ConfigurationManager.ConnectionStrings["QLCV"].ToString();

        OracleConnection conn;

        /// <summary>
        /// 
        /// </summary>
        public DBHelper()
        {
            conn = new OracleConnection(connString_default);
        }

        public DBHelper(string connString)
        {
            conn = new OracleConnection(connString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query_string"></param>
        /// <param name="inParams"></param>
        /// <returns></returns>
        public string ExecuteNonQuery(string query_string, object[] inParams)
        {

            try
            {
                DataTable data = new DataTable();
                OracleCommand cmd;
                string result = "OK";
                conn.Open();

                cmd = conn.CreateCommand();
                cmd.CommandText = query_string;
                cmd.CommandType = CommandType.StoredProcedure;

                if (inParams != null)
                {
                    cmd.Parameters.AddRange(inParams);
                }

                cmd.ExecuteNonQuery();

                conn.Close();

                return result;
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query_string"></param>
        /// <param name="inParams"></param>
        /// <returns></returns>
        public DataTable ExecuteQueryStoreProcedure(string query_string, object[] inParams)
        {

            try
            {
                DataTable data = new DataTable();
                OracleCommand cmd;

                conn.Open();

                cmd = conn.CreateCommand();
                cmd.CommandText = query_string;
                cmd.CommandType = CommandType.StoredProcedure;
                if (inParams != null)
                    cmd.Parameters.AddRange(inParams);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(data);

                conn.Close();
                return data;
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query_string"></param>
        /// <returns></returns>
        public DataTable ExecuteQueryString(string query_string)
        {

            try
            {
                DataTable data = new DataTable();

                OracleCommand cmd;

                conn.Open();

                cmd = conn.CreateCommand();
                cmd.CommandText = query_string;
                cmd.CommandType = CommandType.Text;

                OracleDataAdapter da = new OracleDataAdapter(cmd);

                da.Fill(data);

                conn.Close();
                return data;
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query_string"></param>
        /// <param name="inParams"></param>
        /// <returns></returns>
        public DataTable ExecuteQueryFunction(string query_string, object[] inParams)
        {

            try
            {
                DataTable data = new DataTable();
                OracleCommand cmd;
                conn.Open();

                cmd = conn.CreateCommand();
                cmd.CommandText = query_string;
                cmd.CommandType = CommandType.StoredProcedure;
                if (inParams != null)
                    cmd.Parameters.AddRange(inParams);


                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                conn.Close();

                if (ds.Tables.Count > 0)
                    return ds.Tables[0];
                else return null;
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;

            }
        }
        public DataSet ExecuteQueryStoreProcedure_List(string query_string, object[] inParams)
        {

            try
            {
                DataTable data = new DataTable();
                OracleCommand cmd;
                conn.Open();

                cmd = conn.CreateCommand();
                cmd.CommandText = query_string;
                cmd.CommandType = CommandType.StoredProcedure;
                if (inParams != null)
                    cmd.Parameters.AddRange(inParams);


                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                conn.Close();

                if (ds.Tables.Count > 0)
                    return ds;
                else return null;
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbtype"></param>
        /// <param name="direct"></param>
        /// <returns></returns>
        public OracleParameter BuildParameter(string name, object value, OracleDbType dbtype, ParameterDirection direct)
        {
            OracleParameter param = new OracleParameter(name, dbtype, direct);
            param.Value = value;
            return param;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbtype"></param>
        /// <param name="size"></param>
        /// <param name="scale"></param>
        /// <param name="direct"></param>
        /// <returns></returns>
        public OracleParameter BuildParameter(string name, object value, OracleDbType dbtype, int size, byte scale, ParameterDirection direct)
        {
            OracleParameter param = new OracleParameter();
            param.ParameterName = name;
            param.OracleDbType = dbtype;
            param.Size = size;
            param.Scale = scale;
            param.Value = value;
            return param;
        }
    }

    /// <summary>
    /// connection data DMQUANGCAO
    /// </summary>
    public class DBHelper_DMQC
    {
        static String connString = System.Configuration.ConfigurationManager.ConnectionStrings["HUE_QC_DM"].ToString();

        OracleConnection conn;

        /// <summary>
        /// 
        /// </summary>
        public DBHelper_DMQC()
        {
            conn = new OracleConnection(connString);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query_string"></param>
        /// <param name="inParams"></param>
        /// <returns></returns>
        public string ExecuteNonQuery(string query_string, object[] inParams)
        {

            try
            {
                DataTable data = new DataTable();
                OracleCommand cmd;
                string result = "OK";
                conn.Open();

                cmd = conn.CreateCommand();
                cmd.CommandText = query_string;
                cmd.CommandType = CommandType.StoredProcedure;

                if (inParams != null)
                {
                    cmd.Parameters.AddRange(inParams);
                }

                cmd.ExecuteNonQuery();

                conn.Close();

                return result;
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query_string"></param>
        /// <param name="inParams"></param>
        /// <returns></returns>
        public DataTable ExecuteQueryStoreProcedure(string query_string, object[] inParams)
        {

            try
            {
                DataTable data = new DataTable();
                OracleCommand cmd;

                conn.Open();

                cmd = conn.CreateCommand();
                cmd.CommandText = query_string;
                cmd.CommandType = CommandType.StoredProcedure;
                if (inParams != null)
                    cmd.Parameters.AddRange(inParams);

                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(data);

                conn.Close();
                return data;
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query_string"></param>
        /// <returns></returns>
        public DataTable ExecuteQueryString(string query_string)
        {

            try
            {
                DataTable data = new DataTable();

                OracleCommand cmd;

                conn.Open();

                cmd = conn.CreateCommand();
                cmd.CommandText = query_string;
                cmd.CommandType = CommandType.Text;

                OracleDataAdapter da = new OracleDataAdapter(cmd);

                da.Fill(data);

                conn.Close();
                return data;
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="query_string"></param>
        /// <param name="inParams"></param>
        /// <returns></returns>
        public DataTable ExecuteQueryFunction(string query_string, object[] inParams)
        {

            try
            {
                DataTable data = new DataTable();
                OracleCommand cmd;
                conn.Open();

                cmd = conn.CreateCommand();
                cmd.CommandText = query_string;
                cmd.CommandType = CommandType.StoredProcedure;
                if (inParams != null)
                    cmd.Parameters.AddRange(inParams);


                OracleDataAdapter da = new OracleDataAdapter(cmd);
                DataSet ds = new DataSet();
                da.Fill(ds);

                conn.Close();

                if (ds.Tables.Count > 0)
                    return ds.Tables[0];
                else return null;
            }
            catch (Exception ex)
            {
                conn.Close();
                throw ex;

            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbtype"></param>
        /// <param name="direct"></param>
        /// <returns></returns>
        public OracleParameter BuildParameter(string name, object value, OracleDbType dbtype, ParameterDirection direct)
        {
            OracleParameter param = new OracleParameter(name, dbtype, direct);
            param.Value = value;
            return param;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <param name="dbtype"></param>
        /// <param name="size"></param>
        /// <param name="scale"></param>
        /// <param name="direct"></param>
        /// <returns></returns>
        public OracleParameter BuildParameter(string name, object value, OracleDbType dbtype, int size, byte scale, ParameterDirection direct)
        {
            OracleParameter param = new OracleParameter();
            param.ParameterName = name;
            param.OracleDbType = dbtype;
            param.Size = size;
            param.Scale = scale;
            param.Value = value;
            return param;
        }
    }
}