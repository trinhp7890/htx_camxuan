using API_TPL.DAL;
using Microsoft.AspNet.Identity;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net; 
using System.Net.Http;
using System.Web.Http;


namespace API_TPL.Controllers.Congviec
{
    [Authorize]
    [RoutePrefix("api/duan")]
    public class DuanController : ApiController
    {
        static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["CAHUE"].ToString();
        DBHelper helper = new DBHelper(connString);

        /// <summary>
        /// <b>Mục đích::</b>Thêm mới dự án <br />
        /// <b>Tham số URI:</b> prmJsonData. JSON <br />
        ///<b>Trả về:</b> Datatable <br />
        /// </summary>      
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("create"), HttpPost]
        public IHttpActionResult DM_DUAN_INS([FromBody] dynamic obj)
        {
            string query_str = "DM_DUAN_INS"; //Cái này là tên SP

            object[] aParams = new object[1];

            try
            {
                //Tham số đầu vào
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
        /// <b>Mục đích::</b> Chỉnh sửa dự án <br />
        /// <b>Tham số URI:</b> prmJsonData. JSON <br />
        ///<b>Trả về:</b> Datatable <br />
        /// </summary>      
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("update"), HttpPost]
        public IHttpActionResult DM_DUAN_UP([FromBody] dynamic obj)
        {
            string query_str = "DM_DUAN_UP";

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
        /// Xóa dự án
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>        
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("delete"), HttpPost]
        public IHttpActionResult DM_DUAN_DEL([FromBody] dynamic obj)
        {
            string query_str = "DM_DUAN_DEL";

            object[] aParams = new object[2];

            try
            {
                aParams[0] = helper.BuildParameter("prmMA_DUAN", obj.MA_DUAN, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmNGUOI_NHAP", obj.NGUOI_NHAP, OracleDbType.Varchar2, ParameterDirection.Input);

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
        /// View dự án theo mã
        /// </summary>
        [Route("getbymaduan"), HttpGet]
        public IHttpActionResult DM_DUAN_BYID(string prmMA_DUAN)
        {
            string query_str = "DM_DUAN_BYID";
            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmMA_DUAN", prmMA_DUAN, OracleDbType.Varchar2, ParameterDirection.Input);


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
        ///<summary>
        ///<b>Mục đích:</b>Lấy danh sách tất cả các dự án. <br />
        ///<b>Tham số URI:</b> Không có. <br />
        ///<b>Trả về:</b> Datatable <br />
        ///</summary>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("getAll"), HttpGet]
        public DataTable GetAll()
        {
            string qstr = "DM_DUAN_VIEW_ALL";
            object[] arr_params = new object[1];

            OracleParameter param1 = new OracleParameter("result", OracleDbType.RefCursor, ParameterDirection.Output);
            arr_params[0] = param1;
            return helper.ExecuteQueryStoreProcedure(qstr, arr_params);
        }
    }
}
