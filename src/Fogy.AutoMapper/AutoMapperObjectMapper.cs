using AutoMapper;
using IObjectMapper = Fogy.Core.ObjectMapper.IObjectMapper;

namespace Fogy.AutoMapper
{
    public class AutoMapperObjectMapper : IObjectMapper
    {
        private readonly IMapper _mapper;

        public AutoMapperObjectMapper(IMapper mapper)
        {
            _mapper = mapper;
        }

        public TDestincation Map<TDestincation>(object source)
        {
            return _mapper.Map<TDestincation>(source);
        }

        public TDestincation Map<TSource, TDestincation>(TSource source, TDestincation destincation)
        {
            return _mapper.Map<TSource, TDestincation>(source);
        }
    }
}
