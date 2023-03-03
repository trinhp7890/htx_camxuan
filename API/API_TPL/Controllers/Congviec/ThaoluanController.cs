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

namespace API_TPL.Controllers.Congviec
{
    [Authorize]
    [RoutePrefix("api/thaoluan")]
    public class ThaoluanController : ApiController
    {
        static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["QLCV"].ToString();
        DBHelper helper = new DBHelper(connString);
        /// <summary>
        /// <b>Mục đích::</b>Thêm mới nội dung thảo luận công việc <br />
        /// <b>Tham số URI:</b> prmJsonData. JSON <br />
        ///<b>Trả về:</b> Datatable <br />
        /// </summary>      
        /// 
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("thaoluan_ins_upload"), HttpPost]
        public IHttpActionResult THAOLUAN_INS_UPLOAD()
        {
            string timenow = DateTime.Now.ToString("ddMMyyyy_HHmm");
            string _uploadPath = ConfigurationManager.AppSettings["upload_path"].ToString().Trim();
            string _typePath = ConfigurationManager.AppSettings["image_folder"].ToString().Trim();
            string _typevanban = ConfigurationManager.AppSettings["upload_folder"].ToString().Trim();
            string workingFolder = HttpContext.Current.Server.MapPath("~" + _uploadPath + _typePath);
            string workingFolderfile = HttpContext.Current.Server.MapPath("~" + _uploadPath + _typevanban + "/" + timenow);

            string workingFolderfilesave = _uploadPath + _typevanban + "/" + timenow;


            if (!Directory.Exists(workingFolder))
            {
                Directory.CreateDirectory(workingFolder); //Create directory if it doesn't exist
            }
            if (!Directory.Exists(workingFolderfile))
            {
                Directory.CreateDirectory(workingFolderfile); //Create directory if it doesn't exist
            }
            var httpRequest = HttpContext.Current.Request;
            var Httpfroms = httpRequest.Form;
            var prmMA_CONGVIEC = Httpfroms.Get("prmMA_CONGVIEC");
            var prmMA_NHANVIEN = Httpfroms.Get("prmMA_NHANVIEN");
            var prmNOIDUNG_THAOLUAN = Httpfroms.Get("prmNOIDUNG_THAOLUAN");
            var prmNGUOI_CAPNHAT = Httpfroms.Get("prmNGUOI_CAPNHAT");


            string filePathsave = "";
            string filename = "";
            StringBuilder str_prmJsonDatas = new StringBuilder();
            str_prmJsonDatas.Append("[");
            StringBuilder str_ojb = new StringBuilder();
            for (int i = 0; i < httpRequest.Files.Count; i++)
            {
                filename = httpRequest.Files[i].FileName;
                filePathsave = workingFolderfilesave;
                str_ojb = new StringBuilder();
                str_ojb.Append("'TEN_FILE':");
                str_ojb.Append("'" + filename + "',");

                str_ojb.Append("'DUONGDAN_FILE':");
                str_ojb.Append("'" + filePathsave + "/',");

                str_ojb.Append("'GHICHU':");
                str_ojb.Append("'Upload file insert công việc phát sinh'");

                str_prmJsonDatas.Append("{" + str_ojb.ToString() + "},");

                //httpRequest.Files[i].SaveAs(filePathsave);
            }
            if (httpRequest.Files.Count == 0)
            {
                str_prmJsonDatas.Append("]");
            }
            else
            {
                str_prmJsonDatas.Remove(str_prmJsonDatas.Length - 1, 1);
                str_prmJsonDatas.Append("]");
            }

            //Object jsonObject_ = JsonConvert.DeserializeObject(str_prmJsonDatas.ToString());

            string query_str = "BINHLUAN_INS_UPLOAD";
            object[] aParams = new object[5];
            string prmMULTI_FILE = str_prmJsonDatas.ToString();
            prmMULTI_FILE.Replace("{[", "[");
            prmMULTI_FILE.Replace("]}", "]");
            prmMULTI_FILE.Replace("'", "\"");

            try
            {
                aParams[0] = helper.BuildParameter("prmMA_CONGVIEC", prmMA_CONGVIEC, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmMA_NHANVIEN", prmMA_NHANVIEN, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[2] = helper.BuildParameter("prmNOIDUNG_THAOLUAN", prmNOIDUNG_THAOLUAN, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[3] = helper.BuildParameter("prmNGUOI_CAPNHAT", prmNGUOI_CAPNHAT, OracleDbType.Varchar2, ParameterDirection.Input);                
                aParams[4] = helper.BuildParameter("prmMULTI_FILE", prmMULTI_FILE, OracleDbType.Clob, ParameterDirection.Input);

                String kq = helper.ExecuteNonQuery(query_str, aParams);
                if (kq == "OK")
                {
                    for (int i = 0; i < httpRequest.Files.Count; i++)
                    {
                        filename = httpRequest.Files[i].FileName;
                        filePathsave = workingFolderfile + "\\" + filename;
                        httpRequest.Files[i].SaveAs(filePathsave);
                    }
                }
                else
                {
                    return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Lỗi cập nhật dữ liệu"));
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
        /// View công việc theo mã
        /// </summary>
        [Route("getbymacv"), HttpGet]
        public IHttpActionResult THAOLUAN_BYMA_CV(string prmMA_CONGVIEC)
        {
            string query_str = "CONGVIEC_PHATSINH_BINHLUAN";
            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmMA_CONGVIEC", prmMA_CONGVIEC, OracleDbType.Varchar2, ParameterDirection.Input);


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
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("create"), HttpPost]
        public IHttpActionResult THAOLUAN_CONGVIEC_INS([FromBody] dynamic obj)
        {
            string query_str = "THAOLUAN_CONGVIEC_INS";

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
        /// <b>Mục đích::</b> Chỉnh sửa nội dung thảo luận <br />
        /// <b>Tham số URI:</b> prmJsonData. JSON <br />
        ///<b>Trả về:</b> Datatable <br />
        /// </summary>      
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("update"), HttpPost]
        public IHttpActionResult THAOLUAN_CONGVIEC_UP([FromBody] dynamic obj)
        {
            string query_str = "THAOLUAN_CONGVIEC_UP";

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
        /// Xóa nội dung thảo luận
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>        
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("delete"), HttpPost]
        public IHttpActionResult THAOLUAN_CONGVIEC_DEL([FromBody] dynamic obj)
        {
            string query_str = "THAOLUAN_CONGVIEC_DEL";

            object[] aParams = new object[2];

            try
            {
                aParams[0] = helper.BuildParameter("prmID_THAOLUAN", obj.ID_THAOLUAN, OracleDbType.Int32, ParameterDirection.Input);
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
        /// View nội dung thảo luận của công việc
        /// </summary>
        [Route("getbycongviec"), HttpGet]
        public IHttpActionResult THAOLUAN_CONVIEC_BYMACONGVIEC(string prmMA_CONGVIEC)
        {
            string query_str = "THAOLUAN_CONVIEC_BYIDCONGVIEC";
            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmMA_CONGVIEC", prmMA_CONGVIEC, OracleDbType.Varchar2, ParameterDirection.Input);


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
        /// View thảo luận by id
        /// </summary>
        [Route("getbyid"), HttpGet]
        public IHttpActionResult THAOLUAN_CONVIEC_BYID(int prmID_THAOLUAN)
        {
            string query_str = "THAOLUAN_CONVIEC_BYID";
            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmID_THAOLUAN", prmID_THAOLUAN, OracleDbType.Int32, ParameterDirection.Input);


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
