using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace Mily.RabbitMQ.AcceptFunc
{
    public class Accept : IAccept
    {
        public void AcceptMQ<T>(string msg)
        {
            JsonConvert.DeserializeObject<T>(msg);
            //do somethings
        }
    }
}
