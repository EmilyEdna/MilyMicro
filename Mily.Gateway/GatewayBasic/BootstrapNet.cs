using BeetleX.EventArgs;
using BeetleX.FastHttpApi;
using Mily.Gateway.GatewaySetting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mily.Gateway.GatewayBasic
{
    /// <summary>
    /// 服务中心
    /// </summary>
    public class BootstrapNet
    {
        /// <summary>
        /// 启动API服务
        /// </summary>
        public static void Bootstrap() {
            HttpApiServer ApiServ = new HttpApiServer();
            ApiServ.Register(typeof(BootstrapNet).Assembly);
            ApiServ.Options.LogLevel = LogType.Warring;
            ApiServ.Options.Host = Configuration.SOCKET_Host;
            ApiServ.Options.Port = Configuration.SOCKET_Port;
            ApiServ.Options.SetDebug();//调式用
            ApiServ.Options.AddFilter<DefaultJsonResultFilter>();
            ApiServ.ServerLog += (Server, Event) =>
            {
                if (Event.Type == LogType.Error)
                    if (Event.Message.Contains("http") || Event.Message.Contains("https"))
                    {
                        String ExceptionInfomations = $"Service errored with exception：【{Event.Message}】====write time：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\n";
                        File.AppendAllText(Path.Combine(AppContext.BaseDirectory, "SocketError.log"), ExceptionInfomations);
                        Console.WriteLine(ExceptionInfomations);
                    }
            };
            ApiServ.Options.OutputStackTrace = true;
            ApiServ.Open();
            ApiServ.HttpRequestNotfound += (Obj, Event) =>
            {
                Event.Cancel = true;
                Event.Response.SetStatus("404", "Not Found");
                Event.Response.Result("404 Not Found");
            };
        }
    }
}
