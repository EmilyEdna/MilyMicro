using BeetleX;
using BeetleX.Clients;
using Microsoft.AspNetCore.Mvc;
using Mily.Extension.Attributes;
using Mily.Extension.Infrastructure.GeneralMiddleWare;
using Mily.Extension.LoggerFactory;
using Mily.Setting;
using Mily.Setting.DbTypes;
using Mily.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using XExten.CacheFactory;
using XExten.XCore;

namespace Mily.Extension.SocketClient.SocketCommon
{
    public class SocketAop
    {
        /// <summary>
        /// 数据处理
        /// </summary>
        /// <param name="Reader"></param>
        /// <param name="AsyncClient"></param>
        /// <param name="Clinet"></param>
        public static void RecordHandler(Commanding Reader, Type BaseType, AsyncTcpClient AsyncClient = null, TcpClient Client = null)
        {
            if (!Reader.Content.IsContainsIn(">>>"))
            {
                ParamCmd Cmd = Reader.Content.ToModel<ParamCmd>();
                HitHand(BaseType, Cmd, AsyncClient, Client);
            }
            else
            {
                if (Reader.Type == NetType.DisConnect)
                {
                    if (AsyncClient != null)
                        AsyncClient.DisConnect();
                    else
                        Client.DisConnect();
                    LogFactoryExtension.WriteWarn(typeof(SocketAop).FullName, "DisSocket", null, "Socket断开链接", null);
                }
                LogFactoryExtension.WriteInfo(typeof(SocketAop).FullName, "RecordHandler", null, "Socket注册", null);
            }
        }

        /// <summary>
        /// 同步发送
        /// </summary>
        /// <param name="Client"></param>
        /// <param name="SocketType"></param>
        /// <param name="Content"></param>
        public static void InitClient(TcpClient Client, NetType SocketType, string Content = "")
        {
            string Connecting = ParseCmd.Create(SocketType, Content);
            Client.Stream.ToPipeStream().WriteLine(Connecting);
            Client.Stream.Flush();
        }

        /// <summary>
        /// 异步发送
        /// </summary>
        /// <param name="Client"></param>
        /// <param name="SocketType"></param>
        /// <param name="Content"></param>
        public static void InitClientAsync(AsyncTcpClient Client, NetType SocketType, string Content = "")
        {
            string Connecting = ParseCmd.Create(SocketType, Content);
            Client.Stream.ToPipeStream().WriteLine(Connecting);
            Client.Stream.Flush();
        }

        /// <summary>
        /// 多服务路由纠错服务
        /// </summary>
        /// <param name="BaseType"></param>
        /// <param name="Cmd"></param>
        /// <param name="AsyncClient"></param>
        /// <param name="Client"></param>
        private static void HitHand(Type BaseType, ParamCmd Cmd, AsyncTcpClient AsyncClient = null, TcpClient Client = null)
        {
            if (Cmd.HashData.ContainsKey("Global"))
            {
                MilyConfig.CacheKey = Cmd.HashData["Global"].ToString().ToLzStringDec();
                Cmd.HashData.Remove("Global");
            }
            //查询请求的控制器
            Type Control = MilyConfig.Assembly.SelectMany(t => t.ExportedTypes.Where(x => x.BaseType == BaseType)).Where(t => t.Name.Contains(Cmd.Controller)).FirstOrDefault();
            if (Control != null)
            {
                MethodInfo Method = Control.GetMethod(Cmd.Method);
                List<ParameterInfo> ParameterCollentcion = Method.GetParameters().ToList();
                Object Controller = Activator.CreateInstance(Control);
                Object[] parameters = null;
                //非实体类型参数即多参数
                if (ParameterCollentcion.Count >= 2)
                {
                    ParameterCollentcion.ForEach(item =>
                    {
                        string ParamName = item.Name;
                        string TypeName = item.ParameterType.Name;
                        //判断是否可空参数
                        if (!TypeName.IsContainsIn("Nullable`1"))
                        {
                            //必需参数
                            var CheckParam = Cmd.HashData.Keys.ToList().Where(t => t == ParamName).FirstOrDefault();
                            if (CheckParam.IsNullOrEmpty())
                            {
                                SendByClient(ResultApiMiddleWare.Instance(false, 503, null, "必需参数不正确"), AsyncClient, Client);
                                return;
                            }
                        }
                        else
                        {
                            //可选参数
                            int ParamCount = Control.GetMethod(Cmd.Method).GetParameters().Count();
                            int RequestCount = Cmd.HashData.Count();
                            if (ParamCount > RequestCount)
                                for (int index = RequestCount; index < ParamCount; index++)
                                {
                                    Cmd.HashData.Add(index.ToString(), null);
                                }
                        }
                    });
                    parameters = Cmd.HashData.Values.ToArray();
                }
                //实体类型参数只能用实体
                else if (ParameterCollentcion.Count == 1)
                {
                    Type TargetType = ParameterCollentcion.FirstOrDefault().ParameterType;
                    //是否为系统参数
                    if (!TargetType.Namespace.ToUpper().IsContainsIn("SYSTEM"))
                    {
                        var ViewModel = JsonConvert.DeserializeObject(Cmd.HashData.ToJson(), TargetType);
                        parameters = new[] { ViewModel };
                    }
                    else
                        parameters = new[] { Cmd.HashData.Values.FirstOrDefault() };
                }
                //无参数
                else {
                    //校验是否包含数据库类型
                    MilyConfig.DbTypeAttribute = Cmd.HashData.ContainsKey("DbTypeAttribute") ? (DBType)Convert.ToInt32(Cmd.HashData["DbTypeAttribute"]) : DBType.Default;
                    parameters = null;
                }
                try
                {
                    //判断方法的执行权限是否足够
                    if (JudgeAttribute(Method))
                    {
                        Object Result = ((Task<ActionResult<Object>>)Method.Invoke(Controller, parameters)).Result.Value;
                        SendByClient(ResultApiMiddleWare.Instance(true, 200, Result, "执行成功"), AsyncClient, Client);
                    }
                    else
                        SendByClient(ResultApiMiddleWare.Instance(false, 401, null, "无权访问"), AsyncClient, Client);
                }
                catch (Exception ex)
                {
                    RecordExcetion(ex, Cmd.Path);
                    SendByClient(ResultApiMiddleWare.Instance(false, 500, null, "系统出现异常"), AsyncClient, Client);
                    return;
                }
            }
            else
                SendByClient(ResultApiMiddleWare.Instance(false, 400, null, "错误的请求"), AsyncClient, Client);
        }

        /// <summary>
        /// 统一发送
        /// </summary>
        /// <param name="ResultApi"></param>
        /// <param name="AsyncClient"></param>
        /// <param name="Client"></param>
        private static void SendByClient(ResultApiMiddleWare ResultApi, AsyncTcpClient AsyncClient = null, TcpClient Client = null)
        {
            if (AsyncClient != null)
                InitClientAsync(AsyncClient, NetType.Listen, JsonConvert.SerializeObject(ResultApi));
            else
                InitClient(Client, NetType.Listen, JsonConvert.SerializeObject(ResultApi));
        }

        /// <summary>
        /// 异常记录
        /// </summary>
        /// <param name="ex"></param>
        private static void RecordExcetion(Exception ex, string Path)
        {
            string Parameter = string.Empty;
            ex.TargetSite.GetParameters().ToList().ForEach(t =>
            {
                Parameter += "[" + t.Name + "]";
            });
            LogFactoryExtension.WriteError(ex.Source, ex.TargetSite.Name, Parameter, ex.Message, Path);
        }

        /// <summary>
        /// 调用方法前检查有特性
        /// </summary>
        /// <param name="Method"></param>
        private static bool JudgeAttribute(MethodInfo Method)
        {
            AuthorAttribute Author = (Method.GetCustomAttribute(typeof(AuthorAttribute)) as AuthorAttribute);
            if (Method.Name.IsContainsIn("Search"))
                return true;
            return Author == null ? true : JudgeRoles(Author.Names);
        }

        /// <summary>
        /// 检查权限
        /// </summary>
        /// <param name="Roles"></param>
        /// <returns></returns>
        private static bool JudgeRoles(List<String> Roles)
        {
            AdminRoleViewModel ViewModel = Caches.RedisCacheGet<AdminRoleViewModel>(MilyConfig.CacheKey);
            if (ViewModel.HandlerRole.IsNullOrEmpty())
                return false;
            else
            {
                foreach (String item in Roles)
                {
                    if (ViewModel.HandlerRole.Contains(item))
                    {
                        return true;
                    }
                }
                return false;
            }
        }
    }
}