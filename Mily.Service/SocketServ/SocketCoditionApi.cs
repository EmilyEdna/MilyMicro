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
        /// <Param name="Context">上下文</Param>
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
                //表单数据
                if (!Key.Equals("RequestPath"))
                    MapDate.Add(Key, HttpUtility.UrlEncode(Data));
                else
                    //请求地址
                    RequestPath = Data;
            });
            return await JsonAsync(RequestPath, MapDate);
        }
        /// <summary>
        /// Byte流或者表单方式
        /// </summary>
        /// <Param name="Context">上下文</Param>
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
                //表单数据
                if (!Key.Equals("RequestPath"))
                    MapDate.Add(Key, HttpUtility.UrlEncode(Data));
                else
                    //请求地址
                    RequestPath = Data;
            });
            return Json(RequestPath, MapDate);
        }
        #endregion
        #region AJAX提交方式
        /// <summary>
        /// JSON请求方式
        /// </summary>
        /// <Param name="RequestPath">请求地址格式如下Controller_FunctionName</Param>
        /// <Param name="MapData">请求参数格式如下Data:{Key:key,Name:name}</Param>
        /// <returns></returns>
        [Post]
        [JsonDataConvert]
        public async Task<Object> JsonAsync(String RequestPath, Dictionary<String, Object> MapData)
        {
            ParamCmd Param = new ParamCmd
            {
                Path = RequestPath,
                HashData = MapData
            };
            ParseCmd.HashData = JsonConvert.SerializeObject(Param);
            SocketCodition.Boots = NetType.Listen;
            SessionReceiveEventArgs SessionEvent = SocketCodition.Session[Param.Service.ToUpper()].Values.FirstOrDefault();
            SocketCodition.KeyId= SocketCodition.Session[Param.Service.ToUpper()].Keys.FirstOrDefault();
            SessionEvent.Session.Server.Handler.SessionReceive(SessionEvent.Server, SessionEvent);
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
        /// <Param name="RequestPath">请求地址格式如下Controller_FunctionName</Param>
        /// <Param name="MapData">请求参数格式如下Data:{Key:key,Name:name}</Param>
        /// <returns></returns>
        [Post]
        [JsonDataConvert]
        public Object Json(String RequestPath, Dictionary<String, Object> MapData)
        {
            ParamCmd Param = new ParamCmd
            {
                Path = RequestPath,
                HashData = MapData
            };
            ParseCmd.HashData = JsonConvert.SerializeObject(Param);
            SocketCodition.Boots = NetType.Listen;
            SessionReceiveEventArgs SessionEvent = SocketCodition.Session[Param.Controller.ToUpper()].Values.FirstOrDefault();
            SocketCodition.KeyId = SocketCodition.Session[Param.Controller.ToUpper()].Keys.FirstOrDefault();
            SessionEvent.Session.Server.Handler.SessionReceive(SessionEvent.Server, SessionEvent);
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
        /// <Param name="DirName">目录名称</Param>
        /// <Param name="Context">上下文</Param>
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
