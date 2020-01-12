using Mily.Gateway.GatewayEvent.MicroEvent;
using PeterKottas.DotNetCore.WindowsService;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mily.Gateway.GatewayBasic
{
    internal  class Micro
    {
        public static void Bootstrap() {
            ServiceRunner<MicroBasicEvent>.Run(config =>
            {
                //please see the help link in this here https://github.com/PeterKottas/DotNetCore.WindowsService
                config.SetName("MicroServ");
                config.SetDisplayName("MicroServ");
                config.SetDescription("微服务中间系统,处理TCP、UDP、Socket通信");
                config.SetServiceTimeout(1000);
                string name = config.GetDefaultName();
                config.Service(serviceConfig =>
                {
                    serviceConfig.ServiceFactory((param, ctrl) =>
                    {
                        return new MicroBasicEvent();
                    });
                    serviceConfig.OnStart((self, param) =>
                    {
                        self.Start();
                    });
                    serviceConfig.OnStop(self =>
                    {
                        self.Stop();
                    });
                    serviceConfig.OnContinue(self => { });
                    serviceConfig.OnPause(self => { });
                    serviceConfig.OnShutdown(service => { });
                    serviceConfig.OnError(ex =>
                    {
                        String ExceptionInfomations = $"Service {name} errored with exception：【{ex.ToString()}】====exception info is：【{ex.Message}】====write time：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\n";
                        File.AppendAllText(Path.Combine(AppContext.BaseDirectory, "WindowsService.log"), ExceptionInfomations);
                    });
                });
            });
        }
    }
}
