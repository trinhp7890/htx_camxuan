using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Configuration;
using API_TPL.DAL;
using System.Web;
using System.IO;
using Newtonsoft.Json;
using System.Web.Helpers;
using System.Text;

namespace API_TPL.Controllers.UPLOAD
{
    //[Authorize]
    [RoutePrefix("api/upload")]
    public class UploadController : ApiController
    {
        static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["QLCV"].ToString();
        DBHelper helper = new DBHelper(connString);
        ApiHelpers api_helper = new ApiHelpers();
        [Route("uploadmulti"), HttpPost]
        public IHttpActionResult upload_anh(string prmMA_DOITUONG, int prmLOAI, string prmGHICHU, string prmNGUOI_CAPNHAT, string prmMULTI_FILE)
        {

            string kq_all = "";
            string kq = "";
            int kq_total = 0;
            int kq_error = 0;
            //string kq_max = "";
            int kq_max = 0;
            int kq_suscess = 0;
            string _uploadPath = ConfigurationManager.AppSettings["upload_path"].ToString().Trim();
            string _typePath = ConfigurationManager.AppSettings["image_folder"].ToString().Trim();
            string workingFolder = HttpContext.Current.Server.MapPath("~" + _uploadPath + _typePath + "/" + prmLOAI + "/" + prmMA_DOITUONG + "/");
            if (!Directory.Exists(workingFolder))
            {
                Directory.CreateDirectory(workingFolder); //Create directory if it doesn't exist
            }

            string query_str = "UPLOAD_FILE_INSERT";
            object[] aParams = new object[6];
            aParams[1] = helper.BuildParameter("prmMA_DOITUONG", prmMA_DOITUONG, OracleDbType.Varchar2, ParameterDirection.Input);
            aParams[2] = helper.BuildParameter("prmDUONGDAN_FILE", _typePath + "/" + prmLOAI + "/" + prmMA_DOITUONG + "/", OracleDbType.Varchar2, ParameterDirection.Input);
            aParams[3] = helper.BuildParameter("prmGHICHU", prmGHICHU, OracleDbType.Varchar2, ParameterDirection.Input);
            aParams[4] = helper.BuildParameter("prmLOAI", prmLOAI, OracleDbType.Int32, ParameterDirection.Input);
            aParams[5] = helper.BuildParameter("prmNGUOI_CAPNHAT", prmNGUOI_CAPNHAT, OracleDbType.Varchar2, ParameterDirection.Input);
            aParams[6] = helper.BuildParameter("prmMULTI_FILE", prmMULTI_FILE, OracleDbType.Clob, ParameterDirection.Input);

            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                string file_name = "";
                foreach (string file in httpRequest.Files)
                {
                    int MaxContentLength = 20 * 1024 * 1024;
                    kq_total = kq_total + 1;
                    var postedFile = httpRequest.Files[file];


                    if (postedFile.ContentLength > MaxContentLength)
                    {
                        kq_max = kq_max + 1;
                    }
                    else
                    {
                        //kq_max = kq_max + ";" + postedFile.ContentLength.ToString();
                        file_name = DateTime.Now.ToString("ddMMyyyy HHmm") + "_" + postedFile.FileName;
                        file_name = file_name.Replace(" ", "_");
                        var filePath = workingFolder + file_name;

                        //postedFile.SaveAs(filePath);
                        //docfiles.Add(filePath);
                        WebImage img = new WebImage(postedFile.InputStream);
                        float ratio = (float)img.Width / (float)img.Height;
                        float height1 = 2000 / ratio;
                        if (img.Width > 2000)
                            img.Resize(2000, (int)height1);
                        img.Save(filePath);

                        aParams[0] = helper.BuildParameter("prmTENFILE", file_name, OracleDbType.Varchar2, ParameterDirection.Input);
                        try
                        {
                            kq = helper.ExecuteNonQuery(query_str, aParams) + ";";
                            kq_suscess = kq_suscess + 1;
                        }
                        catch
                        {
                            kq_error = kq_error + 1;
                        }
                        kq_all = "Tổng số file :" + kq_total.ToString() + "; thành công :" + kq_suscess.ToString() + "; Lỗi : File > 5MB :" + kq_max.ToString() + ", Khác :" + kq_error.ToString();
                    }

                }
                result = Request.CreateResponse(HttpStatusCode.Created, kq_all);
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return ResponseMessage(result);
        }

        [Route("upload_anhnoidung"), HttpPost]
        public IHttpActionResult upload_anhnoidung()
        {

            string _uploadPath = ConfigurationManager.AppSettings["upload_path"].ToString().Trim();
            string _typePath = ConfigurationManager.AppSettings["image_folder"].ToString().Trim();
            string workingFolder = HttpContext.Current.Server.MapPath("~" + _uploadPath   + "anh/");
            if (!Directory.Exists(workingFolder))
            {
                Directory.CreateDirectory(workingFolder); //Create directory if it doesn't exist
            }

            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;

            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                string file_name = "";
                foreach (string file in httpRequest.Files)
                {
                    int MaxContentLength = 20 * 1024 * 1024;
                    var postedFile = httpRequest.Files[file];


                    
                        //kq_max = kq_max + ";" + postedFile.ContentLength.ToString();
                        file_name = DateTime.Now.ToString("ddMMyyyy HHmm") + "_" + postedFile.FileName;
                        file_name = file_name.Replace(" ", "_");
                        var filePath = workingFolder + file_name;

                        //postedFile.SaveAs(filePath);
                        //docfiles.Add(filePath);
                        WebImage img = new WebImage(postedFile.InputStream);
                        float ratio = (float)img.Width / (float)img.Height;
                        float height1 = 2000 / ratio;
                        if (img.Width > 2000)
                            img.Resize(2000, (int)height1);
                        img.Save(filePath);

                    

                }
                result = Request.CreateResponse(HttpStatusCode.Created, "");
            }
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return ResponseMessage(result);
        }

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("thongbao_ins_upload"), HttpPost]
        public IHttpActionResult THONGBAO_INS_UPLOAD()
        {
            string timenow = DateTime.Now.ToString("ddMMyyyy_HHmm");
            string _uploadPath = ConfigurationManager.AppSettings["upload_path"].ToString().Trim();
            string _typePath = ConfigurationManager.AppSettings["image_folder"].ToString().Trim();
            string _typevanban = ConfigurationManager.AppSettings["upload_folder"].ToString().Trim();
            string workingFolder = HttpContext.Current.Server.MapPath("~" + _uploadPath + _typePath);
            string workingFolderfile = HttpContext.Current.Server.MapPath("~" + _uploadPath + _typevanban + "/" + timenow);

            string workingFoldersave = _uploadPath + _typePath;
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
            var prmTIEUDE = Httpfroms.Get("prmTIEUDE");
            var prmNOIDUNG = Httpfroms.Get("prmNOIDUNG");
            var prmTENFILE ="";
            var prmMOTA = Httpfroms.Get("prmMOTA");
            var prmLOAI = 1;
            var prmNGUOI_CAPNHAT = Httpfroms.Get("prmNGUOI_CAPNHAT");
            var prmGHICHU = Httpfroms.Get("prmGHICHU");
            var  ISIMG = Httpfroms.Get("ISIMG");            
            var prmDUONGDAN_FILE = "";

            if (httpRequest.Files.Count > 0)
            {                          
                string filePathsave = "";
                string filename = "";
                StringBuilder str_prmJsonDatas = new StringBuilder();
                str_prmJsonDatas.Append("[");
                StringBuilder str_ojb = new StringBuilder();
                string loaifile = "1";                
                for (int i = 0; i < httpRequest.Files.Count; i++)
                {
                    
                                    
                    if (ISIMG == "1" && i == 0)
                    {
                        filename = timenow + httpRequest.Files[i].FileName;
                        prmTENFILE = filename;
                        prmDUONGDAN_FILE = workingFoldersave + "/";
                    }
                    else 
                    {
                        filename = httpRequest.Files[i].FileName;
                        filePathsave = workingFolderfilesave;
                        str_ojb = new StringBuilder();
                        str_ojb.Append("'TEN_FILE':");
                        str_ojb.Append("'" + filename + "',");

                        str_ojb.Append("'DUONGDAN_FILE':");
                        str_ojb.Append("'" + filePathsave + "/',");

                        str_ojb.Append("'GHICHU':");
                        str_ojb.Append("'Upload file insert thông báo'");
                       
                        str_prmJsonDatas.Append("{" + str_ojb.ToString() + "},");
                    }
                                      
                    //httpRequest.Files[i].SaveAs(filePathsave);
                }
                if(httpRequest.Files.Count == 0 || (httpRequest.Files.Count == 1 && ISIMG == "1"))
                {
                    str_prmJsonDatas.Append("]");
                }
                else
                {
                    str_prmJsonDatas.Remove(str_prmJsonDatas.Length - 1, 1);
                    str_prmJsonDatas.Append("]");
                }
                
                //Object jsonObject_ = JsonConvert.DeserializeObject(str_prmJsonDatas.ToString());

                string query_str = "THONGBAO_INS_UPLOAD";
                object[] aParams = new object[9];
                string prmMULTI_FILE = str_prmJsonDatas.ToString();
                prmMULTI_FILE.Replace("{[", "[");
                prmMULTI_FILE.Replace("]}", "]");
                prmMULTI_FILE.Replace("'", "\"");

                try
                {
                    aParams[0] = helper.BuildParameter("prmTIEUDE", prmTIEUDE, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[1] = helper.BuildParameter("prmNOIDUNG", prmNOIDUNG, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[2] = helper.BuildParameter("prmTENFILE", prmTENFILE, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[3] = helper.BuildParameter("prmDUONGDAN_FILE", prmDUONGDAN_FILE, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[4] = helper.BuildParameter("prmGHICHU", prmGHICHU, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[5] = helper.BuildParameter("prmLOAI", prmLOAI, OracleDbType.Int16, ParameterDirection.Input);
                    aParams[6] = helper.BuildParameter("prmMOTA", prmMOTA, OracleDbType.Varchar2, ParameterDirection.Input);                    
                    aParams[7] = helper.BuildParameter("prmNGUOI_CAPNHAT", prmNGUOI_CAPNHAT, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[8] = helper.BuildParameter("prmMULTI_FILE", prmMULTI_FILE, OracleDbType.Clob, ParameterDirection.Input);

                    String kq = helper.ExecuteNonQuery(query_str, aParams);
                    if (kq == "OK")
                    {
                        for (int i = 0; i < httpRequest.Files.Count; i++)
                        {
                            

                            if (ISIMG == "1" && i == 0)
                            {
                                filename = timenow + httpRequest.Files[i].FileName;
                                filePathsave = workingFolder + "\\" + filename;
                            }
                            else
                            {
                                filename = httpRequest.Files[i].FileName;
                                filePathsave = workingFolderfile + "\\" + filename;
                            }                           
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
            else
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Chưa có file để upload"));
            }

        }

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("thongbao_upd_upload"), HttpPost]
        public IHttpActionResult THONGBAO_UPD_UPLOAD()
        {

            string timenow = DateTime.Now.ToString("ddMMyyyy_HHmm");
            string _uploadPath = ConfigurationManager.AppSettings["upload_path"].ToString().Trim();
            string _typePath = ConfigurationManager.AppSettings["image_folder"].ToString().Trim();
            string _typevanban = ConfigurationManager.AppSettings["upload_folder"].ToString().Trim();
            string workingFolder = HttpContext.Current.Server.MapPath("~" + _uploadPath + _typePath);
            string workingFolderfile = HttpContext.Current.Server.MapPath("~" + _uploadPath + _typevanban + "/" + timenow);

            string workingFoldersave = _uploadPath + _typePath;
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
            var prmMA_TB_IN = Httpfroms.Get("prmMA_TB_IN");
            var prmTIEUDE = Httpfroms.Get("prmTIEUDE");
            var prmNOIDUNG = Httpfroms.Get("prmNOIDUNG");
            var prmTENFILE = "";
            var prmMOTA = Httpfroms.Get("prmMOTA");
            var prmLOAI = 1;
            var prmNGUOI_CAPNHAT = Httpfroms.Get("prmNGUOI_CAPNHAT");
            var prmGHICHU = Httpfroms.Get("prmGHICHU");
            var ISIMG = Httpfroms.Get("ISIMG");
            var prmDUONGDAN_FILE = "";

                string filePathsave = "";
                string filename = "";
                StringBuilder str_prmJsonDatas = new StringBuilder();
                str_prmJsonDatas.Append("[");
                StringBuilder str_ojb = new StringBuilder();
                string loaifile = "1";
              
                for (int i = 0; i < httpRequest.Files.Count; i++)
                {
                    

                    if (ISIMG == "1" && i == 0)
                    {
                        filename = timenow + httpRequest.Files[i].FileName;
                        prmTENFILE = filename;
                        prmDUONGDAN_FILE = workingFoldersave + "/";
                    }
                    else
                    {
                        filename = httpRequest.Files[i].FileName;
                        filePathsave = workingFolderfilesave;
                        str_ojb = new StringBuilder();
                        str_ojb.Append("'TEN_FILE':");
                        str_ojb.Append("'" + filename + "',");

                        str_ojb.Append("'DUONGDAN_FILE':");
                        str_ojb.Append("'" + filePathsave + "/',");

                        str_ojb.Append("'GHICHU':");
                        str_ojb.Append("'Upload file insert thông báo'");

                        str_prmJsonDatas.Append("{" + str_ojb.ToString() + "},");
                    }

                    //httpRequest.Files[i].SaveAs(filePathsave);
                }
                if (httpRequest.Files.Count == 0 || (httpRequest.Files.Count == 1 && ISIMG == "1"))
                {
                    str_prmJsonDatas.Append("]");
                }
                else
                {
                    str_prmJsonDatas.Remove(str_prmJsonDatas.Length - 1, 1);
                    str_prmJsonDatas.Append("]");
                }

                //Object jsonObject_ = JsonConvert.DeserializeObject(str_prmJsonDatas.ToString());

                string query_str = "THONGBAO_UPD_UPLOAD";
                object[] aParams = new object[10];
                string prmMULTI_FILE = str_prmJsonDatas.ToString();
                prmMULTI_FILE.Replace("{[", "[");
                prmMULTI_FILE.Replace("]}", "]");
                prmMULTI_FILE.Replace("'", "\"");

                try
                {
                    aParams[0] = helper.BuildParameter("prmMA_TB_IN", prmMA_TB_IN, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[1] = helper.BuildParameter("prmTIEUDE", prmTIEUDE, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[2] = helper.BuildParameter("prmNOIDUNG", prmNOIDUNG, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[3] = helper.BuildParameter("prmTENFILE", prmTENFILE, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[4] = helper.BuildParameter("prmDUONGDAN_FILE", prmDUONGDAN_FILE, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[5] = helper.BuildParameter("prmGHICHU", prmGHICHU, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[6] = helper.BuildParameter("prmLOAI", prmLOAI, OracleDbType.Int16, ParameterDirection.Input);
                    aParams[7] = helper.BuildParameter("prmMOTA", prmMOTA, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[8] = helper.BuildParameter("prmNGUOI_CAPNHAT", prmNGUOI_CAPNHAT, OracleDbType.Varchar2, ParameterDirection.Input);
                    aParams[9] = helper.BuildParameter("prmMULTI_FILE", prmMULTI_FILE, OracleDbType.Clob, ParameterDirection.Input);

                    String kq = helper.ExecuteNonQuery(query_str, aParams);
                    if (kq == "OK")
                    {
                        for (int i = 0; i < httpRequest.Files.Count; i++)
                        {
                           

                            if (ISIMG == "1" && i == 0)
                            {
                               
                                filePathsave = workingFolder + "\\" + filename;
                            }
                            else
                            {
                                filename = httpRequest.Files[i].FileName;
                                filePathsave = workingFolderfile + "\\" + filename;
                            }
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
        [Route("nguoidung_upd_upload"), HttpPost]
        public IHttpActionResult NGUOIDUNG_UPD_UPLOAD()
        {

            string timenow = DateTime.Now.ToString("ddMMyyyy_HHmm");
            string _uploadPath = ConfigurationManager.AppSettings["upload_path"].ToString().Trim();
            string _typePath = ConfigurationManager.AppSettings["image_folder"].ToString().Trim();
            string _typevanban = ConfigurationManager.AppSettings["upload_folder"].ToString().Trim();
            string workingFolder = HttpContext.Current.Server.MapPath("~" + _uploadPath + _typePath + "/" + "NHANSU");
            string workingFolderfile = HttpContext.Current.Server.MapPath("~" + _uploadPath + _typevanban + "/" + timenow);

            string workingFoldersave = _uploadPath + _typePath + "/" + "NHANSU";
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
            var prmMA_NV = Httpfroms.Get("prmMA_NV");
            var prmTEN_NV = Httpfroms.Get("prmTEN_NV");
            var prmSODT = Httpfroms.Get("prmSODT");
            var prmTENFILE = "";
            var prmEMAIL = Httpfroms.Get("prmEMAIL");
            var prmLOAI = 3;
            var prmCHUCDANH = Httpfroms.Get("prmCHUCDANH");
            var prmMA_DV = Httpfroms.Get("prmMA_DV");
            var prmGHICHU = Httpfroms.Get("prmGHICHU");
            var prmNGUOI_CAPNHAT = Httpfroms.Get("prmNGUOI_CAPNHAT");
            var ISIMG = Httpfroms.Get("ISIMG");
            var prmDUONGDAN_FILE = "";

            var prmACTIVE = Httpfroms.Get("prmACTIVE");
            var prmMA_ND = Httpfroms.Get("prmMA_ND");
            var prmNGAYSINH = Httpfroms.Get("prmNGAYSINH");
            var prmGIOITINH = Httpfroms.Get("prmGIOITINH");
            var prmQUEQUAN = Httpfroms.Get("prmQUEQUAN");
            var prmSO_CMND = Httpfroms.Get("prmSO_CMND");
            var prmNGAYCAP_CMND = Httpfroms.Get("prmNGAYCAP_CMND");
            var prmNOICAP_CMDN = Httpfroms.Get("prmNOICAP_CMDN");
            
            var prmLOAIEDIT = Httpfroms.Get("prmLOAIEDIT");

            string filePathsave = "";
            string filename = "";

            for (int i = 0; i < httpRequest.Files.Count; i++)
            {
                if (ISIMG == "1" && i == 0)
                {
                    filename = timenow + httpRequest.Files[i].FileName;
                    prmTENFILE = filename;
                    prmDUONGDAN_FILE = workingFoldersave + "/";
                }              

            }
            
            string query_str = "NGUOIDUNG_UPD_UPLOAD";
            if (prmLOAIEDIT == "1") // insert người dùng
            {
                query_str = "NGUOIDUNG_INS_UPLOAD";
            }
            object[] aParams = new object[19];
            try
            {
                aParams[0] = helper.BuildParameter("prmMA_NV", prmMA_NV, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmTEN_NV", prmTEN_NV, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[2] = helper.BuildParameter("prmSODT", prmSODT, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[3] = helper.BuildParameter("prmEMAIL", prmEMAIL, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[4] = helper.BuildParameter("prmCHUCDANH", prmCHUCDANH, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[5] = helper.BuildParameter("prmMA_DV", prmMA_DV, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[6] = helper.BuildParameter("prmTENFILE", prmTENFILE, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[7] = helper.BuildParameter("prmDUONGDAN_FILE", prmDUONGDAN_FILE, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[8] = helper.BuildParameter("prmGHICHU", prmGHICHU, OracleDbType.Varchar2, ParameterDirection.Input);                
                aParams[9] = helper.BuildParameter("prmLOAI", prmLOAI, OracleDbType.Int16, ParameterDirection.Input);
                aParams[10] = helper.BuildParameter("prmNGUOI_CAPNHAT", prmNGUOI_CAPNHAT, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[11] = helper.BuildParameter("prmACTIVE ", prmACTIVE, OracleDbType.Int16, ParameterDirection.Input);
                aParams[12] = helper.BuildParameter("prmMA_ND ", prmMA_ND, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[13] = helper.BuildParameter("prmNGAYSINH ", prmNGAYSINH, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[14] = helper.BuildParameter("prmGIOITINH ", prmGIOITINH, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[15] = helper.BuildParameter("prmQUEQUAN ", prmQUEQUAN, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[16] = helper.BuildParameter("prmSO_CMND ", prmSO_CMND, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[17] = helper.BuildParameter("prmNGAYCAP_CMND ", prmNGAYCAP_CMND, OracleDbType.Varchar2, ParameterDirection.Input);
                aParams[18] = helper.BuildParameter("prmNOICAP_CMDN ", prmNOICAP_CMDN, OracleDbType.Varchar2, ParameterDirection.Input);                


                String kq = helper.ExecuteNonQuery(query_str, aParams);
                if (kq == "OK")
                {
                    for (int i = 0; i < httpRequest.Files.Count; i++)
                    {


                        if (ISIMG == "1" && i == 0)
                        {

                            filePathsave = workingFolder + "\\" + filename;
                        }                        
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
        [Route("delete_file"), HttpPost]
        public IHttpActionResult UPLOAD_FILE_DELETE(string ID_FILE, string prmDUONGDAN, string prmTenfile, string NGUOI_NHAP)
        {
            string query_str = "UPLOAD_FILE_DELETE";

            object[] aParams = new object[2];

            try
            {
                aParams[0] = helper.BuildParameter("prmID_FILE", ID_FILE, OracleDbType.Int32, ParameterDirection.Input);
                aParams[1] = helper.BuildParameter("prmNGUOI_NHAP", NGUOI_NHAP, OracleDbType.Varchar2, ParameterDirection.Input);
                string kq = helper.ExecuteNonQuery(query_str, aParams);
                var message1 = string.Format("Lỗi");
                var filename = prmTenfile;
                //var filePath = HttpContext.Current.Server.MapPath("~/Uploads/Images/" + prmMADONVI + "/" + prmTenfile);
                if (kq == "OK")
                {
                    var filePath = HttpContext.Current.Server.MapPath("~/Uploads" + prmDUONGDAN + prmTenfile);
                    System.IO.File.Delete(filePath);
                    message1 = string.Format("Xóa thành công!");
                }
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, message1));
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, err));
            }
        }       
    }
}
