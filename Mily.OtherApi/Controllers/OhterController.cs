using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;

namespace Mily.OtherApi.Controllers
{
    [Route("Api/[controller]/[action]")]
    [ApiController]
    public class OhterController : BaseApiController
    {
        public async Task<ActionResult<object>> Test() => await Task.Run(() => { return "Mily"; });
    }
}