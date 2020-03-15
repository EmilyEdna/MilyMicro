using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Extension.Infrastructure.Common
{
    public class ResultCondition
    {
        public Boolean IsSuccess { get; set; }
        public Int32 StatusCode { get; set; }
        public Object ResultData { get; set; }
        public String Info { get; set; }
        public DateTime ServerTime { get; set; }

        public static ResultCondition Instance(Object Param)
        {
            return new ResultCondition() { ResultData = Param };
        }

        public static ResultCondition Instance(Boolean Flag, Int32 Code, Object Param, String Msg, DateTime Time)
        {
            return new ResultCondition() { IsSuccess = Flag, StatusCode = Code, ResultData = Param, Info = Msg, ServerTime = Time };
        }
    }
}
