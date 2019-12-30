using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mily.Extension.Infrastructure.Common
{
    public abstract class ExcuteProxy
    {
        /// <summary>
        /// 开始执行
        /// </summary>
        /// <param name="BeginAction"></param>
        public void StarExcute(Action BeginAction = null)
        {
            if (BeginAction != null) BeginAction.Invoke();
            Console.WriteLine("开始执行：" + DateTime.Now);
        }
        /// <summary>
        /// 正在执行
        /// </summary>
        /// <param name="Dynamic"></param>
        /// <returns></returns>
        public abstract Object OnExcute(dynamic Dynamic);
        /// <summary>
        /// 执行结束
        /// </summary>
        /// <param name="EndAction"></param>
        public void EndExcute(Action EndAction = null)
        {
            if (EndAction != null) EndAction.Invoke();
            Console.WriteLine("执行结束" + DateTime.Now);
        }
    }
}
