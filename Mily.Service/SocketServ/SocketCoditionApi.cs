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
using XExten.XCore;

namespace Mily.Service.SocketServ
{
    [ActionFilter]
    [Options(AllowOrigin = "http://localhost:52978", AllowHeaders ="Cross")]
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

        #endregion InitApi

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
            Int32 Hit = 0;
            Body.Split("&").ToList().ForEach(item =>
            {
                String Key = item.Split("=")[0];
                String Data = item.Split("=")[1];
                //表单数据
                if (!Key.ToUpper().Equals("REQUESTPATH") && !Key.ToUpper().Equals("HIT"))
                    MapDate.Add(Key, HttpUtility.UrlEncode(Data));
                else if (Key.ToUpper().Equals("REQUESTPATH"))
                    //请求地址
                    RequestPath = Data;
                else
                    Hit = Data.IsNullOrEmpty() ? 100 : Convert.ToInt32(Data);
            });
            return await JsonAsync(Context,RequestPath, MapDate, Hit);
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
            Int32 Hit = 0;
            Body.Split("&").ToList().ForEach(item =>
            {
                String Key = item.Split("=")[0];
                String Data = item.Split("=")[1];
                //表单数据
                if (!Key.ToUpper().Equals("REQUESTPATH") && !Key.ToUpper().Equals("HIT"))
                    MapDate.Add(Key, HttpUtility.UrlEncode(Data));
                else if (Key.ToUpper().Equals("REQUESTPATH"))
                    //请求地址
                    RequestPath = Data;
                else
                    Hit = Data.IsNullOrEmpty() ? 100 : Convert.ToInt32(Data);
            });
            return Json(Context,RequestPath, MapDate, Hit);
        }

        #endregion Form提交方式或者Byte流方式

        #region AJAX提交方式

        /// <summary>
        /// JSON请求方式
        /// </summary>
        /// <param name="Context"></param>
        /// <Param name="RequestPath">请求地址格式如下Controller_FunctionName</Param>
        /// <Param name="MapData">请求参数格式如下Data:{Key:key,Name:name}</Param>
        /// <param name="Hit">负载权重</param>
        /// <returns></returns>
        [Post]
        [JsonDataConvert]
        public async Task<Object> JsonAsync(IHttpContext Context,String RequestPath, Dictionary<String, Object> MapData, Int32 Hit = 100)
        {
            ParamCmd Param = new ParamCmd
            {
                Path = RequestPath,
                HashData = MapData
            };
            ParseCmd.HashData = JsonConvert.SerializeObject(Param);
            SocketCodition.Boots = NetType.Listen;
            if (Hit >= 100) //权重100以上默认单机发送不负载
                HitBalance(Param);
            else //默认双机负载
            {
                Dictionary<Int32, SessionReceiveEventArgs> HitHand = SocketCodition.Session.Where(t => t.Key.Contains(Param.Service.ToUpper())).Select(t => t.Value).FirstOrDefault();
                if (Hit >= 50 && Hit < 99)
                {
                    Random Rdom = new Random(Guid.NewGuid().GetHashCode());
                    HitWeight.Hits = HitHand.Keys.Any(t => t >= 5) ? HitHand.Keys.Select(t => Rdom.Next(4, HitHand.Keys.Select(t => t >= 5).Count() + 1)).FirstOrDefault() : HitHand.Keys.Min();
                    HitWeight.SessionEvnet = HitHand[HitWeight.Hits];
                    var High = new Thread(new ThreadStart(() => HitWeight.SessionEvnet.Session.Server.Handler.SessionReceive(HitWeight.SessionEvnet.Server, HitWeight.SessionEvnet)));
                    var Normol = new Thread(new ThreadStart(() => HitBalance(Param)));
                    High.Priority = ThreadPriority.Highest;
                    Normol.Priority = ThreadPriority.Normal;
                    High.Start();
                    Normol.Start();
                }
                else
                {
                    Random Rdom = new Random(Guid.NewGuid().GetHashCode());
                    HitWeight.Hits = HitHand.Keys.Any(t => t > 1 && t < 5) ? HitHand.Keys.Select(t => Rdom.Next(1, HitHand.Keys.Count + 1)).FirstOrDefault() : HitHand.Keys.Min();
                    HitWeight.SessionEvnet = HitHand[HitWeight.Hits];
                    var High = new Thread(new ThreadStart(() => HitWeight.SessionEvnet.Session.Server.Handler.SessionReceive(HitWeight.SessionEvnet.Server, HitWeight.SessionEvnet)));
                    var Normol = new Thread(new ThreadStart(() => HitBalance(Param)));
                    High.Priority = ThreadPriority.Highest;
                    Normol.Priority = ThreadPriority.Normal;
                    High.Start();
                    Normol.Start();
                }
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
        /// <param name="Context"></param>
        /// <Param name="RequestPath">请求地址格式如下Controller_FunctionName</Param>
        /// <Param name="MapData">请求参数格式如下Data:{Key:key,Name:name}</Param>
        /// <param name="Hit">负载权重</param>
        /// <returns></returns>
        [Post]
        [JsonDataConvert]
        public Object Json(IHttpContext Context,String RequestPath, Dictionary<String, Object> MapData, Int32 Hit = 100)
        {
            ParamCmd Param = new ParamCmd
            {
                Path = RequestPath,
                HashData = MapData
            };
            ParseCmd.HashData = JsonConvert.SerializeObject(Param);
            SocketCodition.Boots = NetType.Listen;
            if (Hit >= 100) //权重100以上默认单机发送不负载
                HitBalance(Param);
            else //默认双机负载
            {
                Dictionary<Int32, SessionReceiveEventArgs> HitHand = SocketCodition.Session.Where(t => t.Key.Contains(Param.Service.ToUpper())).Select(t => t.Value).FirstOrDefault();
                if (Hit >= 50 && Hit < 99)
                {
                    Random Rdom = new Random(Guid.NewGuid().GetHashCode());
                    HitWeight.Hits = HitHand.Keys.Any(t => t >= 5) ? HitHand.Keys.Select(t => Rdom.Next(4, HitHand.Keys.Select(t => t >= 5).Count() + 1)).FirstOrDefault() : HitHand.Keys.Min();
                    HitWeight.SessionEvnet = HitHand[HitWeight.Hits];
                    var High = new Thread(new ThreadStart(() => HitWeight.SessionEvnet.Session.Server.Handler.SessionReceive(HitWeight.SessionEvnet.Server, HitWeight.SessionEvnet)));
                    var Normol = new Thread(new ThreadStart(() => HitBalance(Param)));
                    High.Priority = ThreadPriority.Highest;
                    Normol.Priority = ThreadPriority.Normal;
                    High.Start();
                    Normol.Start();
                }
                else
                {
                    Random Rdom = new Random(Guid.NewGuid().GetHashCode());
                    HitWeight.Hits = HitHand.Keys.Any(t => t > 1 && t < 5) ? HitHand.Keys.Select(t => Rdom.Next(1, HitHand.Keys.Count + 1)).FirstOrDefault() : HitHand.Keys.Min();
                    HitWeight.SessionEvnet = HitHand[HitWeight.Hits];
                    var High = new Thread(new ThreadStart(() => HitWeight.SessionEvnet.Session.Server.Handler.SessionReceive(HitWeight.SessionEvnet.Server, HitWeight.SessionEvnet)));
                    var Normol = new Thread(new ThreadStart(() => HitBalance(Param)));
                    High.Priority = ThreadPriority.Highest;
                    Normol.Priority = ThreadPriority.Normal;
                    High.Start();
                    Normol.Start();
                }
            }
            
            try
            {
                Thread.Sleep(1500);
                return JsonConvert.DeserializeObject<Object>(SocketCodition.Result.FirstOrDefault());
            }
            catch (Exception)
            {
                Thread.Sleep(2000);
                return SocketCodition.Result.FirstOrDefault();
            }
        }

        #endregion AJAX提交方式

        #region 文件上传

        /// <summary>
        /// 文件上传
        /// </summary>
        /// <Param name="DirName">目录名称</Param>
        /// <Param name="Context">上下文</Param>
        /// <returns></returns>
        [Post]
        [MultiDataConvert]
        public async Task<Object> UploadFile(IHttpContext Context, String DirName)
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

        #endregion 文件上传

        #region 权重分配

        /// <summary>
        /// 单机负载
        /// </summary>
        /// <param name="Param"></param>
        [NotAction]
        private static void HitBalance(ParamCmd Param)
        {
            //获取TCPSession
            SessionReceiveEventArgs SessionEvent = SocketCodition.Session[Param.Service.ToUpper()].Values.FirstOrDefault();
            //获取TCPSessionId
            SocketCodition.KeyId = SocketCodition.Session[Param.Service.ToUpper()].Keys.FirstOrDefault();
            SessionEvent.Session.Server.Handler.SessionReceive(SessionEvent.Server, SessionEvent);
        }

        #endregion 权重分配
    }
}