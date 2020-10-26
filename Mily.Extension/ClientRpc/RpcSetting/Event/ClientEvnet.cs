using Mily.Extension.ClientRpc.RpcSetting.Handler;
using Mily.Extension.ClientRpc.RpcSetting.Send;
using Mily.Setting;
using Mily.Setting.ModelEnum;
using System;
using System.Linq;
using System.Reflection;
using XExten.Common;
using XExten.XCore;

namespace Mily.Extension.ClientRpc.RpcSetting.Event
{
    public class ClientEvnet
    {
        public static ClientEvnet Instance => new ClientEvnet();
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
            //获取controller
            Type Control = MilyConfig.Assembly.SelectMany(t => t.ExportedTypes.Where(x => x.GetInterfaces().Contains(typeof(IClientGateWayService))))
                .Where(t => t.GetMethods().Any(x => x.Name.ToLower() == Method.ToLower())).FirstOrDefault();
            //获取指定的method
            MethodInfo CtrlMehtod = Control.GetMethod(Method);
            if (CtrlMehtod == null) return ClientSend.Instance.Invoke(Provider, ResponseEnum.NotFound);
            if (!VerifyAuthor.Verify(CtrlMehtod, Provider.DictionaryStringProvider["Authorization"].ToString()))
                return ClientSend.Instance.Invoke(Provider, ResponseEnum.Unauthorized);
            ParameterInfo ParamInfo = CtrlMehtod.GetParameters().FirstOrDefault();
            return ClientSend.Instance.InvokeMthond(Provider, Control, CtrlMehtod, ParamInfo);
        }
        /// <summary>
        /// 转化动态DB
        /// </summary>
        /// <param name="DbType"></param>
        /// <returns></returns>
        internal DBTypeEnum InvokeDyType(Object DbType)
        {
            int.TryParse(DbType?.ToString(), out int Target);
            if (Target > 5 && Target < 0)
                return DBTypeEnum.Default;
            else
                return (DBTypeEnum)Target;
        }
        /// <summary>
        /// 获取缓存的Key
        /// </summary>
        /// <param name="Provider"></param>
        internal void GetCacheKeyInvoke(ResultProvider Provider)
        {
            if (Provider.DictionaryStringProvider.ContainsKey("Global"))
            {
                MilyConfig.CacheKey = Provider.DictionaryStringProvider["Global"].ToString().ToLzStringDec().ToMD5();
                Provider.DictionaryStringProvider.Remove("Global");
            }
        }
    }
}
