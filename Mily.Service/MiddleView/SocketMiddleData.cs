using Mily.Service.MiddleView.ViewEnum;
using Mily.Service.MiddleView.ViewImpl;
using Mily.Service.MiddleView.ViewInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Service.MiddleView
{
    public class SocketMiddleData
    {
        public SendTypeEnum SendType { get; set; }
        public SocketSession Session { get; set; }
        public int? SendPort { get; set; }
        public SocketResult MiddleResult { get; set; }
        /// <summary>
        /// 传输数据
        /// </summary>
        /// <param name="SendType"></param>
        /// <param name="Result"></param>
        /// <param name="Session"></param>
        /// <param name="SendPort"></param>
        /// <returns></returns>
        public static SocketMiddleData Middle(SendTypeEnum SendType, SocketResult Result, SocketSession Session = null, int? SendPort = null)
        {
            return new SocketMiddleData()
            {
                SendType = SendType,
                MiddleResult = Result,
                Session = Session,
                SendPort= SendPort
            };
        }
    }
}
