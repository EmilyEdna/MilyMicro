using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Mily.Extension.Infrastructure.GeneralModel;
using Mily.Setting;
using NLog.Extensions.Logging;
using NLog.Web;
using System;
using System.Collections.Generic;
using System.Text;
using XExten.CacheFactory.RedisCache;

namespace Mily.Extension.InitSystem
{
    public class ConfigInit
    {
        public static ConfigInit Init => new ConfigInit();
        public void InitConfigCollection(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory logger, IConfiguration builder) {
            //Nlog
            logger.AddNLog();
            env.ConfigureNLog("Nlog.config");
            //注册权限
            app.UseAuthentication();
            //注册异常中间件
            app.UseMiddleware<ExceptionMiddleWare>();
            //注册跨域
            app.UseCors("EdnaCore");
            //注册Session
            app.UseSession();
            app.UseSwagger();
            app.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
            SetConfig(builder);
        }
        private void SetConfig(IConfiguration Builder) {
            MilyConfig.ConnectionString1= Builder.GetConnectionString("ConnectionString1");
            MilyConfig.ConnectionString2 = Builder.GetConnectionString("ConnectionString2");
            MilyConfig.ConnectionStringSlave = Builder.GetConnectionString("ConnectionStringSlave");
            MilyConfig.RabbitMQConnectionString = Builder.GetConnectionString("RabbitMQConnectionString");
            RedisCaches.RedisConnectionString = Builder.GetConnectionString("RedisConnectionString");
        }
    }
}
