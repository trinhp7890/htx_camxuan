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
   // [Authorize]
    [RoutePrefix("api/congviecphatsinh")]
    public class CongviecController : ApiController
    {
        static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["QLCV"].ToString();
        DBHelper helper = new DBHelper(connString);

        /// <summary>
        /// <b>Mục đích::</b>Thêm mới công việc <br />
        /// <b>Tham số URI:</b> prmJsonData. JSON <br />
        ///<b>Trả về:</b> Datatable <br />
        /// </summary>      
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("create"), HttpPost]
        public IHttpActionResult CONGVIEC_INS([FromBody] dynamic obj)
        {
            string query_str = "CONGVIEC_INS"; //Cái này là tên SP

            object[] aParams = new object[2];

            try
            {
                //Tham số đầu vào
                aParams[0] = helper.BuildParameter("prmJsonData", obj, OracleDbType.Clob, ParameterDirection.Input);

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
        [Route("congviec_ins_upload"), HttpPost]
        public IHttpActionResult CONGVIEC_INS_UPLOAD()
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
            var prmTEN_CV = Httpfroms.Get("prmTEN_CV");
            var prmNGUON_PHATSINH = Httpfroms.Get("prmNGUON_PHATSINH");
            var prmNGUOI_GIAO = Httpfroms.Get("prmNGUOI_GIAO");
            var prmNOIDUNG = Httpfroms.Get("prmNOIDUNG");
            var prmDONVI_CHUTRI = Httpfroms.Get("prmDONVI_CHUTRI");
            var prmNGUOI_CHUTRI = Httpfroms.Get("prmNGUOI_CHUTRI");
            var prmMA_CONGVIEC_CHA = Httpfroms.Get("prmMA_CONGVIEC_CHA");
            var prmMA_DUAN = Httpfroms.Get("prmMA_DUAN");
            var prmLOAI_CV = "3";
            var prmNGAY_BATDAU = Httpfroms.Get("prmNGAY_BATDAU");
            var prmNGAY_KETTHUC = Httpfroms.Get("prmNGAY_KETTHUC");
            var prmNGUOI_PHOIHOP = Httpfroms.Get("prmNGUOI_PHOIHOP");
            var prmDONVI_PHOIHOP = Httpfroms.Get("prmDONVI_PHOIHOP");
            var prmNGUOI_CAPNHAT = Httpfroms.Get("prmNGUOI_CAPNHAT");
            var prmTINH_CHAT = Httpfroms.Get("prmTINH_CHAT");
            var prmNGUOI_GIAMSAT = Httpfroms.Get("prmNGUOI_GIAMSAT");

           
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

                string query_str = "CONGVIEC_INS_UPLOAD";
                object[] aParams = new object[17];
                string prmMULTI_FILE = str_prmJsonDatas.ToString();
                prmMULTI_FILE.Replace("{[", "[");
                prmMULTI_FILE.Replace("]}", "]");
                prmMULTI_FILE.Replace("'", "\"");

                try
                {
                    aParams[0] = helper.BuildParameter("prmTEN_CV", prmTEN_CV, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[1] = helper.BuildParameter("prmNGUON_PHATSINH", prmNGUON_PHATSINH, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[2] = helper.BuildParameter("prmNGUOI_GIAO", prmNGUOI_GIAO, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[3] = helper.BuildParameter("prmNOIDUNG", prmNOIDUNG, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[4] = helper.BuildParameter("prmDONVI_CHUTRI", prmDONVI_CHUTRI, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[5] = helper.BuildParameter("prmNGUOI_CHUTRI", prmNGUOI_CHUTRI, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[6] = helper.BuildParameter("prmMA_CONGVIEC_CHA", prmMA_CONGVIEC_CHA, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[7] = helper.BuildParameter("prmMA_DUAN", prmMA_DUAN, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[8] = helper.BuildParameter("prmLOAI_CV", prmLOAI_CV, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[9] = helper.BuildParameter("prmNGAY_BATDAU", prmNGAY_BATDAU, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[10] = helper.BuildParameter("prmNGAY_KETTHUC", prmNGAY_KETTHUC, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[11] = helper.BuildParameter("prmNGUOI_PHOIHOP", prmNGUOI_PHOIHOP, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[12] = helper.BuildParameter("prmDONVI_PHOIHOP", prmDONVI_PHOIHOP, OracleDbType.Varchar2, ParameterDirection.Input);

                    aParams[13] = helper.BuildParameter("prmNGUOI_CAPNHAT", prmNGUOI_CAPNHAT, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[14] = helper.BuildParameter("prmTINH_CHAT", prmTINH_CHAT, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[15] = helper.BuildParameter("prmNGUOI_GIAMSAT", prmNGUOI_GIAMSAT, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[16] = helper.BuildParameter("prmMULTI_FILE", prmMULTI_FILE, OracleDbType.Clob, ParameterDirection.Input);

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

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("congviec_up_upload"), HttpPost]
        public IHttpActionResult CONGVIEC_UP_UPLOAD()
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
            var prmTEN_CV = Httpfroms.Get("prmTEN_CV");
            var prmNGUON_PHATSINH = Httpfroms.Get("prmNGUON_PHATSINH");
            var prmNGUOI_GIAO = Httpfroms.Get("prmNGUOI_GIAO");
            var prmNOIDUNG = Httpfroms.Get("prmNOIDUNG");
            var prmDONVI_CHUTRI = Httpfroms.Get("prmDONVI_CHUTRI");
            var prmNGUOI_CHUTRI = Httpfroms.Get("prmNGUOI_CHUTRI");
            var prmMA_CONGVIEC_CHA = Httpfroms.Get("prmMA_CONGVIEC_CHA");
            var prmMA_DUAN = Httpfroms.Get("prmMA_DUAN");
            var prmLOAI_CV = "3";
            var prmNGAY_BATDAU = Httpfroms.Get("prmNGAY_BATDAU");
            var prmNGAY_KETTHUC = Httpfroms.Get("prmNGAY_KETTHUC");
            var prmNGUOI_PHOIHOP = Httpfroms.Get("prmNGUOI_PHOIHOP");
            var prmDONVI_PHOIHOP = Httpfroms.Get("prmDONVI_PHOIHOP");
            var prmNGUOI_CAPNHAT = Httpfroms.Get("prmNGUOI_CAPNHAT");
            var prmTINH_CHAT = Httpfroms.Get("prmTINH_CHAT");
            var prmNGUOI_GIAMSAT = Httpfroms.Get("prmNGUOI_GIAMSAT");
            var prmMA_CV = Httpfroms.Get("prmMA_CV");


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

            string query_str = "CONGVIEC_UP_UPLOAD";
            object[] aParams = new object[18];
            string prmMULTI_FILE = str_prmJsonDatas.ToString();
            prmMULTI_FILE.Replace("{[", "[");
            prmMULTI_FILE.Replace("]}", "]");
            prmMULTI_FILE.Replace("'", "\"");

            try
            {
                aParams[0] = helper.BuildParameter("prmTEN_CV", prmTEN_CV, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmNGUON_PHATSINH", prmNGUON_PHATSINH, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[2] = helper.BuildParameter("prmNGUOI_GIAO", prmNGUOI_GIAO, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[3] = helper.BuildParameter("prmNOIDUNG", prmNOIDUNG, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[4] = helper.BuildParameter("prmDONVI_CHUTRI", prmDONVI_CHUTRI, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[5] = helper.BuildParameter("prmNGUOI_CHUTRI", prmNGUOI_CHUTRI, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[6] = helper.BuildParameter("prmMA_CONGVIEC_CHA", prmMA_CONGVIEC_CHA, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[7] = helper.BuildParameter("prmMA_DUAN", prmMA_DUAN, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[8] = helper.BuildParameter("prmLOAI_CV", prmLOAI_CV, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[9] = helper.BuildParameter("prmNGAY_BATDAU", prmNGAY_BATDAU, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[10] = helper.BuildParameter("prmNGAY_KETTHUC", prmNGAY_KETTHUC, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[11] = helper.BuildParameter("prmNGUOI_PHOIHOP", prmNGUOI_PHOIHOP, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[12] = helper.BuildParameter("prmDONVI_PHOIHOP", prmDONVI_PHOIHOP, OracleDbType.Varchar2, ParameterDirection.Input);

                aParams[13] = helper.BuildParameter("prmNGUOI_CAPNHAT", prmNGUOI_CAPNHAT, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[14] = helper.BuildParameter("prmTINH_CHAT", prmTINH_CHAT, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[15] = helper.BuildParameter("prmNGUOI_GIAMSAT", prmNGUOI_GIAMSAT, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[16] = helper.BuildParameter("prmMULTI_FILE", prmMULTI_FILE, OracleDbType.Clob, ParameterDirection.Input);
                aParams[17] = helper.BuildParameter("prmMA_CV", prmMA_CV, OracleDbType.Varchar2, ParameterDirection.Input);

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
        /// <b>Mục đích::</b> Chỉnh sửa công việc <br />
        /// <b>Tham số URI:</b> prmJsonData. JSON <br />
        ///<b>Trả về:</b> Datatable <br />
        /// </summary>      
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("update"), HttpPost]
        public IHttpActionResult CONGVIEC_UP([FromBody] dynamic obj)
        {
            string query_str = "CONGVIEC_UP";

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
        /// View công việc theo mã
        /// </summary>
        [Route("getbyma"), HttpGet]
        public IHttpActionResult CONGVIEC_BYMA_CV(string prmMA_CONGVIEC)
        {
            string query_str = "CONGVIEC_BYMA_CV";
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
        /// View công việc theo mã
        /// </summary>
        [Route("getcongviecgiao"), HttpGet]
        public IHttpActionResult CONGVIEC_CONGVIECGIAO(string prmMA_NV, string prmUsername)
        {
            string query_str = "CONGVIEC_PHATSINH_GIAOVIEC";
            object[] aParams = new object[3];
            try
            {
                aParams[0] = helper.BuildParameter("prmMA_NV", prmMA_NV, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmUsername", prmUsername, OracleDbType.Varchar2, ParameterDirection.Input);

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
        /// View công việc theo mã
        /// </summary>
        [Route("getcongviecchutri"), HttpGet]
        public IHttpActionResult CONGVIEC_CONGVIECCHUTRI(string prmMA_NV)
        {
            string query_str = "CONGVIEC_PHATSINH_CHUTRI";
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
        /// View công việc theo mã
        /// </summary>
        [Route("getcongviecphoihop"), HttpGet]
        public IHttpActionResult CONGVIEC_CONGVIECPHOIHOP(string prmMA_NV)
        {
            string query_str = "CONGVIEC_PHATSINH_PHOIHOP";
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
        /// JOB nhắc việc
        /// </summary>
        [Route("job_nhacviec"), HttpGet]
        public IHttpActionResult JOB_NHACVIEC()
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            string query_str = "JOB_NHACVIEC";
            object[] aParams = new object[4];
            try
            {
                OracleParameter resultParam_trongngay = new OracleParameter("CV_TRONGNGAY", OracleDbType.RefCursor);
                resultParam_trongngay.Direction = ParameterDirection.Output;  
                aParams[0] = resultParam_trongngay;

                OracleParameter resultParam_quahan = new OracleParameter("CV_QUAHAN", OracleDbType.RefCursor);
                resultParam_quahan.Direction = ParameterDirection.Output;     
                aParams[1] = resultParam_quahan;

                OracleParameter resultParam_con3ngay = new OracleParameter("CV_CON3NGAY", OracleDbType.RefCursor);
                resultParam_con3ngay.Direction = ParameterDirection.Output; 
                aParams[2] = resultParam_con3ngay;

                OracleParameter resultParam_con7ngay = new OracleParameter("CV_CON7NGAY", OracleDbType.RefCursor);
                resultParam_con7ngay.Direction = ParameterDirection.Output; 
                aParams[3] = resultParam_con7ngay;

                DataSet kq = helper.ExecuteQueryStoreProcedure_List(query_str, aParams);
                //Nhắc nhở công việc trong ngày
                var cv_trongngay = kq.Tables["table"];
                if (cv_trongngay.Rows.Count > 0)
                {         
                    for (int i = 0; i < cv_trongngay.Rows.Count; i++)
                    {
                        if(cv_trongngay.Rows[i]["id_user_tele"].ToString() != "")
                        {
                            var noidung_cv_trongngay = "Công việc cần thực hiện trong ngày: " + cv_trongngay.Rows[i]["ten_cv"].ToString() + " http://10.47.135.141:8686/Congviec/Giaoviec/" + cv_trongngay.Rows[i]["ma_congviec" ]+ " (Ngày kết thức: "+ cv_trongngay.Rows[i]["ngay_ketthuc"]+")";
                            string url_send = "https://api.telegram.org/bot5507340091:AAF01yPWs2GcD11_fcaEBdKdPWDtXIw-9zc/sendMessage?chat_id=" + cv_trongngay.Rows[i]["id_user_tele"] + "&text=" + noidung_cv_trongngay;
                            using (var webClient = new System.Net.WebClient())
                            {
                                var json_send = webClient.DownloadString(url_send);
                            }
                        }  
                    }
                }
                //Công việc quá hạn
                var cv_quahan = kq.Tables["table1"];
                if (cv_quahan.Rows.Count > 0)
                {
                    for (int i = 0; i < cv_quahan.Rows.Count; i++)
                    {
                        if (cv_quahan.Rows[i]["id_user_tele"].ToString() != "")
                        {
                            var noidung_cv_quahan = "Công việc quá hạn: " + cv_quahan.Rows[i]["ten_cv"].ToString() + " http://10.47.135.141:8686/Congviec/Giaoviec/" + cv_quahan.Rows[i]["ma_congviec"] + " (Ngày kết thúc: " + cv_quahan.Rows[i]["ngay_ketthuc"]+")";
                            string url_send = "https://api.telegram.org/bot5507340091:AAF01yPWs2GcD11_fcaEBdKdPWDtXIw-9zc/sendMessage?chat_id=" + cv_quahan.Rows[i]["id_user_tele"] + "&text=" + noidung_cv_quahan;
                            using (var webClient = new System.Net.WebClient())
                            {
                                var json_send = webClient.DownloadString(url_send);
                            }
                        }
                    }
                }
                var cv_con3ngay = kq.Tables["table2"];
                if (cv_con3ngay.Rows.Count > 0)
                {
                    for (int i = 0; i < cv_con3ngay.Rows.Count; i++)
                    {
                        if (cv_con3ngay.Rows[i]["id_user_tele"].ToString() != "")
                        {
                            var noidung_cv_con3ngay = "Công việc còn 3 ngày nữa quá hạn: " + cv_con3ngay.Rows[i]["ten_cv"].ToString() + " http://10.47.135.141:8686/Congviec/Giaoviec/" + cv_con3ngay.Rows[i]["ma_congviec"] + " (Ngày kết thúc: " + cv_con3ngay.Rows[i]["ngay_ketthuc"] + ")";
                            string url_send = "https://api.telegram.org/bot5507340091:AAF01yPWs2GcD11_fcaEBdKdPWDtXIw-9zc/sendMessage?chat_id=" + cv_con3ngay.Rows[i]["id_user_tele"] + "&text=" + noidung_cv_con3ngay;
                            using (var webClient = new System.Net.WebClient())
                            {
                                var json_send = webClient.DownloadString(url_send);
                            }
                        }
                    }
                }
                var cv_con7ngay = kq.Tables["table3"];
                if (cv_con7ngay.Rows.Count > 0)
                {
                    for (int i = 0; i < cv_con7ngay.Rows.Count; i++)
                    {
                        if (cv_con7ngay.Rows[i]["id_user_tele"].ToString() != "")
                        {
                            var noidung_cv_con7ngay = "Công việc còn 7 ngày nữa quá hạn: " + cv_con7ngay.Rows[i]["ten_cv"].ToString() + " http://10.47.135.141:8686/Congviec/Giaoviec/" + cv_con7ngay.Rows[i]["ma_congviec"] + " (Ngày kết thúc: " + cv_con7ngay.Rows[i]["ngay_ketthuc"] + ")";
                            string url_send = "https://api.telegram.org/bot5507340091:AAF01yPWs2GcD11_fcaEBdKdPWDtXIw-9zc/sendMessage?chat_id=" + cv_con7ngay.Rows[i]["id_user_tele"] + "&text=" + noidung_cv_con7ngay;
                            using (var webClient = new System.Net.WebClient())
                            {
                                var json_send = webClient.DownloadString(url_send);
                            }
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
        /// View công việc theo mã
        /// </summary>
        [Route("getcongviecgiamsat"), HttpGet]
        public IHttpActionResult CONGVIEC_CONGVIECGIAMSAT(string prmMA_NV)
        {
            string query_str = "CONGVIEC_PHATSINH_GIAMSAT";
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
        /// View công việc theo mã
        /// </summary>
        [Route("getcongviecchitiet"), HttpGet]
        public IHttpActionResult CONGVIEC_CONGVIECCHITIET(string prmMA_CONGVIEC)
        {
            string query_str = "CONGVIEC_PHATSINH_CHITIET";
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
        /// View công việc theo mã
        /// </summary>
        [Route("getfilecongviec"), HttpGet]
        public IHttpActionResult CONGVIEC_GETFILE(string prmMA_CONGVIEC)
        {
            string query_str = "FILE_CONGVIEC_BYMA";
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
        /// View công việc theo mã
        /// </summary>
        [Route("getfilecongviec_all"), HttpGet]
        public IHttpActionResult CONGVIEC_GETFILE_ALL(string prmMA_CONGVIEC, string prmMA_PHATSINH)
        {
            string query_str = "CONGVIEC_FILE_BYMA";
            object[] aParams = new object[3];
            try
            {
                aParams[0] = helper.BuildParameter("prmMA_CONGVIEC", prmMA_CONGVIEC, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmMA_PHATSINH", prmMA_PHATSINH, OracleDbType.Varchar2, ParameterDirection.Input);


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
        /// Xóa thông báo.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>        
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("delete"), HttpPost]
        public IHttpActionResult CONGVIEC_DEL(string prmMA_CONGVIEC)
        {
            string query_str = "CONGVIEC_DEL";

            object[] aParams = new object[1];

            try
            {
                aParams[0] = helper.BuildParameter("prmMA_CONGVIEC", prmMA_CONGVIEC, OracleDbType.Varchar2, ParameterDirection.Input);
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
        /// View công việc theo mã
        /// </summary>
        [Route("getcongviecnguoiphoihop"), HttpGet]
        public IHttpActionResult CONGVIEC_GETNGUOIPHOIHOP(string prmMA_CONGVIEC)
        {
            string query_str = "CONGVIEC_PHATSINH_NGUOIPHOIHOP";
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
        /// View công việc theo mã
        /// </summary>
        [Route("getcongviecdonviphoihop"), HttpGet]
        public IHttpActionResult CONGVIEC_GETDONVIPHOIHOP(string prmMA_CONGVIEC)
        {
            string query_str = "CONGVIEC_PHATSINH_DONVIPHOIHOP";
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
        /// View công việc theo mã
        /// </summary>
        [Route("getcongviec_bydate"), HttpGet]
        public IHttpActionResult CONGVIEC_GETBYDATE(string prmMA_NV, string prmDK, string prmNGAYBATDAU, string prmNGAYKETTHUC)
        {
            string query_str = "CONGVIEC_PHATSINH_TIMKIEM_BYDATE";
            object[] aParams = new object[5];
            try
            {
                aParams[0] = helper.BuildParameter("prmMA_NV", prmMA_NV, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmDK", prmDK, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[2] = helper.BuildParameter("prmNGAYBATDAU", prmNGAYBATDAU, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[3] = helper.BuildParameter("prmNGAYKETTHUC", prmNGAYKETTHUC, OracleDbType.Varchar2, ParameterDirection.Input);

                OracleParameter resultParam = new OracleParameter("results", OracleDbType.RefCursor);
                resultParam.Direction = ParameterDirection.Output;

                aParams[4] = resultParam;

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
        /// <b>Mục đích::</b> Chỉnh sửa thông tin đơn vị <br />
        /// <b>Tham số URI:</b> prmJsonData. JSON <br />
        ///<b>Trả về:</b> Datatable <br />
        /// </summary>      
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("up_trangthai"), HttpPost]
        public IHttpActionResult CONGVIEC_UP_TRANGTHAI(string prmMA_CONGVIEC, string prmTRANGTHAI, string prmTYLE)
        {
            string query_str = "CONGVIEC_UP_TRANGTHAI";

            object[] aParams = new object[3];

            try
            {
                aParams[0] = helper.BuildParameter("prmMA_CONGVIEC", prmMA_CONGVIEC, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmTRANGTHAI", prmTRANGTHAI, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[2] = helper.BuildParameter("prmTYLE", prmTYLE, OracleDbType.Varchar2, ParameterDirection.Input);
                String kq = helper.ExecuteNonQuery(query_str, aParams);

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, kq));
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, err));
            }
        }
        ///<summary>
        ///<b>Mục đích:</b>Lấy danh sách tất cả các công việc. <br />
        ///<b>Tham số URI:</b> Không có. <br />
        ///<b>Trả về:</b> Datatable <br />
        ///</summary>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("getAll"), HttpGet]
        public DataTable GetAll()
        {
            string qstr = "CONGVIEC_VIEW_ALL";
            object[] arr_params = new object[1];

            OracleParameter param1 = new OracleParameter("result", OracleDbType.RefCursor, ParameterDirection.Output);
            arr_params[0] = param1;
            return helper.ExecuteQueryStoreProcedure(qstr, arr_params);
        }
        /// <summary>
        /// Get công việc theo trạng thái
        /// </summary>
        [Route("getbytrangthai"), HttpGet]
        public IHttpActionResult CONGVIEC_BYTRANGTHAI(string prmMA_CONGVIEC, int prmTRANGTHAI)
        {
            string query_str = "CONGVIEC_BYTRANGTHAI";
            object[] aParams = new object[3];
            try
            {
                aParams[0] = helper.BuildParameter("prmMA_CONGVIEC", prmMA_CONGVIEC, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmTRANGTHAI", prmTRANGTHAI, OracleDbType.Int32, ParameterDirection.Input);

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
        /// Cập nhật trạng thái công việc
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>        
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("change_trangthai"), HttpPost]
        public IHttpActionResult CONGVIEC_CHANGE_TRANGTHAI([FromBody] dynamic obj)
        {
            string query_str = "CONGVIEC_CHANGE_TRANGTHAI";

            object[] aParams = new object[3];

            try
            {
                aParams[0] = helper.BuildParameter("prmMA_CONGVIEC", obj.MA_CONGVIEC, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmTRANGTHAI", obj.TRANGTHAI, OracleDbType.Int32, ParameterDirection.Input);
                aParams[2] = helper.BuildParameter("prmNGUOI_CAPNHAT", obj.NGUOI_CAPNHAT, OracleDbType.Varchar2, ParameterDirection.Input);
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
        /// Get công việc theo người giao
        /// </summary>
        [Route("getbynguoigiao"), HttpGet]
        public IHttpActionResult CONGVIEC_BYNGUOIGIAO(string prmNGUOI_GIAO)
        {
            string query_str = "CONGVIEC_BYNGUOIGIAO";
            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmNGUOI_GIAO", prmNGUOI_GIAO, OracleDbType.Varchar2, ParameterDirection.Input);

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
        /// Get công việc theo người nhận việc
        /// </summary>
        [Route("getbynguoinhan"), HttpGet]
        public IHttpActionResult CONGVIEC_BYNGUOINHAN(string prmNGUOI_NHAN)
        {
            string query_str = "CONGVIEC_BYNGUOINHAN";
            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmNGUOI_NHAN", prmNGUOI_NHAN, OracleDbType.Varchar2, ParameterDirection.Input);

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
