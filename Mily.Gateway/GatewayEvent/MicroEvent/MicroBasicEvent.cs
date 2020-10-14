using Mily.Gateway.GatewayBasic;
using Mily.Gateway.GatewayService.ShellServcie;
using Mily.Gateway.GatewaySetting;
using PeterKottas.DotNetCore.WindowsService.Base;
using PeterKottas.DotNetCore.WindowsService.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Gateway.GatewayEvent.MicroEvent
{
    public class MicroBasicEvent : MicroService, IMicroService
    {
        public void Start()
        {
            //初始化链接
            Configuration.InitConnection();
            //初始化服务中心API
            BootstrapNet.Bootstrap();
            //创建powershell指令
            BatCondition.CreateShellCondition();
            //初始化Socket服务
            BootstrapSocket.Bootstrap();
        }

        public void Stop()
        {
            Environment.Exit(1);
        }
    }
}
