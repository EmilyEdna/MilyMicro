using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mily.Socket.SocketHandle
{
    public class CallHandleEvent
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
        private static readonly Dictionary<String, CallHandleEvent> Cache = new Dictionary<String, CallHandleEvent>();
        private Dictionary<String, Object> Result;
        /// <summary>
        /// 结果
        /// </summary>
        public Dictionary<String, Object> Response
        {
            get { return Result; }
            set { Result = value; OnChanged(new CallHandleResultEvent(value)); }
        }
        #endregion

        #region Instance
        /// <summary>
        /// 创建对象
        /// </summary>
        /// <returns></returns>
        public static CallHandleEvent Instance()
        {
            if (Cache.ContainsKey(typeof(CallHandleEvent).Name)) return Cache.Values.FirstOrDefault();
            else
            {
                var Instance = new CallHandleEvent();
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
