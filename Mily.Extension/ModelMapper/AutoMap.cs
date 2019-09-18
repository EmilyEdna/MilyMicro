using AutoMapper;
using AutoMapper.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mily.Extension.ModelMapper
{
    public static class AutoMap
    {
        public static  T AutoMapper<T>(this Object obj)
        {
            if (obj == null) return default(T);
            IMapper mapper = new MapperConfiguration(t => t.CreateMap(obj.GetType(), typeof(T))).CreateMapper();
            return mapper.Map<T>(obj);
        }
        public static T AutoMapper<T>(this Object obj, String IgnoreNames)
        {
            if (obj == null) return default(T);
            MapperConfigurationExpression expression = new MapperConfigurationExpression();
            IMappingExpression mapping = expression.CreateMap(obj.GetType(), typeof(T));
            if (IgnoreNames.Contains("|"))
                IgnoreNames.Split('|').ToList().ForEach(t =>{mapping.ForMember(t, x => x.Ignore());});
            else
                mapping.ForMember(IgnoreNames, x => x.Ignore());
            IMapper mapper = new MapperConfiguration(expression).CreateMapper();
            return mapper.Map<T>(obj);
        }
    }
}
