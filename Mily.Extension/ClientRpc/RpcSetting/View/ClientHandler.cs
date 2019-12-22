using BeetleX.Clients;
using Microsoft.AspNetCore.Mvc;
using Mily.Extension.ClientRpc.RpcSetting.Handler;
using Mily.Extension.Infrastructure.Common;
using Mily.Setting;
using Mily.Setting.DbTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using XExten.Common;
using XExten.XCore;

namespace Mily.Extension.ClientRpc.RpcSetting.View
{
    public class ClientHandler
    {
        public static ClientHandler Instance => new ClientHandler();
        /// <summary>
        /// 查询方法
        /// </summary>
        /// <param name="Provider"></param>
        /// <returns></returns>
        public virtual ResultProvider Invoke(ResultProvider Provider)
        {
            GetCacheKeyInvoke(Provider);
            String Method = Provider.DictionaryStringProvider["Method"].ToString();
            MilyConfig.DbTypeAttribute = InvokeDyType(Provider.DictionaryStringProvider["DataBase"]);
            Type Control = MilyConfig.Assembly.SelectMany(t => t.ExportedTypes.Where(x => x.GetInterfaces().Contains(typeof(IClientService))))
                .Where(t => t.GetMethods().Any(x => x.Name.ToLower() == Method.ToLower())).FirstOrDefault();
            MethodInfo CtrlMehtod = Control.GetMethod(Method);
            ParameterInfo ParamInfo = CtrlMehtod.GetParameters().FirstOrDefault();
            return InvokeMthond(Provider, Control, CtrlMehtod, ParamInfo);
        }
        /// <summary>
        /// 接收数据后回传
        /// </summary>
        /// <param name="ClientAsync"></param>
        /// <param name="Provider"></param>
        /// <returns></returns>
        public virtual AsyncTcpClient SendInvoke(AsyncTcpClient ClientAsync, ResultProvider Provider)
        {
            NetTypeEnum TypeEnum = Provider.ObjectProvider.ToJson().ToModel<ClientKey>().NetType;
            if (TypeEnum == NetTypeEnum.Listened)
                return ClientAsync.Send(ResultProvider.SetValue(ClientKey.SetValue(NetTypeEnum.CallBack, MilyConfig.Discovery), Provider.DictionaryStringProvider));
            return null;
        }
        /// <summary>
        /// 转化动态DB
        /// </summary>
        /// <param name="DbType"></param>
        /// <returns></returns>
        internal DBType InvokeDyType(Object DbType)
        {
            int.TryParse(DbType?.ToString(), out int Target);
            if (Target > 5 && Target < 0)
                return DBType.Default;
            else
                return (DBType)Target;
        }
        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="Provider"></param>
        /// <returns></returns>
        internal ResultProvider InvokeFail(ResultProvider Provider)
        {
            String Method = Provider.DictionaryStringProvider["Method"].ToString();
            Provider.ObjectProvider = ClientKey.SetValue(NetTypeEnum.Listened, Method);
            Provider.DictionaryStringProvider = ResultCondition.Instance(true, 500, null, "执行失败").ToJson().ToModel<Dictionary<String, Object>>();
            RemoveInvoke(Provider);
            return Provider;
        }
        /// <summary>
        /// 执行结果
        /// </summary>
        /// <param name="Provider"></param>
        /// <param name="Control"></param>
        /// <param name="TargetMethod"></param>
        /// <param name="ParamInfo"></param>
        /// <returns></returns>
        internal ResultProvider InvokeMthond(ResultProvider Provider, Type Control, MethodInfo TargetMethod, ParameterInfo ParamInfo)
        {
            Object TargetCtrl = Activator.CreateInstance(Control);
            Object Result;
            if (ParamInfo?.ParameterType == typeof(PageQuery))
            {
                PageQuery TargetParamerter = Provider.DictionaryStringProvider.ToJson().ToModel<PageQuery>();
                Result = ((Task<ActionResult<Object>>)TargetMethod.Invoke(TargetCtrl, new[] { TargetParamerter })).Result.Value;
                if (Result != null)
                    return InvokeSuccess(Provider, Result);
                else
                    return InvokeFail(Provider);
            }
            else if (ParamInfo?.ParameterType == typeof(ResultProvider))
            {
                Result = ((Task<ActionResult<Object>>)TargetMethod.Invoke(TargetCtrl, new[] { Provider })).Result.Value;
                if (Result != null)
                    return InvokeSuccess(Provider, Result);
                else
                    return InvokeFail(Provider);
            }
            else
            {
                Result = ((Task<ActionResult<Object>>)TargetMethod.Invoke(TargetCtrl, null)).Result.Value;
                if (Result != null)
                    return InvokeSuccess(Provider, Result);
                else
                    return InvokeFail(Provider);
            }
        }
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="Provider"></param>
        /// <param name="Result"></param>
        /// <returns></returns>
        internal ResultProvider InvokeSuccess(ResultProvider Provider, Object Result)
        {
            String Method = Provider.DictionaryStringProvider["Method"].ToString();
            Provider.ObjectProvider = ClientKey.SetValue(NetTypeEnum.Listened, Method);
            Provider.DictionaryStringProvider = ResultCondition.Instance(true, 200, Result.ToJson(), "执行成功").ToJson().ToModel<Dictionary<String, Object>>();
            RemoveInvoke(Provider);
            return Provider;
        }
        /// <summary>
        /// 删除Header数据
        /// </summary>
        /// <param name="Provider"></param>
        internal void RemoveInvoke(ResultProvider Provider)
        {
            Provider.DictionaryStringProvider.Remove("Method");
            Provider.DictionaryStringProvider.Remove("DataBase");
        }
        /// <summary>
        /// 获取缓存的Key
        /// </summary>
        /// <param name="Provider"></param>
        internal void GetCacheKeyInvoke(ResultProvider Provider)
        {
            if (Provider.DictionaryStringProvider.ContainsKey("Global"))
            {
                MilyConfig.CacheKey = Provider.DictionaryStringProvider["Global"].ToString();
                Provider.DictionaryStringProvider.Remove("Global");
            }
        }
    }
}
