using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XExten.Common;
using XExten.XCore;

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
            return EachProxy(Provider.ObjectProvider.ToJson().ToModel<ServerKey>());
        }
        /// <summary>
        /// 遍历结果
        /// </summary>
        /// <param name="Net"></param>
        /// <returns></returns>
        private static ResultProvider EachProxy(ServerKey Key)
        {
            return Key.NetType switch
            {
                NetTypeEnum.Connect => ResultProvider.SetValue(ServerKey.SetValue(NetTypeEnum.Connect, Key.ServName),
                new Dictionary<Object, Object> { { "RegistSuccessful", "OK" } }),
                NetTypeEnum.Listened => ResultProvider.SetValue(ServerKey.SetValue(NetTypeEnum.Listened, Key.ServName),
                new Dictionary<Object, Object> { { "ListenedSuccessful", "OK" } }),
                NetTypeEnum.DisConnect => ResultProvider.SetValue(ServerKey.SetValue(NetTypeEnum.DisConnect, Key.ServName),
                new Dictionary<Object, Object> { { "ListenedFailed", "FAILED" } }),
                _ => ResultProvider.SetValue(ServerKey.SetValue(NetTypeEnum.Listened, Key.ServName),
                new Dictionary<Object, Object> { { "ListenedSuccessful", "OK" } }),
            };
        }
    }
}
