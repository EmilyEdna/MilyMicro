using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Polly;

namespace Mily.Extension.Retry
{
    public class RetryFallBack
    {
        /// <summary>
        /// 回退无返回
        /// </summary>
        /// <param name="action"></param>
        public static void DoRetry(Action action)
        {
            Policy.Handle<Exception>().Fallback((Context) =>
            {

            }, (Ex, Context) =>
            {
                Console.WriteLine($"异常信息：{Ex.Message}，错误方法：{Context["MethodName"]}");
            }).Execute(action);
        }
        /// <summary>
        /// 回退有返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <returns></returns>
        public static T DoRetry<T>(Func<T> action)
        {
            return Policy.Handle<Exception>().Fallback((Context) =>
             {

             }, (Ex, Context) =>
             {
                 Console.WriteLine($"异常信息：{Ex.Message}，错误方法：{Context["MethodName"]}");
             }).Execute(action);
        }
    }
}
