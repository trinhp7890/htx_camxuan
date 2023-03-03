using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace API_TPL.DAL
{
    /// <summary>
    /// connect sql
    /// </summary>
    public class SQL_DBHELPERs
    {
        SqlConnection conn;

        /// <summary>
        /// 
        /// </summary>
        public SQL_DBHELPERs(string connString)
        {
            conn = new SqlConnection(connString);
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
                SqlCommand cmd;

                conn.Open();

                cmd = conn.CreateCommand();
                cmd.CommandText = query_string;
                cmd.CommandType = CommandType.StoredProcedure;
                if (inParams != null)
                    cmd.Parameters.AddRange(inParams);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
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
        public SqlParameter BuildParameter(string name, object value, SqlDbType dbtype)
        {
            SqlParameter param = new SqlParameter(name, dbtype);
            param.Value = value;
            return param;
        }
    }
}