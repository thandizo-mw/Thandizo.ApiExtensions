using AutoMapper;
using System.Collections.Generic;

namespace Thandizo.ApiExtensions.DataMapping
{
    public class AutoMapperHelper<TSource, TDestination>
    {
        public TDestination MapToObject(TSource source)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });
            return config.CreateMapper().Map<TSource, TDestination>(source);
        }

        public IEnumerable<TDestination> MapToList(IEnumerable<TSource> sourceList)
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<TSource, TDestination>();
            });
            var mapper = config.CreateMapper();
            return mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(sourceList);
        }
    }
}
