using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Extension.ClientRpc.RpcSetting.View
{
    public class ClientValue
    {
        public const string RegistSuccessful = "RegistSuccessful";
        public const string ListenedSuccessful = "ListenedSuccessful";
        public const string ListenedFailed = "ListenedFailed";
        public const string CallBackSuccessful = "CallBackSuccessful";
        /// <summary>
        /// 设置运行时值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static dynamic SetValue(string Inputkey, dynamic InputValue) => new { Stutas = Inputkey, Result = InputValue };
        /// <summary>
        /// 设置对象字典值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Dictionary<Object, Object> SetValue(Object Inputkey, Object InputValue) => new Dictionary<Object, Object> { { Inputkey, InputValue } };
        /// <summary>
        /// 设置字符串字典值
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static Dictionary<String, Object> SetStrValue(String Inputkey, Object InputValue) => new Dictionary<String, Object> { { Inputkey, InputValue } };
    }
}
