using Microsoft.AspNetCore.Mvc;
using Mily.Extension.AutofacIoc;
using Mily.Extension.ClientRpc.RpcSetting.Handler;
using Mily.MainLogic.LogicInterface;
using Mily.Socket.SocketInterface;

namespace Miliy.MainApi
{
    public abstract class Controller : ControllerBase, IClientGateWayService, ISocketDependency
    {
        public IMainLogic SysService = AutofocManage.CreateInstance().Resolve<IMainLogic>();

    }
}