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
using Newtonsoft.Json.Linq;
using OfficeOpenXml;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Headers;
using System.Web;
using System.Text;

namespace API_TPL.Controllers.Congviec
{
    [Authorize]
    [RoutePrefix("api/baocao")]
    public class BaocaoController : ApiController
    {
        static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["QLCV"].ToString();
        DBHelper helper = new DBHelper(connString);

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("congviecdonvi"), HttpPost]
        public IHttpActionResult BAOCAO_CVDONVI([FromBody] dynamic obj)
        {
            string query_str = "BAOCAO.BAOCAO_CONGVIEC_THEODONVI";

            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmJsonData", obj.prmJsonData, OracleDbType.Clob, ParameterDirection.Input);
                OracleParameter resultParam = new OracleParameter("RESULTS", OracleDbType.RefCursor);
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
        [Route("congviecdonvi_exp"), HttpGet]
        public IHttpActionResult BAOCAO_CVDONVI_EXP(string MA_DV, int LOAI_CV, string TUNGAY, string DENNGAY, int TINHCHAT)
        {
           
            string query_str = "BAOCAO.BAOCAO_CONGVIEC_THEODONVI";

            StringBuilder str_prmJsonDatas = new StringBuilder();
            StringBuilder str_ojb = new StringBuilder();
            str_ojb.Append("'MA_DV':");
            str_ojb.Append("'" + MA_DV + "',");

            str_ojb.Append("'LOAI_CV':");
            str_ojb.Append(LOAI_CV + ",");

            str_ojb.Append("'TUNGAY':");
            str_ojb.Append("'" + TUNGAY + "',");
            str_ojb.Append("'DENNGAY':");
            str_ojb.Append("'" + DENNGAY + "',");
            str_ojb.Append("'TINHCHAT':");
            str_ojb.Append(TINHCHAT);

            str_prmJsonDatas.Append("{" + str_ojb.ToString() + "}");
            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmJsonData", str_prmJsonDatas, OracleDbType.Clob, ParameterDirection.Input);
                OracleParameter resultParam = new OracleParameter("RESULTS", OracleDbType.RefCursor);
                resultParam.Direction = ParameterDirection.Output;

                aParams[1] = resultParam;

                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);

                string templateDocument = HttpContext.Current.Server.MapPath("~/Templates/BaocaoDonvi.xlsx");

                MemoryStream output = new MemoryStream();

                using (FileStream templateDocumentStream = File.OpenRead(templateDocument))
                {
                    using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                    {
                        ExcelWorksheet sheet = package.Workbook.Worksheets["BaocaoDonvi"];

                        int startRow = 7;
                        int rowIndex = startRow;
                        int SL = startRow + kq.Rows.Count + 1;
                        for (int i = 0; i < kq.Rows.Count; i++)
                        {
                            sheet.Cells[rowIndex, 2].Value = (i+1).ToString();
                            sheet.Cells[rowIndex, 3].Value = kq.Rows[i]["ngay_giao"].ToString();
                            sheet.Cells[rowIndex, 4].Value = kq.Rows[i]["ten_cv"].ToString();
                            sheet.Cells[rowIndex, 5].Value = kq.Rows[i]["nguonphatsinh"].ToString();
                            sheet.Cells[rowIndex, 6].Value = kq.Rows[i]["nguoi_giao"].ToString();
                            sheet.Cells[rowIndex, 7].Value = kq.Rows[i]["nguoi_chutri"].ToString();
                            sheet.Cells[rowIndex, 8].Value = kq.Rows[i]["nguoi_phoihop"].ToString();
                            sheet.Cells[rowIndex, 9].Value = kq.Rows[i]["nguoi_giamsat"].ToString();
                            sheet.Cells[rowIndex, 10].Value = kq.Rows[i]["ngay_batdau"].ToString();
                            sheet.Cells[rowIndex, 11].Value = kq.Rows[i]["ngay_ketthuc"].ToString();
                            sheet.Cells[rowIndex, 12].Value = kq.Rows[i]["trangthai"].ToString();
                            sheet.Cells[rowIndex, 13].Value = kq.Rows[i]["tile_hoanthanh"].ToString();
                            sheet.Cells[rowIndex, 14].Value = kq.Rows[i]["tinh_chat"].ToString();
                            rowIndex++;
                        }
                        if (kq.Rows.Count > 0)
                        {
                            sheet.Cells[3, 1].Value = kq.Rows[0]["ten_dv"].ToString();
                            sheet.Cells[4, 1].Value = "Từ ngày " + TUNGAY + " đến ngày " + DENNGAY;
                        }
                        using (ExcelRange range = sheet.Cells[startRow, 2, rowIndex - 1, 14])
                        {
                            range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        }
                        package.SaveAs(output);
                    }
                }

                string documentName = string.Format("baocaotheodonvi.{0}", "xlsx");

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                byte[] m = output.ToArray();
                // Reset Stream Position
                output.Position = 0;
                result.Content = new ByteArrayContent(m);

                // Generic Content Header
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

                //Set Filename sent to client
                result.Content.Headers.ContentDisposition.FileName = documentName;

                return ResponseMessage(result);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("cv_tonghop_nv"), HttpPost]
        public IHttpActionResult BAOCAO_CVTONGHOP_NV([FromBody] dynamic obj)
        {
            string query_str = "BAOCAO.BAOCAO_CONGVIEC_NHANVIEN";

            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmJsonData", obj.prmJsonData, OracleDbType.Clob, ParameterDirection.Input);
                OracleParameter resultParam = new OracleParameter("RESULTS", OracleDbType.RefCursor);
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
        [Route("cv_tonghop_nv_exp"), HttpGet]
        public IHttpActionResult BAOCAO_TH_NHANVIEN_EXP(string MA_DV, string TUNGAY, string DENNGAY)
        {

            string query_str = "BAOCAO.BAOCAO_CONGVIEC_NHANVIEN";

            StringBuilder str_prmJsonDatas = new StringBuilder();
            StringBuilder str_ojb = new StringBuilder();
            str_ojb.Append("'MA_DV':");
            str_ojb.Append("'" + MA_DV + "',");

            str_ojb.Append("'TUNGAY':");
            str_ojb.Append("'" + TUNGAY + "',");
            str_ojb.Append("'DENNGAY':");
            str_ojb.Append("'" + DENNGAY + "'");

            str_prmJsonDatas.Append("{" + str_ojb.ToString() + "}");
            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmJsonData", str_prmJsonDatas, OracleDbType.Clob, ParameterDirection.Input);
                OracleParameter resultParam = new OracleParameter("RESULTS", OracleDbType.RefCursor);
                resultParam.Direction = ParameterDirection.Output;

                aParams[1] = resultParam;

                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);

                string templateDocument = HttpContext.Current.Server.MapPath("~/Templates/BaocaoTonghopDonvi.xlsx");

                MemoryStream output = new MemoryStream();

                using (FileStream templateDocumentStream = File.OpenRead(templateDocument))
                {
                    using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                    {
                        ExcelWorksheet sheet = package.Workbook.Worksheets["BaocaoDonvi"];

                        int startRow = 8;
                        int rowIndex = startRow;
                        int SL = startRow + kq.Rows.Count + 1;
                        for (int i = 0; i < kq.Rows.Count; i++)
                        {
                            sheet.Cells[rowIndex, 2].Value = (i + 1).ToString();
                            sheet.Cells[rowIndex, 3].Value = kq.Rows[i]["ten_nd"].ToString();
                            sheet.Cells[rowIndex, 4].Value = kq.Rows[i]["chutri_moi"].ToString();
                            sheet.Cells[rowIndex, 5].Value = kq.Rows[i]["chutri_dangthuchien"].ToString();
                            sheet.Cells[rowIndex, 6].Value = kq.Rows[i]["chutri_dahoanthanh"].ToString();
                            sheet.Cells[rowIndex, 7].Value = kq.Rows[i]["chutri_tong"].ToString();
                            sheet.Cells[rowIndex, 8].Value = kq.Rows[i]["phoihop_moi"].ToString();
                            sheet.Cells[rowIndex, 9].Value = kq.Rows[i]["phoihop_dangthuchien"].ToString();
                            sheet.Cells[rowIndex, 10].Value = kq.Rows[i]["phoihop_dahoanthanh"].ToString();
                            sheet.Cells[rowIndex, 11].Value = kq.Rows[i]["phoihop_tong"].ToString();
                            rowIndex++;
                        }
                        if (kq.Rows.Count > 0)
                        {
                            sheet.Cells[3, 1].Value = kq.Rows[0]["ten_dv"].ToString();
                            sheet.Cells[4, 1].Value = "Từ ngày " + TUNGAY + " đến ngày " + DENNGAY;
                        }
                        using (ExcelRange range = sheet.Cells[startRow, 2, rowIndex - 1, 11])
                        {
                            range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        }
                        package.SaveAs(output);
                    }
                }

                string documentName = string.Format("BaocaoTonghopDonvi.{0}", "xlsx");

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                byte[] m = output.ToArray();
                // Reset Stream Position
                output.Position = 0;
                result.Content = new ByteArrayContent(m);

                // Generic Content Header
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

                //Set Filename sent to client
                result.Content.Headers.ContentDisposition.FileName = documentName;

                return ResponseMessage(result);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }

        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("cv_theo_nguonps"), HttpPost]
        public IHttpActionResult BAOCAO_CV_NGUONPS([FromBody] dynamic obj)
        {
            string query_str = "BAOCAO.BAOCAO_CONGVIEC_NGUONPHATSINH";

            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmJsonData", obj.prmJsonData, OracleDbType.Clob, ParameterDirection.Input);
                OracleParameter resultParam = new OracleParameter("RESULTS", OracleDbType.RefCursor);
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
        [Route("cv_theo_nguonps_exp"), HttpGet]
        public IHttpActionResult BAOCAO_NGUONPS_EXP(string NGUON_PHATSINH, string TUNGAY, string DENNGAY)
        {

            string query_str = "BAOCAO.BAOCAO_CONGVIEC_NGUONPHATSINH";

            StringBuilder str_prmJsonDatas = new StringBuilder();
            StringBuilder str_ojb = new StringBuilder();
            str_ojb.Append("'NGUON_PHATSINH':");
            str_ojb.Append("'" + NGUON_PHATSINH + "',");

            str_ojb.Append("'TUNGAY':");
            str_ojb.Append("'" + TUNGAY + "',");
            str_ojb.Append("'DENNGAY':");
            str_ojb.Append("'" + DENNGAY + "'");

            str_prmJsonDatas.Append("{" + str_ojb.ToString() + "}");
            object[] aParams = new object[2];
            try
            {
                aParams[0] = helper.BuildParameter("prmJsonData", str_prmJsonDatas, OracleDbType.Clob, ParameterDirection.Input);
                OracleParameter resultParam = new OracleParameter("RESULTS", OracleDbType.RefCursor);
                resultParam.Direction = ParameterDirection.Output;

                aParams[1] = resultParam;

                DataTable kq = helper.ExecuteQueryStoreProcedure(query_str, aParams);

                string templateDocument = HttpContext.Current.Server.MapPath("~/Templates/BaocaoNguonphatsinh.xlsx");

                MemoryStream output = new MemoryStream();

                using (FileStream templateDocumentStream = File.OpenRead(templateDocument))
                {
                    using (ExcelPackage package = new ExcelPackage(templateDocumentStream))
                    {
                        ExcelWorksheet sheet = package.Workbook.Worksheets["BaocaoDonvi"];

                        int startRow = 7;
                        int rowIndex = startRow;
                        int SL = startRow + kq.Rows.Count + 1;
                        for (int i = 0; i < kq.Rows.Count; i++)
                        {
                            sheet.Cells[rowIndex, 2].Value = (i + 1).ToString();
                            sheet.Cells[rowIndex, 3].Value = kq.Rows[i]["ngay_giao"].ToString();
                            sheet.Cells[rowIndex, 4].Value = kq.Rows[i]["ten_cv"].ToString();
                            sheet.Cells[rowIndex, 5].Value = kq.Rows[i]["nguonphatsinh"].ToString();
                            sheet.Cells[rowIndex, 6].Value = kq.Rows[i]["nguoi_giao1"].ToString();
                            sheet.Cells[rowIndex, 7].Value = kq.Rows[i]["nguoi_chutri1"].ToString();
                            sheet.Cells[rowIndex, 8].Value = kq.Rows[i]["nguoi_phoihop"].ToString();
                            sheet.Cells[rowIndex, 9].Value = kq.Rows[i]["nguoi_giamsat1"].ToString();
                            sheet.Cells[rowIndex, 10].Value = kq.Rows[i]["ngay_batdau"].ToString();
                            sheet.Cells[rowIndex, 11].Value = kq.Rows[i]["ngay_ketthuc"].ToString();
                            sheet.Cells[rowIndex, 12].Value = kq.Rows[i]["trangthai1"].ToString();
                            sheet.Cells[rowIndex, 13].Value = kq.Rows[i]["tile_hoanthanh"].ToString();
                            sheet.Cells[rowIndex, 14].Value = kq.Rows[i]["tinh_chat1"].ToString();
                            rowIndex++;
                        }
                        if (kq.Rows.Count > 0)
                        {
                            sheet.Cells[3, 1].Value = kq.Rows[0]["nguonphatsinh"].ToString();
                            sheet.Cells[4, 1].Value = "Từ ngày " + TUNGAY + " đến ngày " + DENNGAY;
                        }
                        using (ExcelRange range = sheet.Cells[startRow, 2, rowIndex - 1, 14])
                        {
                            range.Style.Border.Top.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Right.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Bottom.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                            range.Style.Border.Left.Style = OfficeOpenXml.Style.ExcelBorderStyle.Thin;
                        }
                        package.SaveAs(output);
                    }
                }

                string documentName = string.Format("BaocaoNguonphatsinh.{0}", "xlsx");

                HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);

                byte[] m = output.ToArray();
                // Reset Stream Position
                output.Position = 0;
                result.Content = new ByteArrayContent(m);

                // Generic Content Header
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

                //Set Filename sent to client
                result.Content.Headers.ContentDisposition.FileName = documentName;

                return ResponseMessage(result);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
            }
        }
    }
    public class SubData
    {
        public string MA_DV { get; set; }
        public int LOAI_CV { get; set; }
        public string TUNGAY { get; set; }
        public string DENNGAY { get; set; }
        public int TINHCHAT { get; set; }
    }
}
