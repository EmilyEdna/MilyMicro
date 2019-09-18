using System;
using System.Collections.Generic;
using System.Text;
using XExten.XCore;

namespace Mily.Extension.SocketClient.SocketCommon
{
    public class ParamCmd
    {
        public string Path { get; set; }
        public Dictionary<string, Object> HashData { get; set; }
        public string Controller => !Path.IsNullOrEmpty() ? Path.Split('_')[0] : "";
        public string Method => !Path.IsNullOrEmpty() ? Path.Split('_')[1] : "";
    }
}
