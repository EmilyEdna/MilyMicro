using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using XExten.XCore;

namespace Miliy.MainApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (!args.Contains("--Service:"))
                WebHostDefaults(args).Build().Run();
            else
            {
                args.ToEach<String>(item =>
                {
                    //open cmd with administrator and input SC CREATE [SERVERNAME] BINPATH="[PROGRAM_URL]\*.EXE" --Service:1024
                    if (item.IsContainsIn("--Service:"))
                    {
                        int Port = Convert.ToInt32(item.Split(":")[1]);
                        Startup.Port = Port + 1;
                        WindowsServiceDefaults(args, Port);
                    }
                });
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args);

        public static IHostBuilder WebHostDefaults(string[] args) => CreateHostBuilder(args).ConfigureWebHostDefaults(WebBuilder => { WebBuilder.UseStartup<Startup>(); });

        public static IHostBuilder WindowsServiceDefaults(string[] args, int port) => CreateHostBuilder(args)
            .UseWindowsService()
            .UseContentRoot(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName))
            .ConfigureWebHostDefaults(WebBuilder =>
            {
                WebBuilder.UseKestrel().UseUrls($"http://0.0.0.0:{port}").UseStartup<Startup>();
            });
    }
}