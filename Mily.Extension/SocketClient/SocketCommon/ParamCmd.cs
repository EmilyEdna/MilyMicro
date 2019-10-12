using System;
using System.Collections.Generic;
using XExten.XCore;

namespace Mily.Extension.SocketClient.SocketCommon
{
    public class ParamCmd
    {
        public string Path { get; set; }
        public Dictionary<string, Object> HashData { get; set; }
        public string Controller => !Path.IsNullOrEmpty() ? Path.Split('_')[0].Trim() : "";
        public string Method => !Path.IsNullOrEmpty() ? Path.Split('_')[1].Trim() : "";
        public string Service => !Path.IsNullOrEmpty() ? Path.Split('_')[2].Trim() : "";
    }
}