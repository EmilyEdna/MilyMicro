using BeetleX.EventArgs;
using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Data;
using Mily.Service.ViewSetting;
using Mily.Service.ViewSetting.ApiSettting;
using Mily.Service.ViewSetting.SocketSetting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Mily.Service.SocketServ
{
    [ActionFilter]
    [Options(AllowOrigin = "*")]
    [Controller(BaseUrl = "/Condition")]
    public class SocketCoditionApi
    {
        #region InitApi
        [NotAction]
        public static void NetApiServProvider()
        {
            HttpApiServer ApiServ = new HttpApiServer();
            ApiServ.Register(typeof(SocketCoditionApi).Assembly);
            ApiServ.Options.LogLevel = LogType.Warring;
            ApiServ.Options.Host = Configuration.SOCKET_Host;
            ApiServ.Options.Port = Configuration.SOCKET_Port;
            ApiServ.Options.StaticResourcePath = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + "UpLoadFile";
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
        #endregion
        #region Form提交方式或者Byte流方式
        /// <summary>
        /// Byte流或者表单方式
        /// </summary>
        /// <param name="Context">上下文</param>
        /// <returns></returns>
        [Post]
        [NoDataConvert]
        public async Task<Object> FormAsync(IHttpContext Context)
        {
            String Body = Context.Request.Stream.ReadString(Context.Request.Length);
            Dictionary<String, Object> MapDate = new Dictionary<String, Object>();
            String RequestPath = String.Empty;
            Body.Split("&").ToList().ForEach(item =>
            {
                String Key = item.Split("=")[0];
                String Data = item.Split("=")[1];
                if (!Key.Equals("RequestPath"))
                    MapDate.Add(Key, HttpUtility.UrlEncode(Data));
                else
                    RequestPath = Data;
            });
            return await JsonAsync(RequestPath, MapDate);
        }
        /// <summary>
        /// Byte流或者表单方式
        /// </summary>
        /// <param name="Context">上下文</param>
        /// <returns></returns>
        [Post]
        [NoDataConvert]
        public Object Form(IHttpContext Context)
        {
            String Body = Context.Request.Stream.ReadString(Context.Request.Length);
            Dictionary<String, Object> MapDate = new Dictionary<String, Object>();
            String RequestPath = String.Empty;
            Body.Split("&").ToList().ForEach(item =>
            {
                String Key = item.Split("=")[0];
                String Data = item.Split("=")[1];
                if (!Key.Equals("RequestPath"))
                    MapDate.Add(Key, HttpUtility.UrlEncode(Data));
                else
                    RequestPath = Data;
            });
            return Json(RequestPath, MapDate);
        }
        #endregion
        #region AJAX提交方式
        /// <summary>
        /// JSON请求方式
        /// </summary>
        /// <param name="RequestPath">请求地址格式如下Controller_FunctionName</param>
        /// <param name="MapData">请求参数格式如下Data:{Key:key,Name:name}</param>
        /// <returns></returns>
        [Post]
        [JsonDataConvert]
        public async Task<Object> JsonAsync(String RequestPath, Dictionary<String, Object> MapData)
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
        /// <summary>
        /// JSON请求方式
        /// </summary>
        /// <param name="RequestPath">请求地址格式如下Controller_FunctionName</param>
        /// <param name="MapData">请求参数格式如下Data:{Key:key,Name:name}</param>
        /// <returns></returns>
        [Post]
        [JsonDataConvert]
        public Object Json(String RequestPath, Dictionary<String, Object> MapData)
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
        #endregion
        #region 文件上传
        /// <summary>
        /// 文件上传
        /// </summary>
        /// <param name="DirName">目录名称</param>
        /// <param name="Context">上下文</param>
        /// <returns></returns>
        [Post]
        [MultiDataConvert]
        public async Task<Object> UploadFile(String DirName, IHttpContext Context)
        {
            Char Separator = Path.DirectorySeparatorChar;
            String Bin = $"{Directory.GetCurrentDirectory() + Separator}UpLoadFile{Separator + DirName + DateTime.Now.ToString("yyyyMMdd") + Separator}";
            List<String> Paths = new List<String>();
            if (!Directory.Exists(Bin))
                Directory.CreateDirectory(Bin);
            foreach (PostFile Files in Context.Request.Files)
            {
                using (Stream stream = File.Create(Bin + Files.FileName))
                {
                    await Files.Data.CopyToAsync(stream);
                }
                Paths.Add($"{Separator}UpLoadFile{Separator + DirName + DateTime.Now.ToString("yyyyMMdd") + Separator + Files.FileName}");
            }
            return Paths;
        }
        #endregion
    }
}
