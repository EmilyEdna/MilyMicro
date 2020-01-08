using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace Mily.Service.CenterRpc.RpcSetting.DelegateEvent
{
    public class ListenResult
    {
        #region Event
        /// <summary>
        /// 事件
        /// </summary>
        public event ResultEventHandler Changed;
       /// <summary>
       /// 委托事件
       /// </summary>
       /// <param name="Sender"></param>
       /// <param name="Args"></param>
        public delegate void ResultEventHandler(object Sender, EventArgs Args);
        #endregion

        #region Property
        /// <summary>
        /// 保证是同一个对象
        /// </summary>
        private static readonly Dictionary<String,ListenResult> Cache = new Dictionary<String, ListenResult>();
        private Dictionary<String, Object> Result;
        /// <summary>
        /// 结果
        /// </summary>
        public Dictionary<String, Object> Response
        {
            get { return Result; }
            set { Result = value; OnChanged(new ResultEvent(value)); }
        }
        #endregion

        #region Instance
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <returns></returns>
        public static ListenResult Instance()
        {
            if (Cache.ContainsKey(typeof(ListenResult).Name)) return Cache.Values.FirstOrDefault();
            else {
                var Instance = new ListenResult();
                Cache.Add(Instance.GetType().Name, Instance);
                return Instance;
            }
        }
        #endregion

        /// <summary>
        /// 事件
        /// </summary>
        /// <param name="Args"></param>
        protected virtual void OnChanged(EventArgs Args)
        {
            Changed?.Invoke(this, Args);
        }
    }
}
