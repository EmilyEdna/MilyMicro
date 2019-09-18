using BeetleX;
using BeetleX.EventArgs;
using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Data;
using Mily.Service.ViewSetting;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using Mily.Service.ViewSetting.SocketSetting;

namespace Mily.Service.SocketServ
{
    [Controller(BaseUrl = "/Condition")]
    public class SocketCoditionApi
    {
        public static void NetApiServProvider()
        {
            HttpApiServer ApiServ = new HttpApiServer();
            ApiServ.Register(typeof(SocketCoditionApi).Assembly);
            ApiServ.Options.LogLevel = LogType.Warring;
            ApiServ.Options.Host = Configuration.SOCKET_Host;
            ApiServ.Options.Port = Configuration.SOCKET_Port;
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
        }
        [Post]
        [JsonDataConvert] //ajax用
        public async Task<Object> EventBusAsync(string RequestPath, Dictionary<string, object> MapData)
        {
            ParamCmd param = new ParamCmd
            {
                Path = RequestPath,
                HashData = MapData
            };
            ParseCmd.HashData = JsonConvert.SerializeObject(param);
            SocketCodition.Boots = NetType.Listen;
            foreach (var item in SocketCodition.Session)
            {
                SocketCodition.ResetEvent.Set();
                item.Value.Session.Server.Handler.SessionReceive(item.Value.Server, item.Value);
            }
            return await Task.Run<Object>(() =>
            {
                Thread.Sleep(1500);
                if (SocketCodition.Result.FirstOrDefault() != null)
                    return JsonConvert.DeserializeObject<Object>(SocketCodition.Result.FirstOrDefault());
                else
                    return SocketCodition.Result.FirstOrDefault();
            });
        }
        [Post]
        [JsonDataConvert] //ajax用
        public Object EventBus(string RequestPath, Dictionary<string, object> MapData)
        {
            ParamCmd param = new ParamCmd
            {
                Path = RequestPath,
                HashData = MapData
            };
            ParseCmd.HashData = JsonConvert.SerializeObject(param);
            SocketCodition.Boots = NetType.Listen;
            foreach (var item in SocketCodition.Session)
            {
                SocketCodition.ResetEvent.Set();
                item.Value.Session.Server.Handler.SessionReceive(item.Value.Server, item.Value);
            }
            try
            {
                return JsonConvert.DeserializeObject<Object>(SocketCodition.Result.FirstOrDefault());

            }
            catch (Exception)
            {
                return SocketCodition.Result.FirstOrDefault();
            }
        }
    }
}
