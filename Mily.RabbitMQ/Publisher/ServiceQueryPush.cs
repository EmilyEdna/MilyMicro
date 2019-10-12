using Mily.RabbitMQ.PushFunc;
using System.Threading.Tasks;

namespace Mily.RabbitMQ.Publisher
{
    public class ServiceQueryPush
    {
        public static async Task<bool> PushMQAsync<T>(T Entity, MQEnum MQType = MQEnum.Push) where T : class, new()
        {
            //推送模式
            //推送模式下需指定管道名称和路由键值名称
            //消息只会被发送到指定的队列中去
            //订阅模式
            //订阅模式下只需指定管道名称
            //消息会被发送到该管道下的所有队列中
            //主题路由模式
            //路由模式下需指定管道名称和路由值
            //消息会被发送到该管道下，和路由值匹配的队列中去
            PushEntity<T> entity = new PushEntity<T>();
            entity.BodyData = Entity;
            entity.SendType = MQType;
            if (MQType == MQEnum.Push)
            {
                entity.ExchangeName = "Message.Direct";
                entity.RouteName = "RouteKey";
            }
            else if (MQType == MQEnum.Sub)
            {
                entity.ExchangeName = "Message.Fanout";
            }
            else
            {
                entity.ExchangeName = "Message.Topic";
                entity.RouteName = "RouteKey";
            }
            return await RabbitMQFactory.SendMQAsync(entity).ContinueWith(t => { return t.IsCompleted ? true : false; });
        }

        public static bool PushMQ<T>(T Entity, MQEnum MQType = MQEnum.Push) where T : class, new()
        {
            //推送模式
            //推送模式下需指定管道名称和路由键值名称
            //消息只会被发送到指定的队列中去
            //订阅模式
            //订阅模式下只需指定管道名称
            //消息会被发送到该管道下的所有队列中
            //主题路由模式
            //路由模式下需指定管道名称和路由值
            //消息会被发送到该管道下，和路由值匹配的队列中去
            PushEntity<T> entity = new PushEntity<T>();
            entity.BodyData = Entity;
            entity.SendType = MQType;
            if (MQType == MQEnum.Push)
            {
                entity.ExchangeName = "Message.Direct";
                entity.RouteName = "RouteKey";
            }
            else if (MQType == MQEnum.Sub)
            {
                entity.ExchangeName = "Message.Fanout";
            }
            else
            {
                entity.ExchangeName = "Message.Topic";
                entity.RouteName = "RouteKey";
            }
            return RabbitMQFactory.SendMQ(entity);
        }
    }
}