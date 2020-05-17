using Microsoft.AspNetCore.Mvc;
using Mily.Socket.SocketRoute;
using System.Threading.Tasks;
using System.Linq;
using System;
using Mily.Socket.SocketConfig;
using Mily.Socket.SocketCall;
using System.Threading;
using XExten.Common;

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

    }
}