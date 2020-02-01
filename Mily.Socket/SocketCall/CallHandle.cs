using Mily.Socket.SocketConfig;
using Mily.Socket.SocketDependency;
using Mily.Socket.SocketInterface;
using Mily.Socket.SocketRoute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using XExten.XCore;
using System.Text;
using XExten.Common;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Mily.Socket.SocketInterface.DefaultImpl;
using Mily.Socket.SocketAbstract;

namespace Mily.Socket.SocketCall
{
    public class CallHandle: CallHandleAbstract
    {
        /// <summary>
        /// 逆向解析方法
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        public override ISocketResult ExecuteCallFuncHandler(SocketMiddleData Param)
        {
          return base.ExecuteCallFuncHandler(Param);
        }
        /// <summary>
        /// 处理Session
        /// </summary>
        /// <param name="Controller"></param>
        /// <param name="Method"></param>
        protected override bool ExecuteCallSessionHandler(MethodInfo Method, ISocketSession Session)
        {
            return base.ExecuteCallSessionHandler(Method, Session);
        }
        /// <summary>
        /// 处理数据
        /// </summary>
        /// <param name="Controller"></param>
        /// <param name="Method"></param>
        /// <returns></returns>
        protected override ISocketResult ExecuteCallDataHandler(Type Controller, MethodInfo Method, ISocketResult Param)
        {
            return base.ExecuteCallDataHandler(Controller, Method, Param);
        }
    }
}
