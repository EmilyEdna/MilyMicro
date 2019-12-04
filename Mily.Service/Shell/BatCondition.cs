using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mily.Service.Shell
{
    public class BatCondition
    {
        public static void CreateShellCondition()
        {
            if (!Directory.Exists(PathCondition.Directories))
                Directory.CreateDirectory(PathCondition.Directories);
                PathCondition.Types.ForEach(item =>
                {
                    File.AppendAllText(Path.Combine(PathCondition.Directories, item + ".ps1"), PathCondition.DirectiveBase + item);
                });
        }
    }
}
