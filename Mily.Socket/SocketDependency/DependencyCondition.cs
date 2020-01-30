using Mily.Socket.SocketConfig;
using Mily.Socket.SocketInterface;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Mily.Socket.SocketInterface.DefaultImpl;
using XExten.XCore;
using Newtonsoft.Json.Linq;
using Mily.Socket.SocketEnum;

namespace Mily.Socket.SocketDependency
{
    public class DependencyCondition
    {
        public static DependencyCondition Instance => new DependencyCondition();

        public SocketMiddleData ExecuteMapper(object Param)
        {
            JObject Obj = (Param as string).ToModel<JObject>();
            SendTypeEnum SendType = Enum.Parse<SendTypeEnum>(Obj["SendType"].ToString());
            ISocketResult Result = Obj["MiddleResult"].ToJson().ToModel<SocketResultDefault>();
            ISocketSession Session = Obj["Session"].ToJson().ToModel<SocketSessionDefault>();
            return SocketMiddleData.Middle(SendType, Result, Session);
        }
    }
}
