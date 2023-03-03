using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Web;
using System.Threading.Tasks;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using API_TPL.DAL;
using API_TPL.Services;
using System.Security.Claims;
using Microsoft.Owin.Security;
using TokenAuth;
using Microsoft.Owin;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Linq;

namespace API_TPL.Controllers.Admin
{
    //[RoutePrefix("api/Account")]
    public class AccountController : ApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("FacebookLogin")]
        public async Task<IHttpActionResult> FacebookLogin(string fb_id, string username,string email)
        {
            //[Code to validate input...]
            var tokenExpirationTimeSpan = TimeSpan.FromDays(14);
            //ApplicationUser user = null;
            // Get the fb access token and make a graph call to the /me endpoint    
            // Check if the user is already registered
            // If yes retrieve the user 
            // If not, register it  
            // Finally sign-in the user: this is the key part of the code that creates the bearer token and authenticate the user

            var user_id = fb_id;
            var identity = new ClaimsIdentity(Startup.OAuthBearerOptions.AuthenticationType);
            var userService = new UserService();
            var user = userService.ValidateUser(email, "B346E4FFF0B0403DAAA83AEE71F84390");
            string kq_login = "1";
            if (user == null)
            {
                kq_login = userService.HETHONG_NGUOIDUNG_INSERT_FROM_FACEBOOK(username, user_id, email);
            }

            //identity.AddClaim(new Claim("FacebookAccessToken", "Facebook"));
            //new Claim("FacebookAccessToken", model.token);
            identity.AddClaim(new Claim(ClaimTypes.Name, username, null, "Facebook"));
            // This claim is used to correctly populate user id
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user_id, null, "LOCAL_AUTHORITY"));
            identity.AddClaim(new Claim(ClaimTypes.Email, email, null, "Facebook"));
            identity.AddClaim(new Claim(ClaimTypes.Sid, user_id, null, "Facebook"));
            
            AuthenticationTicket ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
            var currentUtc = new Microsoft.Owin.Infrastructure.SystemClock().UtcNow;
            ticket.Properties.IssuedUtc = currentUtc;
            ticket.Properties.ExpiresUtc = currentUtc.Add(tokenExpirationTimeSpan);
            var accesstoken = Startup.OAuthBearerOptions.AccessTokenFormat.Protect(ticket);
            //Authentication.SignIn(identity);
            // Create the response


            //Xử sau
            //string new_roles = userService.getRoles(email);
            //DataTable ds_roles = userService.get_dsrole(email);


            JObject blob = new JObject(
                new JProperty("userName", email),
                new JProperty("access_token", accesstoken),
                new JProperty("token_type", "bearer"),
                new JProperty("expires_in", tokenExpirationTimeSpan.TotalSeconds.ToString()),
                new JProperty(".issued", ticket.Properties.IssuedUtc.ToString()),
                new JProperty(".expires", ticket.Properties.ExpiresUtc.ToString()),
                new JProperty("kq_login", kq_login)
                //new JProperty("ds_role", ds_roles)
            );            
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(blob);
            // Return OK
            return Ok(blob);
        }
    }
}