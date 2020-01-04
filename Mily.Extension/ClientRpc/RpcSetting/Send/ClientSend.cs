using BeetleX.Clients;
using Microsoft.AspNetCore.Mvc;
using Mily.Extension.ClientRpc.RpcSetting.View;
using Mily.Extension.Infrastructure.Common;
using Mily.Setting;
using Mily.Setting.ModelEnum;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using XExten.Common;
using XExten.XCore;

namespace Mily.Extension.ClientRpc.RpcSetting.Send
{
    public class ClientSend
    {
        public static ClientSend Instance => new ClientSend();
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="Provider"></param>
        /// <param name="Response"></param>
        /// <param name="Result"></param>
        /// <returns></returns>
        internal ResultProvider Invoke(ResultProvider Provider, ResponseEnum Response, Object Result = null)
        {
            String Method = Provider.DictionaryStringProvider["Method"].ToString();
            Provider.ObjectProvider = ClientKey.SetValue(NetTypeEnum.Listened, Method);
            Provider.DictionaryStringProvider = ResultCondition.Instance(true, (int)Response, Result?.ToJson(), Response.ToString()).ToJson().ToModel<Dictionary<String, Object>>();
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
                if (Result != null) return Invoke(Provider,ResponseEnum.请求成功, Result);
                else return Invoke(Provider, ResponseEnum.内部服务器错误);
            }
            else if (ParamInfo?.ParameterType == typeof(ResultProvider))
            {
                Result = ((Task<ActionResult<Object>>)TargetMethod.Invoke(TargetCtrl, new[] { Provider })).Result.Value;
                if (Result != null) return Invoke(Provider, ResponseEnum.请求成功, Result);
                else return Invoke(Provider, ResponseEnum.内部服务器错误);
            }
            else
            {
                Result = ((Task<ActionResult<Object>>)TargetMethod.Invoke(TargetCtrl, null)).Result.Value;
                if (Result != null) return Invoke(Provider, ResponseEnum.请求成功, Result);
                else return Invoke(Provider, ResponseEnum.内部服务器错误);
            }
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
    }
}
