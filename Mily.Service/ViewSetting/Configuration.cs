using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mily.Service.ViewSetting
{
    public class Configuration
    {
        public static string HandleData { get; set; }
        public static IConfiguration Builder => GetSetting();
        public static string TCP_Host { get; set; } = Builder["TCP:TCPHOST"].ToString();
        public static int TCP_Port { get; set; } = Convert.ToInt32(Builder["TCP:TCPPORT"].ToString());
        public static string SOCKET_Host { get; set; } = Builder["SOCKET:SOCKETHOST"].ToString();
        public static int SOCKET_Port { get; set; } = Convert.ToInt32(Builder["SOCKET:SOCKETPORT"].ToString());
        private static IConfiguration GetSetting()
        {
            IConfigurationBuilder Buidler = new ConfigurationBuilder();
            Buidler.SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("setting.json", optional: false, reloadOnChange: true);
            return Buidler.Build();
        }
    }
}
