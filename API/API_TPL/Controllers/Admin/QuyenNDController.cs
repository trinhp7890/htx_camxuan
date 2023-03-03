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
    /// api quyền của người dùng
    /// </summary>
    [Authorize]
    [RoutePrefix("api/quyennd")]
    public class QuyenNDController : ApiController
    {
        static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["QLCV"].ToString();
        DBHelper helper = new DBHelper(connString);

        /// <summary>
        /// Thêm mới quyền người dùng
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("create"), HttpPost]
        public IHttpActionResult HETHONG_QUYEN_ND_INSERT([FromBody] dynamic obj)
        {
            string query_str = "HETHONG_QUYEN_ND_INSERT";

            object[] aParams = new object[3];

            try
            {
                aParams[0] = helper.BuildParameter("prmID_ND", obj.id_nd, OracleDbType.Int32, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmID_QUYEN", obj.id_quyen, OracleDbType.Int32, ParameterDirection.Input);
                aParams[2] = helper.BuildParameter("prmNGUOI_CN", obj.nguoi_cn, OracleDbType.Varchar2, ParameterDirection.Input);

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
        /// Bỏ quyền người dùng
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("delete"), HttpPost]
        public IHttpActionResult HETHONG_QUYEN_ND_DELETE([FromBody] dynamic obj)
        {
            string query_str = "HETHONG_QUYEN_ND_DELETE";

            object[] aParams = new object[3];

            try
            {
                aParams[0] = helper.BuildParameter("prmID_ND", obj.id_nd, OracleDbType.Int32, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmID_QUYEN", obj.id_quyen, OracleDbType.Int32, ParameterDirection.Input);
                aParams[2] = helper.BuildParameter("prmNGUOI_CN", obj.nguoi_cn, OracleDbType.Varchar2, ParameterDirection.Input);

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
        /// Lấy thông tin quyền người dùng theo uid. <br />
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Route("getrolesfromuser"), HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult GetUserRolesByUid(string uid)
        {
            string query_str = "SELECT * FROM HETHONG_QUYEN_ND WHERE ID_ND=" + uid;

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

        /// <summary>
        /// Cập nhật quyền người dùng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("update_role_user"), HttpPost]
        public IHttpActionResult HETHONG_QUYEN_ND_ACTION_UPDATE([FromBody] dynamic obj)
        {
            string query_str = "HETHONG_QUYEN_ND_ACTION_UPDATE";

            object[] aParams = new object[4];

            try
            {
                aParams[0] = helper.BuildParameter("prmID_ND", obj.id_nd, OracleDbType.Int32, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmID_QUYEN", obj.id_quyen, OracleDbType.Int32, ParameterDirection.Input);
                aParams[2] = helper.BuildParameter("prmACTION", obj.action, OracleDbType.Int32, ParameterDirection.Input);
                aParams[3] = helper.BuildParameter("prmNGUOI_CN", obj.nguoi_cn, OracleDbType.Varchar2, ParameterDirection.Input);

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
