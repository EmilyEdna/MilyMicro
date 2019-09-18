using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Service.ViewSetting.SocketSetting
{
    public struct Commanding
    {
        public Commanding(NetType Types, string Contents)
        {
            Type = Types;
            Content = Contents;
        }
        public NetType Type { get; set; }
        public string Content { get; set; }
    }
}
