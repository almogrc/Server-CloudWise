using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Primitives;
using Server_cloudata.Models;
using Server_cloudata.ServerDataManager;
using Server_cloudata.Services;
using System;
using System.Net;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace Server_cloudata.Middleware
{
    public class MachineMiddleware
    {
        public MachineMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor, CustomersService customerService)
        {
            _requestDelegate = next;
            _contextAccessor = httpContextAccessor;
            _customerService = customerService;
        }
        private RequestDelegate _requestDelegate;
        private IHttpContextAccessor _contextAccessor;
        private CustomersService _customerService;
        public async Task Invoke(HttpContext context)
        {
            try
            {
                StringValues machineId;
                if (context.Request.Headers.TryGetValue(ServerUtils.MachineId, out machineId))
                {
                    Customer customer = await _customerService.GetAsyncByEmail(context.Session.GetString(context.Session.Id));
                    context.Request.Headers.Remove(ServerUtils.MachineId);
                    context.Request.Headers.Add(ServerUtils.MachineId, customer.VMs.Find(vm => vm.Name.ToLower() == machineId.ToString().ToLower()).Address);
                    await _requestDelegate.Invoke(context);
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }catch(Exception e)
            {
                throw e;
            }
        }
        
    }
}
