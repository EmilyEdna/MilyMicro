using Mily.Forms.TreeViewTest.Directory.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mily.Forms.TreeViewTest.Directory
{
    public class DirectoryStucture
    {
        /// <summary>
        /// 文件目录名称
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetFileOrFolderName(string path)
        {
            if (string.IsNullOrEmpty(path))
                return string.Empty;
            var normal = path.Replace("/", "\\");
            var last = normal.LastIndexOf("\\");
            return path.Substring(last + 1);
        }

        /// <summary>
        /// 获取目录内容
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static List<DirectoryItem> GetDirectoryContents(string fullPath)
        {
            var Item = new List<DirectoryItem>();
            //获取二级目录
            var dirs = System.IO.Directory.GetDirectories(fullPath);
            if (dirs.Length > 0)
                Item.AddRange(dirs.Select(t => new DirectoryItem { FullPath = t, Type = DirectoryItemType.Folder }));
            //获取文件
            var fs = System.IO.Directory.GetFiles(fullPath);
            if (fs.Length > 0)
                Item.AddRange(fs.Select(t => new DirectoryItem { FullPath = t, Type = DirectoryItemType.File }));
            return Item;
        }

        /// <summary>
        /// 获取物理磁盘
        /// </summary>
        /// <returns></returns>
        public static List<DirectoryItem> GetLogicalDrives()
        {
            return System.IO.Directory.GetLogicalDrives().Select(dirver => new DirectoryItem { FullPath = dirver, Type = DirectoryItemType.Diver }).ToList();
        }
    }
}
