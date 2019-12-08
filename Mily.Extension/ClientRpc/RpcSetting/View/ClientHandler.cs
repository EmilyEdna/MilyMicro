using BeetleX.Clients;
using Microsoft.AspNetCore.Mvc;
using Mily.Extension.Infrastructure.GeneralMiddleWare;
using Mily.Setting;
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
        /// <param name="BaseType"></param>
        /// <returns></returns>
        public virtual ResultProvider Invoke(ResultProvider Provider, Type BaseType)
        {
            String Method = Provider.DictionaryStringProvider["Method"].ToString();
            Type Control = MilyConfig.Assembly.SelectMany(t => t.ExportedTypes.Where(x => x.BaseType == BaseType))
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
        /// 失败
        /// </summary>
        /// <param name="Provider"></param>
        /// <returns></returns>
        internal ResultProvider InvokeFail(ResultProvider Provider)
        {
            Provider.ObjectProvider = ClientKey.SetValue(NetTypeEnum.Listened, MilyConfig.Discovery);
            Provider.DictionaryStringProvider = ResultApiMiddleWare.Instance(true, 500, null, "执行失败").ToJson().ToModel<Dictionary<String, Object>>();
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
            Object Result = null;
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
                Result = ((Task<ActionResult<Object>>)TargetMethod.Invoke(TargetCtrl, new[] { Provider.DictionaryStringProvider })).Result.Value;
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
            Provider.ObjectProvider = ClientKey.SetValue(NetTypeEnum.Listened, MilyConfig.Discovery);
            Provider.DictionaryStringProvider = ResultApiMiddleWare.Instance(true, 200, Result, "执行成功").ToJson().ToModel<Dictionary<String, Object>>();
            return Provider;
        }
    }
}
