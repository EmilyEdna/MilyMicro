using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Mily.Extension.FileReader;
using Mily.Extension.Infrastructure.Common;
using Mily.Extension.Infrastructure.GeneralMiddleWare;
using Mily.Extension.LoadComponent;
using NLog;
using XExten.Profile.AspNetCore.DependencyInject;

namespace Mily.Extension.InitSystem
{
    public class ConfigInit
    {
        public static void InitConfigCollection(IApplicationBuilder App, IWebHostEnvironment Environment, IConfiguration Builder)
        {
            //Nlog
            LogManager.LoadConfiguration("Nlog.config");
            //结果中间件
            App.UseMiddleware<ResultMiddleWare>();
            //注册异常中间件
            App.UseMiddleware<ExceptionMiddleWare>();
            App.UseSwagger();
            App.UseSwaggerUI(opt =>
            {
                opt.SwaggerEndpoint("/swagger/v1/swagger.json", "API V1");
            });
            App.UseRouting();
            //注册跨域
            App.UseCors("MilyMicro");
            //注册权限
            App.UseAuthentication();
            App.UseAuthorization();
            App.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
            App.Instruct(Environment, Builder);
            //注册TraceUI功能
            App.UseTraceUI();
            //加载组件
            App.LoadPlugins();
            //初始化Socket相关组件
            InitSocketProxy.InitSocketDependency();
        }
    }
}