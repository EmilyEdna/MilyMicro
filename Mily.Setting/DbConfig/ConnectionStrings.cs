using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Setting.DbConfig
{
    /// <summary>
    /// 链接字符串配置
    /// </summary>
    public class ConnectionString
    {
        /// <summary>
        /// 连接字符串-DataBase1
        /// </summary>
        public string ConnectionString1 { get; set; }
        /// <summary>
        /// 连接字符串-DataBase2
        /// </summary>
        public string ConnectionString2 { get; set; }
        /// <summary>
        /// 从库连接字符串
        /// </summary>
        public string ConnectionStringSlave { get; set; }
        /// <summary>
        /// 消息队列链接字符串
        /// </summary>
        public string RabbitMQConnectionString { get; set; }
        /// <summary>
        /// RedisL链接字符串
        /// </summary>
        public string RedisConnectionString { get; set; }
        /// <summary>
        /// MongoDb链接字符串
        /// </summary>
        public string MongoDBConnectionString { get; set; }
        /// <summary>
        /// MongoDb数据库名称
        /// </summary>
        public string MongoDbName { get; set; }
    }
}
