using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Primitives;
using Mily.Setting;
using System;
using XExten.CacheFactory;
using XExten.XPlus;

namespace Mily.Extension.FileReader
{
    public class PhysicalFileReader
    {
        #region 属性
        public IConfiguration Configuration { get; set; }
        public IWebHostEnvironment Environment { get; set; }
        #endregion

        #region 加载配置文件
        public PhysicalFileReader SetConfiguration()
        {
            Configuration.Bind(new MilyConfig());
            Caches.DbName = MilyConfig.ConnectionStrings.MongoDbName;
            Caches.RedisConnectionString = MilyConfig.ConnectionStrings.RedisConnectionString;
            Caches.MongoDBConnectionString = MilyConfig.ConnectionStrings.MongoDBConnectionString;
            MilyConfig.XmlSQL = XPlusEx.XReadXml();
            return this;
        }
        #endregion

        #region 配置文件执行器
        public PhysicalFileReader DynamicFileReaderJSON()
        {
            IFileProvider FileProvider = new PhysicalFileProvider(Environment.ContentRootPath);
            FileReaderOnChaged(() => FileProvider.Watch("*.json"), () => SetConfiguration());
            return this;
        }
        public PhysicalFileReader DynamicFileReaderXML()
        {
            IFileProvider FileProvider = new PhysicalFileProvider(Environment.ContentRootPath);
            FileReaderOnChaged(() => FileProvider.Watch("**//*.xml"), () =>SetConfiguration());
            return this;
        }
        public PhysicalFileReader DynamicFileReader(string FullPath, string Pattern, Action action)
        {
            IFileProvider FileProvider = new PhysicalFileProvider(FullPath);
            FileReaderOnChaged(() => FileProvider.Watch(Pattern), () => action.Invoke());
            return this;
        }
        #endregion

        #region 动态读取配置文件
        /// <summary>
        /// 动态读取配置文件
        /// </summary>
        /// <param name="ChangeTokenProducer"></param>
        /// <param name="ChangeTokenConsumer"></param>
        /// <returns></returns>
        protected static IDisposable FileReaderOnChaged(Func<IChangeToken> ChangeTokenProducer, Action ChangeTokenConsumer)
        {
            void CallBack(object obj)
            {
                ChangeTokenConsumer();
                ChangeTokenProducer().RegisterChangeCallback(CallBack, null);
            }
            return ChangeTokenProducer().RegisterChangeCallback(CallBack, null);
        }
        /// <summary>
        ///  动态读取配置文件
        /// </summary>
        /// <typeparam name="TState"></typeparam>
        /// <param name="ChangeTokenProducer"></param>
        /// <param name="ChangeTokenConsumer"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        protected static IDisposable FileReaderOnChaged<TState>(Func<IChangeToken> ChangeTokenProducer, Action<TState> ChangeTokenConsumer, TState state)
        {
            void CallBack(object obj)
            {
                ChangeTokenConsumer((TState)obj);
                ChangeTokenProducer().RegisterChangeCallback(CallBack, obj);
            }
            return ChangeTokenProducer().RegisterChangeCallback(CallBack, state);
        }
        #endregion
    }
}
