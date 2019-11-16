using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Mily.Extension.InitSystem;
using Mily.Extension.SocketClient;
using XExten.XPlus;

namespace Mily.OtherApi
{
    public class Startup
    {
        public static int Port { get; set; }

        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", false, true)
                .AddJsonFile("appsettings.dbconfig.json", false, true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ServInit.InitServCollection(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder Application, IWebHostEnvironment Environment)
        {
            if (Environment.IsDevelopment())
            {
                Application.UseDeveloperExceptionPage();
            }
            ConfigInit.InitConfigCollection(Application, Environment, Configuration);
            XPlusEx.ReadXml();
            NetSocketAsyncClinet.Socket(9090, typeof(BaseApiController));
            //NetSocketClinet.Socket(9090, typeof(BaseApiController));
        }
    }
}