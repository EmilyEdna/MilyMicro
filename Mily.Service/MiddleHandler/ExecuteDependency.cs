using Mily.Service.MiddleView;
using Mily.Service.MiddleView.ViewEnum;
using Mily.Service.MiddleView.ViewInterface;
using XExten.XCore;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Service.MiddleHandler
{
    public class ExecuteDependency
    {
        public static void ExecuteInternalInfo(SocketMiddleData Param)
        {
            switch (Param.SendType)
            {
                case SendTypeEnum.Init:
                    ExecuteSocketApiJson(Param.MiddleResult);
                    break;
                case SendTypeEnum.InternalInfo:
                    break;
                case SendTypeEnum.RequestInfo:
                    break;
                case SendTypeEnum.CallBack:
                    break;
                default:
                    break;
            }
        }
        private static void ExecuteSocketApiJson(ISocketResult Param)
        {
            var Directories = Path.Combine(AppContext.BaseDirectory, "SocketApi");
            if (!Directory.Exists(Directories))
                Directory.CreateDirectory(Directories);
            var JsonData = Param.SocketJsonData.ToModel<Dictionary<string, List<string>>>();
            foreach (var Item in JsonData)
            {
                var FilePath = Path.Combine(Directories, $"{Item.Key}.json");
                ExecuteSocketApiFile(FilePath,Param.SocketJsonData);
                
            }
        }
        private static void ExecuteSocketApiFile(string FilePath,string Data)
        { 
        
        }
    }
}
