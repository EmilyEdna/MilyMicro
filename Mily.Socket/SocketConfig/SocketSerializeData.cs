using System;
using System.Collections.Generic;
using System.Text;
using XExten.XCore;

namespace Mily.Socket.SocketConfig
{
    public class SocketSerializeData
    {
        public String Route { get; set; }
        public Dictionary<String, Object> Providor { get; set; }
        public SocketSerializeData AppendSerialized(string Key, object Value)
        {
            Providor ??= new Dictionary<String, Object>();
            Providor.Add(Key, Value);
            return this;
        }
        public SocketSerializeData AppendSerialized<T>(T Param) {
            Providor ??= new Dictionary<String, Object>();
            var data = Param.ToDic();
            if (data.Count == 0) return this;
            Param.ToDic().ToEachs(Item =>
            {
                Providor.Add(Item.Key, Item.Value);
            });
            return this;
        }
        public SocketSerializeData AppendSerialized(Dictionary<String, Object> Param)
        {
            Providor ??= new Dictionary<String, Object>();
            if (Param.Count == 0) return this;
            Param.ToEachs(Item =>
            {
                Providor.Add(Item.Key, Item.Value);
            });
            return this;
        }
        public SocketSerializeData AppendRoute(string Router)
        {
            Route = Router.ToLower();
            return this;
        }
    }
}
