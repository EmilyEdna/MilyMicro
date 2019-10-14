using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Service.ViewSetting.ApiSettting
{
    public class ResultMiddleWare
    {
        public Boolean IsSuccess { get; set; }
        public Int32 StatusCode { get; set; }
        public Object ResultData { get; set; }
        public String Info { get; set; }
    }
}
