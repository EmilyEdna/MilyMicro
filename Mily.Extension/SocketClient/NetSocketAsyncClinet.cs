using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BeetleX;
using BeetleX.Clients;
using XExten.XCore;
using XExten.XExpres;
using Mily.Extension.LoggerFactory;
using Mily.Setting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Mily.Extension.SocketClient.SocketCommon;
using System.Linq.Expressions;
using System.Reflection;
using Mily.Extension.Attributes;

namespace Mily.Extension.SocketClient
{
    public class NetSocketAsyncClinet
    {
        public static bool Boots { get; set; } = true;
        public static Type BaseType { get; set; }
        /// <summary>
        /// 异步注册Socket服务
        /// </summary>
        /// <param name="Port"></param>
        public static void Socket(int Port, Type type)
        {
            BaseType = type;
            AsyncTcpClient Client = SocketFactory.CreateClient<AsyncTcpClient>("127.0.0.1", Port);
            Client.Connect();
            Client.Socket.SendBufferSize = int.MaxValue;
            Client.Socket.ReceiveBufferSize = int.MaxValue;
            Client.ClientError = (client, Err) =>
            {
                LogFactoryExtension.WriteError(Err.Error.Source, Err.Error.TargetSite.Name, string.Join("|", Err.Error.TargetSite.GetParameters().ToList()), Err.Message, "");
            };
            Client.DataReceive = (client, reader) =>
            {
                string result = reader.Stream.ToPipeStream().ReadLine();
                Commanding Command = ParseCmd.Parse(result);
                DataHandler(Command.Content, Client);
                DisSocket(Client, Command.Type);
            };
            if (Boots)
                InitClient(Client, NetType.Connect);
        }
        /// <summary>
        /// 初始化链接
        /// </summary>
        private static void InitClient(AsyncTcpClient Client, NetType SocketType, string Content = "")
        {
            string Connecting = ParseCmd.Create(SocketType, Content);
            Client.Stream.ToPipeStream().WriteLine(Connecting);
            Client.Stream.Flush();
            Boots = false;
        }
        /// <summary>
        /// 数据处理
        /// </summary>
        /// <param name="Data"></param>
        private static void DataHandler(string Data, AsyncTcpClient Client)
        {
            if (!Data.IsContainsIn(">>>"))
            {
                var data = Data.ToModel<ParamCmd>();
                var ss = MilyConfig.Assembly.SelectMany(t => t.ExportedTypes.Where(x => x.BaseType == BaseType)).ToList();
                var Collection = MilyConfig.Assembly.SelectMany(t => t.ExportedTypes.Where(x => x.BaseType == BaseType))
                    .Where(t => t.Name.Contains(data.Controller))
                    .FirstOrDefault();
                if (Collection != null)
                {
                    var ParameterCollentcion = Collection.GetMethod(data.Method).GetParameters().ToList();
                    var Controller = Activator.CreateInstance(Collection);
                    Object[] parameters = null;
                    if (ParameterCollentcion.Count >= 2)
                    {
                        ParameterCollentcion.ForEach(item =>
                        {
                            string ParamName = item.Name;
                            string TypeName = item.ParameterType.Name;
                            if (!TypeName.IsContainsIn("Nullable`1"))
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
                        parameters = data.HashData.Values.ToArray();
                    }
                    if (ParameterCollentcion.Count == 1)
                    {
                        Type TargetType = ParameterCollentcion.FirstOrDefault().ParameterType;
                        Object ViewModel = Activator.CreateInstance(TargetType);
                        data.HashData.Keys.ToEachs(item =>
                        {
                            TargetType.GetProperty(item).SetValue(ViewModel, data.HashData[item]);
                        });
                        parameters = new[] { ViewModel };
                    }
                    try
                    {
                        JudgeAttribute(Collection.GetMethod(data.Method));
                        var result = JsonConvert.SerializeObject(((Task<ActionResult<Object>>)Collection.GetMethod(data.Method).Invoke(Controller, parameters)).Result.Value);
                        InitClient(Client, NetType.Listen, result.ToString());
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
                LogFactoryExtension.WriteInfo(typeof(NetSocketAsyncClinet).FullName, "DataHandler", null, "Socket注册", null);
            }
        }
        /// <summary>
        /// 断开连接
        /// </summary>
        /// <param name="Client"></param>
        /// <param name="Types"></param>
        private static void DisSocket(AsyncTcpClient Client, NetType Types)
        {
            if (Types == NetType.DisConnect)
            {
                Client.DisConnect();
                LogFactoryExtension.WriteWarn(typeof(NetSocketAsyncClinet).FullName, "DisSocket", null, "Socket断开链接", null);
            }
        }
        /// <summary>
        /// 调用方法前检查有特性
        /// </summary>
        /// <param name="Method"></param>
        private static void JudgeAttribute(MethodInfo Method)
        {
            InvokeAttribute Invokes = (Method.GetCustomAttribute(typeof(InvokeAttribute)) as InvokeAttribute);
            Invokes.Name
        }
    }
}
