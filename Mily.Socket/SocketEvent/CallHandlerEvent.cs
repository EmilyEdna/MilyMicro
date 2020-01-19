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

namespace Mily.Socket.SocketEvent
{
    public class CallHandlerEvent
    {
        /// <summary>
        /// 逆向解析方法
        /// </summary>
        /// <param name="Param"></param>
        /// <returns></returns>
        public static ISocketResult ExecuteCallFuncHandler(SocketMiddleData Param)
        {
            //一定是其它服务
            List<Type> SourceTypes = DependencyLibrary.Dependency.Where(item => item.GetCustomAttribute(typeof(SocketRouteAttribute)) != null).ToList();
            var Routes = Param.MiddleResult.Router.Split("/").ToList();//接受的数据路由
            foreach (var Items in SourceTypes)
            {
                SocketRouteAttribute SocketRoute = (Items.GetCustomAttribute(typeof(SocketRouteAttribute)) as SocketRouteAttribute);
                //比较接受的路由和当前程序的路由是否一直
                //全部得小写
                if (SocketRoute.InternalServer.ToLower() == Routes[0] && (SocketRoute.ControllerName.IsNullOrEmpty() ? Items.Name.ToLower() == Routes[1] : Items.Name.ToLower() == SocketRoute.ControllerName.ToLower()))
                {
                    //查询到了这个类
                    //开始处理所有方法
                    var SoucreMethods = Items.GetMethods().Where(x => x.GetCustomAttribute(typeof(SocketMethodAttribute)) != null).ToList();
                    foreach (var Item in SoucreMethods)
                    {
                        SocketMethodAttribute SocketMethod = (Item.GetCustomAttribute(typeof(SocketMethodAttribute)) as SocketMethodAttribute);
                        //找到对应方法
                        if (SocketMethod.MethodName.IsNullOrEmpty() ? SocketMethod.MethodName.ToLower() == Item.Name.ToLower() : SocketMethod.MethodName.ToLower() == Routes[2])
                        {
                            //路由数量大于等于4说明有版本号
                            if (Routes.Count >= 4 && !SocketMethod.MethodVersion.IsNullOrEmpty() && SocketMethod.MethodVersion.ToLower() == Routes.LastOrDefault())
                            {
                                //1.如果启用了Session需要用户实现ISocketSessionHandler处理
                                //2.Invoke方法
                                if (ExecuteCallSessionHandler(Items, Item))
                                    return null;
                                return null;
                            }
                            else {
                                //1.如果启用了Session需要用户实现ISocketSessionHandler处理
                                //2.Invoke方法
                                if (ExecuteCallSessionHandler(Items, Item))
                                    return null;
                                else
                                    return null;
                            }
                        }
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 处理Session
        /// </summary>
        /// <param name="Controller"></param>
        /// <param name="Method"></param>
        private static bool ExecuteCallSessionHandler(Type Controller, MethodInfo Method)
        {
            SocketAuthorAttribute CtrlAuthor = (Controller.GetCustomAttribute(typeof(SocketAuthorAttribute)) as SocketAuthorAttribute);
            SocketAuthorAttribute MethodAuthor = (Method.GetCustomAttribute(typeof(SocketAuthorAttribute)) as SocketAuthorAttribute);
            if (CtrlAuthor != null)
            {
                if (CtrlAuthor.UseAuthor)
                {
                    return DependencyLibrary.SessionDependency.Count() == 0 ? true : false;
                }
            }
            if (MethodAuthor != null)
            {
                if (MethodAuthor.UseAuthor)
                {
                    return DependencyLibrary.SessionDependency.Count() == 0 ? true : false;
                }
            }
            return false;
        }
    }
}
