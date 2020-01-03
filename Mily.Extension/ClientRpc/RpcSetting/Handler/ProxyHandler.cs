using Mily.Extension.ClientRpc.RpcSetting.Event;
using Mily.Extension.ClientRpc.RpcSetting.View;
using System;
using XExten.Common;
using XExten.XCore;

namespace Mily.Extension.ClientRpc.RpcSetting.Handler
{
    /// <summary>
    /// 代理中心
    /// </summary>
    public class ProxyHandler
    {
        public static ProxyHandler Instance => new ProxyHandler();
        /// <summary>
        /// 处理数据集
        /// </summary>
        /// <param name="Provider"></param>
        /// <returns></returns>
        public virtual ResultProvider InitProxy(ResultProvider Provider)
        {
            return EachProxy(Provider.ObjectProvider.ToJson().ToModel<ClientKey>(),Provider);
        }
        /// <summary>
        /// 遍历结果
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Provider"></param>
        /// <returns></returns>
        internal ResultProvider EachProxy(ClientKey Key, ResultProvider Provider)
        {
            return Key.NetType switch
            {
                NetTypeEnum.Connect => null,
                NetTypeEnum.Listened => ClientEvnet.Instance.Invoke(Provider),
                NetTypeEnum.DisConnect => ResultProvider.SetValue(ClientKey.SetValue(NetTypeEnum.DisConnect, Key.ServName),
                ClientValue.SetStrValue(ClientValue.ListenedFailed, "FAIL")),
                _ => ResultProvider.SetValue(ClientKey.SetValue(NetTypeEnum.Listened, Key.ServName),
                ClientValue.SetStrValue(ClientValue.ListenedSuccessful, "Ok")),
            };
        }
    }
}

