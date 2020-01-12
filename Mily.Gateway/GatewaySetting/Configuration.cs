using Microsoft.Extensions.Configuration;
using XExten.CacheFactory;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using XExten.XPlus;

namespace Mily.Gateway.GatewaySetting
{
    public class Configuration
    {
        public static IConfiguration Builder => GetSetting();
        public static string TCP_Host { get; set; } = Builder["TCP:TcpHost"] ?? "0.0.0.0";
        public static int TCP_Port { get; set; } = Convert.ToInt32(Builder["TCP:TcpPort"] ?? "9090");
        public static string SOCKET_Host { get; set; } = Builder["Socket:SocketHost"] ?? "0.0.0.0";
        public static int SOCKET_Port { get; set; } = Convert.ToInt32(Builder["Socket:SocketPort"] ?? "9091");
        public static List<string> FuncArray { get; set; } = Builder["ProxyFunctionAarry:FuncArray"].Split(",").ToList();
        public static HeadConfiger Heads { get; set; }
        public static string Authorization { get; set; }
        public static void InitConnection()
        {
            XPlusEx.XTry(() =>
            {
                Caches.DbName = Builder["MongoDb:DbName"] == null ? "MilyConfiger" : Builder["MongoDb:DbName"].ToString();
                Caches.MongoDBConnectionString = Builder["MongoDb:MongoConnectionString"] == null ? "mongodb://127.0.0.1:27017" : Builder["MongoDb:MongoConnectionString"].ToString();
                Caches.RedisConnectionString = Builder["Redis:RedisConnectionString"] == null ? "127.0.0.1:6379" : Builder["Redis:RedisConnectionString"].ToString();
            }, Ex => throw new Exception("请配置Redis和Mongodb链接字符串"));
        }
        private static IConfiguration GetSetting()
        {
            IConfigurationBuilder Buidler = new ConfigurationBuilder();
            Buidler.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("setting.json", false, true);
            return Buidler.Build();
        }
    }
}
