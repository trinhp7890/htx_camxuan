using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using API_TPL.DAL;
using Microsoft.AspNet.Identity;

namespace API_TPL.Controllers.UPLOAD
{
    //[Authorize]
    [RoutePrefix("api/upload")]
    public class AnhController : ApiController
    {
        static string connString = System.Configuration.ConfigurationManager.ConnectionStrings["QLCV"].ToString();
        DBHelper helper = new DBHelper(connString);
        /// <summary>
        /// View theo loại
        /// </summary>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("getbyloai"), HttpGet]
        public IHttpActionResult getbyloai(string ID_LOAI)
        {
            string query_str = "Select * from UPLOAD where Active=0 AND ID_DOITUONG="+ ID_LOAI + "'";
            try
            {

                DataTable kq = helper.ExecuteQueryString(query_str);

                return ResponseMessage(Request.CreateResponse(HttpStatusCode.OK, kq));
            }
            catch (Exception ex)
            {
                string err = ex.Message.Substring(0, ex.Message.IndexOf("\n", 0)).Substring(ex.Message.IndexOf(":") + 2).Trim();
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.InternalServerError, err));
            }
        }
        /// <summary>
        /// View theo loại
        /// </summary>
        [HostAuthentication(DefaultAuthenticationTypes.ExternalBearer)]
        [Route("getbydoituong"), HttpGet]
        public IHttpActionResult getbydoituong(string Id_doituong,string loai)
        {
            string query_str = "Select * from UPLOAD where ID_DOITUONG='" + Id_doituong + "' AND LOAI='"+ loai+"'";
            try
            {

                DataTable kq = helper.ExecuteQueryString(query_str);

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
