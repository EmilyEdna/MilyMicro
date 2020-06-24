using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using XExten.Common;

namespace Mily.WebClient.MainApi
{
    [HttpHost("http://127.0.0.1:5000/api/system/")]
    public interface IMainApi : IHttpApi
    {
        [HttpGet("Login")]
        ITask<Object> Login(ResultProvider Provider);

        [HttpGet("SearchMenuItemPage")]
        ITask<Object> SearchMenuItemPage(PageQuery Page);
    }
}
