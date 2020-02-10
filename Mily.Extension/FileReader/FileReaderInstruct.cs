using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mily.Extension.FileReader
{
    public static class FileReaderInstruct
    {
        /// <summary>
        /// 执行配置文件管理
        /// </summary>
        /// <param name="Application"></param>
        /// <param name="Environment"></param>
        /// <param name="Builder"></param>
        public static void Instruct(this IApplicationBuilder Application, IWebHostEnvironment Environment, IConfiguration Builder)
        {
            new PhysicalFileReader()
            {
                Environment = Environment,
                Configuration = Builder
            }.SetConfiguration().DynamicFileReaderJSON().DynamicFileReaderXML();

        }
    }
}
