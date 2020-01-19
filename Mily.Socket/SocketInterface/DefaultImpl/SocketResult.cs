using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Socket.SocketInterface.DefaultImpl
{
    public class SocketResult : ISocketResult
    {
        public string Router { get; set; }
        public string SocketData { get; set; }
        public static SocketResult SetValue(string Router, string SocketData)
        {
            return new SocketResult
            {
                Router = Router,
                SocketData = SocketData
            };
        }
    }
}
