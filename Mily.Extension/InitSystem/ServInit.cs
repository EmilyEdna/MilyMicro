using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Mily.Extension.Attributes.PermissionHandler;
using Mily.Extension.AutofacIoc;
using Mily.Extension.Filters;
using Newtonsoft.Json.Serialization;
using Swashbuckle.AspNetCore.Swagger;
using System;

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
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();
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
                option.AddPolicy("EdnaCore", builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            });
            //启用Session
            services.AddSession(opt =>
            {
                //Session 5分钟后过期
                opt.IdleTimeout = TimeSpan.FromMinutes(5);
            });
            //启用Swagger
            services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new Info { Title = "Api", Version = "v1" });
                //opt.IncludeXmlComments(Path.Combine(Directory.GetCurrentDirectory(), "Edna.ApiCore.xml"));
            });
            return services;
        }
    }
}
