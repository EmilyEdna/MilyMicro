using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Service.CenterRpc.RpcSetting.DelegateEvent
{
    /// <summary>
    /// 结果事件
    /// </summary>
    public class ResultEvent : EventArgs
    {
        private Dictionary<String, Object> Result;
        public ResultEvent(Dictionary<String, Object> Param)
        {
            Result = Param;
        }
        /// <summary>
        /// 结果检查
        /// </summary>
        internal bool ResultCheck
        {
            get
            {
                if (Result == null) return false;
                if (Result.Count == 0) return false;
                return true;
            }
        }
    }
}
