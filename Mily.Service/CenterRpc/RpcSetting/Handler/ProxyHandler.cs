using Mily.Service.CenterRpc.RpcSetting.Result;
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
            return EachProxy(Provider.ObjectProvider.ToJson().ToModel<ServerKey>(), Provider);
        }
        /// <summary>
        /// 遍历结果
        /// </summary>
        /// <param name="Key"></param>
        /// <param name="Provider"></param>
        /// <returns></returns>
        private static ResultProvider EachProxy(ServerKey Key, ResultProvider Provider)
        {
            switch (Key.NetType)
            {
                case NetTypeEnum.Connect:
                    return ResultProvider.SetValue(ServerKey.SetValue(NetTypeEnum.Connect, Key.ServName), ServerValue.SetStrValue(ServerValue.RegistSuccessful, "OK"));
                case NetTypeEnum.Listened:
                    return Provider;
                case NetTypeEnum.DisConnect:
                    return ResultProvider.SetValue(ServerKey.SetValue(NetTypeEnum.DisConnect, Key.ServName), ServerValue.SetStrValue(ServerValue.ListenedFailed, "FAIL"));
                case NetTypeEnum.CallBack:
                    ResultEvent.StaticResult = Provider.DictionaryStringProvider;
                    ResultEvent.Event.CacheResult(Key.ServName, Provider.DictionaryStringProvider);
                    return ResultProvider.SetValue(ServerKey.SetValue(NetTypeEnum.CallBack, Key.ServName), ServerValue.SetStrValue(ServerValue.CallBackSuccessful, "CALL"));
                default:
                    return ResultProvider.SetValue(ServerKey.SetValue(NetTypeEnum.Listened, Key.ServName), ServerValue.SetStrValue(ServerValue.ListenedSuccessful, "OK"));
            }
        }
    }
}
