using Microsoft.AspNetCore.Mvc;
using Mily.Socket.SocketRoute;
using System.Threading.Tasks;
using System.Linq;
using System;
using Mily.Socket.SocketConfig;
using Mily.Socket.SocketEvent;
using System.Threading;

namespace Mily.OtherApi.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    [SocketRoute("OtherApi", ControllerName = "Other")]
    public class OtherController : Controller
    {
        [HttpGet]
        [SocketMethod]
        public async Task<ActionResult<string>> Test1()
        {
            return await Task.FromResult("123");
        }
        [HttpGet]
        public string Test2()
        {
            var m = new SocketSerializeData();
            m.AppendRoute("otherapi/other/test1");
            CallEvent.SendInternalInfo(m);
            return "1";
        }
    }
}