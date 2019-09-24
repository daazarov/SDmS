using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SDmS.Mvc.Modules
{
    public class ApplyTokenModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += HandleBeginRequest;
        }

        private void HandleBeginRequest(object src, EventArgs args)
        {
            HttpContext context = HttpContext.Current;

            HttpCookie cookieReq = context.Request.Cookies["Oauth_token"];

            string token;
            if (cookieReq != null && !context.Request.Headers.AllKeys.Any(x => x == "Authorization"))
            {
                token = cookieReq["token"];
                context.Request.Headers.Add("Authorization", "Bearer" + " " + token);
            }
        }
    }
}