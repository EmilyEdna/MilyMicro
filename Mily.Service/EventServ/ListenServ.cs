using Mily.Service.CenterApi;
using Mily.Service.Shell;
using Mily.Service.SocketServ;
using Mily.Service.ViewSetting;
using PeterKottas.DotNetCore.WindowsService.Base;
using PeterKottas.DotNetCore.WindowsService.Interfaces;

namespace Mily.Service.EventServ
{
    public class ListenServ : MicroService, IMicroService
    {
        public void Start()
        {
            //初始化链接
            Configuration.InitConnection();
            //激活TCP服务端
            // SocketCodition.NetServProvider();
            //激活Api
            //SocketCoditionApi.NetApiServProvider();
            NetApiServProvider.InitApiProvider();
            //创建powershell指令
            BatCondition.CreateShellCondition();
        }

        public void Stop()
        {
        }
    }
}