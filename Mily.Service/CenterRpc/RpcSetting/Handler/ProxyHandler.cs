using System;
using System.Collections.Generic;
using System.Text;
using XExten.Common;

namespace Mily.Service.CenterRpc.RpcSetting.Handler
{
    /// <summary>
    /// 代理中心
    /// </summary>
    public class ProxyHandler
    {
        /// <summary>
        /// 处理数据集
        /// </summary>
        /// <param name="Provider"></param>
        public static ResultProvider InitProxy(ResultProvider Provider)
        {
            return EachProxy((NetTypeEnum)Convert.ToInt32(Provider.ObjectProvider));
        }
        /// <summary>
        /// 遍历结果
        /// </summary>
        /// <param name="Net"></param>
        /// <returns></returns>
        private static ResultProvider EachProxy(NetTypeEnum Net)
        {
            return Net switch
            {
                NetTypeEnum.Connect => ResultProvider.SetValue(NetTypeEnum.Listened, new Dictionary<Object, Object> { { "RegistSuccessful", "OK" } }),
                NetTypeEnum.Listened => ResultProvider.SetValue(NetTypeEnum.Listened, new Dictionary<Object, Object> { { "ListenedSuccessful", "OK" } }),
                NetTypeEnum.DisConnect => ResultProvider.SetValue(NetTypeEnum.DisConnect, new Dictionary<Object, Object> { { "ListenedFailed", "FAILED" } }),
                _ => ResultProvider.SetValue(NetTypeEnum.Listened, new Dictionary<Object, Object> { { "ListenedSuccessful", "OK" } }),
            };
        }
    }
}
