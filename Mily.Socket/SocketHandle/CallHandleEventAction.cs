using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Mily.Socket.SocketHandle
{
    public class CallHandleEventAction
    {
        private Dictionary<String, Object> ResponseResult;
        private static readonly Dictionary<String, CallHandleEventAction> Cache = new Dictionary<string, CallHandleEventAction>();
        /// <summary>
        /// 实例
        /// </summary>
        /// <returns></returns>
        public static CallHandleEventAction Instance()
        {
            if (Cache.ContainsKey(typeof(CallHandleEventAction).Name))
            {
                var Instance = Cache.Values.FirstOrDefault();
                Instance.ResponseResult = null;
                return Instance;
            }
            else
            {
                var Instance = new CallHandleEventAction();
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
            if ((Event as CallHandleResultEvent).ResultCheck)
                ResponseResult = (Sender as CallHandleEvent).Response;
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
                return ResponseResult;
        }
    }
}
