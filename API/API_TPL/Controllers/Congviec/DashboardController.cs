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
    [RoutePrefix("api/dashboard")]
    public class DashboardController : ApiController
    {
        static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["QLCV"].ToString();
        DBHelper helper = new DBHelper(connString);

        /// <summary>
        /// View box
        /// </summary>
        [Route("viewbox"), HttpGet]
        public IHttpActionResult DASHBOARD_CANHAN_BOX(string prmMA_NV)
        {
            string query_str = "DASHBOARD.DASHBOARD_CANHAN_BOX";
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
        /// View chi tiết công việc theo trạng thái
        /// </summary>
        [Route("view_chitiet"), HttpGet]
        public IHttpActionResult DASHBOARD_CANHAN_CHITIET(string prmMA_NV)
        {
            string query_str = "DASHBOARD.DASHBOARD_CANHAN_CHITIET";
            object[] aParams = new object[4];
            try
            {
                aParams[0] = helper.BuildParameter("prmMA_NV", prmMA_NV, OracleDbType.Varchar2, ParameterDirection.Input);


                OracleParameter resultParam_Quahan = new OracleParameter("RESULTS_CV_QUAHAN", OracleDbType.RefCursor);
                resultParam_Quahan.Direction = ParameterDirection.Output;

                aParams[1] = resultParam_Quahan;
                OracleParameter resultParam_Dagiao = new OracleParameter("RESULTS_CV_DAGIAO", OracleDbType.RefCursor);
                resultParam_Dagiao.Direction = ParameterDirection.Output;

                aParams[2] = resultParam_Dagiao;
                OracleParameter resultParam_Duocgiao = new OracleParameter("RESULTS_CV_DUOCGIAO", OracleDbType.RefCursor);
                resultParam_Duocgiao.Direction = ParameterDirection.Output;

                aParams[3] = resultParam_Duocgiao;

                DataSet kq = helper.ExecuteQueryStoreProcedure_List(query_str, aParams);

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, kq));
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, err));
            }
        }
        /// <summary>
        /// View chi tiết công việc theo trạng thái
        /// </summary>
        [Route("view_bieudo"), HttpGet]
        public IHttpActionResult DASHBOARD_CANHAN_BIEUDO(string prmMA_NV)
        {
            string query_str = "DASHBOARD.DASHBOARD_CANHAN_BIEUDO";
            object[] aParams = new object[3];
            try
            {
                aParams[0] = helper.BuildParameter("prmMA_NV", prmMA_NV, OracleDbType.Varchar2, ParameterDirection.Input);


                OracleParameter resultParam_Dagiao = new OracleParameter("RESULTS_CV_DAGIAO", OracleDbType.RefCursor);
                resultParam_Dagiao.Direction = ParameterDirection.Output;

                aParams[1] = resultParam_Dagiao;
                OracleParameter resultParam_Duocgiao = new OracleParameter("RESULTS_CV_DUOCGIAO", OracleDbType.RefCursor);
                resultParam_Duocgiao.Direction = ParameterDirection.Output;

                aParams[2] = resultParam_Duocgiao;  

                DataSet kq = helper.ExecuteQueryStoreProcedure_List(query_str, aParams);

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, kq));
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, err));
            }
        }
        /// <summary>
        /// View box
        /// </summary>
        [Route("viewbox_donvi"), HttpGet]
        public IHttpActionResult DASHBOARD_DONVI_BOX(string prmMA_DV,int prmChildNumber)
        {
            string query_str = "DASHBOARD.DASHBOARD_DONVI_BOX";
            object[] aParams = new object[3];
            try
            {
                aParams[0] = helper.BuildParameter("prmMA_DV", prmMA_DV, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmChildNumber", prmChildNumber, OracleDbType.Int32, ParameterDirection.Input);

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
        /// View chi tiết công việc theo trạng thái của đơn vị
        /// </summary>
        [Route("view_chitiet_donvi"), HttpGet]
        public IHttpActionResult DASHBOARD_DONVI_CHITIET(string prmMA_DV, int prmChildNumber)
        {
            string query_str = "DASHBOARD.DASHBOARD_DONVI_CHITIET";
            object[] aParams = new object[4];
            try
            {
                aParams[0] = helper.BuildParameter("prmMA_DV", prmMA_DV, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmChildNumber", prmChildNumber, OracleDbType.Int32, ParameterDirection.Input);


                OracleParameter resultParam_Quahan = new OracleParameter("RESULTS_CV_QUAHAN", OracleDbType.RefCursor);
                resultParam_Quahan.Direction = ParameterDirection.Output;

                aParams[2] = resultParam_Quahan;
                OracleParameter resultParam_Duocgiao = new OracleParameter("RESULTS_CV_DUOCGIAO", OracleDbType.RefCursor);
                resultParam_Duocgiao.Direction = ParameterDirection.Output;

                aParams[3] = resultParam_Duocgiao;
                

                DataSet kq = helper.ExecuteQueryStoreProcedure_List(query_str, aParams);

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
