using System;
using System.Collections.Generic;
using System.Text;
using Polly;

namespace Mily.Extension.Retry
{
    public class RetryException
    {
        /// <summary>
        ///  无返回重试
        /// </summary>
        /// <param name="Times"></param>
        /// <returns></returns>
        public static void DoRetry(Action action, int Times = 3)
        {
            Policy.Handle<Exception>().Retry(Times, (Ex, Count, Context) =>
            {
                Console.WriteLine($"重试次数{Count}，异常{Ex.Message}");
            }).Execute(action);
        }
        /// <summary>
        /// 有返回重试
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="Times"></param>
        /// <returns></returns>
        public static T DoRetry<T>(Func<T> action, int Times = 3)
        {
            return Policy.Handle<Exception>().Retry(Times, (Ex, Count, Context) =>
             {
                 Console.WriteLine($"重试次数{Count}，异常{Ex.Message}");
             }).Execute(action);
        }
    }
}
