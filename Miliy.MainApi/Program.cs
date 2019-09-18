using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.WindowsServices;

namespace Miliy.MainApi
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
                Console.Write("请输入端口号：");
                int Port = Convert.ToInt32(Console.ReadLine());
                Startup.Port = Port + 1;
                CreateHostBuilder(args).UseKestrel()
                .UseContentRoot(Path.GetDirectoryName(Process.GetCurrentProcess().MainModule.FileName))//加载当前目录
                .UseUrls($"http://0.0.0.0:{Port}")//监听的IP和端口
                .UseStartup<Startup>().Build().RunAsService();
            }
        }
        public static IWebHostBuilder CreateHostBuilder(string[] args) => WebHost.CreateDefaultBuilder(args);
    }
}
