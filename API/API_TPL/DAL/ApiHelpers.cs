using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace API_TPL.DAL
{
    public class ApiHelpers
    {
        
        private string Ntkey = ConfigurationManager.AppSettings["Nt"].ToString().Trim();
        public HttpResponseMessage getResponse(string url,string path)
        {
            HttpResponseMessage response;           
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Ntkey);
                response = client.GetAsync(path).Result;
            }

            return response;
        }



        public string postResponse(string url,string path,dynamic obj)
        {
            
            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key","="+ Ntkey);
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

            HttpResponseMessage response =  client.PostAsync(path, content).Result;
            
            if (response.IsSuccessStatusCode)
            {
                return "200";
            }
            else
            {
                //HttpResponseMessage response = Request.CreateResponse(HttpStatusCode.BadRequest, "Error message");
                return "500";
            }
            
        }
        public HttpResponseMessage post(string url, string path, dynamic obj)
        {

            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("key", "=" + Ntkey);
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.PostAsync(path, content).Result;
            return response;



        }


        public HttpResponseMessage postNew(string url, string path, dynamic obj)
        {

            var client = new HttpClient();
            client.BaseAddress = new Uri(url);
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", Ntkey);
            var content = new StringContent(JsonConvert.SerializeObject(obj), Encoding.UTF8, "application/json");
            HttpResponseMessage response = client.PostAsync(path, content).Result;            
            return response;

        }
        public string getString(string url, string path)
        {
            string _response = "";
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    _response = client.GetStringAsync(path).Result;
                }
                return _response;
            }
            catch (Exception ex)
            {
                _response = ex.Message;
                return "0";
            }

        }
        public HttpResponseMessage getNew (string url, string path)
        {
            string token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6IlNtYXJ0QWRzOyIsInBhc3N3b3JkIjoiU21hcnRBZHNAMTIzOyIsIm9yZ2FuaXphdGlvbiI6IlZOUFQtSVQiLCJwcm9qZWN0IjoiU21hcnRBZHMtSFVFIiwiY3JlYXRlZF9ieSI6IkNVT05HS1YiLCJpYXQiOjE1NzI5MzgwNjh9.2e8PhDX2zuOpLDZFkBQFmcY-cP6FsBLGNdoHbN2EsoQ";
            HttpResponseMessage response;
            using (var client = new HttpClient())
            {

                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(token);
                response = client.GetAsync(path).Result;
                if (!response.IsSuccessStatusCode)
                {
                    return null;
                }

            }

            return response;

        }

    }
}