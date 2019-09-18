using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.RabbitMQ.AcceptFunc
{
    public interface IAccept
    {
        void AcceptMQ<T>(string msg);
    }
}
