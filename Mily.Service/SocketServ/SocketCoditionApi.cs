using BeetleX.EventArgs;
using BeetleX.FastHttpApi;
using BeetleX.FastHttpApi.Data;
using Mily.Service.ViewSetting;
using Mily.Service.ViewSetting.ApiSettting;
using Mily.Service.ViewSetting.SocketSetting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using XExten.Common;
using XExten.XCore;
using XExten.CacheFactory.RedisCache;

namespace Mily.Service.SocketServ
{
    [Options(AllowOrigin = "*", AllowHeaders = "*")]
    [Controller(BaseUrl = "/Condition")]
    [ActionFilter]
    public class SocketCoditionApi
    {
        #region InitApi

        [NotAction]
        public static void NetApiServProvider()
        {
            //RedisCaches.RedisConnectionString = Configuration.Redis;
            HttpApiServer ApiServ = new HttpApiServer();
            ApiServ.Register(typeof(SocketCoditionApi).Assembly);
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
            Int32 Hit = 100;
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
                    Hit = Data.IsNullOrEmpty() ? Hit : Convert.ToInt32(Data);
            });
            return await JsonAsync(Context, RequestPath, MapDate, Hit);
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
            Int32 Hit = 100;
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
                    Hit = Data.IsNullOrEmpty() ? Hit : Convert.ToInt32(Data);
            });
            return Json(Context, RequestPath, MapDate, Hit);
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
        public async Task<Object> JsonAsync(IHttpContext Context, String RequestPath, Dictionary<String, Object> MapData, Int32 Hit = 100)
        {
            Object Result = RedisCaches.StringGet<Object>(RequestPath);
            if (Result != null)
                return Result;
            SocketComplate(RequestPath, MapData, Hit);
            return await Task.Run<Object>(() =>
            {
                return HandleSocketResults(Context, RequestPath, MapData, Hit);
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
        public Object Json(IHttpContext Context, String RequestPath, Dictionary<String, Object> MapData, Int32 Hit = 100)
        {
            Object Result = RedisCaches.StringGet<Object>(RequestPath);
            if (Result != null)
                return Result;
            SocketComplate(RequestPath, MapData, Hit);
            return HandleSocketResults(Context, RequestPath, MapData, Hit);
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
            String Bin = $"{Directory.GetCurrentDirectory() + Separator}Views{Separator}UpLoadFile{Separator + DirName + DateTime.Now.ToString("yyyyMMdd") + Separator}";
            List<String> Paths = new List<String>();
            if (!Directory.Exists(Bin))
                Directory.CreateDirectory(Bin);
            foreach (PostFile Files in Context.Request.Files)
            {
                using (Stream stream = File.Create(Bin + Files.FileName))
                {
                    await Files.Data.CopyToAsync(stream);
                }
                Paths.Add($"Views{Separator}UpLoadFile{Separator + DirName + DateTime.Now.ToString("yyyyMMdd") + Separator + Files.FileName}");
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

        /// <summary>
        /// 校验请求设置数据库类型
        /// </summary>
        /// <param name="Cmd"></param>
        [NotAction]
        private static Dictionary<String, Object> TypeJudge(String RequestPath, Dictionary<String, Object> MapData)
        {
            if (RequestPath.ToUpper().IsContainsIn("PAGE"))
            {
                PageQuery View = MapData.ToJson().ToModel<PageQuery>();
                if (View.KeyWord.ContainsKey("DbTypeAttribute"))
                    View.KeyWord["DbTypeAttribute"] = Configuration.DbType;
                else
                    View.KeyWord.Add("DbTypeAttribute", Configuration.DbType);
                return View.ToJson().ToModel<Dictionary<string, Object>>();
            }
            else
            {
                if (MapData.ContainsKey("DbTypeAttribute"))
                    MapData["DbTypeAttribute"] = Configuration.DbType;
                else
                    MapData.Add("DbTypeAttribute", Configuration.DbType);
                return MapData;
            }
        }

        /// <summary>
        /// 检测是否熔断
        /// </summary>
        [NotAction]
        private static void Fusing(Dictionary<Int32, SessionReceiveEventArgs> HitHand, Int32 Hit)
        {
            while (true)
            {
                //取随机英子
                Random Rdom = new Random(Guid.NewGuid().GetHashCode());
                //取一个高位负载
                if (Hit >= 50 && Hit < 99)
                {
                    //高位负载值
                    HitWeight.Hits = HitHand.Keys.Any(t => t >= 5) ? HitHand.Keys.Select(t => Rdom.Next(4, HitHand.Keys.Select(t => t >= 5).Count() + 1)).FirstOrDefault() : HitHand.Keys.Min();
                    if (HitWeight.HitsRecord.ContainsKey(HitWeight.Hits))
                        //错误次数大于3取新值
                        if (HitWeight.HitsRecord[HitWeight.Hits] > 2)
                            Fusing(HitHand, Hit);
                        else
                            break;
                    else
                        break;
                }
                else //低位负载
                {
                    //低位负载值
                    HitWeight.Hits = HitHand.Keys.Any(t => t > 1 && t < 5) ? HitHand.Keys.Select(t => Rdom.Next(1, HitHand.Keys.Count + 1)).FirstOrDefault() : HitHand.Keys.Min();
                    if (HitWeight.HitsRecord.ContainsKey(HitWeight.Hits))
                        //错误次数大于3取新值
                        if (HitWeight.HitsRecord[HitWeight.Hits] > 2)
                            Fusing(HitHand, Hit);
                        else
                            break;
                    else
                        break;
                }
            }
        }
        #endregion 权重分配

        #region 优化请求
        /// <summary>
        /// 优化Socket请求
        /// </summary>
        /// <param name="RequestPath"></param>
        /// <param name="MapData"></param>
        /// <param name="Hit"></param>
        [NotAction]
        private void SocketComplate(String RequestPath, Dictionary<String, Object> MapData, Int32 Hit)
        {
            ParamCmd Param = new ParamCmd
            {
                Path = RequestPath,
                HashData = TypeJudge(RequestPath, MapData)
            };
            ParseCmd.HashData = JsonConvert.SerializeObject(Param);
            SocketCodition.Boots = NetType.Listen;
            if (Hit >= 100) //权重100以上默认单机发送不负载
                HitBalance(Param);
            else //默认双机负载
            {
                Dictionary<Int32, SessionReceiveEventArgs> HitHand = SocketCodition.Session.Where(t => t.Key.Contains(Param.Service.ToUpper())).Select(t => t.Value).FirstOrDefault();
                Fusing(HitHand, Hit);
                HitWeight.SessionEvnet = HitHand[HitWeight.Hits];
                var High = new Thread(new ThreadStart(() => HitWeight.SessionEvnet.Session.Server.Handler.SessionReceive(HitWeight.SessionEvnet.Server, HitWeight.SessionEvnet)));
                var Normol = new Thread(new ThreadStart(() => HitBalance(Param)));
                High.Priority = ThreadPriority.Highest;
                Normol.Priority = ThreadPriority.Normal;
                High.Start();
                Normol.Start();
            }
        }
        /// <summary>
        /// 处理返回结果
        /// </summary>
        /// <param name="Context"></param>
        /// <param name="RequestPath"></param>
        /// <param name="MapData"></param>
        /// <param name="Hit"></param>
        /// <returns></returns>
        [NotAction]
        private Object HandleSocketResults(IHttpContext Context, String RequestPath, Dictionary<String, Object> MapData, Int32 Hit)
        {
            Thread.Sleep(1000);
            if (SocketCodition.Result.FirstOrDefault() != null)
            {
                Object ResultObject = JsonConvert.DeserializeObject<Object>(SocketCodition.Result.FirstOrDefault());
                String Code = JToken.FromObject(ResultObject).SelectToken("StatusCode").ToString();
                //返回数据中的状态码不是200则服务器有问题
                if (Convert.ToInt32(Code) != 200)
                {
                    if (HitWeight.HitsRecord.ContainsKey(HitWeight.Hits))
                    {
                        //重试如果2次都错误则直接返回错误
                        if (HitWeight.HitsRecord[HitWeight.Hits] >= 2)
                            return ResultObject;
                        HitWeight.HitsRecord[HitWeight.Hits] += 1;
                    }
                    else
                        HitWeight.HitsRecord.Add(HitWeight.Hits, 1);
                    //重试
                    Json(Context, RequestPath, MapData, Hit);
                    return null;
                }
                else
                {
                    //错误的服务器如果修复了则移除
                    if (HitWeight.HitsRecord.ContainsKey(HitWeight.Hits))
                    {
                        HitWeight.HitsRecord.Remove(HitWeight.Hits);
                    }
                    RedisCaches.StringSet(RequestPath, ResultObject, new TimeSpan(0, 0, 10));
                    return ResultObject;
                }
            }
            else
            {
                return HandleSocketResults(Context, RequestPath, MapData, Hit);
            }
        }
        #endregion
    }
}