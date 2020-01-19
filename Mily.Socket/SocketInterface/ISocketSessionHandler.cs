using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Socket.SocketInterface
{
    public interface ISocketSessionHandler
    {
        bool Executing(ISocketSession Session);
    }
}
