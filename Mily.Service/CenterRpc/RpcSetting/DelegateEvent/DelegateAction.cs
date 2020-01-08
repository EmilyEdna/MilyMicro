using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Linq;

namespace Mily.Service.CenterRpc.RpcSetting.DelegateEvent
{
    /// <summary>
    /// 委托返回
    /// </summary>
    public class DelegateAction
    {
        private static readonly Dictionary<String, DelegateAction> Cache = new Dictionary<string, DelegateAction>();
        /// <summary>
        /// 实例
        /// </summary>
        /// <returns></returns>
        public static DelegateAction Instance()
        {
            if (Cache.ContainsKey(typeof(DelegateAction).Name))
            {
                var Instance = Cache.Values.FirstOrDefault();
                Instance.ResponseResult = null;
                return Instance;
            }
            else
            {
                var Instance = new DelegateAction();
                Cache.Add(Instance.GetType().Name, Instance);
                return Instance;
            }
        }
        private Dictionary<String, Object> ResponseResult;
        /// <summary>
        /// 返回结果
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="Event"></param>
        public void OnResponse(Object Sender, EventArgs Event)
        {
            if ((Event as ResultEvent).ResultCheck)
                ResponseResult = (Sender as ListenResult).Response;
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
