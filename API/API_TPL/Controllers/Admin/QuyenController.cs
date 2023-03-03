using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Threading.Tasks;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using API_TPL.DAL;

namespace HUE_CDC.Controllers.Admin
{
    /// <summary>
    /// api về quyền
    /// </summary>
    [Authorize]
    [RoutePrefix("api/quyen")]
    public class QuyenController : ApiController
    {
        static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["QLCV"].ToString();
        DBHelper helper = new DBHelper(connString);

        /// <summary>
        /// Thêm mới quyền
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("create"), HttpPost]
        public IHttpActionResult HETHONG_QUYEN_INSERT([FromBody] dynamic obj)
        {
            string query_str = "HETHONG_QUYEN_INSERT";

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

        /// <summary>
        /// Cập nhật quyền
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("update"), HttpPost]
        public IHttpActionResult HETHONG_QUYEN_UPDATE([FromBody] dynamic obj)
        {
            string query_str = "HETHONG_QUYEN_UPDATE";

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


        /// <summary>
        /// Lấy thông tin quyền. <br />
        /// </summary>
        /// <returns></returns>
        [Route("getall"), HttpGet]        
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult GetAllRoles()
        {
            string query_str = "SELECT * FROM HETHONG_QUYEN";
            
            try
            {
                
                DataTable kq = helper.ExecuteQueryString(query_str);

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
