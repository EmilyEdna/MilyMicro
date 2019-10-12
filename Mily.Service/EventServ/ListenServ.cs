using Mily.Service.SocketServ;
using PeterKottas.DotNetCore.WindowsService.Base;
using PeterKottas.DotNetCore.WindowsService.Interfaces;

namespace Mily.Service.EventServ
{
    public class ListenServ : MicroService, IMicroService
    {
        public void Start()
        {
            //激活TCP服务端
            SocketCodition.NetServProvider();
            //激活Api
            SocketCoditionApi.NetApiServProvider();
        }

        public void Stop()
        {
        }
    }
}