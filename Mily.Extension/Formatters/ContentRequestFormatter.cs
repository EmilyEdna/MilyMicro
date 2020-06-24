using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using XExten.Common;
using XExten.XCore;

namespace Mily.Extension.Formatters
{
    public class ContentRequestFormatter : InputFormatter
    {
        public ContentRequestFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/json"));
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/x-www-form-urlencoded"));
        }

        public override bool CanRead(InputFormatterContext context)
        {
            if (context == null) return false;
            string ContentType = context.HttpContext.Request.ContentType;
            if (ContentType.IsNullOrEmpty() || ContentType.Contains("application/json")||ContentType.Contains("application/x-www-form-urlencoded"))
                return true;
            return false;
        }
        public override async Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var Request = context.HttpContext.Request;
            var ContentType = Request.ContentType;

            if (ContentType.IsNullOrEmpty() || ContentType.Contains("application/json"))
            {
                using var reader = new StreamReader(Request.Body);
                var content = await reader.ReadToEndAsync();
                if (content.Contains("DictionaryStringProvider"))
                    return await InputFormatterResult.SuccessAsync(content.ToModel<ResultProvider>());
                else if (content.Contains("KeyWord"))
                    return await InputFormatterResult.SuccessAsync(content.ToModel<PageQuery>());
                else
                    return await InputFormatterResult.SuccessAsync(content);
            }
            if (ContentType.IsNullOrEmpty() || ContentType.Contains("application/x-www-form-urlencoded"))
            {
                if (context.ModelType == typeof(ResultProvider))
                {
                    ResultProvider Provider = new ResultProvider() { DictionaryStringProvider = new Dictionary<string, object>() };
                    Request.Form.ToEachs(item =>
                    {
                        Provider.DictionaryStringProvider.Add(item.Key, item.Value);
                    });
                    return await InputFormatterResult.SuccessAsync(Provider);
                }
                if (context.ModelType == typeof(PageQuery)) 
                {
                    PageQuery Query = new PageQuery()
                    {
                        PageIndex = Convert.ToInt32(Request.Form["PageIndex"]),
                        PageSize = Convert.ToInt32(Request.Form["PageSize"]),
                        KeyWord = new Dictionary<string, object>()
                    };
                    Request.Form.ToEachs(item =>
                    {
                        if (item.Key.Contains("KeyWord"))
                        {
                            string Key = item.Key.Split("[")[1].Split("]")[0];
                            Query.KeyWord.Add(Key, item.Value.ToString());
                        }
                    });
                    return await InputFormatterResult.SuccessAsync(Query);
                }
            }
            return await InputFormatterResult.FailureAsync();
        }
    }
}
