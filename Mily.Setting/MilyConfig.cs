using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Mily.Setting
{
    public class MilyConfig
    {
        /// <summary>
        /// 连接字符串-DataBase1
        /// </summary>
        public static String ConnectionString1 { get; set; } 
        /// <summary>
        /// 连接字符串-DataBase2
        /// </summary>
        public static String ConnectionString2 { get; set; } 
        /// <summary>
        /// 连接字符串
        /// </summary>
        public static String ConnectionStringSlave { get; set; }
        /// <summary>
        /// 消息队列链接字符串
        /// </summary>
        public static String RabbitMQConnectionString { get; set; }
        /// <summary>
        /// 所有程序集
        /// </summary>
        public static IList<Assembly> Assembly { get; set; } = GetAssembly();
        private static IList<Assembly> GetAssembly()
        {
            IList<Assembly> ass = new List<Assembly>();
            var lib = DependencyContext.Default;
            var libs = lib.CompileLibraries.Where(t => !t.Serviceable).Where(t => t.Type == "project").ToList();
            foreach (var item in libs)
            {
                Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(item.Name));
                ass.Add(assembly);
            }
            return ass;
        }
    }
}
