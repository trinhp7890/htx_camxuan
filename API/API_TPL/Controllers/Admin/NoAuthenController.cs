using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using API_TPL.DAL;

namespace API_TPL.Controllers.Admin
{
    [RoutePrefix("api/nguoidungkut")]
    public class NoAuthenController : ApiController
    {
        static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["QLCV"].ToString();
        DBHelper helper = new DBHelper(connString);
        /// <summary>
        /// Đổi mật khẩu người dùng
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("doimatkhau"), HttpPost]
        public IHttpActionResult HETHONG_NGUOIDUNG_CHANGE_PW([FromBody] dynamic obj)
        {
            string query_str = "HETHONG_NGUOIDUNG_CHANGE_PW";

            object[] aParams = new object[1];

            try
            {
                aParams[0] = helper.BuildParameter("prmJsonData", obj, OracleDbType.Clob, ParameterDirection.Input);

                String kq = helper.ExecuteNonQuery(query_str, aParams);

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, kq));
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, err));
            }
        }
    }
}
