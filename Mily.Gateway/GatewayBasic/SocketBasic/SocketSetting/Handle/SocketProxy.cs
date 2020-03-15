using Mily.Gateway.GatewayBasic.SocketBasic.SocketSetting.Result;
using Mily.Gateway.GatewayBasic.SocketBasic.SocketSetting.View;
using Mily.Gateway.GatewayEvent.SocketEvent;
using XExten.Common;
using XExten.XCore;

namespace Mily.Gateway.GatewayBasic.SocketBasic.SocketSetting.Handle
{
    /// <summary>
    /// 代理中心
    /// </summary>
    public class SocketProxy
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
                case SocketTypeEnum.Connect:
                    return ResultProvider.SetValue(ServerKey.SetValue(SocketTypeEnum.Connect, Key.ServName), ServerValue.SetStrValue(ServerValue.RegistSuccessful, "OK"));
                case SocketTypeEnum.Listened:
                    return Provider;
                case SocketTypeEnum.DisConnect:
                    return ResultProvider.SetValue(ServerKey.SetValue(SocketTypeEnum.DisConnect, Key.ServName), ServerValue.SetStrValue(ServerValue.ListenedFailed, "FAIL"));
                case SocketTypeEnum.CallBack:
                    ListenEvent.Instance().Response = Provider.DictionaryStringProvider;
                    return ResultProvider.SetValue(ServerKey.SetValue(SocketTypeEnum.CallBack, Key.ServName), ServerValue.SetStrValue(ServerValue.CallBackSuccessful, "CALL"));
                default:
                    return ResultProvider.SetValue(ServerKey.SetValue(SocketTypeEnum.Listened, Key.ServName), ServerValue.SetStrValue(ServerValue.ListenedSuccessful, "OK"));
            }
        }

    }
}
