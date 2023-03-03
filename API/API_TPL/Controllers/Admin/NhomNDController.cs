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
    /// api nhóm người dùng
    /// </summary>
    [Authorize]
    [RoutePrefix("api/nhomnd")]
    public class NhomNDController : ApiController
    {
        static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["QLCV"].ToString();
        DBHelper helper = new DBHelper(connString);

        /// <summary>
        /// Thêm mới nhóm người dùng
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("create"), HttpPost]
        public IHttpActionResult HETHONG_NHOM_ND_INSERT([FromBody] dynamic obj)
        {
            string query_str = "HETHONG_NHOM_ND_INSERT";

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
        /// Cập nhật nhóm người dùng
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("update"), HttpPost]
        public IHttpActionResult HETHONG_NHOM_ND_UPDATE([FromBody] dynamic obj)
        {
            string query_str = "HETHONG_NHOM_ND_UPDATE";

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
        /// Lấy thông nhóm ND. <br />
        /// </summary>
        /// <returns></returns>
        [Route("getall"), HttpGet]        
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult GetAllGroup()
        {
            string query_str = "HETHONG_NHOM_ND_GETALL";
            object[] aParams = new object[1];
            try
            {
                OracleParameter resultParam = new OracleParameter("results", OracleDbType.RefCursor);
                resultParam.Direction = ParameterDirection.Output;

                aParams[0] = resultParam;
                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);                

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, kq));
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, err));
            }
        }


        /// <summary>
        /// Lấy thông tin nhóm theo uid. <br />
        /// </summary>
        /// <param name="gid"></param>
        /// <returns></returns>
        [Route("get"), HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult GetGroupByUid(int gid)
        {
            string query_str = "SELECT * FROM HETHONG_NHOM_ND WHERE ID=" + gid;

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
        /// Lấy thông tin người dùng theo gid. <br />
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        [Route("inusers"), HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult GetUsersByGroupId(string groupid)
        {
            string query_str = "HETHONG_NGUOIDUNG_GETBY_NHOM";           
            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmID_NHOM", groupid, OracleDbType.Int16, ParameterDirection.Input);

                OracleParameter resultParam = new OracleParameter("results", OracleDbType.RefCursor);
                resultParam.Direction = ParameterDirection.Output;

                aParams[1] = resultParam;

                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, kq));
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, err));
            }
        }
        /// <summary>
        /// Lấy thông tin người dùng theo nhom, donvi. <br />
        /// </summary>
        /// <param name="groupid"></param>
        /// <param name="donvi"></param>
        /// <returns></returns>
        [Route("inusers_v2"), HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult GetUsersByGroupId_v2(string groupid, string donvi)
        {
            string query_str = "HETHONG_NGUOIDUNG_GETBY_NHOM_V2";
            object[] aParams = new object[3];
            try
            {
                aParams[0] = helper.BuildParameter("prmID_NHOM", groupid, OracleDbType.Int16, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmMA_DV", donvi, OracleDbType.Varchar2, ParameterDirection.Input);

                OracleParameter resultParam = new OracleParameter("results", OracleDbType.RefCursor);
                resultParam.Direction = ParameterDirection.Output;

                aParams[2] = resultParam;

                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, kq));
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, err));
            }
        }
        /// <summary>
        /// Lấy thông tin người dùng không thuộc nhóm. <br />
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        [Route("outusers"), HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult GetUsersNotByGroupId(string groupid)
        {
            string query_str = "HETHONG_NGUOIDUNG_GET_NOTBY_NHOM";
            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmID_NHOM", groupid, OracleDbType.Int16, ParameterDirection.Input);

                OracleParameter resultParam = new OracleParameter("results", OracleDbType.RefCursor);
                resultParam.Direction = ParameterDirection.Output;

                aParams[1] = resultParam;

                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, kq));
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, err));
            }
        }
        /// <summary>
        /// Lấy thông tin người dùng không thuộc nhóm, đơn vị. <br />
        /// </summary>
        /// <param name="uid"></param>
        /// <returns></returns>
        [Route("outusers_v2"), HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult GetUsersNotByGroupId_v2(string groupid, string donvi)
        {
            string query_str = "HETHONG_NGUOIDUNG_GET_NOTBY_NHOM_V2";
            object[] aParams = new object[3];
            try
            {
                aParams[0] = helper.BuildParameter("prmID_NHOM", groupid, OracleDbType.Int16, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmMA_DV", donvi, OracleDbType.Varchar2, ParameterDirection.Input);
                OracleParameter resultParam = new OracleParameter("results", OracleDbType.RefCursor);
                resultParam.Direction = ParameterDirection.Output;

                aParams[2] = resultParam;

                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, kq));
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, err));
            }
        }
        /// <summary>
        /// Thêm người dùng vào nhóm
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("setusertogroup"), HttpPost]
        public IHttpActionResult HETHONG_NGUOIDUNG_NHOM_INSERT([FromBody] dynamic obj)
        {
            string query_str = "HETHONG_NGUOIDUNG_NHOM_INSERT";

            object[] aParams = new object[3];

            try
            {
                aParams[0] = helper.BuildParameter("prmID_ND", obj.id_nd, OracleDbType.Int32, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmID_NHOM", obj.id_nhom, OracleDbType.Int32, ParameterDirection.Input);
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
        /// Gỡ người dùng ra khỏi nhóm
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("removeuserfromgroup"), HttpPost]
        public IHttpActionResult HETHONG_NGUOIDUNG_NHOM_DELETE([FromBody] dynamic obj)
        {
            string query_str = "HETHONG_NGUOIDUNG_NHOM_DELETE";

            object[] aParams = new object[3];

            try
            {
                aParams[0] = helper.BuildParameter("prmID_ND", obj.id_nd, OracleDbType.Int32, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmID_NHOM", obj.id_nhom, OracleDbType.Int32, ParameterDirection.Input);
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
        /// Lấy thông tin quyền theo gid. <br />
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        [Route("inroles"), HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult GetRolesByGroupId(string groupid)
        {
            string query_str = "HETHONG_QUYEN_GETBY_NHOM";
            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmID_NHOM", groupid, OracleDbType.Int16, ParameterDirection.Input);

                OracleParameter resultParam = new OracleParameter("results", OracleDbType.RefCursor);
                resultParam.Direction = ParameterDirection.Output;

                aParams[1] = resultParam;

                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, kq));
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, err));
            }
        }

        /// <summary>
        /// Lấy thông tin quyền không thuộc nhóm. <br />
        /// </summary>
        /// <param name="groupid"></param>
        /// <returns></returns>
        [Route("outroles"), HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult GetRolesNotByGroupId(string groupid)
        {
            string query_str = "HETHONG_QUYEN_GET_NOTBY_NHOM";
            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmID_NHOM", groupid, OracleDbType.Int16, ParameterDirection.Input);

                OracleParameter resultParam = new OracleParameter("results", OracleDbType.RefCursor);
                resultParam.Direction = ParameterDirection.Output;

                aParams[1] = resultParam;

                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, kq));
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, err));
            }
        }

        /// <summary>
        /// Cập nhật quyền nhóm người dùng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("update_role_group"), HttpPost]
        public IHttpActionResult HETHONG_QUYEN_NHOM_ACTION_UPDATE([FromBody] dynamic obj)
        {
            string query_str = "HETHONG_QUYEN_NHOM_ACTION_UPDATE";

            object[] aParams = new object[4];

            try
            {
                aParams[0] = helper.BuildParameter("prmID_NHOM", obj.id_nhom, OracleDbType.Int32, ParameterDirection.Input);
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


        /// <summary>
        /// Thêm quyền vào nhóm
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("setroletogroup"), HttpPost]
        public IHttpActionResult HETHONG_QUYEN_NHOM_INSERT([FromBody] dynamic obj)
        {
            string query_str = "HETHONG_QUYEN_NHOM_INSERT";

            object[] aParams = new object[3];

            try
            {
                aParams[0] = helper.BuildParameter("prmID_QUYEN", obj.id_quyen, OracleDbType.Int32, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmID_NHOM", obj.id_nhom, OracleDbType.Int32, ParameterDirection.Input);
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
        /// Gỡ quyền ra khỏi nhóm
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("removerolefromgroup"), HttpPost]
        public IHttpActionResult HETHONG_QUYEN_NHOM_DELETE([FromBody] dynamic obj)
        {
            string query_str = "HETHONG_QUYEN_NHOM_DELETE";

            object[] aParams = new object[3];

            try
            {
                aParams[0] = helper.BuildParameter("prmID_QUYEN", obj.id_quyen, OracleDbType.Int32, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmID_NHOM", obj.id_nhom, OracleDbType.Int32, ParameterDirection.Input);
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
    }
}
