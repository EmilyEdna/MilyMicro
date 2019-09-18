using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Mily.OtherApi.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class OhterController : BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<Object>> GetValue(string key, long? age) => await Task.Run(() => (key + "123"));
    }
}
