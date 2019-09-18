using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Extension.LoggerFactory
{
    /// <summary>
    /// NLog日志工厂
    /// </summary>
    public static class LogFactoryExtension
    {
        static Logger logger = LogManager.GetCurrentClassLogger();
        /// <summary>
        /// 信息日志
        /// </summary>
        /// <param name="Msg"></param>
        public static void WriteInfo(string Path, string MethodName, string Parameter, string Msg, string WebPath)
        {
            logger.Info($"信息位置：{Path}，调用方法名：{MethodName}，相关参数：{Parameter}，相关信息：{Msg}，请求路径：{WebPath}");
        }
        /// <summary>
        /// 栈日志
        /// </summary>
        /// <param name="Msg"></param>
        public static void WriteTrace(string Path, string MethodName, string Parameter, string Msg, string WebPath)
        {
            logger.Trace($"异常位置：{Path}，调用方法名：{MethodName}，相关参数：{Parameter}，异常信息：{Msg}，请求路径：{WebPath}");
        }
        /// <summary>
        /// 调试日志
        /// </summary>
        /// <param name="Msg"></param>
        public static void WriteDebug(string Path, string MethodName, string Parameter, string Msg, string WebPath)
        {
            logger.Debug($"调试位置：{Path}，调用方法名：{MethodName}，相关参数：{Parameter}，调试信息：{Msg}，请求路径：{WebPath}");
        }
        /// <summary>
        /// 错误日志
        /// </summary>
        /// <param name="Msg"></param>
        public static void WriteError(string Path, string MethodName, string Parameter, string Msg,string WebPath)
        {
            logger.Error($"错误位置：{Path}，调用方法名：{MethodName}，相关参数：{Parameter}，错误信息：{Msg}，请求路径：{WebPath}");
        }
        /// <summary>
        /// 警告日志
        /// </summary>
        /// <param name="Msg"></param>
        public static void WriteWarn(string Path, string MethodName, string Parameter, string Msg, string WebPath)
        {
            logger.Warn($"警告位置：{Path}，调用方法名：{MethodName}，相关参数：{Parameter}，警告信息：{Msg}，请求路径：{WebPath}");
        }
        /// <summary>
        /// 异常日志
        /// </summary>
        /// <param name="Msg"></param>
        public static void WriteFatal(string Path, string MethodName, string Parameter, string Msg, string WebPath)
        {
            logger.Fatal($"失败位置：{Path}，调用方法名：{MethodName}，相关参数：{Parameter}，失败信息：{Msg}，请求路径：{WebPath}");
        }
        /// <summary>
        /// SQL错误记录
        /// </summary>
        /// <param name="SQLException"></param>
        public static void WriteSqlError(string SQLException)
        {
            logger.Error(SQLException);
        }
        /// <summary>
        /// SQL执行记录
        /// </summary>
        /// <param name="SQLException"></param>
        public static void WriteSqlWarn(string SQLInfo)
        {
            logger.Warn(SQLInfo);
        }
    }
}
