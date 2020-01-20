using Microsoft.Extensions.DependencyModel;
using Mily.Setting.ModelEnum;
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
        /// 数据库类型(动态切换)
        /// </summary>
        public static DBTypeEnum DbTypeAttribute { get; set; }

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
        /// 记录名称
        /// </summary>
        public static String Discovery { get; set; }

        /// <summary>
        /// 缓存Key
        /// </summary>
        public static String CacheKey { get; set; }

        /// <summary>
        /// 数据库名集合
        /// </summary>
        public static  List<string> DbName { get; set; }

        /// <summary>
        /// 默认数据库
        /// </summary>
        public static string Default { get; set; }

        /// <summary>
        /// 服务中心IP
        /// </summary>
        public static string ServerCenterIP { get; set; }

        /// <summary>
        /// 服务中心端口
        /// </summary>
        public static int ServerCenterPort { get; set; }

        /// <summary>
        /// 通信中心IP
        /// </summary>
        public static string SocketInfoIP { get; set; }

        /// <summary>
        /// 通信中心端口
        /// </summary>
        public static int SocketInfoPort { get; set; }

        /// <summary>
        /// 客服端IP
        /// </summary>
        public static string ClientIP { get; set; }

        /// <summary>
        /// 客服端端口
        /// </summary>
        public static int ClientPort { get; set; }

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