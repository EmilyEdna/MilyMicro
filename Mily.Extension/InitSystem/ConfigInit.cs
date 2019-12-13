using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Mily.Extension.ClientRpc;
using Mily.Extension.Infrastructure.GeneralMiddleWare;
using Mily.Setting;
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
            //注册权限
            app.UseAuthentication();
            //注册异常中间件
            app.UseMiddleware<ExceptionMiddleWare>();
            //结果中间件
            app.UseMiddleware<ResultMiddleWare>();
            //注册跨域
            app.UseCors("EdnaCore");
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            SetConfig(builder);
            //初始化RPC客户端
            NetRpcClientProvider.InitClinet(Option =>
            {
                Option.IPAddress = MilyConfig.ServerCenterIP;
                Option.IPPort = MilyConfig.ServerCenterPort;
            });
            MilyConfig.XmlSQL = XPlusEx.ReadXml();
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
            MilyConfig.ServerCenterIP = Builder["ServerCenterIPV4"];
            MilyConfig.ServerCenterPort =Convert.ToInt32(Builder["ServerCenterIPV4Port"]);
            Caches.DbName = Builder["MongoDbName"];
            Caches.RedisConnectionString = Builder.GetConnectionString("RedisConnectionString");
            Caches.MongoDBConnectionString = Builder.GetConnectionString("MongoDBConnectionString");
        }
    }
}