﻿using Owin;
namespace TokenAuth
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);            
            ConfigureAuth(app);
        }
    }
}