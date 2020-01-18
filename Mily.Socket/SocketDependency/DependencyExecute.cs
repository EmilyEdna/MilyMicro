using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using Mily.Socket.SocketRoute;
using XExten.XCore;
using XExten.XPlus;
using System.IO;

namespace Mily.Socket.SocketDependency
{
    public class DependencyExecute
    {
        private static IEnumerable<Type> Ctrl => DependencyLibrary.Assembly.SelectMany(item => item.ExportedTypes.Where(t => t.GetInterfaces().Contains(typeof(ISocketDependency))));
        private static Dictionary<string, List<string>> SocketJson = new Dictionary<string, List<string>>();
        /// <summary>
        /// 查询需要生成APIJson的接口
        /// </summary>
        public static void FindLibrary()
        {
            List<Type> SourceType = Ctrl.Where(item => item.GetCustomAttribute(typeof(SocketRouteAttribute)) != null).ToList();
            foreach (var Items in SourceType)
            {
                StringBuilder sb = new StringBuilder();
                List<string> Route = new List<string>();
                SocketRouteAttribute SocketRoute = (Items.GetCustomAttribute(typeof(SocketRouteAttribute)) as SocketRouteAttribute);
                string ControllerName = string.Empty;
                if (SocketRoute.ControllerName.IsNullOrEmpty())
                    ControllerName = Items.Name;
                else
                    ControllerName = SocketRoute.ControllerName;
                Items.GetMethods().Where(x => x.GetCustomAttribute(typeof(SocketMethodAttribute)) != null).ToList().ForEach(Item =>
                {
                    SocketMethodAttribute SocketMethod = (Item.GetCustomAttribute(typeof(SocketMethodAttribute)) as SocketMethodAttribute);
                    string SocketUrl = string.Empty;
                    if (SocketMethod.MethodName.IsNullOrEmpty())
                        SocketUrl = $"{SocketRoute.InternalServer}/{ControllerName}/{Item.Name}";
                    else
                        SocketUrl = $"{SocketRoute.InternalServer}/{ControllerName}/{SocketMethod.MethodName}";
                    if (!SocketMethod.MethodVersion.IsNullOrEmpty())
                        SocketUrl = SocketUrl + "/" + SocketMethod.MethodVersion;
                    Route.Add(sb.Append(SocketUrl).ToString());
                });
                XPlusEx.XTry(() => SocketJson.Add(ControllerName, Route), Ex => throw Ex);
            }
            CreateSocketApiJsonScript();
        }
        /// <summary>
        /// 创建API文件
        /// </summary>
        private static void CreateSocketApiJsonScript()
        {
            if (SocketJson.Count > 0)
            {
                string Paths = Path.Combine(AppContext.BaseDirectory, @"SocketJsonApi\");
                if (!Directory.Exists(Paths))
                    Directory.CreateDirectory(Paths);
                string FilePath = Path.Combine(Paths, "SocketApi.json");
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                    CreateSocketFileContent(FilePath);
                }
                else CreateSocketFileContent(FilePath);
            }
        }
        /// <summary>
        /// 添加文件内容
        /// </summary>
        /// <param name="FilePath"></param>
        private static void CreateSocketFileContent(string FilePath)
        {
            using FileStream Fs = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite);
            using StreamWriter Sw = new StreamWriter(Fs);
            Sw.Write(SocketJson.ToJson());
            Sw.Flush();
            Sw.Close();
            Fs.Close();
        }
    }
}
