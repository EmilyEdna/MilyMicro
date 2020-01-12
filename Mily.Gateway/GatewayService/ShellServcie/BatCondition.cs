using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Mily.Gateway.GatewayService.ShellServcie
{
    public class BatCondition
    {
        /// <summary>
        /// 创建PowerShell脚本
        /// </summary>
        public static void CreateShellCondition()
        {
            if (!Directory.Exists(PathCondition.Directories))
                Directory.CreateDirectory(PathCondition.Directories);
            PathCondition.Types.ForEach(item =>
            {
                String Dir = Path.Combine(PathCondition.Directories, item + ".ps1");
                if (File.Exists(Dir))
                {
                    File.Delete(Dir);
                    CreateStream(Dir, item);
                }
                else
                    CreateStream(Dir, item);
            });
        }
        /// <summary>
        /// 创建文件并写入内容
        /// </summary>
        /// <param name="Path"></param>
        /// <param name="TypeItem"></param>
        private static void CreateStream(String Path, String TypeItem)
        {
            using FileStream Fs = new FileStream(Path, FileMode.Create, FileAccess.ReadWrite);
            using StreamWriter Sw = new StreamWriter(Fs);
            Sw.Write(PathCondition.DirectiveBase + TypeItem);
            Sw.Flush();
            Sw.Close();
            Fs.Close();
        }
    }
}
