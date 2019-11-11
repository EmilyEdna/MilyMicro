using Mily.RabbitMQ.AcceptFunc;
using System;
using System.Threading.Tasks;

namespace Mily.RabbitMQ.Subscriber
{
    public class ServiceQueryExcute
    {
        public static void ExtuteMQ<T>(MQEnum Type) where T : new()
        {
            Task.Run(() =>
            {
                AcceptEntity accept = new AcceptEntity();
                if (Type == MQEnum.Push)
                {
                    accept.SendType = MQEnum.Push;
                    accept.ExchangeName = "Message.Direct";
                    accept.QueeName = "Meesage.DirectQuene";
                    accept.RouteName = "RouteKey";
                }
                else if (Type == MQEnum.Sub)
                {
                    accept.SendType = MQEnum.Sub;
                    accept.ExchangeName = "Message.Fanout";
                    accept.QueeName = "Meesage.FanoutQuene";
                }
                else
                {
                    accept.SendType = MQEnum.Top;
                    accept.ExchangeName = "Message.Topic";
                    accept.QueeName = "Meesage.FanoutQuene";
                    accept.RouteName = "RouteKey";
                }
                RabbitMQFactory.Subscriber<Accept, T>(accept);
            });
        }
    }
}