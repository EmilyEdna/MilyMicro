using Mily.Service.MiddleView.ViewInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Service.MiddleView.ViewImpl
{
    public class SocketSession : ISocketSession
    {
        public string PrimaryKey { get; set; }
        public string SessionAccount { get; set; }
        public string SessionRole { get; set; }
        public object CustomizeData { get; set; }
    }
}
