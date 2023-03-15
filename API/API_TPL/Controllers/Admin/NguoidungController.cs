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
using API_TPL.Services;
using System.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http.Headers;
using Newtonsoft.Json;

namespace HUE_CDC.Controllers.Admin
{
    /// <summary>
    /// hệ thống người dùng
    /// </summary>
    //[Authorize]
    [RoutePrefix("api/nguoidung")]
    public class NguoidungController : ApiController
    {
        static String connString = ConfigurationManager.ConnectionStrings["PHANBONConnection"].ToString();
        SQL_DBHELPERs helper = new SQL_DBHELPERs(connString);
        ApiHelpers apixl = new ApiHelpers();
        /// <summary>
        /// Thêm mới tài khoản người dùng
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>

        [Route("create"), HttpPost]
        public IHttpActionResult HETHONG_NGUOIDUNG_INSERT([FromBody] dynamic obj)
        {
            string query_str = "HETHONG_NGUOIDUNG_INSERT";

            object[] aParams = new object[1];            
            
            //obj.password = HashHelper.GetMd5Hash(obj.password);
            try
            {
                aParams[0] = helper.BuildParameter("prmJsonData", obj, System.Data.SqlDbType.NVarChar);

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
        /// Cập nhật tài khoản người dùng
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("update"), HttpPost]
        public IHttpActionResult HETHONG_NGUOIDUNG_UPDATE([FromBody] dynamic obj)
        {
            string query_str = "HETHONG_NGUOIDUNG_UPDATE";

            object[] aParams = new object[1];

            try
            {
                aParams[0] = helper.BuildParameter("prmJsonData", obj, System.Data.SqlDbType.NVarChar);

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
        /// Cập nhật trạng thái người dùng
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("update_active"), HttpPost]
        public IHttpActionResult update_active([FromBody] dynamic obj)//int prm_Id_nd, int prmActive)
        {
            string query_str = "HETHONG_NGUOIDUNG_UPDATE_ACTIVE";

            object[] aParams = new object[2];

            try
            {
                aParams[0] = helper.BuildParameter("prm_Id_nd", obj.id_nd,System.Data.SqlDbType.Int);                
                aParams[1] = helper.BuildParameter("prm_NGUOI_CN", obj.nguoi_cn,System.Data.SqlDbType.NVarChar);

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
        /// reset pass người dùng
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("resetpw"), HttpPost]
        public IHttpActionResult reset_ps([FromBody] dynamic obj)
        {
            string query_str = "HETHONG_NGUOIDUNG_RESET_PW";

            object[] aParams = new object[2];

            try
            {
                aParams[0] = helper.BuildParameter("prmID_ND", obj.id_nd, System.Data.SqlDbType.Int);
                aParams[1] = helper.BuildParameter("prmNGUOI_CN", obj.nguoi_cn, System.Data.SqlDbType.NVarChar);

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
        /// change pass người dùng
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("cpw"), HttpPost]
        public IHttpActionResult cpw([FromBody] dynamic obj)
        {
            string query_str = "HETHONG_NGUOIDUNG_CHANGE_PW";

            object[] aParams = new object[1];

            try
            {
																																 
                aParams[0] = helper.BuildParameter("prmJsonData", obj.prmJsonData, System.Data.SqlDbType.NVarChar);
																															  

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
        /// Lấy thông tin người dùng. <br />
        /// </summary>
        /// <returns></returns>
        [Route("getall"), HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult GetAllGroup()
        {
            string query_str = "HETHONG_NGUOIDUNG_GETALL";
            object[] aParams = new object[0];
            try
            {
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
        /// Lấy thông tin người dùng theo đơn vị. <br />
        /// </summary>
        /// <returns></returns>
        [Route("getall_bydv"), HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult GetAllUser_BYDV(string prmMA_DV)
        {

            string query_str = "HETHONG_NGUOIDUNG_GETALL_BYDV";
            object[] aParams = new object[1];
            try
            {
                aParams[0] = helper.BuildParameter("prmMA_DV", prmMA_DV, System.Data.SqlDbType.NVarChar);
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
        /// Lấy thông tin người nguoi giam sat. <br />
        /// </summary>
        /// <returns></returns>
        [Route("getall_giamsat"), HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult GetAllGiamsat()
        {

            string query_str = "HETHONG_NGUOIDUNG_GIAMSAT";
            object[] aParams = new object[0];
            try
            {
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
        /// Lấy thông tin người nguoi giam sat. <br />
        /// </summary>
        /// <returns></returns>
        [Route("get_telegram"), HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult Get_Telegram( string prmMA_NV)
        {

            string query_str = "HETHONG_NGUOIDUNG_CHECKTELEGRAM";
            object[] aParams = new object[1];
            try
            {
                aParams[0] = helper.BuildParameter("prmMA_NV", prmMA_NV , System.Data.SqlDbType.NVarChar);

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
        /// đăng ký thông tin nhận thông báo telegram.
        /// </summary>
        /// <param>
        /// string prmID_USER_TELE: ID telegram của người dùng
        /// string prmUSERNAME_WEB: username
        /// string prmMA_NV: mã nhân viên của người dùng
        /// </param>
        /// <returns></returns>        
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("Telegram_up"), HttpPost]
        public IHttpActionResult TELEGRAM_UP(string prmID_USER_TELE, string prmUSERNAME_WEB, string prmMA_NV)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

            string query_str = "HETHONG_NGUOIDUNG_TELEGRAM_UP";

            object[] aParams = new object[3];

            try
            {
                aParams[0] = helper.BuildParameter("prmID_USER_TELE", prmID_USER_TELE, System.Data.SqlDbType.NVarChar);
                aParams[1] = helper.BuildParameter("prmUSERNAME_WEB", prmUSERNAME_WEB, System.Data.SqlDbType.NVarChar);
                aParams[2] = helper.BuildParameter("prmMA_NV", prmMA_NV, System.Data.SqlDbType.NVarChar);

                String kq = helper.ExecuteNonQuery(query_str, aParams);

                string url_send = "https://api.telegram.org/bot5507340091:AAF01yPWs2GcD11_fcaEBdKdPWDtXIw-9zc/sendMessage?chat_id=" + prmID_USER_TELE + "&text=Đăng ký nhận thông báo Telegram thành công!";
                using (var webClient = new System.Net.WebClient())
                {
                    var json_send = webClient.DownloadString(url_send);
                }

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, kq));
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, err));
            }
        }		 
        /// Lấy thông tin người dùng theo uid. <br />
        /// </summary>
        /// <returns></returns>
        [Route("get"), HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult GetUserByUid(int uid)
        {

            string query_str = "HETHONG_NGUOIDUNG_BYID";
            object[] aParams = new object[1];
            try
            {
                aParams[0] = helper.BuildParameter("prmID_ND", uid, System.Data.SqlDbType.NVarChar);

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
        /// Lấy thông tin người dùng theo uid. <br />
        /// </summary>
        /// <returns></returns>
        [Route("getchitiet"), HttpGet]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult Getchitietusser(string prmUSERNAME)
        {

            string query_str = "NGUOIDUNG_CHITIET";
            object[] aParams = new object[1];
            try
            {
                aParams[0] = helper.BuildParameter("prmUSERNAME", prmUSERNAME, System.Data.SqlDbType.NVarChar);
  
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
        /// Lấy thông tin người dùng theo uid. <br />
        /// </summary>
        /// <returns></returns>
        [Route("job_update_userid"), HttpGet]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult job_update_userid()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string url = "https://api.telegram.org/bot5507340091:AAF01yPWs2GcD11_fcaEBdKdPWDtXIw-9zc/getUpdates";
            var idString = "";
            /*Lấy danh sách id_user có tương tác với bot*/
            using (var webClient = new System.Net.WebClient())
            {
                
                var json = webClient.DownloadString(url);
                JObject obj = JObject.Parse(json);
                var result = obj.Last.Last;
                var id_telegram_get = "";
                for (int i = 0; i < result.Count(); i++)
                {
                    id_telegram_get = result[i].Last.Last.Parent.First["from"]["id"].ToString();
                    if (!idString.Contains(id_telegram_get))
                    {
                        idString += id_telegram_get + ",";
                    }
                }
            }
            /*Thêm mới vào db các id chưa được gửi link*/
            string query_str = "TELEGRAM_IDUSER_INS";
            object[] aParams = new object[1];
            try
            {
                aParams[0] = helper.BuildParameter("prmID", idString, System.Data.SqlDbType.NVarChar);

                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);
                var Data_length = kq.Rows.Count;

                /*Gửi tin nhắn link đăng ký*/
                if (kq.Rows.Count > 1)
                {
                    for (int i = 0; i < Data_length; i++)
                    {
                        var url_register = "Vui lòng click vào đường link để đăng ký nhận thông báo từ ứng dụng Quản lý công việc:http://10.47.135.141:8087/Hethong/logintelegram/" + kq.Rows[i]["id_user"].ToString();
                        string url_send = "https://api.telegram.org/bot5507340091:AAF01yPWs2GcD11_fcaEBdKdPWDtXIw-9zc/sendMessage?chat_id=" + kq.Rows[i]["id_user"] + "&text=" + url_register;
                        using (var webClient = new System.Net.WebClient())
                        {
                            var json_send = webClient.DownloadString(url_send);
                        }
                    }
                }
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, kq)); 
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, err));
            }    

        }  
        
        /// <summary>
        /// Xóa người dùng
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("nguoigiamsat_add"), HttpPost]
        public IHttpActionResult NGUOIGIAMSAT_ADD(string prmMA_NV, int prmTRANGTHAI)
        {
            string query_str = "NGUOIGIAMSAT_ADD";

            object[] aParams = new object[2];

            try
            {
                aParams[0] = helper.BuildParameter("prmMA_NV", prmMA_NV, System.Data.SqlDbType.NVarChar);
                aParams[1] = helper.BuildParameter("prmTRANGTHAI", prmTRANGTHAI, System.Data.SqlDbType.Int);

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
        /// Xóa người dùng
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("nguoidungdel"), HttpPost]
        public IHttpActionResult NGUOIDUNG_DEL(string prmMA_NV, string prmNGUOI_CAPNHAT)
        {
            string query_str = "NGUOIDUNG_DEL";

            object[] aParams = new object[2];

            try
            {
                aParams[0] = helper.BuildParameter("prmMA_NV", prmMA_NV, System.Data.SqlDbType.NVarChar);
                aParams[1] = helper.BuildParameter("prmNGUOI_CAPNHAT", prmNGUOI_CAPNHAT, System.Data.SqlDbType.NVarChar);

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
        /// Xóa người dùng
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        /// <summary>
        /// Xóa người dùng
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]																   
        [Route("delete"), HttpPost]
        public IHttpActionResult HETHONG_NGUOIDUNG_DELETE([FromBody]dynamic obj )
        {
            string query_str = "HETHONG_NGUOIDUNG_DELETE";

            object[] aParams = new object[2];

            try
            {
                aParams[0] = helper.BuildParameter("prmID_ND", obj.id_nd, System.Data.SqlDbType.Int);
                aParams[1] = helper.BuildParameter("prmNGUOI_CN", obj.nguoi_cn, System.Data.SqlDbType.NVarChar);

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
        /// View nhân viên by id
        /// </summary>
        [Route("getbydonvi"), HttpGet]
        public IHttpActionResult NHANVIEN_BYDONVI(string prmMA_DV)
        {
            string query_str = "NHANVIEN_BYDONVI";
            object[] aParams = new object[1];
            try
            {
                aParams[0] = helper.BuildParameter("prmMA_DV", prmMA_DV, System.Data.SqlDbType.NVarChar);

                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);

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
