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
    public class OtherController : Controller
    {
    }
}