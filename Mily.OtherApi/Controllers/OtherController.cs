using Microsoft.AspNetCore.Mvc;
using Mily.Socket.SocketRoute;
using System.Threading.Tasks;
using System.Linq;
using System;


namespace Mily.OtherApi.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    [SocketRoute("OtherApi",ControllerName = "Other")]
    public class OtherController : Controller
    {
        [SocketMethod]
        public void Test1() { 
        
        }
        public void Test2()
        {

        }
    }
}