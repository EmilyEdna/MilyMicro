using Microsoft.AspNetCore.Mvc;
using Mily.Extension.AutofacIoc;
using Mily.Extension.ClientRpc.RpcSetting.Handler;
using Mily.MainLogic.LogicInterface;

namespace Miliy.MainApi
{
    public class BaseApiController : ControllerBase, IClientService
    {
        public IMainLogic SysService = AutofocManage.CreateInstance().Resolve<IMainLogic>();
    }
}