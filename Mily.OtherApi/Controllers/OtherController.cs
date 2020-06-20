using Microsoft.AspNetCore.Mvc;
using Mily.Socket.SocketRoute;
using System.Threading.Tasks;
using System.Linq;
using System;
using Mily.Socket.SocketConfig;
using Mily.Socket.SocketCall;
using System.Threading;
using XExten.Common;
using Mily.Socket.SocketInterface;

namespace Mily.OtherApi.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    [SocketRoute("TestApi")]
    public class OtherController : Controller
    {
        [SocketMethod]
        [SocketAuthor(false)]
        public async Task<ActionResult<Object>> GetTest(ResultProvider Param)
        {
            return await Task.FromResult("我是测测试");
        }


        [SocketMethod]
        [SocketAuthor(true)]
        public async Task<ActionResult<Object>> GetTest1(ResultProvider Param)
        {
            return await Task.FromResult("我是测测试");
        }
    }

    public class SocketSessionHandler : ISocketSessionHandler
    {
        public bool Executing(ISocketSession Session)
        {
            var ss = Session;
            return false;
        }
    }
}