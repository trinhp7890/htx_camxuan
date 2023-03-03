using API_TPL.DAL;
using Microsoft.AspNet.Identity;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using API_TPL.Models;

namespace API_TPL.Controllers.Danhmuc
{
    //[Authorize]
    [RoutePrefix("api/dmuyquyengiaoviec")]
    public class UyquyengiaoviecController : ApiController
    {
        static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["QLCV"].ToString();
        DBHelper helper = new DBHelper(connString);

        /// <summary>
        /// <b>Mục đích::</b> Thêm mới ủy quyền giao việc <br />
        /// <b>Tham số URI:</b> Httpfroms<br />
        ///<b>Trả về:</b> Datatable <br />
        /// </summary>    
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UYQUYENGIAOVIEC_INS"), HttpPost]
        public IHttpActionResult UYQUYENGIAOVIEC_INS(string prmMA_NV_UYQUYEN, string prmMA_NV_DUOCUYQUYEN)
        {
            
            string query_str = "UYQUYENGIAOVIEC_INS";
            object[] aParams = new object[2];

            try
            {
                aParams[0] = helper.BuildParameter("prmMA_NV_UYQUYEN", prmMA_NV_UYQUYEN, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmMA_NV_DUOCUYQUYEN", prmMA_NV_DUOCUYQUYEN, OracleDbType.Varchar2, ParameterDirection.Input);                
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
        /// <b>Mục đích::</b> Thêm mới ủy quyền giao việc <br />
        /// <b>Tham số URI:</b> Httpfroms<br />
        ///<b>Trả về:</b> Datatable <br />
        /// </summary>    
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("UYQUYENGIAOVIEC_UP"), HttpPost]
        public IHttpActionResult UYQUYENGIAOVIEC_UP(int prmID_UYQUYEN_GV,  string prmMA_NV_UYQUYEN, string prmMA_NV_DUOCUYQUYEN, string prmTRANGTHAI)
        {

            string query_str = "UYQUYENGIAOVIEC_UP";
            object[] aParams = new object[4];

            try
            {
                aParams[0] = helper.BuildParameter("prmID_UYQUYEN_GV", prmID_UYQUYEN_GV, OracleDbType.Int32, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmMA_NV_UYQUYEN", prmMA_NV_UYQUYEN, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[2] = helper.BuildParameter("prmMA_NV_DUOCUYQUYEN", prmMA_NV_DUOCUYQUYEN, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[3] = helper.BuildParameter("prmTRANGTHAI", prmTRANGTHAI, OracleDbType.Varchar2, ParameterDirection.Input);
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
        /// Get ALL danh sách ủy quyền giao việc
        /// </summary>
        [Route("GETALL"), HttpGet]
        public IHttpActionResult UYQUYENGIAOVIEC_VIEWALL()
        {
            string query_str = "UYQUYENGIAOVIEC_VIEWALL";
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
        /// Get ALL danh sách ủy quyền giao việc
        /// </summary>
        [Route("GETBYMNV"), HttpGet]
        public IHttpActionResult UYQUYENGIAOVIEC_GETBYMNV(string prmMA_NV)
        {
            string query_str = "UYQUYENGIAOVIEC_GETBY_MNV";
            object[] aParams = new object[2];
            try
            {
                
                OracleParameter resultParam = new OracleParameter("results", OracleDbType.RefCursor);
                resultParam.Direction = ParameterDirection.Output;
                
                aParams[0] = helper.BuildParameter("prmMA_NV", prmMA_NV, OracleDbType.Varchar2, ParameterDirection.Input);
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
        /// Xóa thông báo.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>        
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("delete"), HttpPost]
        public IHttpActionResult NGUONPHATSINH_DEL(int prmID_UYQUYEN_GV)
        {
            string query_str = "UYQUYENGIAOVIEC_DEL";

            object[] aParams = new object[1];

            try
            {
                aParams[0] = helper.BuildParameter("prmID_UYQUYEN_GV", prmID_UYQUYEN_GV, OracleDbType.Int32, ParameterDirection.Input);                
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
        [Route("Getbyid"), HttpGet]
        public IHttpActionResult Getbyid(int prmID_UYQUYEN_GV)
        {
            string query_str = "UYQUYENGIAOVIEC_BYID";
            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmID_UYQUYEN_GV", prmID_UYQUYEN_GV, OracleDbType.Varchar2, ParameterDirection.Input);
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
    }
}
