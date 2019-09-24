using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using XExten.XCore;

namespace Mily.OtherApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            if (!args.Contains("--Service"))
                CreateHostBuilder(args).UseStartup<Startup>().Build().Run();
            else
            {
                //open cmd with administrator and input SC CREATE [SERVERNAME] BINPATH="[PROGRAM_URL]\*.EXE" --Service
                args.ToEach<string>(item =>
                {
                    if (item.Trim().IsContainsIn("--Service:"))
                    {
                        int Port = Convert.ToInt32(item.Split(":")[1].Trim());
                        Startup.Port = Port + 1;
                        CreateHostBuilder(args).UseKestrel()
                        .UseContentRoot(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName))//加载当前目录
                        .UseUrls($"http://0.0.0.0:{Port}")//监听的IP和端口
                        .UseStartup<Startup>().Build().RunAsService();
                    }
                });
            }
        }
        public static IWebHostBuilder CreateHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args);
    }
}
