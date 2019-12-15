using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System;
using Mily.Extension.AutofacIoc;

namespace Mily.OtherApi.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class OtherController : Controller
    {
        public string Test() => "lzh";
    }
}