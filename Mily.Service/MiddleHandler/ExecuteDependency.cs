using Mily.Service.MiddleView;
using Mily.Service.MiddleView.ViewEnum;
using Mily.Service.MiddleView.ViewInterface;
using XExten.XCore;
using System.IO;
using System;
using System.Collections.Generic;
using System.Text;
using BeetleX.EventArgs;
using System.Linq;

namespace Mily.Service.MiddleHandler
{
    public class ExecuteDependency
    {
        /// <summary>
        /// 缓存Seesion
        /// </summary>
        private readonly static Dictionary<String, PacketDecodeCompletedEventArgs> PacketCache = new Dictionary<String, PacketDecodeCompletedEventArgs>();

        /// <summary>
        /// 缓存Session
        /// </summary>
        /// <param name="Provider"></param>
        /// <param name="Event"></param>
        public static void ExecutePacketCache(SocketMiddleData Provider, PacketDecodeCompletedEventArgs Event)
        {
            var Keys = Provider.MiddleResult.SocketJsonData.ToModel<Dictionary<string, List<string>>>().Keys.ToList();
            PacketCache.Add(string.Join(",", Keys), Event);
        }
        /// <summary>
        /// 处理内部消息
        /// </summary>
        /// <param name="Param"></param>
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
        /// <summary>
        /// 把服务注入的API持久化
        /// </summary>
        /// <param name="Param"></param>
        private static void ExecuteSocketApiJson(ISocketResult Provider)
        {
            var Directories = Path.Combine(AppContext.BaseDirectory, "SocketApi");
            if (!Directory.Exists(Directories))
                Directory.CreateDirectory(Directories);
            var JsonData = Provider.SocketJsonData.ToModel<Dictionary<string, List<string>>>();
            foreach (var Item in JsonData)
            {
                var FilePath = Path.Combine(Directories, $"{Item.Key}.json");
                if (File.Exists(FilePath))
                {
                    File.Delete(FilePath);
                    ExecuteSocketApiFile(FilePath, Provider.SocketJsonData);
                }
                else ExecuteSocketApiFile(FilePath, Provider.SocketJsonData);

            }
        }
        /// <summary>
        /// API写入文件
        /// </summary>
        /// <param name="FilePath"></param>
        /// <param name="Data"></param>
        private static void ExecuteSocketApiFile(string FilePath, string Data)
        {
            using FileStream Fs = new FileStream(FilePath, FileMode.Create, FileAccess.ReadWrite);
            using StreamWriter Sw = new StreamWriter(Fs);
            Sw.Write(Data);
            Sw.Flush();
            Sw.Close();
            Fs.Close();
        }
    }
}
