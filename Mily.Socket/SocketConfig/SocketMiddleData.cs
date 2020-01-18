using Mily.Socket.SocketEnum;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Socket.SocketConfig
{
    public class SocketMiddleData
    {
        public Dictionary<SendTypeEnum, Object> MiddleResult { get; set; }
        public static SocketMiddleData Middle(SendTypeEnum SendType, Object Data)
        {
          return  new SocketMiddleData() { MiddleResult = new Dictionary<SendTypeEnum, object> { { SendType, Data } } };
        }
    }
}
