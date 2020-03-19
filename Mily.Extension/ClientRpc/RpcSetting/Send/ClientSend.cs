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
        /// 返回结果给请求端
        /// </summary>
        /// <param name="Provider"></param>
        /// <param name="Response"></param>
        /// <param name="Result"></param>
        /// <returns></returns>
        internal ResultProvider Invoke(ResultProvider Provider, ResponseEnum Response, Object Result = null)
        {
            String Method = Provider.DictionaryStringProvider["Method"].ToString();
            Provider.ObjectProvider = ClientKey.SetValue(NetTypeEnum.Listened, Method);
            ResultCondition Condition = ResultCondition.Instance(Item =>
            {
                Item.IsSuccess = false;
                Item.StatusCode = (int)Response;
                Item.ResultData = Result?.ToJson();
                Item.Info = Response.ToDescription();
                Item.ServerDateStr = DateTime.Now.ToString("yyyy年MM月dd日HH时mm分ss秒ffff毫秒");
                Item.ServerDateLong = Convert.ToInt64(DateTime.Now.ToString("yyyyMMddhhmmssffff"));
            });
            Provider.DictionaryStringProvider = Condition.ToJson().ToModel<Dictionary<String, Object>>();
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
            if (ParamInfo?.ParameterType == typeof(PageQuery))
                return InvokePage(Provider, TargetMethod, TargetCtrl);
            else if (ParamInfo?.ParameterType == typeof(ResultProvider))
                return InvokeProvider(Provider, TargetMethod, TargetCtrl);
            else
                return InvokeNull(Provider, TargetMethod, TargetCtrl);
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

        #region 结果回调
        /// <summary>
        /// 分页结果
        /// </summary>
        /// <param name="Provider"></param>
        /// <param name="TargetMethod"></param>
        /// <param name="TargetCtrl"></param>
        /// <returns></returns>
        private ResultProvider InvokePage(ResultProvider Provider, MethodInfo TargetMethod, Object TargetCtrl)
        {
            PageQuery TargetParamerter = Provider.DictionaryStringProvider.ToJson().ToModel<PageQuery>();
            var PreResult = (Task<ActionResult<Object>>)TargetMethod.Invoke(TargetCtrl, new[] { TargetParamerter });
            var BaseException = PreResult.Exception?.GetBaseException();
            if (BaseException != null)
            {
                Console.WriteLine($"Exception Message：{BaseException.Message}");
                return Invoke(Provider, ResponseEnum.InternalServerError);
            }
            else
            {
                Object Result = ((Task<ActionResult<Object>>)TargetMethod.Invoke(TargetCtrl, new[] { TargetParamerter })).Result.Value;
                if (Result != null) return Invoke(Provider, ResponseEnum.OK, Result);
                else return Invoke(Provider, ResponseEnum.InternalServerError);
            }
        }
        /// <summary>
        /// 通常结果
        /// </summary>
        /// <param name="Provider"></param>
        /// <param name="TargetMethod"></param>
        /// <param name="TargetCtrl"></param>
        /// <returns></returns>
        private ResultProvider InvokeProvider(ResultProvider Provider, MethodInfo TargetMethod, Object TargetCtrl)
        {
            var PreResult = (Task<ActionResult<Object>>)TargetMethod.Invoke(TargetCtrl, new[] { Provider });
            var BaseException = PreResult.Exception?.GetBaseException();
            if (BaseException != null)
            {
                Console.WriteLine($"Exception Message：{BaseException.Message}");
                return Invoke(Provider, ResponseEnum.InternalServerError);
            }
            else
            {
                Object Result = ((Task<ActionResult<Object>>)TargetMethod.Invoke(TargetCtrl, new[] { Provider })).Result.Value;
                if (Result != null) return Invoke(Provider, ResponseEnum.OK, Result);
                else return Invoke(Provider, ResponseEnum.InternalServerError);
            }
        }
        /// <summary>
        /// 无参数结果
        /// </summary>
        /// <param name="Provider"></param>
        /// <param name="TargetMethod"></param>
        /// <param name="TargetCtrl"></param>
        /// <returns></returns>
        private ResultProvider InvokeNull(ResultProvider Provider, MethodInfo TargetMethod, Object TargetCtrl)
        {
            var PreResult = (Task<ActionResult<Object>>)TargetMethod.Invoke(TargetCtrl, null);
            var BaseException = PreResult.Exception?.GetBaseException();
            if (BaseException != null)
            {
                Console.WriteLine($"Exception Message：{BaseException.Message}");
                return Invoke(Provider, ResponseEnum.InternalServerError);
            }
            else
            {
                Object Result = ((Task<ActionResult<Object>>)TargetMethod.Invoke(TargetCtrl, null)).Result.Value;
                if (Result != null) return Invoke(Provider, ResponseEnum.OK, Result);
                else return Invoke(Provider, ResponseEnum.InternalServerError);
            }
        }
        #endregion
    }
}
