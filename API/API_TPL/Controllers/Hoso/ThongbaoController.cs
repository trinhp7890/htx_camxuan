using API_TPL.DAL;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace API_TPL.Controllers.Congviec
{
    [Authorize]
    [RoutePrefix("api/thongbao")]
    public class ThongbaoController : ApiController
    {
        static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["QLCV"].ToString();
        DBHelper helper = new DBHelper(connString);
        ApiHelpers api_helper = new ApiHelpers();
        /// <summary>
        /// Thêm mới 
        /// </summary>
        [Route("create"), HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult THONGBAO_INS([FromBody] dynamic obj)
        {
            string query_str = "THONGBAO_INS";

            object[] aParams = new object[1];

            try
            {
                aParams[0] = helper.BuildParameter("prmJsonData", obj, OracleDbType.Clob, ParameterDirection.Input);
                String kq = helper.ExecuteNonQuery(query_str, aParams);
                if (kq == "OK")
                {
                    HttpResponseMessage tn;
                    tn = api_helper.post("https://fcm.googleapis.com", "fcm/send", obj);
                    string a = tn.Content.ToString();
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
        /// notifi
        /// </summary>
        [Route("notifi"), HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult THONGBAO_NOTIFI([FromBody] dynamic obj)
        {
            try
            {
                if (obj != null)
                {
                    HttpResponseMessage tn;
                    tn = api_helper.post("https://fcm.googleapis.com", "fcm/send", obj.model);
                    string a = tn.Content.ToString();
                }
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, "OK"));
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, err));
            }
        }
        /// <summary>
        /// update nội dung thông báo
        /// </summary>
        [Route("update"), HttpPost]
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult THONGBAO_UP([FromBody] dynamic obj)
        {
            string query_str = "THONGBAO_UP";

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
        /// Xóa thông báo.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>        
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("delete"), HttpPost]
        public IHttpActionResult THONGBAO_DEL(string prmMA_TB, string prmNGUOI_NHAP)
        {
            string query_str = "THONGBAO_DEL";

            object[] aParams = new object[2];

            try
            {
                aParams[0] = helper.BuildParameter("prmMA_TB", prmMA_TB, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmNGUOI_NHAP", prmNGUOI_NHAP, OracleDbType.Varchar2, ParameterDirection.Input);
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
        /// Xóa thông báo.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>        
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("deletefile"), HttpPost]
        public IHttpActionResult THONGBAO_DEL_FILE(string prmIDFILE, string prmNGUOI_NHAP)
        {
            string query_str = "THONGBAO_DEL_FILE";

            object[] aParams = new object[2];

            try
            {
                aParams[0] = helper.BuildParameter("prmIDFILE", prmIDFILE, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmNGUOI_NHAP", prmNGUOI_NHAP, OracleDbType.Varchar2, ParameterDirection.Input);
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
        /// Xem tất cả thông báo.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>    
        [Route("getall"), HttpGet]
        public IHttpActionResult getall()
        {
            string query_str = "THONGBAO_VIEW_ALL";
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
        /// View thông báo theo mã
        /// </summary>
        [Route("getbyma"), HttpGet]
        public IHttpActionResult getbyma(string prmMA_TB)
        {
            string query_str = "THONGBAO_BYMA";
            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmMA_TB", prmMA_TB, OracleDbType.Varchar2, ParameterDirection.Input);
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
        /// View thông báo theo mã
        /// </summary>
        [Route("realtimebymanv"), HttpGet]
        public IHttpActionResult realtimebymanv(string prmMA_NV)
        {
            string query_str = "THONGBAO_REALTIME_BYMANV";
            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmMA_NV", prmMA_NV, OracleDbType.Varchar2, ParameterDirection.Input);
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
        /// update thông báo realtime
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>        
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("realtimeupdate"), HttpPost]
        public IHttpActionResult realtimeupdate(int prmIDTHONGBAO)
        {
            string query_str = "THONGBAO_REALTIME_UP";

            object[] aParams = new object[1];

            try
            {
                aParams[0] = helper.BuildParameter("prmIDTHONGBAO", prmIDTHONGBAO, OracleDbType.Int32, ParameterDirection.Input);                
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
        /// View thông báo theo mã
        /// </summary>
        [Route("getfilebyma"), HttpGet]
        public IHttpActionResult getfilebyma(string prmMA_TB)
        {
            string query_str = "FILE_THONGBAO_BYMA";
            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmMA_TB", prmMA_TB, OracleDbType.Varchar2, ParameterDirection.Input);
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
        /// Gửi thông báo telegram <br />
        /// </summary>
        /// <returns></returns>
        [Route("send_msg_telegram"), HttpGet]
        //[HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        public IHttpActionResult send_msg_telegram(string noidung, string prmMA_NVS, string prmfrom, string prmChangeType)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;          
           
            /*Thêm mới vào db các id chưa được gửi link*/
            string query_str = "TELEGRAM_GETBY_MANV";
            string qry = "HETHONG_NGUOIDUNG_GET_BYMANV";

            object[] aParams = new object[2];

            try
            {
                aParams[0] = helper.BuildParameter("prmMA_NVS", prmMA_NVS, OracleDbType.Varchar2, ParameterDirection.Input);

                OracleParameter resultParam = new OracleParameter("results", OracleDbType.RefCursor);
                resultParam.Direction = ParameterDirection.Output;

                aParams[1] = resultParam;
                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);
                var Data_length = kq.Rows.Count;
                
                // lấy thông tin người gửi
                aParams = new object[2];
                aParams[0] = helper.BuildParameter("prmMA_NV", prmfrom, OracleDbType.Varchar2, ParameterDirection.Input);

                resultParam = new OracleParameter("results", OracleDbType.RefCursor);
                resultParam.Direction = ParameterDirection.Output;

                aParams[1] = resultParam;
                DataTable kq_from = helper.ExecuteQueryStoreProcedure(qry, aParams);
                string tennguoigui = "";
                if(kq_from.Rows.Count> 0)
                {
                    tennguoigui = kq_from.Rows[0]["TEN_ND"].ToString();
                }

                string noidung_msg = tennguoigui  + " đã " + prmChangeType + " công việc [" + noidung + "]";
                /*Gửi tin nhắn link đăng ký*/
                for (int i = 0; i < Data_length; i++) {
                    
                    string url_send = "https://api.telegram.org/bot5507340091:AAF01yPWs2GcD11_fcaEBdKdPWDtXIw-9zc/sendMessage?chat_id=" + kq.Rows[i]["ID_USER_TELE"] + "&text=" + noidung_msg;
                    using (var webClient = new System.Net.WebClient())
                    {
                        var json_send = webClient.DownloadString(url_send);
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
    }
}
