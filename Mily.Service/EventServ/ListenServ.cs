using Mily.Service.CenterApi;
using Mily.Service.CenterRpc;
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
            //SocketCodition.NetServProvider();
            //激活Api
            //SocketCoditionApi.NetApiServProvider();
            //初始化服务中心API
            NetApiServProvider.InitApiProvider();
            //创建powershell指令
            BatCondition.CreateShellCondition();
            //初始化Rpc服务
            NetRpcServProvider.InitRpcProvider();
        }

        public void Stop()
        {
        }
    }
}