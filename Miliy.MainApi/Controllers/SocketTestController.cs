using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Mily.Socket.SocketCall;
using Mily.Socket.SocketConfig;
using Mily.Socket.SocketEvent;
using Mily.Socket.SocketInterface.DefaultImpl;

namespace Miliy.MainApi.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class SocketTestController : Controller
    {
        [HttpGet]
        public async Task<ActionResult<string>> SocketTest() 
        {
            SocketSerializeData SSD = new SocketSerializeData(); 
            SSD.AppendRoute("TestApi/other/GetTest")
                .AppendSerialized("Name", "lzh")
                .AppendSerialized(new Dictionary<string, object> { { "Age", 26 } })
                .AppendSerialized(new { Card = 100 });
            Call.SendInternalInfo(SSD);
            var data = CallEventAction.Instance().DelegateResult;


            SocketSerializeData SSD1 = new SocketSerializeData(); ;
            SSD1.AppendRoute("TestApi/other/GetTest1").AppendSerialized("Name", "lzh");
            Call.SendInternalInfo(SSD1, new SocketSessionDefault
            {
                SessionAccount = "admin",
                CustomizeData = "aa",
                PrimaryKey = "123",
                SessionRole = "Admin"
            });
            var data1 = CallEventAction.Instance().DelegateResult;

            return await Task.FromResult("123");
        }
    }
}