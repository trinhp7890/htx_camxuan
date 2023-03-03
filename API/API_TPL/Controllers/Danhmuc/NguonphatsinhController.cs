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
    [RoutePrefix("api/dmnguonphatsinh")]
    public class NguonphatsinhController : ApiController
    {
        static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["QLCV"].ToString();
        DBHelper helper = new DBHelper(connString);

        /// <summary>
        /// <b>Mục đích::</b> Thêm mới nguồn phát sinh <br />
        /// <b>Tham số URI:</b> Httpfroms<br />
        ///<b>Trả về:</b> Datatable <br />
        /// </summary>    
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("nguonphatsinh_ins_upload"), HttpPost]
        public IHttpActionResult NGUONPHATSINH_INS_UPLOAD()
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
            var prmTEN = Httpfroms.Get("prmTEN");
            var prmMOTA = Httpfroms.Get("prmMOTA");
            var prmMA_DV = Httpfroms.Get("prmMA_DV");
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

            string query_str = "NGUONPHATSINH_INS_UPLOAD";
            object[] aParams = new object[5];
            string prmMULTI_FILE = str_prmJsonDatas.ToString();
            prmMULTI_FILE.Replace("{[", "[");
            prmMULTI_FILE.Replace("]}", "]");
            prmMULTI_FILE.Replace("'", "\"");

            try
            {
                aParams[0] = helper.BuildParameter("prmTEN", prmTEN, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmMOTA", prmMOTA, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[2] = helper.BuildParameter("prmMA_DV", prmMA_DV, OracleDbType.Varchar2, ParameterDirection.Input);
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

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("nguonphatsinh_up_upload"), HttpPost]
        public IHttpActionResult NGUONPHATSINH_UP_UPLOAD()
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
            var prmMA_NGUON_PS = Httpfroms.Get("prmMA_NGUON_PS");
            var prmTEN = Httpfroms.Get("prmTEN");
            var prmTRANG_THAI = Httpfroms.Get("prmTRANG_THAI");
            var prmMOTA = Httpfroms.Get("prmMOTA");
            var prmMA_DV = Httpfroms.Get("prmMA_DV");
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

            string query_str = "NGUONPHATSINH_UPD_UPLOAD";
            object[] aParams = new object[7];
            string prmMULTI_FILE = str_prmJsonDatas.ToString();
            prmMULTI_FILE.Replace("{[", "[");
            prmMULTI_FILE.Replace("]}", "]");
            prmMULTI_FILE.Replace("'", "\"");

            try
            {
                aParams[0] = helper.BuildParameter("prmMA_NGUON_PS", prmMA_NGUON_PS, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmTEN", prmTEN, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[2] = helper.BuildParameter("prmMOTA", prmMOTA, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[3] = helper.BuildParameter("prmMA_DV", prmMA_DV, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[4] = helper.BuildParameter("prmTRANG_THAI", prmTRANG_THAI, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[5] = helper.BuildParameter("prmNGUOI_CAPNHAT", prmNGUOI_CAPNHAT, OracleDbType.Varchar2, ParameterDirection.Input);

                aParams[6] = helper.BuildParameter("prmMULTI_FILE", prmMULTI_FILE, OracleDbType.Clob, ParameterDirection.Input);

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
        [Route("getnguonphatsinh"), HttpGet]
        public IHttpActionResult NGUONPHATSINH_GETALL(string prmDK, string prmMA_DONVI)
        {
            string query_str = "DM_NGUONPHATSINH_GETALL";
            object[] aParams = new object[3];
            try
            {
                aParams[0] = helper.BuildParameter("prmDK", prmDK, OracleDbType.Int32, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmMA_DONVI", prmMA_DONVI, OracleDbType.Varchar2, ParameterDirection.Input);


                OracleParameter resultParam = new OracleParameter("results", OracleDbType.RefCursor);
                resultParam.Direction = ParameterDirection.Output;

                aParams[2] = resultParam;

                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);

                List<Nguonphatsinh> listreturnnps = new List<Nguonphatsinh>();
                if (kq.Rows.Count > 0)
                {
                    Nguonphatsinh dv0 = new Nguonphatsinh();
                    List<object> file = new List<object>();
                    for (int i = 0; i < kq.Rows.Count; i++)
                    {
                        file = new List<object>();
                        dv0 = new Nguonphatsinh();
                        dv0.id = kq.Rows[i]["id_nguon_ps"].ToString();
                        dv0.ten = kq.Rows[i]["ten"].ToString();
                        dv0.noidung = kq.Rows[i]["mota"].ToString();
                        dv0.tendonvi = kq.Rows[i]["TEN_DV"].ToString();
                        dv0.madonvi = kq.Rows[i]["ma_dv"].ToString();
                        dv0.ma_nguonphatsinh = kq.Rows[i]["ma_nguon_ps"].ToString();
                        dv0.trangthai = kq.Rows[i]["TRANG_THAI"].ToString();
                        dv0.file = new List<object>();
                        //file.Add(kq.Rows[i]);                        
                        if (listreturnnps.Count == 0)
                        {
                            if (kq.Rows[i]["TEN_FILE"].ToString() != "")
                            {
                                dv0.file.Add(kq.Rows[i].ItemArray);
                            }
                            listreturnnps.Add(dv0);
                        }
                        else
                        {
                            int tk = 0;
                            for (int j = 0; j < listreturnnps.Count; j++)
                            {
                                if (listreturnnps[j].id == dv0.id)
                                {

                                    if (kq.Rows[i]["TEN_FILE"].ToString() != "")
                                    {
                                        listreturnnps[j].file.Add(kq.Rows[i].ItemArray);
                                    }
                                    tk = 1;
                                    break;
                                }
                            }
                            if (tk == 0)
                            {
                                if (kq.Rows[i]["TEN_FILE"].ToString() != "")
                                {
                                    dv0.file.Add(kq.Rows[i].ItemArray);
                                }
                                listreturnnps.Add(dv0);
                            }
                        }
                    }
                }
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, listreturnnps));
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
        public IHttpActionResult NGUONPHATSINH_DEL(string prmMA_NGUON_PS)
        {
            string query_str = "NGUONPHATSINH_DEL";

            object[] aParams = new object[1];

            try
            {
                aParams[0] = helper.BuildParameter("prmMA_NGUON_PS", prmMA_NGUON_PS, OracleDbType.Varchar2, ParameterDirection.Input);                
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
