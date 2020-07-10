using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using Microsoft.AspNetCore.Builder;
using XExten.Profile.AspNetCore.InvokeTracing;
using XExten.XCore;

namespace Mily.Extension.LoadComponent
{
    public static class LoadAssembliyFactory
    {
        /// <summary>
        /// 加载组件
        /// </summary>
        /// <param name="App"></param>
        /// <param name="AssemblyName">指定程序集的初始化类</param>
        public static void LoadPlugins(this IApplicationBuilder App, params string[] AssemblyName)
        {
            LoadAssembliy loader = new LoadAssembliy();
            string ComponentPath = Path.Combine(AppContext.BaseDirectory, @"Component\");
            //找到目录下所有组件
            string[] FullDllPath = Directory.GetFiles(ComponentPath, "*.dll");
            FullDllPath.ToEach<string>(Item =>
            {
                using FileStream Fs = new FileStream(Item, FileMode.Open, FileAccess.Read);
                Assembly assembly = loader.LoadFromStream(Fs);
                AssemblyName.ToEach<string>(Paths =>
                {
                    Type type = assembly.GetType(Paths);
                    MethodInfo info = type?.GetMethod("InitComponent");
                    if (type != null)
                    {
                        object obj = Activator.CreateInstance(type);
                        info.ByTraceInvoke(obj, null);
                    }
                });
                loader.Unload();

                //GC.Collect();
                //GC.WaitForPendingFinalizers();
            });
        }
    }
}
