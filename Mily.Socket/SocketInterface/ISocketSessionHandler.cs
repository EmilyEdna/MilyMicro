using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Socket.SocketInterface
{
    public interface ISocketSessionHandler
    {
        void BeforeExecute(ISocketSession Session);
        bool Executing(ISocketSession Session);
        void Executed(ISocketSession Session);
    }
}
