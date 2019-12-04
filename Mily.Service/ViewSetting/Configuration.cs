using Microsoft.Extensions.Configuration;
using XExten.CacheFactory;
using System;
using System.IO;

namespace Mily.Service.ViewSetting
{
    public class Configuration
    {
        public static IConfiguration Builder => GetSetting();
        public static string TCP_Host { get; set; } = Builder["TCP:TcpHost"].ToString();
        public static int TCP_Port { get; set; } = Convert.ToInt32(Builder["TCP:TcpPort"].ToString());
        public static string SOCKET_Host { get; set; } = Builder["Socket:SocketHost"].ToString();
        public static int SOCKET_Port { get; set; } = Convert.ToInt32(Builder["Socket:SocketPort"].ToString());
        public static string Redis { get; set; } = Builder["Redis:RedisConnectionString"].ToString();
        public static string DbType { get; set; }
        public static void InitConnection()
        {
            try
            {
                Caches.DbName = Builder["MongoDb:DbName"].ToString();
                Caches.MongoDBConnectionString = Builder["MongoDb:MongoConnectionString"].ToString();
                Caches.RedisConnectionString = Builder["Redis:RedisConnectionString"].ToString();
            }
            catch (Exception)
            {
                throw new Exception("请配置Redis和Mongodb链接字符串");
            }

        }
        private static IConfiguration GetSetting()
        {
            IConfigurationBuilder Buidler = new ConfigurationBuilder();
            Buidler.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("setting.json", optional: false, reloadOnChange: true);
            return Buidler.Build();
        }
    }
}