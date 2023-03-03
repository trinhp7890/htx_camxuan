using System;
using Microsoft.Owin.Security;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Owin.Security.OAuth;
using API_TPL.Services;
using System.Configuration;
using System.Linq;
using System.Data;

namespace TokenAuth.Providers
{
    public class OAuthProvider : OAuthAuthorizationServerProvider
    {
        #region[GrantResourceOwnerCredentials]
        public override Task GrantResourceOwnerCredentials(OAuthGrantResourceOwnerCredentialsContext context)
        {
            return Task.Factory.StartNew(() =>
            {
                
                var userName = context.UserName;
                var password = context.Password;
                var userService = new UserService(); // our created one
                var user = userService.ValidateUser(userName, password);
                if (user != null)
                {
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.Sid, Convert.ToString(user.Id), null, "LOCAL_AUTHORITY"),
                        new Claim(ClaimTypes.Name, user.Name, null, "LOCAL_AUTHORITY"),
                        new Claim(ClaimTypes.Email, user.Email, null, "LOCAL_AUTHORITY")
                    };
                    ClaimsIdentity oAuthIdentity = new ClaimsIdentity(claims,
                                Startup.OAuthOptions.AuthenticationType);


                    string new_roles = userService.getRoles(user.UserName);
                    DataTable ds_roles = userService.get_dsrole(user.UserName);                    
                    AuthenticationProperties properties = CreateProperties(
                        user.UserName,
                        Newtonsoft.Json.JsonConvert.SerializeObject(ds_roles),
                        new_roles,
                        user.Madonvi,
                        user.Manhanvien
                        );
                    
                    var ticket = new AuthenticationTicket(oAuthIdentity, properties);
                    context.Validated(ticket);
                }
                else
                {
                    context.SetError("invalid_grant", "The user name or password is incorrect");
                }
            });
        }
        #endregion

        #region[ValidateClientAuthentication]
        public override Task ValidateClientAuthentication(OAuthValidateClientAuthenticationContext context)
        {
            if (context.ClientId == null)
                context.Validated();

            return Task.FromResult<object>(null);
        }
        #endregion

        #region[TokenEndpoint]
        public override Task TokenEndpoint(OAuthTokenEndpointContext context)
        {
            foreach (KeyValuePair<string, string> property in context.Properties.Dictionary)
            {
                context.AdditionalResponseParameters.Add(property.Key, property.Value);
            }

            return Task.FromResult<object>(null);
        }
        #endregion

        #region[CreateProperties]
        public static AuthenticationProperties CreateProperties(string userName)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName }
            };
            return new AuthenticationProperties(data);
        }
        #endregion

        public static AuthenticationProperties CreateProperties(string userName, string roles, string new_roles, string madonvi, string manhanvien)
        {
            IDictionary<string, string> data = new Dictionary<string, string>
            {
                { "userName", userName },
                { "roles", roles },
                { "new_roles", new_roles },
                { "madonvi", madonvi },
                { "manhanvien", manhanvien }
            };

            return new AuthenticationProperties(data);
        }
    }
}
