using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Mily.Extension.Authentication.CookieAuthentication;
using Mily.Extension.Authentication.JwtAuthentication;
using Mily.Extension.AutofacIoc;
using Mily.Extension.Filters;
using Newtonsoft.Json.Serialization;
using NLog.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;

namespace Mily.Extension.InitSystem
{
    public class ServInit
    {
        public static IServiceCollection InitServCollection(IServiceCollection services)
        {
            AutofocManage.CreateInstance().ServiceProvider(services);
            services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();
            services.Configure<ApiBehaviorOptions>(opt =>
            {
                opt.SuppressModelStateInvalidFilter = true;
                opt.SuppressInferBindingSourcesForParameters = true;
                opt.SuppressConsumesConstraintForFormFileParameters = true;
            });
            //启用权限认证
            services.AddAuthentication(options=> {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options => {
                options.SecurityTokenValidators.Clear();
                options.SecurityTokenValidators.Add(new VerifyToken());
                options.Events = new JwtBearerEvents() {
                    OnMessageReceived = Context => 
                    {
                        var token = Context.Request.Headers["Authorization"];//修改默认的http headers
                        Context.Token = token.FirstOrDefault();
                        return Task.CompletedTask;
                    }
                };
            });
            //设置数据格式
            services.AddControllers(opt =>
            {
                opt.Filters.Add(typeof(ActionFilter));
                opt.RespectBrowserAcceptHeader = true;
            }).AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                opt.SerializerSettings.ContractResolver = new DefaultContractResolver();
            }).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            //启用跨域
            services.AddCors(option =>
            {
                option.AddPolicy("MilyMicro", builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin());
            });
            //启用Swagger
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo { Title = "Api", Version = "v1" });
                //opt.IncludeXmlComments(Path.Combine(Directory.GetCurrentDirectory(), "Mily.OtherApi.xml"));
            });
            //使用NLog
            services.AddLogging(builder =>
            {
                builder.AddNLog();
            });
            return services;
        }
    }
}