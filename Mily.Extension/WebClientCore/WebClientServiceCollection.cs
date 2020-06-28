using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using WebApiClientCore;
using XExten.XPlus;

namespace Mily.Extension.WebClientCore
{
    public static class WebClientServiceCollection
    {
        /// <summary>
        /// 注册ClientApi
        /// </summary>
        /// <param name="Services"></param>
        /// <returns></returns>
        public static IServiceCollection RegiestClientApi(this IServiceCollection Services)
        {
            var Assemlies = XPlusEx.XAssembly();
            var Types = Assemlies.SelectMany(t => t.ExportedTypes.Where(x => x.GetInterfaces().Contains(typeof(IHttpApi))));
            foreach (var Item in Types)
            {
                Services.AddHttpApi(Item, opt =>
                {
                    opt.JsonSerializeOptions.PropertyNamingPolicy = null;
                    opt.JsonSerializeOptions.DictionaryKeyPolicy = null;
                    opt.UseParameterPropertyValidate = false;
                    opt.UseReturnValuePropertyValidate = false;
                });
            }
            return Services;
        }
    }
}
