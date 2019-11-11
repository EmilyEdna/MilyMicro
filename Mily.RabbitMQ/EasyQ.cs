using Mily.RabbitMQ.Publisher;
using Mily.RabbitMQ.Subscriber;
using System;
using System.Threading.Tasks;

namespace Mily.RabbitMQ
{
    /// <summary>
    /// RabbitMQ
    /// </summary>
    public class EasyQ
    {
        #region 发布消息
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Entity">消息内容</param>
        /// <param name="MQType">模式</param>
        /// <returns></returns>
        public static Boolean PushMQ<T>(T Entity, MQEnum MQType = MQEnum.Push) where T : class, new()
        {
            return ServiceQueryPush.PushMQ(Entity, MQType);
        }
        /// <summary>
        /// 发布消息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Entity">消息内容</param>
        /// <param name="MQType">模式</param>
        /// <returns></returns>
        public static async Task<Boolean> PushMQAsync<T>(T Entity, MQEnum MQType = MQEnum.Push) where T : class, new()
        {
            return await ServiceQueryPush.PushMQAsync(Entity, MQType);
        }
        #endregion

        #region 订阅消息
        /// <summary>
        /// 订阅消息
        /// </summary>
        /// <typeparam name="T">数据类型的实体</typeparam>
        /// <param name="Type">模式</param>
        public static void ExtuteMQ<T>(MQEnum Type) where T : new()
        {
            ServiceQueryExcute.ExtuteMQ<T>(Type);
        }
        #endregion
    }
}
