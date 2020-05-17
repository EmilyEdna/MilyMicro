using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mily.Socket.SocketCall;
using Mily.Socket.SocketConfig;
using Mily.Socket.SocketEvent;

namespace Miliy.MainApi.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class SocketTestController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<string>> SocketTest() 
        {
            SocketSerializeData SSD = new SocketSerializeData(); ;
            SSD.AppendRoute("TestApi/other/GetTest");
            SSD.AppendSerialized("Name", "lzh");
            Call.SendInternalInfo(SSD);
            var data = CallEventAction.Instance().DelegateResult;
            return await Task.FromResult("123");
        }
    }
}