using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Socket.SocketInterface
{
    public interface ISocketSession
    {
        string PrimaryKey { get; set; }
        string SessionAccount { get; set; }
        string SessionRole { get; set; }
    }
}
