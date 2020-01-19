using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Socket.SocketInterface
{
    public interface ISocketResult
    {
        string Router { get; set; }
        string SocketData { get; set; }
    }
}
