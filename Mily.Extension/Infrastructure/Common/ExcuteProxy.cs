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
        public void StarExcute()
        {
            Console.WriteLine("开始执行：" + DateTime.Now);
        }
        /// <summary>
        /// 正在执行
        /// </summary>
        /// <param name="Dynamic"></param>
        /// <returns></returns>
        public abstract Task<Object> OnExcute(dynamic Dynamic);
        /// <summary>
        /// 执行结束
        /// </summary>
        public void EndExcute()
        {
            Console.WriteLine("执行结束" + DateTime.Now);
        }
    }
}
