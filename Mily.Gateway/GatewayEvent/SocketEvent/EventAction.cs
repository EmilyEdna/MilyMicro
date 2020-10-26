using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using XExten.XCore;

namespace Mily.Gateway.GatewayEvent.SocketEvent
{
    public class EventAction 
    {
        private Dictionary<String, Object> ResponseResult;
        private static readonly Dictionary<String, EventAction> Cache = new Dictionary<String, EventAction>();
        private static readonly ConcurrentDictionary<String, Object> ThreadResult = new ConcurrentDictionary<String, Object>();
        /// <summary>
        /// 实例
        /// </summary>
        /// <returns></returns>
        public static EventAction Instance()
        {
            if (Cache.ContainsKey(typeof(EventAction).Name))
            {
                var Instance = Cache.Values.FirstOrDefault();
                Instance.ResponseResult = null;
                return Instance;
            }
            else
            {
                var Instance = new EventAction();
                Cache.Add(Instance.GetType().Name, Instance);
                return Instance;
            }
        }
        /// <summary>
        /// 返回结果
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="Event"></param>
        public void OnResponse(Object Sender, EventArgs Event)
        {
            if ((Event as ResultEvent).ResultCheck)
                ResponseResult = (Sender as ListenEvent).Response;
            else
                ResponseResult = null;
        }
        /// <summary>
        /// 事件结果
        /// </summary>
        public Dictionary<String, Object> DelegateResult => IsNull();
        /// <summary>
        /// 处理空结果
        /// </summary>
        /// <returns></returns>
        protected Dictionary<String, Object> IsNull()
        {
            if (ResponseResult == null)
            {
                Thread.Sleep(100);
                return IsNull();
            }
            else
            {
                Dispose();
                var Key = ResponseResult["ServerDate"].ToString();
                //判断有这个重复键则
                if (ThreadResult.ContainsKey(Key))
                {
                    Thread.Sleep(100);
                    return IsNull();
                }
                else
                {
                    ResponseResult["ResultData"] = ResponseResult["ResultData"]?.ToString().ToModel<Object>();
                    ThreadResult.GetOrAdd(Key, Key);
                    return ResponseResult;
                }
            }
        }
        /// <summary>
        /// 每次满足100则清理
        /// </summary>
        protected void Dispose() {
            if (ThreadResult.Count > 50)
                ThreadResult.Clear();
        }
    }
}
