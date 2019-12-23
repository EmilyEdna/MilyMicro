using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Polly;

namespace Mily.Extension.Retry
{
    public class RetryBreaker
    {
        /// <summary>
        /// 短路由无返回
        /// </summary>
        /// <param name="action"></param>
        /// <param name="Times"></param>
        /// <param name="Seconds"></param>
        public static void DoRetry(Action action, int Times = 3, int Seconds = 60)
        {
            Policy.Handle<Exception>().CircuitBreaker(Times, TimeSpan.FromSeconds(Seconds)).Execute(action);
        }
        /// <summary>
        /// 短路由有返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="Times"></param>
        /// <param name="Seconds"></param>
        /// <returns></returns>
        public static T DoRetry<T>(Func<T> action, int Times = 3, int Seconds = 60)
        {
            return Policy.Handle<Exception>().CircuitBreaker(Times, TimeSpan.FromSeconds(Seconds)).Execute(action);
        }
        /// <summary>
        /// 短路由无返回
        /// </summary>
        /// <param name="action"></param>
        /// <param name="Times"></param>
        /// <param name="Seconds"></param>
        /// <returns></returns>
        public static async Task DoRetryAsync(Func<Task> action, int Times = 3, int Seconds = 60)
        {
            await Policy.Handle<Exception>().CircuitBreakerAsync(Times, TimeSpan.FromSeconds(Seconds)).ExecuteAsync(action);
        }
        /// <summary>
        /// 短路由有返回
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action"></param>
        /// <param name="Times"></param>
        /// <param name="Seconds"></param>
        /// <returns></returns>
        public static async Task<T> DoRetryAsync<T>(Func<Task<T>> action, int Times = 3, int Seconds = 60)
        {
            return await Policy.Handle<Exception>().CircuitBreakerAsync(Times, TimeSpan.FromSeconds(Seconds)).ExecuteAsync(action);
        }
    }
}
