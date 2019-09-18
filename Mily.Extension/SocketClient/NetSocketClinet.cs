using System;
using System.Threading.Tasks;
using BeetleX;
using BeetleX.Clients;
using XExten.XCore;
using Mily.Extension.LoggerFactory;
using Mily.Setting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using Mily.Extension.SocketClient.SocketCommon;

namespace Mily.Extension.SocketClient
{
    public class NetSocketClinet
    {
        public static Type BaseType { get; set; }
        /// <summary>
        /// 同步注册Socket服务
        /// </summary>
        /// <param name="Port"></param>
        /// <param name="type"></param>
        public static void Socket(int Port, Type type)
        {
            BaseType = type;
            TcpClient Client = SocketFactory.CreateClient<TcpClient>("127.0.0.1", Port);
            Client.Connect();
            Client.Socket.ReceiveTimeout = int.MaxValue;
            Client.Socket.SendBufferSize = int.MaxValue;
            Client.Socket.ReceiveBufferSize = int.MaxValue;
            InitClient(Client, NetType.Connect);
            Task.Run(() =>
            {
                while (true)
                {
                    DataHandler(Client);
                }
            });
        }
        /// <summary>
        /// 初始化链接
        /// </summary>
        private static void InitClient(TcpClient Client, NetType SocketType, string Content = "")
        {
            string Connecting = ParseCmd.Create(SocketType, Content);
            Client.Stream.ToPipeStream().WriteLine(Connecting);
            Client.Stream.Flush();
        }
        /// <summary>
        /// 数据处理
        /// </summary>
        /// <param name="Data"></param>
        private static void DataHandler(TcpClient Client)
        {
            var Data = Client.Receive().ToPipeStream().ReadLine();
            Commanding Reader = ParseCmd.Parse(Data);
            if (!Reader.Content.IsContainsIn(">>>"))
            {
                var data = Reader.Content.ToModel<ParamCmd>();
                var Collection = MilyConfig.Assembly.SelectMany(t => t.ExportedTypes.Where(x => x.BaseType == BaseType))
                    .Where(t => t.Name.Contains(data.Controller))
                    .FirstOrDefault();
                if (Collection != null)
                {
                    Collection.GetMethod(data.Method).GetParameters().ToList().ForEach(item =>
                    {
                        string ParamName = item.Name;
                        string TypeName = item.ParameterType.Name;
                        if (!TypeName.Contains("Nullable`1"))
                        {
                            var CheckParam = data.HashData.Keys.ToList().Where(t => t == ParamName).FirstOrDefault();
                            if (CheckParam.IsNullOrEmpty())
                            {
                                InitClient(Client, NetType.Listen, "必需参数不正确");
                                return;
                            }
                        }
                        else
                        {
                            var ParamCount = Collection.GetMethod(data.Method).GetParameters().Count();
                            var RequestCount = data.HashData.Count();
                            if (ParamCount > RequestCount)
                                for (int index = RequestCount; index < ParamCount; index++)
                                {
                                    data.HashData.Add(index.ToString(), null);
                                }
                        }
                    });
                    try
                    {
                        var Controller = Activator.CreateInstance(Collection);
                        Object[] parameters = data.HashData.Values.ToArray();
                        var result = JsonConvert.SerializeObject(((Task<ActionResult<Object>>)Collection.GetMethod(data.Method).Invoke(Controller, parameters)).Result.Value);
                        InitClient(Client, NetType.Listen, result);
                    }
                    catch (Exception ex)
                    {
                        string Parameter = string.Empty;
                        ex.TargetSite.GetParameters().ToList().ForEach(t =>
                        {
                            Parameter += "[" + t.Name + "]";
                        });
                        LogFactoryExtension.WriteError(ex.Source, ex.TargetSite.Name, Parameter, ex.Message, data.Path);
                        InitClient(Client, NetType.Listen, JsonConvert.SerializeObject(new { Error = "系统出现异常!" }));
                        return;
                    }
                }
            }
            else
            {
                if (Reader.Type == NetType.DisConnect)
                {
                    Client.DisConnect();
                    LogFactoryExtension.WriteWarn(typeof(NetSocketClinet).FullName, "DisSocket", null, "Socket断开链接", null);
                }
                LogFactoryExtension.WriteInfo(typeof(NetSocketAsyncClinet).FullName, "DataHandler", null, "Socket注册", null);
            }
        }
    }
}
