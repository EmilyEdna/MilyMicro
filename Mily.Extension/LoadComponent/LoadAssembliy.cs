using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;

namespace Mily.Extension.LoadComponent
{
    public class LoadAssembliy : AssemblyLoadContext
    {
        public LoadAssembliy() : base(true)
        {
        }

    }
}
