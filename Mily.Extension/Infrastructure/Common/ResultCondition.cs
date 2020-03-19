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
        public String ServerDateStr { get; set; }
        public long ServerDateLong { get; set; }
        public static ResultCondition Instance(Action<ResultCondition> Action) 
        {
            ResultCondition Condition = new ResultCondition();
            Action(Condition);
            return Condition;
        }
    }
}
