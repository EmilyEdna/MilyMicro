using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mily.Extension.Infrastructure.GeneralModel;
using Mily.Setting;
using NLog;

using System;
using XExten.CacheFactory.RedisCache;

namespace Mily.Extension.InitSystem
{
    public class ConfigInit
    {
        public static String WebPath { get; set; }

        public static void InitConfigCollection(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory logger, IConfiguration builder)
        {
            //Nlog
            LogManager.LoadConfiguration("Nlog.config");
            //logger.ConfigureNLog("Nlog.config");
            //注册权限
            app.UseAuthentication();
            //注册异常中间件
            app.UseMiddleware<ExceptionMiddleWare>();
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
            WebPath = env.WebRootPath;
        }

        private static void SetConfig(IConfiguration Builder)
        {
            MilyConfig.ConnectionString1 = Builder.GetConnectionString("ConnectionString1");
            MilyConfig.ConnectionString2 = Builder.GetConnectionString("ConnectionString2");
            MilyConfig.ConnectionStringSlave = Builder.GetConnectionString("ConnectionStringSlave");
            MilyConfig.RabbitMQConnectionString = Builder.GetConnectionString("RabbitMQConnectionString");
            MilyConfig.Discovery = Builder["Discovery"].ToString();
            RedisCaches.RedisConnectionString = Builder.GetConnectionString("RedisConnectionString");
        }
    }
}