using Microsoft.AspNetCore.Mvc;
using Mily.Extension.AutofacIoc;
using Mily.Extension.ClientRpc.RpcSetting.Handler;
using Mily.OhterLogic.LogicInterface;

namespace Mily.OtherApi
{
    public abstract class Controller : ControllerBase, IClientService
    {
       public  IOtherLogic OtherService = AutofocManage.CreateInstance().Resolve<IOtherLogic>();
    }
}