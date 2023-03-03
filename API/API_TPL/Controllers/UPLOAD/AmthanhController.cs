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
using System.Web.Helpers;

namespace API_TPL.Controllers.UPLOAD
{
    [RoutePrefix("api/upload1")]
    public class AmthanhController : ApiController
    {
        static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["QLCV"].ToString();
        DBHelper helper = new DBHelper(connString);
        [Route("upload_anh"), HttpPost]
        public IHttpActionResult upload_anh()
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
            string workingFolder = HttpContext.Current.Server.MapPath("~" + _uploadPath + _typePath + "/" );
            if (!Directory.Exists(workingFolder))
            {
                Directory.CreateDirectory(workingFolder); //Create directory if it doesn't exist
            }

            string query_str = "BAODUONG_TBMVT.BAODUONG_UPLOAD_FILE";
            object[] aParams = new object[5];
           // aParams[0] = helper.BuildParameter("prmIDBAODUONG", prmIDBAODUONG, OracleDbType.Int32, ParameterDirection.Input);
          //  aParams[2] = helper.BuildParameter("prmDUONGDANFILE", _typePath + "/" + prmMADONVI + "/", OracleDbType.Varchar2, ParameterDirection.Input);
         //   aParams[3] = helper.BuildParameter("prmNSD", prmNSD, OracleDbType.Varchar2, ParameterDirection.Input);
         //   aParams[4] = helper.BuildParameter("prmTRANGTHAI", prmTRANGTHAI, OracleDbType.Int32, ParameterDirection.Input);

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

                       // aParams[1] = helper.BuildParameter("prmTENFILE", file_name, OracleDbType.Varchar2, ParameterDirection.Input);
                        try
                        {
                          //  kq = helper.ExecuteNonQuery(query_str, aParams) + ";";
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


        [Route("upload_amthanh"), HttpPost]
        public IHttpActionResult upload_amthanh()
        {

            string kq_all = "";
            string kq = "";
            int kq_total = 0;
            int kq_error = 0;
            //string kq_max = "";
            int kq_max = 0;
            int kq_suscess = 0;
            string _uploadPath = ConfigurationManager.AppSettings["upload_path"].ToString().Trim();
            string _typePath = ConfigurationManager.AppSettings["audio_folder"].ToString().Trim();
            string workingFolder = HttpContext.Current.Server.MapPath("~" + _uploadPath + _typePath + "/");
            if (!Directory.Exists(workingFolder))
            {
                Directory.CreateDirectory(workingFolder); //Create directory if it doesn't exist
            }

            string query_str = "UPLOAD_INSERT";
            object[] aParams = new object[5];
            // aParams[0] = helper.BuildParameter("prmIDBAODUONG", prmIDBAODUONG, OracleDbType.Int32, ParameterDirection.Input);
            //  aParams[2] = helper.BuildParameter("prmDUONGDANFILE", _typePath + "/" + prmMADONVI + "/", OracleDbType.Varchar2, ParameterDirection.Input);
            //   aParams[3] = helper.BuildParameter("prmNSD", prmNSD, OracleDbType.Varchar2, ParameterDirection.Input);
            //   aParams[4] = helper.BuildParameter("prmTRANGTHAI", prmTRANGTHAI, OracleDbType.Int32, ParameterDirection.Input);

            HttpResponseMessage result = null;
            var httpRequest = HttpContext.Current.Request;
            if (httpRequest.Files.Count > 0)
            {
                var docfiles = new List<string>();
                string file_name = "";
                foreach (string file in httpRequest.Files)
                {

                    var postedFile = httpRequest.Files[file];
                    file_name = DateTime.Now.ToString("ddMMyyyy HHmm") + "_" + postedFile.FileName;
                    file_name = file_name.Replace(" ", "_");
                    var filePath = workingFolder + file_name;
                    postedFile.SaveAs(filePath);

                    try
                    {
                        //  kq = helper.ExecuteNonQuery(query_str, aParams) + ";";
                        kq_suscess = kq_suscess + 1;
                    }
                    catch (Exception e)
                    {
                        kq_error = kq_error + 1;
                    }
                    kq_all = "Tổng số file :" + kq_total.ToString() + "; thành công :" + kq_suscess.ToString() + "; Lỗi : File > 5MB :" + kq_max.ToString() + ", Khác :" + kq_error.ToString();
                }
                result = Request.CreateResponse(HttpStatusCode.Created, kq_all);
            }
                
            
            else
            {
                result = Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            return ResponseMessage(result);
        }
    }
}

