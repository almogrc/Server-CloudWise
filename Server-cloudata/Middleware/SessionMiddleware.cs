using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Amazon.Runtime.Internal;
using System.IO;

namespace Server_cloudata.Middleware
{
    public class SessionMiddleware
    {
        private RequestDelegate _requestDelegate;
        private IHttpContextAccessor _contextAccessor;
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
            //var reader = new StreamReader(context.Request.Body);
            //var bodyAsText = await reader.ReadToEndAsync();
            //JObject body = JObject.Parse(bodyAsText);
            //string sessionId = body["SessionId"].ToString();
            byte[] values;
            if (_contextAccessor.HttpContext.Session.TryGetValue(_contextAccessor.HttpContext.Session.Id, out values))
            {
                return true;
            }
            return false;
        }

        private bool RequestToLoginOrRegister(HttpContext context)
        {
            string url = context.Request.Path;
            url.ToLower();
            if (url.Contains("login") || url.Contains("signup"))
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
