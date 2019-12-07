using BeetleX.Clients;
using Mily.Setting;
using System;
using System.Linq;
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
        public static ResultProvider Invoke(ResultProvider Provider,Type BaseType)
        {
            String[] ControllerAndMethod = Provider.DictionaryStringProvider["TargetPath"].ToString().Split("|");
            String Controller = ControllerAndMethod.FirstOrDefault();
            String Method = ControllerAndMethod.LastOrDefault();
            Type Control = MilyConfig.Assembly.SelectMany(t => t.ExportedTypes.Where(x => x.BaseType == BaseType))
                .Where(t => t.Name.Contains(Controller)).FirstOrDefault();

            //ControllerAndMethod[0]
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
