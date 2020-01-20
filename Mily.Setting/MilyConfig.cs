using Microsoft.Extensions.DependencyModel;
using Mily.Setting.DbConfig;
using Mily.Setting.ModelEnum;
using Mily.Setting.SocketConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;

namespace Mily.Setting
{
    public class MilyConfig
    {
        /// <summary>
        /// 链接字符串配置
        /// </summary>
        public static ConnectionString ConnectionStrings { get; set; }
        /// <summary>
        /// 网关配置
        /// </summary>
        public static SocketGateWayConfig SocketGateWay { get; set; }
        /// <summary>
        /// 内部通信配置
        /// </summary>
        public static SocketInternalConfig SocketInternal { get; set; }
        /// <summary>
        /// 数据库类型(动态切换)
        /// </summary>
        public static DBTypeEnum DbTypeAttribute { get; set; }
        /// <summary>
        /// 缓存Key
        /// </summary>
        public static String CacheKey { get; set; }
        /// <summary>
        /// 记录名称
        /// </summary>
        public static String Discovery { get; set; }
        /// <summary>
        /// 数据库名集合
        /// </summary>
        public static  List<string> DbName { get; set; }
        /// <summary>
        /// 默认数据库
        /// </summary>
        public static string Default { get; set; }
        /// <summary>
        /// 配置SQL语句
        /// </summary>
        public static Dictionary<string, string> XmlSQL { get; set; }
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