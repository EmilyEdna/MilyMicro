using Microsoft.AspNetCore.Mvc;
using Mily.Extension.AutofacIoc;
using Mily.Extension.ClientRpc.RpcSetting.Handler;
using Mily.Extension.WebClientCore.MainApi;
using Mily.OhterLogic.LogicInterface;
using Mily.Socket.SocketInterface;

namespace Mily.OtherApi
{
    public abstract class Controller : ControllerBase, IClientGateWayService, ISocketDependency
    {
        public IOtherLogic OtherService = AutofocManage.CreateInstance().Resolve<IOtherLogic>();
        public IMainApi MainApi = AutofocManage.CreateInstance().Resolve<IMainApi>();
    }
}