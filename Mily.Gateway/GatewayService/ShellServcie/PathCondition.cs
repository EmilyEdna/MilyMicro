using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mily.Gateway.GatewayService.ShellServcie
{
    public class PathCondition
    {
        public static List<string> Types = new List<string>
        {
            "install",
            "uninstall",
            "start",
            "stop",
            "pause",
            "continue"
        };
        public static string Directories => Path.Combine(AppContext.BaseDirectory, @"ShellScript\");
        public static string DirectiveBase => $"dotnet.exe {Path.Combine(AppContext.BaseDirectory, typeof(PathCondition).Module.Name)} action:";
    }
}
