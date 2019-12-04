using BeetleX.EventArgs;
using BeetleX.FastHttpApi;
using Mily.Service.ViewSetting;
using System;
using System.IO;

namespace Mily.Service.CenterApi
{
    public class NetApiServProvider
    {
        public static void InitApiProvider() {
            HttpApiServer ApiServ = new HttpApiServer();
            ApiServ.Register(typeof(NetApiServProvider).Assembly);
            ApiServ.Options.LogLevel = LogType.Warring;
            ApiServ.Options.Host = Configuration.SOCKET_Host;
            ApiServ.Options.Port = Configuration.SOCKET_Port;
            ApiServ.Options.StaticResourcePath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "Views";
            ApiServ.Options.SetDebug();
            ApiServ.ServerLog = (Server, Event) =>
            {
                if (Event.Type == LogType.Error)
                    if (Event.Message.Contains("http"))
                    {
                        String ExceptionInfomations = $"Service errored with exception：【{Event.Message}】====write time：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}\n";
                        File.AppendAllText(Path.Combine(AppContext.BaseDirectory, "FastApiError.log"), ExceptionInfomations);
                    }
            };
            ApiServ.Options.OutputStackTrace = true;
            ApiServ.Open();
            ApiServ.HttpRequestNotfound += (o, e) =>
            {
                e.Cancel = true;
                e.Response.SetStatus("404", "Not Found");
                e.Response.Result("404 Not Found");
            };
        }
    }
}
