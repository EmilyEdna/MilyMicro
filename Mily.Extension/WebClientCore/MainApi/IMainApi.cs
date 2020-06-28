using System;
using System.Collections.Generic;
using System.Text;
using WebApiClientCore;
using WebApiClientCore.Attributes;
using XExten.Common;

namespace Mily.Extension.WebClientCore.MainApi
{
    [MilyToken]
    [HttpHost("http://127.0.0.1:5000/Api/System/")]
    public interface IMainApi : IHttpApi
    {
        
        [HttpGet("SearchMenuItemPage")]
        ITask<Object> SearchMenuItemPage(PageQuery Page);
    }
}
