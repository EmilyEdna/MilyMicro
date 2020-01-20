using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Mily.Extension.ClientRpc;
using Mily.Extension.Infrastructure.GeneralMiddleWare;
using Mily.Setting;
using Mily.Socket;
using NLog;
using System;
using XExten.CacheFactory;
using XExten.XPlus;

namespace Mily.Extension.InitSystem
{
    public class ConfigInit
    {
        public static String WebPath { get; set; }

        public static void InitConfigCollection(IApplicationBuilder app, IWebHostEnvironment env, IConfiguration builder)
        {
            //Nlog
            LogManager.LoadConfiguration("Nlog.config");
            //注册异常中间件
            app.UseMiddleware<ExceptionMiddleWare>();
            //结果中间件
            app.UseMiddleware<ResultMiddleWare>();
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
            app.UseRouting();
            //注册跨域
            app.UseCors("MilyMicro");
            //注册权限
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            SetConfig(builder);
            //初始化网关客户端
            NetClientProvider.InitGateWayClinet(option =>
            {
                option.ServerPath = MilyConfig.ServerCenterIP;
                option.ServerPort = MilyConfig.ServerCenterPort;
                option.ClientPath = MilyConfig.ClientIP;
                option.ClientPort = MilyConfig.ClientPort;
            });
            //初始化Socket通信中心
            SocketBasic.InitInternalSocket(option =>
            {
                option.SockInfoIP = MilyConfig.SocketInfoIP;
                option.SockInfoPort = MilyConfig.SocketInfoPort;
                option.ClientPath = MilyConfig.ClientIP;
                option.ClientPort = MilyConfig.ClientPort;
            }, true);
            MilyConfig.XmlSQL = XPlusEx.XReadXml();
            WebPath = env.WebRootPath;
        }

        private static void SetConfig(IConfiguration Builder)
        {
            Builder.Bind(new MilyConfig());
            MilyConfig.ConnectionString1 = Builder.GetConnectionString("ConnectionString1");
            MilyConfig.ConnectionString2 = Builder.GetConnectionString("ConnectionString2");
            MilyConfig.ConnectionStringSlave = Builder.GetConnectionString("ConnectionStringSlave");
            MilyConfig.RabbitMQConnectionString = Builder.GetConnectionString("RabbitMQConnectionString");
            MilyConfig.Discovery = Builder["Discovery"];
            MilyConfig.ServerCenterIP = Builder["ServerCenter:ServerCenterIPV4"];
            MilyConfig.ServerCenterPort = Convert.ToInt32(Builder["ServerCenter:ServerCenterIPV4Port"]);
            MilyConfig.ClientIP = Builder["ClientConfig:ClientIPV4"];
            MilyConfig.ClientPort = Convert.ToInt32(Builder["ClientConfig:ClientIPV4Port"]);
            MilyConfig.SocketInfoIP = Builder["SocketInfoConfig:SocketInfoIPV4"];
            MilyConfig.ClientPort = Convert.ToInt32(Builder["SocketInfoConfig:SocketInfoIPV4Port"]);
            Caches.DbName = Builder["MongoDbName"];
            Caches.RedisConnectionString = Builder.GetConnectionString("RedisConnectionString");
            Caches.MongoDBConnectionString = Builder.GetConnectionString("MongoDBConnectionString");
        }
    }
}