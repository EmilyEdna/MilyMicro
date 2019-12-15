using Autofac;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Castle.DynamicProxy;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Mily.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using XExten.XCore;

namespace Mily.Extension.AutofacIoc
{
    /// <summary>
    /// 程序初始化类
    /// </summary>
    public class AutofocManage
    {
        protected ContainerBuilder builder = null;
        protected static readonly IDictionary<Object, Object> AutofacInstance = new Dictionary<Object, Object>();
        protected IContainer Container { get; set; }
        private IEnumerable<Type> Service => MilyConfig.Assembly.SelectMany(t => t.ExportedTypes.Where(x => x.GetInterfaces().Contains(typeof(IService))));
        private Type Aop => Activator.CreateInstance(MilyConfig.Assembly.SelectMany(t => t.ExportedTypes.Where(x => x.GetInterfaces().Contains(typeof(IInterceptor)))).FirstOrDefault()).GetType();
        public AutofocManage() => builder = new ContainerBuilder();

        /// <summary>
        /// 完成构建
        /// </summary>
        protected void CompleteBuiler() => Container = builder.Build();

        /// <summary>
        /// 取出实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Resolve<T>() => Container == null ? default : Container.Resolve<T>();

        /// <summary>
        /// 取出实例
        /// </summary>
        /// <param name="Target"></param>
        /// <returns></returns>
        public Object Resolve(Type Target) => Container == null ? default : Container.Resolve(Target);

        /// <summary>
        /// 返回AutoFac服务
        /// </summary>
        /// <param name="Collection"></param>
        /// <returns></returns>
        public IServiceProvider ServiceProvider(IServiceCollection Collection)
        {
            //托管.net core自带的DI
            builder.Populate(Collection);
            Register(builder);
            CompleteBuiler();
            return Container.Resolve<IServiceProvider>();
        }

        /// <summary>
        /// 程序注入
        /// </summary>
        /// <param name="builder"></param>
        protected void Register(ContainerBuilder builder)
        {
            //注入请求上下文为了使用PaySharp
            builder.RegisterType<HttpContextAccessor>().As<IHttpContextAccessor>().SingleInstance();
            builder.RegisterType(Aop);
            //注入业务逻辑
            Service.ToEachs(item =>
            {
                if (item.IsClass)
                    builder.RegisterType(Activator.CreateInstance(item).GetType())
                    .As(item.GetInterfaces().Where(imp => imp.GetInterfaces().Contains(typeof(IService)))
                    .FirstOrDefault()).InterceptedBy(Aop)
                    .EnableInterfaceInterceptors().SingleInstance();
            });
        }

        /// <summary>
        /// 创建实例
        /// </summary>
        /// <param name="CreateNewInstance"></param>
        /// <returns></returns>
        public static AutofocManage CreateInstance(bool CreateNewInstance = false)
        {
            if (CreateNewInstance)
                return new AutofocManage();
            else
            {
                if (AutofacInstance.Count != 0)
                    return (AutofocManage)AutofacInstance[typeof(AutofocManage).Name];
                else
                {
                    AutofocManage autofoc = new AutofocManage();
                    AutofacInstance.Add(typeof(AutofocManage).Name, autofoc);
                    return autofoc;
                }
            }
        }
    }
}