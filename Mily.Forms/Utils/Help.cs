using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using XExten.XPlus;

namespace Mily.Forms.Utils
{
    public class Help
    {
        public static string Config_cof = AppDomain.CurrentDomain.BaseDirectory + "config.cof";
        public static string Tags_xml = AppDomain.CurrentDomain.BaseDirectory + "tags.xml";
        public static string Config_json = AppDomain.CurrentDomain.BaseDirectory + "config.json";

        public static bool FileCreater(string Path, Action action = null) {
            bool Flag = !File.Exists(Path);
            if (Flag)
            {
                File.Create(Path).Dispose();
                action?.Invoke();
            }
            return Flag;
        }
        public static string Read(string Path) 
        {
            using StreamReader reader = new StreamReader(Path);
            var res = reader.ReadToEnd();
            reader.Close();
            reader.Dispose();
            return res;
        }
        public static void Write(string Path, string data, Action action = null)
        {
            using StreamWriter writer = new StreamWriter(Path, false);
            XPlusEx.XTry(() =>
            {
                action?.Invoke();
                writer.Write(data);
            }, ex => throw ex, () =>
            {
                writer.Close();
                writer.Dispose();
            });
        }
    }
}
