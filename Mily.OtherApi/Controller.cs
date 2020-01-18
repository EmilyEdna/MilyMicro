using Microsoft.AspNetCore.Mvc;
using Mily.Extension.AutofacIoc;
using Mily.Extension.ClientRpc.RpcSetting.Handler;
using Mily.OhterLogic.LogicInterface;
using Mily.Socket.SocketDependency;

namespace Mily.OtherApi
{
    public abstract class Controller : ControllerBase, IClientService,ISocketDependency
    {
       public  IOtherLogic OtherService = AutofocManage.CreateInstance().Resolve<IOtherLogic>();
    }
}