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
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            if (BeginAction != null) BeginAction.Invoke();
            Console.WriteLine($"开始执行时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}\n");
            Console.ForegroundColor = ConsoleColor.White;
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
            Console.ForegroundColor = ConsoleColor.DarkRed;
            if (EndAction != null) EndAction.Invoke();
            Console.WriteLine($"执行结束时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff")}\n");
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
