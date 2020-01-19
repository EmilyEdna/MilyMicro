using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Socket.SocketRoute
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
    public class SocketAuthorAttribute : Attribute
    {
        public bool UseAuthor { get; set; }
        public SocketAuthorAttribute(bool _Author)
        {
            UseAuthor = _Author;
        }
    }
}
