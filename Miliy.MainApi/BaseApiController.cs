using Microsoft.AspNetCore.Mvc;
using Mily.Extension.AutofacIoc;
using Mily.MainLogic.LogicInterface;

namespace Miliy.MainApi
{
    public class BaseApiController: ControllerBase
    {
        public IMainLogic SysService = AutofocManage.CreateInstance().Resolve<IMainLogic>();
    }
}
