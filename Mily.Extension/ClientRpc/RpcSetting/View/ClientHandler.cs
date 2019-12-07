using BeetleX.Clients;
using System;
using System.Collections.Generic;
using System.Text;
using XExten.Common;
using XExten.XCore;

namespace Mily.Extension.ClientRpc.RpcSetting.View
{
    public class ClientHandler
    {
        /// <summary>
        /// 执行数据
        /// </summary>
        /// <param name="Provider"></param>
        /// <returns></returns>
        public static ResultProvider Invoke(ResultProvider Provider)
        {
            return null;
        }
        /// <summary>
        /// 接收数据后回传
        /// </summary>
        /// <param name="ClientAsync"></param>
        /// <param name="Provider"></param>
        /// <returns></returns>
        public static AsyncTcpClient SendInvoke(AsyncTcpClient ClientAsync, ResultProvider Provider)
        {
            NetTypeEnum TypeEnum = Provider.ObjectProvider.ToJson().ToModel<ClientKey>().NetType;
            if (TypeEnum == NetTypeEnum.Listened)
                return ClientAsync.Send(Provider);
            return null;
        }
    }
}
