using Mily.Extension.ClientRpc.RpcSetting.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XExten.Common;
using XExten.XCore;

namespace Mily.Extension.ClientRpc.RpcSetting.Handler
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
            return EachProxy(Provider.ObjectProvider.ToJson().ToModel<ClientKey>());
        }
        /// <summary>
        /// 遍历结果
        /// </summary>
        /// <param name="Net"></param>
        /// <returns></returns>
        private static ResultProvider EachProxy(ClientKey Key)
        {
            return Key.NetType switch
            {
                NetTypeEnum.Connect => ResultProvider.SetValue(ClientKey.SetValue(NetTypeEnum.Listened, Key.ServName),
                new Dictionary<Object, Object> { { "RegistSuccessful", "OK" } }),
                NetTypeEnum.Listened => ResultProvider.SetValue(ClientKey.SetValue(NetTypeEnum.Listened, Key.ServName),
                new Dictionary<Object, Object> { { "ListenedSuccessful", "OK" } }),
                NetTypeEnum.DisConnect => ResultProvider.SetValue(ClientKey.SetValue(NetTypeEnum.DisConnect, Key.ServName),
                new Dictionary<Object, Object> { { "ListenedFailed", "FAILED" } }),
                _ => ResultProvider.SetValue(ClientKey.SetValue(NetTypeEnum.Listened, Key.ServName),
                new Dictionary<Object, Object> { { "ListenedSuccessful", "OK" } }),
            };
        }
    }
}

