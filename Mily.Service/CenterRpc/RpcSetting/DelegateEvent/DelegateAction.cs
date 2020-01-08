using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Mily.Service.CenterRpc.RpcSetting.DelegateEvent
{
    /// <summary>
    /// 委托返回
    /// </summary>
    public class DelegateAction
    {
        private static Dictionary<String, Object> _DelegateResult;
        /// <summary>
        /// 返回结果
        /// </summary>
        /// <param name="Sender"></param>
        /// <param name="Event"></param>
        public static void OnResponse(Object Sender, EventArgs Event)
        {
            if ((Event as ResultEvent).ResultCheck)
                _DelegateResult = (Sender as ListenResult).Response;
            else
                _DelegateResult = null;
        }
        /// <summary>
        /// 事件结果
        /// </summary>
        public static Dictionary<String, Object> DelegateResult => IsNull();
        /// <summary>
        /// 处理空结果
        /// </summary>
        /// <returns></returns>
        protected static Dictionary<String, Object> IsNull()
        {
            if (_DelegateResult == null)
            {
                Thread.Sleep(100);
                return IsNull();
            }
            else
                return _DelegateResult;
        }
    }
}
