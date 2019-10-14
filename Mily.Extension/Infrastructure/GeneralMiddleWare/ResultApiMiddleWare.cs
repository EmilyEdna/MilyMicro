using System;

namespace Mily.Extension.Infrastructure.GeneralMiddleWare
{
    public class ResultApiMiddleWare
    {
        public Boolean IsSuccess { get; set; }
        public Int32 StatusCode { get; set; }
        public Object ResultData { get; set; }
        public String Info { get; set; }

        public static ResultApiMiddleWare Instance(Object Param)
        {
            return new ResultApiMiddleWare() { ResultData = Param };
        }

        public static ResultApiMiddleWare Instance(Boolean Flag, Int32 Code, Object Param, String Msg)
        {
            return new ResultApiMiddleWare() { IsSuccess = Flag, StatusCode = Code, ResultData = Param, Info = Msg };
        }
    }
}