using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Amazon.Runtime.Internal;
using System.IO;
using Server_cloudata.ServerDataManager;

namespace Server_cloudata.Middleware
{
    public class SessionMiddleware
    {
        private RequestDelegate _requestDelegate;
        private IHttpContextAccessor _contextAccessor;
        private readonly string _sessionCookie = "sessionCookie";

        public SessionMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
        {
            _requestDelegate = next;
            _contextAccessor = httpContextAccessor; 
        }
        public async Task Invoke(HttpContext context)
        {
            if (RequestToLoginOrRegister(context))
            {
                 await _requestDelegate(context);
            }
            else if (await CheckIfSessionExist(context)) 
            { 
                await _requestDelegate(context);
            }
            else 
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("{{\"result\":\"login\"}}");
                // Bad request, do not call next middleware.
                return;
            }
        }

        private async Task<bool> CheckIfSessionExist(HttpContext context)
        {
            string sessionId;
            if(!context.Request.Cookies.TryGetValue(ServerDataManager.ServerUtils.SessionCookie, out sessionId))
            {
                 return false;
            }
            byte[] values;
            if (_contextAccessor.HttpContext.Session.TryGetValue(sessionId, out values))
            {
                return true;
            }
            return false;
        }

        private bool RequestToLoginOrRegister(HttpContext context)
        {
            string url = context.Request.Path;
            url.ToLower();
            if (url.Contains("login") || url.Contains("signUp"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
