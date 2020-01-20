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
                option.ServerPath = MilyConfig.SocketGateWay.GateWayIPV4;
                option.ServerPort = MilyConfig.SocketGateWay.GateWayIPV4Port;
                option.ClientPath = MilyConfig.SocketGateWay.ClientIPV4;
                option.ClientPort = MilyConfig.SocketGateWay.ClientIPV4Port;
            });
            //初始化Socket通信中心
            SocketBasic.InitInternalSocket(option =>
            {
                option.SockInfoIP = MilyConfig.SocketInternal.InternalIPV4;
                option.SockInfoPort = MilyConfig.SocketInternal.InternalIPV4Port;
                option.ClientPath = MilyConfig.SocketInternal.ClientInternalIPV4;
                option.ClientPort = MilyConfig.SocketInternal.ClientInternalIPV4Port;
            }, true);
            MilyConfig.XmlSQL = XPlusEx.XReadXml();
            WebPath = env.WebRootPath;
        }

        private static void SetConfig(IConfiguration Builder)
        {
            Builder.Bind(new MilyConfig());
            Caches.DbName = MilyConfig.ConnectionStrings.MongoDbName;
            Caches.RedisConnectionString = MilyConfig.ConnectionStrings.RedisConnectionString;
            Caches.MongoDBConnectionString = MilyConfig.ConnectionStrings.MongoDBConnectionString;
        }
    }
}