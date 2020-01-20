using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Service.MiddleView.ViewInterface
{
    public interface ISocketResult
    {
        string Router { get; set; }
        string SocketJsonData { get; set; }
    }
}
