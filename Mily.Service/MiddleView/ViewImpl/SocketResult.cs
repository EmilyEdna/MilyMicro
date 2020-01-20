using Mily.Service.MiddleView.ViewInterface;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Service.MiddleView.ViewImpl
{
    public class SocketResult : ISocketResult
    {
        public string Router { get; set; }
        public string SocketJsonData { get; set; }
    }
}
