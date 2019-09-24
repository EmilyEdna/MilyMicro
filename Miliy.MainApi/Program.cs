using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Hosting;
using XExten.XCore;

namespace Miliy.MainApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (!args.Contains("--Service:"))
                CreateHostBuilder(args).Build().Run();
            else
            {
                args.ToEach<String>(item =>
                {
                    //open cmd with administrator and input SC CREATE [SERVERNAME] BINPATH="[PROGRAM_URL]\*.EXE" --Service:1024
                    if (item.IsContainsIn("--Service:"))
                    {
                        int Port = Convert.ToInt32(item.Split(":")[1]);
                        Startup.Port = Port + 1;
                        CreateWebHostBuilder(args, Port);
                    }
                });
            }
        }
        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
        public static void CreateWebHostBuilder(string[] args, int Port) =>
            WebHost.CreateDefaultBuilder(args).UseKestrel()
                .UseContentRoot(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName))//加载当前目录
                .UseUrls($"http://0.0.0.0:{Port}")//监听的IP和端口
                .UseStartup<Startup>().Build().RunAsService();
    }
}
