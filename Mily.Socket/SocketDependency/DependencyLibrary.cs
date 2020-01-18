using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Mily.Socket.SocketDependency
{
    public class DependencyLibrary
    {
        /// <summary>
        /// 所有程序集
        /// </summary>
        public static IList<Assembly> Assembly { get; set; } = GetAssembly();

        private static IList<Assembly> GetAssembly()
        {
            IList<Assembly> ass = new List<Assembly>();
            var lib = DependencyContext.Default;
            var libs = lib.CompileLibraries.Where(t => !t.Serviceable).Where(t => t.Type == "project").ToList();
            foreach (var item in libs)
            {
                Assembly assembly = AssemblyLoadContext.Default.LoadFromAssemblyName(new AssemblyName(item.Name));
                ass.Add(assembly);
            }
            return ass;
        }
    }
}
